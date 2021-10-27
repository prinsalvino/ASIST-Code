using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using DAL.RepoInterfaces;
using Domain;

namespace DAL
{
    public class UserRepository : Repository<UserBase>, IUserRepository
    {
        public UserRepository(SportingContext context) :base(context)
        {
            
        }

        public UserBase Authenticate(UserLogin userLogin)
        {
            var user = DbContext.Set<UserBase>().SingleOrDefault(x =>
                x.EmailAddress == userLogin.EmailAddress);

            return user;
        }
        public IEnumerable<UserBase> GetCoachesByOrganisationId(long organisationId)
        {
            var users = DbContext.Set<Organisation>().Where(u => u.OrganisationId == organisationId).SelectMany(u => u.Coaches);
            return users;
        }

        public IEnumerable<UserBase> GetStudentsByOrganisationId(long organisationId)
        {
            var users = DbContext.Set<Organisation>().Where(u => u.OrganisationId == organisationId).SelectMany(u => u.Students);
            return users;
        }

        public long SearchUserByEmailAddress(string emailAddress)
        {
            var userId = DbContext.Set<UserBase>().Where(m => m.EmailAddress == emailAddress).Select(m => m.UserId).SingleOrDefault();
            return userId;
        }
    }
}