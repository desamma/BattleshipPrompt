using Microsoft.AspNetCore.Mvc;
using Battleship.Models;

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
                HttpContext.Session.SetObject(SessionKeyTwoPlayerGame, gameState);
            }

            return View(gameState);
        }

        [HttpPost]
        public IActionResult PlaceShip(int row, int col, bool isHorizontal)
        {
            var gameState = HttpContext.Session.GetObject<TwoPlayerGameState>(SessionKeyTwoPlayerGame);

            if (gameState.IsShipPlacementPhase)
            {
                var shipsToPlace = gameState.IsPlayer1Turn ? gameState.Player1ShipsToPlace : gameState.Player2ShipsToPlace;
                var board = gameState.IsPlayer1Turn ? gameState.Player1Board : gameState.Player2Board;

                if (shipsToPlace.Count > 0)
                {
                    var shipSize = shipsToPlace[0];
                    if (board.PlaceShip(new Ship { Size = shipSize }, row, col, isHorizontal))
                    {
                        shipsToPlace.RemoveAt(0);
                    }
                }

                if (gameState.Player1ShipsToPlace.Count == 0 && gameState.Player2ShipsToPlace.Count == 0)
                {
                    gameState.IsShipPlacementPhase = false;
                    gameState.Message = "Player 1's turn.";
                }
                else if (gameState.Player1ShipsToPlace.Count == 0)
                {
                    gameState.IsPlayer1Turn = false;
                    gameState.Message = "Player 2, place your ships.";
                }
                else if (shipsToPlace.Count == 0)
                {
                    gameState.IsPlayer1Turn = true;
                    gameState.Message = "Player 1, place your ships.";
                }
            }

            HttpContext.Session.SetObject(SessionKeyTwoPlayerGame, gameState);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Attack(int row, int col)
        {
            var gameState = HttpContext.Session.GetObject<TwoPlayerGameState>(SessionKeyTwoPlayerGame);

            if (!gameState.IsGameOver && !gameState.IsShipPlacementPhase)
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
            HttpContext.Session.SetObject(SessionKeyTwoPlayerGame, gameState);
            return RedirectToAction("Index");
        }
    }
}
