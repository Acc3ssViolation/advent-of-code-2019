using Advent.Assignments;

namespace Advent.Shared
{
    public enum Direction
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3,
    }

    [Flags]
    public enum DirectionFlags
    {
        North = 1 << Direction.North,
        East = 1 << Direction.East,
        South = 1 << Direction.South,
        West = 1 << Direction.West,
    }

    public static class DirectionExtensions
    {
        public static Direction Opposite(this Direction direction)
        {
            return direction switch 
            {
                Direction.North => Direction.South,
                Direction.East => Direction.West,
                Direction.South => Direction.North,
                Direction.West => Direction.East,
                _ => Direction.North,       // Should never happen
            };
        }

        public static Direction Left(this Direction direction)
        {
            return direction switch 
            {
                Direction.North => Direction.West,
                Direction.East => Direction.North,
                Direction.South => Direction.East,
                Direction.West => Direction.South,
                _ => Direction.North,       // Should never happen
            };
        }

        public static Direction Right(this Direction direction)
        {
            return direction switch 
            {
                Direction.North => Direction.East,
                Direction.East => Direction.South,
                Direction.South => Direction.West,
                Direction.West => Direction.North,
                _ => Direction.North,       // Should never happen
            };
        }

        public static Point ToVector(this Direction direction)
        {
            return direction switch
            {
                Direction.North => Point.North,
                Direction.East => Point.East,
                Direction.South => Point.South,
                Direction.West => Point.West,
                _ => Point.North,       // Should never happen
            };
        }

        public static Direction ToDirection(this Point point)
        {
            return point switch
            {
                (0, -1) => Direction.North,
                (1, 0) => Direction.East,
                (0, 1) => Direction.South,
                (-1, 0) => Direction.West,
                _ => throw new ArgumentException("Point is not a direction"),
            };
        }
    }
}