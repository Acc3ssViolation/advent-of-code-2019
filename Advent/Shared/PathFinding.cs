namespace Advent.Shared
{
    public static class PathFinding
    {
        public interface INodeProvider<T>
        {
            T GetStart();
            bool IsGoal(T node);
            List<T> GetNeighbours(T node);
            int GetDistance(T node, T neighbour);
            int GetHeuristic(T node);
        }

        public static List<T>? Dijkstra<T>(INodeProvider<T> nodeProvider) where T : notnull
        {
            var visited = new HashSet<T>();
            var openSet = new PriorityQueue<T, int>();
            var gScore = new Dictionary<T, int>();
            var cameFrom = new Dictionary<T, T?>();
            var start = nodeProvider.GetStart();
            openSet.Enqueue(start, 0);
            cameFrom[start] = default;
            gScore[start] = 0;
            while (openSet.TryDequeue(out var node, out var _))
            {
                if (visited.Contains(node))
                    continue;
                visited.Add(node);

                if (nodeProvider.IsGoal(node))
                {
                    var path = new List<T>();
                    while (node != null)
                    {
                        path.Add(node);
                        node = cameFrom[node];
                    }
                    return path;
                }

                var neighbours = nodeProvider.GetNeighbours(node);
                foreach (var neighbour in neighbours)
                {
                    var tgScore = gScore[node] + nodeProvider.GetDistance(node, neighbour);
                    if (!gScore.TryGetValue(neighbour, out var gScoreNeighbour))
                    {
                        gScoreNeighbour = int.MaxValue;
                    }
                    if (tgScore < gScoreNeighbour)
                    {
                        cameFrom[neighbour] = node;
                        gScore[neighbour] = tgScore;
                        openSet.Enqueue(neighbour, tgScore);
                    }
                }
            }

            return null;
        }
    }
}
