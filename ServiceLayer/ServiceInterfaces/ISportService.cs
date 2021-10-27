using System.Collections.Generic;
using Domain;

namespace ServiceLayer.ServiceInterfaces
{
    public interface ISportService
    {
        Sport GetSportById(long sportId);
        IEnumerable<Sport> GetAllSports();
    }
}