using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Field_Sotnikov
{
    abstract class Cell
    {
        public char CellName { get; protected set;   }
        public int X { get; set; }
        public int Y { get; set; }
        public Cell(int y, int x)
        {

            CellName = ' ';
            X = x;
            Y = y;
        }
    }
    class Player : Cell
    {
        public Player(int y, int x) : base(y, x) { CellName = 'I'; }
    }
    class Empty : Cell
    {
        public Empty(int y, int x) : base(y, x)
        {
            
        }
    }
    class Wall : Cell
    {
       
        public Wall(int y, int x) : base(y, x)
        {
           
            CellName = '#';
        }
    }
    class Stair : Cell
    {
        public Stair(int y, int x) : base(y, x)
        {
            CellName = '|';
        }
    }
    class GoldBar : Cell
    {
        public GoldBar(int y, int x) : base(y, x) { CellName = '@'; }
    }
    class Arrow : Cell
    {
        public Arrow(int x, int y) : base(y, x)
        {
            CellName = ' ';
        }

    }
    class Teleport : Cell
    {
        public Teleport(int y,int x) : base(y, x)
        {
            CellName = '0';
        }
    }
}
