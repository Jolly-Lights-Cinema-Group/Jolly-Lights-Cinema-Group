using Jolly_Lights_Cinema_Group.Enum;

public class Employee
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DateofBirth { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }


    public Employee(string firstname, string lastName, string dateofBirth, string address, string email, string userName, string password, Role role) : this(firstname, lastName, dateofBirth, email, userName, password, role)
    {
        Address = address;
    }

    public Employee(string firstname, string lastName, string dateofBirth, string email, string userName, string password, Role role)
    {
        FirstName = firstname;
        LastName = lastName;
        DateofBirth = dateofBirth;
        Email = email;
        UserName = userName;
        Password = password;
        Role = role;
    }
}
