using System.Collections.Generic;
using Domain;

namespace DAL.RepoInterfaces
{
    public interface ISkillStudentRepository:IRepository<SkillStudent>
    {
        void RemoveAllSkillsPerformedByStudentId(long studentId);
    }
}