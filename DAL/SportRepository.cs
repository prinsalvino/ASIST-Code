using DAL.RepoInterfaces;
using Domain;

namespace DAL
{
    public class SportRepository: Repository<Sport>, ISportRepository
    {
        public SportRepository(SportingContext context) : base(context)
        {
            
        }
    }
}