using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Field_Sotnikov
{
    class Game
    {
        private Area area;
        private bool isLastActionPit = false;
        private Cell lastPit;

        private bool isLastMoveAble = false;
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
        public void Run()
        {
            Console.WriteLine("w -\t подняться по лестнице вверх на ряд вверх\n" +
                             "s -\t опуститься по лестнице вниз на ряд ниже\n" +
                             "d -\t пройти вправо на одну клетку\n" +
                             "a -\t пройти влево на одну клетку\n" +
                             "z -\t выкопать яму слева\n" +
                             "d - \t выкопать яму справа\n\n\n");
            while (true)
            {
                area.ShowArea();
                //Thread.Sleep(1000);
                action = Console.ReadLine();
                Console.WriteLine("\n");
                switch (action)
                {

                    case "w":
                        if (isLastActionPit) { area[lastPit.Y, lastPit.X] = new Wall(lastPit.Y, lastPit.X); isLastActionPit = false; }
                        if (IsMoveAble(action, area.PlayerY - 1, area.PlayerX))
                        {
                            area[area.PlayerY - 2, area.PlayerX] = new Player(area.PlayerY - 2, area.PlayerX);
                            area[area.PlayerY, area.PlayerX] = new Empty(area.PlayerY, area.PlayerX);
                            area.PlayerY -= 2;
                        }
                        break;
                    case "d":
                        if (IsMoveAble(action, area.PlayerY, area.PlayerX + 1))
                        {
                            if (area[area.PlayerY + 1, area.PlayerX + 1] is Empty)
                            {
                                EmptyisGoldBar(area.PlayerY + 2, area.PlayerX + 1);
                                area[area.PlayerY + 2, area.PlayerX + 1] = new Player(area.PlayerY + 2, area.PlayerX + 1);
                                area[area.PlayerY, area.PlayerX] = new Empty(area.PlayerY, area.PlayerX);
                                area.PlayerX++;
                                area.PlayerY += 2;
                            }
                            else
                            {
                                area[area.PlayerY, area.PlayerX + 1] = new Player(area.PlayerY, area.PlayerX + 1);
                                area[area.PlayerY, area.PlayerX] = new Empty(area.PlayerY, area.PlayerX);
                                area.PlayerX++;
                            }
                            if (isLastActionPit) { area[lastPit.Y, lastPit.X] = new Wall(lastPit.Y, lastPit.X); isLastActionPit = false; }
                        }

                        break;
                    case "a":
                        if (IsMoveAble(action, area.PlayerY, area.PlayerX - 1))
                        {
                            if (area[area.PlayerY + 1, area.PlayerX - 1] is Empty)
                            {
                                EmptyisGoldBar(area.PlayerY + 2, area.PlayerX - 1);
                                area[area.PlayerY + 2, area.PlayerX - 1] = new Player(area.PlayerY + 2, area.PlayerX - 1);
                                area[area.PlayerY, area.PlayerX] = new Empty(area.PlayerY, area.PlayerX);
                                area.PlayerX--;
                                area.PlayerY += 2;

                            }
                            else
                            {
                                area[area.PlayerY, area.PlayerX - 1] = new Player(area.PlayerY, area.PlayerX - 1);
                                area[area.PlayerY, area.PlayerX] = new Empty(area.PlayerY, area.PlayerX);
                                area.PlayerX--;
                            }
                            if (isLastActionPit) { area[lastPit.Y, lastPit.X] = new Wall(lastPit.Y, lastPit.X); isLastActionPit = false; }
                        }

                        break;
                    case "s":
                        if (isLastActionPit) { area[lastPit.Y, lastPit.X] = new Wall(lastPit.Y, lastPit.X); isLastActionPit = false; }
                        if (IsMoveAble(action, area.PlayerY + 1, area.PlayerX))
                        {
                            area[area.PlayerY + 2, area.PlayerX] = new Player(area.PlayerY + 2, area.PlayerX);
                            area[area.PlayerY, area.PlayerX] = new Empty(area.PlayerY, area.PlayerX);
                            area.PlayerY += 2;
                        }
                        break;

                    case "z":
                        if (isLastActionPit) { area[lastPit.Y, lastPit.X] = new Wall(lastPit.Y, lastPit.X); isLastActionPit = false; }
                        if (IsPitAble(action, area.PlayerY + 1, area.PlayerX - 1))
                        {
                            area[area.PlayerY + 1, area.PlayerX - 1] = new Empty(area.PlayerY + 1, area.PlayerX - 1);
                        }
                        break;
                    case "x":
                        if (isLastActionPit) { area[lastPit.Y, lastPit.X] = new Wall(lastPit.Y, lastPit.X); isLastActionPit = false; }
                        if (IsPitAble(action, area.PlayerY + 1, area.PlayerX + 1))
                        {
                            area[area.PlayerY + 1, area.PlayerX + 1] = new Empty(area.PlayerY + 1, area.PlayerX + 1);
                        }
                        break;
                    default:
                        break;
                }
                //Console.Clear();
            }
        }
        private bool IsMoveAble(string action, int y, int x)
        {
            if (area[y, x] is Wall)
            {
                Console.WriteLine("Стена! Куда прешь?!!\n");
                isLastMoveAble = false;
                return false;
            }
            else if (area[y, x] is Stair)
            {
                if (action == "w")
                {
                    EmptyisGoldBar(y - 1, x);
                }
                if (action == "s")
                {
                    EmptyisGoldBar(y + 1, x);
                }
                Console.WriteLine("Перешел по лестнице\n");
                isLastMoveAble = true;
                return true;
            }
            else if (area[y, x] is Empty || area[y, x] is GoldBar)
            {
                EmptyisGoldBar(y, x);
                isLastMoveAble = true;
                return true;
            }
            else
            {
                isLastMoveAble = false; return false;
            }
        }
        private bool IsPitAble(string action, int y, int x)
        {
            if (x == 0 || x == area.AreaWidth - 1) return false;
            else if (area[y, x] is Stair) { return false; }
            isLastActionPit = true;
            lastPit = new Empty(y, x);
            return true;
        }
        private void EmptyisGoldBar(int y, int x)
        {
            if (area[y, x] is GoldBar) { Console.WriteLine("Скушал золото!!!"); area.points++; }
        }
    }
}
