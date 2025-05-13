using ClinicProject1.Data;
using ClinicProject1.Extensions;
using ClinicProject1.Models.DTOs.AuthDTOs;
using ClinicProject1.Models.Entities;
using ClinicProject1.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClinicProject1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(JWT jwtOptions, ClinicDbContext _context) : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        public ActionResult<String> login(string email, string password)
        {
            var validUser = _context.Users.FirstOrDefault(x => x.Email == email
            && x.Password == password);

            if (validUser != null)
            {
                var token = generateToken(validUser.Username, validUser.Role, validUser.UserId); 
                return Ok(token);
            }
            return Unauthorized("wrong password or username");
        }

        private ActionResult<String> generateToken(string username, Role role, int id)
        {
            var key = Encoding.UTF8.GetBytes(jwtOptions.SigningKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtOptions.Issuer,
                Audience = jwtOptions.Audience,
                SigningCredentials = new SigningCredentials
                (
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256
                 ),
                
                Subject = new ClaimsIdentity(new Claim[]
                    {
                        new(ClaimTypes.Name, username),
                        new(ClaimTypes.Role, role.ToString()),
                        new(ClaimTypes.NameIdentifier, id.ToString())
                    })
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<string>> register(RegisterPatientDto dto)
        {
            var userExist = _context.Users.FirstOrDefault(x => x.Email == dto.Email);
            if (userExist != null)
            {
                return BadRequest("User already exist");
            }

            string userName = $"{dto.FirstName}{dto.LastName}";

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Password = dto.Password,
                Username = userName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Role = Role.Patient
            };

            var patient = new Patient
            {
                User = user,
                Age = dto.Age,
                Gender = dto.Gender,
                ChronicDiseases = dto.ChronicDiseases,
                MedicalComplaint = dto.MedicalComplaint
            };

            _context.Patients.Add(patient);
            try
            {
                await _context.SaveChangesAsync();
                return Ok("registerd successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("logOut")]
        public ActionResult<string> logOut()
        {

            return Ok("logged out successfully");
        }
    }
}
