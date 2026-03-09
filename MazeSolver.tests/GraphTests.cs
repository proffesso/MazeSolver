using MazeSolver;
using NUnit.Framework;

namespace MazeSolver.tests
{
    [TestFixture]
    public class GraphTests
    {
        [Test]
        public void EmptyGraph_DoesNotThrowException()
        {
            // Arrange
            var graph = new Graph();

            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                // Try to access nodes and edges, should not throw
                var nodes = graph.Nodes;
                var edges = graph.Edges;
            });
        }
    }
}