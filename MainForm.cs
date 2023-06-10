using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace LB3
{
    delegate void delegateFor2Ints(int y, int x);

    public partial class MainForm : Form
    {
        private Engine engine;
        private Timer gameTimer;
        private Button buttonStart;
        private Button buttonRules;
        private Button buttonExit;
        private bool isPlaying;
        public MainForm()
        {
            InitializeComponent();
            InitializeGame();
            
        }
        
        private void InitializeGame()
        {
            engine = new Engine(11, 20, 15, 5, 1 );
            gameTimer = new Timer();
            gameTimer.Interval = 1000 / 60; // 60 FPS
            gameTimer.Tick += GameTimer_Tick;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            
            
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
           
            if (isPlaying) engine.HandleKeyPress(e.KeyCode, ref isPlaying);
                else
                {
                    this.Controls.Add(buttonStart);
                    this.Controls.Add(buttonRules);
                    this.Controls.Add(buttonExit);
                    this.KeyDown -= MainForm_KeyDown;
                    this.Paint -= MainForm_Paint;
                }
                Refresh();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            engine.DrawGame(e.Graphics);
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

       


        private void InitializeComponent()
        {
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonRules = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.AutoSize = true;
            this.buttonStart.Location = new System.Drawing.Point(474, 86);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(220, 78);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonRules
            // 
            this.buttonRules.AutoSize = true;
            this.buttonRules.Location = new System.Drawing.Point(474, 199);
            this.buttonRules.Name = "buttonRules";
            this.buttonRules.Size = new System.Drawing.Size(220, 72);
            this.buttonRules.TabIndex = 2;
            this.buttonRules.Text = "Rules";
            this.buttonRules.UseVisualStyleBackColor = true;
            this.buttonRules.Click += new System.EventHandler(this.buttonRules_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.AutoSize = true;
            this.buttonExit.Location = new System.Drawing.Point(474, 318);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(220, 101);
            this.buttonExit.TabIndex = 3;
            this.buttonExit.Text = "Exit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.buttonRules);
            this.Controls.Add(this.buttonStart);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "GameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainForm_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
                Close();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            Size = new Size((engine.AreaWidth) * Cell.Size + Cell.Size / 2 + 410, (engine.AreaHeigth + 1) * Cell.Size+100);
            this.Controls.Remove(buttonStart);
            this.Controls.Remove(buttonRules);
            this.Controls.Remove(buttonExit);
            this.KeyDown += MainForm_KeyDown;
            this.Paint += MainForm_Paint;
            isPlaying = true;
            Refresh();
        }

        private void buttonExit_Click(object sender, EventArgs e) => Close();

        private void buttonRules_Click(object sender, EventArgs e)
        {
            FormRules rulesForm = new FormRules();
            rulesForm.Show();
        }
    }
}
