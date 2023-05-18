using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
        public Cell[,] cells;

        private Random randomForFillArea = new Random();
        private int[] ArrayForNoEmpty;

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

            ArrayForNoEmpty = new int[cells.GetLength(0) / 2]; for (int i = 0; i < ArrayForNoEmpty.Length; i++) ArrayForNoEmpty[i] = i * 2;

            fillArea();
           
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

            ArrayForNoEmpty= new int[cells.GetLength(0) / 2]; for (int i = 0; i < ArrayForNoEmpty.Length; i++) ArrayForNoEmpty[i] = i * 2;

            fillArea();
        }
        private void fillArea()
        {
            if (AreaHeigth % 2 == 0)
            {
                fillAreaEmptys();
                fillAreaWalls();
                fillAreaPlayer();
                fillAreaStairs();
                fillAreaGoldBars();
                fillAreaTeleports();
            }
        }

        private void fillAreaEmptys()
        {
            for (int i = 0; i < cells.GetLength(0); i++)
                for (int j = 0; j < cells.GetLength(1); j++)
                   this[i, j] = new Empty(i, j);
            
        }
        private void fillAreaWalls()
        {
            for(int i=1; i < cells.GetLength(0);i+=2)
                for(int j=0; j < cells.GetLength(1);j++)
                    cells[i, j]= new Wall(i, j);
            for (int j = 0; j < cells.GetLength(1); j += cells.GetLength(1) - 1)
                for (int i = 0; i < cells.GetLength(0); i++)
                    this[i, j] = new Wall(i, j);
        }

        private void fillAreaPlayer()
        {
            while (!isPlayerThere)
            {
                PlayerY = randomForFillArea.Next(ArrayForNoEmpty[randomForFillArea.Next(0, ArrayForNoEmpty.Length)]);
                PlayerX = randomForFillArea.Next(0, cells.GetLength(1) - 2);
                if (this[PlayerY, PlayerX] is Empty) {
                    this[PlayerY, PlayerX] = new Player(PlayerY, PlayerX);
                    isPlayerThere = true;
                }
            }   
        }


        private void fillAreaStairs()
        {
            int[] ArrayForStairs = new int[cells.GetLength(0) / 2-1];
            ArrayForStairs[0] = 1;
            for (int i = 1; i < ArrayForStairs.Length; i++) ArrayForStairs[i] = ArrayForStairs[i-1]+2;
            int stairX;
            int stairY;
            int stairFactCount = 0;

            for (int i = 0; i < ArrayForStairs.Length; i++)
            {
                stairX = randomForFillArea.Next(1, cells.GetLength(1) - 1);
                stairY = ArrayForStairs[i];
                makeStair(ref stairFactCount, stairX, stairY);
            }
            while (stairFactCount < userStairs)
            {
                stairX = randomForFillArea.Next(1, cells.GetLength(1) - 1);
                stairY = randomForFillArea.Next(ArrayForStairs[randomForFillArea.Next(0, ArrayForStairs.Length)]);
                makeStair(ref stairFactCount, stairX, stairY);
            }
        }
        private void makeStair(ref int stairFactCount, int stairX, int stairY)
        {
            if (this[stairY, stairX] is Wall)
            {
                this[stairY, stairX] = new Stair(stairY, stairX);
                stairFactCount++;
                //if (this[stairY + 1, stairX] is Empty)
                //{
                //    this[stairY + 1, stairX] = new Stair(stairY + 1, stairX);
                //    stairFactCount++;
                //}
            }
        }

        private void fillAreaGoldBars()
        {
            int goldBarFactCount = 0;
            int goldBarX;
            int goldBarY;

            while (goldBarFactCount < UserGoldBars)
            {
                goldBarX = randomForFillArea.Next(1, cells.GetLength(1) - 1);
                goldBarY = randomForFillArea.Next(ArrayForNoEmpty[randomForFillArea.Next(0, ArrayForNoEmpty.Length)]);
                if (this[goldBarY, goldBarX] is Empty)
                {
                    this[goldBarY, goldBarX] = new GoldBar(goldBarY, goldBarX);
                    isPlayerThere = true;
                    goldBarFactCount++;
                }
            }
        }
        private void fillAreaTeleports()
        {
            int teleportsFactCount = 0;
            int teleportsX;
            int teleportsY;
            while (teleportsFactCount < UserTeleports)
            {
                teleportsX = randomForFillArea.Next(1, cells.GetLength(1) - 1);
                teleportsY = randomForFillArea.Next(ArrayForNoEmpty[randomForFillArea.Next(0, ArrayForNoEmpty.Length)]);
                if (this[teleportsY, teleportsX] is Empty)
                {
                    this[teleportsY, teleportsX] = new Teleport(teleportsY, teleportsX);
                    isPlayerThere = true;
                    teleportsFactCount++;
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
                   return new Wall();
                }

            }
            set {

                if (x >= 0 && x < cells.GetLength(1) && y >= 0 && y < cells.GetLength(0)) cells[y, x] = value;
            
            }  // поставить исключение на подачу ссылки другого класса
        }

        public void ShowArea()
        {
            for (int i = 0; i < this.AreaHeigth; i++)
            {
                for (int j = 0; j < this.AreaWidth; j++) Console.Write(this[i, j].CellName);
                Console.WriteLine("");

            }
        }
    }
}
