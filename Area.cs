using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Field_Sotnikov
{
    class Area
    {
        private int stepCount;
        public int StepCount {
            get { return stepCount; } 
            set { 
                if(value==stepCount+1) stepCount = value;
            }
        }
        private int userStairs; // столько, сколько хочет пользователь лестниц
        private int userTeleports;
        public int UserTeleports {
            get { return userTeleports; }
            set { if (value == userTeleports - 1) userTeleports = value; }
        }
        public int UserGoldBars { get; } // столько, сколько хочет пользователь золотых слитков
        private int maxStairs; // сколько всего физически может быть на карте лестниц 
        private int maxGoldBars; // сколько всего физически может быть на карте золотых слитков
        private int maxTeleports=4;
        private bool isPlayerThere=false;
        private Cell[,] cells;
        public int AreaHeigth { get; private set; }
        public int AreaWidth { get; private set; }
        public int PlayerX { get; set; } //  сет должно быть приват
        public int PlayerY { get; set; } // сет должно быть приват

        public Area(int AreaHeigth = 10, int AreaWidth = 10, int Stairs = 4, int GoldBars = 5, int Teleports=1)
        {
            this.AreaHeigth = AreaHeigth;
            this.AreaWidth = AreaWidth;
            cells = new Cell[AreaHeigth, AreaWidth];
 
            maxStairs = (AreaWidth-2) * (AreaHeigth/2-1);
            maxGoldBars= (AreaWidth - 2) * (AreaHeigth / 2);

            if(GoldBars >maxGoldBars)  GoldBars= maxGoldBars;
            UserGoldBars = GoldBars;

            if (Stairs >maxStairs)   Stairs = maxStairs;
            userStairs = Stairs;

            if (Teleports > maxTeleports) Teleports = maxTeleports;
            userTeleports = Teleports;
            FillArea();
           
        }
        public Area(int n = 10, int Stairs = 4, int GoldBars = 5, int Teleports=1) : this(n, n, Stairs, GoldBars, Teleports)
        {
        }
        public Area()
        {
            AreaHeigth = 10;
            AreaWidth = 10;
            maxStairs = (AreaWidth - 2) * (AreaHeigth / 2 - 1);
            maxGoldBars = (AreaWidth - 2) * (AreaHeigth / 2);
            UserGoldBars = maxGoldBars/4;
            userStairs = maxStairs/4;
            cells = new Cell[AreaHeigth, AreaWidth];
            userTeleports = 1;
            FillArea();
        }
        private void FillArea()
        {
            Random rnd = new Random();
            int[] ArrayForPlayer = new int[cells.GetLength(0) / 2];

            if (AreaHeigth % 2 == 0)
            {
                for (int i = 0; i < cells.GetLength(0); i++)
                    for (int j = 0; j < cells.GetLength(1); j++)
                        cells[i, j] = new Empty(i, j);

                for (int i = 0; i < ArrayForPlayer.Length; i++) ArrayForPlayer[i] = i * 2;
                int RndIntForCells;
                for (int i = 0; i < cells.GetLength(0); i++)
                {
                    for (int j = 0; j < cells.GetLength(1); j++)
                    {
                        if (!isPlayerThere)
                        {
                            RndIntForCells = rnd.Next(1, cells.GetLength(1) - 1);
                            cells[/*ArrayForPlayer[rnd.Next(0, ArrayForPlayer.Length)]*/0, RndIntForCells] = new Player(0, RndIntForCells);
                            PlayerY = 0;
                            PlayerX = RndIntForCells;
                            isPlayerThere = true;
                        }
                        if (j == 0 || j == cells.GetLength(1) - 1 || i % 2 == 1) cells[i, j] = new Wall(i, j);
                    }
                }
                {
                    int j = 0;
                    int goldBarFactCount=0;
                    int stairFactCount=0;
                    int teleportsFactCount = 0;

                    for (int i = 0; i < cells.GetLength(0) - 1 && stairFactCount < userStairs; i++)
                    {
                        j = rnd.Next(1, cells.GetLength(1) - 1);
                        if (cells[i, j] as Stair == null && i % 2 == 1 && stairFactCount < userStairs ) { cells[i, j] = new Stair(i, j); stairFactCount++; }
                    }
                    while(goldBarFactCount < UserGoldBars)
                    {
                        RndIntForCells = ArrayForPlayer[rnd.Next(0, ArrayForPlayer.Length)];
                        j = rnd.Next(1, cells.GetLength(1) - 1);
                        if (cells[RndIntForCells, j] as Player == null && cells[RndIntForCells, j] as GoldBar == null) { cells[RndIntForCells, j] = new GoldBar(RndIntForCells, j); goldBarFactCount++; }
                    }
                    while(teleportsFactCount < UserTeleports) {
                        RndIntForCells = ArrayForPlayer[rnd.Next(0, ArrayForPlayer.Length)];
                        j = rnd.Next(1, cells.GetLength(1) - 1);
                        if (cells[RndIntForCells, j] as Player == null && cells[RndIntForCells, j] as GoldBar == null && cells[RndIntForCells, j] as Teleport == null) { cells[RndIntForCells, j] = new Teleport(RndIntForCells, j); teleportsFactCount++; }
                    }
                }
            }
        }
        public Cell this[int y, int x]
        {
            get
            {
                if (x >= 0 && x<cells.GetLength(1) && y >= 0 && y<cells.GetLength(0))  return cells[y, x];
                else 
                {
                    Console.WriteLine("Пожалуйста, введите корректные координаты\n\n");  // переделать под исключение потом
                    return null;
                }

            }
            set {

                if (x >= 0 && x < cells.GetLength(1) && y >= 0 && y < cells.GetLength(0)) cells[y, x] = value;
            
            }  // поставить исключение на подачу ссылки другого класса
        }
    }
}
