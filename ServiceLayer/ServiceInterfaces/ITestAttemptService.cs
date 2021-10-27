using System.Collections.Generic;
using Domain;

namespace ServiceLayer.ServiceInterfaces
{
    public interface ITestAttemptService
    {
        IEnumerable<TestAttempt> GetAllTestsAttemptedByStudentId(long studentId);
        void AddTestAttempt(Student student, int age, int tiger, int sprint, int ballHandling, int rolling, int agility);
        void RemoveAllTestsAttemptedByStudentId(long studentId);
    }
}