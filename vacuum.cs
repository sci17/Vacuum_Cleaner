using System;
using System.Data;

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
    private Random random = new Rabdom();
    public void Clean(Robot robot, Map map)
    {
        Console.WriteLine("Cleaning with Random Path Strategy");

        int TotalTiles = map.Rows * map.Cols;
        for (int i = 0; i < TotalTiles; i++)
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
            Console.WriteLine("No Cleaning strategy set!");
            return;
        }
        cleaningStrategy.Clean(this, map);
    }
    public void CleanTitle(int row, int col)
    {
        Console.WriteLine($"{Name} cleaned tile at ({row}, {col})");
    }
}

public class Map
{
    public int Rows { get; }
    public int Cols { get; }

    public Map(int rows, int cols)
    {
        Rows = rows;
        Cols = cols;
    }
} 
