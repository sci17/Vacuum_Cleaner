using System;

public interface ICleaningStrategy
{
    void Clean(Robot robot, Map map);
}

public class SPatternStrategy : ICleaningStrategy
{
    public void Clean(Robot robot, Map map)
    {
        Console.WriteLine("Cleaning with S-Pattern Strategy...");
        for (int row = 0; row < map.Rows; row++)
        {
            if (row % 2 == 0)
            {
                for (int col = 0; col < map.Cols; col++)
                {
                    robot.CleanTile(row, col);
                }
            }
            else
            {
                for (int col = map.Cols - 1; col >= 0; col--)
                {
                    robot.CleanTile(row, col);
                }
            }
        }
    }
}

public class RandomPathStrategy : ICleaningStrategy
{
    private Random random = new Random(); 

    public void Clean(Robot robot, Map map)
    {
        Console.WriteLine("Cleaning with Random Path Strategy...");

        int totalTiles = map.Rows * map.Cols;
        for (int i = 0; i < totalTiles; i++)
        {
            int row = random.Next(map.Rows);
            int col = random.Next(map.Cols);
            robot.CleanTile(row, col);
        }
    }
}

public class Robot
{
    private ICleaningStrategy cleaningStrategy;
    public string Name { get; set; }

    public Robot(string name)
    {
        Name = name;
    }

    public void SetStrategy(ICleaningStrategy strategy)
    {
        cleaningStrategy = strategy;
    }

    public void StartCleaning(Map map)
    {
        if (cleaningStrategy == null)
        {
            Console.WriteLine("No cleaning strategy set!");
            return;
        }
        cleaningStrategy.Clean(this, map);
    }

    public void CleanTile(int row, int col) 
    {
        Console.WriteLine($"{Name} cleaned tile at ({row}, {col})");
    }
}

public class Map
{
    private bool[,] dirtyTiles;
    public int Rows { get; }
    public int Cols { get; }

    public Map(int rows, int cols)
    {
        Rows = rows;
        Cols = cols;
        dirtyTiles = new bool[rows, cols];

        Random random = new Random();
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                dirtyTiles[r, c] = random.Next(2) == 1;
            }
        }
    }
    public bool IsDirty(int row, int col) => dirtyTiles[row, col];

    public void CleanTile(int row, int col)
    {
        dirtyTiles[Rows, col] = false;
    }
    public bool HasDirtyTiles()
    {
        foreach (bool dirty in dirtyTiles)
        {
            if (dirty) return true;
        }
        return false;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Map roomMap = new Map(3, 4);
        Robot vacuum = new Robot("Robot Vacuum");

        vacuum.SetStrategy(new SPatternStrategy());
        vacuum.StartCleaning(roomMap);

        Console.WriteLine();

        vacuum.SetStrategy(new RandomPathStrategy());
        vacuum.StartCleaning(roomMap);

        Console.WriteLine("\nCleaning demonstration completed.");
    }
}
