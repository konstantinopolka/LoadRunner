using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Field_Sotnikov
{
    delegate void delegateFor2Ints(int y, int x);
    class Engine
    {
        private delegateFor2Ints checkOfNextCell;
        private Dictionary<char, Tuple<int, int>> dicMoves;
        private bool isPlaying;
        private Area area;
        private bool isThereThePit;
        private Cell lastPit;
        private bool isLastActionPossible = false;
        private Stopwatch stopwatch;
        private int points;
        private int teleports;
        private List<string> lastActionComments;
        private char actionChar { get; set; }
        private ConsoleKeyInfo action;
        private Cell lastCell;

        private List<string> rulesOfGame;
        public Engine(int AreaHeigth = 10, int AreaWidth = 10, int MaxStairs = 4, int MaxGoldBars = 5, int Teleports=1)
        {
            stopwatch = new Stopwatch();
            area = new Area(AreaHeigth, AreaWidth, MaxStairs, MaxGoldBars, Teleports);
            isPlaying = true;
            dicMoves = new Dictionary<char, Tuple<int, int> >() // Item1 - y, Item2 - x
            {
                {'w', Tuple.Create(-1, 0)},
                {'s', Tuple.Create(1, 0) },
                {'d', Tuple.Create(0, 1) },
                {'a', Tuple.Create(0, -1)},
                {'z', Tuple.Create(1, -1)},
                {'x', Tuple.Create(1, 1) },
            };
            lastCell = new Empty(area.PlayerY, area.PlayerX);
            checkOfNextCell += isNextCellGoldBar;
            checkOfNextCell += isNextCellTeleport;
            rulesInitialization();
            lastActionCommentsInitialization();

        }
        public Engine(int n = 10, int MaxStairs = 4, int MaxGoldBars = 5, int Teleports=1) : this(n,n, MaxStairs, MaxGoldBars, Teleports)
        {
           
        }
        public Engine():this(10,10,4,5,1)
        {
           
        }
        public Engine(int n):this(n,n,n,n,1) 
        {
            
        }
        public Engine(int AreaHeigth = 10, int AreaWidth = 10) : this(AreaHeigth, AreaWidth, AreaHeigth, AreaHeigth, AreaHeigth/10)
        {

        }
        private void lastActionCommentsInitialization()
        {
            lastActionComments = new List<string>();
            lastActionComments.Add("Тут будуть коментарії твого останнього кроку\n");
        }
        private void showlastActionComments()
        {
            lastActionComments.Add("\n");
            for (int i = 0; i < lastActionComments.Count; i++)
                Console.Write(lastActionComments[i]);
        }
        private void rulesInitialization()
        {
            rulesOfGame = new List<string>();
            rulesOfGame.Add("w -\t подняться по лестнице вверх на ряд вверх\n");
            rulesOfGame.Add("s -\t опуститься по лестнице вниз на ряд ниже\n");
            rulesOfGame.Add("d -\t пройти вправо на одну клетку\n");
            rulesOfGame.Add("a -\t пройти влево на одну клетку\n");
            rulesOfGame.Add("z -\t выкопать яму слева\n");
            rulesOfGame.Add("x -\t выкопать яму справа\n");
            rulesOfGame.Add("Не копать яму на дне!!! иначе GAME OVER\n");
            rulesOfGame.Add("Для паузы нажмите Esc\n");
            rulesOfGame.Add("\n");
            rulesOfGame.Add("\n");
        }
        private void showRules()
        {
            for(int i = 0; i < rulesOfGame.Count; i++)
                Console.Write(rulesOfGame[i]);
        }
        
        private void pauseText()
        {
            Console.Clear();
            Console.SetCursorPosition(60, 15);
            Console.Write("Pause\n");
            Console.CursorLeft = 52;
            Console.Write("Please enter Esc again\n");
        }
        private void pause()
        {
            pauseText();
            while (Console.ReadKey(true).Key!=ConsoleKey.Escape)
                pauseText();
        }
        private void showGame()
        {
            showRules();
            

            area.ShowArea();
            for (int i = 0; i < area.AreaHeight; i++)
            {
                Console.SetCursorPosition(area.AreaWidth + 4, i + rulesOfGame.Count);
                if (i == 0) Console.Write("Points:\t\t" + points);
                if (i == 1) Console.Write("Teleports:\t" + teleports);
                if (i == 2) Console.Write("Steps count:\t" + area.StepCount);
                if (i == 3) Console.Write("Time:\t\t" + stopwatch.Elapsed);
                Console.WriteLine();
            }


            Console.SetCursorPosition (0, area.AreaHeight+rulesOfGame.Count+2);
            Console.WriteLine("\n");
            


            showlastActionComments();
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
                if (area[newPlayerY, newPlayerX] is Passable) {
                    area.PlayerY = newPlayerY;
                    area.PlayerX= newPlayerX;
                    checkOfNextCell(area.PlayerY, area.PlayerX);
                    isPLayerThere = true;
                    lastCell = (Cell)area[area.PlayerY, area.PlayerX].Clone();
                }
            }
            teleports--;
        }
        public void Playing()
        {
            while (isPlaying)
            {          
                showGame();
                
                action = Console.ReadKey(true);
                if (action.Key == ConsoleKey.Escape)
                    pause();
                actionChar = action.KeyChar;
                Console.WriteLine("\n");

                stopwatch.Start();
                if (actionChar == 'w' || actionChar == 'a' || actionChar == 'd' || actionChar == 's' || actionChar == ' ')
                {
                    if(actionChar == 'w' || actionChar == 'a' || actionChar == 'd' || actionChar == 's')
                    {
                        lastActionComments.Add(area[area.PlayerY + dicMoves[actionChar].Item1, area.PlayerX + dicMoves[actionChar].Item2].LastCellComment);

                        if (isMoveAble(actionChar, area.PlayerY + dicMoves[actionChar].Item1, area.PlayerX + dicMoves[actionChar].Item2))
                        {
                            isLastActionPossible = true;
                            area.StepCount++;

                            if(lastCell is Stair)
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
                        lastActionComments.Add( area[area.PlayerY , area.PlayerX].LastCellComment);
                        checkOfNextCell(area.PlayerY, area.PlayerX);
                    }

                    if (isThereThePit && isLastActionPossible)
                    {
                        area[lastPit.Y, lastPit.X] = new Wall(lastPit.Y, lastPit.X);
                        isThereThePit = false;
                    }
                } 
                if ( (actionChar == 'z' || actionChar == 'x') &&
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

               
                Console.Clear();
                if(area.UserGoldBars==points) {
                    for (int i = 0; i < 1000; i++) Console.WriteLine("Victory!!!");
                    stopwatch.Stop();
                    isPlaying=false;
                }
                if (area.PlayerY == area.AreaHeight - 1) {
                    for (int i = 0; i < 1000; i++) Console.WriteLine("GAME OVER"); 
                    stopwatch.Stop();
                    isPlaying = false;
                } // проверка на выпадание под поле
            }
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
            {
                points++;
            }
        } 
        private void isNextCellTeleport(int y, int x)
        {
            if (area[y, x] is Teleport)
            {
                teleports++;
            }
        }
    }
}
