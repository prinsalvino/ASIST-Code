using System;
using System.Collections.Generic;
using Domain;


namespace ServiceLayer.ServiceInterfaces
{
    public interface IUserService
    {
        IEnumerable<UserBase> GetAllByRole(UserRoles role);
        UserBase GetUser(long id);
        void UpdateUser(UserBase userBase);
        void AddUser(UserBase userBase);
        void DeleteUser(UserBase userBase);
        IEnumerable<UserBase> GetCoachesByOrganisationId(long organisationId);
        IEnumerable<UserBase> GetStudentsByOrganisationId(long organisationId);
        IEnumerable<UserBase> GetStudentsOfCoachOrganisation(IEnumerable<Organisation> organisations);
        bool CheckIfUserIsStudent(long userId);
        bool CheckIfUserIsCoach(long userId);
        bool CheckIfUserIsAdmin(long userId);
        bool CheckIfUsersListIsEmpty(IEnumerable<UserBase> users);

    }
}