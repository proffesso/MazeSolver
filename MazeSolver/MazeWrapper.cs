using System.Text.RegularExpressions;
using MazeSolver.Interfaces;
using MazeSolver.Models;

namespace MazeSolver;

public class MazeWrapper
{
    private readonly IDataProvider _dataProvider;
    private const string REGEX_FOR_MAZE_DEFINITION = "^[01]+$";

    private readonly Dictionary<int, int> MappingMazeToNode = new();
    private readonly Graph Graph = new();
    private (int, int) _size;

    public MazeWrapper(IDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }
    public (Graph, Node, Node) Read()
    {
        
        _size = _dataProvider.GetSize();
        
        _dataProvider.StartReadData(_size);
        
        int i = 0;
        while (i < _size.Item1)
        {
            var line = _dataProvider.GetLine(i+1).Trim();
            if (line?.Length != _size.Item2)
            {
                _dataProvider.ExposeError($"Wrong input: Provided line does not match expected size {_size.Item2}");
                continue;
            }

            if (!Regex.IsMatch(line, REGEX_FOR_MAZE_DEFINITION))
            {
                _dataProvider.ExposeError("Wrong input: line has symbols other the '0' or '1' symbol.");
                break;
            }

            for (int j = 0; j < line.Length; j++)
            {
                if (line[j] == '0') // open
                {
                    var node = Graph.AddNode();
                    MappingMazeToNode.Add(i * _size.Item2 + j, node.Id);

                    if (i > 0 && MappingMazeToNode.TryGetValue((i - 1) * _size.Item2 + j, out int prevI))
                    {
                        Graph.AddEdge(node.Id, prevI, 1);
                        Graph.AddEdge(prevI, node.Id, 1);
                    }

                    if (j > 0 && MappingMazeToNode.TryGetValue(i * _size.Item2 + j - 1, out int prevJ))
                    {
                        Graph.AddEdge(node.Id, prevJ, 1);
                        Graph.AddEdge(prevJ, node.Id, 1);
                    }
                }
            }
            i++;
        }
        return (Graph, Graph.nodes.First(), Graph.nodes.Last());
    }

    private const string PATH = "x";
    private const string WALL = "█";
    private const string OPEN = "░";
    
    public void Print(IList<Edge>? path)
    {
                
        List<int> nodes = new List<int>();
        if (path != null)
        {
            nodes.Add(path.First().startNode);
            foreach (var edge in path)
            {
                nodes.Add(edge.endNode);
            }
            nodes.Sort();
        }

        var nodesCursor = 0;
        Console.WriteLine();
        
        for (int i = 0; i < _size.Item1; i++)
        {
            Console.Write("|");

            for (int j = 0; j < _size.Item2; j++)
            {
                if (MappingMazeToNode.TryGetValue(i * _size.Item2 + j, out int nodeId))
                {
                    while (nodesCursor < nodes.Count && nodeId > nodes[nodesCursor])
                        nodesCursor++;

                    Console.Write(nodeId == nodes[nodesCursor] ? PATH : OPEN);
                }
                else
                {
                    Console.Write(WALL);
                }
            }
            Console.WriteLine("|");
        }
    }
}