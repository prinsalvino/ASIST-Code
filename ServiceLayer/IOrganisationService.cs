using System.Collections.Generic;
using Domain;

namespace ServiceLayer
{
    public interface IOrganisationService
    {
        IEnumerable<Organisation> GetAll();
        Organisation GetOrganisationById(long id);
        void AddOrganisation(Organisation organisation);
        
    }
}