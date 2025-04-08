public class Movie
{
    public string? Title {get; set;}
    public int? Duration {get; set;}
    public int? MinimumAge {get; set;}
    public string? MovieCast;

    public Movie(string title, int duration,int minimumage,string moviecast)
    {
        Title = title;
        Duration = duration;
        MinimumAge = minimumage;
        MovieCast = moviecast;
    }

}