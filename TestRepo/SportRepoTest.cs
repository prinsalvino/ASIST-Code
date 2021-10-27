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
    public class SportRepoTest
    {

        List<Sport> MockListSports;
        Mock<ISportRepository> MockSportRepository;
        ISportRepository SportRepository;
        Sport modifySport;

        [SetUp]
        public void Setup()
        {
            MockListSports = new List<Sport>();
            MockSportRepository = new Mock<ISportRepository>();
            modifySport = new Sport()
            {
                SportId = 1, Name = "BasketBall"
            };
            MockListSports.Add(modifySport);

            Sport sport = new Sport()
            {
                SportId = 2,
                Name = "FootBall"
            };
            MockListSports.Add(sport);
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
            SportRepository = MockSportRepository.Object;
        }


        [Test]
        public void GetAll_Should_Return_All_SkillUsers()
        {
            //Arrange 

            //Act
            var sports = (IList<Sport>)SportRepository.GetAll();

            //Assert
            Assert.AreEqual(2, sports.Count);
            Assert.AreSame(sports[0], MockListSports[0]);
        }

        [Test]
        public void GetById_Should_Return_Correct_Skill()
        {
            // Arrange

            //Act
            Sport sport = SportRepository.GetSingle(1);

            //Assert
            Assert.AreEqual(1, sport.SportId);
        }

        [Test]
        public void Insert_Should_Return_Increased_SkillList()
        {
            // Arrange
            Sport sport = new Sport
            {
               SportId = 3, Name = "VolleyBall"

            };

            //Act
            SportRepository.Add(sport);
            var after = (IList<Sport>)SportRepository.GetAll();

            //Assert
            Assert.AreEqual(3, after.Count);
            Assert.AreEqual(after[2], sport);
        }
        [Test]
        public void Delete_Should_Return_Decreased_SkillList()
        {

            // Act
            SportRepository.Delete(modifySport);
            var after = (IList<Sport>)SportRepository.GetAll();

            // Assert
            Assert.AreEqual(1, after.Count);
        }

    }
}
