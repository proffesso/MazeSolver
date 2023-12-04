using MazeSolver;
using MazeSolver.Interfaces;
using MazeSolver.Readers;

ConsoleKeyInfo key;
do
{
    Console.WriteLine("How to read Maze data? ");
    Console.WriteLine("0 - manual, 1 - file path ");
    key = Console.ReadKey();
} while (!(key.KeyChar == '1' || key.KeyChar == '0'));

IDataProvider dataProvider = key.KeyChar == '0' ? new ManualDataProvider() : new FileDataProvider();

dataProvider.Init();

var mazeWrapper = new MazeWrapper(dataProvider);

var (maze, first, last) = mazeWrapper.Read();

Console.WriteLine("Maze entering successful");
Console.WriteLine("-------------------");
Console.WriteLine("Start searching for path...");

var path = BFS.Search(maze, first, last);

Console.WriteLine(path == null ? "Path does NOT exist!" : "Path exists!");

mazeWrapper.Print(path);