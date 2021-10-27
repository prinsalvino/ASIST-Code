using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DAL.RepoInterfaces;
using Domain;

namespace DAL
{
    public class OrganisationRepository:Repository<Organisation>, IOrganisationRepository
    {
        private readonly SportingContext _context;
        public OrganisationRepository(SportingContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Organisation> GetOrganisationsByCoachId(long coachId)
        {
            var organisations = DbContext.Set<Coach>().Where(u => u.UserId == coachId).SelectMany(u => u.Organisations);
            return organisations;
        }
        public void AssignStudentToOrganisation(long studentId, long organisationId)
        {
            var student = DbContext.Set<Student>().Single(x => x.UserId == studentId);
            var organisation = DbContext.Set<Organisation>().Single(o => o.OrganisationId == organisationId);
            student.Organisation = organisation;

            DbContext.SaveChangesAsync();
        }

        public void AssignCoachToOrganisation(long coachId, long organisationId)
        {
            var coach = DbContext.Set<Coach>().Include(x => x.Organisations).Single(x => x.UserId == coachId);
            var organisation = DbContext.Set<Organisation>().Single(x => x.OrganisationId == organisationId);
            coach.Organisations.Add(organisation);
            
            DbContext.SaveChangesAsync();
        }

        public void UnAssignCoachFromOrganisation(long coachId, long organisationId)
        {
            var coaches = _context.Coaches.Find(coachId);
            var organisations = _context.Organisation.Find(organisationId);

            DbContext.Entry(coaches).Collection(o => o.Organisations).Load();

            coaches.Organisations.Remove(organisations);
            
            DbContext.SaveChangesAsync();
        }
    }
}