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

        [Test]
        public void MazeAStarFindsPathForSimpleMaze()
        {
            var dataProvider = new FileDataProvider();
            dataProvider.FilePath = "../../../../maze_test_simple.txt";
            var mazeWrapper = new MazeWrapper(dataProvider);

            var (graph, start, end) = mazeWrapper.Read();

            var path = AStar.Search(graph, start, end);

            Assert.IsNotNull(path);
            Assert.AreEqual(2, path!.Count);
            Assert.AreEqual(end.Id, path.Last().endNode);
        }

        [Test]
        public void MazeAStarReturnsNullWhenPathNotExists()
        {
            var graph = new Models.Graph();
            var n1 = graph.AddNode(0, 0);
            var n2 = graph.AddNode(1, 1);

            var path = AStar.Search(graph, n1, n2);

            Assert.IsNull(path);
        }
    }
}