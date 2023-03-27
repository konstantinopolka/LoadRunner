using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Field_Sotnikov
{
    class Area
    {
        public int points { get; set; }
        private int maxStairs = 2;
        private int maxGoldBars = 5;
        private Cell[,] m_cells;
        public int AreaHeigth { get; private set; }
        public int AreaWidth { get; private set; }
        public int PlayerX { get; set; } //  сет должно быть приват
        public int PlayerY { get; set; } // сет должно быть приват

        public Area(int AreaHeigth = 10, int AreaWidth = 10, int MaxStairs = 4, int MaxGoldBars = 5)
        {
            maxGoldBars = MaxGoldBars;
            maxStairs = MaxStairs;
            m_cells = new Cell[AreaHeigth, AreaWidth];
            this.AreaHeigth = AreaHeigth;
            this.AreaWidth = AreaWidth;
            FillArea();
        }
        public Area(int n = 10, int MaxStairs = 4, int MaxGoldBars = 5) : this(n, n, MaxStairs, MaxGoldBars)
        {
        }
        public Area()
        {
            int AreaHeigth = 10;
            int AreaWidth = 10;
            int MaxStairs = 4;
            int MaxGoldBars = 5;
            maxGoldBars = MaxGoldBars;
            maxStairs = MaxStairs;
            m_cells = new Cell[AreaHeigth, AreaWidth];
            this.AreaHeigth = AreaHeigth;
            this.AreaWidth = AreaWidth;
            FillArea();
        }
        public void ShowArea()
        {
            for (int i = 0; i < m_cells.GetLength(0); i++)
            {
                for (int j = 0; j < m_cells.GetLength(1); j++) Console.Write(m_cells[i, j].CellName);
                if (i == 0) Console.Write("\t\tPoints:\t" + points);
                Console.WriteLine("");

            }
            Console.WriteLine("\n");
        }
        private void FillArea()
        {
            Random rnd = new Random();
            int[] ArrayForPlayer = new int[m_cells.GetLength(0) / 2];

            if (AreaHeigth % 2 == 0)
            {
                for (int i = 0; i < m_cells.GetLength(0); i++)
                    for (int j = 0; j < m_cells.GetLength(1); j++)
                        m_cells[i, j] = new Empty(i, j);

                for (int i = 0; i < ArrayForPlayer.Length; i++) ArrayForPlayer[i] = i * 2;
                int RndIntForCells;
                for (int i = 0; i < m_cells.GetLength(0); i++)
                {
                    for (int j = 0; j < m_cells.GetLength(1); j++)
                    {
                        if (Player.Count < 1)
                        {
                            RndIntForCells = rnd.Next(1, m_cells.GetLength(1) - 1);
                            m_cells[/*ArrayForPlayer[rnd.Next(0, ArrayForPlayer.Length)]*/0, RndIntForCells] = new Player(0, RndIntForCells);
                            PlayerY = 0;
                            PlayerX = RndIntForCells;
                        }
                        if (j == 0 || j == m_cells.GetLength(1) - 1 || i % 2 == 1) m_cells[i, j] = new Wall(i, j);
                    }
                }
                for (int i = 0, j; i < m_cells.GetLength(0); i++)
                {
                    j = rnd.Next(1, m_cells.GetLength(1) - 1);
                    if (m_cells[i, j] as Stair == null && Stair.Count < maxStairs && i % 2 == 1) m_cells[i, j] = new Stair(i, j);
                    j = rnd.Next(1, m_cells.GetLength(1) - 1);
                    if (m_cells[i, j] as Player == null && m_cells[i, j] as GoldBar == null && GoldBar.Count <= maxGoldBars && i % 2 == 0) m_cells[i, j] = new GoldBar(i, j);

                }
            }
        }
        public Cell this[int y, int x]
        {
            get
            {
                if (x >= 0 && y >= 0) return m_cells[y, x];
                else
                {
                    Console.WriteLine("Пожалуйста, введите корректные координаты\n\n");  // переделать под исключение потом
                    return null;
                }

            }
            set { m_cells[y, x] = value; }  // поставить исключение на подачу ссылки другого класса
        }
    }
}
