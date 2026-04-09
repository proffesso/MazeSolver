using MazeSolver;
using MazeSolver.Models;

namespace MazeSolver.tests
{
    [TestFixture]
    public class AStarTests
    {
        [Test]
        public void AStarFindsLowestCostPathInWeightedGraph()
        {
            var graph = new Graph();
            var start = graph.AddNode();
            var via1 = graph.AddNode();
            var via2 = graph.AddNode();
            var direct = graph.AddNode();
            var end = graph.AddNode();

            graph.AddEdge(start.Id, via1.Id, 2);
            graph.AddEdge(via1.Id, via2.Id, 2);
            graph.AddEdge(via2.Id, end.Id, 2);
            graph.AddEdge(start.Id, direct.Id, 5);
            graph.AddEdge(direct.Id, end.Id, 5);

            var path = AStar.Search(graph, start, end);

            Assert.IsNotNull(path);
            var nodes = path!.Select(x => x.endNode).ToArray();
            CollectionAssert.AreEqual(new[] { via1.Id, via2.Id, end.Id }, nodes);
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
        public void AStarReturnsEmptyPathWhenStartEqualsEnd()
        {
            var graph = new Graph();
            var node = graph.AddNode();

            var path = AStar.Search(graph, node, node);

            Assert.IsNotNull(path);
            Assert.AreEqual(0, path!.Count);
        }
    }
}