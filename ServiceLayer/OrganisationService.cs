using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using DAL.RepoInterfaces;
using Domain;
using ServiceLayer.ServiceInterfaces;

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
            try
            {
                var organisations = _repository.GetAll().ToList();
                CheckIfOrganisationListIsEmpty(organisations);
                return organisations;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Organisation GetOrganisationById(long id)
        {
            try
            {
                var organisation = _repository.GetSingle(id);
                if (organisation == null)
                {
                    throw new Exception("No organisation found");
                }

                return organisation;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void AddOrganisation(Organisation organisation)
        {
            try
            {
                _repository.Add(organisation);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public IEnumerable<Organisation> GetOrganisationsByCoachId(long coachId)
        {
            try
            {
                var organisations = _repository.GetOrganisationsByCoachId(coachId).ToList();
                CheckIfOrganisationListIsEmpty(organisations);
                return organisations;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public void AssignStudentToOrganisation(long studentId, long organisationId)
        {
            try
            {
                CheckIfOrganisationExists(organisationId);
                _repository.AssignStudentToOrganisation(studentId, organisationId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void AssignCoachToOrganisation(long coachId, long organisationId)
        {
            try
            {
                CheckIfOrganisationExists(organisationId);
                _repository.AssignCoachToOrganisation(coachId,organisationId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void UnAssignCoachFromOrganisation(long coachId, long organisationId)
        {
            try
            {
                CheckIfOrganisationExists(organisationId);
                _repository.UnAssignCoachFromOrganisation(coachId, organisationId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public bool CheckIfOrganisationExists(long organisationId)
        {
            try
            {
                var organisations = GetAll();
            
                if (organisations.Any(c => c.OrganisationId == organisationId))
                {
                    return true;
                }
                else
                {
                    throw new Exception("No organisation found");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public bool CheckIfOrganisationListIsEmpty(IEnumerable<Organisation> organisations)
        {
            try
            {
                if (organisations == null || !organisations.Any())
                {
                    throw new Exception("No organisations found");
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}