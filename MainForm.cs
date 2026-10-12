using BaobabRidge.Services;

namespace BaobabRidge
{

    public partial class MainForm : Form
    {
        private readonly FileManager fileManager;

        // Member 2 can subscribe to this event to reload the grid after an animal is saved.
        public event EventHandler AnimalAdded;

        // Opens the form with the production file beside the executable.
        public MainForm() : this(new FileManager())
        {
        }

        // Allows tests to use a temporary file without modifying the real animal records.
        public MainForm(FileManager fileManager)
        {
            this.fileManager = fileManager;
            InitializeComponent();
        }

        // Validates, checks uniqueness, saves the animal, and reports its classification.
        private void btnAdd_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            lblFeedback.Text = "";
            Animal animal;
            ValidationError error;
            if (!AnimalValidator.TryCreate(txtAnimalId.Text, txtName.Text, txtSpecies.Text,
                txtAge.Text, txtRecoveryScore.Text, out animal, out error))
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
            // Notify the future View feature only after the append has completed successfully.
            OnAnimalAdded();
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
            errorProvider.Clear();
            lblFeedback.Text = "Enter the animal's details, then select Add Animal.";
            lblFeedback.ForeColor = SystemColors.ControlText;
            lblStatusValue.Text = "-";
            lblHousingValue.Text = "-";
            ClearInputs();
        }

        // Announces a completed add so the shared form can refresh its animal grid.
        protected virtual void OnAnimalAdded()
        {
            if (AnimalAdded != null)
            {
                AnimalAdded(this, EventArgs.Empty);
            }
        }
    }
}
