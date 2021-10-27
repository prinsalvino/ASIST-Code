using System.Collections.Generic;
using Domain;

namespace DAL.RepoInterfaces
{
    public interface ITestAttemptRepository: IRepository<TestAttempt>
    {
        IEnumerable<TestAttempt> GetAllTestsAttemptedByStudentId(long studentId);
        void RemoveAllTestAttemptsByStudentId(long studentId);
    }
}