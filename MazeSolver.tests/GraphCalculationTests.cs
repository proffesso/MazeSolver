using MazeSolver;
using MazeSolver.Models;
using MazeSolver.Readers;

namespace MazeSolver.tests
{
    [TestFixture]
    public class GraphCalculationTests
    {
        [Test]
        public void LargeGridGraphBfsFindsShortestPath()
        {
            const int rows = 200;
            const int cols = 200;

            var (graph, start, end) = BuildGridGraph(rows, cols);
            var searchResult = BFS.Search(graph, start, end);

            Assert.IsNotNull(searchResult);

            var path = searchResult!;
            Assert.AreEqual(rows + cols - 2, path.Count);
            Assert.AreEqual(start.Id, path[0].startNode);
            Assert.AreEqual(end.Id, path[path.Count - 1].endNode);
        }

        [Test]
        public void DisconnectedLargeGraphBfsReturnsNull()
        {
            var graph = new Graph();
            const int componentSize = 3000;

            var firstComponentStart = AddLinearComponent(graph, componentSize);
            var secondComponentStart = AddLinearComponent(graph, componentSize);

            var path = BFS.Search(graph, graph.nodes[firstComponentStart], graph.nodes[secondComponentStart]);

            Assert.IsNull(path);
        }

        [Test]
        public void LargeCycleGraphBfsNavigatesWithoutLooping()
        {
            const int nodeCount = 10000;

            var (graph, start, end) = BuildCycleGraph(nodeCount);
            var searchResult = BFS.Search(graph, start, end);

            Assert.IsNotNull(searchResult);

            var path = searchResult!;
            Assert.AreEqual(nodeCount / 2, path.Count);
            Assert.AreEqual(start.Id, path[0].startNode);
            Assert.AreEqual(end.Id, path[path.Count - 1].endNode);
        }

        [Test]
        public void RealWorldMazeGraphMatchesExpectedShortestPathLength()
        {
            var dataProvider = new FileDataProvider();
            dataProvider.FilePath = "../../../../maze_test.txt";
            var mazeWrapper = new MazeWrapper(dataProvider);

            var (graph, start, end) = mazeWrapper.Read();
            var searchResult = BFS.Search(graph, start, end);

            Assert.IsNotNull(searchResult);

            var path = searchResult!;
            Assert.AreEqual(19, path.Count); // Pre-calculated shortest path length for maze_test.txt
            Assert.AreEqual(start.Id, path[0].startNode);
            Assert.AreEqual(end.Id, path[path.Count - 1].endNode);
        }

        private static (Graph graph, Node start, Node end) BuildGridGraph(int rows, int cols)
        {
            var graph = new Graph();
            var nodeIds = new int[rows, cols];

            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < cols; col++)
                {
                    nodeIds[row, col] = graph.AddNode().Id;
                }
            }

            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < cols; col++)
                {
                    var current = nodeIds[row, col];

                    if (row + 1 < rows)
                    {
                        AddUndirectedEdge(graph, current, nodeIds[row + 1, col]);
                    }

                    if (col + 1 < cols)
                    {
                        AddUndirectedEdge(graph, current, nodeIds[row, col + 1]);
                    }
                }
            }

            return (graph, graph.nodes[nodeIds[0, 0]], graph.nodes[nodeIds[rows - 1, cols - 1]]);
        }

        private static int AddLinearComponent(Graph graph, int length)
        {
            var firstNodeId = graph.AddNode().Id;
            var previousId = firstNodeId;

            for (var i = 1; i < length; i++)
            {
                var currentId = graph.AddNode().Id;
                AddUndirectedEdge(graph, previousId, currentId);
                previousId = currentId;
            }

            return firstNodeId;
        }

        private static (Graph graph, Node start, Node end) BuildCycleGraph(int nodeCount)
        {
            var graph = new Graph();
            var nodeIds = new int[nodeCount];

            for (var i = 0; i < nodeCount; i++)
            {
                nodeIds[i] = graph.AddNode().Id;
            }

            for (var i = 0; i < nodeCount; i++)
            {
                var nextIndex = (i + 1) % nodeCount;
                AddUndirectedEdge(graph, nodeIds[i], nodeIds[nextIndex]);
            }

            return (graph, graph.nodes[nodeIds[0]], graph.nodes[nodeIds[nodeCount / 2]]);
        }

        private static void AddUndirectedEdge(Graph graph, int startNodeId, int endNodeId)
        {
            graph.AddEdge(startNodeId, endNodeId, 1);
            graph.AddEdge(endNodeId, startNodeId, 1);
        }
    }
}