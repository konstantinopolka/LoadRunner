using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection;

namespace Field_Sotnikov
{
    class Game
    {
        private Area area;
        private bool isThereThePit;
        private Cell lastPit;
        private bool isLastActionAble = false;
        private Cell lastMove;
        private Stopwatch stopwatch;
        private char actionChar { get; set; }
        private ConsoleKeyInfo action;
        public Game(int AreaHeigth = 10, int AreaWidth = 10, int MaxStairs = 4, int MaxGoldBars = 5)
        {
            stopwatch = new Stopwatch();
            area = new Area(AreaHeigth, AreaWidth, MaxStairs, MaxGoldBars);
        }
        public Game(int n = 10, int MaxStairs = 4, int MaxGoldBars = 5)
        {
            stopwatch = new Stopwatch();
            area = new Area(n, n, MaxStairs, MaxGoldBars);
        }
        public Game()
        {
            stopwatch = new Stopwatch();
            int AreaHeigth = 10;
            int AreaWidth = 10;
            int MaxStairs = 4;
            int MaxGoldBars = 5;
            area = new Area(AreaHeigth, AreaWidth, MaxStairs, MaxGoldBars);
        }
        public Game(int n)
        {
            stopwatch = new Stopwatch();
            area = new Area(n, n, n, n);
        }
        private void pauseText()
        {
            Console.Clear();
            for (int i = 0; i < 15; i++) Console.WriteLine();
            for (int i = 0; i < 7; i++) Console.Write("\t");
            Console.WriteLine("Pause\n");
            for (int i = 0; i < 7; i++) Console.Write("\t");
            Console.WriteLine("Please enter Esc again\n");
        }
        private void Pause()
        {
            pauseText();
            while (Console.ReadKey(true).Key!=ConsoleKey.Escape)
                pauseText();
        }
        private void openGameOutput()
        {
           
            Console.BackgroundColor=ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("\t\t\t\t");
            Console.WriteLine("Вас привествует LoadRunner для бедных!");

            Console.BackgroundColor = ConsoleColor.Black; Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("*         ********           *          **********         *********     *          *  **        *  **        *  ***********  *********    ");
            Console.WriteLine("*        *        *         * *         *         *        *        *    *          *  * *       *  * *       *  *            *        *   ");
            Console.WriteLine("*        *        *        *   *        *          *       *         *   *          *  *  *      *  *  *      *  *            *         *  ");
            Console.WriteLine("*        *        *       *     *       *          *       *         *   *          *  *   *     *  *   *     *  *            *         *  ");
            Console.WriteLine("*        *        *      *********      *          *       *        *    *          *  *    *    *  *    *    *  ***********  *        *   ");
            Console.WriteLine("*        *        *     *         *     *          *       *********     *         **  *     *   *  *     *   *  *            *********    ");
            Console.WriteLine("*        *        *    *           *    *          *       *        *    *        * *  *      *  *  *      *  *  *            *        *   ");
            Console.WriteLine("*        *        *   *             *   *         *        *         *   *       *  *  *       * *  *       * *  *            *         *  ");
            Console.WriteLine("*******   ********    *             *   **********         *          *   *******   *  *         *  *         *  ***********  *          * ");
            Thread.Sleep(2000);
            Console.Clear() ;
        }
        public void ShowArea()
        {
            Console.WriteLine("w -\t подняться по лестнице вверх на ряд вверх\n" +
                            "s -\t опуститься по лестнице вниз на ряд ниже\n" +
                            "d -\t пройти вправо на одну клетку\n" +
                            "a -\t пройти влево на одну клетку\n" +
                            "z -\t выкопать яму слева\n" +
                            "d - \t выкопать яму справа\n" +
                            "Не копать яму на дне!!! иначе GAME OVER\n" +
                            "Для паузы нажмите Esc\n\n");
            for (int i = 0; i < area.AreaHeigth; i++)
            {
                for (int j = 0; j < area.AreaWidth; j++) Console.Write(area[i, j].CellName);
                if (i == 0) Console.Write("\t\tPoints:\t\t" + area.Points);
                if (i == 1) Console.Write("\t\tSteps count:\t" + area.StepCount);
                if (i == 2) Console.Write("\t\tTime:\t" + stopwatch.Elapsed); 
                Console.WriteLine("");

            }
            Console.WriteLine("\n");
        }

