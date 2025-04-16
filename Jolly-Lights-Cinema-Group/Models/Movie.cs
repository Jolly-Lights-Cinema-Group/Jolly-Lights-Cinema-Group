public class Movie
{
    public int? Id { get; set; }
    public string? Title {get; set;}
    public int? Duration {get; set;}
    public int? MinimumAge {get; set;}
    public string? MovieCast;

    public Movie(string title, int duration,int minimumAge,string movieCast)
    {
        Title = title;
        Duration = duration;
        MinimumAge = minimumAge;
        MovieCast = movieCast;
    }

    public Movie(int id, string title, int duration,int minimumAge,string movieCast)
        : this(title, duration, minimumAge, movieCast)
    {
        Id = id;
    }
}