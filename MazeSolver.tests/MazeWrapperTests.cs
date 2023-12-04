using MazeSolver.Readers;

namespace MazeSolver.tests
{
    [TestFixture]
    public class MazeWrapperTest
    {
        [Test]
        public void MazeReadReturnsCorrectGraph()
        {
            //arrange
            var dataProvider = new FileDataProvider();
            dataProvider.FilePath = "../../../../maze_test.txt";
            var mazeWrapper = new MazeWrapper(dataProvider);
            
            //act
            var (graph, start, end) = mazeWrapper.Read();
            
            // assert
            Assert.AreEqual(graph.nodes.Count, 26);
            
        }

        [Test]
        public void MazeReadReturnsCorrectGraphWithEdges()
        {
            //arrange
            var dataProvider = new FileDataProvider();
            dataProvider.FilePath = "../../../../maze_test_simple.txt";
            var mazeWrapper = new MazeWrapper(dataProvider);
            
            //act
            var (graph, start, end) = mazeWrapper.Read();
            
            // assert
            Assert.AreEqual(3, graph.nodes.Count);
            Assert.AreEqual(1, graph.edges.Where(x => x.startNode == 0).Count());
            Assert.AreEqual(2, graph.edges.Where(x => x.startNode == 1).Count());
            Assert.AreEqual(1, graph.edges.Where(x => x.startNode == 2).Count());
            Assert.AreEqual(0, graph.edges.Where(x => x.startNode == 3).Count());
        }
    }
}