using DAL.RepoInterfaces;
using Domain;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRepo
{
    public class TestAttemptRepoTest
    {
        List<TestAttempt> MockListTest;
        Mock<ITestAttemptRepository> MockTestRepository;
        ITestAttemptRepository TestRepository;
        TestAttempt modifyTest;

        [SetUp]
        public void Setup()
        {
            MockListTest = new List<TestAttempt>();
            MockTestRepository = new Mock<ITestAttemptRepository>();
            modifyTest = new TestAttempt()
            {
                TestAttemptId = 1,
                StudentId = 1,
                DateOfTest = DateTime.Now,
                FinalScore = 8
            };
            MockListTest.Add(modifyTest);

            TestAttempt test = new TestAttempt()
            {
                TestAttemptId = 2,
                StudentId = 1,
                DateOfTest = DateTime.Now,
                FinalScore = 9

            };
            MockListTest.Add(test);

            MockTestRepository.Setup(ur => ur.GetAllTestsAttemptedByStudentId(It.IsAny<long>())).Returns((long id) => MockListTest.FindAll(x => x.StudentId == id));
            MockTestRepository.Setup(ur => ur.RemoveAllTestAttemptsByStudentId(It.IsAny<long>())).Callback(new Action<long>(id =>
            {
                MockListTest.RemoveAll(x => x.StudentId == id);
            }));

            TestRepository = MockTestRepository.Object;
        }

        [Test]
        public void GetById_Should_Return_Correct_TestAttempt()
        {
            // Arrange

            //Act
            var sports = TestRepository.GetAllTestsAttemptedByStudentId(1);

            //Assert
            Assert.AreEqual(2, sports.Count());
            foreach (var item in sports)
            {
                Assert.AreEqual(1, item.StudentId);
            }
        }


        [Test]
        public void DeleteAllById_Should_Return_Zero_TestAttempt()
        {
            // Arrange

            //Act
            TestRepository.RemoveAllTestAttemptsByStudentId(1);
            var sports = TestRepository.GetAllTestsAttemptedByStudentId(1);

            //Assert
            Assert.AreEqual(0, sports.Count());
            
        }
    }
}
