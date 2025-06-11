using Jolly_Lights_Cinema_Group.Enum;

namespace JollyLightsCinemaGroup.DataAccess;

public class MovieRoomRepository
{
    public bool AddMovieRoom(MovieRoom movieRoom)
    {
        using (var connection = DatabaseManager.GetConnection())
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                    INSERT INTO MovieRoom (RoomNumber, RoomLayoutJson, SupportedMovieType, LocationId)
                    VALUES (@roomNumber, @roomLayoutJson, @supportedMovieType, @locationId);";

            command.Parameters.AddWithValue("@roomNumber", movieRoom.RoomNumber);
            command.Parameters.AddWithValue("@roomLayoutJson", movieRoom.RoomLayoutJson);
            command.Parameters.AddWithValue("@supportedMovieType", movieRoom.SupportedMovieType);
            command.Parameters.AddWithValue("@locationId", movieRoom.LocationId);

            return command.ExecuteNonQuery() > 0;
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
                    var room = new MovieRoom(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), (MovieType)reader.GetInt32(3), reader.GetInt32(4));
                    movieRooms.Add(room);
                }
            }
        }

        return movieRooms;
    }

    public bool DeleteMovieRoom(MovieRoom movieRoom)
    {
        using var connection = DatabaseManager.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM MovieRoom WHERE Id = @id;";
        command.Parameters.AddWithValue("@id", movieRoom.Id);

        return command.ExecuteNonQuery() > 0;
    }    
    
    public virtual string GetRoomLayoutJson(int id)
    {
        var movieRoomLayout = "";

        using (var connection = DatabaseManager.GetConnection())
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                "SELECT RoomLayoutJson FROM MovieRoom WHERE Id = @id;";
            command.Parameters.AddWithValue("@id", id);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    movieRoomLayout = reader.GetString(0);
                }
            }
        }

        return movieRoomLayout;
    }

    public virtual MovieRoom? GetMovieRoomById(int id)
    {
        using (var connection = DatabaseManager.GetConnection())
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Id, RoomNumber, RoomLayoutJson, SupportedMovieType, LocationId
                FROM MovieRoom
                WHERE Id = @id;";

            command.Parameters.AddWithValue("@id", id);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    MovieRoom movieRoom = new MovieRoom(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2),
                        (MovieType)reader.GetInt32(3), reader.GetInt32(4));
                    return movieRoom;
                }
            }
        }
        return null;
    }
}