using DocumentFormat.OpenXml.Spreadsheet;
using OfficeOpenXml;
using System;
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

            this.dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);
            this.dataGridView1.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dataGridView1_EditingControlShowing1);
           // this.FormClosing += new FormClosingEventHandler(MatchForm_FormClosing);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            this.dataGridView1.CellValueChanged += (s, e) =>
            {
                if (e.RowIndex >= 0 &&
                   (dataGridView1.Columns[e.ColumnIndex].Name == "Column11" || // Points
                    dataGridView1.Columns[e.ColumnIndex].Name == "Column14"))  // Position
                {
                    CalculatePointsForPositionSummary();
                }
            };

            this.dataGridView1.RowsAdded += (s, e) => CalculatePointsForPositionSummary();
            this.dataGridView1.RowsRemoved += (s, e) => CalculatePointsForPositionSummary();


        }
        private void MatchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Displays the file save dialog.
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                saveFileDialog.Title = "Save Match Data to Excel File";
                saveFileDialog.FileName = "MatchData.xlsx";

                // If the user clicks OK, it saves the Excel file.
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.SaveDataToExcel(saveFileDialog.FileName);
                }
            }
        }

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
            this.basketicsBindingSource = new System.Windows.Forms.BindingSource(this.components);
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
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Summary)).BeginInit();
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
            this.Column10});
            this.dataGridView1.Location = new System.Drawing.Point(52, 261);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1446, 353);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
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
            this.btnSave.Location = new System.Drawing.Point(669, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 45);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
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
            // basketicsBindingSource
            // 
            this.basketicsBindingSource.DataSource = typeof(Basktics_v2._0.Basketics);
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
            // MatchForm
            // 
            this.ClientSize = new System.Drawing.Size(1812, 638);
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

        private void MatchForm_Load1(object sender, EventArgs e)
        {
            for (int i = 0; i < 16; i++)
            {
                dataGridView1.Rows.Add();
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
                // ""Checks if the change was made in one of the columns 2, 3, or 4.""
                if (e.ColumnIndex == this.Column2.Index || e.ColumnIndex == this.Column3.Index || e.ColumnIndex == this.Column4.Index || e.ColumnIndex == this.Column16.Index)
                {
                    // "Get the current row."
                    DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                    // "Calculate the sum of columns 2, 3, and 4."
                    int twoPoint = Convert.ToInt32(row.Cells["Column2"].Value ?? 0);
                    int twoPointb = Convert.ToInt32(row.Cells["Column16"].Value ?? 0);
                    int threePoint = Convert.ToInt32(row.Cells["Column3"].Value ?? 0);
                    int freeThrows = Convert.ToInt32(row.Cells["Column4"].Value ?? 0);

                    int sum = 2 * twoPoint + 2 * twoPointb + 3 * threePoint + freeThrows;

                    // "Store the sum in column 11 (Points)."
                    row.Cells["Column11"].Value = sum;
                }
                // ""Checks if the change was made in one of the columns 5, 6 "
                if (e.ColumnIndex == this.Column5.Index || e.ColumnIndex == this.Column6.Index)
                {
                    // "Get the current row."
                    DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                    // "Calculate the sum of columns 5, 6 "
                    int offence = Convert.ToInt32(row.Cells["Column5"].Value ?? 0);
                    int defence = Convert.ToInt32(row.Cells["Column6"].Value ?? 0);


                    int sum = offence + defence;

                    // "Store the sum in column 12 (Points)."
                    row.Cells["Column12"].Value = sum;
                }

                if(e.ColumnIndex == this.Column11.Index || e.ColumnIndex == this.Column12.Index || e.ColumnIndex == this.Column7.Index || e.ColumnIndex == this.Column8.Index || e.ColumnIndex == this.Column9.Index || e.ColumnIndex == this.Column24.Index || e.ColumnIndex == this.Column15.Index || e.ColumnIndex == this.Column17.Index || e.ColumnIndex == this.Column18.Index || e.ColumnIndex == this.Column19.Index || e.ColumnIndex == this.Column13.Index || e.ColumnIndex == this.Column25.Index || e.ColumnIndex == this.Column26.Index || e.ColumnIndex == this.Column20.Index)
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

                    // "Store the pir in column 20 (PIR)."
                    row.Cells["Column20"].Value = pir;
                }
                if (e.ColumnIndex == this.Column10.Index || e.ColumnIndex == this.Column13.Index || e.ColumnIndex == this.Column7.Index)
                {
                    DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                    double assists = Convert.ToDouble(row.Cells["Column7"].Value ?? 0);
                    double turnover = Convert.ToDouble(row.Cells["Column13"].Value ?? 0);

                    double astto;
                    if (turnover == 0)
                    {
                        astto = assists; 
                    }
                    else
                    {
                        astto = assists / turnover;
                    }
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

        private void SaveDataToExcel(string filepath)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                // --- Φύλλο με τα δεδομένα των παικτών ---
                ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Players");
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    sheet.Cells[1, i + 1].Value = dataGridView1.Columns[i].HeaderText;

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                        sheet.Cells[i + 2, j + 1].Value = dataGridView1.Rows[i].Cells[j].Value;

                // --- Φύλλο με τα summary ---
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

                // --- Αποθήκευση ---
                FileInfo excelFile = new FileInfo(filepath);
                package.SaveAs(excelFile);
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
            // "Only digits and the Backspace key are allowed."
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void MatchForm_Load(object sender, EventArgs e)
        {

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
    }
}