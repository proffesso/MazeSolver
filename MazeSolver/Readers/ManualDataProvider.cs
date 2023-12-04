using System.Text.RegularExpressions;
using MazeSolver.Interfaces;

namespace MazeSolver.Readers;

public class ManualDataProvider: IDataProvider
{
    private const string REG_EXP_SIZE_FORMAT = "^\\d+\\s\\d+$";

    public void Init()
    {
        Console.WriteLine("Manual mode chosen for Maze definition");
    }

    public (int, int) GetSize()
    {
        Console.WriteLine("Enter Maze size and Maze map.");
        
        while (true)
        {
            Console.WriteLine("Provide a maze size in format: 'n m'");
            var rawSize = Console.ReadLine()?.Trim();
            if (String.IsNullOrEmpty(rawSize))
            {
                Console.WriteLine("Wrong input: Provided size is Empty, pls try again");
                continue;
            }

            if (!Regex.IsMatch(rawSize, REG_EXP_SIZE_FORMAT))
            {
                Console.WriteLine("Wrong input: Provided size is wrong, pls try again");
                continue;
            }

            var sizeToParse = rawSize.Split(" ");
            try
            {
                return (int.Parse(sizeToParse[0]), int.Parse(sizeToParse[1]));
            }
            catch (Exception e)
            {
                Console.WriteLine("Wrong input: Provided size is wrong, pls try again");
            }
        }
    }

    public void StartReadData((int, int) size)
    {
        Console.WriteLine("Provide a Maze structure, using following rules:");
        Console.WriteLine($"1. Maze contains {size.Item1} lines");
        Console.WriteLine($"2. Each line contains {size.Item2} symbols. Symbol can be '0' or '1',");
        Console.WriteLine("where 0 - means cell is open, 1 - means wall, no pass allowed");
        Console.WriteLine("3. Top left and bottom right should be opened cells");
        Console.WriteLine("For, example, maze 3x4: ");
        Console.WriteLine("0111");
        Console.WriteLine("0111");
        Console.WriteLine("0000");
        Console.WriteLine("--------------------");
    }

    public string? GetLine(int i)
    {
        Console.WriteLine($"Enter line number {i}:");
        return Console.ReadLine();
    }

    public void ExposeError(string message)
    {
        Console.WriteLine(message);
    }
}