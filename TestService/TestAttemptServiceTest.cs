using DAL;
using Domain;
using Moq;
using NUnit.Framework;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.RepoInterfaces;

namespace TestService
{
    public class TestAttemptServiceTest
    {

        Mock<ITestAttemptRepository> MockTestRepository;
        TestAttemptService testService;
        List<TestAttempt> MockListTest;
        TestAttempt modifyTest;
        

        [SetUp]
        public void Setup()
        {
            MockTestRepository = new Mock<ITestAttemptRepository>();
            testService = new TestAttemptService(MockTestRepository.Object);
            MockListTest = new List<TestAttempt>();

            modifyTest = new TestAttempt()
            {
                DateOfTest = DateTime.Now,
                FinalScore = 9,
                StudentId = 1,
                TestAttemptId = 1
            };
            MockListTest.Add(modifyTest);

            TestAttempt test1 = new TestAttempt()
            {
                DateOfTest = DateTime.Now,
                FinalScore = 6,
                StudentId = 1,
                TestAttemptId = 2
            };
            MockListTest.Add(test1);

            MockTestRepository.Setup(ur => ur.GetAllTestsAttemptedByStudentId(It.IsAny<long>())).Returns((long id) => MockListTest.FindAll(x => x.StudentId == id));

            MockTestRepository.Setup(ur => ur.RemoveAllTestAttemptsByStudentId(It.IsAny<long>())).Callback(new Action<long>(id =>
            {
                MockListTest.RemoveAll(x => x.StudentId == id);
            }));

            MockTestRepository.Setup(ur => ur.GetAll()).Returns(MockListTest);
        }

        [Test]
        public void Calling_GetAllById_ON_ServiceLayer_Should_Call_UserRepo_and_Return_all_MockListUserOfRole()
        {
            //Arrange

            //act
            IEnumerable<TestAttempt> result = testService.GetAllTestsAttemptedByStudentId(1);


            //Assert
            Assert.AreEqual(result.Count(), 2);
            Assert.That(result, Is.InstanceOf(typeof(IEnumerable<TestAttempt>)));

            //Check that the GetAll method was called once
            MockTestRepository.Verify(c => c.GetAllTestsAttemptedByStudentId(1), Times.Once);
        }
        [Test]
        public void Calling_RemoveAllById_ON_ServiceLayer_Should_Call_UserRepo_and_Return_all_MockListUserOfRole()
        {
            //Arrange
          
            //act
            testService.RemoveAllTestsAttemptedByStudentId(1);

            //Assert
            foreach (var item in MockListTest)
            {
                Assert.AreNotEqual(item.StudentId, 1);
            }

            //Check that the GetAll method was called once
            MockTestRepository.Verify(c => c.RemoveAllTestAttemptsByStudentId(1), Times.Once);
        }
    }
}
