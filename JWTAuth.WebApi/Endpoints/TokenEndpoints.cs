namespace JWTAuth.WebApi.Endpoints;

public static class TokenEndpoints
{
    private const string BaseApiUrl = @"api/tokens";

    public static IEndpointRouteBuilder MapTokenEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(BaseApiUrl, Post);
        return app;
    }

    private static async Task<IResult> Post(UserInfo userData, [FromServices] IConfiguration config, [FromServices] DatabaseContext context)
    {
        if (userData is null || userData.Email is null || userData.Password is null)
        {
            return Results.BadRequest();
        }
        
        var user = await GetUser(userData.Email, userData.Password, config, context);

        if (user is null)
        {
            return Results.BadRequest("Invalid credentials");
        }
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, config["Jwt:Subject"]),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim("UserId", user.UserId.ToString()),
            new Claim("DisplayName", user.DisplayName ?? string.Empty),
            new Claim("UserName", user.UserName ?? string.Empty),
            new Claim("Email", user.Email ?? string.Empty)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            config["Jwt:Issuer"],
            config["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: signIn);

        return Results.Ok(new JwtSecurityTokenHandler().WriteToken(token));
    }

    private static async Task<UserInfo?> GetUser(string email, string password, IConfiguration config, DatabaseContext context) =>
        await context.UserInfos.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
}
