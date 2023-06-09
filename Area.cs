using System;

namespace Field_Sotnikov
{

    class Area
    {
        public delegate void A(); 
        private int stepCount;
        public int StepCount {
            get => stepCount;
            set { 
                if(value==stepCount+1)
                    stepCount = value;
            }
        }
        private int userStairs; // столько, сколько хочет пользователь лестниц
        private int userTeleports;
        public int UserTeleports {
            get => userTeleports;
            set { 
                if (value == userTeleports - 1)
                    userTeleports = value;
             }
        }
        public int UserGoldBars { get; } // столько, сколько хочет пользователь золотых слитков
        private int maxStairs; // сколько всего физически может быть на карте лестниц 
        private int maxGoldBars; // сколько всего физически может быть на карте золотых слитков
        private int maxTeleports=4;
        private bool isPlayerThere=false;
        public Cell[,] cells;

        private Random randomForFillArea = new Random();

        public int AreaHeight { get; private set; }
        public int AreaWidth { get; private set; }
        public int PlayerX { get; set; } //  сет должно быть приват
        public int PlayerY { get; set; } // сет должно быть приват

        public Area(int AreaHeight = 10, int AreaWidth = 10, int Stairs = 4, int GoldBars = 5, int Teleports=1)
        {
            this.AreaHeight = AreaHeight;
            this.AreaWidth = AreaWidth;
            cells = new Cell[AreaHeight, AreaWidth];
 
            maxStairs = (AreaWidth-2) * (AreaHeight/2-1);
            maxGoldBars= (AreaWidth - 2) * (AreaHeight / 2);

            if(GoldBars >maxGoldBars)  GoldBars= maxGoldBars;
            UserGoldBars = GoldBars;

            if (Stairs >maxStairs)   Stairs = maxStairs;
            userStairs = Stairs;

            if (Teleports > maxTeleports) Teleports = maxTeleports;
            userTeleports = Teleports;


            fillArea();
           
        }
        public Area(int n = 10, int Stairs = 4, int GoldBars = 5, int Teleports=1) : this(n, n, Stairs, GoldBars, Teleports)
        {
        }
        public Area()
        {
            AreaHeight = 10;
            AreaWidth = 10;
            maxStairs = (AreaWidth - 2) * (AreaHeight / 2 - 1);
            maxGoldBars = (AreaWidth - 2) * (AreaHeight / 2);
            UserGoldBars = maxGoldBars/4;
            userStairs = maxStairs/4;
            cells = new Cell[AreaHeight, AreaWidth];
            userTeleports = 1;
            fillArea();
        }
        private void fillArea()
        {
            fillAreaEmptys();
            fillAreaWalls();
            fillAreaStairs();
            fillAreaPlayer();
            fillAreaGoldBars();
            fillAreaTeleports();
        }
        
        private void fillAreaEmptys()
        {
            for (int i = 0; i < cells.GetLength(0); i++)
                for (int j = 0; j < cells.GetLength(1); j++)
                   this[i, j] = new Empty(i, j);
            
        }

        private void fillAreaWalls()
        {
            int highestWall;
            if (AreaHeight % 2 == 0) highestWall = 1;
            else highestWall = 0;

            for(int i = highestWall; i < cells.GetLength(0);i+=2)
                for(int j=0; j < cells.GetLength(1);j++)
                    cells[i, j]= new Wall(i, j);

            for (int i = 0; i < cells.GetLength(0); i++)
            {
                this[i, 0] = new Wall(i, 0);
                this[i, cells.GetLength(1) - 1] = new Wall(i, cells.GetLength(1) - 1);
            }
        }
        private void fillAreaPlayer()
        {
            while (!isPlayerThere)
            {
                PlayerY = randomForFillArea.Next(0, cells.GetLength(0) - 2);
                PlayerX = randomForFillArea.Next(1, cells.GetLength(1) - 2);
                if (this[PlayerY, PlayerX] is Empty) {
                    this[PlayerY, PlayerX] = new Player(PlayerY, PlayerX);
                    isPlayerThere = true;
                }
            }   
        }


        private void fillAreaStairs()
        {
            int stairX;
            int stairY;
            int stairFactCount = 0;

            for (int i = 1; i < cells.GetLength(0)-1; i++)
            {
                stairX = randomForFillArea.Next(1, cells.GetLength(1) - 1);
                stairY = i;
                if (this[stairY, stairX] is Wall)
                    makeStair(ref stairFactCount, stairX, stairY);
            }
            while (stairFactCount < userStairs)
            {
                stairX = randomForFillArea.Next(1, cells.GetLength(1) - 1);
                stairY = randomForFillArea.Next(1, cells.GetLength(0) - 1);
                makeStair(ref stairFactCount, stairX, stairY);
            }
        }
        private void makeStair(ref int stairFactCount, int stairX, int stairY)
        {
            if (this[stairY, stairX] is Wall)
            {
                this[stairY, stairX] = new Stair(stairY, stairX);
                stairFactCount++;
                if (this[stairY + 1, stairX] is Empty)
                {
                    this[stairY + 1, stairX] = new Stair(stairY + 1, stairX);
                    stairFactCount++;
                }
            }
        }
        private void fillPassableCells(int factCount, Func<int, int, Cell> createInstance)
        {
            int count = 0;
            int x;
            int y;

            while (count < factCount)
            {
                x = randomForFillArea.Next(1, cells.GetLength(1) - 1);
                y = randomForFillArea.Next(0, cells.GetLength(0) - 1);

                if (this[y, x] is Empty)
                {
                    var instance = createInstance(y, x);
                    this[y, x] = instance;
                    count++;
                }
            }
        }
        private void fillAreaGoldBars()
        {
            fillPassableCells(UserGoldBars, (y, x) => new GoldBar(y, x));
        }
        private void fillAreaTeleports()
        {
            fillPassableCells(UserTeleports, (y, x) => new Teleport(y, x));
        }


        public Cell this[int y, int x]
        {
            get => cells[y, x];
            set => cells[y, x] = value;
        }

        public void ShowArea()
        {
            for (int i = 0; i < this.AreaHeight; i++)
            {
                for (int j = 0; j < this.AreaWidth; j++)
                    Console.Write(this[i, j].CellName);
                Console.WriteLine();
            }
        }
    }
}
