using System.Collections.Generic;
using System.Linq;
using DAL.RepoInterfaces;
using Domain;

namespace DAL
{
    public class SkillStudentRepository:Repository<SkillStudent>, ISkillStudentRepository
    {
        private readonly SportingContext _context;
        public SkillStudentRepository(SportingContext context) : base(context)
        {
            _context = context;
        }

        public void RemoveAllSkillsPerformedByStudentId(long studentId)
        {
            var student = _context.Students.Find(studentId);

            DbContext.Entry(student).Collection(o => o.SkillsPerformed).Load();

            var skillsPerformed = DbContext.Set<SkillStudent>()
                .Where(u => u.StudentId == student.UserId);
            
            DbContext.Set<SkillStudent>().RemoveRange(skillsPerformed);
            
            DbContext.SaveChangesAsync();
        }
    }
}