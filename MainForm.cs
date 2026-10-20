using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BaobabRidge.Services;

namespace BaobabRidge
{

    public partial class MainForm : Form
    {
        private FileManager fileManager = new FileManager();

        // Kept for existing Add subscribers; the grid uses AnimalsChanged for all mutations.
        public event EventHandler AnimalAdded;
        public event EventHandler AnimalsChanged;

        //it ensures that the form loads animals when it opens
        public MainForm() : this(new FileManager())
        {
        }

        // Allows tests to use a temporary file without modifying the real animal records.
        public MainForm(FileManager fileManager)
        {
            this.fileManager = fileManager;
            InitializeComponent();

            InitializeMember3Feature();
            AnimalsChanged += ReloadAnimals;
            LoadAnimals();
        }

        // Validates, checks uniqueness, saves the animal, and reports its classification.
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (editMode) return;
            errorProvider.Clear();
            lblFeedback.Text = "";

            // Capture the values from the text boxes, as in the class form examples.
            string animalId = txtAnimalId.Text;
            string name = txtName.Text;
            string species = txtSpecies.Text;
            string age = txtAge.Text;
            string recoveryScore = txtRecoveryScore.Text;

            // Validate before converting numbers or writing anything to the file.
            Animal animal;
            ValidationError error;
            if (!AnimalValidator.TryCreate(animalId, name, species,
                age, recoveryScore, out animal, out error))
            {
                ShowValidationError(error);
                return;
            }

            try
            {
                if (!fileManager.TryAppend(animal))
                {
                    ShowValidationError(new ValidationError(AnimalField.AnimalId,
                        "Animal ID already exists. Enter a different Animal ID."));
                    return;
                }
            }
            catch (UnauthorizedAccessException)
            {
                ShowFileError("Cannot save animals.txt. Check that the file is not read-only and that you have permission to write to its folder.");
                return;
            }
            catch (DirectoryNotFoundException)
            {
                ShowFileError("Cannot find the folder for animals.txt. Restore the application folder and try again.");
                return;
            }
            catch (IOException)
            {
                ShowFileError("Cannot save animals.txt. Close any program locking the file, check available disk space, and try again.");
                return;
            }

            lblFeedback.ForeColor = Color.FromArgb(28, 94, 56);
            lblFeedback.Text = "Successfully added " + animal.AnimalId + " (" + animal.Name + ").";
            lblStatusValue.Text = animal.Status;
            lblHousingValue.Text = animal.HousingUnit;
            ClearInputs();
            // Publish events only after the append has completed successfully.
            OnAnimalAdded();
            OnAnimalsChanged();
        }

        // Connects a field-specific message to its input and returns keyboard focus there.
        private void ShowValidationError(ValidationError error)
        {
            TextBox field;
            if (error.Field == AnimalField.AnimalId)
            {
                field = txtAnimalId;
            }
            else if (error.Field == AnimalField.Name)
            {
                field = txtName;
            }
            else if (error.Field == AnimalField.Species)
            {
                field = txtSpecies;
            }
            else if (error.Field == AnimalField.Age)
            {
                field = txtAge;
            }
            else
            {
                field = txtRecoveryScore;
            }
            lblFeedback.ForeColor = Color.Firebrick;
            lblFeedback.Text = error.Message;
            errorProvider.SetError(field, error.Message);
            field.Focus();
            field.SelectAll();
        }

        // Shows a friendly file error while retaining the typed values for a retry.
        private void ShowFileError(string message)
        {
            lblFeedback.ForeColor = Color.Firebrick;
            lblFeedback.Text = message;
        }

        // Resets the input fields after a successful add without removing the confirmation.
        private void ClearInputs()
        {
            dgvAnimals.ClearSelection();
            dgvAnimals.CurrentCell = null;
            txtAnimalId.Clear();
            txtName.Clear();
            txtSpecies.Clear();
            txtAge.Clear();
            txtRecoveryScore.Clear();
            txtAnimalId.Focus();
        }

        // Clears all input, validation errors, and previous confirmation details.
        private void btnClear_Click(object sender, EventArgs e)
        {
            SetEditMode(false);
            errorProvider.Clear();
            lblFeedback.Text = "Enter the animal's details, then select Add Animal.";
            lblFeedback.ForeColor = SystemColors.ControlText;
            lblStatusValue.Text = "-";
            lblHousingValue.Text = "-";
            ClearInputs();
        }

