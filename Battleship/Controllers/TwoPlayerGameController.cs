using Microsoft.AspNetCore.Mvc;
using Battleship.Models;
using System;

namespace Battleship.Controllers
{
    public class TwoPlayerGameController : Controller
    {
        private const string SessionKeyTwoPlayerGame = "_TwoPlayerGame";

        public IActionResult Index()
        {
            var gameState = HttpContext.Session.GetObject<TwoPlayerGameState>(SessionKeyTwoPlayerGame);

            if (gameState == null)
            {
                gameState = new TwoPlayerGameState();
                gameState.Player1Board.RandomizeShipPlacement();
                gameState.Player2Board.RandomizeShipPlacement();
                HttpContext.Session.SetObject(SessionKeyTwoPlayerGame, gameState);
            }

            return View(gameState);
        }

        [HttpPost]
        public IActionResult Attack(int row, int col)
        {
            var gameState = HttpContext.Session.GetObject<TwoPlayerGameState>(SessionKeyTwoPlayerGame);

            if (!gameState.IsGameOver)
            {
                var board = gameState.IsPlayer1Turn ? gameState.Player2Board : gameState.Player1Board;
                var result = board.Attack(row, col);

                if (result == CellState.Hit)
                {
                    gameState.Message = "Hit!";
                    if (board.AllShipsSunk())
                    {
                        gameState.IsGameOver = true;
                        gameState.Message = (gameState.IsPlayer1Turn ? "Player 1" : "Player 2") + " wins!";
                    }
                }
                else if (result == CellState.Miss)
                {
                    gameState.Message = "Miss.";
                }

                // Always switch turns
                gameState.IsPlayer1Turn = !gameState.IsPlayer1Turn;
            }

            HttpContext.Session.SetObject(SessionKeyTwoPlayerGame, gameState);
            return RedirectToAction("Index");
        }

        public IActionResult NewGame()
        {
            var gameState = new TwoPlayerGameState();
            gameState.Player1Board.RandomizeShipPlacement();
            gameState.Player2Board.RandomizeShipPlacement();
            HttpContext.Session.SetObject(SessionKeyTwoPlayerGame, gameState);
            return RedirectToAction("Index");
        }
    }
}
