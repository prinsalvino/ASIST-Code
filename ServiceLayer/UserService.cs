using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ASIST.Helpers;
using ASIST.Repository;
using Domain;
using Microsoft.IdentityModel.Tokens;

namespace ServiceLayer
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _repository;
        private readonly AppSettings _appSettings;
        
        public UserService(IUserRepository userRepository, AppSettings appSettings)
        {
            _repository = userRepository;
            _appSettings = appSettings;
        }

        public IEnumerable<UserBase> GetAll(UserRoles role)
        {
            return _repository.GetAll().Where(user => user.UserRole == role);
        }

        public UserBase GetUser(long id)
        {
            return _repository.GetSingle(id);
        }
        public void AddUser(UserBase userBase)
        {
            userBase.Password = BCrypt.Net.BCrypt.HashPassword(userBase.Password); 
            _repository.Add(userBase);
        }

        public void UpdateUser(UserBase userBase)
        {
            userBase.Password = BCrypt.Net.BCrypt.HashPassword(userBase.Password); 
            _repository.Update(userBase);
        }
        public void DeleteUser(UserBase userBase)
        {
            _repository.Delete(userBase);
        }

        public JWTResponse Login(UserLogin userLogin)
        {
           var user =  _repository.Authenticate(userLogin);

           if (user == null || !BCrypt.Net.BCrypt.Verify(userLogin.Password, user.Password))
           {
               // authentication failed
               return null;
           }
           else
           {
               // authentication successful
               var token = generateJwtToken(user);
               return new JWTResponse(user, token);
           }

           
        }
        private string generateJwtToken(UserBase user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("hltoleffebefbegbegbegbeaeafbgnarnrtnsrnnrno");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.UserId.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        
    }
}