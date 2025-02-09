using System;
using System.Windows.Forms;

namespace Basktics_v2._0
{
    public partial class Basketics : Form
    {
        public Basketics()
        {
            InitializeComponent();
        }

        private void matchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MatchForm matchForm = new MatchForm();
            matchForm.Show();
        }

        private void playersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlayerForm playerForm = new PlayerForm();
            playerForm.Show();
        }
        private void Basketics_Load(object sender, EventArgs e)
        {
            // Any initialization code can go here
        }


    }
}
