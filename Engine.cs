using LB3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace LB3
{
    //delegate void delegateFor2Ints(int y, int x);
    class Engine
    {
        private bool isThereThePit;
        private bool isLastActionPossible = false;
        
        private int points;
        private int teleports;
        public int AreaHeigth { get; private set; } 
        public int AreaWidth { get; private set; }
        private char actionChar { get; set; }

        private delegateFor2Ints checkOfNextCell;
        private Dictionary<char, Tuple<int, int>> dicMoves;
        private List<string> lastActionComments;
        public List<string> statistics { 
            get {
                return new List<string>
                    {
                    $"Points:\t\t{points}",
                    $"Teleports:\t\t{teleports}",
                    $"Steps count:\t\t{area.StepCount}",
                    $"Time:\t\t{stopwatch.Elapsed}"
                    };
            } 
        }

        private Area area;
        private Cell lastPit;
        private Cell lastCell;

        private Stopwatch stopwatch;

        public Engine(int AreaHeigth = 10, int AreaWidth = 10, int MaxStairs = 4, int MaxGoldBars = 5, int Teleports = 1)
        {
            this.AreaHeigth = AreaHeigth;
            this.AreaWidth = AreaWidth;
            stopwatch = new Stopwatch();
            area = new Area(AreaHeigth, AreaWidth, MaxStairs, MaxGoldBars, Teleports);
            dicMoves = new Dictionary<char, Tuple<int, int>>() // Item1 - y, Item2 - x
            {
                {'W', Tuple.Create(-1, 0)},
                {'S', Tuple.Create(1, 0) },
                {'D', Tuple.Create(0, 1) },
                {'A', Tuple.Create(0, -1)},
                {'Z', Tuple.Create(1, -1)},
                {'X', Tuple.Create(1, 1) },
            };
            lastCell = new Empty(area.PlayerY, area.PlayerX);
            checkOfNextCell += isNextCellGoldBar;
            checkOfNextCell += isNextCellTeleport;
            lastActionCommentsInitialization();
           
        }
        public Engine(int n = 10, int MaxStairs = 4, int MaxGoldBars = 5, int Teleports = 1) : this(n, n, MaxStairs, MaxGoldBars, Teleports)
        {

        }
        public Engine() : this(10, 10, 4, 5, 1)
        {

        }
        public Engine(int n) : this(n, n, n, n, 1)
        {

        }
        public Engine(int AreaHeigth = 10, int AreaWidth = 10) : this(AreaHeigth, AreaWidth, AreaHeigth, AreaHeigth, AreaHeigth / 10)
        {

        }

        private void showStatistics(Graphics graphics)
        {
            List<string> Statistics = this.statistics;
            int startX = (AreaWidth + 1) * Cell.Size + Cell.Size / 2;
            int startY = 0;

            Font font = new Font(FontFamily.GenericMonospace, 14);
            for (int i = 0; i < Statistics.Count; i++)
                graphics.DrawString(Statistics[i], font, Brushes.Black, startX, startY + i * Cell.Size);
        }

        private void showLastActionComments(Graphics graphics)
        {
            // Отрисовка комментариев последнего действия
            int startY = (AreaHeigth + 1) * Cell.Size;
            int startX = (AreaWidth + 1) * Cell.Size + Cell.Size / 2;
            Font font = new Font(FontFamily.GenericMonospace, 14);
            for (int i = 0; i < lastActionComments.Count; i++)
                graphics.DrawString(lastActionComments[i], font, Brushes.Black, 0, startY);

            lastActionComments.Clear();
        }
        private void teleport()
        {
            if (lastCell is Stair)
                area[area.PlayerY, area.PlayerX] = lastCell;
            else
                area[area.PlayerY, area.PlayerX] = new Empty(area.PlayerY, area.PlayerX);

            bool isPLayerThere = false;
            Random rnd = new Random();
            while (!isPLayerThere)
            {
                int newPlayerX = rnd.Next(0, area.AreaWidth);
                int newPlayerY = rnd.Next(0, area.AreaHeight);
                if (area[newPlayerY, newPlayerX] is Passable)
                {
                    area.PlayerY = newPlayerY;
                    area.PlayerX = newPlayerX;
                    checkOfNextCell(area.PlayerY, area.PlayerX);
                    isPLayerThere = true;
                    lastCell = (Cell)area[area.PlayerY, area.PlayerX].Clone();
                }
            }
            teleports--;
        }
        private bool isMoveAble(char Action, int y, int x)
        {
            if (area[y, x] is Passable)
                return true;
            else
                return false;
        }
        private bool isPitAble(char Action, int y, int x)
        {
            if (x == 0 || x == area.AreaWidth - 1 || area[y, x] is Passable)
                return false;
            else
                return true;
        }
        private void isNextCellGoldBar(int y, int x)
        {
            if (area[y, x] is GoldBar)
                points++;
        }
        private void isNextCellTeleport(int y, int x)
        {
            if (area[y, x] is Teleport)
                teleports++;
        }
        public void DrawGame(Graphics graphics)
        {

            area.DrawArea(graphics);
            showStatistics(graphics);
            showLastActionComments(graphics);
        }
        private void lastActionCommentsInitialization()
        {
            lastActionComments = new List<string>();
            lastActionComments.Add("Тут будуть коментарії твого останнього кроку\n");
        }
        public void HandleKeyPress(Keys keyCode, ref bool isPlaying)
        {
            actionChar = (char)keyCode;

            stopwatch.Start();
            if (actionChar == 'W' || actionChar == 'A' || actionChar == 'D' || actionChar == 'S' || actionChar == ' ')
            {
                if (actionChar == 'W' || actionChar == 'A' || actionChar == 'D' || actionChar == 'S')
                {
                    lastActionComments.Add(area[area.PlayerY + dicMoves[actionChar].Item1, area.PlayerX + dicMoves[actionChar].Item2].LastCellComment);

                    if (isMoveAble(actionChar, area.PlayerY + dicMoves[actionChar].Item1, area.PlayerX + dicMoves[actionChar].Item2))
                    {
                        isLastActionPossible = true;
                        area.StepCount++;

                        if (lastCell is Stair)
                            area[area.PlayerY, area.PlayerX] = lastCell;
                        else
                            area[area.PlayerY, area.PlayerX] = new Empty(area.PlayerY, area.PlayerX);

                        area.PlayerX += dicMoves[actionChar].Item2;
                        area.PlayerY += dicMoves[actionChar].Item1;

                        checkOfNextCell(area.PlayerY, area.PlayerX);

                        lastCell = (Cell)area[area.PlayerY, area.PlayerX].Clone();

                    }
                    else
                        isLastActionPossible = false;
                }
                if (actionChar == ' ')
                {
                    if (teleports > 0)
                    {
                        lastActionComments.Add("Телепортнувся\n");
                        isLastActionPossible = true;
                        teleport();
                    }
                    else
                        lastActionComments.Add("У тебя немає телепорті\n");
                }

                while (area[area.PlayerY + 1, area.PlayerX] is Passable &&
                       !(area[area.PlayerY + 1, area.PlayerX] is Stair) &&
                       area.PlayerY + 1 != area.AreaHeight &&
                       !(lastCell is Stair)
                       )
                {
                    area.PlayerY++;
                    lastActionComments.Add(area[area.PlayerY, area.PlayerX].LastCellComment);
                    checkOfNextCell(area.PlayerY, area.PlayerX);
                }

                if (isThereThePit && isLastActionPossible)
                {
                    area[lastPit.Y, lastPit.X] = new Wall(lastPit.Y, lastPit.X);
                    isThereThePit = false;
                }
            }
            if ((actionChar == 'Z' || actionChar == 'X') &&
                isPitAble(actionChar, area.PlayerY + dicMoves[actionChar].Item1, area.PlayerX + dicMoves[actionChar].Item2))
            {
                lastActionComments.Add("Зробив яму\n");
                if (isThereThePit)
                    area[lastPit.Y, lastPit.X] = new Wall(lastPit.Y, lastPit.X);

                isThereThePit = true;
                area[area.PlayerY + dicMoves[actionChar].Item1, area.PlayerX + dicMoves[actionChar].Item2] = new Empty(area.PlayerY + dicMoves[actionChar].Item1, area.PlayerX + dicMoves[actionChar].Item2);
                area.StepCount++;
                lastPit = new Empty(area.PlayerY + dicMoves[actionChar].Item1, area.PlayerX + dicMoves[actionChar].Item2);
                isLastActionPossible = true;
            }
            else
                isLastActionPossible = false;

            area[area.PlayerY, area.PlayerX] = new Player(area.PlayerY, area.PlayerX);

            IsWin(ref isPlaying);
            IsLose(ref isPlaying);
        }
        private void IsWin(ref bool isPlaying)
        {
            if (area.UserGoldBars == points) {
                isPlaying = false;
                Win();
            }
        }
        private void Win() { }
       
        private void IsLose(ref bool isPlaying)
        {
            if(area.PlayerY == area.AreaHeight - 1)
            {
                isPlaying = false;
                Lose();
            }
        }
        private void Lose() { }
        
    }
}
