using System;
using System.Drawing;
using System.Windows.Forms;

namespace LB3
{
    public static class Program // Головний клас програми
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