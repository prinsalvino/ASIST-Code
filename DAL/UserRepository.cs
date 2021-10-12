using System.Linq;
using Domain;

namespace ASIST.Repository
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
    }
}