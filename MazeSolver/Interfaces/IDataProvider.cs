namespace MazeSolver.Interfaces;

public interface IDataProvider
{
    void Init();
    (int, int) GetSize();
    void StartReadData((int, int) size);
    string GetLine(int i);

    void ExposeError(string message);
}