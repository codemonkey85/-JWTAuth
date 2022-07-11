namespace JWTAuth.WebApi.Repository;

public class EmployeeRepository : IEmployees
{
    private readonly DatabaseContext _dbContext = new();

    public EmployeeRepository(DatabaseContext dbContext) => _dbContext = dbContext;

    public List<Employee> GetEmployeeDetails()
    {
        try
        {
            return _dbContext.Employees.ToList();
        }
        catch
        {
            throw;
        }
    }

    public Employee GetEmployeeDetails(int id)
    {
        try
        {
            var employee = _dbContext.Employees.Find(id);
            return employee ?? throw new ArgumentNullException();
        }
        catch
        {
            throw;
        }
    }

    public void AddEmployee(Employee employee)
    {
        try
        {
            _dbContext.Employees.Add(employee);
            _dbContext.SaveChanges();
        }
        catch
        {
            throw;
        }
    }

    public void UpdateEmployee(Employee employee)
    {
        try
        {
            _dbContext.Entry(employee).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
        catch
        {
            throw;
        }
    }

    public Employee DeleteEmployee(int id)
    {
        try
        {
            var employee = _dbContext.Employees.Find(id);

            if (employee != null)
            {
                _dbContext.Employees.Remove(employee);
                _dbContext.SaveChanges();
                return employee;
            }
            else
            {
                throw new ArgumentNullException(nameof(employee));
            }
        }
        catch
        {
            throw;
        }
    }

    public bool CheckEmployee(int id) => _dbContext.Employees.Any(e => e.EmployeeID == id);
}
