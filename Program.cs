using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Field_Sotnikov
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game Game = new Game(8,5,2);
            Game.Run();
        }
    }
}
