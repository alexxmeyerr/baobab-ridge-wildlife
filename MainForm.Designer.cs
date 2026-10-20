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
        private Button btnSummaryReport;
        private Label lblFeedback;
        private Label lblStatusTitle;
        private Label lblStatusValue;
        private Label lblHousingTitle;
        private Label lblHousingValue;
        private ErrorProvider errorProvider;
        private DataGridView dgvAnimals;
        private TextBox txtSummaryReport;
        private Label lblLoadInfo;

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
            btnSummaryReport = new Button();
            lblFeedback = new Label();
            lblStatusTitle = new Label();
            lblStatusValue = new Label();
            lblHousingTitle = new Label();
            lblHousingValue = new Label();
            errorProvider = new ErrorProvider(components);
            dgvAnimals = new DataGridView();
            txtSummaryReport = new TextBox();
            lblLoadInfo = new Label();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvAnimals).BeginInit();
            SuspendLayout();
            // 
            // lblHeading
            // 
            lblHeading.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            lblHeading.ForeColor = Color.FromArgb(28, 94, 56);
            lblHeading.Location = new Point(24, 20);
            lblHeading.Name = "lblHeading";
            lblHeading.Size = new Size(560, 65);
            lblHeading.TabIndex = 17;
            lblHeading.Text = "Baobab Ridge\nAnimal records";
            // 
            // lblAnimalId
            // 
            lblAnimalId.Location = new Point(24, 109);
            lblAnimalId.Name = "lblAnimalId";
            lblAnimalId.Size = new Size(150, 25);
            lblAnimalId.TabIndex = 0;
            lblAnimalId.Text = "Animal &ID";
            // 
            // txtAnimalId
            // 
            txtAnimalId.AccessibleName = "Animal ID";
            txtAnimalId.Location = new Point(190, 105);
            txtAnimalId.Name = "txtAnimalId";
            txtAnimalId.PlaceholderText = "WR-0007";
            txtAnimalId.Size = new Size(280, 23);
            txtAnimalId.TabIndex = 1;
            // 
            // lblName
            // 
            lblName.Location = new Point(24, 155);
            lblName.Name = "lblName";
            lblName.Size = new Size(150, 25);
            lblName.TabIndex = 2;
            lblName.Text = "&Name";
            // 
            // txtName
            // 
            txtName.AccessibleName = "Name";
            txtName.Location = new Point(190, 151);
            txtName.Name = "txtName";
            txtName.PlaceholderText = "Naledi";
            txtName.Size = new Size(410, 23);
            txtName.TabIndex = 3;
            // 
            // lblSpecies
            // 
            lblSpecies.Location = new Point(24, 201);
            lblSpecies.Name = "lblSpecies";
            lblSpecies.Size = new Size(150, 25);
            lblSpecies.TabIndex = 4;
            lblSpecies.Text = "&Species";
            // 
            // txtSpecies
            // 
            txtSpecies.AccessibleName = "Species";
            txtSpecies.Location = new Point(190, 197);
            txtSpecies.Name = "txtSpecies";
            txtSpecies.PlaceholderText = "Serval";
            txtSpecies.Size = new Size(410, 23);
            txtSpecies.TabIndex = 5;
            // 
            // lblAge
            // 
            lblAge.Location = new Point(24, 247);
            lblAge.Name = "lblAge";
            lblAge.Size = new Size(150, 25);
            lblAge.TabIndex = 6;
            lblAge.Text = "A&ge (years)";
            // 
            // txtAge
            // 
            txtAge.AccessibleName = "Age (years)";
            txtAge.Location = new Point(190, 243);
            txtAge.Name = "txtAge";
            txtAge.PlaceholderText = "Whole number: 0 to 100";
            txtAge.Size = new Size(410, 23);
            txtAge.TabIndex = 7;
            // 
            // lblRecoveryScore
            // 
            lblRecoveryScore.Location = new Point(24, 293);
            lblRecoveryScore.Name = "lblRecoveryScore";
            lblRecoveryScore.Size = new Size(150, 25);
            lblRecoveryScore.TabIndex = 8;
            lblRecoveryScore.Text = "&Recovery Score";
            // 
            // txtRecoveryScore
            // 
            txtRecoveryScore.AccessibleName = "Recovery Score";
            txtRecoveryScore.Location = new Point(190, 289);
            txtRecoveryScore.Name = "txtRecoveryScore";
            txtRecoveryScore.PlaceholderText = "Whole number: 0 to 100";
            txtRecoveryScore.Size = new Size(410, 23);
            txtRecoveryScore.TabIndex = 9;
            // 
            // btnAdd
            // 
            btnAdd.AccessibleDescription = "Adds a new animal after checking all input fields.";
            btnAdd.Location = new Point(190, 340);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(150, 38);
            btnAdd.TabIndex = 10;
            btnAdd.Text = "&Add Animal";
            btnAdd.Click += btnAdd_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(350, 340);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(100, 38);
            btnClear.TabIndex = 11;
            btnClear.Text = "&Clear";
            btnClear.Click += btnClear_Click;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(480, 102);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(120, 30);
            btnSearch.TabIndex = 2;
            btnSearch.Text = "&Search";
            btnSearch.Click += btnSearch_Click;
            // 
            // btnSaveChanges
            // 
            btnSaveChanges.Enabled = false;
            btnSaveChanges.Location = new Point(190, 390);
            btnSaveChanges.Name = "btnSaveChanges";
            btnSaveChanges.Size = new Size(150, 38);
            btnSaveChanges.TabIndex = 12;
            btnSaveChanges.Text = "Sa&ve Changes";
            btnSaveChanges.Click += btnSaveChanges_Click;
            // 
            // btnDelete
            // 
            btnDelete.Enabled = false;
            btnDelete.Location = new Point(350, 390);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(150, 38);
            btnDelete.TabIndex = 13;
            btnDelete.Text = "&Delete Animal";
            btnDelete.Click += btnDelete_Click;
            // 
            // btnSummaryReport
            // 
            btnSummaryReport.Location = new Point(24, 390);
            btnSummaryReport.Name = "btnSummaryReport";
            btnSummaryReport.Size = new Size(150, 38);
            btnSummaryReport.TabIndex = 14;
            btnSummaryReport.Text = "&Generate Summary Report";
            btnSummaryReport.Click += btnSummaryReport_Click;
            // 
            // lblFeedback
            // 
            lblFeedback.AccessibleName = "Save result";
            lblFeedback.Location = new Point(24, 445);
            lblFeedback.Name = "lblFeedback";
            lblFeedback.Size = new Size(580, 75);
            lblFeedback.TabIndex = 18;
            lblFeedback.Text = "Enter the animal's details, then select Add Animal.";
            // 
            // lblStatusTitle
            // 
            lblStatusTitle.Location = new Point(24, 530);
            lblStatusTitle.Name = "lblStatusTitle";
            lblStatusTitle.Size = new Size(155, 30);
            lblStatusTitle.TabIndex = 19;
            lblStatusTitle.Text = "Animal status";
            // 
            // lblStatusValue
            // 
            lblStatusValue.Location = new Point(190, 530);
            lblStatusValue.Name = "lblStatusValue";
            lblStatusValue.Size = new Size(410, 30);
            lblStatusValue.TabIndex = 20;
            lblStatusValue.Text = "-";
            // 
            // lblHousingTitle
            // 
            lblHousingTitle.Location = new Point(24, 570);
            lblHousingTitle.Name = "lblHousingTitle";
            lblHousingTitle.Size = new Size(155, 30);
            lblHousingTitle.TabIndex = 21;
            lblHousingTitle.Text = "Housing unit";
            // 
            // lblHousingValue
            // 
            lblHousingValue.Location = new Point(190, 570);
            lblHousingValue.Name = "lblHousingValue";
            lblHousingValue.Size = new Size(410, 30);
            lblHousingValue.TabIndex = 22;
            lblHousingValue.Text = "-";
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            // 
            // dgvAnimals
            // 
            dgvAnimals.AllowUserToAddRows = false;
            dgvAnimals.AllowUserToDeleteRows = false;
            dgvAnimals.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvAnimals.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAnimals.Location = new Point(24, 635);
            dgvAnimals.MultiSelect = false;
            dgvAnimals.Name = "dgvAnimals";
            dgvAnimals.ReadOnly = true;
            dgvAnimals.RowHeadersVisible = false;
            dgvAnimals.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAnimals.Size = new Size(1052, 195);
            dgvAnimals.TabIndex = 23;
            dgvAnimals.CellClick += dgvAnimals_CellClick;
            // 
            // txtSummaryReport
            // 
            txtSummaryReport.AccessibleName = "Summary report";
            txtSummaryReport.Font = new Font("Consolas", 9F);
            txtSummaryReport.Location = new Point(630, 105);
            txtSummaryReport.Multiline = true;
            txtSummaryReport.Name = "txtSummaryReport";
            txtSummaryReport.ReadOnly = true;
            txtSummaryReport.ScrollBars = ScrollBars.Both;
            txtSummaryReport.Size = new Size(446, 490);
            txtSummaryReport.TabIndex = 15;
            txtSummaryReport.Text = "Select Generate Summary Report to view current statistics.";
            txtSummaryReport.WordWrap = false;
            // 
            // lblLoadInfo
            // 
            lblLoadInfo.Location = new Point(24, 607);
            lblLoadInfo.Name = "lblLoadInfo";
            lblLoadInfo.Size = new Size(1052, 25);
            lblLoadInfo.TabIndex = 16;
            // 
            // MainForm
            // 
            AcceptButton = btnAdd;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 860);
            Controls.Add(txtSummaryReport);
            Controls.Add(lblLoadInfo);
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
            Controls.Add(btnSummaryReport);
            Controls.Add(lblFeedback);
            Controls.Add(lblStatusTitle);
            Controls.Add(lblStatusValue);
            Controls.Add(lblHousingTitle);
            Controls.Add(lblHousingValue);
            Controls.Add(dgvAnimals);
            MinimumSize = new Size(1116, 814);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Baobab Ridge - Animal Records";
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvAnimals).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}