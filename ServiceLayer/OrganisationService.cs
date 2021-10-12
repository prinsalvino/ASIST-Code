using System.Collections.Generic;
using ASIST.Repository;
using Domain;

namespace ServiceLayer
{
    public class OrganisationService:IOrganisationService
    {
        private readonly IOrganisationRepository _repository;

        public OrganisationService(IOrganisationRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Organisation> GetAll()
        {
            return _repository.GetAll();
        }

        public Organisation GetOrganisationById(long id)
        {
            return _repository.GetSingle(id);
        }

        public void AddOrganisation(Organisation organisation)
        {
            _repository.Add(organisation);
        }
    }
}