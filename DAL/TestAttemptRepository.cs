using System.Collections.Generic;
using System.Linq;
using DAL.RepoInterfaces;
using Domain;

namespace DAL
{
    public class TestAttemptRepository: Repository<TestAttempt>, ITestAttemptRepository
    {
        private readonly SportingContext _context;
        public TestAttemptRepository(SportingContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<TestAttempt> GetAllTestsAttemptedByStudentId(long studentId)
        {
            var testsAttempted = DbContext.Set<TestAttempt>().Where(t => t.StudentId == studentId);
            return testsAttempted;
        }

        public void RemoveAllTestAttemptsByStudentId(long studentId)
        {
            var student = _context.Students.Find(studentId);

            DbContext.Entry(student).Collection(o => o.TestsAttempted).Load();

            var testAttempts = DbContext.Set<TestAttempt>()
                .Where(u => u.StudentId == student.UserId);
            
            DbContext.Set<TestAttempt>().RemoveRange(testAttempts);
            
            DbContext.SaveChangesAsync();
        }
    }
}