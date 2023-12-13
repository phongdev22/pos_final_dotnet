using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace pos.Services
{
	public class GenerateToken
	{
		private readonly IConfiguration _configuration;

		public GenerateToken(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string GenerateJwtToken(List<Claim> authClaims)
		{

			var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

			var token = new JwtSecurityToken(
				issuer: _configuration["JWT:Issuer"],
				audience: _configuration["JWT:Audience"],
				expires: DateTime.Now.AddHours(3),
				claims: authClaims,
				signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
				);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
