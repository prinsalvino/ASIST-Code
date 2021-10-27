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
    public class SportStudentServiceTest
    {
        Mock<ISportStudentRepository> MockSportRepository;
        SportStudentService sportService;
        List<SportStudent> MockListSport;
        SportStudent modifySport;


        [SetUp]
        public void Setup()
        {
            MockSportRepository = new Mock<ISportStudentRepository>();
            sportService = new SportStudentService(MockSportRepository.Object);
            MockListSport = new List<SportStudent>();

            modifySport = new SportStudent()
            {
                SportId = 1,
                DateOfSportAdvices = new DateTime(),
                Score = 9,
                SportStudentId = 1,
                StudentId = 1
            };
            MockListSport.Add(modifySport);

            SportStudent test1 = new SportStudent()
            {
                SportId = 3,
                DateOfSportAdvices = new DateTime(),
                Score = 3,
                SportStudentId = 2,
                StudentId = 2
            };
            MockListSport.Add(test1);

            MockSportRepository.Setup(ur => ur.GetAll()).Returns(MockListSport);

            // Setting up get a Sport
            MockSportRepository.Setup(ur => ur.GetAllSportAdvicesByStudentId(It.IsAny<long>())).Returns((long id) => MockListSport.FindAll(x => x.StudentId == id));

            // Setting up ADD method
            MockSportRepository.Setup(ur => ur.Add(It.IsAny<SportStudent>())).Callback(new Action<SportStudent>(x =>
            {
                MockListSport.Add(x);
            }));

            // Setting up DELETE method
            MockSportRepository.Setup(ur => ur.Delete(It.IsAny<SportStudent>())).Callback(new Action<SportStudent>(u =>
            {
                MockListSport.RemoveAll(d => d.SportId == u.SportId);
            }));
        }

        [Test]
        public void Calling_GetAllById_ON_ServiceLayer_Should_Call_SportStudentRepo_and_Return_all_MockListSportStudent()
        {
            //Arrange

            //act
            IEnumerable<SportStudent> result = sportService.GetAllSportAdvicesByStudentId(1);


            //Assert
            Assert.AreEqual(result.Count(), 1);
            Assert.That(result, Is.InstanceOf(typeof(IEnumerable<SportStudent>)));

            //Check that the GetAll method was called once
            MockSportRepository.Verify(c => c.GetAllSportAdvicesByStudentId(1), Times.Once);
        }

        [Test]
        public void Calling_GetAllByStudentId_AND_SportID_ON_ServiceLayer_Should_Call_SportStudentRepo_and_Return_all_MockListSportStudent()
        {
            //Arrange
            MockSportRepository.Setup(ur => ur.GetSportAdviceByStudentIdAndSportId(It.IsAny<long>(), It.IsAny<long>())).Returns((long id, long sportId) => MockListSport.Find((x) => (x.StudentId == id) && (x.SportId == sportId)));

            //act
            SportStudent result = sportService.GetSportAdviceByStudentIdAndSportId(1,1);


            //Assert
            Assert.AreEqual(result.SportId, 1);
            Assert.AreEqual(result.StudentId, 1);

            Assert.That(result, Is.InstanceOf(typeof(SportStudent)));

            //Check that the GetAll method was called once
            MockSportRepository.Verify(c => c.GetSportAdviceByStudentIdAndSportId(1,1), Times.Once);
        }

        [Test]
        public void Calling_DeleteAllByStudentId_AND_SportID_ON_ServiceLayer_Should_Call_SportStudentRepo_and_Return_all_MockListSportStudent()
        {
            //Arrange
            // Setting up DELETE method
            MockSportRepository.Setup(ur => ur.RemoveAllSportAdvicesByStudentId(It.IsAny<long>())).Callback(new Action<long>(id =>
            {
                MockListSport.RemoveAll(d => d.SportId == id);
            }));
            //act
            sportService.RemoveSportAdvicesByStudentId(1);

            foreach (var item in MockListSport)
            {
                Assert.AreNotEqual(item.StudentId, 1);
            }

            //Check that the GetAll method was called once
            MockSportRepository.Verify(c => c.RemoveAllSportAdvicesByStudentId(1), Times.Once);
        }
    }
}
