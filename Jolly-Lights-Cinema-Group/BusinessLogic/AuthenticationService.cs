using Jolly_Lights_Cinema_Group.Common;

namespace Jolly_Lights_Cinema_Group;

public class AuthenticationService
{
    public bool Login(string userName, string password)
    {
        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            return false;
        
        var hashedPassword = 
    }

    public bool Logout()
    {
        Globals.CurrentUser = null;
        return true;
    }
}