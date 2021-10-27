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
    public class SkillServiceTest
    {
        Mock<ISkillRepository> MockSkillRepository;
        SkillService SkillService;
        List<Skill> MockListSkills;
        Skill modifySkill;

        [SetUp]
        public void Setup()
        {
            MockSkillRepository = new Mock<ISkillRepository>();
            SkillService = new SkillService(MockSkillRepository.Object);
            MockListSkills = new List<Skill>();

            modifySkill = new Skill()
            {
                SkillId = 1,
                Name = "Skill"
            };

            Skill skill = new Skill()
            {
                SkillId = 2,
                Name = "Skill1"
            };
            MockListSkills.Add(modifySkill);

            MockListSkills.Add(skill);

            // Setting up get all
            MockSkillRepository.Setup(ur => ur.GetAll()).Returns(MockListSkills);

            // Setting up get a user
            MockSkillRepository.Setup(ur => ur.GetSingle(It.IsAny<long>())).Returns((long id) => MockListSkills.Find(x => x.SkillId == id));

        }


        [Test]
        public void Calling_GetAll_ON_ServiceLayer_Should_Call_SportStudentRepo_and_Return_all_MockListSportStudent()
        {
            //Arrange

            //act
            IEnumerable<Skill> result = SkillService.GetAllSkills();


            //Assert
            Assert.AreEqual(2, result.Count());
            Assert.That(result, Is.InstanceOf(typeof(IEnumerable<Skill>)));

            //Check that the GetAll method was called once
            MockSkillRepository.Verify(c => c.GetAll(), Times.Once);
        }
        [Test]
        public void Calling_GetById_ON_ServiceLayer_Should_Call_SportStudentRepo_and_Return_all_MockListSportStudent()
        {
            //Arrange

            //act
            Skill result = SkillService.GetSkillById(1);


            //Assert
            Assert.AreEqual(result.SkillId, 1);
            Assert.That(result, Is.InstanceOf(typeof(Skill)));

            //Check that the GetAll method was called once
            MockSkillRepository.Verify(c => c.GetSingle(1), Times.Once);
        }
    }
}
