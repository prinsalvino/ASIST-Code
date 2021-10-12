
using ASIST.Repository;
using Domain;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    public class UnitTest
    {
        List<UserBase> MockListUsers;
        UserBase foundUser;
        private int _falseId = 3332;

        [SetUp]
        public void Setup()
        {
            MockListUsers = new List<UserBase>();   
            UserBase user = new UserBase
            {
                FirstName = "Prins",
                LastName = "Alvino",
                EmailAddress = "prinsalvino@gmail.com",
                Password = "123456",
                UserId = 1,
                UserRole = UserRoles.Student
            };
            MockListUsers.Add(user);

            foundUser = new UserBase()
            {
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = "johndoe@gmail.com",
                Password = "123456",
                UserId = 2,
                UserRole = UserRoles.Coach
            };
            MockListUsers.Add(foundUser);
        }

        [Test]
        public void Get_All_Should_Return_User()
        {
            var dalMock = new Mock<IUserRepository>();
            dalMock.Setup(x => x.GetAll()).Returns(MockListUsers);

            var mockedDal = dalMock.Object;
            var list = mockedDal.GetAll();

            Assert.AreEqual(2, MockListUsers.Count);
        }

        [Test]
        public void FindByID_Should_Return_a_User()
        {
            //arrange
            var dalMock = new Mock<IUserRepository>();
            dalMock.Setup(mr => mr.GetSingle(It.IsAny<long>())).Returns((long s) => MockListUsers.Where(x => x.UserId == s).Single());

            //act
            var mockedDal = dalMock.Object;
            UserBase user = mockedDal.GetSingle(1);

            //assert
            Assert.IsNotNull(user);
            Assert.IsInstanceOf(typeof(UserBase), user);
            Assert.AreEqual(1, user.UserId);
        }

        [Test]
        public void FindByID_With_InvalidID_Should_Return_a_NULL()
        {
            //arrange
            var dalMock = new Mock<IUserRepository>();
            dalMock.Setup(mr => mr.GetSingle(It.IsAny<int>())).Returns((int s) => MockListUsers.Find(x => x.UserId == s));

            //act
            var mockedDal = dalMock.Object;
            UserBase user = mockedDal.GetSingle(_falseId);

            //assert
            Assert.IsNull(user);
        }

        [TearDown]
        public void TestCleanUp()
        {
            MockListUsers = null;
            foundUser = null;
        }
    }
}
