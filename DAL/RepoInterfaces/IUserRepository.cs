using System.Collections.Generic;
using Domain;

namespace DAL.RepoInterfaces
{
    public interface IUserRepository:IRepository<UserBase>
    {
        UserBase Authenticate(UserLogin userLogin);
        IEnumerable<UserBase> GetCoachesByOrganisationId(long organisationId);
        IEnumerable<UserBase> GetStudentsByOrganisationId(long organisationId);
        
        long SearchUserByEmailAddress(string emailAddress);

    }
}