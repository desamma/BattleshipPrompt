namespace Battleship.Models
{
    public class CellState
    {
        public bool IsOccupied { get; set; }
        public bool IsHit { get; set; }
        public Ship Ship { get; set; }
    }
}
