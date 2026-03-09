using System;
using System.Collections.Generic;
using MazeSolver;
using Xunit;

namespace MazeSolver.tests
{
    public class LoopGraphTest
    {
        /// <summary>
        /// Test correct behaviour in case of a loop graph.
        /// The maze below contains a loop, so the solver should not get stuck and should find the shortest path.
        /// Maze layout:
        /// 0 0 0 0
        /// 1 1 0 1
        /// 0 0 0 1
        /// 1 0 1 0
        /// Start: (0,0), End: (3,3)
        /// 0 = open, 1 = wall
        /// </summary>
        [Fact]
        public void Solver_FindsShortestPath_InLoopGraph()
        {
            // Arrange: create a maze with a loop
            var maze = new int[,]
            {
                {0, 0, 0, 0},
                {1, 1, 0, 1},
                {0, 0, 0, 1},
                {1, 0, 1, 0}
            };
            var graphBuilder = new GraphBuilder();
            var graph = graphBuilder.BuildGraph(maze);

            // Act: solve from top-left to bottom-right
            var solver = new MazeSolverAlgorithm();
            var path = solver.FindPath(graph, new Cell(0, 0), new Cell(3, 3));

            // Assert: path should exist and be shortest (length 7)
            Assert.NotNull(path);
            Assert.True(path.Count > 0, "Path should not be empty");
            Assert.Equal(new Cell(0, 0), path[0]);
            Assert.Equal(new Cell(3, 3), path[path.Count - 1]);
            Assert.Equal(7, path.Count); // Shortest path in this maze is 7 steps

            // Check that path does not revisit any cell (no infinite loop)
            var visited = new HashSet<Cell>();
            foreach (var cell in path)
            {
                Assert.True(visited.Add(cell), $"Cell {cell} visited more than once");
            }
        }
    }
}