using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Field_Sotnikov
{
    abstract class Cell : ICloneable
    {
        public char CellName { get; protected set;   }
        public int X { get; set; }
        public int Y { get; set; }
        public abstract bool IsThrough { get; }
        public virtual int PointsForCell { 
            get => 0;
        }
        public virtual object Clone()
        {
            return MemberwiseClone();
        }
        public Cell(int y=0, int x = 0)
        {
            CellName = ' ';
            X = x;
            Y = y;
        }
    }
    class Player : Cell, ICloneable
    {
        public Player(int y, int x) : base(y, x) { 
            CellName = 'I'; 
        }
        public override bool IsThrough => true;
        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
    class Empty : Cell, ICloneable
    {
        public Empty(int y = 0, int x = 0) : base(y, x)
        {
            
        }
        public override object Clone()
        {
            return MemberwiseClone();
        }

        public override bool IsThrough => true;
    }
    class Wall : Cell, ICloneable
    {
        public Wall(int y = 0, int x = 0) : base(y, x)
        {
           
            CellName = '#';
        }
        public override object Clone()
        {
            return MemberwiseClone();
        }
        public override bool IsThrough => false;
    }
    class Stair : Cell, ICloneable
    {
        public Stair(int y = 0, int x = 0) : base(y, x)
        {
            CellName = '|';
        }
        public override object Clone()
        {
            return MemberwiseClone();
        }
        public override bool IsThrough => true;
    }
    class GoldBar : Cell, ICloneable
    {
        public GoldBar(int y = 0, int x = 0) : base(y, x) { 
            CellName = '@'; 
        }
        public override bool IsThrough => true;
        public override int PointsForCell => 1;
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
    class Arrow : Cell, ICloneable
    {
        public Arrow(int x = 0, int y = 0) : base(y, x)
        {
            CellName = ' ';
        }
        public override bool IsThrough => true;
        public override object Clone()
        {
            return MemberwiseClone();
        }

    }
    class Teleport : Cell, ICloneable
    {
        public Teleport(int y = 0,int x = 0) : base(y, x)
        {
            CellName = '0';
        }
        public override bool IsThrough => true;
        public override object Clone()
        {
            return MemberwiseClone();
        }   
    }
}
