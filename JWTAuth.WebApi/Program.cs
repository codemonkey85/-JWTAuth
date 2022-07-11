var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var config = builder.Configuration;

services
    .AddDbContext<DatabaseContext>(options => options.UseSqlServer(config.GetConnectionString("dbConnection")))
    .AddTransient<IEmployees, EmployeeRepository>()
    .AddControllers();

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = config["Jwt:Audience"],
        ValidIssuer = config["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
    };
});

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

app.MapTokenEndpoints();
app.MapWeatherEndpoints();
app.MapEmployeeEndpoints();

if (app.Environment.IsDevelopment())
{
    app
        .UseSwagger()
        .UseSwaggerUI();
}

app
    .UseHttpsRedirection()
    .UseAuthentication()
    .UseAuthorization();

app.MapControllers();

app.Run();
