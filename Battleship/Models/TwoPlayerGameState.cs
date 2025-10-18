namespace Battleship.Models
{
    public class TwoPlayerGameState
    {
        public GameBoard Player1Board { get; set; }
        public GameBoard Player2Board { get; set; }
        public bool IsPlayer1Turn { get; set; }
        public string Message { get; set; }
        public bool IsGameOver { get; set; }

        public TwoPlayerGameState()
        {
            Player1Board = new GameBoard();
            Player2Board = new GameBoard();
            IsPlayer1Turn = true;
            Message = "Player 1's turn.";
            IsGameOver = false;
        }
    }
}
