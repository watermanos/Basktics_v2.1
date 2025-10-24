using DocumentFormat.OpenXml.Spreadsheet;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Forms;

namespace Basktics_v2._0
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public partial class MatchForm : Form
    {
        public MatchForm()
        {
            InitializeComponent();
            
            // Event handlers για υπάρχοντα controls
            this.dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);
            this.dataGridView1.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dataGridView1_EditingControlShowing1);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // Timer initialization
            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 1000;
            gameTimer.Tick += GameTimer_Tick;

            currentGameTime = TimeSpan.FromMinutes(40);
            isTimerRunning = false;

            playerTotalTime = new Dictionary<string, TimeSpan>();
            playerStartTimes = new Dictionary<string, DateTime>();
            activePlayers = new List<string>();

            // Event handlers για τα νέα κουμπιά
            this.btnStartTimer.Click += new System.EventHandler(this.btnStartTimer_Click);
            this.btnPauseTimer.Click += new System.EventHandler(this.btnPauseTimer_Click);
            this.btnResetTimer.Click += new System.EventHandler(this.btnResetTimer_Click);
            this.btnSubstitute.Click += new System.EventHandler(this.btnSubstitute_Click);
            //this.btnSetStartingFive.Click += new System.EventHandler(this.btnSetStartingFive_Click);

            // Ενεργοποίηση των events για summary
            this.dataGridView1.CellValueChanged += (s, e) =>
            {
                if (e.RowIndex >= 0 &&
                   (dataGridView1.Columns[e.ColumnIndex].Name == "Column11" ||
                    dataGridView1.Columns[e.ColumnIndex].Name == "Column14"))
                {
                    CalculatePointsForPositionSummary();
                }
            };

            this.dataGridView1.RowsAdded += (s, e) => CalculatePointsForPositionSummary();
            this.dataGridView1.RowsRemoved += (s, e) => CalculatePointsForPositionSummary();

            // Αρχικοποίηση εμφάνισης
            UpdateGameTimeDisplay();
            lblTimerStatus.Text = "GAME CLOCK";

            btnStartTimer.Enabled = false;
        }

        private void SetStartingFiveAutomatically()
        {
            activePlayers.Clear();
            playerTotalTime.Clear();
            playerStartTimes.Clear();

            int playersAdded = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (playersAdded >= 5) break;

                // Έλεγχος αν η γραμμή δεν είναι new row και αν έχει όνομα παίκτη
                if (!row.IsNewRow && row.Cells["Column1"].Value != null &&
                    !string.IsNullOrWhiteSpace(row.Cells["Column1"].Value.ToString()))
                {
                    string playerName = row.Cells["Column1"].Value.ToString().Trim();
                    if (!string.IsNullOrEmpty(playerName) && !activePlayers.Contains(playerName))
                    {
                        activePlayers.Add(playerName);
                        playerTotalTime[playerName] = TimeSpan.Zero;
                        playersAdded++;
                    }
                }
            }

            // Ενημέρωση εμφάνισης
            UpdateActivePlayersList();
            UpdatePlayingTimeInGrid();
            RefreshPlayersComboBox();

            // Ενεργοποίηση Play μόνο αν υπάρχουν 5 παίκτες
            btnStartTimer.Enabled = (activePlayers.Count == 5);
        }



        private void RefreshPlayersComboBox()
        {
            if (cmbPlayersOut == null || cmbPlayersIn == null) return;

            cmbPlayersOut.Items.Clear();
            cmbPlayersIn.Items.Clear();

           
            cmbPlayersOut.Items.Add("-- Select Player --");
            cmbPlayersIn.Items.Add("-- Select Player --");


            foreach (string player in activePlayers)
            {
                cmbPlayersOut.Items.Add(player);
            }


            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow && row.Cells["Column1"].Value != null)
                {
                    string playerName = row.Cells["Column1"].Value.ToString().Trim();
                    if (!string.IsNullOrEmpty(playerName) && !activePlayers.Contains(playerName))
                    {
                        cmbPlayersIn.Items.Add(playerName);
                    }
                }
            }
            if (cmbPlayersOut.Items.Count > 0) cmbPlayersOut.SelectedIndex = 0;
            if (cmbPlayersIn.Items.Count > 0) cmbPlayersIn.SelectedIndex = 0;
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (currentGameTime.TotalSeconds > 0)
            {
                currentGameTime = currentGameTime.Subtract(TimeSpan.FromSeconds(1));
                UpdateGameTimeDisplay();
                UpdateActivePlayersTime();
            }
            else
            {
                gameTimer.Stop();
                isTimerRunning = false;
                lblTimerStatus.Text = "FINISHED";
            }
        }

        private void UpdateGameTimeDisplay()
        {
            if (lblGameClock != null)
            {
                lblGameClock.Text = currentGameTime.ToString(@"mm\:ss");
            }
        }

        private void UpdateActivePlayersTime()
        {
            if (!isTimerRunning) return;

            DateTime currentTime = DateTime.Now;

            foreach (var player in activePlayers)
            {
                if (playerStartTimes.ContainsKey(player))
                {
                    TimeSpan elapsed = currentTime - playerStartTimes[player];
                    if (playerTotalTime.ContainsKey(player))
                    {
                        playerTotalTime[player] += elapsed;
                    }
                    playerStartTimes[player] = currentTime;
                }
            }

            UpdateActivePlayersList();
            UpdatePlayingTimeInGrid();
        }

        private void UpdatePlayingTimeInGrid()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                DataGridViewCell playerCell = row.Cells["Column1"];
                if (playerCell?.Value != null)
                {
                    string playerName = playerCell.Value.ToString();
                    if (playerTotalTime.ContainsKey(playerName))
                    {
                        row.Cells["ColumnPlayingTime"].Value = playerTotalTime[playerName].ToString(@"mm\:ss");
                    }
                    else
                    {
                        row.Cells["ColumnPlayingTime"].Value = "00:00";
                    }
                }
            }
        }
        private void UpdatePlayersTime()
        {
            // Για κάθε ενεργό παίκτη, ενημερώνουμε τον χρόνο
            foreach (string player in activePlayers)
            {
                if (playerStartTimes.ContainsKey(player))
                {
                    TimeSpan elapsed = DateTime.Now - playerStartTimes[player];
                    playerTotalTime[player] = playerTotalTime[player].Add(elapsed);
                    playerStartTimes[player] = DateTime.Now;
                }
            }

            // Ενημέρωση εμφάνισης χρόνου στον πίνακα
            UpdatePlayingTimeInGrid();
            UpdateActivePlayersList();
        }

       
        private void UpdateActivePlayersList()
        {
            if (lstActivePlayers == null) return;

            lstActivePlayers.Items.Clear();
            foreach (string player in activePlayers)
            {
                if (playerTotalTime.ContainsKey(player))
                {
                    string timeStr = playerTotalTime[player].ToString(@"mm\:ss");
                    lstActivePlayers.Items.Add($"{player} - {timeStr}");
                }
                else
                {
                    lstActivePlayers.Items.Add($"{player} - 00:00");
                }
            }
        }



        private void btnStartTimer_Click(object sender, EventArgs e)
        {
            if (activePlayers.Count != 5)
            {
                MessageBox.Show("Please ensure there are exactly 5 players with names in the first five rows.", "Game Start", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!isTimerRunning)
            {
                gameTimer.Start();
                isTimerRunning = true;
                lastUpdateTime = DateTime.Now;
                lblTimerStatus.Text = "RUNNING";

                foreach (var player in activePlayers)
                {
                    playerStartTimes[player] = DateTime.Now;
                }
            }
        }

        private void btnPauseTimer_Click(object sender, EventArgs e)
        {
            if (isTimerRunning)
            {
                gameTimer.Stop();
                isTimerRunning = false;
                lblTimerStatus.Text = "PAUSED";

                // Ενημέρωση συνολικού χρόνου για ενεργούς παίκτες
                UpdateActivePlayersTime();
            }
        }

        private void btnResetTimer_Click(object sender, EventArgs e)
        {
            gameTimer.Stop();
            isTimerRunning = false;
            currentGameTime = TimeSpan.FromMinutes(40);
            lblTimerStatus.Text = "STOPPED";

            // Επαναφορά χρόνων
            playerTotalTime.Clear();
            playerStartTimes.Clear();
            activePlayers.Clear();

            // Αυτόματη επαναφορά πρώτων 5 παικτών
            SetStartingFiveAutomatically();

            UpdateGameTimeDisplay();
            UpdateActivePlayersList();
            UpdatePlayingTimeInGrid();
            RefreshPlayersComboBox();
        }

        private void btnSubstitute_Click(object sender, EventArgs e)
        {
            if (cmbPlayersOut.SelectedIndex <= 0 || cmbPlayersIn.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select both players for substitution.", "Substitution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string playerOut = cmbPlayersOut.SelectedItem.ToString();
            string playerIn = cmbPlayersIn.SelectedItem.ToString();

            // Έλεγχος αν ο παίκτης που βγαίνει είναι ενεργός
            if (!activePlayers.Contains(playerOut))
            {
                MessageBox.Show($"{playerOut} is not in the active players list.", "Substitution Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Έλεγχος αν ο παίκτης που μπαίνει είναι ήδη ενεργός
            if (activePlayers.Contains(playerIn))
            {
                MessageBox.Show($"{playerIn} is already in the active players list.", "Substitution Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Ανανέωση χρόνου πριν την αλλαγή
            if (isTimerRunning)
            {
                UpdateActivePlayersTime();
            }

            // Αλλαγή παίκτη
            activePlayers.Remove(playerOut);
            playerStartTimes.Remove(playerOut);

            activePlayers.Add(playerIn);
            if (!playerTotalTime.ContainsKey(playerIn))
            {
                playerTotalTime[playerIn] = TimeSpan.Zero;
            }

            if (isTimerRunning)
            {
                playerStartTimes[playerIn] = DateTime.Now;
            }

            // Ενημέρωση εμφάνισης
            UpdateActivePlayersList();
            UpdatePlayingTimeInGrid();

            // Επαναφορά selections
            cmbPlayersOut.SelectedIndex = 0;
            cmbPlayersIn.SelectedIndex = 0;

            // Ενημέρωση των combo boxes με βάση την τρέχουσα λίστα activePlayers
            RefreshPlayersComboBox();

            MessageBox.Show($"Substitution: {playerOut} → {playerIn}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private System.Windows.Forms.Timer gameTimer;
        private TimeSpan currentGameTime;
        private bool isGameRunning;
        private Dictionary<string, TimeSpan> playerTotalTime;
        private Dictionary<string, DateTime> playerStartTimes;
        private List<string> activePlayers;
        private bool isTimerRunning;
        private DateTime lastUpdateTime;
        private void SaveDataToExcelb(string filePath)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Match Data");

                // Προσθήκη επικεφαλίδων από το DataGridView
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = dataGridView1.Columns[i].HeaderText;
                }

                //Adds headers from the DataGridView.
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1].Value = dataGridView1.Rows[i].Cells[j].Value;
                    }
                }

                // data save excel
                FileInfo excelFile = new FileInfo(filePath);
                package.SaveAs(excelFile);
            }

            MessageBox.Show("Data successfully saved to Excel!", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column18 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column19 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column26 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column24 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column25 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column20 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPlayingTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.checkBox8 = new System.Windows.Forms.CheckBox();
            this.checkBox9 = new System.Windows.Forms.CheckBox();
            this.checkBox10 = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.Summary = new System.Windows.Forms.DataGridView();
            this.Column21 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column22 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column23 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.lblGameClock = new System.Windows.Forms.Label();
            this.btnStartTimer = new System.Windows.Forms.Button();
            this.btnPauseTimer = new System.Windows.Forms.Button();
            this.btnResetTimer = new System.Windows.Forms.Button();
            this.cmbPlayersOut = new System.Windows.Forms.ComboBox();
            this.cmbPlayersIn = new System.Windows.Forms.ComboBox();
            this.btnSubstitute = new System.Windows.Forms.Button();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.lblTimerStatus = new System.Windows.Forms.Label();
            this.groupBoxActivePlayers = new System.Windows.Forms.Label();
            this.lblSubstitutions = new System.Windows.Forms.Label();
            this.lstActivePlayers = new System.Windows.Forms.ListBox();
            this.groupBoxTimer = new System.Windows.Forms.GroupBox();
            this.groupActiveplr = new System.Windows.Forms.GroupBox();
            this.groupBoxSubstitutions = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.basketicsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Summary)).BeginInit();
            this.groupBoxTimer.SuspendLayout();
            this.groupActiveplr.SuspendLayout();
            this.groupBoxSubstitutions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.basketicsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(578, 93);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(70, 22);
            this.numericUpDown1.TabIndex = 0;
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(765, 93);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(70, 22);
            this.numericUpDown2.TabIndex = 1;
            this.numericUpDown2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column14,
            this.Column2,
            this.Column15,
            this.Column16,
            this.Column17,
            this.Column3,
            this.Column18,
            this.Column19,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8,
            this.Column9,
            this.Column26,
            this.Column24,
            this.Column25,
            this.Column13,
            this.Column11,
            this.Column12,
            this.Column20,
            this.Column10,
            this.ColumnPlayingTime});
            this.dataGridView1.Location = new System.Drawing.Point(12, 261);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1489, 353);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Player";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.Width = 75;
            // 
            // Column14
            // 
            this.Column14.HeaderText = "Position";
            this.Column14.MinimumWidth = 6;
            this.Column14.Name = "Column14";
            this.Column14.Width = 60;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "LayUp Made";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.Width = 50;
            // 
            // Column15
            // 
            this.Column15.HeaderText = "LayUp Missed";
            this.Column15.MinimumWidth = 6;
            this.Column15.Name = "Column15";
            this.Column15.Width = 55;
            // 
            // Column16
            // 
            this.Column16.HeaderText = "MidRange Made";
            this.Column16.MinimumWidth = 6;
            this.Column16.Name = "Column16";
            this.Column16.Width = 65;
            // 
            // Column17
            // 
            this.Column17.HeaderText = "MidRange Missed";
            this.Column17.MinimumWidth = 6;
            this.Column17.Name = "Column17";
            this.Column17.Width = 65;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "3-point";
            this.Column3.MinimumWidth = 6;
            this.Column3.Name = "Column3";
            this.Column3.Width = 55;
            // 
            // Column18
            // 
            this.Column18.HeaderText = "3-point Missed";
            this.Column18.MinimumWidth = 6;
            this.Column18.Name = "Column18";
            this.Column18.Width = 60;
            // 
            // Column19
            // 
            this.Column19.HeaderText = "Ft missed";
            this.Column19.MinimumWidth = 6;
            this.Column19.Name = "Column19";
            this.Column19.Width = 60;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Free Throws";
            this.Column4.MinimumWidth = 6;
            this.Column4.Name = "Column4";
            this.Column4.Width = 65;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Off. Rebounds";
            this.Column5.MinimumWidth = 6;
            this.Column5.Name = "Column5";
            this.Column5.Width = 70;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Def. Rebounds";
            this.Column6.MinimumWidth = 6;
            this.Column6.Name = "Column6";
            this.Column6.Width = 70;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Assists";
            this.Column7.MinimumWidth = 6;
            this.Column7.Name = "Column7";
            this.Column7.Width = 55;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Steals";
            this.Column8.MinimumWidth = 6;
            this.Column8.Name = "Column8";
            this.Column8.Width = 50;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Blocks";
            this.Column9.MinimumWidth = 6;
            this.Column9.Name = "Column9";
            this.Column9.Width = 50;
            // 
            // Column26
            // 
            this.Column26.HeaderText = "Shots Rejected";
            this.Column26.MinimumWidth = 6;
            this.Column26.Name = "Column26";
            this.Column26.Width = 65;
            // 
            // Column24
            // 
            this.Column24.HeaderText = "Fouls Drawn";
            this.Column24.MinimumWidth = 6;
            this.Column24.Name = "Column24";
            this.Column24.Width = 55;
            // 
            // Column25
            // 
            this.Column25.HeaderText = "Fouls Committed";
            this.Column25.MinimumWidth = 6;
            this.Column25.Name = "Column25";
            this.Column25.Width = 70;
            // 
            // Column13
            // 
            this.Column13.HeaderText = "Turnover";
            this.Column13.MinimumWidth = 6;
            this.Column13.Name = "Column13";
            this.Column13.Width = 65;
            // 
            // Column11
            // 
            this.Column11.HeaderText = "Points";
            this.Column11.MinimumWidth = 6;
            this.Column11.Name = "Column11";
            this.Column11.Width = 50;
            // 
            // Column12
            // 
            this.Column12.HeaderText = "Rebounds";
            this.Column12.MinimumWidth = 6;
            this.Column12.Name = "Column12";
            this.Column12.Width = 70;
            // 
            // Column20
            // 
            this.Column20.HeaderText = "PIR";
            this.Column20.MinimumWidth = 6;
            this.Column20.Name = "Column20";
            this.Column20.Width = 50;
            // 
            // Column10
            // 
            this.Column10.HeaderText = "AST/TO";
            this.Column10.MinimumWidth = 6;
            this.Column10.Name = "Column10";
            this.Column10.Width = 60;
            // 
            // ColumnPlayingTime
            // 
            this.ColumnPlayingTime.HeaderText = "Time";
            this.ColumnPlayingTime.MinimumWidth = 6;
            this.ColumnPlayingTime.Name = "ColumnPlayingTime";
            this.ColumnPlayingTime.Width = 40;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(575, 57);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(68, 22);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "Home";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(765, 57);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(68, 22);
            this.textBox2.TabIndex = 4;
            this.textBox2.Text = "Away";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(578, 122);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(65, 20);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "Foul 1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(578, 148);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(65, 20);
            this.checkBox2.TabIndex = 6;
            this.checkBox2.Text = "Foul 2";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(578, 174);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(65, 20);
            this.checkBox3.TabIndex = 7;
            this.checkBox3.Text = "Foul 3";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(578, 200);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(65, 20);
            this.checkBox4.TabIndex = 8;
            this.checkBox4.Text = "Foul 4";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point(578, 226);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(65, 20);
            this.checkBox5.TabIndex = 9;
            this.checkBox5.Text = "Foul 5";
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Location = new System.Drawing.Point(765, 226);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(65, 20);
            this.checkBox6.TabIndex = 14;
            this.checkBox6.Text = "Foul 5";
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // checkBox7
            // 
            this.checkBox7.AutoSize = true;
            this.checkBox7.Location = new System.Drawing.Point(765, 200);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(65, 20);
            this.checkBox7.TabIndex = 13;
            this.checkBox7.Text = "Foul 4";
            this.checkBox7.UseVisualStyleBackColor = true;
            this.checkBox7.CheckedChanged += new System.EventHandler(this.checkBox7_CheckedChanged);
            // 
            // checkBox8
            // 
            this.checkBox8.AutoSize = true;
            this.checkBox8.Location = new System.Drawing.Point(765, 174);
            this.checkBox8.Name = "checkBox8";
            this.checkBox8.Size = new System.Drawing.Size(65, 20);
            this.checkBox8.TabIndex = 12;
            this.checkBox8.Text = "Foul 3";
            this.checkBox8.UseVisualStyleBackColor = true;
            // 
            // checkBox9
            // 
            this.checkBox9.AutoSize = true;
            this.checkBox9.Location = new System.Drawing.Point(765, 148);
            this.checkBox9.Name = "checkBox9";
            this.checkBox9.Size = new System.Drawing.Size(65, 20);
            this.checkBox9.TabIndex = 11;
            this.checkBox9.Text = "Foul 2";
            this.checkBox9.UseVisualStyleBackColor = true;
            // 
            // checkBox10
            // 
            this.checkBox10.AutoSize = true;
            this.checkBox10.Location = new System.Drawing.Point(765, 122);
            this.checkBox10.Name = "checkBox10";
            this.checkBox10.Size = new System.Drawing.Size(65, 20);
            this.checkBox10.TabIndex = 10;
            this.checkBox10.Text = "Foul 1";
            this.checkBox10.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Comic Sans MS", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.btnSave.Location = new System.Drawing.Point(669, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 45);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // Summary
            // 
            this.Summary.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.Summary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Summary.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column21,
            this.Column22,
            this.Column23});
            this.Summary.Location = new System.Drawing.Point(52, 31);
            this.Summary.Name = "Summary";
            this.Summary.RowHeadersWidth = 51;
            this.Summary.RowTemplate.Height = 24;
            this.Summary.Size = new System.Drawing.Size(247, 189);
            this.Summary.TabIndex = 16;
            this.Summary.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Summary_CellContentClick);
            // 
            // Column21
            // 
            this.Column21.HeaderText = "Position";
            this.Column21.MinimumWidth = 6;
            this.Column21.Name = "Column21";
            this.Column21.Width = 60;
            // 
            // Column22
            // 
            this.Column22.HeaderText = "Total Score";
            this.Column22.MinimumWidth = 6;
            this.Column22.Name = "Column22";
            this.Column22.ReadOnly = true;
            this.Column22.Width = 60;
            // 
            // Column23
            // 
            this.Column23.HeaderText = "Avg Score";
            this.Column23.MinimumWidth = 6;
            this.Column23.Name = "Column23";
            this.Column23.ReadOnly = true;
            this.Column23.Width = 70;
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("Stencil", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(97, 2);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(175, 23);
            this.textBox3.TabIndex = 17;
            this.textBox3.Text = "Points from Position";
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblGameClock
            // 
            this.lblGameClock.AutoSize = true;
            this.lblGameClock.Font = new System.Drawing.Font("Comic Sans MS", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.lblGameClock.Location = new System.Drawing.Point(76, 62);
            this.lblGameClock.Name = "lblGameClock";
            this.lblGameClock.Size = new System.Drawing.Size(114, 47);
            this.lblGameClock.TabIndex = 18;
            this.lblGameClock.Text = "00:00";
            // 
            // btnStartTimer
            // 
            this.btnStartTimer.Font = new System.Drawing.Font("Comic Sans MS", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.btnStartTimer.Location = new System.Drawing.Point(2, 117);
            this.btnStartTimer.Name = "btnStartTimer";
            this.btnStartTimer.Size = new System.Drawing.Size(75, 23);
            this.btnStartTimer.TabIndex = 19;
            this.btnStartTimer.Text = "Play";
            this.btnStartTimer.UseVisualStyleBackColor = true;
            this.btnStartTimer.Click += new System.EventHandler(this.btnStartTimer_Click_1);
            // 
            // btnPauseTimer
            // 
            this.btnPauseTimer.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.btnPauseTimer.Location = new System.Drawing.Point(83, 117);
            this.btnPauseTimer.Name = "btnPauseTimer";
            this.btnPauseTimer.Size = new System.Drawing.Size(75, 23);
            this.btnPauseTimer.TabIndex = 20;
            this.btnPauseTimer.Text = "Pause";
            this.btnPauseTimer.UseVisualStyleBackColor = true;
            // 
            // btnResetTimer
            // 
            this.btnResetTimer.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.btnResetTimer.Location = new System.Drawing.Point(164, 117);
            this.btnResetTimer.Name = "btnResetTimer";
            this.btnResetTimer.Size = new System.Drawing.Size(75, 23);
            this.btnResetTimer.TabIndex = 21;
            this.btnResetTimer.Text = "Stop";
            this.btnResetTimer.UseVisualStyleBackColor = true;
            // 
            // cmbPlayersOut
            // 
            this.cmbPlayersOut.FormattingEnabled = true;
            this.cmbPlayersOut.Location = new System.Drawing.Point(95, 48);
            this.cmbPlayersOut.Name = "cmbPlayersOut";
            this.cmbPlayersOut.Size = new System.Drawing.Size(121, 24);
            this.cmbPlayersOut.TabIndex = 23;
            // 
            // cmbPlayersIn
            // 
            this.cmbPlayersIn.FormattingEnabled = true;
            this.cmbPlayersIn.Location = new System.Drawing.Point(95, 73);
            this.cmbPlayersIn.Name = "cmbPlayersIn";
            this.cmbPlayersIn.Size = new System.Drawing.Size(121, 24);
            this.cmbPlayersIn.TabIndex = 24;
            // 
            // btnSubstitute
            // 
            this.btnSubstitute.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.btnSubstitute.Location = new System.Drawing.Point(48, 102);
            this.btnSubstitute.Name = "btnSubstitute";
            this.btnSubstitute.Size = new System.Drawing.Size(121, 23);
            this.btnSubstitute.TabIndex = 25;
            this.btnSubstitute.Text = "Subtitution";
            this.btnSubstitute.UseVisualStyleBackColor = true;
            // 
            // textBox5
            // 
            this.textBox5.Font = new System.Drawing.Font("Stencil", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox5.Location = new System.Drawing.Point(0, 49);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(89, 23);
            this.textBox5.TabIndex = 27;
            this.textBox5.Text = "Player out";
            this.textBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox6
            // 
            this.textBox6.Font = new System.Drawing.Font("Stencil", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox6.Location = new System.Drawing.Point(0, 74);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(89, 23);
            this.textBox6.TabIndex = 28;
            this.textBox6.Text = "Player IN";
            this.textBox6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblTimerStatus
            // 
            this.lblTimerStatus.AutoSize = true;
            this.lblTimerStatus.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.lblTimerStatus.Location = new System.Drawing.Point(51, 35);
            this.lblTimerStatus.Name = "lblTimerStatus";
            this.lblTimerStatus.Size = new System.Drawing.Size(172, 35);
            this.lblTimerStatus.TabIndex = 29;
            this.lblTimerStatus.Text = "GAME CLOCK";
            // 
            // groupBoxActivePlayers
            // 
            this.groupBoxActivePlayers.Location = new System.Drawing.Point(0, 0);
            this.groupBoxActivePlayers.Name = "groupBoxActivePlayers";
            this.groupBoxActivePlayers.Size = new System.Drawing.Size(100, 23);
            this.groupBoxActivePlayers.TabIndex = 33;
            // 
            // lblSubstitutions
            // 
            this.lblSubstitutions.AutoSize = true;
            this.lblSubstitutions.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.lblSubstitutions.Location = new System.Drawing.Point(43, 16);
            this.lblSubstitutions.Name = "lblSubstitutions";
            this.lblSubstitutions.Size = new System.Drawing.Size(167, 35);
            this.lblSubstitutions.TabIndex = 31;
            this.lblSubstitutions.Text = "Substitutions";
            // 
            // lstActivePlayers
            // 
            this.lstActivePlayers.FormattingEnabled = true;
            this.lstActivePlayers.ItemHeight = 16;
            this.lstActivePlayers.Items.AddRange(new object[] {
            "Player "});
            this.lstActivePlayers.Location = new System.Drawing.Point(17, 35);
            this.lstActivePlayers.Name = "lstActivePlayers";
            this.lstActivePlayers.Size = new System.Drawing.Size(154, 100);
            this.lstActivePlayers.TabIndex = 32;
            // 
            // groupBoxTimer
            // 
            this.groupBoxTimer.Controls.Add(this.lblTimerStatus);
            this.groupBoxTimer.Controls.Add(this.btnResetTimer);
            this.groupBoxTimer.Controls.Add(this.btnPauseTimer);
            this.groupBoxTimer.Controls.Add(this.btnStartTimer);
            this.groupBoxTimer.Controls.Add(this.lblGameClock);
            this.groupBoxTimer.Location = new System.Drawing.Point(318, 31);
            this.groupBoxTimer.Name = "groupBoxTimer";
            this.groupBoxTimer.Size = new System.Drawing.Size(257, 211);
            this.groupBoxTimer.TabIndex = 33;
            this.groupBoxTimer.TabStop = false;
            // 
            // groupActiveplr
            // 
            this.groupActiveplr.Controls.Add(this.label1);
            this.groupActiveplr.Controls.Add(this.lstActivePlayers);
            this.groupActiveplr.Controls.Add(this.groupBoxActivePlayers);
            this.groupActiveplr.Location = new System.Drawing.Point(968, 9);
            this.groupActiveplr.Name = "groupActiveplr";
            this.groupActiveplr.Size = new System.Drawing.Size(184, 162);
            this.groupActiveplr.TabIndex = 35;
            this.groupActiveplr.TabStop = false;
            this.groupActiveplr.Text = "groupBox1";
            // 
            // groupBoxSubstitutions
            // 
            this.groupBoxSubstitutions.Controls.Add(this.lblSubstitutions);
            this.groupBoxSubstitutions.Controls.Add(this.textBox5);
            this.groupBoxSubstitutions.Controls.Add(this.cmbPlayersOut);
            this.groupBoxSubstitutions.Controls.Add(this.btnSubstitute);
            this.groupBoxSubstitutions.Controls.Add(this.textBox6);
            this.groupBoxSubstitutions.Controls.Add(this.cmbPlayersIn);
            this.groupBoxSubstitutions.Location = new System.Drawing.Point(1225, 22);
            this.groupBoxSubstitutions.Name = "groupBoxSubstitutions";
            this.groupBoxSubstitutions.Size = new System.Drawing.Size(222, 149);
            this.groupBoxSubstitutions.TabIndex = 36;
            this.groupBoxSubstitutions.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label1.Location = new System.Drawing.Point(33, -3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 35);
            this.label1.TabIndex = 34;
            this.label1.Text = "First Five";
            // 
            // basketicsBindingSource
            // 
            this.basketicsBindingSource.DataSource = typeof(Basktics_v2._0.Basketics);
            // 
            // MatchForm
            // 
            this.ClientSize = new System.Drawing.Size(1540, 638);
            this.Controls.Add(this.groupBoxSubstitutions);
            this.Controls.Add(this.groupActiveplr);
            this.Controls.Add(this.groupBoxTimer);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.Summary);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.checkBox6);
            this.Controls.Add(this.checkBox7);
            this.Controls.Add(this.checkBox8);
            this.Controls.Add(this.checkBox9);
            this.Controls.Add(this.checkBox10);
            this.Controls.Add(this.checkBox5);
            this.Controls.Add(this.checkBox4);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "MatchForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MatchForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Summary)).EndInit();
            this.groupBoxTimer.ResumeLayout(false);
            this.groupBoxTimer.PerformLayout();
            this.groupActiveplr.ResumeLayout(false);
            this.groupActiveplr.PerformLayout();
            this.groupBoxSubstitutions.ResumeLayout(false);
            this.groupBoxSubstitutions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.basketicsBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown2;

        private void label1_Click(object sender, EventArgs e)
        {

        }
        //private IContainer components;

        /* private void MatchForm_Load(object sender, EventArgs e)
         {

                 _ = DataGridViewTextBoxColumn.Rows.Add();
         }*/

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private DataGridView dataGridView1;
        private TextBox textBox1;
        private TextBox textBox2;
        // private DataGridViewTextBoxColumn Column13;

        private void MatchForm_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 16; i++)
            {
                dataGridView1.Rows.Add();
            }

            // Προσθήκη event για αυτόματη ενημέρωση των active players όταν αλλάζουν τα ονόματα
            this.dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(dataGridView1_CellValueChanged_ForPlayers);

            RefreshPlayersComboBox();
        }
        private void dataGridView1_CellValueChanged_ForPlayers(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0) // Column1 είναι η πρώτη στήλη (Player)
            {
                SetStartingFiveAutomatically();
            }
        }


        private void MatchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                saveFileDialog.Title = "Save Match Data to Excel File";
                saveFileDialog.FileName = "MatchData.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.SaveDataToExcel(saveFileDialog.FileName);
                }
            }
        }

        private void SaveDataToExcel(string filepath)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Players");
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    sheet.Cells[1, i + 1].Value = dataGridView1.Columns[i].HeaderText;

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                        sheet.Cells[i + 2, j + 1].Value = dataGridView1.Rows[i].Cells[j].Value;

                ExcelWorksheet summarySheet = package.Workbook.Worksheets.Add("Summary");
                summarySheet.Cells[1, 1].Value = "Position";
                summarySheet.Cells[1, 2].Value = "Total Points";
                summarySheet.Cells[1, 3].Value = "Average Points";

                for (int i = 0; i < Summary.Rows.Count; i++)
                {
                    summarySheet.Cells[i + 2, 1].Value = Summary.Rows[i].Cells[0].Value;
                    summarySheet.Cells[i + 2, 2].Value = Summary.Rows[i].Cells[1].Value;
                    summarySheet.Cells[i + 2, 3].Value = Summary.Rows[i].Cells[2].Value;
                }

                FileInfo excelFile = new FileInfo(filepath);
                package.SaveAs(excelFile);
            }
        }

        private static object GetDebuggerDisplay()
        {
            throw new NotImplementedException();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == this.Column2.Index || e.ColumnIndex == this.Column3.Index || e.ColumnIndex == this.Column4.Index || e.ColumnIndex == this.Column16.Index)
                {
                    DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                    int twoPoint = Convert.ToInt32(row.Cells["Column2"].Value ?? 0);
                    int twoPointb = Convert.ToInt32(row.Cells["Column16"].Value ?? 0);
                    int threePoint = Convert.ToInt32(row.Cells["Column3"].Value ?? 0);
                    int freeThrows = Convert.ToInt32(row.Cells["Column4"].Value ?? 0);
                    int sum = 2 * twoPoint + 2 * twoPointb + 3 * threePoint + freeThrows;
                    row.Cells["Column11"].Value = sum;
                }

                if (e.ColumnIndex == this.Column5.Index || e.ColumnIndex == this.Column6.Index)
                {
                    DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                    int offence = Convert.ToInt32(row.Cells["Column5"].Value ?? 0);
                    int defence = Convert.ToInt32(row.Cells["Column6"].Value ?? 0);
                    int sum = offence + defence;
                    row.Cells["Column12"].Value = sum;
                }

                if (e.ColumnIndex == this.Column11.Index || e.ColumnIndex == this.Column12.Index || e.ColumnIndex == this.Column7.Index || e.ColumnIndex == this.Column8.Index || e.ColumnIndex == this.Column9.Index || e.ColumnIndex == this.Column24.Index || e.ColumnIndex == this.Column15.Index || e.ColumnIndex == this.Column17.Index || e.ColumnIndex == this.Column18.Index || e.ColumnIndex == this.Column19.Index || e.ColumnIndex == this.Column13.Index || e.ColumnIndex == this.Column25.Index || e.ColumnIndex == this.Column26.Index || e.ColumnIndex == this.Column20.Index)
                {
                    DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                    int points = Convert.ToInt32(row.Cells["Column11"].Value ?? 0);
                    int rebounds = Convert.ToInt32(row.Cells["Column12"].Value ?? 0);
                    int assists = Convert.ToInt32(row.Cells["Column7"].Value ?? 0);
                    int steals = Convert.ToInt32(row.Cells["Column8"].Value ?? 0);
                    int blocks = Convert.ToInt32(row.Cells["Column9"].Value ?? 0);
                    int foulsdrawn = Convert.ToInt32(row.Cells["Column24"].Value ?? 0);
                    int missedlayups = Convert.ToInt32(row.Cells["Column15"].Value ?? 0);
                    int missedmidrange = Convert.ToInt32(row.Cells["Column17"].Value ?? 0);
                    int threepointmissed = Convert.ToInt32(row.Cells["Column18"].Value ?? 0);
                    int ftmissed = Convert.ToInt32(row.Cells["Column19"].Value ?? 0);
                    int turnovers = Convert.ToInt32(row.Cells["Column13"].Value ?? 0);
                    int shotsrejected = Convert.ToInt32(row.Cells["Column26"].Value ?? 0);
                    int foulscommited = Convert.ToInt32(row.Cells["Column25"].Value ?? 0);
                    int pir = points + rebounds + assists + steals + blocks + foulsdrawn - missedlayups - missedmidrange - threepointmissed - ftmissed - turnovers - shotsrejected - foulscommited;
                    row.Cells["Column20"].Value = pir;
                }

                if (e.ColumnIndex == this.Column10.Index || e.ColumnIndex == this.Column13.Index || e.ColumnIndex == this.Column7.Index)
                {
                    DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                    double assists = Convert.ToDouble(row.Cells["Column7"].Value ?? 0);
                    double turnover = Convert.ToDouble(row.Cells["Column13"].Value ?? 0);
                    double astto = turnover == 0 ? assists : assists / turnover;
                    row.Cells["Column10"].Value = Math.Round(astto, 2);
                }
            }
            CalculatePointsForPositionSummary();
        }

        private void CalculatePointsForPositionSummary()
        {
            Summary.Rows.Clear();
            var groups = dataGridView1.Rows
                .Cast<DataGridViewRow>()
                .Where(r => !r.IsNewRow && r.Cells["Column14"]?.Value != null)
                .GroupBy(r => r.Cells["Column14"].Value.ToString());

            foreach (var g in groups)
            {
                double totalPoints = g.Sum(r => Convert.ToDouble(r.Cells["Column11"].Value ?? 0));
                double avgPoints = g.Average(r => Convert.ToDouble(r.Cells["Column11"].Value ?? 0));
                Summary.Rows.Add(g.Key, Math.Round(totalPoints, 2), Math.Round(avgPoints, 2));
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {

            //SaveDataToExcel();
            MessageBox.Show("Data successfully saved to Excel!", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                saveFileDialog.Title = "Save file Excel";
                saveFileDialog.FileName = "MatchStats.xlsx"; // προτεινόμενο όνομα

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SaveDataToExcel(saveFileDialog.FileName);
                    MessageBox.Show("The file saved successfull", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }



        // "Required to register the change when the user finishes editing a cell."
        private void dataGridView1_EditingControlShowing1(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column_KeyPress);
            if (this.dataGridView1.CurrentCell.ColumnIndex == this.Column2.Index ||
                this.dataGridView1.CurrentCell.ColumnIndex == this.Column3.Index ||
                this.dataGridView1.CurrentCell.ColumnIndex == this.Column4.Index ||
                this.dataGridView1.CurrentCell.ColumnIndex == this.Column5.Index ||
                this.dataGridView1.CurrentCell.ColumnIndex == this.Column6.Index) // for columns 3 , 4 , 5 ,6 
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Column_KeyPress);
                }
            }
        }

        private void Column_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }



        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private CheckBox checkBox3;
        private CheckBox checkBox4;
        private CheckBox checkBox5;
        private CheckBox checkBox6;
        private CheckBox checkBox7;
        private CheckBox checkBox8;
        private CheckBox checkBox9;
        private CheckBox checkBox10;

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {

        }
       
        private List<string> allPlayers = new List<string>();
        private BindingList<string> playersInGame = new BindingList<string>();
        private BindingList<string> playersOnBench = new BindingList<string>();

        private void InitializePlayers()
        {
            // Όλοι οι παίκτες - απευθείας strings
            allPlayers = new List<string>
    {
        "Παίκτης 1", "Παίκτης 2", "Παίκτης 3", "Παίκτης 4", "Παίκτης 5",
        "Παίκτης 6", "Παίκτης 7", "Παίκτης 8", "Παίκτης 9", "Παίκτης 10"
    };

            // Αρχικά, όλοι στον πάγκο
            playersOnBench.Clear();
            foreach (var player in allPlayers)
            {
                playersOnBench.Add(player);
            }

            playersInGame.Clear(); // Αρχικά κανένας στο γήπεδο
        }
        private void SetupSubstitutionColumns()
        {
            // Column OUT - παίκτες μέσα
            DataGridViewComboBoxColumn colOut = new DataGridViewComboBoxColumn();
            colOut.Name = "ColumnOut";
            colOut.HeaderText = "Εξερχόμενος";
            colOut.DataSource = new BindingList<string>(playersInGame.ToList());
            // ΔΕΝ χρειάζεται DisplayMember ή ValueMember για strings!

            // Column IN - παίκτες έξω
            DataGridViewComboBoxColumn colIn = new DataGridViewComboBoxColumn();
            colIn.Name = "ColumnIn";
            colIn.HeaderText = "Εισερχόμενος";
            colIn.DataSource = new BindingList<string>(playersOnBench.ToList());
            // ΔΕΝ χρειάζεται DisplayMember ή ValueMember για strings!

            // Προσθήκη columns
            if (!dataGridView1.Columns.Contains("ColumnOut"))
                dataGridView1.Columns.Add(colOut);

            if (!dataGridView1.Columns.Contains("ColumnIn"))
                dataGridView1.Columns.Add(colIn);
        }
        private void dataGridView1_CellValueChangeds(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == dataGridView1.Columns["ColumnOut"].Index ||
                e.ColumnIndex == dataGridView1.Columns["ColumnIn"].Index)
            {
                ProcessSubstitution(e.RowIndex);
                RefreshAllDropdowns();
            }
        }

        private void ProcessSubstitution(int rowIndex)
        {
            DataGridViewRow row = dataGridView1.Rows[rowIndex];

            // Απευθείας strings - όχι Player objects
            string playerOut = row.Cells["ColumnOut"].Value as string;
            string playerIn = row.Cells["ColumnIn"].Value as string;

            if (!string.IsNullOrEmpty(playerOut) && !string.IsNullOrEmpty(playerIn))
            {
                // Μετακίνηση παικτών
                playersInGame.Remove(playerOut);
                playersOnBench.Remove(playerIn);

                playersOnBench.Add(playerOut);
                playersInGame.Add(playerIn);

                // Κάθαρισμα επιλογών
                row.Cells["ColumnOut"].Value = null;
                row.Cells["ColumnIn"].Value = null;

                MessageBox.Show($"Αλλαγή: {playerOut} OUT → {playerIn} IN");
            }
        }

      
      
        private void SetStartingPlayers(List<string> startingPlayers)
        {
            playersInGame.Clear();
            playersOnBench.Clear();

            foreach (var player in allPlayers)
            {
                if (startingPlayers.Contains(player))
                    playersInGame.Add(player);
                else
                    playersOnBench.Add(player);
            }

            RefreshAllDropdowns();
        }

        // Παράδειγμα χρήσης:
        private void btnStartGame_Click(object sender, EventArgs e)
        {
            SetStartingPlayers(new List<string> { "Παίκτης 1", "Παίκτης 2", "Παίκτης 3", "Παίκτης 4", "Παίκτης 5" });
        }

        private void RefreshAllDropdowns()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    DataGridViewComboBoxCell cellOut = (DataGridViewComboBoxCell)row.Cells["ColumnOut"];
                    DataGridViewComboBoxCell cellIn = (DataGridViewComboBoxCell)row.Cells["ColumnIn"];

                    cellOut.DataSource = new BindingList<string>(playersInGame.ToList());
                    cellIn.DataSource = new BindingList<string>(playersOnBench.ToList());
                }
            }
        }



        private void UpdatePlayerLists()
        {
            playersInGame.Clear();
            playersOnBench.Clear();

            // Αρχικά, όλοι στον πάγκο
            foreach (var player in allPlayers)
            {
                playersOnBench.Add(player);
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private Button btnSave;
        private DataGridView Summary;
        private BindingSource basketicsBindingSource;
        private IContainer components;
        private DataGridViewTextBoxColumn Column21;
        private DataGridViewTextBoxColumn Column22;
        private DataGridViewTextBoxColumn Column23;
        private TextBox textBox3;

        private void Summary_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private Label lblGameClock;
        private Button btnStartTimer;
        private Button btnPauseTimer;
        private Button btnResetTimer;
        private ComboBox cmbPlayersOut;
        private ComboBox cmbPlayersIn;
        private Button btnSubstitute;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column14;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column15;
        private DataGridViewTextBoxColumn Column16;
        private DataGridViewTextBoxColumn Column17;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column18;
        private DataGridViewTextBoxColumn Column19;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn Column6;
        private DataGridViewTextBoxColumn Column7;
        private DataGridViewTextBoxColumn Column8;
        private DataGridViewTextBoxColumn Column9;
        private DataGridViewTextBoxColumn Column26;
        private DataGridViewTextBoxColumn Column24;
        private DataGridViewTextBoxColumn Column25;
        private DataGridViewTextBoxColumn Column13;
        private DataGridViewTextBoxColumn Column11;
        private DataGridViewTextBoxColumn Column12;
        private DataGridViewTextBoxColumn Column20;
        private DataGridViewTextBoxColumn Column10;
        private DataGridViewTextBoxColumn ColumnPlayingTime;
        private TextBox textBox5;
        private TextBox textBox6;
        private Label lblTimerStatus;
        private Label groupBoxActivePlayers;
        private Label lblSubstitutions;
        private ListBox lstActivePlayers;
        private GroupBox groupBoxTimer;
        private GroupBox groupActiveplr;
        private GroupBox groupBoxSubstitutions;
        private bool isStartingFiveSet = false; // Προσθήκη μεταβλητής παρακολούθησης
        private void btnSave_Click_1(object sender, EventArgs e)
        {

        }

        private void btnStartTimer_Click_1(object sender, EventArgs e)
        {

        }

        private Label label1;
    }
}