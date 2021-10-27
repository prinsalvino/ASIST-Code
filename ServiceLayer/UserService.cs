using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DAL;
using DAL.RepoInterfaces;
using Domain;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.IdentityModel.Tokens;
using ServiceLayer.SecurityConfig;
using ServiceLayer.ServiceInterfaces;

namespace ServiceLayer
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository userRepository)
        {
            _repository = userRepository;
        }

        public IEnumerable<UserBase> GetAllByRole(UserRoles role)
        {
            return _repository.GetAll().Where(user => user.UserRole == role);
        }

        public UserBase GetUser(long id)
        {
            try
            {
                var user = _repository.GetSingle(id);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
        public void AddUser(UserBase userBase)
        {
            var userId = _repository.SearchUserByEmailAddress(userBase.EmailAddress);

            if (userId.IsNullOrDefault())
            {
                userBase.Password = BCrypt.Net.BCrypt.HashPassword(userBase.Password); 
                _repository.Add(userBase);
            }
            else
            {
                var user = GetUser(userId);

                if (user.EmailAddress != userBase.EmailAddress)
                {
                    userBase.Password = BCrypt.Net.BCrypt.HashPassword(userBase.Password); 
                    _repository.Add(userBase);
                }
                else
                {
                    throw new Exception("user already exists!");
                }
            }
        }

        public void UpdateUser(UserBase userBase)
        {
            try
            {
                _repository.Update(userBase);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public void DeleteUser(UserBase userBase)
        {
            try
            {
                _repository.Delete(userBase);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public IEnumerable<UserBase> GetCoachesByOrganisationId(long organisationId)
        {
            try
            {
                var users = _repository.GetCoachesByOrganisationId(organisationId).ToList();
                CheckIfUsersListIsEmpty(users);
                return users;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public IEnumerable<UserBase> GetStudentsByOrganisationId(long organisationId)
        {
            try
            {
                var users = _repository.GetStudentsByOrganisationId(organisationId).ToList();
                CheckIfUsersListIsEmpty(users);
                return users;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public IEnumerable<UserBase> GetStudentsOfCoachOrganisation(IEnumerable<Organisation> organisations)
        {
            try
            {
                List<UserBase> users = new List<UserBase>();
                foreach (var organisation in organisations)
                {
                    users.AddRange(_repository.GetStudentsByOrganisationId(organisation.OrganisationId));
                }

                CheckIfUsersListIsEmpty(users);
                return users;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public bool CheckIfUserIsStudent(long userId)
        {
            try
            {
                var user = GetUser(userId);

                if (user == null || user.UserRole != UserRoles.Student)
                {
                    throw new Exception("User is not a student or User is not found");
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public bool CheckIfUserIsCoach(long userId)
        {
            try
            {
                var user = GetUser(userId);

                if (user == null || user.UserRole != UserRoles.Coach)
                {
                    throw new Exception("User is not a coach or User is not found");
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public bool CheckIfUserIsAdmin(long userId)
        {
            try
            {
                var user = GetUser(userId);

                if (user == null || user.UserRole != UserRoles.Admin)
                {
                    throw new Exception("User is not an admin or User is not found");
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public bool CheckIfUsersListIsEmpty(IEnumerable<UserBase> users)
        {
            try
            {
                if (users == null || !users.Any())
                {
                    throw new Exception("No users found");
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}