using JollyLightsCinemaGroup.DataAccess;
using Jolly_Lights_Cinema_Group.Enum;

namespace Jolly_Lights.Tests
{
    [TestClass]
    public class Movietest
    {
        [TestInitialize]
        public void Setup()
        {
            DatabaseManager.InitializeDatabase();
        }

        [TestMethod]
        public void Test_Addmovie_AddmovietoDB()
        {

            Movie movie = new Movie("TestMovie1", 90, 18, "Jason Mamoa");

            MovieRepository movierepository = new MovieRepository();

            bool result = movierepository.AddMovie(movie);

            Assert.IsTrue(result, "Addmovie should return true if insertion was successful.");

            Movie inserted = movierepository.GetMovieByTitle(movie);
            Assert.IsNotNull(inserted, "Movie should exist.");
            Assert.AreEqual("TestMovie1", inserted.Title, "TestMovie1 should be the title");
            Assert.AreEqual(90, inserted.Duration, "90 minutes should be the duration.");
            Assert.AreEqual(18, inserted.MinimumAge, "18 should be the minimum age.");
            Assert.AreEqual("Jason Mamoa", inserted.MovieCast, "Jason Mamoa should be the cast.");

            movierepository.DeleteMovie(movie);
        }


        [TestMethod]
        public void Test_Deletemovie_DeleteMoviefromDB()
        {

            Movie movie = new Movie("TestMovie1", 90, 18, "Jason Mamoa");

            MovieRepository movierepository = new MovieRepository();

            movierepository.AddMovie(movie);
            bool result = movierepository.DeleteMovie(movie);

            Assert.IsTrue(result, "Deletemovie should return true if deletion was successful.");

            Movie inserted = movierepository.GetMovieByTitle(movie);
            Assert.IsNull(inserted, "Movie should not exist in database.");

        }

        [TestMethod]
        [DoNotParallelize]
        public void Test_ShowAllMovies_ShowAllMoviesFromDB()
        {
            Movie movie1 = new Movie("TestMovie1", 90, 18, "Jason Mamoa");
            Movie movie2 = new Movie("TestMovie2", 90, 18, "Jason Mamoa");
            Movie movie3 = new Movie("TestMovie3", 90, 18, "Jason Mamoa");

            MovieRepository movieRepository = new MovieRepository();

            movieRepository.AddMovie(movie1);
            movieRepository.AddMovie(movie2);
            movieRepository.AddMovie(movie3);

            List<Movie> AllMovies = movieRepository.GetAllMovies();

            bool MovieTest1 = false;
            bool MovieTest2 = false;
            bool MovieTest3 = false;

            foreach (Movie movie in AllMovies)
            {
                if (movie.Title == movie1.Title) { MovieTest1 = true; }
                if (movie.Title == movie2.Title) { MovieTest2 = true; }
                if (movie.Title == movie3.Title) { MovieTest3 = true; }
            }

            Assert.IsTrue(MovieTest1, $"Title: TestMovie1 should have been detected.");
            Assert.IsTrue(MovieTest2, $"Title: TestMovie2 should have been detected.");
            Assert.IsTrue(MovieTest3, $"Title: TestMovie3 should have been detected.");

            movieRepository.DeleteMovie(movie1);
            movieRepository.DeleteMovie(movie2);
            movieRepository.DeleteMovie(movie3);
        }
    }
}
