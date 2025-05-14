using Fundo.Applications.Domain.Interfaces;
using Fundo.Applications.Domain.Models;
using Fundo.Applications.Domain.Validations;
using Fundo.Applications.Repository.Entity;
using Fundo.Applications.Repository.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Fundo.Applications.Domain.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IApplicantRepository _applicantRepository;

        public JwtTokenService(IConfiguration configuration, IApplicantRepository applicantRepository)
        {
            _configuration = configuration;
            _applicantRepository = applicantRepository;
        }

        public async Task<string> LoginUserAsync(RequestLogin requestLogin)
        {
            UserValidationLogin validation = new();

            var validationResult = await validation.ValidateAsync(requestLogin);

            if (!validationResult.IsValid)
                return String.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage).ToList());

            var foundUser = await _applicantRepository.LoginApplicantAsync(requestLogin.User, requestLogin.Password);

            return (foundUser == null) ? "" : GenerateJwtToken(username: foundUser.User);
        }

        private string GenerateJwtToken(string username)
        {
            string key = _configuration.GetSection("jwt:secretKey").Value!;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var symmetrickey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(symmetrickey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "yourdomain.com",
                audience: "yourdomain.com",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
