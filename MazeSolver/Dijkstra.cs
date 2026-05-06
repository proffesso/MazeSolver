using System.Collections.Generic;
using MazeSolver.Models;

namespace MazeSolver
{
    public static class Dijkstra
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

            var distances = new Dictionary<int, int>(graph.nodes.Count);
            foreach (var node in graph.nodes)
            {
                distances[node.Id] = int.MaxValue;
            }

            var previous = new Dictionary<int, Edge>();
            var visited = new HashSet<int>();
            var queue = new PriorityQueue<int, int>();

            distances[startNode.Id] = 0;
            queue.Enqueue(startNode.Id, 0);

            while (queue.TryDequeue(out var currentNodeId, out _))
            {
                if (!visited.Add(currentNodeId))
                {
                    continue;
                }

                if (currentNodeId == endNode.Id)
                {
                    break;
                }

                if (distances[currentNodeId] == int.MaxValue)
                {
                    continue;
                }

                foreach (var connectionId in graph.nodes[currentNodeId].Connections)
                {
                    var edge = graph.edges[connectionId];
                    var candidateDistance = distances[currentNodeId] + edge.edgeLength;
                    if (candidateDistance < distances[edge.endNode])
                    {
                        distances[edge.endNode] = candidateDistance;
                        previous[edge.endNode] = edge;
                        queue.Enqueue(edge.endNode, candidateDistance);
                    }
                }
            }

            if (!previous.ContainsKey(endNode.Id))
            {
                return null;
            }

            return ConstructPath(previous, startNode.Id, endNode.Id);
        }

        private static IList<Edge>? ConstructPath(IDictionary<int, Edge> previous, int startNodeId, int endNodeId)
        {
            var path = new List<Edge>();
            var current = endNodeId;

            while (current != startNodeId)
            {
                if (!previous.TryGetValue(current, out var edge))
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