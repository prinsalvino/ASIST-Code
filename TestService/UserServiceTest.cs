using DAL;
using Domain;
using Moq;
using NUnit.Framework;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using DAL.RepoInterfaces;

namespace TestService
{
    public class UserServiceTest
    {
        Mock<IUserRepository> MockUserRepository;
        UserService UserService;
        List<UserBase> MockListUsers;
        UserBase modifyUser;

        [SetUp]
        public void Setup()
        {
            MockUserRepository = new Mock<IUserRepository>();
            UserService = new UserService(MockUserRepository.Object);
            MockListUsers = new List<UserBase>();

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

            // Setting up ADD method
            MockUserRepository.Setup(ur => ur.Add(It.IsAny<UserBase>())).Callback(new Action<UserBase>(x =>
            {
                MockListUsers.Add(x);
            }));

            // Setting up Update method
            MockUserRepository.Setup(ur => ur.Update(It.IsAny<UserBase>())).Callback(new Action<UserBase>(x =>
            {
                UserBase found = MockListUsers.Find(c => c.UserId == x.UserId);
                found.EmailAddress = x.EmailAddress;
            }));

            // Setting up Get a User method
            MockUserRepository.Setup(ur => ur.GetSingle(It.IsAny<long>())).Returns((long id) => MockListUsers.Find(x => x.UserId == id));
        }

        [Test]
        public void Calling_GetAll_ON_ServiceLayer_Should_Call_UserRepo_and_Return_all_MockListUserOfRole()
        {
            //Arrange
            MockUserRepository.Setup(m => m.GetAll()).Returns(MockListUsers);

            //act
            IEnumerable<UserBase> result = UserService.GetAllByRole(UserRoles.Coach);


            //Assert
            Assert.AreEqual(result.Count(), 1);
            Assert.That(result, Is.InstanceOf(typeof(IEnumerable<UserBase>)));

            //Check that the GetAll method was called once
            MockUserRepository.Verify(c => c.GetAll(), Times.Once);
        }

        [Test]
        public void Calling_GetAUser_ON_ServiceLayer_Should_Call_UserRepo_and_Return_a_User()
        {
            //Arrange
            MockUserRepository.Setup(m => m.GetSingle(It.IsAny<long>())).Returns((long id) => MockListUsers.Find(x => x.UserId == id));

            //act
            UserBase result = UserService.GetUser(1);


            //Assert
            Assert.AreEqual(result, modifyUser);
            Assert.That(result, Is.InstanceOf(typeof(UserBase)));

            //Check that the GetAll method was called once
            MockUserRepository.Verify(c => c.GetSingle(1), Times.Once);
        }

        [Test]
        public void Calling_AddUser_ON_ServiceLayer_Should_Call_UserRepo_and_Adding_a_User()
        {


            UserBase newUser = new UserBase { UserId = 3, Password = "123456" };
            UserService.AddUser(newUser);

            Assert.AreSame(newUser, MockListUsers[2]);
            Assert.AreEqual(3, MockListUsers.Count);
            
            MockUserRepository.Verify(c => c.Add(newUser), Times.Once);
        }


        [Test]
        public void Calling_UpdateUser_ON_ServiceLayer_Should_Call_UserRepo_and_Adding_a_User()
        {
         

            string email = "modify@gmail.com";
            modifyUser.EmailAddress = email;

            UserService.UpdateUser(modifyUser);
            UserBase foundUser = UserService.GetUser(modifyUser.UserId);

            Assert.AreEqual(email, foundUser.EmailAddress);

            MockUserRepository.Verify(c => c.Update(modifyUser), Times.Once);
            MockUserRepository.Verify(c => c.GetSingle(modifyUser.UserId), Times.Once);
        }


        [TearDown]
        public void TestCleanUp()
        {
            MockUserRepository = null;
            MockListUsers = null;
        }
    }
}
