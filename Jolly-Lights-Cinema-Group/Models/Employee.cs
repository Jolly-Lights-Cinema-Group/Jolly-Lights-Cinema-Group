using Jolly_Lights_Cinema_Group.Enum;

public class Employee
{
    public int? Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DateofBirth { get; set; }
    public string? Address { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }

    public Employee(string firstName, string lastName, string dateOfBirth, string email, string userName, string password, Role role)
    {
        FirstName = firstName;
        LastName = lastName;
        DateofBirth = dateOfBirth;
        Email = email;
        UserName = userName;
        Password = password;
        Role = role;
    }

    public Employee(string firstName, string lastName, string dateOfBirth, string address, string email, string userName, string password, Role role) : this(firstName, lastName, dateOfBirth, email, userName, password, role)
    {
        Address = address;
    }

    public Employee(int id, string firstName, string lastName, string dateOfBirth, string? address, string email, string userName, string password, Role role)
        : this(firstName, lastName, dateOfBirth, address ?? "", email, userName, password, role)
    {
        Id = id;
    }
}
