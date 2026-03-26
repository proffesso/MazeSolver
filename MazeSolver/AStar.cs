using System;
using System.Collections.Generic;
using MazeSolver.Models;

namespace MazeSolver
{
    public static class AStar
    {
        public static IList<Edge>? Search(Graph graph, Node startNode, Node endNode, IReadOnlyDictionary<int, (int Row, int Col)>? nodePositions = null)
        {
            if (graph == null) throw new ArgumentNullException(nameof(graph));

            if (startNode.Id == endNode.Id)
            {
                return new List<Edge>();
            }

            var openSet = new PriorityQueue<int, double>();
            var cameFrom = new Dictionary<int, Edge>();
            var gScore = new Dictionary<int, double>
            {
                [startNode.Id] = 0
            };

            openSet.Enqueue(startNode.Id, Heuristic(startNode.Id, endNode.Id, nodePositions));

            while (openSet.Count > 0)
            {
                var currentId = openSet.Dequeue();

                if (!gScore.TryGetValue(currentId, out var currentScore))
                {
                    continue;
                }

                if (currentId == endNode.Id)
                {
                    return ConstructPath(cameFrom, startNode.Id, endNode.Id);
                }

                var currentNode = graph.nodes[currentId];
                foreach (var connectionId in currentNode.Connections)
                {
                    var edge = graph.edges[connectionId];
                    var neighborId = edge.endNode;
                    var tentativeGScore = currentScore + edge.edgeLength;

                    if (gScore.TryGetValue(neighborId, out var neighborScore) && tentativeGScore >= neighborScore)
                    {
                        continue;
                    }

                    cameFrom[neighborId] = edge;
                    gScore[neighborId] = tentativeGScore;
                    var fScore = tentativeGScore + Heuristic(neighborId, endNode.Id, nodePositions);
                    openSet.Enqueue(neighborId, fScore);
                }
            }

            return null;
        }

        private static double Heuristic(int nodeId, int targetId, IReadOnlyDictionary<int, (int Row, int Col)>? nodePositions)
        {
            if (nodePositions != null &&
                nodePositions.TryGetValue(nodeId, out var current) &&
                nodePositions.TryGetValue(targetId, out var target))
            {
                return Math.Abs(current.Row - target.Row) + Math.Abs(current.Col - target.Col);
            }

            return 0;
        }

        private static IList<Edge> ConstructPath(Dictionary<int, Edge> cameFrom, int startId, int endId)
        {
            var path = new List<Edge>();
            var current = endId;

            while (current != startId)
            {
                if (!cameFrom.TryGetValue(current, out var edge))
                {
                    return new List<Edge>();
                }

                path.Add(edge);
                current = edge.startNode;
            }

            path.Reverse();
            return path;
        }
    }
}