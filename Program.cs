using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Field_Sotnikov
{
    class Area
    {
        public Cell[,] m_cells;
        private int m_AreaHeigth;
        private int m_AreaWidth;

        public Area(int AreaHeigth = 11, int AreaWidth = 10)
        {
            m_cells = new Cell[AreaHeigth, AreaWidth];
            m_AreaHeigth = AreaHeigth;
            m_AreaWidth = AreaWidth;
            for (int i = 0; i < m_cells.GetLength(0); i++)
            {
                for (int j = 0; j < m_cells.GetLength(1); j++)
                {
                    if (i % 2 == 0 || j == 0 || j == m_cells.GetLength(1) - 1)
                    {
                        m_cells[i, j] = new Wall();
                    }
                    else m_cells[i, j] = new Empty();
                }
            }
            FillArea();
        }
        public void ShowArea()
        {
            for (int i = 0; i < m_cells.GetLength(0); i++)
            {
                for (int j = 0; j < m_cells.GetLength(1); j++) Console.Write(m_cells[i, j].CellName);
                Console.WriteLine();
            }
        }
        private void UpdateArea()
        {

        }
        private void FillArea()
        {
            for (int i = 0; i < m_cells.GetLength(0); i++)
            {
                for (int j = 0; j < m_cells.GetLength(1); j++)
                {
                    if (i % 2 == 1) m_cells[i, j].CellName = '#';
                }
            }
        }
        /*public int Count()
        {
            return m_cells.Count;   // 
                                    // 
                                    // 
        }*/

    }
    abstract class Cell
    {
        private static int cell_count;
        public char CellName { get; set; }
        public virtual int Count { get => cell_count; set { cell_count = value; } }
        private int m_x;
        private int m_y;
        public Cell()
        {
            CellName = '0';
            m_x = 0;
            m_y = 0;
            Count++;
        }
    }
    class Player : Cell
    {
        private static int player_count;
        public override int Count { get => player_count; set { player_count = value; } }
        public Player() : base()
        {
            CellName = 'I';
            Count++;
        }
    }
    class Empty : Cell
    {
        private static int empty_count;
        public override int Count { get => empty_count; set { empty_count = value; } }
        public Empty() : base()
        {
            Count++;
        }
    }
    class Wall : Cell
    {
        private static int wall_count;
        public override int Count { get => wall_count; set { wall_count = value; } }
        public Wall()
        {
            Count++;
            CellName = '#';
        }
    }
    class Stair : Cell
    {
        private static int stair_count;
        public override int Count { get => stair_count; set { stair_count = value; } }
        public Stair()
        {
            CellName = '|';
            Count++;
        }
    }
    class GoldBar : Cell
    {
        private static int goldBar_count;
        public override int Count { get => goldBar_count; set { goldBar_count = value; } }
        public GoldBar()
        {
            CellName = '@';
            Count++;
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {


            Area area = new Area();
            area.ShowArea();
        }
    }
}
