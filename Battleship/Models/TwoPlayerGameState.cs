using System.Collections.Generic;

namespace Battleship.Models
{
    public class TwoPlayerGameState
    {
        public GameBoard Player1Board { get; set; }
        public GameBoard Player2Board { get; set; }
        public bool IsPlayer1Turn { get; set; }
        public string Message { get; set; }
        public bool IsGameOver { get; set; }
        public bool IsShipPlacementPhase { get; set; }
        public List<int> Player1ShipsToPlace { get; set; }
        public List<int> Player2ShipsToPlace { get; set; }

        public TwoPlayerGameState()
        {
            Player1Board = new GameBoard();
            Player2Board = new GameBoard();
            IsPlayer1Turn = true;
            Message = "Player 1, place your ships.";
            IsGameOver = false;
            IsShipPlacementPhase = true;
            Player1ShipsToPlace = new List<int> { 3, 2, 1 };
            Player2ShipsToPlace = new List<int> { 3, 2, 1 };
        }
    }
}
