namespace Basktics_v2._0
{
    partial class Basketics
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Basketics));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.teamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.matchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newTeamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.yourTeamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewPlayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.AllowDrop = true;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.teamToolStripMenuItem,
            this.playersToolStripMenuItem,
            this.matchToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            // 
            // teamToolStripMenuItem
            // 
            this.teamToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newTeamToolStripMenuItem,
            this.yourTeamToolStripMenuItem});
            this.teamToolStripMenuItem.Name = "teamToolStripMenuItem";
            resources.ApplyResources(this.teamToolStripMenuItem, "teamToolStripMenuItem");
            // 
            // playersToolStripMenuItem
            // 
            this.playersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewPlayerToolStripMenuItem});
            this.playersToolStripMenuItem.Name = "playersToolStripMenuItem";
            resources.ApplyResources(this.playersToolStripMenuItem, "playersToolStripMenuItem");
            // 
            // matchToolStripMenuItem
            // 
            this.matchToolStripMenuItem.Name = "matchToolStripMenuItem";
            resources.ApplyResources(this.matchToolStripMenuItem, "matchToolStripMenuItem");
            // 
            // newTeamToolStripMenuItem
            // 
            this.newTeamToolStripMenuItem.Name = "newTeamToolStripMenuItem";
            resources.ApplyResources(this.newTeamToolStripMenuItem, "newTeamToolStripMenuItem");
            // 
            // yourTeamToolStripMenuItem
            // 
            this.yourTeamToolStripMenuItem.Name = "yourTeamToolStripMenuItem";
            resources.ApplyResources(this.yourTeamToolStripMenuItem, "yourTeamToolStripMenuItem");
            // 
            // addNewPlayerToolStripMenuItem
            // 
            this.addNewPlayerToolStripMenuItem.Name = "addNewPlayerToolStripMenuItem";
            resources.ApplyResources(this.addNewPlayerToolStripMenuItem, "addNewPlayerToolStripMenuItem");
            // 
            // Basketics
            // 
            this.AllowDrop = true;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.Controls.Add(this.menuStrip1);
            this.Name = "Basketics";
            this.Load += new System.EventHandler(this.Basketics_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem teamToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newTeamToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem yourTeamToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewPlayerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem matchToolStripMenuItem;
    }
}

