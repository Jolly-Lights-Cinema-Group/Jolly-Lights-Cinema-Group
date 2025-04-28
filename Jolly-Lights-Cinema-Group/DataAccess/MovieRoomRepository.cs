namespace JollyLightsCinemaGroup.DataAccess;

public class MovieRoomRepository
{
    public void AddMovieRoom(int roomNumber, string roomLayoutJson, int supportedMovieType, int locationId)
    {
        using (var connection = DatabaseManager.GetConnection())
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                    INSERT INTO MovieRoom (RoomNumber, RoomLayoutJson, SupportedMovieType, LocationId)
                    VALUES (@roomNumber, @roomLayoutJson, @supportedMovieType, @locationId);";

            command.Parameters.AddWithValue("@roomNumber", roomNumber);
            command.Parameters.AddWithValue("@roomLayoutJson", roomLayoutJson);
            command.Parameters.AddWithValue("@supportedMovieType", supportedMovieType);
            command.Parameters.AddWithValue("@locationId", locationId);

            command.ExecuteNonQuery();
            Console.WriteLine("Movie room added successfully.");
        }
    }

    public List<MovieRoom> GetAllMovieRooms(int locationId)
    {
        var movieRooms = new List<MovieRoom>();

        using (var connection = DatabaseManager.GetConnection())
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                "SELECT Id, RoomNumber, RoomLayoutJson, SupportedMovieType, LocationId FROM MovieRoom WHERE LocationId = @LocationId;";
            command.Parameters.AddWithValue("@LocationId", locationId);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var room = new MovieRoom(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2),
                        reader.GetInt32(3), reader.GetInt32(4));
                    movieRooms.Add(room);
                }
            }
        }

        return movieRooms;
    }

    public void DeleteMovieRoom(int roomNumber, int locationId)
    {
        using var connection = DatabaseManager.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM MovieRoom WHERE RoomNumber = @RoomNumber AND LocationId = @LocationId;";
        command.Parameters.AddWithValue("@RoomNumber", roomNumber);
        command.Parameters.AddWithValue("@LocationId", locationId);
    }
}