        // Reads current records, displays the report, and overwrites summary.txt.
        private void btnSummaryReport_Click(object sender, EventArgs e)
        {
            try
            {
                int skipped;
                List<Animal> animals = fileManager.TryLoadAll(out skipped);
                SummaryReport report = SummaryReportService.Create(animals);
                txtSummaryReport.Text = report.ToText();
                SummaryReportService.Save(report, fileManager.SummaryPath);
                lblFeedback.ForeColor = Color.FromArgb(28, 94, 56);
                lblFeedback.Text = "Summary saved to: " + fileManager.SummaryPath;
                lblLoadInfo.Text = skipped + " malformed record(s) excluded from this report.";
            }
            catch (UnauthorizedAccessException)
            {
                ShowFileError("Cannot read animals.txt or save summary.txt. Check file permissions.");
            }
            catch (DirectoryNotFoundException)
            {
                ShowFileError("Cannot find the data folder. Restore the application folder and try again.");
            }
            catch (IOException)
            {
                ShowFileError("Cannot generate the report. Close any program locking the files and try again.");
            }
        }

        // Announces a completed add so the shared form can refresh its animal grid.
        protected virtual void OnAnimalAdded()
        {
            if (AnimalAdded != null)
            {
                AnimalAdded(this, EventArgs.Empty);
            }
        }

        // Reloads once after a successful add, update, or delete.
        private void ReloadAnimals(object sender, EventArgs e)
        {
            LoadAnimals();
        }

        // Creates a missing file and lists valid records in their original file order.
        private void LoadAnimals()
        {
            CreateAnimalColumns();
            ConfigureAnimalsGrid();
            dgvAnimals.Rows.Clear();
            try
            {
                fileManager.EnsureFileExists();
                int skipped;
                List<Animal> animals = fileManager.TryLoadAll(out skipped);
                foreach (Animal animal in animals)
                {
                    dgvAnimals.Rows.Add(animal.AnimalId, animal.Name, animal.Species,
                        animal.Age, animal.RecoveryScore, animal.Status, animal.HousingUnit);
                }
                dgvAnimals.ClearSelection();
                dgvAnimals.CurrentCell = null;
                lblLoadInfo.Text = skipped == 0 ? "Select an animal row to edit or delete it."
                    : skipped + " malformed record(s) skipped when loading animals.txt.";
            }
            catch (UnauthorizedAccessException)
            {
                ShowFileError("Cannot load animals.txt. Check file and folder permissions.");
            }
            catch (IOException)
            {
                ShowFileError("Cannot load animals.txt. Check the folder and close any program locking the file.");
            }
        }

        // Stops users changing cells, adding rows, deleting rows, or sorting file order.
        private void ConfigureAnimalsGrid()
        {
            dgvAnimals.ReadOnly = true;
            dgvAnimals.AllowUserToAddRows = false;
            dgvAnimals.AllowUserToDeleteRows = false;
            dgvAnimals.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAnimals.MultiSelect = false;
            dgvAnimals.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAnimals.RowHeadersVisible = false;
            foreach (DataGridViewColumn column in dgvAnimals.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        // Adds the seven file fields as grid columns once.
        private void CreateAnimalColumns()
        {
            if (dgvAnimals.Columns.Count > 0) return;
            dgvAnimals.Columns.Add("AnimalId", "Animal ID");
            dgvAnimals.Columns.Add("Name", "Name");
            dgvAnimals.Columns.Add("Species", "Species");
            dgvAnimals.Columns.Add("Age", "Age");
            dgvAnimals.Columns.Add("RecoveryScore", "Recovery Score");
            dgvAnimals.Columns.Add("Status", "Status");
            dgvAnimals.Columns.Add("HousingUnit", "Housing Unit");
        }

        // A row click uses the same search and edit workflow as the Search button.
        private void dgvAnimals_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string animalId = Convert.ToString(dgvAnimals.Rows[e.RowIndex].Cells[0].Value);
            FindAndLoadAnimal(animalId);
        }

        // Notifies subscribers only after a file mutation has succeeded.
        protected virtual void OnAnimalsChanged()
        {
            if (AnimalsChanged != null) AnimalsChanged(this, EventArgs.Empty);
        }
    }
}
