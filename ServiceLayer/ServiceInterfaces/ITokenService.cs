using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.ServiceInterfaces
{
    public  interface ITokenService
    {
        
            Task<JWTResponse> CreateToken(UserLogin Login);
            Task<ClaimsPrincipal> GetByValue(string Value);
        

    }
}
