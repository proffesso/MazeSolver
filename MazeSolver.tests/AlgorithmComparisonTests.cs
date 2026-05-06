using System.Collections.Generic;
using System.IO;
using System.Linq;
using MazeSolver;
using MazeSolver.Models;
using MazeSolver.Readers;

namespace MazeSolver.tests
{
    [TestFixture]
    public class AlgorithmComparisonTests
    {
        [TestCase("../../../../maze_test_simple.txt", TestName = "AlgorithmsProduceComparablePaths_SimpleMaze")]
        [TestCase("../../../../maze_test.txt", TestName = "AlgorithmsProduceComparablePaths_StandardMaze")]
        public void AlgorithmsProduceComparablePaths(string mazePath)
        {
            var scenario = $"Scenario:{Path.GetFileName(mazePath)}";
            var (graph, start, end) = LoadMaze(mazePath);

            var bfsPath = PerformanceLogger.Measure(scenario, "BFS", () => BFS.Search(graph, start, end));
            var dijkstraPath = PerformanceLogger.Measure(scenario, "Dijkstra", () => Dijkstra.Search(graph, start, end));
            var greedyPath = PerformanceLogger.Measure(scenario, "GreedyBestFirstSearch", () => GreedyBestFirstSearch.Search(graph, start, end));

            Assert.That(bfsPath, Is.Not.Null);
            Assert.That(dijkstraPath, Is.Not.Null);
            Assert.That(greedyPath, Is.Not.Null);

            Assert.That(dijkstraPath!.Count, Is.EqualTo(bfsPath!.Count));
            Assert.That(greedyPath!.Count, Is.GreaterThanOrEqualTo(bfsPath.Count));
        }

        [Test]
        public void AlgorithmsHandleComplexMazeWithExpectedCosts()
        {
            const string mazePath = "../../../../maze_test_complex.txt";
            var scenario = $"Scenario:{Path.GetFileName(mazePath)}";
            var (graph, start, end) = LoadMaze(mazePath);

            var bfsPath = PerformanceLogger.Measure(scenario, "BFS", () => BFS.Search(graph, start, end));
            var dijkstraPath = PerformanceLogger.Measure(scenario, "Dijkstra", () => Dijkstra.Search(graph, start, end));
            var greedyPath = PerformanceLogger.Measure(scenario, "GreedyBestFirstSearch", () => GreedyBestFirstSearch.Search(graph, start, end));

            Assert.Multiple(() =>
            {
                Assert.That(bfsPath, Is.Not.Null);
                Assert.That(dijkstraPath, Is.Not.Null);
                Assert.That(greedyPath, Is.Not.Null);
            });

            var bfsCost = PathCost(bfsPath);
            var dijkstraCost = PathCost(dijkstraPath);
            var greedyCost = PathCost(greedyPath);

            Assert.That(bfsCost, Is.EqualTo(22));
            Assert.That(dijkstraCost, Is.EqualTo(bfsCost));
            Assert.That(greedyCost, Is.GreaterThanOrEqualTo(bfsCost));
        }

        private static (Graph graph, Node start, Node end) LoadMaze(string path)
        {
            var dataProvider = new FileDataProvider();
            dataProvider.FilePath = path;
            var mazeWrapper = new MazeWrapper(dataProvider);
            return mazeWrapper.Read();
        }

        private static int PathCost(IList<Edge>? path)
        {
            return path?.Sum(edge => edge.edgeLength) ?? int.MaxValue;
        }
    }
}