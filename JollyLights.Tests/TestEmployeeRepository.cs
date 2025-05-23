using JollyLightsCinemaGroup.DataAccess;
using Jolly_Lights_Cinema_Group.Enum;

namespace Jolly_Lights.Tests
{
    [TestClass]
    public class Employeetest
    {
        private string? _tempDir;

        [TestInitialize]
        public void Setup()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempDir);

            string testSchemaPath = Path.Combine(_tempDir, "schema.sql");

            var originalSchemaPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Jolly-Lights-Cinema-Group", "Database", "schema.sql");
            File.Copy(originalSchemaPath, testSchemaPath);

            DatabaseManager.OverridePaths(_tempDir);

            DatabaseManager.InitializeDatabase();
        }

        [TestMethod]
        public void Test_Adduser_AddUsertoDB()
        {

            Employee employee = new Employee("Test", "Employee", "01-01-1111", "Testway 32", "test.employee@email.com", "test_subject_AddEmployee", "testemployee", Role.Employee);

            EmployeeRepository employeerepository = new EmployeeRepository();

            bool result = employeerepository.AddEmployee(employee);

            Assert.IsTrue(result, "AddEmployee should return true if insertion was successful.");

            Employee? inserted = employeerepository.GetEmployeeByUsername(employee.UserName, employee.FirstName, employee.LastName);
            Assert.IsNotNull(inserted, "Employee should exist in database.");
            Assert.AreEqual("Test", inserted.FirstName, "Test Should be the firstname.");
            Assert.AreEqual("Employee", inserted.LastName, "Employee Should be the LastName.");
            Assert.AreEqual("01-01-1111", inserted.DateofBirth, "01-01-1111 should be the DateOfBirth");
            Assert.AreEqual("Testway 32", inserted.Address, "Testway 32 should be the adress.");
            Assert.AreEqual("test.employee@email.com", inserted.Email, "test.employee@email.com should be the email");
            Assert.AreEqual("test_subject_AddEmployee", inserted.UserName, "test_subject_AddEmployee should be the username");
            Assert.AreEqual(Role.Employee, inserted.Role, "Employee Should be the Role");

            Assert.AreNotEqual("testemployee", inserted.Password);
            Assert.IsTrue(BCrypt.Net.BCrypt.Verify("testemployee", inserted.Password));
        }

        [TestMethod]
        public void Test_DeleteUser_DeleteUserFromDB()
        {
            Employee employee = new Employee("Test2", "Employee", "01-01-1111", "Testway 32", "test.employee@email.com", "test_subject_DeleteEmployee", "testemployee", Role.Employee);
            EmployeeRepository employeerepository = new EmployeeRepository();

            employeerepository.AddEmployee(employee);
            Employee? employeeToDelete = employeerepository.GetEmployeeByUsername(employee.UserName, employee.FirstName, employee.LastName);

            bool result = employeerepository.DeleteEmployee(employeeToDelete!);

            Assert.IsTrue(result, "User Deleted from the database");
        }

        [TestMethod]
        [DoNotParallelize]
        public void Test_ShowAllEmployees_ShowAllEmployeesFromDB()
        {
            Employee employee1 = new Employee("Test2", "Employee", "01-01-1111", "Testway 32", "test.employee@email.com", "test1_subject_Showallemployees1", "testemployee", Role.Employee);
            Employee employee2 = new Employee("Test2", "Employee", "01-01-1111", "Testway 32", "test.employee@email.com", "test2_subject_Showallemployees2", "testemployee", Role.Employee);
            Employee employee3 = new Employee("Test2", "Employee", "01-01-1111", "Testway 32", "test.employee@email.com", "test3_subject_Showallemployees3", "testemployee", Role.Employee);

            EmployeeRepository employeerepository = new EmployeeRepository();

            employeerepository.AddEmployee(employee1);
            employeerepository.AddEmployee(employee2);
            employeerepository.AddEmployee(employee3);

            List<Employee> allemployees = employeerepository.GetAllEmployees();

            bool EmployeeTest1 = false;
            bool EmployeeTest2 = false;
            bool EmployeeTest3 = false;

            foreach (Employee emp in allemployees)
            {
                if (emp.UserName == employee1.UserName) { EmployeeTest1 = true; }
                if (emp.UserName == employee2.UserName) { EmployeeTest2 = true; }
                if (emp.UserName == employee3.UserName) { EmployeeTest3 = true; }
            }

            Assert.IsTrue(EmployeeTest1, $"Username: test1_subject_Showallemployees1  should have been detected.");
            Assert.IsTrue(EmployeeTest2, $"Username: test1_subject_Showallemployees2  should have been detected.");
            Assert.IsTrue(EmployeeTest3, $"Username: test1_subject_Showallemployees3  should have been detected.");
        }

        [TestMethod]
        public void Test_ChangeFirstName_ChangingFirstNameOfEmployee()
        {
            Employee employee = new Employee("Test2", "Employee", "01-01-1111", "Testway 32", "test.employee@email.com", "test1_subject_ChangeFirstname", "testemployee", Role.Employee);
            EmployeeRepository employeerepository = new EmployeeRepository();

            employeerepository.AddEmployee(employee);

            Employee? FirstnameNotchanged = employeerepository.GetEmployeeByUsername(employee.UserName, employee.FirstName, employee.LastName);
            Assert.IsNotNull(FirstnameNotchanged, "Employee with first name not changed not found in database");
            Assert.AreEqual("Test2", FirstnameNotchanged.FirstName, "Username should be Test2.");

            employeerepository.ChangeFirstNameDB("ChangedUser", employee.UserName);
            Employee? FirstnameChanged = employeerepository.GetEmployeeByUsername(employee.UserName, employee.FirstName, employee.LastName);

            Assert.IsNotNull(FirstnameChanged, "Employee with first name changed not found in database");
            Assert.AreEqual("ChangedUser", FirstnameChanged.FirstName, "Test2 should be changed to ChangedUser");
        }

        [TestMethod]
        public void Test_ChangeLastName_ChangingLastNameOfEmployee()
        {
            Employee employee = new Employee("Test2", "Employee", "01-01-1111", "Testway 32", "test.employee@email.com", "test1_subject_ChangeLastname", "testemployee", Role.Employee);
            EmployeeRepository employeerepository = new EmployeeRepository();

            employeerepository.AddEmployee(employee);

            Employee? LastnameNotchanged = employeerepository.GetEmployeeByUsername(employee.UserName, employee.FirstName, employee.LastName);
            Assert.IsNotNull(LastnameNotchanged, "Employee with last name not changed not found in database");
            Assert.AreEqual("Employee", LastnameNotchanged.LastName, "Lastname should be Employee.");

            employeerepository.ChangeLastNameDB("ChangedUser", employee.UserName);
            Employee? LastnameChanged = employeerepository.GetEmployeeByUsername(employee.UserName, employee.FirstName, employee.LastName);

            Assert.IsNotNull(LastnameChanged, "Employee with last name changed not found in database");
            Assert.AreEqual("ChangedUser", LastnameChanged.LastName, "Employee should be changed to ChangedUser");
        }

        [TestMethod]
        public void Test_ChangeEmail_ChangingEmailOfEmployee()
        {
            Employee employee = new Employee("Test2", "Employee", "01-01-1111", "Testway 32", "test.employee@email.com", "test1_subject_Changeemail", "testemployee", Role.Employee);
            EmployeeRepository employeerepository = new EmployeeRepository();

            employeerepository.AddEmployee(employee);

            Employee? EmailNotchanged = employeerepository.GetEmployeeByUsername(employee.UserName, employee.FirstName, employee.LastName);
            Assert.IsNotNull(EmailNotchanged, "Employee with email not changed  not found in database");
            Assert.AreEqual("test.employee@email.com", EmailNotchanged.Email, "Email should be test.employee@email.com.");

            employeerepository.ChangeEmailDB("Changedemail@email.com", employee.UserName);
            Employee? EmailChanged = employeerepository.GetEmployeeByUsername(employee.UserName, employee.FirstName, employee.LastName);

            Assert.IsNotNull(EmailChanged, "Employee with email changed not found in database");
            Assert.AreEqual("Changedemail@email.com", EmailChanged.Email, "Employee should be changed to ChangedUser");
        }

        [TestMethod]
        public void Test_ChangePassword_ChangingPasswordOfEmployee()
        {
            Employee employee = new Employee("Test2", "Employee", "01-01-1111", "Testway 32", "test.employee@email.com", "test1_subject_ChangePassword", "testemployee", Role.Employee);
            EmployeeRepository employeerepository = new EmployeeRepository();

            employeerepository.AddEmployee(employee);

            Employee? PasswordNotchanged = employeerepository.GetEmployeeByUsername(employee.UserName, employee.FirstName, employee.LastName);
            Assert.IsNotNull(PasswordNotchanged, "Employee with password not changed not found in database");
            Assert.IsTrue(BCrypt.Net.BCrypt.Verify("testemployee", PasswordNotchanged.Password), "Password should match the newly changed password.");

            employeerepository.ChangePasswordDB("Changedpassword", employee.UserName);
            Employee? PasswordChanged = employeerepository.GetEmployeeByUsername(employee.UserName, employee.FirstName, employee.LastName);

            Assert.IsNotNull(PasswordChanged, "Employee with password changed not found in database");
            Assert.IsTrue(BCrypt.Net.BCrypt.Verify("Changedpassword", PasswordChanged.Password), "Password should match the newly changed password.");
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (_tempDir != null && Directory.Exists(_tempDir))
            {
                for (int i = 0; i < 5; i++)
                {
                    try
                    {
                        Directory.Delete(_tempDir, recursive: true);
                        break;
                    }
                    catch (IOException)
                    {
                        Thread.Sleep(1); // If cinemaDB is still busy, then wait 1ms and try again
                    }
                }
            }
        }
    }
}
