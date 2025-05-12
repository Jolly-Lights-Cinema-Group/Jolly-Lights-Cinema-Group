public class MovieRoom
{
    public int? Id { get; set; }
    public int RoomNumber { get; set; }
    public string? RoomLayoutJson { get; set; }
    public int SupportedMovieType { get; set; }
    public int LocationId { get; set; }

    public MovieRoom(int roomNumber, string roomLayoutJson, int supportedMovieType, int locationId)
    {
        RoomNumber = roomNumber;
        RoomLayoutJson = roomLayoutJson;
        SupportedMovieType = supportedMovieType;
        LocationId = locationId;
    }

    public MovieRoom(int id, int roomNumber, string roomLayoutJson, int supportedMovieType, int locationId)
        : this(roomNumber, roomLayoutJson, supportedMovieType, locationId)
    {
        Id = id;
    }
}