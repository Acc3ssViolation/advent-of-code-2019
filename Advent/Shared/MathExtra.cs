namespace Advent.Shared
{
    internal static class MathExtra
    {
        public static long LowestCommonMultiple(this IEnumerable<long> nums)
        {
            return nums.Aggregate(LowestCommonMultiple);
        }

        public static long LowestCommonMultiple(long a, long b)
        {
            return Math.Abs(a * b) / GreatestCommonDivisor(a, b);
        }

        public static long GreatestCommonDivisor(long a, long b)
        {
            if (b == 0)
                return a;
            return GreatestCommonDivisor(b, a % b);
        }

        public static void Populate<T>(this T[] arr, T value)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = value;
            }
        }

        public static T[] Repeat<T>(this List<T> list, int count)
        {
            var result = new T[count * list.Count];
            for (var i = 0; i < count; i++)
            {
                list.CopyTo(result, list.Count * i);
            }
            return result;
        }

        public static int UniquePairCount(int count)
        {
            return count * (count - 1) / 2;
        }

        /// <summary>
        /// Calculates the area of a polygon using the shoelace algorithm.
        /// </summary>
        public static long Shoelace(this List<Point> vertices)
        {
            var sum = 0L;
            for (var i = 0; i < vertices.Count; i++)
            {
                var a = vertices[i];
                var b = vertices[(i + 1) % vertices.Count];
                sum += ((long)b.x + a.x) * ((long)b.y - a.y);
            }
            return sum / 2;
        }

        /// <summary>
        /// Calculates the area of a polygon using the shoelace algorithm.
        /// </summary>
        public static double Shoelace(this List<RealPoint> vertices)
        {
            var sum = 0.0;
            for (var i = 0; i < vertices.Count; i++)
            {
                var a = vertices[i];
                var b = vertices[(i + 1) % vertices.Count];
                sum += (b.x + a.x) * (b.y - a.y);
            }
            return sum / 2;
        }

        /// <summary>
        /// Expands a polygon by the given distance along world axis
        /// </summary>
        public static List<RealPoint> Expand(this List<Point> vertices, double distance)
        {
            var edgeNormals = new List<RealPoint>(vertices.Count);
            for (var i = 0; i < vertices.Count; i++)
            {
                var start = vertices[i];
                var end = vertices[(i + 1) % vertices.Count];
                var normal = end - start;
                normal /= normal.Length;
                normal = normal.ToDirection().Left().ToVector();
                edgeNormals.Add(new RealPoint(normal));
            }

            var offsetVertices = new List<RealPoint>(vertices.Count);
            for (var i = 0; i < vertices.Count; i++)
            {
                var prevEdgeIndex = (i + edgeNormals.Count - 1) % edgeNormals.Count;
                var nextEdgeIndex = (i) % edgeNormals.Count;

                var prevNormal = edgeNormals[prevEdgeIndex];
                var nextNormal = edgeNormals[nextEdgeIndex];
                var normal = (prevNormal + nextNormal);
                var offset = new RealPoint(Math.Sign(normal.x) * distance, Math.Sign(normal.y) * distance);
                offsetVertices.Add(new RealPoint(vertices[i]) + offset);
            }

            return offsetVertices;
        }
    }
}
