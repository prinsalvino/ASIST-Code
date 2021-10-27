using DAL;
using Domain;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.RepoInterfaces;

namespace TestRepo
{
    public class SkillStudentRepoTest
    {
        List<SkillStudent> MockListSkills;
        Mock<ISkillStudentRepository> MockSkillRepository;
        ISkillStudentRepository SkillRepository;
        SkillStudent modifySkill;

        [SetUp]
        public void Setup()
        {
            MockListSkills = new List<SkillStudent>();
            MockSkillRepository = new Mock<ISkillStudentRepository>();
            modifySkill = new SkillStudent()
            {
                SkillId = 1, CoachId = 1, DateOfSkill = DateTime.Now,Score = 8, StudentId = 1, SkillStudentId = 1, TimeOfCompletion = 3
            };

            SkillStudent skill1 = new SkillStudent()
            {
                SkillId = 2,
                CoachId = 2,
                DateOfSkill = DateTime.Now,
                Score = 7,
                StudentId = 1,
                SkillStudentId = 2,
                TimeOfCompletion = 4
            };
            MockListSkills.Add(modifySkill);
            MockListSkills.Add(skill1);

            // Setting up get all
            MockSkillRepository.Setup(ur => ur.GetAll()).Returns(MockListSkills);

            // Setting up get a user
            MockSkillRepository.Setup(ur => ur.GetSingle(It.IsAny<long>())).Returns((long id) => MockListSkills.Find(x => x.SkillStudentId == id));

            // Setting up ADD method
            MockSkillRepository.Setup(ur => ur.Add(It.IsAny<SkillStudent>())).Callback(new Action<SkillStudent>(x =>
            {
                MockListSkills.Add(x);
            }));

            // Setting up DELETE method
            MockSkillRepository.Setup(ur => ur.Delete(It.IsAny<SkillStudent>())).Callback(new Action<SkillStudent>(u =>
            {
                MockListSkills.RemoveAll(d => d.SkillStudentId == u.SkillStudentId);
            }));

            // Setting up UPDATE method
            MockSkillRepository.Setup(ur => ur.Update(It.IsAny<SkillStudent>())).Callback(new Action<SkillStudent>(u =>
            {
                SkillStudent found = MockListSkills.Find(c => c.SkillStudentId == u.SkillStudentId);
                found.Score = u.Score;
            }));
            SkillRepository = MockSkillRepository.Object;
        }

        [Test]
        public void GetAll_Should_Return_All_SkillUsers()
        {
            //Arrange 

            //Act
            var skills = (IList<SkillStudent>)SkillRepository.GetAll();

            //Assert
            Assert.AreEqual(2, skills.Count);
            Assert.AreSame(skills[0], MockListSkills[0]);
        }

        [Test]
        public void GetById_Should_Return_Correct_Skill()
        {
            // Arrange

            //Act
            SkillStudent skill = SkillRepository.GetSingle(1);

            //Assert
            Assert.AreEqual(1, skill.SkillStudentId);
        }

        [Test]
        public void Insert_Should_Return_Increased_SkillList()
        {
            // Arrange
            SkillStudent skill = new SkillStudent
            {
                SkillId = 3,
                CoachId = 3,
                DateOfSkill = DateTime.Now,
                Score = 4,
                StudentId = 3,
                SkillStudentId = 3,
                TimeOfCompletion = 7

            };

            //Act
            SkillRepository.Add(skill);
            var after = (IList<SkillStudent>)SkillRepository.GetAll();

            //Assert
            Assert.AreEqual(3, after.Count);
            Assert.AreEqual(after[2], skill);
        }
        [Test]
        public void Delete_Should_Return_Decreased_SkillList()
        {

            // Act
            SkillRepository.Delete(modifySkill);
            var after = (IList<SkillStudent>)SkillRepository.GetAll();

            // Assert
            Assert.AreEqual(1, after.Count);
        }
    }
}
