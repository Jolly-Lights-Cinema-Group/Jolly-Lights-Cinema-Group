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
        var movieRooms = _movieRoomRepo.GetAllMovieRooms(locationId);
        if (movieRooms.Count == 0)
        {
            Console.WriteLine($"No movies rooms found for location with ID: {locationId}.");
        }
        else
        {
            Console.WriteLine($"Movie Rooms at location: {locationId}:");
            foreach (var movieRoom in movieRooms)
            {
                Console.WriteLine($"ID: {movieRoom.Id}, Room number: {movieRoom.RoomNumber}, Supported movie type: {movieRoom.SupportedMovieType}");
            }
        }
    }
    
    public void DeleteRoom(int roomNumber, int locationId)
    {
        _movieRoomRepo.DeleteMovieRoom(roomNumber, locationId);
    }
}
