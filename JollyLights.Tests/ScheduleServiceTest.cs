using System;
using Xunit;
using Moq;
using JollyLightsCinemaGroup.DataAccess;
using System.Collections.Generic;

public class ScheduleServiceTests
{
    private readonly Mock<ScheduleRepository> _mockScheduleRepo = new Mock<ScheduleRepository>();
    private readonly Mock<MovieRepository> _mockMovieRepo = new Mock<MovieRepository>();
    private readonly Mock<MovieRoomRepository> _mockMovieRoomRepo = new Mock<MovieRoomRepository>();
    private readonly ScheduleService _service;

    public ScheduleServiceTests()
    {
        _service = new ScheduleService(_mockScheduleRepo.Object, _mockMovieRepo.Object, _mockMovieRoomRepo.Object);
    }

    [Theory]
    [InlineData(true, true, true)]
    [InlineData(true, false, false)]
    [InlineData(false, true, false)]
    [InlineData(false, false, false)]
    public void CanAddSchedule_ReturnsExpectedResult(bool canAddAfter, bool canAddBefore, bool expected)
    {
        _mockScheduleRepo.Setup(repo => repo.CanAddScheduleAfter(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<TimeSpan>(), It.IsAny<int>()))
                         .Returns(canAddAfter);
        _mockScheduleRepo.Setup(repo => repo.CanAddScheduleBefore(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<TimeSpan>(), It.IsAny<int>()))
                         .Returns(canAddBefore);

        bool result = _service.CanAddSchedule(1, DateTime.Today, TimeSpan.FromHours(15), 10, 120);

        Xunit.Assert.Equal(expected, result);
    }
}