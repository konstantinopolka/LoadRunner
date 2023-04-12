using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Field_Sotnikov
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game Game = new Game(10,5,4);
            Game.Run(); 
        }
    }
}
