using Domain;
using System;
using System.Collections.Generic;
using NUnit.Framework;
using Moq;
using DAL;
using DAL.RepoInterfaces;

namespace TestRepo
{
    public class UserRepoTest
    {
        List<UserBase> MockListUsers;
        Mock<IUserRepository> MockUserRepository;
        IUserRepository UserRepository;
        UserBase modifyUser;

        [SetUp]
        public void Setup()
        {
            MockListUsers = new List<UserBase>();
            MockUserRepository = new Mock<IUserRepository>();
            modifyUser = new UserBase
            {
                FirstName = "Prins",
                LastName = "Alvino",
                EmailAddress = "prinsalvino@gmail.com",
                Password = "123456",
                UserId = 1,
                UserRole = UserRoles.Student
            };
            MockListUsers.Add(modifyUser);

            UserBase user1 = new UserBase()
            {
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = "johndoe@gmail.com",
                Password = "123456",
                UserId = 2,
                UserRole = UserRoles.Coach
            };
            MockListUsers.Add(user1);

            // Setting up get all
            MockUserRepository.Setup(ur => ur.GetAll()).Returns(MockListUsers);

            // Setting up get a user
            MockUserRepository.Setup(ur => ur.GetSingle(It.IsAny<long>())).Returns((long id) => MockListUsers.Find(x=> x.UserId == id));

            // Setting up ADD method
            MockUserRepository.Setup(ur=> ur.Add(It.IsAny<UserBase>())).Callback(new Action<UserBase>(x =>
            {
                MockListUsers.Add(x);
            }));

            // Setting up DELETE method
            MockUserRepository.Setup(ur => ur.Delete(It.IsAny<UserBase>())).Callback(new Action<UserBase>(u =>
            {
                MockListUsers.RemoveAll(d => d.UserId == u.UserId);
            }));

            // Setting up UPDATE method
            MockUserRepository.Setup(ur => ur.Update(It.IsAny<UserBase>())).Callback(new Action<UserBase>(u =>
            {
                UserBase found = MockListUsers.Find(c => c.UserId == u.UserId);
                found.EmailAddress = u.EmailAddress;
            }));
            UserRepository = MockUserRepository.Object;
        }
        [Test]
        public void GetAll_Should_Return_All_Users()
        {
            //Arrange 

            //Act
            var users = (IList<UserBase>)UserRepository.GetAll();

            //Assert
            Assert.AreEqual(2, users.Count);
            Assert.AreSame(users[0], MockListUsers[0]);
        }

        [Test]
        public void GetById_Should_Return_Correct_User()
        {
            // Arrange

            //Act
            UserBase user = UserRepository.GetSingle(1);

            //Assert
            Assert.AreEqual(1, user.UserId);
        }

        [Test]
        public void Insert_Should_Return_Increased_UserList()
        {
            // Arrange
            UserBase user = new UserBase
            {
                UserId = 3,
                FirstName = "Test3"
            };

            //Act
            UserRepository.Add(user);
            var after = (IList<UserBase>)UserRepository.GetAll();

            //Assert
            Assert.AreEqual(3, after.Count);
            Assert.AreEqual(after[2], user);
        }

        [Test]
        public void Delete_Should_Return_Decreased_UserList() {

            // Act
            UserRepository.Delete(modifyUser);
            var after = (IList<UserBase>) UserRepository.GetAll();

            // Assert
            Assert.AreEqual(1, after.Count);
        }

        [Test]
        public void Update_Should_Change_User()
        {
            string email = "modify@gmail.com";
            modifyUser.EmailAddress = email;
            
            UserRepository.Update(modifyUser);
            UserBase foundUser = UserRepository.GetSingle(modifyUser.UserId);

            Assert.AreEqual(email,foundUser.EmailAddress);
        }

    }
}
