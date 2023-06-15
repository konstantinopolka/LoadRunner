using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace LB3
{
    public partial class FormRules : Form // клас форми правил гри
    {
        public FormRules() => InitializeComponent();
        private void FormRules_KeyPress(object sender, KeyPressEventArgs e) => Close();
        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e) => Close();
        
    }
}
