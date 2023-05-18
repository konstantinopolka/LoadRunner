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
            Engine game = new Engine(10, 5, 5);
            game.Playing();

            //List<Cell>[,] cells = new List<Cell>[10, 10];

            //for(int i = 0; i < cells.GetLength(0); i++)
            //{
            //    for(int j=0; j < cells.GetLength(1); j++)
            //    {
            //        cells[i, j] = new List<Cell>();
            //    }
            //}

            //Area area = new Area();
            //{
            //    area.ShowArea();
            //}
            //Cell cell = (Cell)area[0,0].Clone();
            //Console.WriteLine(cell.GetType());

        }
    }
}
