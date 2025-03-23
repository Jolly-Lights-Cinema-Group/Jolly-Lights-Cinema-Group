using JollyLightsCinemaGroup.DataAccess;
using System;
using System.Collections.Generic;

public class MovieRoomService
{
    private readonly MovieRoomRepository _movieRoomRepo;

    public MovieRoomService()
    {
        _movieRoomRepo = new MovieRoomRepository();
    }
// CREATE TABLE IF NOT EXISTS MovieRoom (
//   Id INTEGER PRIMARY KEY AUTOINCREMENT,
//   RoomNumber INTEGER NOT NULL,
//   RoomLayoutJson TEXT NOT NULL,
//   SupportedMovieType INTEGER NOT NULL,
//   LocationId INTEGER NOT NULL,
//   FOREIGN KEY (LocationId) REFERENCES Location (Id) ON DELETE CASCADE
    public void RegisterMovieRoom(int roomNumber, string roomLayoutJson, int supportedMovieType, int locationId)
    {
        if (string.IsNullOrWhiteSpace(roomLayoutJson))
        {
            Console.WriteLine("Error: Room lay out cannot be empty.");
            return;
        }

        _movieRoomRepo.AddMovieRoom(roomNumber, roomLayoutJson, supportedMovieType, locationId);
    }

    public void ShowMoviesRoomsLocation(int locationId)
    {
        List<string> movieRooms = _movieRoomRepo.GetAllMovieRooms(locationId);
        if (movieRooms.Count == 0)
        {
            Console.WriteLine($"No movies rooms found for location with ID: {locationId}.");
        }
        else
        {
            Console.WriteLine("Movie Rooms:");
            foreach (var movieRoom in movieRooms)
            {
                Console.WriteLine(movieRoom);
            }
        }
    }
}
