namespace Battleship.Models
{
    public class Ship
    {
        public int Size { get; set; }
        public int Hits { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public bool IsHorizontal { get; set; }
        public string ImagePath { get; set; }

        public bool IsSunk()
        {
            return Hits >= Size;
        }

        public bool IsAt(int row, int col)
        {
            if (IsHorizontal)
            {
                return row == Row && col >= Col && col < Col + Size;
            }
            else
            {
                return col == Col && row >= Row && row < Row + Size;
            }
        }
    }
}
