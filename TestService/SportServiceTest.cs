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
    public class SportServiceTest
    {
        Mock<ISportRepository> MockSportRepository;
        SportService sportService;
        List<Sport> MockListSports;
        Sport modifySport;


        [SetUp]
        public void Setup()
        {
            MockSportRepository = new Mock<ISportRepository>();
            sportService = new SportService(MockSportRepository.Object);
            MockListSports = new List<Sport>();

            modifySport = new Sport()
            {
                SportId = 1, Name = "VoetBall"
            };
            MockListSports.Add(modifySport);

            Sport test1 = new Sport()
            {
                SportId = 2,
               Name = "BasketBall"
            };
            MockListSports.Add(test1);

            MockSportRepository.Setup(ur => ur.GetAll()).Returns(MockListSports);

            // Setting up get a Sport
            MockSportRepository.Setup(ur => ur.GetSingle(It.IsAny<long>())).Returns((long id) => MockListSports.Find(x => x.SportId == id));

            // Setting up ADD method
            MockSportRepository.Setup(ur => ur.Add(It.IsAny<Sport>())).Callback(new Action<Sport>(x =>
            {
                MockListSports.Add(x);
            }));

            // Setting up DELETE method
            MockSportRepository.Setup(ur => ur.Delete(It.IsAny<Sport>())).Callback(new Action<Sport>(u =>
            {
                MockListSports.RemoveAll(d => d.SportId == u.SportId);
            }));
        }


        [Test]
        public void Calling_GetAll_ON_ServiceLayer_Should_Call_SportStudentRepo_and_Return_all_MockListSportStudent()
        {
            //Arrange

            //act
            IEnumerable<Sport> result = sportService.GetAllSports();


            //Assert
            Assert.AreEqual(result.Count(), 2);
            Assert.That(result, Is.InstanceOf(typeof(IEnumerable<Sport>)));

            //Check that the GetAll method was called once
            MockSportRepository.Verify(c => c.GetAll(), Times.Once);
        }
        [Test]
        public void Calling_GetById_ON_ServiceLayer_Should_Call_SportStudentRepo_and_Return_all_MockListSportStudent()
        {
            //Arrange

            //act
            Sport result = sportService.GetSportById(1);


            //Assert
            Assert.AreEqual(result.SportId, 1);
            Assert.That(result, Is.InstanceOf(typeof(Sport)));

            //Check that the GetAll method was called once
            MockSportRepository.Verify(c => c.GetSingle(1), Times.Once);
        }
    }
}

