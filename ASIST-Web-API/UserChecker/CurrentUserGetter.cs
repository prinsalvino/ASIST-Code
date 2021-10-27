using Domain;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.ServiceInterfaces;

namespace ASIST_Project_Web_API.UserChecker
{
    public class CurrentUserGetter
    {
        public UserBase getCurrentUser(ClaimsPrincipal User, IUserService _userService)
        {
            var claims = User.Identities.FirstOrDefault().Claims.ToList();
            long userId = Convert.ToInt64(claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault());
            return _userService.GetUser(userId);
        }


    }
}
