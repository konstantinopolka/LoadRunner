namespace LB3
{
    partial class FormRules // клас форми правил гри
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
       

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() 
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRules));
            this.BoxWithRules = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // BoxWithRules
            // 
            this.BoxWithRules.BackColor = System.Drawing.SystemColors.Window;
            this.BoxWithRules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BoxWithRules.Font = new System.Drawing.Font("Microsoft Sans Serif", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BoxWithRules.Location = new System.Drawing.Point(0, 0);
            this.BoxWithRules.Name = "BoxWithRules";
            this.BoxWithRules.Size = new System.Drawing.Size(1264, 681);
            this.BoxWithRules.TabIndex = 0;
            this.BoxWithRules.Text = resources.GetString("BoxWithRules.Text");
            this.BoxWithRules.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.richTextBox1_KeyPress);
            // 
            // FormRules
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.BoxWithRules);
            this.Name = "FormRules";
            this.Text = "MainForm";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormRules_KeyPress);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox BoxWithRules;
    }
}
