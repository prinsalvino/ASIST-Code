using DAL.RepoInterfaces;
using Domain;

namespace DAL
{
    public class SkillRepository: Repository<Skill>, ISkillRepository
    {
        public SkillRepository(SportingContext context) : base(context)
        {
            
        }
    }
}