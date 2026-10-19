using System.Drawing;
using System.Windows.Forms;

namespace BaobabRidge
{

    partial class MainForm
    {
        private System.ComponentModel.IContainer components;
        private Label lblHeading;
        private Label lblAnimalId;
        private TextBox txtAnimalId;
        private Label lblName;
        private TextBox txtName;
        private Label lblSpecies;
        private TextBox txtSpecies;
        private Label lblAge;
        private TextBox txtAge;
        private Label lblRecoveryScore;
        private TextBox txtRecoveryScore;
        private Button btnAdd;
        private Button btnClear;
        private Button btnSearch;
        private Button btnSaveChanges;
        private Button btnDelete;
        private Label lblFeedback;
        private Label lblStatusTitle;
        private Label lblStatusValue;
        private Label lblHousingTitle;
        private Label lblHousingValue;
        private ErrorProvider errorProvider;
        private DataGridView dgvAnimals;

        // Releases the components when the form closes.
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        // Creates the controls and sets their positions, labels, tab order, and click handlers.
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblHeading = new Label();
            lblAnimalId = new Label();
            txtAnimalId = new TextBox();
            lblName = new Label();
            txtName = new TextBox();
            lblSpecies = new Label();
            txtSpecies = new TextBox();
            lblAge = new Label();
            txtAge = new TextBox();
            lblRecoveryScore = new Label();
            txtRecoveryScore = new TextBox();
            btnAdd = new Button();
            btnClear = new Button();
            btnSearch = new Button();
            btnSaveChanges = new Button();
            btnDelete = new Button();
            lblFeedback = new Label();
            lblStatusTitle = new Label();
            lblStatusValue = new Label();
            lblHousingTitle = new Label();
            lblHousingValue = new Label();
            errorProvider = new ErrorProvider(components);
            dgvAnimals = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvAnimals).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();

            // dgvAnimals
            dgvAnimals.Name = "dgvAnimals";
            dgvAnimals.Location = new Point(24, 610);
            dgvAnimals.Size = new Size(580, 220);
            dgvAnimals.Anchor = AnchorStyles.Top
                | AnchorStyles.Left
                | AnchorStyles.Right;
            dgvAnimals.ReadOnly = true;
            dgvAnimals.AllowUserToAddRows = false;
            dgvAnimals.AllowUserToDeleteRows = false;
            dgvAnimals.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgvAnimals.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvAnimals.MultiSelect = false;
            dgvAnimals.RowHeadersVisible = false;

            // lblHeading
            lblHeading.Name = "lblHeading";
            lblHeading.Text = "Baobab Ridge\nAnimal records";
            lblHeading.Location = new Point(24, 20);
            lblHeading.Size = new Size(560, 65);
            lblHeading.Font = new Font("Segoe UI", 17, FontStyle.Bold);
            lblHeading.ForeColor = Color.FromArgb(28, 94, 56);

            // lblAnimalId
            lblAnimalId.Name = "lblAnimalId";
            lblAnimalId.Text = "Animal &ID";
            lblAnimalId.Location = new Point(24, 109);
            lblAnimalId.Size = new Size(150, 25);
            lblAnimalId.TabIndex = 0;

            // txtAnimalId
            txtAnimalId.Name = "txtAnimalId";
            txtAnimalId.Location = new Point(190, 105);
            txtAnimalId.Size = new Size(280, 25);
            txtAnimalId.TabIndex = 1;
            txtAnimalId.AccessibleName = "Animal ID";
            txtAnimalId.PlaceholderText = "WR-0007";

            // lblName
            lblName.Name = "lblName";
            lblName.Text = "&Name";
            lblName.Location = new Point(24, 155);
            lblName.Size = new Size(150, 25);
            lblName.TabIndex = 2;

            // txtName
            txtName.Name = "txtName";
            txtName.Location = new Point(190, 151);
            txtName.Size = new Size(410, 25);
            txtName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtName.TabIndex = 3;
            txtName.AccessibleName = "Name";
            txtName.PlaceholderText = "Naledi";

            // lblSpecies
            lblSpecies.Name = "lblSpecies";
            lblSpecies.Text = "&Species";
            lblSpecies.Location = new Point(24, 201);
            lblSpecies.Size = new Size(150, 25);
            lblSpecies.TabIndex = 4;

            // txtSpecies
            txtSpecies.Name = "txtSpecies";
            txtSpecies.Location = new Point(190, 197);
            txtSpecies.Size = new Size(410, 25);
            txtSpecies.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSpecies.TabIndex = 5;
            txtSpecies.AccessibleName = "Species";
            txtSpecies.PlaceholderText = "Serval";

            // lblAge
            lblAge.Name = "lblAge";
            lblAge.Text = "A&ge (years)";
            lblAge.Location = new Point(24, 247);
            lblAge.Size = new Size(150, 25);
            lblAge.TabIndex = 6;

            // txtAge
            txtAge.Name = "txtAge";
            txtAge.Location = new Point(190, 243);
            txtAge.Size = new Size(410, 25);
            txtAge.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtAge.TabIndex = 7;
            txtAge.AccessibleName = "Age (years)";
            txtAge.PlaceholderText = "Whole number: 0 to 100";

