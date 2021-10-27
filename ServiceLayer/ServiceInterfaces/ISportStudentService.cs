using System.Collections.Generic;
using Domain;

namespace ServiceLayer.ServiceInterfaces
{
    public interface ISportStudentService
    {
        IEnumerable<SportStudent> GetAllSportAdvicesByStudentId(long studentId);
        SportStudent GetSportAdviceByStudentIdAndSportId(long studentId, long sportId);
        void AddSportAdvices(Student student, int age, int tiger, int sprint, int ballHandling, int rolling, int agility);
        void RemoveSportAdvicesByStudentId(long studentId);
    }
}