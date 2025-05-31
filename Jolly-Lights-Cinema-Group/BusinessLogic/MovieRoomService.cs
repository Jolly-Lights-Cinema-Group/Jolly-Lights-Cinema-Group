using JollyLightsCinemaGroup.DataAccess;
using Jolly_Lights_Cinema_Group.Helpers;
using JollyLightsCinemaGroup.BusinessLogic;

public class MovieRoomService
{
    private readonly MovieRoomRepository _movieRoomRepo;

    public MovieRoomService()
    {
        _movieRoomRepo = new MovieRoomRepository();
    }
    public bool RegisterMovieRoom(MovieRoom movieRoom)
    {
        return _movieRoomRepo.AddMovieRoom(movieRoom);
    }

    public bool DeleteRoom(MovieRoom movieRoom)
    {
        return _movieRoomRepo.DeleteMovieRoom(movieRoom);
    }

    public List<List<string>>? GetRoomLayout(int id)
    {
        var roomLayoutJson = _movieRoomRepo.GetRoomLayoutJson(id);
        return MovieRoomJsonHelper.ConvertJsonToGrid(roomLayoutJson);
    }

    public List<MovieRoom> GetMovieRooms(int locationId)
    {
        return _movieRoomRepo.GetAllMovieRooms(locationId);
    }

    public MovieRoom? GetMovieRoomById(int id)
    {
        return _movieRoomRepo.GetMovieRoomById(id);
    }

    public int GetLeftOverSeats(Schedule schedule)
    {
        List<List<string>>? movieRoomLayout = GetRoomLayout(schedule.MovieRoomId);
        if (movieRoomLayout == null) return 0;

        int totalSeats = 0;
        foreach (List<string> row in movieRoomLayout)
        {
            foreach (string seat in row)
            {
                if (seat != "#" && seat != "_")
                {
                    totalSeats++;
                }
            }
        }

        ScheduleSeatService scheduleSeatService = new();
        List<ScheduleSeat> reservedSeats = scheduleSeatService.ShowAllLScheduleSeats(schedule.Id!.Value);

        int reservedCount = reservedSeats.Count;

        int freeSeats = totalSeats - reservedCount;

        return freeSeats;
    }
}
