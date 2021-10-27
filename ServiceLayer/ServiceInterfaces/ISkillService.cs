using System.Collections.Generic;
using Domain;

namespace ServiceLayer.ServiceInterfaces
{
    public interface ISkillService
    {
        IEnumerable<Skill> GetAllSkills();
        Skill GetSkillById(long skillId);
    }
}