        public void Teleport()
        {
            area[area.PlayerY, area.PlayerX] = new Empty(area.PlayerY, area.PlayerX);
            bool isPLayerThere = false;
            Random rnd = new Random();
            while (!isPLayerThere)
            {
                int newPlayerX = rnd.Next(0, area.AreaWidth);
                int newPlayerY = rnd.Next(0, area.AreaHeigth);
                if (area[newPlayerY, newPlayerX] is Empty || area[newPlayerY, newPlayerX] is GoldBar) {
                    IsNextCellGoldBar(newPlayerY, newPlayerX);
                    area.PlayerX= newPlayerX;
                    area.PlayerY = newPlayerY;
                    isPLayerThere = true; 
                }
            }
        }
        public void Run()
        {
            openGameOutput();
            while (true)
            {
                ShowArea();
                action = Console.ReadKey(true);
                if (action.Key == ConsoleKey.Escape) Pause();
                actionChar = action.KeyChar;
                Console.WriteLine("\n");

                stopwatch.Start();
                if (actionChar == 'w' || actionChar == 'a' || actionChar == 'd' || actionChar == 's' || actionChar==' ' )
                {
                    if (actionChar == 'w' && IsMoveAble(actionChar, area.PlayerY - 1, area.PlayerX)) area.PlayerY -= 2;
                    if (actionChar == 'd' && IsMoveAble(actionChar, area.PlayerY, area.PlayerX + 1)) area.PlayerX++;
                    if (actionChar == 'a' && IsMoveAble(actionChar, area.PlayerY, area.PlayerX - 1)) area.PlayerX--;
                    if (actionChar == 's' && IsMoveAble(actionChar, area.PlayerY + 1, area.PlayerX)) area.PlayerY += 2;
                    if (actionChar == ' ') Teleport();

                    while ((area[area.PlayerY + 1, area.PlayerX] is Empty || area[area.PlayerY + 1, area.PlayerX] is GoldBar) &&
                            area.PlayerY + 1 != area.AreaHeigth)
                    {
                        IsNextCellGoldBar(area.PlayerY + 1, area.PlayerX);
                        area.PlayerY++;  // 
                    }

                    area[area.PlayerY, area.PlayerX] = new Player(area.PlayerY, area.PlayerX);

                    if (isThereThePit && isLastActionAble ) {
                        area[lastPit.Y, lastPit.X] = new Wall(lastPit.Y, lastPit.X); 
                        isThereThePit = false;
                           
                    }
                }
               
                   
                 if (actionChar == 'z' && IsPitAble(actionChar, area.PlayerY + 1, area.PlayerX - 1)) area[area.PlayerY + 1, area.PlayerX - 1] = new Empty(area.PlayerY + 1, area.PlayerX - 1);     
                 if (actionChar == 'x' && IsPitAble(actionChar, area.PlayerY + 1, area.PlayerX + 1)) area[area.PlayerY + 1, area.PlayerX + 1] = new Empty(area.PlayerY + 1, area.PlayerX + 1);
               
                Console.Clear();
                if(area.userGoldBars==area.Points) { for (int i = 0; i < 1000; i++) { Console.WriteLine("Victory!!!"); } stopwatch.Stop();  break;  }
                if (area.PlayerY == area.AreaHeigth - 1) { for (int i = 0; i < 1000; i++) { Console.WriteLine("GAME OVER"); } stopwatch.Stop(); break;  } // проверка на выпадние под поле
            }
        }
        private bool IsMoveAble(char Action, int y, int x)
        {
            bool ans=true;
            if (area[y, x] is Wall) ans = false;
            else if (area[y, x] is Stair)
            {
                if (Action == 'w') IsNextCellGoldBar(y - 1, x);
                if (Action == 's') IsNextCellGoldBar(y + 1, x);
            }
            else if (area[y, x] is Empty || area[y, x] is GoldBar) IsNextCellGoldBar(y, x);
            else ans = false;
           
            if (ans)
            {
                area[area.PlayerY, area.PlayerX] = new Empty(area.PlayerY, area.PlayerX);
                isLastActionAble = true;
                area.StepCount++;
                return true;
            }
            else { isLastActionAble = false; return false; }
        }
        private bool IsPitAble(char Action, int y, int x)
        {
            if (x == 0 || x == area.AreaWidth - 1) { isLastActionAble = false; return false;  }
            else if (area[y, x] is Stair) { isLastActionAble = false; return false; }
            isLastActionAble = true;
            if (isThereThePit && isLastActionAble) { area[lastPit.Y, lastPit.X] = new Wall(lastPit.Y, lastPit.X); }
            isThereThePit = true;
            lastPit = new Empty(y, x);
            area.StepCount++;
            return true;
        }
        private void IsNextCellGoldBar(int y, int x)
        {
            if (area[y, x] is GoldBar) { Console.WriteLine("Скушал золото!!!"); area.Points++; }
        }
    }
}
