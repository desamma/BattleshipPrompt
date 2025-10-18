using System;

namespace Battleship.Models
{
    public class GameState
    {
        public GameBoard PlayerBoard { get; set; }
        public GameBoard ComputerBoard { get; set; }
        public bool IsPlayerTurn { get; set; }
        public string Message { get; set; }
        public bool IsGameOver { get; set; }

        public GameState()
        {
            PlayerBoard = new GameBoard();
            ComputerBoard = new GameBoard();
            IsPlayerTurn = true;
            Message = "Make your move.";
            IsGameOver = false;

            InitializeBoards();
        }

        private void InitializeBoards()
        {
            // Simplified ship placement for demonstration
            PlayerBoard.PlaceShip(new Ship { Size = 3 }, 0, 0, true);
            PlayerBoard.PlaceShip(new Ship { Size = 4 }, 2, 1, false);

            ComputerBoard.PlaceShip(new Ship { Size = 3 }, 1, 1, true);
            ComputerBoard.PlaceShip(new Ship { Size = 4 }, 3, 0, false);
        }
    }
}
