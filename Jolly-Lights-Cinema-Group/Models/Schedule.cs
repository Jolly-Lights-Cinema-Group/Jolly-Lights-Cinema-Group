class Schedule
{
    public int MovieRoomId { get; set; }
    public int MovieId { get; set; }
    public DateTime StartDate { get; set; }

    public Schedule(int movieroomid, int movieid, DateTime datetime)
    {
        MovieRoomId = movieroomid;
        MovieId = movieid;
        StartDate = datetime;
    }
}