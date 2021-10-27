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
    public class OrganisationRepoTest
    {
        List<Organisation> MockListTest;
        List<Coach> MockListCoach;

        Mock<IOrganisationRepository> MockTestRepository;
        IOrganisationRepository OrgRepository;
        Organisation modifyTest;

        [SetUp]
        public void Setup()
        {
            MockListTest = new List<Organisation>();
            MockListCoach = new List<Coach>();
            MockListCoach.Add(new Coach()
            {
                UserId = 1,
                Organisations = new List<Organisation>()
            });
            MockListCoach.Add(new Coach()
            {
                UserId = 2,
                Organisations = new List<Organisation>()
            });
            MockTestRepository = new Mock<IOrganisationRepository>();
            modifyTest = new Organisation()
            {
                OrganisationId = 1,
                OrganisationName = "HvA",
                Coaches = MockListCoach
            };
            MockListTest.Add(modifyTest);

            Organisation organisation = new Organisation()
            {
                OrganisationId = 2,
                OrganisationName = "UvA",
                Coaches = MockListCoach
            };
            MockListTest.Add(organisation);
    
            MockTestRepository.Setup(ur => ur.GetOrganisationsByCoachId(It.IsAny<long>())).Returns((long id) => MockListTest.
            FindAll(x => x.Coaches.Any(c => c.UserId == id)));

            MockTestRepository.Setup(ur => ur.AssignCoachToOrganisation(It.IsAny<long>(), It.IsAny<long>())).Callback(new Action<long,long>((u,y) =>
            {
                Organisation organisation1 = MockListTest.Find(x => x.OrganisationId == y);
                Coach coach = organisation1.Coaches.First(c => c.UserId == u);
                coach.Organisations.Add(organisation1);
            }));

            MockTestRepository.Setup(ur => ur.UnAssignCoachFromOrganisation(It.IsAny<long>(), It.IsAny<long>())).Callback(new Action<long, long>((u, y) =>
            {
                Organisation organisation1 = MockListTest.Find(x => x.OrganisationId == y);
                Coach coach = organisation1.Coaches.First(c => c.UserId == u);
                coach.Organisations.Remove(organisation1);
            }));


            OrgRepository = MockTestRepository.Object;
        }
        [Test]
        public void GetByCoachId_Should_Return_Correct_Org()
        {
            // Arrange

            //Act
            var orgs = OrgRepository.GetOrganisationsByCoachId(1);

            //Assert
            Assert.AreEqual(2, orgs.Count());
        }

        [Test]
        public void AddOrgByCoachId_Should_Increase_list_Org()
        {
            // Arrange
            var before = OrgRepository.GetOrganisationsByCoachId(1);

            //Act
            OrgRepository.AssignCoachToOrganisation(1,1);

            var after = OrgRepository.GetOrganisationsByCoachId(1);

            //Assert
            Assert.AreEqual(2, after.Count());
        }

        [Test]
        public void DeleteOrgByCoachId_Should_Decrease_list_Org()
        {
            // Arrange
            var before = OrgRepository.GetOrganisationsByCoachId(1);

            //Act

            OrgRepository.UnAssignCoachFromOrganisation(1, 1);

            var after = OrgRepository.GetOrganisationsByCoachId(1);

            //Assert
            Assert.AreEqual(2, after.Count());
        }
    }
}
