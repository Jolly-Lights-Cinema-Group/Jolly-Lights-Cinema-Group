using System.Text.Json;
public class Movie
{
    public string? Title {get; set;}
    public int Duration {get; set;}
    public int MinimumAge {get; set;}
    // public Dictionary<string,string>? MovieCast {get; set;} // Made this generic because of the MovieCastdict. Can be string-int or string-string etc.

    public Movie(string title, int duration,int minimumage)
    {
        Title = title;
        Duration = duration;
        MinimumAge = minimumage;
        // MovieCast = new Dictionary<string,string>();
    }

    // public void AddCast(string key, string value) //Adding cast manual.
    // {
    //     if (!MovieCast.ContainsKey(key))
    //     {
    //         MovieCast[key] = value;
    //     }
    // }

    // public void AddCast(string filepath) //Adding cast with JSON file.
    // {
    //     if (File.Exists(filepath))
    //     {
    //         string jsonContent = File.ReadAllText(filepath);
    //         MovieCast = JsonSerializer.Deserialize<Dictionary<string,string>>(jsonContent) ?? new Dictionary<string, string>();
    //         Console.WriteLine("JSON file loaded successfully.");
    //     }
    //     else
    //     {
    //         Console.WriteLine("File not Found.");
    //     }
    // }

}