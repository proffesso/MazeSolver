using MazeSolver.Models;

namespace MazeSolver.tests
{
    [TestFixture]
    public class GraphsTest
    {
        [Test]
        public void BFSFindsCorrectPath()
        {
            var graph = new Graph();
            var node1 = graph.AddNode();
            var node2 = graph.AddNode();
            var node3 = graph.AddNode();
            var node4 = graph.AddNode();
            var node5 = graph.AddNode();

            graph.AddEdge(node1.Id, node2.Id, 1);
            graph.AddEdge(node2.Id, node3.Id, 1);
            graph.AddEdge(node3.Id, node1.Id, 1);
            graph.AddEdge(node3.Id, node4.Id, 1);
            graph.AddEdge(node4.Id, node5.Id, 1);
            
            var path = BFS.Search(graph, node1, node4).Select( x => x.endNode).ToArray();
            var referencePath = new[] { node2.Id, node3.Id, node4.Id };
            
            Assert.IsTrue( path.SequenceEqual( referencePath ) ); 
        }
        
        [Test]
        public void BFSNolockForLoops()
        {
            var graph = new Graph();
            var node1 = graph.AddNode();
            var node2 = graph.AddNode();
            var node3 = graph.AddNode();
            var node4 = graph.AddNode();
            var node5 = graph.AddNode();

            graph.AddEdge(node1.Id, node2.Id, 1);
            graph.AddEdge(node2.Id, node3.Id, 1);
            graph.AddEdge(node3.Id, node1.Id, 1);

            var path = BFS.Search(graph, node1, node5);
            Assert.IsNull(path);
        }

        [Test]
        public void BFSReturnsNullIfNoPath()
        {
            var graph = new Graph();
            var node1 = graph.AddNode();
            var node2 = graph.AddNode();

            var path = BFS.Search(graph, node1, node2);
            Assert.IsNull(path);
        }
    }
}