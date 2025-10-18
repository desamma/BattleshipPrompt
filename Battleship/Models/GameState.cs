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
            Message = "Player's turn.";
            IsGameOver = false;
        }
    }
}
