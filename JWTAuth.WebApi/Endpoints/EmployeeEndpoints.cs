namespace JWTAuth.WebApi.Endpoints;

public static class EmployeeEndpoints
{
    private const string BaseApiUrl = @"api/employees";

    public static IEndpointRouteBuilder MapEmployeeEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(BaseApiUrl, GetAllEmployees);
        app.MapGet($"{BaseApiUrl}/{{id}}", GetEmployee);
        app.MapPost(BaseApiUrl, Post);
        app.MapPut($"{BaseApiUrl}/{{id}}", Put);
        app.MapDelete($"{BaseApiUrl}/{{id}}", Delete);
        return app;
    }

    [Authorize]
    private static async Task<IResult> GetAllEmployees([FromServices] IEmployees _IEmployee)
    {
        return Results.Ok(await Task.FromResult(_IEmployee.GetEmployeeDetails()));
    }

    [Authorize]
    private static async Task<IResult> GetEmployee(int id, [FromServices] IEmployees _IEmployee)
    {
        var employees = await Task.FromResult(_IEmployee.GetEmployeeDetails(id));
        return employees is null ? Results.NotFound() : Results.Ok(employees);
    }

    [Authorize]
    private static async Task<IResult> Post(Employee employee, [FromServices] IEmployees _IEmployee)
    {
        _IEmployee.AddEmployee(employee);
        //return await Task.FromResult(CreatedAtAction("GetEmployees", new { id = employee.EmployeeID }, employee));
        return Results.Ok(employee);
    }

    [Authorize]
    private static async Task<IResult> Put(int id, Employee employee, [FromServices] IEmployees _IEmployee)
    {
        if (id != employee.EmployeeID)
        {
            return Results.BadRequest();
        }
        try
        {
            _IEmployee.UpdateEmployee(employee);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EmployeeExists(id, _IEmployee))
            {
                return Results.NotFound();
            }
            else
            {
                throw;
            }
        }
        return Results.Ok(employee);
    }

    [Authorize]
    private static async Task<IResult> Delete(int id, [FromServices] IEmployees _IEmployee)
    {
        var employee = _IEmployee.DeleteEmployee(id);
        return Results.Ok(employee);
    }

    private static bool EmployeeExists(int id, IEmployees _IEmployee) => _IEmployee.CheckEmployee(id);
}
