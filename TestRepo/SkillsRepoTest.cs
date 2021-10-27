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
    public class SkillsRepoTest
    {

        List<Skill> MockListSkills;
        Mock<ISkillRepository> MockSkillRepository;
        ISkillRepository SkillRepository;
        Skill modifySkill;


        [SetUp]
        public void Setup()
        {
            MockListSkills = new List<Skill>();
            MockSkillRepository = new Mock<ISkillRepository>();
            modifySkill = new Skill()
            {
                SkillId = 1, Name = "Skill"
            };
            MockListSkills.Add(modifySkill);

            Skill skill = new Skill()
            {
                SkillId = 2,
                Name = "Skill1"
            };
            MockListSkills.Add(skill);
            // Setting up get all
            MockSkillRepository.Setup(ur => ur.GetAll()).Returns(MockListSkills);

            // Setting up get a skill
            MockSkillRepository.Setup(ur => ur.GetSingle(It.IsAny<long>())).Returns((long id) => MockListSkills.Find(x => x.SkillId == id));


            SkillRepository = MockSkillRepository.Object;
        }


        [Test]
        public void GetAll_Should_Return_All_SkillUsers()
        {
            //Arrange 

            //Act
            var skills = (IList<Skill>)SkillRepository.GetAll();

            //Assert
            Assert.AreEqual(2, skills.Count);
            Assert.AreSame(skills[0], MockListSkills[0]);
        }

        [Test]
        public void GetById_Should_Return_Correct_Skill()
        {
            // Arrange

            //Act
            Skill skill = SkillRepository.GetSingle(1);

            //Assert
            Assert.AreEqual(1, skill.SkillId);
        }
    }
}
