using System.Collections.Generic;
using Domain;

namespace ServiceLayer.ServiceInterfaces
{
    public interface ISkillStudentService
    {
        IEnumerable<SkillStudent> GetAll(long studentId);
        void AddSkillLapTimes(List<SkillStudent> skills, Gender gender, int age, int tiger, int sprint, int ballHandling, int rolling, int agility);
        bool CheckIfSkillStudentListIsEmpty(IEnumerable<SkillStudent> skillStudents);
        void RemoveAllSkillsPerformedByStudentId(long studentId);
    }
}