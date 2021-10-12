using System.Collections.Generic;
using Domain;

namespace ASIST.Repository
{
    public class SkillRepository:Repository<SkillStudent>, ISkillRepository
    {
        public SkillRepository(SportingContext context) : base(context)
        {
            
        }
    }
}