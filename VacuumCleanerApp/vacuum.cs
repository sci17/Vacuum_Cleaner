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
// public class IntelligentStrategy : ICleaningStrategy
// {
//     public void Clean(Robot robot, Map map)
//     {
//         Console.WriteLine("Cleaning with Intelligent Strategy");

//         while (map.HasDirtyTiles())
//         {
//             for (int row = 0; row < map.Rows; row++)
//             {
//                 for (int col = 0; col < map.Cols; col++)
//                 {
//                     if (map.IsDirty(row, col))
//                     {
//                         robot.CleanTile(row, col,map);
//                     }
//                 }
//             }
//         }
//         Console.WriteLine("All dirty tiles have been cleaned intelligently!");
//     }
// }
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
        if (map.IsObstacle(row, col))
        {
            Console.WriteLine();
            return;
        }
        map.Display(row, col);

        if (map.IsDirty(row, col))
        {
            Console.WriteLine();
            map.CleanTile(row, col);
        }
        else
        {
            Console.WriteLine();
        }
        Thread.Sleep(400);
    }
}

public class Map
{
    private bool[,] dirtyTiles;
    private bool[,] obstacles;
    public int Rows { get; }
    public int Cols { get; }

    public Map(int rows, int cols)
    {
        Rows = rows;
        Cols = cols;
        dirtyTiles = new bool[rows, cols];
        obstacles = new bool[rows, cols];

        Random random = new Random();
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                dirtyTiles[r, c] = random.Next(2) == 1;
                obstacles[r, c] = random.Next(10) == 0;
            }
        }
    }
    public bool IsDirty(int row, int col) => IsValid(row, col) && dirtyTiles[row, col];
    public bool IsObstacle(int row, int col) => IsValid(row, col) && obstacles[row, col];

    public void CleanTile(int row, int col)
    {
        if (IsValid(row, col))
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
                else if (obstacles[r, c])
                    Console.Write(" # ");
                else
                    Console.Write(" . ");

            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
    public bool HasDirtyTiles()
    {
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Cols; c++)
            {
                if (dirtyTiles[r, c] && !obstacles[r, c])
                {
                    return true;
                }
            }
        }
        return false;
    }
    private bool IsValid(int row, int col) => row >= 0 && row < Rows && col < Cols;
}

public class Program
{
    public static void Main(string[] args)
    {
        Map roomMap = new Map(5, 5);
        Robot vacuum = new Robot("Robot Vacuum");

        vacuum.SetStrategy(new SPatternStrategy());
        vacuum.StartCleaning(roomMap);
        Console.WriteLine("S-Pattern Done. Press any key for Random Strategy.");
        Console.ReadKey();

        vacuum.SetStrategy(new RandomPathStrategy());
        vacuum.StartCleaning(roomMap);
        Console.WriteLine("Random Strategy done.");
        Console.ReadKey();

        Console.WriteLine("\nCleaning demonstration completed.");
    }
}
