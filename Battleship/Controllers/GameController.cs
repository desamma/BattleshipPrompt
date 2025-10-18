
using Microsoft.AspNetCore.Mvc;
using Battleship.Models;
using System;

namespace Battleship.Controllers
{
    public class GameController : Controller
    {
        private const string SessionKeyGame = "_Game";

        public IActionResult Index()
        {
            var gameState = HttpContext.Session.Get<GameState>(SessionKeyGame);

            if (gameState == null)
            {
                gameState = new GameState();
                InitializeGame(gameState);
                HttpContext.Session.Set(SessionKeyGame, gameState);
            }

            return View(gameState);
        }

        [HttpPost]
        public IActionResult Attack(int row, int col)
        {
            var gameState = HttpContext.Session.Get<GameState>(SessionKeyGame);

            if (gameState != null && !gameState.IsGameOver && gameState.IsPlayerTurn)
            {
                ProcessPlayerAttack(gameState, row, col);

                if (!gameState.IsGameOver)
                {
                    ProcessComputerTurn(gameState);
                }
            }

            HttpContext.Session.Set(SessionKeyGame, gameState);
            return RedirectToAction(nameof(Index));
        }
        
        public IActionResult NewGame()
        {
            var gameState = new GameState();
            InitializeGame(gameState);
            HttpContext.Session.Set(SessionKeyGame, gameState);

            return RedirectToAction(nameof(Index));
        }


        private void InitializeGame(GameState gameState)
        {
            // Clear boards
            gameState.PlayerBoard = new GameBoard();
            gameState.ComputerBoard = new GameBoard();

            // Place ships
            PlaceShips(gameState.PlayerBoard);
            PlaceShips(gameState.ComputerBoard);

            gameState.IsPlayerTurn = true;
            gameState.Message = "Player's turn.";
            gameState.IsGameOver = false;
        }

        private void PlaceShips(GameBoard board)
        {
            var random = new Random();
            var shipsToPlace = new[] { 3, 2, 1 };

            foreach (var shipSize in shipsToPlace)
            {
                bool placed = false;
                while (!placed)
                {
                    int row = random.Next(GameBoard.Size);
                    int col = random.Next(GameBoard.Size);
                    bool isHorizontal = random.Next(2) == 0;

                    if (board.PlaceShip(new Ship { Size = shipSize, Hits = 0 }, row, col, isHorizontal))
                    {
                        placed = true;
                    }
                }
            }
        }
        
        private void ProcessPlayerAttack(GameState gameState, int row, int col)
        {
            var result = gameState.ComputerBoard.Attack(row, col);
            switch (result)
            {
                case CellState.Hit:
                    gameState.Message = "Hit!";
                    break;
                case CellState.Miss:
                    gameState.Message = "Miss.";
                    break;
                case CellState.Occupied:
                    gameState.Message = "Already attacked.";
                    break;
            }

            if (gameState.ComputerBoard.AllShipsSunk())
            {
                gameState.IsGameOver = true;
                gameState.Message = "You win!";
            }
            else
            {
                gameState.IsPlayerTurn = false;
            }
        }

        private void ProcessComputerTurn(GameState gameState)
        {
            var random = new Random();
            CellState result;
            int row, col;
            do
            {
                row = random.Next(GameBoard.Size);
                col = random.Next(GameBoard.Size);
                result = gameState.PlayerBoard.Attack(row, col);

            } while (result == CellState.Occupied);


            if (gameState.PlayerBoard.AllShipsSunk())
            {
                gameState.IsGameOver = true;
                gameState.Message = "Computer wins!";
            }
            else
            {
                gameState.IsPlayerTurn = true;
            }
        }
    }
}
namespace Microsoft.AspNetCore.Http
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, System.Text.Json.JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : System.Text.Json.JsonSerializer.Deserialize<T>(value);
        }
    }
}
