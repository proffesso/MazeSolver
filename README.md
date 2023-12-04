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

Path solving algorithm used quite simple (Breadth-first search). Implementation targeted possibility to replace algorithm according to the needs. 
Using algorithms like `A*` could be more official for particular task, but firstly, there is not specified diagonal movements possibility and also design requirements usually going into unpromising one direction and weighed edges and makes `A*` and grid based algorithms useless.

Complexity of implemented the algorithm could vary from O(n) to O(n²) in worst case (had to check all nodes).

### Lage mazes.
Steps done for solving lage amount of data:
* Read data extracted to separate class this allow to free the memory from the Raw data one file processed (garbage collector is not acting here yet, but implementation supports this with just extending `IDataProvider` interface ).
* Also reading of the data implemented in the line-by-line (supported be interface) that's possible allows to avoid store all file in the memory (not fully implemented reading line-by-line)
* Storage for algorithm used a List for Nodes and Edges, this is about 2^31 items, could be lower depending on the memory size. 
* Printing original interfaces are implemented with help of the single mapping between graph nodes Ids and original Maze cell (no need to reread original data again, storing only open cells).  

## Multi paths [not implemented] 

Multi paths for solving the maze could be done but vary weight of the original graph, that's does not bring any harm to the stored dtaa and no need to back up.

## Testing

There are simple tests in solution. Those tests are covering small part of the solution adding  tests and gathering high code coverage was not priority tasks, but still possible with some efforts
(improvements are endless possible of course). 