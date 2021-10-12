using Domain;

namespace ASIST.Repository
{
    public interface IUserRepository:IRepository<UserBase>
    {
        UserBase Authenticate(UserLogin userLogin);
    }
}