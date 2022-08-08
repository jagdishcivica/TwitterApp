using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TwitterApp.Data;
using TwitterApp.Dtos;
using TwitterApp.Models;

namespace TwitterApp.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;

        public IConfiguration _configuration { get; }

        public AuthRepository(IMapper mapper ,DataContext dataContext, IConfiguration configuration)
        {
            _mapper = mapper;
            _dataContext = dataContext;
            _configuration = configuration;
        }
        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.LoginId.ToLower() == username.ToLower());
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong password";
            }
            else
            {
                response.Success = true;
                response.Message = "Login successful";
                response.Data = CreateToken(user);
            }
            return response;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            if (await UserExists(user.LoginId))
            {
                response.Success = false;
                response.Message = "User already exists!";
                return response;
            }
            
            if (await EmailExists(user.Email))
            {
                response.Success = false;
                response.Message = "Email already exists!";
                return response;
            }
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();

            response.Data = user.Id;
            response.Success = true;
            response.Message = "User registerd successfully.";
            return response;
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _dataContext.Users.AnyAsync(u => u.LoginId.ToLower() == username.ToLower()))
                return true;
            return false;
        }

        public async Task<bool> EmailExists(string email)
        {
            if (await _dataContext.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower()))
                return true;
            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.LoginId),
                new Claim(ClaimTypes.Email,user.Email)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(2),
                SigningCredentials = credentials
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<ServiceResponse<List<UserListDto>>> UserList()
        {
            ServiceResponse<List<UserListDto>> serviceResponse = new ServiceResponse<List<UserListDto>>();
            serviceResponse.Data = await _dataContext.Users.Select(x => _mapper.Map<UserListDto>(x)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<UserListDto>>> SearchUser(string username)
        {
            ServiceResponse<List<UserListDto>> serviceResponse = new ServiceResponse<List<UserListDto>>();
            serviceResponse.Data = await _dataContext.Users.Where(x=>x.LoginId.Contains(username)).Select(x => _mapper.Map<UserListDto>(x)).ToListAsync();
            return serviceResponse;
        }
    }
}
