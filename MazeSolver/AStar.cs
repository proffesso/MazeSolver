using System;
using System.Collections.Generic;
using MazeSolver.Models;

namespace MazeSolver
{
    public static class AStar
    {
        public static IList<Edge>? Search(Graph graph, Node startNode, Node endNode, Func<Node, Node, double>? heuristic = null)
        {
            if (graph.nodes.Count == 0)
            {
                return null;
            }

            heuristic ??= DefaultHeuristic;

            var openSet = new PriorityQueue<int, double>();
            var cameFrom = new Dictionary<int, Edge>();
            var gScore = new Dictionary<int, double>();
            var fScore = new Dictionary<int, double>();

            gScore[startNode.Id] = 0;
            var startEstimate = heuristic(startNode, endNode);
            fScore[startNode.Id] = startEstimate;
            openSet.Enqueue(startNode.Id, startEstimate);

            while (openSet.TryDequeue(out var currentId, out var currentPriority))
            {
                if (!fScore.TryGetValue(currentId, out var recordedPriority) || currentPriority > recordedPriority)
                {
                    continue;
                }

                if (currentId == endNode.Id)
                {
                    return ConstructPath(graph, cameFrom, startNode.Id, endNode.Id);
                }

                if (!gScore.TryGetValue(currentId, out var currentScore))
                {
                    continue;
                }

                foreach (var edgeIndex in graph.nodes[currentId].Connections)
                {
                    var edge = graph.edges[edgeIndex];
                    var tentativeGScore = currentScore + edge.edgeLength;

                    if (!gScore.TryGetValue(edge.endNode, out var existingScore) || tentativeGScore < existingScore)
                    {
                        cameFrom[edge.endNode] = edge;
                        gScore[edge.endNode] = tentativeGScore;

                        var neighborNode = graph.nodes[edge.endNode];
                        var estimatedScore = tentativeGScore + heuristic(neighborNode, endNode);
                        fScore[edge.endNode] = estimatedScore;
                        openSet.Enqueue(edge.endNode, estimatedScore);
                    }
                }
            }

            return null;
        }

        private static IList<Edge> ConstructPath(Graph graph, IDictionary<int, Edge> cameFrom, int startNode, int endNode)
        {
            var path = new List<Edge>();
            var current = endNode;

            while (current != startNode)
            {
                var edge = cameFrom[current];
                path.Add(edge);
                current = edge.startNode;
            }

            path.Reverse();
            return path;
        }

        private static double DefaultHeuristic(Node current, Node goal)
        {
            if (current.Row >= 0 && current.Column >= 0 && goal.Row >= 0 && goal.Column >= 0)
            {
                return Math.Abs(current.Row - goal.Row) + Math.Abs(current.Column - goal.Column);
            }

            return 0;
        }
    }
}