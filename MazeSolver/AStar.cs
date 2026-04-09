using MazeSolver.Models;

namespace MazeSolver
{
    public static class AStar
    {
        public static IList<Edge>? Search(Graph graph, Node startNode, Node endNode, Func<Node, Node, double>? heuristic = null)
        {
            heuristic ??= static (_, _) => 0d;

            if (graph.nodes.Count == 0)
            {
                return null;
            }

            if (startNode.Id == endNode.Id)
            {
                return new List<Edge>();
            }

            var nodeCount = graph.nodes.Count;
            var gScore = new double[nodeCount];
            for (var i = 0; i < nodeCount; i++)
            {
                gScore[i] = double.PositiveInfinity;
            }

            var closedSet = new bool[nodeCount];
            var cameFrom = new Dictionary<int, Edge>();
            var openSet = new PriorityQueue<int, double>();

            gScore[startNode.Id] = 0;
            var startPriority = heuristic(graph.nodes[startNode.Id], endNode);
            openSet.Enqueue(startNode.Id, startPriority);

            while (openSet.Count > 0)
            {
                var currentId = openSet.Dequeue();
                if (closedSet[currentId])
                {
                    continue;
                }

                if (currentId == endNode.Id)
                {
                    return ConstructPath(cameFrom, startNode.Id, endNode.Id);
                }

                closedSet[currentId] = true;

                foreach (var connectionId in graph.nodes[currentId].Connections)
                {
                    var edge = graph.edges[connectionId];
                    var neighborId = edge.endNode;

                    if (closedSet[neighborId])
                    {
                        continue;
                    }

                    var tentativeGScore = gScore[currentId] + edge.edgeLength;
                    if (tentativeGScore >= gScore[neighborId])
                    {
                        continue;
                    }

                    cameFrom[neighborId] = edge;
                    gScore[neighborId] = tentativeGScore;

                    var priority = tentativeGScore + heuristic(graph.nodes[neighborId], endNode);
                    openSet.Enqueue(neighborId, priority);
                }
            }

            return null;
        }

        private static IList<Edge>? ConstructPath(IReadOnlyDictionary<int, Edge> cameFrom, int startNodeId, int endNodeId)
        {
            var path = new List<Edge>();
            var current = endNodeId;

            while (current != startNodeId)
            {
                if (!cameFrom.TryGetValue(current, out var edge))
                {
                    return null;
                }

                path.Add(edge);
                current = edge.startNode;
            }

            path.Reverse();
            return path;
        }
    }
}