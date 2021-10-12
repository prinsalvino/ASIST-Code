using System.Collections.Generic;
using Domain;

namespace ServiceLayer
{
    public interface ISkillService
    {
        IEnumerable<SkillStudent> GetAll(long studentId);
        void AddSkills(List<SkillStudent> skills);
    }
}