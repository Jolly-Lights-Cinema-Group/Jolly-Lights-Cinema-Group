using System.Text.Json;

namespace Jolly_Lights_Cinema_Group.Helpers;

public class MovieRoomJsonHelper
{
    public static List<List<string>> ReadGridFromFile(string? path)
    {
        var grid = new List<List<string>>();
        foreach (var line in File.ReadLines(path))
        {
            var row = new List<string>();
            var elements = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            row.AddRange(elements);
            grid.Add(row);
        }
        return grid;
    }

    public static void WriteGridToFile(List<List<string>> grid, string path)
    {
        using (var writer = new StreamWriter(path))
        {
            foreach (var row in grid)
            {
                writer.WriteLine(string.Join(' ', row));
            }
        }
    }

    public static string ConvertGridToJson(List<List<string>> grid)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        return JsonSerializer.Serialize(grid, options);
    }

    public static List<List<string>>? ConvertJsonToGrid(string json)
    {
        return JsonSerializer.Deserialize<List<List<string>>>(json);
    }
}