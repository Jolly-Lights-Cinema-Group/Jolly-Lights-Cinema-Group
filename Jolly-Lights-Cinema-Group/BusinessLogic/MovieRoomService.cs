using JollyLightsCinemaGroup.DataAccess;
using Jolly_Lights_Cinema_Group.Helpers;

public class MovieRoomService
{
    private readonly MovieRoomRepository _movieRoomRepo;
    private readonly ScheduleSeatRepository _scheduleSeatRepository;

    public MovieRoomService(MovieRoomRepository? movieRoomRepository = null, ScheduleSeatRepository? scheduleSeatRepository = null)
    {
        _movieRoomRepo = movieRoomRepository ?? new MovieRoomRepository();
        _scheduleSeatRepository = scheduleSeatRepository ?? new ScheduleSeatRepository();
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

    public virtual int GetLeftOverSeats(Schedule schedule)
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

        List<ScheduleSeat> reservedSeats = _scheduleSeatRepository.GetSeatsBySchedule(schedule.Id!.Value);

        int reservedCount = reservedSeats.Count;
        int freeSeats = totalSeats - reservedCount;

        return freeSeats;
    }
}
