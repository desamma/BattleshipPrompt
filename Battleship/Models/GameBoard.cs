
using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleship.Models
{
    public class GameBoard
    {
        public const int Size = 7;
        public List<List<CellState>> Cells { get; set; }
        public List<Ship> Ships { get; set; }

        public GameBoard()
        {
            Cells = new List<List<CellState>>(Size);
            for (int i = 0; i < Size; i++)
            {
                var row = new List<CellState>(Size);
                for (int j = 0; j < Size; j++)
                {
                    row.Add(CellState.Water);
                }
                Cells.Add(row);
            }
            Ships = new List<Ship>();
        }

        public void RandomizeShipPlacement()
        {
            var random = new Random();
            var shipConfigs = new[]
            {
                new { Size = 4, ImagePath = "/images/Carrier/ShipCarrierHull.png" },
                new { Size = 3, ImagePath = "/images/Destroyer/ShipDestroyerHull.png" },
                new { Size = 2, ImagePath = "/images/PatrolBoat/ShipPatrolHull.png" }
            };

            foreach (var config in shipConfigs)
            {
                var ship = new Ship { Size = config.Size, ImagePath = config.ImagePath };
                bool placed = false;
                while (!placed)
                {
                    var isHorizontal = random.Next(2) == 0;
                    var row = random.Next(Size);
                    var col = random.Next(Size);

                    if (CanPlaceShip(ship, row, col, isHorizontal))
                    {
                        PlaceShip(ship, row, col, isHorizontal);
                        placed = true;
                    }
                }
            }
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
                        Cells[row][col + i] = CellState.Ship;
                    }
                    else
                    {
                        Cells[row + i][col] = CellState.Ship;
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
                    if (Cells[row][col + i] == CellState.Ship) return false;
                }
            }
            else
            {
                if (row + ship.Size > Size) return false;
                for (int i = 0; i < ship.Size; i++)
                {
                    if (Cells[row + i][col] == CellState.Ship) return false;
                }
            }
            return true;
        }

        public CellState Attack(int row, int col)
        {
            if (Cells[row][col] == CellState.Hit || Cells[row][col] == CellState.Miss)
            {
                return CellState.Occupied;
            }

            var ship = Ships.FirstOrDefault(s => s.IsAt(row, col));
            if (ship != null)
            {
                Cells[row][col] = CellState.Hit;
                ship.Hits++;
                return CellState.Hit;
            }

            Cells[row][col] = CellState.Miss;
            return CellState.Miss;
        }

        public bool AllShipsSunk()
        {
            return Ships.All(s => s.IsSunk());
        }

        public CellDisplay GetCellDisplay(int row, int col, bool isEnemy = false)
        {
            var display = new CellDisplay();
            var state = Cells[row][col];

            display.BackgroundClass = "water";

            switch (state)
            {
                case CellState.Hit:
                    display.MarkerClass = "hit";
                    break;
                case CellState.Miss:
                    display.MarkerClass = "miss";
                    break;
                case CellState.Ship:
                    if (!isEnemy)
                    {
                        var ship = Ships.FirstOrDefault(s => s.IsAt(row, col));
                        if (ship != null && !string.IsNullOrEmpty(ship.ImagePath))
                        {
                            var imageName = ship.ImagePath.Split('/').Last().Split('.').First();
                            var rotationClass = ship.IsHorizontal ? "rotate-90" : "";
                            var segment = ship.IsHorizontal ? col - ship.Col : row - ship.Row;
                            display.ShipClass = $"ship {imageName} {rotationClass} ship-segment-{segment}";
                        }
                    }
                    break;
            }

            return display;
        }
    }
}
