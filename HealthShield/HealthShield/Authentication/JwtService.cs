using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Authentication;
using Microsoft.IdentityModel.Tokens;

namespace HealthShield.Authentication;

public class JwtService 
{
    public JwtDTO CreateToken(IConfiguration config, string email, string role)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub,
                config["Jwt:Subject"]),
            new Claim(JwtRegisteredClaimNames.Jti
                , Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat
                , DateTime.UtcNow.ToString()),
            new Claim("id", "1"),
            new Claim("email", email),
            new Claim("role", role),
        };
        
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        
        var signIn = new SigningCredentials(
            key, SecurityAlgorithms.HmacSha256);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(int.Parse(config["JWT:ExpireInDays"])),
            SigningCredentials = signIn,
            Issuer = config["JWT:Issuer"],
            Audience = config["JWT:Audience"]
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwt = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

        return new JwtDTO{ Token = tokenHandler.WriteToken(jwt) };
    }

    public JwtDTO CreateToken(IConfiguration config, UserResponse account, string role = "Client")
    {
        //create claims details based on the user information
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub,
                config["Jwt:Subject"]),
            new Claim(JwtRegisteredClaimNames.Jti
                , Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat
                , DateTime.UtcNow.ToString()),
            new Claim("id", account.Id.ToString()),
            new Claim("fullName", account.FullName),
            // new Claim("avatar", account.?? ""),
            new Claim("email", account.Email),
            new Claim("role", role),
            new Claim("status", account.Status),
        };
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        var signIn = new SigningCredentials(
            key, SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(int.Parse(config["JWT:ExpireInDays"])),
            SigningCredentials = signIn,
            Issuer = config["JWT:Issuer"],
            Audience = config["JWT:Audience"]
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwt = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

        // return new JwtDTO{Token = tokenHandler.WriteToken(jwt)};
        return new JwtDTO{ Token = tokenHandler.WriteToken(jwt)};
    }
}
