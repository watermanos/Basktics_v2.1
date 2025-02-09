using System;

namespace Basktics_v2._0
{
    partial class PlayerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        /* protected void Dispose(bool disposing)
         {
             if (disposing && (components != null))
             {
                 components.Dispose();
             }
             base.Dispose(disposing);
         }
        */

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        ///     this.btnSave = new System.Windows.Forms.Button();
        private System.Windows.Forms.Button btnSave;
        private void InitializeComponent()
        {
            this.Date = new System.Windows.Forms.TextBox();
            this.Player = new System.Windows.Forms.TextBox();
            this.Goal = new System.Windows.Forms.TextBox();
            this.Sprint = new System.Windows.Forms.TextBox();
            this.Vertical = new System.Windows.Forms.TextBox();
            this.TwoShoots = new System.Windows.Forms.TextBox();
            this.ThreeShoots = new System.Windows.Forms.TextBox();
            this.FreeThrows = new System.Windows.Forms.TextBox();
            this.Duration = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Date
            // 
            this.Date.Location = new System.Drawing.Point(120, 20);
            this.Date.Name = "Date";
            this.Date.Size = new System.Drawing.Size(150, 22);
            this.Date.TabIndex = 0;
            // 
            // Player
            // 
            this.Player.Location = new System.Drawing.Point(120, 60);
            this.Player.Name = "Player";
            this.Player.Size = new System.Drawing.Size(150, 22);
            this.Player.TabIndex = 1;
            // 
            // Goal
            // 
            this.Goal.Location = new System.Drawing.Point(120, 100);
            this.Goal.Name = "Goal";
            this.Goal.Size = new System.Drawing.Size(150, 22);
            this.Goal.TabIndex = 2;
            // 
            // Sprint
            // 
            this.Sprint.Location = new System.Drawing.Point(120, 140);
            this.Sprint.Name = "Sprint";
            this.Sprint.Size = new System.Drawing.Size(150, 22);
            this.Sprint.TabIndex = 3;
            // 
            // Vertical
            // 
            this.Vertical.Location = new System.Drawing.Point(120, 180);
            this.Vertical.Name = "Vertical";
            this.Vertical.Size = new System.Drawing.Size(150, 22);
            this.Vertical.TabIndex = 4;
            // 
            // TwoShoots
            // 
            this.TwoShoots.Location = new System.Drawing.Point(120, 220);
            this.TwoShoots.Name = "TwoShoots";
            this.TwoShoots.Size = new System.Drawing.Size(150, 22);
            this.TwoShoots.TabIndex = 5;
            // 
            // ThreeShoots
            // 
            this.ThreeShoots.Location = new System.Drawing.Point(120, 260);
            this.ThreeShoots.Name = "ThreeShoots";
            this.ThreeShoots.Size = new System.Drawing.Size(150, 22);
            this.ThreeShoots.TabIndex = 6;
            // 
            // FreeThrows
            // 
            this.FreeThrows.Location = new System.Drawing.Point(120, 300);
            this.FreeThrows.Name = "FreeThrows";
            this.FreeThrows.Size = new System.Drawing.Size(150, 22);
            this.FreeThrows.TabIndex = 7;
            // 
            // Duration
            // 
            this.Duration.Location = new System.Drawing.Point(120, 340);
            this.Duration.Name = "Duration";
            this.Duration.Size = new System.Drawing.Size(150, 22);
            this.Duration.TabIndex = 8;
            
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(148, 368);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 45);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 16);
            this.label1.TabIndex = 9;
            this.label1.Text = "Date";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 16);
            this.label2.TabIndex = 10;
            this.label2.Text = "Player Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 16);
            this.label3.TabIndex = 11;
            this.label3.Text = "Goal";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 146);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 16);
            this.label4.TabIndex = 12;
            this.label4.Text = "Sprint";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 186);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 16);
            this.label5.TabIndex = 13;
            this.label5.Text = "Vertical";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(25, 220);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 16);
            this.label6.TabIndex = 14;
            this.label6.Text = "2p Shoots";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(25, 260);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 16);
            this.label7.TabIndex = 15;
            this.label7.Text = "3p Shoots";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(25, 300);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 16);
            this.label8.TabIndex = 16;
            this.label8.Text = "Free Throwing";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(25, 346);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(57, 16);
            this.label9.TabIndex = 17;
            this.label9.Text = "Duration";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // PlayerForm
            // 
            this.ClientSize = new System.Drawing.Size(386, 425);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Duration);
            this.Controls.Add(this.FreeThrows);
            this.Controls.Add(this.ThreeShoots);
            this.Controls.Add(this.TwoShoots);
            this.Controls.Add(this.Vertical);
            this.Controls.Add(this.Sprint);
            this.Controls.Add(this.Goal);
            this.Controls.Add(this.Player);
            this.Controls.Add(this.Date);
            this.Controls.Add(this.btnSave);
            this.Name = "PlayerForm";
            this.Text = "Player Training Data";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Playerg_TextChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Dateg_TextChanged_1(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        //mia ta dika mas
        private System.Windows.Forms.TextBox txtDate;

        #endregion

        private System.Windows.Forms.TextBox Date;
        private System.Windows.Forms.TextBox Player;
        private System.Windows.Forms.TextBox Goal;
        private System.Windows.Forms.TextBox Sprint;
        private System.Windows.Forms.TextBox Vertical;
        private System.Windows.Forms.TextBox TwoShoots;
        private System.Windows.Forms.TextBox ThreeShoots;
        private System.Windows.Forms.TextBox FreeThrows;
        private System.Windows.Forms.TextBox Duration;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
    }
}