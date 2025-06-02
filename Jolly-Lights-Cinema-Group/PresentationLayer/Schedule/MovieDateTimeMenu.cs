namespace Jolly_Lights_Cinema_Group
{
    public class MovieDateTimeMenu
    {
        private readonly ScheduleService _scheduleService = new();

        public Schedule? SelectSchedule(Movie selectedMovie, int locationId)
        {
            var groupedSchedules = _scheduleService.GroupedSchedules(selectedMovie, locationId);
            Schedule? selectedSchedule = null;

            while (true)
            {
                string[] dateMenuItems = groupedSchedules
                    .Select(g => g.Key.ToString("dddd dd MMMM yyyy"))
                    .Append("Cancel")
                    .ToArray();

                Menu dateMenu = new("Choose a date:", dateMenuItems);
                int dateChoice = dateMenu.Run();

                if (dateChoice == groupedSchedules.Count)
                {
                    Console.WriteLine("Canceled.");
                    return null;
                }

                var selectedDateGroup = groupedSchedules[dateChoice];

                MovieRoomService movieRoomService = new();

                List<Schedule> timesForDate = selectedDateGroup
                    .OrderBy(s => s.StartTime)
                    .Where(s => movieRoomService.GetLeftOverSeats(s) > 0)
                    .ToList();

                string[] timeMenuItems = timesForDate
                    .Select(s => s.StartTime.ToString(@"hh\:mm"))
                    .Append("Go back to dates")
                    .ToArray();

                Menu timeMenu = new("Choose a time:", timeMenuItems);
                int timeChoice = timeMenu.Run();

                if (timeChoice == timesForDate.Count)
                {
                    continue;
                }

                selectedSchedule = timesForDate[timeChoice];

                return selectedSchedule;
            }
        }
    }
}

