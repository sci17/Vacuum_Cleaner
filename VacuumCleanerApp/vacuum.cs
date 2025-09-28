using System;
using System.Threading;

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
                    robot.CleanTile(row, col,map);
                }
            }
            else
            {
                for (int col = map.Cols - 1; col >= 0; col--)
                {
                    robot.CleanTile(row, col,map);
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
            robot.CleanTile(row, col,map);
        }
    }
}
public class IntelligentStrategy : ICleaningStrategy
{
    public void Clean(Robot robot, Map map)
    {
        Console.WriteLine("Cleaning with Intelligent Strategy");

        while (map.HasDirtyTiles())
        {
            for (int row = 0; row < map.Rows; row++)
            {
                for (int col = 0; col < map.Cols; col++)
                {
                    if (map.IsDirty(row, col))
                    {
                        robot.CleanTile(row, col,map);
                    }
                }
            }
        }
        Console.WriteLine("All dirty tiles have been cleaned intelligently!");
    }
}
public class Robot
{
    private ICleaningStrategy? cleaningStrategy;
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

    public void CleanTile(int row, int col, Map map)
    {
        map.Display(row, col);

        if (map.IsDirty(row, col))
        {
            Console.WriteLine($"{Name} cleaned tile at ({row}, {col})");
            map.CleanTile(row, col);
        }
        else
        {
            Console.WriteLine($"{Name} moved to ({row}, {col}) - already clean.");
        }
        Thread.Sleep(400);
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
        dirtyTiles[row, col] = false;
    }

    public void Display(int robotRow, int robotCol)
    {
        Console.Clear();
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Cols; c++)
            {
                if (r == robotRow && c == robotCol)
                    Console.Write(" R ");
                else if (dirtyTiles[r, c])
                    Console.Write(" X ");
                else
                    Console.Write(" . ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
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

        Console.WriteLine("S-Pattern done. Press any key for Random Strategy. ");
        Console.ReadKey();


        vacuum.SetStrategy(new RandomPathStrategy());
        vacuum.StartCleaning(roomMap);

        Console.WriteLine("Random Strategy done, Press any key for Intelligent Strategy.");
        Console.ReadKey();

        vacuum.SetStrategy(new IntelligentStrategy());
        vacuum.StartCleaning(roomMap);

        Console.WriteLine("\nCleaning demonstration completed.");
    }
}
