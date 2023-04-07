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
        public int Points { get; set; }
        private int userStairs; // столько, сколько хочет пользователь лестниц
        private int userGoldBars; // столько, сколько хочет пользователь золотых слитков
        private int maxStairs; // сколько всего физически может быть на карте лестниц 
        private int maxGoldBars; // сколько всего физически может быть на карте золотых слитков
        private bool isPlayerThere=false;
        private Cell[,] cells;
        public int AreaHeigth { get; private set; }
        public int AreaWidth { get; private set; }
        public int PlayerX { get; set; } //  сет должно быть приват
        public int PlayerY { get; set; } // сет должно быть приват

        public Area(int AreaHeigth = 10, int AreaWidth = 10, int Stairs = 4, int GoldBars = 5)
        {
            this.AreaHeigth = AreaHeigth;
            this.AreaWidth = AreaWidth;
            cells = new Cell[AreaHeigth, AreaWidth];
           

            maxStairs = (AreaWidth-2) * (AreaHeigth/2-1);
            maxGoldBars= (AreaWidth - 2) * (AreaHeigth / 2);

            if(GoldBars >maxGoldBars)  GoldBars= maxGoldBars;
            userGoldBars = GoldBars;

            if (Stairs >maxStairs)  maxStairs = Stairs;
            userStairs = Stairs;
            FillArea();
        }
        public Area(int n = 10, int MaxStairs = 4, int MaxGoldBars = 5) : this(n, n, MaxStairs, MaxGoldBars)
        {
        }
        public Area()
        {
            AreaHeigth = 10;
            AreaWidth = 10;
            maxStairs = (AreaWidth - 2) * (AreaHeigth / 2 - 1);
            maxGoldBars = (AreaWidth - 2) * (AreaHeigth / 2);
            userGoldBars = maxGoldBars/4;
            userStairs = maxStairs/4;
            cells = new Cell[AreaHeigth, AreaWidth];
            FillArea();
        }
        public void ShowArea()
        {
            Console.WriteLine("w -\t подняться по лестнице вверх на ряд вверх\n" +
                            "s -\t опуститься по лестнице вниз на ряд ниже\n" +
                            "d -\t пройти вправо на одну клетку\n" +
                            "a -\t пройти влево на одну клетку\n" +
                            "z -\t выкопать яму слева\n" +
                            "d - \t выкопать яму справа\n" +
                            "Не копать яму на дне!!! иначе GAME OVER\n\n"); 
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++) Console.Write(cells[i, j].CellName);
                if (i == 0) Console.Write("\t\tPoints:\t\t" + Points);
                if (i == 1) Console.Write("\t\tStep count:\t" + stepCount);
                Console.WriteLine("");

            }
            Console.WriteLine("\n");
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
                    int goldBarFactCount=0;
                    int stairFactCount=0;
                    while (goldBarFactCount < userGoldBars || stairFactCount < userStairs)
                    {
                        for (int i = 0, j; i < cells.GetLength(0) - 1; i++)
                        {
                            j = rnd.Next(1, cells.GetLength(1) - 1);
                            if (cells[i, j] as Stair == null && stairFactCount < userStairs && i % 2 == 1 && stairFactCount < userStairs && stairFactCount < maxStairs) { cells[i, j] = new Stair(i, j); stairFactCount++; }
                            j = rnd.Next(1, cells.GetLength(1) - 1);
                            if (cells[i, j] as Player == null && cells[i, j] as GoldBar == null && goldBarFactCount <= userGoldBars && i % 2 == 0 && goldBarFactCount < userGoldBars && goldBarFactCount < maxGoldBars) {cells[i, j] = new GoldBar(i, j); goldBarFactCount++;}
                        }
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
