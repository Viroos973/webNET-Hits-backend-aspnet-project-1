using DeliveryFoodBackend.Data;
using DeliveryFoodBackend.DTO;
using DeliveryFoodBackend.Services.Interfaces;
using DeliveryFoodBackend.Data.Models.Enums;
using DeliveryFoodBackend.Data.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace DeliveryFoodBackend.Service
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TokenResponse> RegisterUser(UserRegisterModel userRegisterModel)
        {
            if (userRegisterModel.BirthDate != null && userRegisterModel.BirthDate > DateTime.UtcNow)
            {
                throw new BadHttpRequestException(message: "Birth date can't be later than today");
            }

            var email = _context.Users.Where(x => x.EmailAddress == userRegisterModel.Email).FirstOrDefault();

            if (email != null)
            {
                throw new BadHttpRequestException(message: $"Username {userRegisterModel.Email} is already taken");
            }

            var address = _context.AsHouses.Where(x => x.Objectguid == userRegisterModel.AddressId).FirstOrDefault();

            if (address == null)
            {
                throw new BadHttpRequestException(message: "Address not found");
            }

            if (!userRegisterModel.Password.Any(char.IsDigit))
            {
                throw new BadHttpRequestException(message: "Password requires at least one digit");
            }

            var passwordHash = await PasswordHashing(userRegisterModel.Password);

            await _context.Users.AddAsync(new User
            {
                Id = Guid.NewGuid(),
                FullName = userRegisterModel.FullName,
                BirthDate = userRegisterModel.BirthDate,
                Address = userRegisterModel.AddressId,
                EmailAddress = userRegisterModel.Email,
                Genders = userRegisterModel.Gender.ToString(),
                Password = passwordHash,
                Phone = userRegisterModel.PhoneNumber
            });
            await _context.SaveChangesAsync();

            var credentials = new LoginCredentials
            {
                Email = userRegisterModel.Email,
                Password = userRegisterModel.Password
            };

            return await Login(credentials);
        }

        public async Task<TokenResponse> Login(LoginCredentials credentials)
        {
            var userEntity = _context.Users.Where(x => x.EmailAddress == credentials.Email).FirstOrDefault();

            if (userEntity == null)
            {
                throw new BadHttpRequestException(message: "User is not found");
            }

            var passwordHash = await PasswordHashing(credentials.Password);

            if (userEntity.Password != passwordHash) 
            {
                throw new BadHttpRequestException(message: "Wrong password");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("jAoRAj9kzVPykFATzV1Ye0LJNmdcuB");

            var TokenDescriptor = new SecurityTokenDescriptor()
            {
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = "DeliveryFoodBackend",
                Audience = "DeliveryFoodFronted",
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userEntity.Id.ToString()),
                })
            };

            return new TokenResponse
            {
                Token = tokenHandler.WriteToken(tokenHandler.CreateToken(TokenDescriptor))
            };
        }

        public async Task LogOut(string token)
        {
            string invalideToken = token;
            DateTime expires = new JwtSecurityTokenHandler().ReadJwtToken(token).ValidTo;

            await _context.Tokens.AddAsync(new Token
            {
                InvalideToken = invalideToken,
                ExpiredDate = expires
            });

            await _context.SaveChangesAsync();
        }

        public async Task<UserDto> GetProfile(Guid id)
        {
            var user = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();

            return new UserDto
            {
                Id = id,
                FullName = user.FullName,
                BirthDate = user.BirthDate,
                Gender = user.Genders,
                Address = user.Address,
                Email = user.EmailAddress,
                PhoneNumber = user.Phone
            };
        }

        public async Task EditProfile(UserEditModel editModel, Guid id)
        {
            if (editModel.BirthDate != null && editModel.BirthDate > DateTime.UtcNow)
            {
                throw new BadHttpRequestException(message: "Birth date can't be later than today");
            }

            var address = _context.AsHouses.Where(x => x.Objectguid == editModel.AddressId).FirstOrDefault();

            if (address == null)
            {
                throw new BadHttpRequestException(message: "Address not found");
            }

            var user = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();

            user.FullName = editModel.FullName;
            user.BirthDate = editModel.BirthDate;
            user.Genders = editModel.Gender.ToString();
            user.Address = editModel.AddressId;
            user.Phone = editModel.PhoneNumber;

            await _context.SaveChangesAsync();
        }

        private async Task<string> PasswordHashing(string password)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }
    }
}
