using System;
using System.Diagnostics;
using System.Windows.Forms;
using OfficeOpenXml;
using System.IO;

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
            this.FormClosing += new FormClosingEventHandler(MatchForm_FormClosing);


        }
        private void MatchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Εμφανίζει το παράθυρο διαλόγου αποθήκευσης αρχείου
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                saveFileDialog.Title = "Save Match Data to Excel File";
                saveFileDialog.FileName = "MatchData.xlsx";

                // Αν ο χρήστης πατήσει OK, αποθηκεύει το αρχείο Excel
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SaveDataToExcel(saveFileDialog.FileName);
                }
            }
        }

        private void SaveDataToExcel(string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Match Data");

                // Προσθήκη επικεφαλίδων από το DataGridView
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = dataGridView1.Columns[i].HeaderText;
                }

                // Προσθήκη δεδομένων από το DataGridView
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1].Value = dataGridView1.Rows[i].Cells[j].Value;
                    }
                }

                // Αποθήκευση του αρχείου Excel
                FileInfo excelFile = new FileInfo(filePath);
                package.SaveAs(excelFile);
            }

            MessageBox.Show("Τα δεδομένα αποθηκεύτηκαν με επιτυχία στο Excel!", "Αποθήκευση", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void InitializeComponent()
        {
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(594, 93);
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
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8,
            this.Column9,
            this.Column10});
            this.dataGridView1.Location = new System.Drawing.Point(333, 260);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(815, 264);
            this.dataGridView1.TabIndex = 2;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Player";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.Width = 75;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "2-point";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.Width = 75;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "3-point";
            this.Column3.MinimumWidth = 6;
            this.Column3.Name = "Column3";
            this.Column3.Width = 75;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Free Throws";
            this.Column4.MinimumWidth = 6;
            this.Column4.Name = "Column4";
            this.Column4.Width = 75;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Rebounds";
            this.Column5.MinimumWidth = 6;
            this.Column5.Name = "Column5";
            this.Column5.Width = 75;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Assists";
            this.Column6.MinimumWidth = 6;
            this.Column6.Name = "Column6";
            this.Column6.Width = 75;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Steals";
            this.Column7.MinimumWidth = 6;
            this.Column7.Name = "Column7";
            this.Column7.Width = 75;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Blocks";
            this.Column8.MinimumWidth = 6;
            this.Column8.Name = "Column8";
            this.Column8.Width = 75;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Fouls";
            this.Column9.MinimumWidth = 6;
            this.Column9.Name = "Column9";
            this.Column9.Width = 75;
            // 
            // Column10
            // 
            this.Column10.HeaderText = "Points";
            this.Column10.MinimumWidth = 6;
            this.Column10.Name = "Column10";
            this.Column10.Width = 75;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(595, 57);
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
            // MatchForm
            // 
            this.ClientSize = new System.Drawing.Size(1635, 625);
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
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn Column6;
        private DataGridViewTextBoxColumn Column7;
        private DataGridViewTextBoxColumn Column8;
        private DataGridViewTextBoxColumn Column9;
        private DataGridViewTextBoxColumn Column10;

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
                // Έλεγχος αν η αλλαγή έγινε σε μια από τις στήλες 2, 3 ή 4
                if (e.ColumnIndex == this.Column2.Index || e.ColumnIndex == this.Column3.Index || e.ColumnIndex == this.Column4.Index)
                {
                    // Πάρε την τρέχουσα γραμμή
                    DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                    // Υπολόγισε το άθροισμα των στηλών 2, 3 και 4
                    int twoPoint = Convert.ToInt32(row.Cells["Column2"].Value ?? 0);
                    int threePoint = Convert.ToInt32(row.Cells["Column3"].Value ?? 0);
                    int freeThrows = Convert.ToInt32(row.Cells["Column4"].Value ?? 0);

                    int sum = 2 * twoPoint + 3 * threePoint + freeThrows;

                    // Αποθήκευσε το άθροισμα στη στήλη 10 (Points)
                    row.Cells["Column10"].Value = sum;
                }
            }
        }





        // Χρειάζεται για να καταχωρηθεί η αλλαγή όταν ο χρήστης τελειώνει την επεξεργασία ενός κελιού
        private void dataGridView1_EditingControlShowing1(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column_KeyPress);
            if (this.dataGridView1.CurrentCell.ColumnIndex == this.Column2.Index ||
                this.dataGridView1.CurrentCell.ColumnIndex == this.Column3.Index ||
                this.dataGridView1.CurrentCell.ColumnIndex == this.Column4.Index) // Μόνο για τις στήλες 2, 3 και 4
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
            // Επιτρέπονται μόνο αριθμοί και το πλήκτρο Backspace
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
    }
}