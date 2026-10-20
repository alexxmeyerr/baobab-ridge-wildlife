using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using BaobabRidge.Services;

namespace BaobabRidge
{
    // Member 3's search/save/delete workflow, connected to Member 1's shared form.
    public partial class MainForm
    {
        private UpdateDeleteService member3Service;
        private bool editMode;
        private string editingAnimalId;
        private string editingAnimalName;

        // Connects update and delete to the same file as Add and View.
        private void InitializeMember3Feature()
        {
            member3Service = new UpdateDeleteService(fileManager.AnimalsPath);
            SetEditMode(false);
        }

        // Locks the ID during editing and restores Add mode when editing ends.
        private void SetEditMode(bool editing)
        {
            editMode = editing;
            txtAnimalId.ReadOnly = editing;
            btnAdd.Enabled = !editing;
            btnSearch.Enabled = !editing;
            btnSaveChanges.Enabled = editing;
            btnDelete.Enabled = true;
            AcceptButton = editing ? btnSaveChanges : btnAdd;
            if (!editing)
            {
                editingAnimalId = null;
                editingAnimalName = null;
            }
        }

        // Searches using the ID typed in the shared input field.
        private void btnSearch_Click(object sender, EventArgs e)
        {
            FindAndLoadAnimal(txtAnimalId.Text);
        }

        // Reads the current record before loading shared fields; failed searches retain input.
        private void FindAndLoadAnimal(string animalId)
        {
            errorProvider.Clear();
            animalId = animalId.Trim().ToUpperInvariant();
            if (animalId.Length == 0)
            {
                ShowValidationError(new ValidationError(AnimalField.AnimalId,
                    "Enter an Animal ID to search."));
                return;
            }
            try
            {
                Animal animal = member3Service.FindAnimal(animalId);
                if (animal == null)
                {
                    ShowFileError("Animal not found. Check the Animal ID.");
                    return;
                }
                txtAnimalId.Text = animal.AnimalId;
                txtName.Text = animal.Name;
                txtSpecies.Text = animal.Species;
                txtAge.Text = animal.Age.ToString(CultureInfo.InvariantCulture);
                txtRecoveryScore.Text = animal.RecoveryScore.ToString(CultureInfo.InvariantCulture);
                lblStatusValue.Text = animal.Status;
                lblHousingValue.Text = animal.HousingUnit;
                editingAnimalId = animal.AnimalId;
                editingAnimalName = animal.Name;
                SetEditMode(true);
                lblFeedback.ForeColor = SystemColors.ControlText;
                lblFeedback.Text = "Animal loaded. Edit the details, then Save Changes. Clear cancels editing.";
                txtName.Focus();
            }
            catch (InvalidDataException ex) { ShowFileError(ex.Message); }
            catch (UnauthorizedAccessException) { ShowFileError("Cannot read animals.txt. Check file permissions."); }
            catch (IOException) { ShowFileError("Cannot read animals.txt. Close any program locking the file and try again."); }
        }

        // Validates and replaces only the selected record, then refreshes the grid.
        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            if (!editMode) return;
            errorProvider.Clear();
            Animal animal;
            ValidationError error;
            // The original ID is fixed; validation also recalculates status and housing.
            if (!AnimalValidator.TryCreate(editingAnimalId, txtName.Text, txtSpecies.Text,
                txtAge.Text, txtRecoveryScore.Text, out animal, out error))
            {
                ShowValidationError(error);
                return;
            }
            try
            {
                if (!member3Service.UpdateAnimal(animal))
                {
                    ShowFileError("Animal no longer exists. Clear the form and search again.");
                    return;
                }
            }
            catch (InvalidDataException ex) { ShowFileError(ex.Message); return; }
            catch (UnauthorizedAccessException) { ShowFileError("Cannot update animals.txt. Check read-only settings and folder permissions."); return; }
            catch (IOException) { ShowFileError("Cannot update animals.txt. Close any program locking the file and try again."); return; }
            SetEditMode(false);
            ClearInputs();
            lblStatusValue.Text = animal.Status;
            lblHousingValue.Text = animal.HousingUnit;
            lblFeedback.ForeColor = Color.FromArgb(28, 94, 56);
            lblFeedback.Text = "Successfully updated " + animal.AnimalId + " (" + animal.Name + ").";
            OnAnimalsChanged();
        }

        // Requires a selection and confirmation before removing one record.
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!editMode)
            {
                ShowFileError("Select an animal row or search for an Animal ID before deleting.");
                return;
            }
            if (!ConfirmDelete(editingAnimalId)) return;
            string animalId = editingAnimalId;
            try
            {
                if (!member3Service.DeleteAnimal(animalId))
                {
                    ShowFileError("Animal no longer exists. Clear the form and search again.");
                    return;
                }
            }
            catch (InvalidDataException ex) { ShowFileError(ex.Message); return; }
            catch (UnauthorizedAccessException) { ShowFileError("Cannot delete from animals.txt. Check read-only settings and folder permissions."); return; }
            catch (IOException) { ShowFileError("Cannot delete from animals.txt. Close any program locking the file and try again."); return; }
            btnClear_Click(sender, e);
            lblFeedback.ForeColor = Color.FromArgb(28, 94, 56);
            lblFeedback.Text = "Successfully deleted " + animalId + ".";
            OnAnimalsChanged();
        }

        // Virtual so tests can exercise both answers without deleting real records.
        protected virtual bool ConfirmDelete(string animalId)
        {
            return MessageBox.Show(this,
                "Are you sure you want to delete " + animalId + " (" + editingAnimalName + ")?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }
    }
}