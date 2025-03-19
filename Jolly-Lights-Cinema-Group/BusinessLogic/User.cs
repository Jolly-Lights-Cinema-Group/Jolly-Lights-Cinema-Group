namespace Jolly_Lights_Cinema_Group.Domain;

// Class for the user of the menu. 
// This class will set the location, get the role of the user and checks if the user is authenticated.


public class User
{
    public string? Location { get; set; }
    public string? Role { get; set; }
    public bool IsAuthenticated { get; set; }

    public User(string role, string location, bool isauthenticated)
    {
        Location = location;
        IsAuthenticated = isauthenticated;  // for test purpose will be true
        Role = role;
    }

    public void Authenticate() => IsAuthenticated = true;   // Will be from the database 

    public void SetLocation(string location) => Location = location;   

    public void SetRole(string role) => Role = role;   // Will be from the database.
}