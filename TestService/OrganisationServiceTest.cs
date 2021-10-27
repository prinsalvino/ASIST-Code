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
    public class OrganisationServiceTest
    {
        Mock<IOrganisationRepository> MockOrgRepository;
        OrganisationService OrganisationService;
        List<Organisation> MockListOrganisation;
        Organisation modifyOrg;

        [SetUp]
        public void Setup()
        {
            MockListOrganisation = new List<Organisation>();
            MockOrgRepository = new Mock<IOrganisationRepository>();
            OrganisationService = new OrganisationService(MockOrgRepository.Object);

            modifyOrg = new Organisation()
            {
                OrganisationId = 1,
                OrganisationName = "HvA",
                Coaches = new List<Coach>()
                {
                    new Coach(){ UserId = 1}
                }
            };
            MockListOrganisation.Add(modifyOrg);

            Organisation organisation = new Organisation()
            {
                OrganisationId = 2,
                OrganisationName = "UvA",
                Coaches = new List<Coach>()
                {
                    new Coach(){ UserId = 1}
                }
            };
            MockListOrganisation.Add(organisation);

            MockOrgRepository.Setup(ur => ur.GetAll()).Returns(MockListOrganisation);
            MockOrgRepository.Setup(ur => ur.GetSingle(It.IsAny<long>())).Returns((long id) => MockListOrganisation.Find(x => x.OrganisationId == id));

        }

        [Test]
        public void Calling_GetAll_ON_ServiceLayer_Should_Call_SkillRepo_and_Return_all_MockListSkills()
        {
            //Arrange

            //act
            IEnumerable<Organisation> result = OrganisationService.GetAll();


            //Assert
            Assert.AreEqual(result.Count(), 2);
            Assert.That(result, Is.InstanceOf(typeof(IEnumerable<Organisation>)));

            //Check that the GetAll method was called once
            MockOrgRepository.Verify(c => c.GetAll(), Times.Once);
        }

        [Test]
        public void Calling_GetAllById_ON_ServiceLayer_Should_Call_SkillRepo_and_Return_all_MockListSkills()
        {
            //Arrange

            //act
            Organisation result = OrganisationService.GetOrganisationById(1);


            //Assert
            Assert.AreEqual(result.OrganisationId, 1);
            Assert.That(result, Is.InstanceOf(typeof(Organisation)));

            //Check that the GetAll method was called once
            MockOrgRepository.Verify(c => c.GetSingle(1), Times.Once);
        }

        [Test]
        public void Calling_AddOrganisation_ON_ServiceLayer_Should_Call_SkillRepo_and_Return_all_MockListSkills()
        {
            //Arrange
            // Setting up ADD method
            MockOrgRepository.Setup(ur => ur.Add(It.IsAny<Organisation>())).Callback(new Action<Organisation>(x =>
            {
                MockListOrganisation.Add(x);
            }));
            //act
            Organisation newOrg = new Organisation { OrganisationId = 3, OrganisationName = "PvA"};
            OrganisationService.AddOrganisation(newOrg);

            Assert.AreSame(newOrg, MockListOrganisation[2]);
            Assert.AreEqual(3, MockListOrganisation.Count);

            MockOrgRepository.Verify(c => c.Add(newOrg), Times.Once);
        }


        [Test]
        public void Calling_GetAllByCoachId_ON_ServiceLayer_Should_Call_SkillRepo_and_Return_all_MockListSkills()
        {
            //Arrange
            MockOrgRepository.Setup(ur => ur.GetOrganisationsByCoachId(It.IsAny<long>())).Returns((long id) => MockListOrganisation.
                      FindAll(x => x.Coaches.Any(c => c.UserId == id)));
            //act
            IEnumerable<Organisation> result = OrganisationService.GetOrganisationsByCoachId(1);


            //Assert
            Assert.AreEqual(result.Count(), 2);
            Assert.That(result, Is.InstanceOf(typeof(IEnumerable<Organisation>)));

            //Check that the GetAll method was called once
            MockOrgRepository.Verify(c => c.GetOrganisationsByCoachId(1), Times.Once);
        }


        [Test]
        public void Calling_Check_IF_Org_Exist()
        {
           
            var result = OrganisationService.CheckIfOrganisationExists(1);


            //Assert
            Assert.IsTrue(result);

            //Check that the GetAll method was called once
            MockOrgRepository.Verify(c => c.GetAll(), Times.Once);
        }


        [Test]
        public void Calling_Check_IF_Org_NotEmpty()
        {
          
            var result = OrganisationService.CheckIfOrganisationListIsEmpty(MockListOrganisation);


            //Assert
            Assert.IsTrue(result);
        }

    }
}
