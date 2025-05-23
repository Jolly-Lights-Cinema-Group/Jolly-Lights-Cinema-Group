using JollyLightsCinemaGroup.DataAccess;

public class EmployeeService
{
    private readonly EmployeeRepository _employeeRepo;

    public EmployeeService()
    {
        _employeeRepo = new EmployeeRepository();
    }

    public bool RegisterEmployee(Employee employee)
    {
        return _employeeRepo.AddEmployee(employee);
    }

    public bool DeleteEmployee(Employee employee)
    {

        return _employeeRepo.DeleteEmployee(employee);
    }

    public List<Employee> ShowAllEmployees()
    {
        return _employeeRepo.GetAllEmployees();
    }

    public bool ChangeFirstName(string firstname, string username)
    {
        return _employeeRepo.ChangeFirstNameDB(firstname, username);
    }

    public bool ChangeLastName(string lastname, string username)
    {
        return _employeeRepo.ChangeLastNameDB(lastname, username);
    }

    public bool ChangeEmail(string email, string username)
    {
        return _employeeRepo.ChangeEmailDB(email, username);
    }

    public bool ChangePassword(string password, string username)
    {
        return _employeeRepo.ChangePasswordDB(password, username);
    }

    public bool UserNameExists(string userName)
    {
        return _employeeRepo.UserNameAlreadyExist(userName);
    }

    public Employee? GetEmployeeByUserName(string userName, string firstName, string lastName)
    {
        return _employeeRepo.GetEmployeeByUsername(userName, firstName, lastName);
    }
}
