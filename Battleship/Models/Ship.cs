namespace Battleship.Models
{
    public class Ship
    {
        public int Size { get; set; }
        public int Hits { get; set; }
        public bool IsSunk => Hits >= Size;

        public Ship(int size)
        {
            Size = size;
            Hits = 0;
        }
    }
}
