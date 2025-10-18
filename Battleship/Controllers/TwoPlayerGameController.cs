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
                RandomlyPlaceShips(gameState.Player1Board);
                RandomlyPlaceShips(gameState.Player2Board);
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
                    gameState.IsPlayer1Turn = !gameState.IsPlayer1Turn;
                }
            }

            HttpContext.Session.SetObject(SessionKeyTwoPlayerGame, gameState);
            return RedirectToAction("Index");
        }

        public IActionResult NewGame()
        {
            var gameState = new TwoPlayerGameState();
            RandomlyPlaceShips(gameState.Player1Board);
            RandomlyPlaceShips(gameState.Player2Board);
            HttpContext.Session.SetObject(SessionKeyTwoPlayerGame, gameState);
            return RedirectToAction("Index");
        }

        private void RandomlyPlaceShips(GameBoard board)
        {
            var random = new Random();
            var ships = new[] { 3, 2, 1 };

            foreach (var shipSize in ships)
            {
                bool placed = false;
                while (!placed)
                {
                    var row = random.Next(GameBoard.Size);
                    var col = random.Next(GameBoard.Size);
                    var isHorizontal = random.Next(2) == 0;
                    placed = board.PlaceShip(new Ship { Size = shipSize }, row, col, isHorizontal);
                }
            }
        }
    }
}
