using System.Collections.Generic;
using Domain;

namespace DAL.RepoInterfaces
{
    public interface IOrganisationRepository:IRepository<Organisation>
    {
        IEnumerable<Organisation> GetOrganisationsByCoachId(long coachId);
        void AssignStudentToOrganisation(long studentId, long organisationId);
        void AssignCoachToOrganisation(long coachId, long organisationId);
        void UnAssignCoachFromOrganisation(long coachId, long organisationId);
    }
}