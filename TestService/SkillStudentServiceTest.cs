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
    public class SkillStudentServiceTest
    {
        Mock<ISkillStudentRepository> MockSkillRepository;
        SkillStudentService SkillService;
        List<SkillStudent> MockListSkills;
        SkillStudent modifySkill;

        [SetUp]
        public void Setup()
        {
            MockSkillRepository = new Mock<ISkillStudentRepository>();
            SkillService = new SkillStudentService(MockSkillRepository.Object);
            MockListSkills = new List<SkillStudent>();

            modifySkill = new SkillStudent()
            {
                SkillId = 1,
                CoachId = 1,
                DateOfSkill = DateTime.Now,
                Score = 8,
                StudentId = 1,
                SkillStudentId = 1,
                TimeOfCompletion = 3
            };
            MockListSkills.Add(modifySkill);

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
            MockListSkills.Add(skill1);


        }

        [Test]
        public void Calling_GetAll_ON_ServiceLayer_Should_Call_SkillRepo_and_Return_all_MockListSkills()
        {
            //Arrange
            MockSkillRepository.Setup(m => m.GetAll()).Returns(MockListSkills);

            //act
            IEnumerable<SkillStudent> result = SkillService.GetAll(1);


            //Assert
            Assert.AreEqual(2, result.Count());
            Assert.That(result, Is.InstanceOf(typeof(IEnumerable<SkillStudent>)));

            //Check that the GetAll method was called once
            MockSkillRepository.Verify(c => c.GetAll(), Times.Once);
        }


        [Test]
        public void Calling_AddSkill_ON_ServiceLayer_Should_Call_SkillRepo_and_Adding_a_SkillStudent()
        {

            // Setting up ADD method
            MockSkillRepository.Setup(ur => ur.AddMany(It.IsAny<List<SkillStudent>>())).Callback(new Action<List<SkillStudent>>(x =>
            {
                foreach (var item in x)
                {
                    MockListSkills.Add(item);
                }
            }));
            List<SkillStudent> skillStudents = new List<SkillStudent>();
            SkillStudent newSkill = new SkillStudent
            {
                SkillId = 3,
                CoachId = 3,
                DateOfSkill = DateTime.Now,
                Score = 4,
                StudentId = 3,
                SkillStudentId = 3,
                TimeOfCompletion = 7

            };
            skillStudents.Add(newSkill);
            skillStudents.Add(newSkill);
            skillStudents.Add(newSkill);
            skillStudents.Add(newSkill);
            skillStudents.Add(newSkill);

            SkillService.AddSkillLapTimes(skillStudents, Gender.Male, 8, 9, 9, 7, 7, 5);

            Assert.AreEqual(7, MockListSkills.Count);
            Assert.AreSame(newSkill, MockListSkills[2]);

            MockSkillRepository.Verify(c => c.AddMany(skillStudents), Times.Once);
        }



        [TearDown]
        public void TestCleanUp()
        {
            MockSkillRepository = null;
            MockListSkills = null;
        }
    }
}
