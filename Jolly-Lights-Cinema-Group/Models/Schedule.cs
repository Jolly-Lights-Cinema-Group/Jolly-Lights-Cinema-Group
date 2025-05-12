public class Schedule
{
    public int? Id { get; set; }
    public int MovieRoomId { get; set; }
    public int MovieId { get; set; }
    public DateTime StartDate { get; set; }
    public TimeSpan StartTime { get; set; }

    public Schedule(int movieRoomId, int movieId, DateTime startDate, TimeSpan startTime)
    {
        MovieRoomId = movieRoomId;
        MovieId = movieId;
        StartDate = startDate;
        StartTime = startTime;
    }

    public Schedule(int id, int movieRoomId, int movieId, DateTime startDate, TimeSpan startTime)
        : this(movieRoomId, movieId, startDate, startTime)
    {
        Id = id;
    }
}