# Battleship

Welcome to Battleship! This is a classic naval combat game where your mission is to locate and sink the enemy's fleet before they sink yours. This is a web-based version of the game built with ASP.NET Core.

## How to Play

### Objective

The objective of the game is to be the first to sink all of your opponent's ships.

### Game Modes

*   **Player vs. Computer:** Test your skills against an AI opponent. The computer's ships are placed randomly, and it will try to find and sink your ships.
*   **Two Player:** Play against a friend on the same computer. Take turns to attack each other's board.

### Game Setup

*   Each player has a 7x7 grid for their fleet.
*   The fleet consists of three ships:
    *   Aircraft Carrier (4 cells)
    *   Destroyer (3 cells)
    *   Patrol Boat (2 cells)
*   In the single-player mode, the ships are placed randomly on both your board and the computer's board at the start of a new game.

### Gameplay

1.  The game is played in turns.
2.  On your turn, you will click on a cell in the opponent's grid to fire a shot.
3.  If your shot hits a ship, the cell will be marked with a "Hit" marker (an explosion).
4.  If your shot misses, the cell will be marked with a "Miss" marker (a splash).
5.  A ship is considered "sunk" when all of its cells have been hit.
6.  The game continues until one player has sunk all of the opponent's ships.

### Winning the Game

The first player to sink all of the opponent's ships wins the game!
