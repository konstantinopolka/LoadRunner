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
        private int points;
        private int teleports;
        private string lastActionComment = "Тут будет коммент твоего последнего шага";
        private char actionChar { get; set; }
        private ConsoleKeyInfo action;
        public Game(int AreaHeigth = 10, int AreaWidth = 10, int MaxStairs = 4, int MaxGoldBars = 5)
        {
            stopwatch = new Stopwatch();
            area = new Area(AreaHeigth, AreaWidth, MaxStairs, MaxGoldBars,1);
        }
        public Game(int n = 10, int MaxStairs = 4, int MaxGoldBars = 5)
        {
            stopwatch = new Stopwatch();
            area = new Area(n, n, MaxStairs, MaxGoldBars,1);
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
        private void pause()
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
        private void showArea()
        {
            Console.WriteLine("w -\t подняться по лестнице вверх на ряд вверх\n" +
                            "s -\t опуститься по лестнице вниз на ряд ниже\n" +
                            "d -\t пройти вправо на одну клетку\n" +
                            "a -\t пройти влево на одну клетку\n" +
                            "z -\t выкопать яму слева\n" +
                            "d - \t выкопать яму справа\n" +
                            "Не копать яму на дне!!! иначе GAME OVER\n" +
                            "Для паузы нажмите Esc\n" +
                            "\n\n"+
                            lastActionComment + 
                            "\n\n");
            for (int i = 0; i < area.AreaHeigth; i++)
            {
                for (int j = 0; j < area.AreaWidth; j++) Console.Write(area[i, j].CellName);
                if (i == 0) Console.Write("\t\tPoints:\t\t" + points);
                if (i == 1) Console.Write("\t\tTeleports:\t" + teleports);
                if (i == 2) Console.Write("\t\tSteps count:\t" + area.StepCount);
                if (i == 3) Console.Write("\t\tTime:\t\t" + stopwatch.Elapsed); 
                Console.WriteLine("");

            }
            Console.WriteLine("\n");
        }

        private void teleport()
        {
            area[area.PlayerY, area.PlayerX] = new Empty(area.PlayerY, area.PlayerX);
            bool isPLayerThere = false;
            Random rnd = new Random();
            while (!isPLayerThere)
            {
                int newPlayerX = rnd.Next(0, area.AreaWidth);
                int newPlayerY = rnd.Next(0, area.AreaHeigth);
                if (area[newPlayerY, newPlayerX] is Empty || area[newPlayerY, newPlayerX] is GoldBar) {
                    isNextCellGoldBarOrTeleport(newPlayerY, newPlayerX);
                    area.PlayerX= newPlayerX;
                    area.PlayerY = newPlayerY;
                    isPLayerThere = true; 
                }
            }
            lastActionComment = "Телепортнулся";
            teleports--;
        }
        public void Run()
        {
           xophenGameOutput();s
            while (true)
            {
                showArea();
                action = Console.ReadKey(true);
                if (action.Key == ConsoleKey.Escape) pause();
                actionChar = action.KeyChar;
                Console.WriteLine("\n");

                stopwatch.Start();
                if (actionChar == 'w' || actionChar == 'a' || actionChar == 'd' || actionChar == 's' || actionChar==' ' )
                {
                    if (actionChar == 'w' && isMoveAble(actionChar, area.PlayerY - 1, area.PlayerX)) area.PlayerY -= 2;
                    if (actionChar == 'd' && isMoveAble(actionChar, area.PlayerY, area.PlayerX + 1)) area.PlayerX++;
                    if (actionChar == 'a' && isMoveAble(actionChar, area.PlayerY, area.PlayerX - 1)) area.PlayerX--;
                    if (actionChar == 's' && isMoveAble(actionChar, area.PlayerY + 1, area.PlayerX)) area.PlayerY += 2;
                    if (actionChar == ' ')
                    {
                        if (teleports > 0) teleport();
                        else lastActionComment = "У тебя нет телепортов";
                    }

                    while ((area[area.PlayerY + 1, area.PlayerX] is Empty || area[area.PlayerY + 1, area.PlayerX] is GoldBar) &&
                            area.PlayerY + 1 != area.AreaHeigth)
                    {
                        isNextCellGoldBarOrTeleport(area.PlayerY + 1, area.PlayerX);
                        area.PlayerY++;  // 
                    }

                    area[area.PlayerY, area.PlayerX] = new Player(area.PlayerY, area.PlayerX);

                    if (isThereThePit && isLastActionAble ) {
                        area[lastPit.Y, lastPit.X] = new Wall(lastPit.Y, lastPit.X); 
                        isThereThePit = false;
                           
                    }
                }
               
                   
                 if (actionChar == 'z' && isPitAble(actionChar, area.PlayerY + 1, area.PlayerX - 1)) area[area.PlayerY + 1, area.PlayerX - 1] = new Empty(area.PlayerY + 1, area.PlayerX - 1);     
                 if (actionChar == 'x' && isPitAble(actionChar, area.PlayerY + 1, area.PlayerX + 1)) area[area.PlayerY + 1, area.PlayerX + 1] = new Empty(area.PlayerY + 1, area.PlayerX + 1);
               
                Console.Clear();
                if(area.UserGoldBars==points) { for (int i = 0; i < 1000; i++) { Console.WriteLine("Victory!!!"); } stopwatch.Stop();  break;  }
                if (area.PlayerY == area.AreaHeigth - 1) { for (int i = 0; i < 1000; i++) { Console.WriteLine("GAME OVER"); } stopwatch.Stop(); break;  } // проверка на выпадание под поле
            }
        }
        private bool isMoveAble(char Action, int y, int x)
        {
            bool ans=true;
            if (area[y, x] is Wall) { ans = false; lastActionComment = "Стена!!! Куда прешь??!!"; }
            else if (area[y, x] is Stair)
            {
                if (Action == 'w') isNextCellGoldBarOrTeleport(y - 1, x);
                if (Action == 's') isNextCellGoldBarOrTeleport(y + 1, x);
                lastActionComment = "Перешел по лестнице";
            }
            else if (area[y, x] is Empty || area[y, x] is GoldBar || area[y, x] is Teleport) { lastActionComment = "Просто прошелся"; isNextCellGoldBarOrTeleport(y, x);  }
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
        private bool isPitAble(char Action, int y, int x)
        {
            if (x == 0 || x == area.AreaWidth - 1) { isLastActionAble = false; lastActionComment = "За границы карты не выходим, пожалуйста"; return false;   }
            else if (area[y, x] is Stair) { isLastActionAble = false; lastActionComment = "Лестница не стена, так просто не сломаешь"; return false; }
            isLastActionAble = true;
            if (isThereThePit && isLastActionAble) { area[lastPit.Y, lastPit.X] = new Wall(lastPit.Y, lastPit.X); }
            isThereThePit = true;
            lastPit = new Empty(y, x);
            area.StepCount++;
            lastActionComment = "Дырку сделал, молодец";
            return true;
        }
        private void isNextCellGoldBarOrTeleport(int y, int x)
        {
            if (area[y, x] is GoldBar) { points++; lastActionComment += " и скушал золото!"; }
            if (area[y, x] is Teleport) { teleports++; lastActionComment += " и скушал телепорт!"; }
        }
        
    }
}
