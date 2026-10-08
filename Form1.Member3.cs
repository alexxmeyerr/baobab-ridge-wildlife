using System;
using System.Drawing;
using System.Windows.Forms;
using BaobabRidge.Services;

namespace BaobabRidge;

public partial class Form1
{
    private UpdateDeleteService member3Service = null!;

    private TextBox member3AnimalIdTextBox = null!;
    private TextBox member3NameTextBox = null!;
    private TextBox member3SpeciesTextBox = null!;
    private TextBox member3AgeTextBox = null!;
    private TextBox member3RecoveryScoreTextBox = null!;
    private TextBox member3StatusTextBox = null!;
    private TextBox member3HousingUnitTextBox = null!;

    private Button member3SearchButton = null!;
    private Button member3SaveButton = null!;
    private Button member3DeleteButton = null!;
    private Button member3ClearButton = null!;

    private Label member3MessageLabel = null!;

    private bool member3EditMode = false;

    // Called from Form1.cs
    private void InitializeMember3Feature()
    {
        member3Service = new UpdateDeleteService();

        CreateMember3Controls();

        member3SaveButton.Enabled = false;
        member3DeleteButton.Enabled = false;

        member3SetMessage(
            "Enter an Animal ID and click Search.",
            false);
    }

    private void CreateMember3Controls()
    {
        // Main group box
        GroupBox groupBox = new GroupBox();

        groupBox.Text = "Update / Delete Animal";
        groupBox.Location = new Point(20, 20);
        groupBox.Size = new Size(740, 380);

        // Animal ID
        Label idLabel = new Label();
        idLabel.Text = "Animal ID:";
        idLabel.Location = new Point(25, 35);
        idLabel.AutoSize = true;

        member3AnimalIdTextBox = new TextBox();
        member3AnimalIdTextBox.Location = new Point(150, 30);
        member3AnimalIdTextBox.Size = new Size(180, 27);

        // Search button
        member3SearchButton = new Button();
        member3SearchButton.Text = "Search";
        member3SearchButton.Location = new Point(350, 28);
        member3SearchButton.Size = new Size(100, 32);
        member3SearchButton.Click += member3SearchButton_Click;

        // Name
        Label nameLabel = new Label();
        nameLabel.Text = "Name:";
        nameLabel.Location = new Point(25, 80);
        nameLabel.AutoSize = true;

        member3NameTextBox = new TextBox();
        member3NameTextBox.Location = new Point(150, 75);
        member3NameTextBox.Size = new Size(300, 27);

        // Species
        Label speciesLabel = new Label();
        speciesLabel.Text = "Species:";
        speciesLabel.Location = new Point(25, 120);
        speciesLabel.AutoSize = true;

        member3SpeciesTextBox = new TextBox();
        member3SpeciesTextBox.Location = new Point(150, 115);
        member3SpeciesTextBox.Size = new Size(300, 27);

        // Age
        Label ageLabel = new Label();
        ageLabel.Text = "Age:";
        ageLabel.Location = new Point(25, 160);
        ageLabel.AutoSize = true;

        member3AgeTextBox = new TextBox();
        member3AgeTextBox.Location = new Point(150, 155);
        member3AgeTextBox.Size = new Size(100, 27);

        // Recovery Score
        Label recoveryLabel = new Label();
        recoveryLabel.Text = "Recovery Score:";
        recoveryLabel.Location = new Point(25, 200);
        recoveryLabel.AutoSize = true;

        member3RecoveryScoreTextBox = new TextBox();
        member3RecoveryScoreTextBox.Location = new Point(150, 195);
        member3RecoveryScoreTextBox.Size = new Size(100, 27);

        // Status
        Label statusLabel = new Label();
        statusLabel.Text = "Status:";
        statusLabel.Location = new Point(25, 240);
        statusLabel.AutoSize = true;

        member3StatusTextBox = new TextBox();
        member3StatusTextBox.Location = new Point(150, 235);
        member3StatusTextBox.Size = new Size(200, 27);
        member3StatusTextBox.ReadOnly = true;

        // Housing Unit
        Label housingLabel = new Label();
        housingLabel.Text = "Housing Unit:";
        housingLabel.Location = new Point(25, 280);
        housingLabel.AutoSize = true;

        member3HousingUnitTextBox = new TextBox();
        member3HousingUnitTextBox.Location = new Point(150, 275);
        member3HousingUnitTextBox.Size = new Size(200, 27);
        member3HousingUnitTextBox.ReadOnly = true;

        // Save button
        member3SaveButton = new Button();
        member3SaveButton.Text = "Save Changes";
        member3SaveButton.Location = new Point(480, 75);
        member3SaveButton.Size = new Size(130, 40);
        member3SaveButton.Click += member3SaveButton_Click;

        // Delete button
        member3DeleteButton = new Button();
        member3DeleteButton.Text = "Delete Animal";
        member3DeleteButton.Location = new Point(480, 130);
        member3DeleteButton.Size = new Size(130, 40);
        member3DeleteButton.Click += member3DeleteButton_Click;

        // Clear button
        member3ClearButton = new Button();
        member3ClearButton.Text = "Clear";
        member3ClearButton.Location = new Point(480, 185);
        member3ClearButton.Size = new Size(130, 40);
        member3ClearButton.Click += member3ClearButton_Click;

        // Message
        member3MessageLabel = new Label();
        member3MessageLabel.Text = "";
        member3MessageLabel.Location = new Point(25, 325);
        member3MessageLabel.Size = new Size(650, 30);
        member3MessageLabel.AutoSize = false;

        // Add controls to group box
        groupBox.Controls.Add(idLabel);
        groupBox.Controls.Add(member3AnimalIdTextBox);

        groupBox.Controls.Add(member3SearchButton);

        groupBox.Controls.Add(nameLabel);
        groupBox.Controls.Add(member3NameTextBox);

        groupBox.Controls.Add(speciesLabel);
        groupBox.Controls.Add(member3SpeciesTextBox);

        groupBox.Controls.Add(ageLabel);
        groupBox.Controls.Add(member3AgeTextBox);

        groupBox.Controls.Add(recoveryLabel);
        groupBox.Controls.Add(member3RecoveryScoreTextBox);

        groupBox.Controls.Add(statusLabel);
        groupBox.Controls.Add(member3StatusTextBox);

        groupBox.Controls.Add(housingLabel);
        groupBox.Controls.Add(member3HousingUnitTextBox);

        groupBox.Controls.Add(member3SaveButton);
        groupBox.Controls.Add(member3DeleteButton);
        groupBox.Controls.Add(member3ClearButton);

        groupBox.Controls.Add(member3MessageLabel);

        // Add group box to the form
        Controls.Add(groupBox);
    }

