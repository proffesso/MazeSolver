using System.Linq;
using MazeSolver;
using MazeSolver.Models;

namespace MazeSolver.tests
{
    [TestFixture]
    public class AStarTests
    {
        [Test]
        public void AStarFindsShortestPathOnUnweightedGraph()
        {
            var graph = new Graph();
            var node1 = graph.AddNode();
            var node2 = graph.AddNode();
            var node3 = graph.AddNode();
            var node4 = graph.AddNode();

            graph.AddEdge(node1.Id, node2.Id, 1);
            graph.AddEdge(node2.Id, node3.Id, 1);
            graph.AddEdge(node3.Id, node4.Id, 1);
            graph.AddEdge(node1.Id, node3.Id, 5);

            var path = AStar.Search(graph, node1, node4);

            Assert.IsNotNull(path);
            var pathNodes = path!.Select(x => x.endNode).ToArray();
            var expected = new[] { node2.Id, node3.Id, node4.Id };
            Assert.IsTrue(pathNodes.SequenceEqual(expected));
        }

        [Test]
        public void AStarPrefersLowerCostPath()
        {
            var graph = new Graph();
            var start = graph.AddNode();
            var mid = graph.AddNode();
            var detour = graph.AddNode();
            var goal = graph.AddNode();

            graph.AddEdge(start.Id, goal.Id, 10);
            graph.AddEdge(start.Id, mid.Id, 2);
            graph.AddEdge(mid.Id, goal.Id, 2);
            graph.AddEdge(start.Id, detour.Id, 1);
            graph.AddEdge(detour.Id, goal.Id, 9);

            var path = AStar.Search(graph, start, goal);

            Assert.IsNotNull(path);
            var ends = path!.Select(x => x.endNode).ToArray();
            var expected = new[] { mid.Id, goal.Id };
            Assert.IsTrue(ends.SequenceEqual(expected));
            Assert.AreEqual(4, path.Sum(edge => edge.edgeLength));
        }

        [Test]
        public void AStarReturnsNullWhenNoPathExists()
        {
            var graph = new Graph();
            var node1 = graph.AddNode();
            var node2 = graph.AddNode();

            var path = AStar.Search(graph, node1, node2);

            Assert.IsNull(path);
        }
    }
}