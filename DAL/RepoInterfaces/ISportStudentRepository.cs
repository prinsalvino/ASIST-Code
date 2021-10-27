using System.Collections.Generic;
using Domain;

namespace DAL.RepoInterfaces
{
    public interface ISportStudentRepository: IRepository<SportStudent>
    {
        IEnumerable<SportStudent> GetAllSportAdvicesByStudentId(long studentId);
        SportStudent GetSportAdviceByStudentIdAndSportId(long studentId, long sportId);
        void RemoveAllSportAdvicesByStudentId(long studentId);

    }
}