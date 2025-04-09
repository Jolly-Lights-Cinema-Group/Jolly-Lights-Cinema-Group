class MovieRoom
{
    public int RoomNumber { get; set; }
    public string? RoomLayoutJson { get; set; }
    public int SupportedMovieType { get; set; }
    public int LocationId { get; set; }

    public MovieRoom(int roomnumber, string roomlayoutjson, int supportedmovietype, int locationid)
    {
        RoomNumber = roomnumber;
        RoomLayoutJson = roomlayoutjson;
        SupportedMovieType = supportedmovietype;
        LocationId = locationid;
    }
}