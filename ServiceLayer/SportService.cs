using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using DAL.RepoInterfaces;
using Domain;
using ServiceLayer.ServiceInterfaces;

namespace ServiceLayer
{
    public class SportService: ISportService
    {
        private readonly ISportRepository _repository;

        public SportService(ISportRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Sport> GetAllSports()
        {
            try
            {
                var sports = _repository.GetAll().ToList();
                if (sports == null || !sports.Any())
                {
                    throw new Exception("No sports found");
                }
                return sports;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public Sport GetSportById(long sportId)
        {
            try
            {
                var sport = _repository.GetSingle(sportId);

                if (sport == null)
                {
                    throw new Exception("Sport not found");
                }

                return sport;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}