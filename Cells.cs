using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Field_Sotnikov
{
    abstract class Cell
    {
        private static int cell_count; // должно приват
        public char CellName { get; set; }
        public static int Count { get => cell_count; protected set { cell_count = value; } }
        public int X { get; set; }
        public int Y { get; set; }
        public Cell(int y, int x)
        {

            CellName = ' ';
            Count++;
            X = x;
            Y = y;
        }
    }
    class Player : Cell
    {
        private static int player_count;
        public static new int Count { get => player_count; protected set { player_count = value; } }
        public Player(int y, int x) : base(y, x)
        {
            CellName = 'I';
            Count++;
        }
    }
    class Empty : Cell
    {
        private static int empty_count;
        public static new int Count { get => empty_count; protected set { empty_count = value; } }
        public Empty(int y, int x) : base(y, x)
        {
            Count++;
        }
    }
    class Wall : Cell
    {
        private static int wall_count;
        public static new int Count { get => wall_count; protected set { wall_count = value; } }
        public Wall(int y, int x) : base(y, x)
        {
            Count++;
            CellName = '#';
        }
    }
    class Stair : Cell
    {
        private static int stair_count;
        public static new int Count { get => stair_count; protected set { stair_count = value; } }
        public Stair(int y, int x) : base(y, x)
        {
            CellName = '|';
            Count++;
        }
    }
    class GoldBar : Cell
    {
        private static int goldBar_count;
        public static new int Count { get => goldBar_count; protected set { goldBar_count = value; } }
        public GoldBar(int y, int x) : base(y, x)
        {
            CellName = '@';
            Count++;
        }
    }
    class Arrow : Cell
    {

        private static int arrow_count;
        public static new int Count { get => arrow_count; protected set { arrow_count = value; } }
        public Arrow(int x, int y) : base(x, y)
        {
            CellName = ' ';
            Count++;
        }

    }
}
