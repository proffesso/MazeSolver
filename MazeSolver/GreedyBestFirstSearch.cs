using System.Collections.Generic;
using MazeSolver.Models;

namespace MazeSolver
{
    public static class GreedyBestFirstSearch
    {
        public static IList<Edge>? Search(Graph graph, Node startNode, Node endNode)
        {
            if (graph.nodes.Count == 0)
            {
                return null;
            }

            if (startNode.Id == endNode.Id)
            {
                return new List<Edge>();
            }

            var heuristics = ComputeHeuristics(graph, endNode.Id);
            var queue = new PriorityQueue<int, int>();
            var visited = new HashSet<int>();
            var cameFrom = new Dictionary<int, Edge>();

            var startPriority = heuristics.TryGetValue(startNode.Id, out var startHeuristic) ? startHeuristic : int.MaxValue;
            queue.Enqueue(startNode.Id, startPriority);

            while (queue.TryDequeue(out var currentNodeId, out _))
            {
                if (!visited.Add(currentNodeId))
                {
                    continue;
                }

                if (currentNodeId == endNode.Id)
                {
                    return ConstructPath(cameFrom, startNode.Id, endNode.Id);
                }

                foreach (var connectionId in graph.nodes[currentNodeId].Connections)
                {
                    var edge = graph.edges[connectionId];
                    if (visited.Contains(edge.endNode))
                    {
                        continue;
                    }

            if (!cameFrom.ContainsKey(edge.endNode))
                    {
                        cameFrom[edge.endNode] = edge;
                    }

                    var priority = heuristics.TryGetValue(edge.endNode, out var heuristic) ? heuristic : int.MaxValue;
                    queue.Enqueue(edge.endNode, priority);
                }
            }

            return null;
        }

        private static Dictionary<int, int> ComputeHeuristics(Graph graph, int targetNodeId)
        {
            var transpose = graph.Transpose();
            var heuristics = new Dictionary<int, int>();
            var queue = new Queue<int>();

            heuristics[targetNodeId] = 0;
            queue.Enqueue(targetNodeId);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var currentDistance = heuristics[current];

                foreach (var connectionId in transpose.nodes[current].Connections)
                {
                    var edge = transpose.edges[connectionId];
                    var neighbor = edge.endNode;
                    if (heuristics.ContainsKey(neighbor))
                    {
                        continue;
                    }

                    heuristics[neighbor] = currentDistance + edge.edgeLength; // transpose provides distance-to-go estimates
                    queue.Enqueue(neighbor);
                }
            }

            return heuristics;
        }

        private static IList<Edge>? ConstructPath(IDictionary<int, Edge> cameFrom, int startNodeId, int endNodeId)
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