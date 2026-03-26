using System.Collections.Generic;
using System.Linq;
using MazeSolver;
using MazeSolver.Interfaces;
using MazeSolver.Models;

namespace MazeSolver.tests
{
    [TestFixture]
    public class AStarTests
    {
        [Test]
        public void AStarFindsShortestPathUsingHeuristic()
        {
            var graph = new Graph();
            var start = graph.AddNode();
            var mid = graph.AddNode();
            var detour = graph.AddNode();
            var end = graph.AddNode();

            graph.AddEdge(start.Id, mid.Id, 1);
            graph.AddEdge(mid.Id, end.Id, 1);
            graph.AddEdge(start.Id, detour.Id, 5);
            graph.AddEdge(detour.Id, end.Id, 1);

            var positions = new Dictionary<int, (int Row, int Col)>
            {
                [start.Id] = (0, 0),
                [mid.Id] = (0, 1),
                [detour.Id] = (1, 0),
                [end.Id] = (0, 2)
            };

            var path = AStar.Search(graph, start, end, positions);

            Assert.IsNotNull(path);
            var nodeSequence = path!.Select(e => e.endNode).ToArray();
            Assert.That(nodeSequence, Is.EqualTo(new[] { mid.Id, end.Id }));
        }

        [Test]
        public void AStarReturnsNullWhenNoPathExists()
        {
            var graph = new Graph();
            var start = graph.AddNode();
            var end = graph.AddNode();

            var path = AStar.Search(graph, start, end);

            Assert.IsNull(path);
        }

        [Test]
        public void AStarSolvesMazeUsingMazeWrapperCoordinates()
        {
            var provider = new StubDataProvider(new[]
            {
                "000",
                "010",
                "000"
            });

            var wrapper = new MazeWrapper(provider);
            var (graph, start, end) = wrapper.Read();

            var path = AStar.Search(graph, start, end, wrapper.NodeCoordinates);

            Assert.IsNotNull(path);
            Assert.AreEqual(4, path!.Count);
        }

        private class StubDataProvider : IDataProvider
        {
            private readonly string[] _lines;

            public StubDataProvider(string[] lines)
            {
                _lines = lines;
            }

            public void Init()
            {
            }

            public (int, int) GetSize()
            {
                return (_lines.Length, _lines.Length > 0 ? _lines[0].Length : 0);
            }

            public void StartReadData((int, int) size)
            {
            }

            public string GetLine(int i)
            {
                return _lines[i - 1];
            }

            public void ExposeError(string message)
            {
            }
        }
    }
}