            // lblRecoveryScore
            lblRecoveryScore.Name = "lblRecoveryScore";
            lblRecoveryScore.Text = "&Recovery Score";
            lblRecoveryScore.Location = new Point(24, 293);
            lblRecoveryScore.Size = new Size(150, 25);
            lblRecoveryScore.TabIndex = 8;

            // txtRecoveryScore
            txtRecoveryScore.Name = "txtRecoveryScore";
            txtRecoveryScore.Location = new Point(190, 289);
            txtRecoveryScore.Size = new Size(410, 25);
            txtRecoveryScore.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtRecoveryScore.TabIndex = 9;
            txtRecoveryScore.AccessibleName = "Recovery Score";
            txtRecoveryScore.PlaceholderText = "Whole number: 0 to 100";

            // btnAdd
            btnAdd.Name = "btnAdd";
            btnAdd.Text = "&Add Animal";
            btnAdd.Location = new Point(190, 340);
            btnAdd.Size = new Size(150, 38);
            btnAdd.TabIndex = 10;

            btnAdd.Click += btnAdd_Click;
            // btnClear
            btnClear.Name = "btnClear";
            btnClear.Text = "&Clear";
            btnClear.Location = new Point(350, 340);
            btnClear.Size = new Size(100, 38);
            btnClear.TabIndex = 11;

            btnClear.Click += btnClear_Click;

            // Search uses the shared ID field; Save and Delete become available in Edit Mode.
            btnSearch.Name = "btnSearch";
            btnSearch.Text = "&Search";
            btnSearch.Location = new Point(480, 102);
            btnSearch.Size = new Size(120, 30);
            btnSearch.TabIndex = 2;
            btnSearch.Click += btnSearch_Click;
            btnSaveChanges.Name = "btnSaveChanges";
            btnSaveChanges.Text = "Sa&ve Changes";
            btnSaveChanges.Location = new Point(190, 390);
            btnSaveChanges.Size = new Size(150, 38);
            btnSaveChanges.TabIndex = 12;
            btnSaveChanges.Enabled = false;
            btnSaveChanges.Click += btnSaveChanges_Click;
            btnDelete.Name = "btnDelete";
            btnDelete.Text = "&Delete Animal";
            btnDelete.Location = new Point(350, 390);
            btnDelete.Size = new Size(150, 38);
            btnDelete.TabIndex = 13;
            btnDelete.Enabled = false;
            btnDelete.Click += btnDelete_Click;
            // lblFeedback
            lblFeedback.Name = "lblFeedback";
            lblFeedback.Text = "Enter the animal's details, then select Add Animal.";
            lblFeedback.Location = new Point(24, 445);
            lblFeedback.Size = new Size(580, 75);
            lblFeedback.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblFeedback.AccessibleName = "Save result";

            // lblStatusTitle
            lblStatusTitle.Name = "lblStatusTitle";
            lblStatusTitle.Text = "Animal status";
            lblStatusTitle.Location = new Point(24, 530);
            lblStatusTitle.Size = new Size(155, 30);

            // lblStatusValue
            lblStatusValue.Name = "lblStatusValue";
            lblStatusValue.Text = "-";
            lblStatusValue.Location = new Point(190, 530);
            lblStatusValue.Size = new Size(410, 30);

            // lblHousingTitle
            lblHousingTitle.Name = "lblHousingTitle";
            lblHousingTitle.Text = "Housing unit";
            lblHousingTitle.Location = new Point(24, 570);
            lblHousingTitle.Size = new Size(155, 30);

            // lblHousingValue
            lblHousingValue.Name = "lblHousingValue";
            lblHousingValue.Text = "-";
            lblHousingValue.Location = new Point(190, 570);
            lblHousingValue.Size = new Size(410, 30);

            errorProvider.ContainerControl = this;
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            AutoScaleDimensions = new SizeF(7, 15);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(640, 860);
            MinimumSize = new Size(656, 899);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Baobab Ridge - Animal Records";
            AcceptButton = btnAdd;
            Controls.Add(lblHeading);
            Controls.Add(lblAnimalId);
            Controls.Add(txtAnimalId);
            Controls.Add(lblName);
            Controls.Add(txtName);
            Controls.Add(lblSpecies);
            Controls.Add(txtSpecies);
            Controls.Add(lblAge);
            Controls.Add(txtAge);
            Controls.Add(lblRecoveryScore);
            Controls.Add(txtRecoveryScore);
            Controls.Add(btnAdd);
            Controls.Add(btnClear);
            Controls.Add(btnSearch);
            Controls.Add(btnSaveChanges);
            Controls.Add(btnDelete);
            Controls.Add(lblFeedback);
            Controls.Add(lblStatusTitle);
            Controls.Add(lblStatusValue);
            Controls.Add(lblHousingTitle);
            Controls.Add(lblHousingValue);
            Controls.Add(dgvAnimals);
            ((System.ComponentModel.ISupportInitialize)dgvAnimals).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
