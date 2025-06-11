using System.Data;
using Jolly_Lights_Cinema_Group.Enum;
using Jolly_Lights_Cinema_Group.Models;

namespace JollyLightsCinemaGroup.DataAccess;

public class AuthenticationRepository
{
    public virtual User Login(string username, string password)
    {
        var user = new User();
        using (var connection = DatabaseManager.GetConnection())
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT UserName, Password, Role from Employee WHERE username = @username;";

            command.Parameters.AddWithValue("@username", username);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                user.UserName = reader.GetString(0);
                user.Password = reader.GetString(1);
                user.Role = reader.GetFieldValue<Role>(2);

                if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                    user.ValidLogin = true;
            }
        }

        return user;
    }
}