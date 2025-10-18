
using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleship.Models
{
    public class GameBoard
    {
        public const int Size = 7;
        public CellState[,] Cells { get; private set; }
        public List<Ship> Ships { get; private set; }

        public GameBoard()
        {
            Cells = new CellState[Size, Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Cells[i, j] = CellState.Water;
                }
            }
            Ships = new List<Ship>();
        }

        public bool PlaceShip(Ship ship, int row, int col, bool isHorizontal)
        {
            if (CanPlaceShip(ship, row, col, isHorizontal))
            {
                ship.Row = row;
                ship.Col = col;
                ship.IsHorizontal = isHorizontal;

                for (int i = 0; i < ship.Size; i++)
                {
                    if (isHorizontal)
                    {
                        Cells[row, col + i] = CellState.Ship;
                    }
                    else
                    {
                        Cells[row + i, col] = CellState.Ship;
                    }
                }
                Ships.Add(ship);
                return true;
            }
            return false;
        }

        private bool CanPlaceShip(Ship ship, int row, int col, bool isHorizontal)
        {
            if (isHorizontal)
            {
                if (col + ship.Size > Size) return false;
                for (int i = 0; i < ship.Size; i++)
                {
                    if (Cells[row, col + i] == CellState.Ship) return false;
                }
            }
            else
            {
                if (row + ship.Size > Size) return false;
                for (int i = 0; i < ship.Size; i++)
                {
                    if (Cells[row + i, col] == CellState.Ship) return false;
                }
            }
            return true;
        }

        public CellState Attack(int row, int col)
        {
            if (Cells[row, col] == CellState.Hit || Cells[row, col] == CellState.Miss)
            {
                return CellState.Occupied;
            }

            if (Cells[row, col] == CellState.Ship)
            {
                Cells[row, col] = CellState.Hit;
                var ship = Ships.First(s => s.IsAt(row, col));
                ship.Hits++;
                return CellState.Hit;
            }

            Cells[row, col] = CellState.Miss;
            return CellState.Miss;
        }

        public bool AllShipsSunk()
        {
            return Ships.All(s => s.IsSunk());
        }

        public string GetCellClass(int row, int col, bool isEnemy = false)
        {
            var state = Cells[row, col];
            switch (state)
            {
                case CellState.Hit:
                    return "hit";
                case CellState.Miss:
                    return "miss";
                case CellState.Ship:
                    return isEnemy ? "water" : "ship"; 
                default:
                    return "water";
            }
        }
    }
}
