using System.Collections.Generic;
using Domain;

namespace ServiceLayer.ServiceInterfaces
{
    public interface IOrganisationService
    {
        IEnumerable<Organisation> GetAll();
        Organisation GetOrganisationById(long id);
        void AddOrganisation(Organisation organisation);
        IEnumerable<Organisation> GetOrganisationsByCoachId(long coachId);
        void AssignStudentToOrganisation(long studentId, long organisationId);
        void AssignCoachToOrganisation(long coachId, long organisationId);
        void UnAssignCoachFromOrganisation(long coachId, long organisationId);
        bool CheckIfOrganisationExists(long organisationId);
        bool CheckIfOrganisationListIsEmpty(IEnumerable<Organisation> organisations);
    }
}