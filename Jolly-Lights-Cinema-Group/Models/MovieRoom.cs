using Jolly_Lights_Cinema_Group.Enum;

public class MovieRoom
{
    public int? Id { get; set; }
    public int RoomNumber { get; set; }
    public string? RoomLayoutJson { get; set; }
    public MovieType SupportedMovieType { get; set; }
    public int LocationId { get; set; }

    public MovieRoom(int roomNumber, string roomLayoutJson, MovieType supportedMovieType, int locationId)
    {
        RoomNumber = roomNumber;
        RoomLayoutJson = roomLayoutJson;
        SupportedMovieType = supportedMovieType;
        LocationId = locationId;
    }

    public MovieRoom(int id, int roomNumber, string roomLayoutJson, MovieType supportedMovieType, int locationId)
        : this(roomNumber, roomLayoutJson, supportedMovieType, locationId)
    {
        Id = id;
    }
}