using System;
using System.Drawing;
using System.Windows.Forms;

namespace LB3
{
    // Головний клас програми
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}