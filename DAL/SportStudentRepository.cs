using System.Collections.Generic;
using System.Linq;
using DAL.RepoInterfaces;
using Domain;

namespace DAL
{
    public class SportStudentRepository: Repository<SportStudent>, ISportStudentRepository
    {
        private readonly SportingContext _context;
        public SportStudentRepository(SportingContext context) : base(context)
        {
            _context = context;
        }
        
        public IEnumerable<SportStudent> GetAllSportAdvicesByStudentId(long studentId)
        {
            var sportAdvices = DbContext.Set<Student>().Where(u => u.UserId == studentId)
                .SelectMany(s => s.SportAdvices);
            return sportAdvices;
        }

        public SportStudent GetSportAdviceByStudentIdAndSportId(long studentId, long sportId)
        {
            var sportAdvice = DbContext.Set<SportStudent>()
                .Single(u => u.StudentId == studentId && u.SportId == sportId);
            return sportAdvice;
        }

        public void RemoveAllSportAdvicesByStudentId(long studentId)
        {
            var student = _context.Students.Find(studentId);

            DbContext.Entry(student).Collection(o => o.SportAdvices).Load();

            var sportAdvices = DbContext.Set<SportStudent>()
                .Where(u => u.StudentId == student.UserId);
            
            DbContext.Set<SportStudent>().RemoveRange(sportAdvices);
            
            DbContext.SaveChangesAsync();
        }
    }
}