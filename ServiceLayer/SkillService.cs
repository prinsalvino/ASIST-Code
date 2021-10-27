using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using DAL.RepoInterfaces;
using Domain;
using ServiceLayer.ServiceInterfaces;

namespace ServiceLayer
{
    public class SkillService: ISkillService
    {
        private readonly ISkillRepository _repository;

        public SkillService(ISkillRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Skill> GetAllSkills()
        {
            try
            {
                var skills = _repository.GetAll().ToList();
                if (skills == null || !skills.Any())
                {
                    throw new Exception("No skills found");
                }

                return skills;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public Skill GetSkillById(long skillId)
        {
            try
            {
                var skill = _repository.GetSingle(skillId);
                if (skill == null)
                {
                    throw new Exception("Skill not found");
                }

                return skill;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}