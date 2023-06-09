using System;

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
        public abstract object Clone();
        
        public abstract string LastCellComment { get; }
        public Cell(int y=0, int x = 0)
        {
            CellName = ' ';
            X = x;
            Y = y;
        }
    }

    abstract class Passable : Cell, ICloneable
    {
        public override bool IsThrough => true;
        public Passable(int y = 0, int x = 0)
        {
            CellName = ' ';
            X = x;
            Y = y;
        }
    }
    
    abstract class Unpassable : Cell, ICloneable
    {
        public override bool IsThrough => false;
        public Unpassable(int y = 0, int x = 0)
        {
            CellName = ' ';
            X = x;
            Y = y;
        }
    }
    class Player : Passable, ICloneable
    {
        public Player(int y=0, int x = 0) : base(x,y) { 
            CellName = 'I'; 
        }
       
        
        public override object Clone() => MemberwiseClone();

        public override string LastCellComment => "";

    }
    class Empty : Passable, ICloneable
    {
        public Empty(int y = 0, int x = 0) : base(y, x)
        {
            
        }
        public override object Clone() => MemberwiseClone();

       
        public override string LastCellComment => "Просто пройшовся\n";

    }
    class Wall : Unpassable, ICloneable
    {
        public Wall(int y = 0, int x = 0) : base(y, x)
        {
            CellName = '#';
        }
        public override object Clone() => MemberwiseClone();
        public override string LastCellComment => "Стіна!\n";
    }
    class Stair : Passable, ICloneable
    {
        public Stair(int y = 0, int x = 0) : base(y, x)
        {
            CellName = '|';
        }
        public override object Clone() => MemberwiseClone();
       
        public override string LastCellComment => "Перейшов по драбині\n";
    }
    class GoldBar : Passable, ICloneable
    {
        public GoldBar(int y = 0, int x = 0) : base(y, x) { 
            CellName = '@'; 
        }
        public override int PointsForCell => 1;
        public override object Clone() => MemberwiseClone();
        public override string LastCellComment => "Зібрав золотий зливок\n";
    }
    class Arrow : Passable, ICloneable
    {
        public Arrow(int x = 0, int y = 0) : base(y, x)
        {
            CellName = ' ';
        }

        public override object Clone() => MemberwiseClone();
        public override string LastCellComment => " ";

    }
    class Teleport : Passable, ICloneable
    {
        public Teleport(int y = 0,int x = 0) : base(y, x)
        {
            CellName = '0';
        }
        public override object Clone() => MemberwiseClone();
        public override string LastCellComment => "Зібрав телепорт\n";
    }
}
