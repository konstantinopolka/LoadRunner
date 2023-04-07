using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Field_Sotnikov
{
    class Game
    {
        private Area area;
        private bool isThereThePit;
        private Cell lastPit;
        private bool isLastActionAble = false;
        private Cell lastMove;
        private string action { get; set; }
        public Game(int AreaHeigth = 10, int AreaWidth = 10, int MaxStairs = 4, int MaxGoldBars = 5)
        {
            area = new Area(AreaHeigth, AreaWidth, MaxStairs, MaxGoldBars);
        }
        public Game(int n = 10, int MaxStairs = 4, int MaxGoldBars = 5)
        {
            area = new Area(n, n, MaxStairs, MaxGoldBars);
        }
        public Game()
        {
            int AreaHeigth = 10;
            int AreaWidth = 10;
            int MaxStairs = 4;
            int MaxGoldBars = 5;
            area = new Area(AreaHeigth, AreaWidth, MaxStairs, MaxGoldBars);
        }
        public Game(int n)
        {
            area = new Area(n, n, n, n);
        }
        public void Run()
        {
            while (true)
            {
                area.ShowArea();
                if(area.PlayerY==area.AreaHeigth-1) { Console.WriteLine("GAME OVER"); break; } // проверка на выпадние под поле
                action = Console.ReadKey().KeyChar.ToString();
                Console.WriteLine("\n");
                

                if (action == "w" || action == "a" || action == "d" || action == "s")
                {
                    if (action == "w")
                    {
                        if (IsMoveAble(action, area.PlayerY - 1, area.PlayerX))
                        {
                            area[area.PlayerY, area.PlayerX] = new Empty(area.PlayerY, area.PlayerX);
                            area.PlayerY -= 2;

                        }
                    }
                    if (action == "d")
                    {
                        if (IsMoveAble(action, area.PlayerY, area.PlayerX + 1))
                        {
                            area[area.PlayerY, area.PlayerX] = new Empty(area.PlayerY, area.PlayerX);
                            area.PlayerX++;
                        }
                    }
                    if (action == "a")
                    {
                        if (IsMoveAble(action, area.PlayerY, area.PlayerX - 1))
                        {
                            area[area.PlayerY, area.PlayerX] = new Empty(area.PlayerY, area.PlayerX);
                            area.PlayerX--;
                        }
                    }
                    if (action == "s")
                    {
                        if (IsMoveAble(action, area.PlayerY + 1, area.PlayerX))
                        {
                            area[area.PlayerY, area.PlayerX] = new Empty(area.PlayerY, area.PlayerX);
                            area.PlayerY += 2;
                        }
                    }
                    while (area[area.PlayerY + 1, area.PlayerX] is Empty && area.PlayerY + 1 != area.AreaHeigth) area.PlayerY++;
                    area[area.PlayerY, area.PlayerX] = new Player(area.PlayerY, area.PlayerX);

                    if (isThereThePit
                        && isLastActionAble
                        ) {
                        area[lastPit.Y, lastPit.X] = new Wall(lastPit.Y, lastPit.X); 
                        isThereThePit = false;
                           
                    }
                }
                else if (action == "z" || action == "x")
                {
                    if (isThereThePit && isLastActionAble) { area[lastPit.Y, lastPit.X] = new Wall(lastPit.Y, lastPit.X); isThereThePit = false; }
                    if (action == "z")
                    {
                        if (IsPitAble(action, area.PlayerY + 1, area.PlayerX - 1))
                            {
                           
                            area[area.PlayerY + 1, area.PlayerX - 1] = new Empty(area.PlayerY + 1, area.PlayerX - 1);

                        }
                    }
                    if (action == "x")
                    {

                        if (IsPitAble(action, area.PlayerY + 1, area.PlayerX + 1))
                        {
                            
                            area[area.PlayerY + 1, area.PlayerX + 1] = new Empty(area.PlayerY + 1, area.PlayerX + 1);
                        }

                            }
                }
                



                Console.Clear();
            }
        }
        private bool IsMoveAble(string Action, int y, int x)
        {
            if (area[y, x] is Wall)
            {
                Console.WriteLine("Стена! Куда прешь?!!\n");
                isLastActionAble = false;
                return false;
            }
            else if (area[y, x] is Stair)
            {
                if (Action == "w")
                {
                    IsNextCellGoldBar(y - 1, x);
                }
                if (Action == "s")
                {
                    IsNextCellGoldBar(y + 1, x);
                }
                Console.WriteLine("Перешел по лестнице\n");
                isLastActionAble = true;
                area.StepCount++;
                return true;
            }
            else if (area[y, x] is Empty || area[y, x] is GoldBar)
            {
                IsNextCellGoldBar(y, x);
                isLastActionAble = true;
                area.StepCount++;
                return true;
            }
            else
            {
                isLastActionAble = false; return false;
            }
        }
        private bool IsPitAble(string Action, int y, int x)
        {
            if (x == 0 || x == area.AreaWidth - 1) { isLastActionAble = false; return false;  }
            else if (area[y, x] is Stair) { isLastActionAble = false; return false; }
            isLastActionAble = true;
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
