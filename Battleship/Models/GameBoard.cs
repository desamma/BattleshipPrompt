using System;
using System.Collections.Generic;

namespace Battleship.Models
{
    public class GameBoard
    {
        public CellState[,] Cells { get; set; }
        public List<Ship> Ships { get; set; }

        public GameBoard()
        {
            Cells = new CellState[7, 7];
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    Cells[i, j] = new CellState();
                }
            }
            Ships = new List<Ship>();
        }
    }
}
