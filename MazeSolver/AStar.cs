using MazeSolver.Models;

namespace MazeSolver
{
    public static class AStar
    {
        public static IList<Edge>? Search(Graph graph, Node startNode, Node endNode)
        {
            var openSet = new PriorityQueue<int, int>();
            var cameFrom = new Dictionary<int, Edge>();
            var gScore = new Dictionary<int, int>();
            var closed = new HashSet<int>();

            gScore[startNode.Id] = 0;
            openSet.Enqueue(startNode.Id, Heuristic(startNode, endNode));

            while (openSet.Count > 0)
            {
                var currentId = openSet.Dequeue();
                if (closed.Contains(currentId))
                    continue;

                if (currentId == endNode.Id)
                    return ConstructPath(graph, cameFrom, startNode.Id, endNode.Id);

                closed.Add(currentId);
                var current = graph.nodes[currentId];

                foreach (var edgeId in current.Connections)
                {
                    var edge = graph.edges[edgeId];
                    var neighborId = edge.endNode;

                    if (closed.Contains(neighborId))
                        continue;

                    var currentG = gScore.TryGetValue(currentId, out var cg) ? cg : int.MaxValue;
                    var tentativeG = currentG + edge.edgeLength;
                    var neighborG = gScore.TryGetValue(neighborId, out var ng) ? ng : int.MaxValue;

                    if (tentativeG < neighborG)
                    {
                        cameFrom[neighborId] = edge;
                        gScore[neighborId] = tentativeG;
                        var fScore = tentativeG + Heuristic(graph.nodes[neighborId], endNode);
                        openSet.Enqueue(neighborId, fScore);
                    }
                }
            }

            return null;
        }

        private static int Heuristic(Node a, Node b)
        {
            return Math.Abs(a.Row - b.Row) + Math.Abs(a.Col - b.Col);
        }

        private static IList<Edge> ConstructPath(Graph graph, IDictionary<int, Edge> cameFrom, int startNode, int endNode)
        {
            var result = new List<Edge>();
            var node = endNode;

            while (node != startNode)
            {
                var edge = cameFrom[node];
                result.Add(edge);
                node = edge.startNode;
            }

            result.Reverse();
            return result;
        }
    }
}