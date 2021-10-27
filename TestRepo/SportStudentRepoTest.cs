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
    public class SportStudentRepoTest
    {
        List<SportStudent> MockListSports;
        Mock<ISportStudentRepository> MockSportRepository;
        ISportStudentRepository SportRepository;
        SportStudent modifySport;

        [SetUp]
        public void Setup()
        {
            MockListSports = new List<SportStudent>();
            MockSportRepository = new Mock<ISportStudentRepository>();
            modifySport = new SportStudent()
            {
                SportId = 1,
                DateOfSportAdvices = new DateTime(),
                Score = 9,
                SportStudentId = 1,
                StudentId = 1
            };
            MockListSports.Add(modifySport);

            SportStudent sport = new SportStudent()
            {
                SportId = 3,
                DateOfSportAdvices = new DateTime(),
                Score = 3,
                SportStudentId = 2,
                StudentId = 2
            };
            MockListSports.Add(sport);
            MockSportRepository.Setup(ur => ur.GetAll()).Returns(MockListSports);

            // Setting up get a Sport
            MockSportRepository.Setup(ur => ur.GetSingle(It.IsAny<long>())).Returns((long id) => MockListSports.Find(x => x.SportId == id));

            // Setting up ADD method
            MockSportRepository.Setup(ur => ur.Add(It.IsAny<SportStudent>())).Callback(new Action<SportStudent>(x =>
            {
                MockListSports.Add(x);
            }));

            // Setting up DELETE method
            MockSportRepository.Setup(ur => ur.Delete(It.IsAny<SportStudent>())).Callback(new Action<SportStudent>(u =>
            {
                MockListSports.RemoveAll(d => d.SportId == u.SportId);
            }));
            SportRepository = MockSportRepository.Object;
        }

        [Test]
        public void GetAll_Should_Return_All_SportStudent()
        {
            //Arrange 

            //Act
            var sports = (IList<SportStudent>)SportRepository.GetAll();

            //Assert
            Assert.AreEqual(2, sports.Count);
            Assert.AreSame(sports[0], MockListSports[0]);
        }

        [Test]
        public void GetAll_ById_Should_Return_All_SportStudent()
        {
            //Arrange 
            MockSportRepository.Setup(ur => ur.GetAllSportAdvicesByStudentId(It.IsAny<long>())).Returns((long id) => MockListSports.FindAll(x=>x.StudentId == id));

            //Act
            var sports = (IList<SportStudent>)SportRepository.GetAllSportAdvicesByStudentId(1);

            //Assert
            Assert.AreEqual(1, sports.Count);
            Assert.AreSame(sports[0], MockListSports[0]);
        }

        [Test]
        public void GetById_Should_Return_Correct_SportStudent()
        {
            // Arrange

            //Act
            SportStudent sport = SportRepository.GetSingle(1);

            //Assert
            Assert.AreEqual(1, sport.SportId);
        }

        [Test]
        public void GetById_AND_SportId_Should_Return_Correct_SportStudent()
        {
            // Arrange
            MockSportRepository.Setup(ur => ur.GetSportAdviceByStudentIdAndSportId(It.IsAny<long>(), It.IsAny<long>())).Returns((long id, long sportId) => MockListSports.Find((x) => (x.StudentId == id) && (x.SportId == sportId)));

            //Act
            SportStudent sport = SportRepository.GetSportAdviceByStudentIdAndSportId(2,3);

            //Assert
            Assert.AreEqual(2, sport.StudentId);
            Assert.AreEqual(3, sport.SportId);

        }

        [Test]
        public void Insert_Should_Return_Increased_SportStudentList()
        {
            // Arrange
            SportStudent sport = new SportStudent()
            {
                SportId = 3,
                DateOfSportAdvices = new DateTime(),
                Score = 6,
                SportStudentId = 3,
                StudentId = 1
            };

            //Act
            SportRepository.Add(sport);
            var after = (IList<SportStudent>)SportRepository.GetAll();

            //Assert
            Assert.AreEqual(3, after.Count);
            Assert.AreEqual(after[2], sport);
        }
        [Test]
        public void Delete_Should_Return_Decreased_SportStudentList()
        {

            // Act
            SportRepository.Delete(modifySport);
            var after = (IList<SportStudent>)SportRepository.GetAll();

            // Assert
            Assert.AreEqual(1, after.Count);
        }

        [Test]
        public void Delete_All_Should_Return_ZERO_SportStudentList_ById()
        {
            MockSportRepository.Setup(ur => ur.RemoveAllSportAdvicesByStudentId(It.IsAny<long>())).Callback(new Action<long>(id =>
            {
                MockListSports.RemoveAll(d => d.StudentId == id);
            }));
           
            // Act
            SportRepository.RemoveAllSportAdvicesByStudentId(1);
            var after = (IList<SportStudent>)SportRepository.GetAllSportAdvicesByStudentId(1);

            // Assert
            Assert.AreEqual(0, after.Count);
        }
    }
}