    // Search for an animal
    private void member3SearchButton_Click(
        object? sender,
        EventArgs e)
    {
        string animalId = member3AnimalIdTextBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(animalId))
        {
            member3SetMessage(
                "Please enter an Animal ID.",
                true);

            return;
        }

        AnimalRecord? animal =
            member3Service.FindAnimal(animalId);

        if (animal == null)
        {
            member3SetMessage(
                "Animal not found.",
                true);

            member3ClearFields();

            return;
        }

        // Load animal into the form
        member3NameTextBox.Text = animal.Name;
        member3SpeciesTextBox.Text = animal.Species;
        member3AgeTextBox.Text = animal.Age;
        member3RecoveryScoreTextBox.Text =
            animal.RecoveryScore;

        member3StatusTextBox.Text = animal.Status;
        member3HousingUnitTextBox.Text =
            animal.HousingUnit;

        // Enter edit mode
        member3EditMode = true;

        // Animal ID cannot be changed
        member3AnimalIdTextBox.ReadOnly = true;

        member3SaveButton.Enabled = true;
        member3DeleteButton.Enabled = true;

        member3SetMessage(
            "Animal loaded. You are now in Edit Mode.",
            false);
    }

    // Save changes
    private void member3SaveButton_Click(
        object? sender,
        EventArgs e)
    {
        if (!member3EditMode)
        {
            member3SetMessage(
                "Please search for an animal first.",
                true);

            return;
        }

        string animalId =
            member3AnimalIdTextBox.Text.Trim();

        string name =
            member3NameTextBox.Text.Trim();

        string species =
            member3SpeciesTextBox.Text.Trim();

        string age =
            member3AgeTextBox.Text.Trim();

        string recoveryScore =
            member3RecoveryScoreTextBox.Text.Trim();

        string status =
            member3StatusTextBox.Text.Trim();

        string housingUnit =
            member3HousingUnitTextBox.Text.Trim();

        // Basic checking
        if (string.IsNullOrWhiteSpace(name) ||
            string.IsNullOrWhiteSpace(species) ||
            string.IsNullOrWhiteSpace(age) ||
            string.IsNullOrWhiteSpace(recoveryScore))
        {
            member3SetMessage(
                "Please complete all required fields.",
                true);

            return;
        }

        AnimalRecord updatedAnimal =
            new AnimalRecord(
                animalId,
                name,
                species,
                age,
                recoveryScore,
                status,
                housingUnit);

        bool updated =
            member3Service.UpdateAnimal(updatedAnimal);

        if (updated)
        {
            MessageBox.Show(
                "Animal updated successfully.",
                "Update Successful",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            member3ClearFields();
        }
        else
        {
            member3SetMessage(
                "The animal could not be updated.",
                true);
        }
    }

    // Delete animal
    private void member3DeleteButton_Click(
        object? sender,
        EventArgs e)
    {
        if (!member3EditMode)
        {
            member3SetMessage(
                "Please search for an animal first.",
                true);

            return;
        }

        string animalId =
            member3AnimalIdTextBox.Text.Trim();

        DialogResult result = MessageBox.Show(
            $"Are you sure you want to delete animal '{animalId}'?",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (result != DialogResult.Yes)
        {
            return;
        }

        bool deleted =
            member3Service.DeleteAnimal(animalId);

        if (deleted)
        {
            MessageBox.Show(
                "Animal deleted successfully.",
                "Delete Successful",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            member3ClearFields();
        }
        else
        {
            member3SetMessage(
                "The animal could not be deleted.",
                true);
        }
    }

    // Clear button
    private void member3ClearButton_Click(
        object? sender,
        EventArgs e)
    {
        member3ClearFields();
    }

    // Clear form and leave edit mode
    private void member3ClearFields()
    {
        member3AnimalIdTextBox.Clear();
        member3NameTextBox.Clear();
        member3SpeciesTextBox.Clear();
        member3AgeTextBox.Clear();
        member3RecoveryScoreTextBox.Clear();
        member3StatusTextBox.Clear();
        member3HousingUnitTextBox.Clear();

        member3AnimalIdTextBox.ReadOnly = false;

        member3SaveButton.Enabled = false;
        member3DeleteButton.Enabled = false;

        member3EditMode = false;

        member3SetMessage(
            "Form cleared.",
            false);
    }

    private void member3SetMessage(
        string message,
        bool error)
    {
        member3MessageLabel.Text = message;
    }
}