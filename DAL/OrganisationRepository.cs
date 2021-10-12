using Domain;

namespace ASIST.Repository
{
    public class OrganisationRepository:Repository<Organisation>, IOrganisationRepository
    {
        public OrganisationRepository(SportingContext context) : base(context)
        {
            
        }
    }
}