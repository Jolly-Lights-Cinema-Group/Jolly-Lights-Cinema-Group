class Schedule
{
    public int? Id { get; set; }
    public int MovieRoomId { get; set; }
    public int MovieId { get; set; }
    public DateTime StartDate { get; set; }

    public Schedule(int movieRoomId, int movieId, DateTime startDate)
    {
        MovieRoomId = movieRoomId;
        MovieId = movieId;
        StartDate = startDate;
    }

    public Schedule(int id, int movieRoomId, int movieId, DateTime startDate)
        : this(movieRoomId, movieId, startDate)
    {
        Id = id;
    }
}