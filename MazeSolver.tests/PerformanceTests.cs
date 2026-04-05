using System;
using System.Collections.Generic;
using System.Diagnostics;
using MazeSolver;
using MazeSolver.Models;

namespace MazeSolver.tests
{
    [TestFixture]
    [Category("Performance")]
    public class PerformanceTests
    {
        [Test]
        public void BFSSolvesLargeGridWithinTimeLimit()
        {
            var (graph, start, end) = BuildGridGraph(60, 60);
            var stopwatch = Stopwatch.StartNew();
            var path = BFS.Search(graph, start, end);
            stopwatch.Stop();

            Assert.IsNotNull(path);
            Assert.Less(stopwatch.ElapsedMilliseconds, 2000);
        }

        [Test]
        public void AStarSolvesLargeGridWithinTimeLimit()
        {
            var (graph, start, end) = BuildGridGraph(80, 80);
            var stopwatch = Stopwatch.StartNew();
            var path = AStar.Search(graph, start, end);
            stopwatch.Stop();

            Assert.IsNotNull(path);
            Assert.Less(stopwatch.ElapsedMilliseconds, 1500);
        }

        [Test]
        public void AStarIsNotSlowerThanBfsOnComparableGrid()
        {
            var (graph, start, end) = BuildGridGraph(70, 70);
            var bfs = Measure(() => BFS.Search(graph, start, end));
            var aStar = Measure(() => AStar.Search(graph, start, end));

            Assert.IsNotNull(bfs.Path);
            Assert.IsNotNull(aStar.Path);

            Assert.LessOrEqual(
                aStar.Duration.TotalMilliseconds,
                bfs.Duration.TotalMilliseconds * 3 + 5);
        }

        private static (Graph Graph, Node Start, Node End) BuildGridGraph(int rows, int columns)
        {
            var graph = new Graph();
            var nodes = new Node[rows, columns];

            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < columns; c++)
                {
                    nodes[r, c] = graph.AddNode(r, c);
                }
            }

            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < columns; c++)
                {
                    var currentId = nodes[r, c].Id;

                    if (r > 0)
                    {
                        var upId = nodes[r - 1, c].Id;
                        graph.AddEdge(currentId, upId, 1);
                        graph.AddEdge(upId, currentId, 1);
                    }

                    if (c > 0)
                    {
                        var leftId = nodes[r, c - 1].Id;
                        graph.AddEdge(currentId, leftId, 1);
                        graph.AddEdge(leftId, currentId, 1);
                    }
                }
            }

            var startNode = graph.nodes[nodes[0, 0].Id];
            var endNode = graph.nodes[nodes[rows - 1, columns - 1].Id];

            return (graph, startNode, endNode);
        }

        private static (IList<Edge>? Path, TimeSpan Duration) Measure(Func<IList<Edge>?> search)
        {
            var stopwatch = Stopwatch.StartNew();
            var path = search();
            stopwatch.Stop();
            return (path, stopwatch.Elapsed);
        }
    }
}