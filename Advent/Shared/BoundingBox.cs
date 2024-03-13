namespace Advent.Shared
{
    internal struct BoundingBox
    {
        public int xMin;
        public int xMax;
        public int yMin;
        public int yMax;

        public void Expand(Point p)
        {
            if (xMin > p.x)
                xMin = p.x;
            if (yMin > p.y)
                yMin = p.y;
            if (xMax < p.x)
                xMax = p.x;
            if (yMax < p.y) 
                yMax = p.y;
        }
    }
}
