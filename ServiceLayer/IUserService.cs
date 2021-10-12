using System;
using System.Collections.Generic;
using Domain;


namespace ServiceLayer
{
    public interface IUserService
    {
        IEnumerable<UserBase> GetAll(UserRoles role);
        UserBase GetUser(long id);
        void UpdateUser(UserBase userBase);
        void AddUser(UserBase userBase);
        void DeleteUser(UserBase userBase);
        JWTResponse Login(UserLogin userLogin);

    }
}