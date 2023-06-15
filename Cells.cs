using System;
using System.Drawing;

namespace LB3
{
    abstract class Cell : ICloneable // загальний клас для усіх клітинок
    {
        public Cell(int y = 0, int x = 0) //  загальний конструктор для усіх клітинок
        {
            CellName = ' ';
            X = x;
            Y = y;
        }
        public static int Size { get => 50; } // розмір клітинки на моніторі
        public int X { get; set; } // координата клітинки за віссю абсцис
        public int Y { get; set; } // координата клітинки за віссю ординат

        public abstract bool IsThrough { get; } // характеризує чи може гравець пройти через клітинку
        public virtual int PointsForCell // кількість очок, які отримує гравець при попаданні на клітинку
        {
            get => 0;
        }
        public char CellName { get; protected set; } // ім'я клітинки
        public abstract object Clone(); // загальний метод для побітового копіювання клітинки

        public abstract string LastCellComment { get; } // коментар, який виводиться при попаданні на клітинку

        public abstract void Draw(Graphics graphics); // загальний метод для малювання клітинки у формі

        internal Area Area
        {
            get => default;
            set
            {
            }
        }
    }

    abstract class Passable : Cell, ICloneable // субклас для проходимих клітинок
    {
        public override bool IsThrough => true;
        public override void Draw(Graphics graphics)
        {
            graphics.FillRectangle(Brushes.LightGray, X * Size, Y * Size, Size, Size);
            graphics.DrawRectangle(Pens.Black, X * Size, Y * Size, Size, Size);
        }
        public Passable(int y = 0, int x = 0)
        {
            CellName = ' ';
            X = x;
            Y = y;
        }
    }
    abstract class Unpassable : Cell, ICloneable // субклас для непроходимих клітинок
    {
        public override bool IsThrough => false;
        public override void Draw(Graphics graphics)
            => graphics.FillRectangle(Brushes.Black, X * Size, Y * Size, Size, Size);
        public Unpassable(int y = 0, int x = 0)
        {
            CellName = ' ';
            X = x;
            Y = y;
        }
    }
    class Player : Passable, ICloneable // клас для гравця
    {
        public Player(int y = 0, int x = 0) : base(y, x) => CellName = 'I';
        public override string LastCellComment => "";
        public override void Draw(Graphics graphics)
        {
            graphics.FillRectangle(Brushes.Red, X * Size, Y * Size, Size, Size);
            graphics.DrawRectangle(Pens.Black, X * Size, Y * Size, Size, Size);

        }
        public override object Clone() => MemberwiseClone();
    }
    class Empty : Passable, ICloneable // клас для порожньої клітинки
    {
        public Empty(int y = 0, int x = 0) : base(y, x)
        {

        }
        public override string LastCellComment => "Просто пройшовся\n";
        public override object Clone() => MemberwiseClone();
    }
    class Wall : Unpassable, ICloneable // клас для стіни
    {
        public Wall(int y = 0, int x = 0) : base(y, x)  => CellName = '#';
        public override string LastCellComment => "Стіна!\n";
        public override object Clone() => MemberwiseClone();
       
    }
    class Stair : Passable, ICloneable // клас для драбини
    {
        public Stair(int y = 0, int x = 0) : base(y, x) => CellName = '|';
        public override string LastCellComment => "Перейшов по драбині\n";
        public override void Draw(Graphics graphics)
        {
            graphics.FillRectangle(Brushes.SandyBrown, X * Size, Y * Size, Size, Size);
            graphics.DrawRectangle(Pens.Black, X * Size, Y * Size, Size, Size);

        }
        public override object Clone() => MemberwiseClone();  
    }
    class GoldBar : Passable, ICloneable // клас для золотого зливку
    {
        public GoldBar(int y = 0, int x = 0) : base(y, x) => CellName = '@';
        public override int PointsForCell => 1;
        public override string LastCellComment => "Зібрав золотий зливок\n";
        public override void Draw(Graphics graphics)
        {
            graphics.FillRectangle(Brushes.Gold, X * Size, Y * Size, Size, Size);
            graphics.DrawRectangle(Pens.Black, X * Size, Y * Size, Size, Size);

        }
        public override object Clone() => MemberwiseClone();
        
    }
    class Teleport : Passable, ICloneable // клас для телепорту
    {
        public Teleport(int y = 0, int x = 0) : base(y, x) => CellName = '0';
       
        public override string LastCellComment => "Зібрав телепорт\n";

        public override void Draw(Graphics graphics)
        {
            graphics.FillRectangle(Brushes.Blue, X * Size, Y * Size, Size, Size);
            graphics.DrawRectangle(Pens.Black, X * Size, Y * Size, Size, Size);

        }
        public override object Clone() => MemberwiseClone();
    }
}
