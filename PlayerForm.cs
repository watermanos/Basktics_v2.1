using System;
using System.Windows.Forms;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Numerics;
using System.Windows;

namespace Basktics_v2._0
{
    public partial class PlayerForm : Form
    {

        public PlayerForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Δημιουργία νέου αρχείου Excel
                var excelApp = new Excel.Application();
                excelApp.Visible = true;
                var workbooks = excelApp.Workbooks;
                var workbook = workbooks.Add();
                var worksheet = (Excel.Worksheet)workbook.Sheets[1];

                // Εγγραφή τίτλων στη πρώτη γραμμή του Excel
                worksheet.Cells[1, 1] = "Date";
                worksheet.Cells[1, 2] = "Player";
                worksheet.Cells[1, 3] = "Goal";
                worksheet.Cells[1, 4] = "Sprint";
                worksheet.Cells[1, 5] = "Vertical";
                worksheet.Cells[1, 6] = "TwoShoots";
                worksheet.Cells[1, 7] = "ThreeShoots";
                worksheet.Cells[1, 8] = "FreeThrow";
                worksheet.Cells[1, 9] = "Duration";

                // Εισαγωγή δεδομένων από τα TextBox στο Excel
                worksheet.Cells[2, 1] = Date.Text;
                worksheet.Cells[2, 2] = Player.Text;
                worksheet.Cells[2, 3] = Goal.Text;
                worksheet.Cells[2, 4] = Sprint.Text;
                worksheet.Cells[2, 5] = Vertical.Text;
                worksheet.Cells[2, 6] = TwoShoots.Text;
                worksheet.Cells[2, 7] = ThreeShoots.Text;
                worksheet.Cells[2, 8] = FreeThrows.Text;
                worksheet.Cells[2, 9] = Duration.Text;

                // Προαιρετική αποθήκευση του αρχείου Excel (σε συγκεκριμένο φάκελο ή τοπική διαδρομή)
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    workbook.SaveAs(filePath);
                }

                MessageBox.Show("Data saved successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while saving data: " + ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}
