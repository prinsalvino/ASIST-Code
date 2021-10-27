using Microsoft.Azure.Functions.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ASIST_Project_Web_API.Utils
{
    public static  class FunctionContextExtension
    {
		public static ClaimsPrincipal GetUser(this FunctionContext FunctionContext)
		{
			try
			{
				return (ClaimsPrincipal)FunctionContext.Items["User"];
			}
			catch (Exception e)
			{
				throw new UnauthorizedAccessException(/*e.Message*/);
			}
		}
	}
}
