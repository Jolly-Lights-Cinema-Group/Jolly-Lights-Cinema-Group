using Jolly_Lights_Cinema_Group.Enum;

namespace Jolly_Lights_Cinema_Group.Models;

public class User
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
    public bool ValidLogin  { get; set; }
}