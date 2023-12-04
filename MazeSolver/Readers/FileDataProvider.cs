using MazeSolver.Interfaces;

namespace MazeSolver.Readers;

public class FileDataProvider: IDataProvider
{
    public string? FilePath
    {
        set
        {
            using (var sr = new StreamReader(value))
            {
                // Read the stream as a string, and write the string to the console.
                var fileContent = sr.ReadToEnd();
                _fileLines = fileContent.Split('\n').Select(_=>_.Trim()).ToList();
            }
        }
    }
    
    private List<string> _fileLines ;
    
    public void Init()
    {
        var fileProvided = false;
        while (!fileProvided)
        {
            try
            {
                Console.WriteLine("Provide path to File:");

                FilePath = Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            fileProvided =  _fileLines?.Count > 0 && LinesAreEqual(_fileLines);
            if (!fileProvided) 
                Console.WriteLine("The file format not expected. File empty or lines with different length.");
        }
    }

    private bool LinesAreEqual(List<string> fileLines)
    {
        var line0 = fileLines[0].Length;
        foreach (var line in _fileLines)
        {
            if (line.Length != line0) return false;
        }

        return true;
    }

    public (int, int) GetSize()
    {
        return (_fileLines.Count, _fileLines.Count > 0 ? _fileLines[0].Trim().Length : 0);
    }

    public void StartReadData((int, int) size)
    {
        
    }

    public string GetLine(int i)
    {
        return _fileLines[i-1].Trim();
    }

    public void ExposeError(string message)
    {
        Console.WriteLine(message);
    }
}