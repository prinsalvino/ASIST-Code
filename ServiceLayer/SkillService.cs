using System.Collections.Generic;
using System.Linq;
using ASIST.Repository;
using Domain;

namespace ServiceLayer
{
    public class SkillService: ISkillService
    {
        private readonly ISkillRepository _repository;

        public SkillService(ISkillRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<SkillStudent> GetAll(long studentId)
        {
            return _repository.GetAll().Where(e=>e.StudentId == studentId);
        }

        public void AddSkills(List<SkillStudent> skillStudents)
        {
            _repository.AddMany(skillStudents);
        }

    }
}