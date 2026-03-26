# MazeSolver

## Possible inputs:

After run application user will be able to choose a way to enter Maze.
There are two options: File or Manual input.

### File

For file input possible to use existing files :

* maze_test.txt
* maze_test_simply.txt

Those files used for the tests as well.
For providing files possible to provide absolute or relative path. 
Relative path in case application looks like:

>../../../../maze_test.txt

### Manual
In case user decided to use manual enter, application will be asking step by step for data, starting from size of the maze and then per line of the 0-s and 1-s representation of the maze.

## Data preparation
There are plenty of algorithms for path-finding tasks. I found most common case is a weighed graph, that's why for implementing I did convert entered char matrix into to more flexible graph.
Algorithm skips wall cells and creates Graph nodes only for open cells.
Similar approach with edges: going line by line: if discovered network has a connection, then this edges are creating in the graph.     

## Path Solving

MazeSolver ships with two algorithms for path solving:

* **Breadth-first search (BFS)** – explores the maze level by level. This is the default strategy used by the console application and guarantees the shortest path when all edges have the same weight.
* **A\*** – a heuristic-driven search that prioritizes nodes based on the sum of the current path cost and an estimated distance to the goal. Maze nodes now retain their grid coordinates so the default heuristic uses the Manhattan distance, which keeps the search tightly focused on the target.

Using A* in code is straightforward:

```csharp
var (graph, start, end) = mazeWrapper.Read();

// Default Manhattan heuristic (when coordinates are known)
var path = AStar.Search(graph, start, end);

// Custom heuristic (optional)
var pathWithCustomHeuristic = AStar.Search(
    graph,
    start,
    end,
    (current, goal) => Math.Sqrt(
        Math.Pow(current.Row - goal.Row, 2) + Math.Pow(current.Column - goal.Column, 2))
);
```

A* falls back to Dijkstra’s algorithm automatically when node coordinates are unknown (heuristic value `0`), so it remains safe to use with any graph produced by the solver.

### Large mazes.
Steps done for solving large amount of data:
* Read data extracted to separate class this allow to free the memory from the Raw data one file processed (garbage collector is not acting here yet, but implementation supports this with just extending `IDataProvider` interface ).
* Also reading of the data implemented in the line-by-line (supported be interface) that's possible allows to avoid store all file in the memory (not fully implemented reading line-by-line)
* Storage for algorithm used a List for Nodes and Edges, this is about 2^31 items, could be lower depending on the memory size. 
* Printing original interfaces are implemented with help of the single mapping between graph nodes Ids and original Maze cell (no need to reread original data again, storing only open cells).  

## Multi paths [not implemented] 

Multi paths for solving the maze could be done but vary weight of the original graph, that's does not bring any harm to the stored dtaa and no need to back up.

## Testing

There are simple tests in solution. Those tests are covering small part of the solution adding  tests and gathering high code coverage was not priority tasks, but still possible with some efforts
(improvements are endless possible of course). 

A dedicated `AStarTests` suite now validates correctness on both weighted and unweighted graphs, while `PerformanceTests` exercises the BFS and A* implementations on large grids. Performance-oriented checks are grouped under the `Performance` category and can be executed explicitly with:

```
dotnet test --filter TestCategory=Performance
```

Running `dotnet test` without filters will execute the full unit and performance test suites.