namespace CalculatriceFR2
{
    partial class Chaos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Chaos));
            this.TXT_Expression = new System.Windows.Forms.TextBox();
            this.ListeFonctions = new System.Windows.Forms.ListBox();
            this.ListeVariables = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fonctionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.BTN_Exec = new System.Windows.Forms.Button();
            this.BTN_ClearCalculs = new System.Windows.Forms.Button();
            this.radio_Degres = new System.Windows.Forms.RadioButton();
            this.radio_Radians = new System.Windows.Forms.RadioButton();
            this.radio_Grades = new System.Windows.Forms.RadioButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ListeCalculs = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TXT_Expression
            // 
            resources.ApplyResources(this.TXT_Expression, "TXT_Expression");
            this.TXT_Expression.Name = "TXT_Expression";
            this.TXT_Expression.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Expression_KeyPress);
            // 
            // ListeFonctions
            // 
            resources.ApplyResources(this.ListeFonctions, "ListeFonctions");
            this.ListeFonctions.FormattingEnabled = true;
            this.ListeFonctions.Name = "ListeFonctions";
            // 
            // ListeVariables
            // 
            resources.ApplyResources(this.ListeVariables, "ListeVariables");
            this.ListeVariables.FormattingEnabled = true;
            this.ListeVariables.Name = "ListeVariables";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fonctionsToolStripMenuItem,
            this.toolStripMenuItem1});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // fonctionsToolStripMenuItem
            // 
            this.fonctionsToolStripMenuItem.Name = "fonctionsToolStripMenuItem";
            resources.ApplyResources(this.fonctionsToolStripMenuItem, "fonctionsToolStripMenuItem");
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // BTN_Exec
            // 
            resources.ApplyResources(this.BTN_Exec, "BTN_Exec");
            this.BTN_Exec.Name = "BTN_Exec";
            this.BTN_Exec.UseVisualStyleBackColor = true;
            this.BTN_Exec.Click += new System.EventHandler(this.BTN_Exec_Click);
            // 
            // BTN_ClearCalculs
            // 
            resources.ApplyResources(this.BTN_ClearCalculs, "BTN_ClearCalculs");
            this.BTN_ClearCalculs.Name = "BTN_ClearCalculs";
            this.BTN_ClearCalculs.UseVisualStyleBackColor = true;
            this.BTN_ClearCalculs.Click += new System.EventHandler(this.BTN_ClearCalculs_Click);
            // 
            // radio_Degres
            // 
            resources.ApplyResources(this.radio_Degres, "radio_Degres");
            this.radio_Degres.Checked = true;
            this.radio_Degres.Name = "radio_Degres";
            this.radio_Degres.TabStop = true;
            this.radio_Degres.UseVisualStyleBackColor = true;
            this.radio_Degres.CheckedChanged += new System.EventHandler(this.radio_Degres_CheckedChanged);
            // 
            // radio_Radians
            // 
            resources.ApplyResources(this.radio_Radians, "radio_Radians");
            this.radio_Radians.Name = "radio_Radians";
            this.radio_Radians.UseVisualStyleBackColor = true;
            this.radio_Radians.CheckedChanged += new System.EventHandler(this.radio_Radians_CheckedChanged);
            // 
            // radio_Grades
            // 
            resources.ApplyResources(this.radio_Grades, "radio_Grades");
            this.radio_Grades.Name = "radio_Grades";
            this.radio_Grades.UseVisualStyleBackColor = true;
            this.radio_Grades.CheckedChanged += new System.EventHandler(this.radio_Grades_CheckedChanged);
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ListeCalculs);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ListeFonctions);
            // 
            // ListeCalculs
            // 
            resources.ApplyResources(this.ListeCalculs, "ListeCalculs");
            this.ListeCalculs.Name = "ListeCalculs";
            this.ListeCalculs.ReadOnly = true;
            // 
            // Chaos
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.radio_Grades);
            this.Controls.Add(this.radio_Radians);
            this.Controls.Add(this.radio_Degres);
            this.Controls.Add(this.BTN_ClearCalculs);
            this.Controls.Add(this.BTN_Exec);
            this.Controls.Add(this.ListeVariables);
            this.Controls.Add(this.TXT_Expression);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Chaos";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TXT_Expression;
        private System.Windows.Forms.ListBox ListeFonctions;
        private System.Windows.Forms.ListBox ListeVariables;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.Button BTN_Exec;
        private System.Windows.Forms.Button BTN_ClearCalculs;
        private System.Windows.Forms.RadioButton radio_Degres;
        private System.Windows.Forms.RadioButton radio_Radians;
        private System.Windows.Forms.RadioButton radio_Grades;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem fonctionsToolStripMenuItem;
        private System.Windows.Forms.TextBox ListeCalculs;


    }
}

