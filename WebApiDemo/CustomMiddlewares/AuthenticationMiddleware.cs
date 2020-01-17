using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebApiDemo.CustomMiddlewares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string authHeader = httpContext.Request.Headers["Authorization"];
            if(authHeader!=null&&
                authHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
            {
                var token = authHeader.Substring(6).Trim();
                var credentialString = "";
                try
                {

                    credentialString = Encoding.UTF8.GetString
                    (Convert.FromBase64String(token));
                }
                catch (Exception)
                {
                    httpContext.Response.StatusCode = 500;
                }
                var credentials = credentialString.Split(":");
                if(credentials[0]=="elvin" && credentials[1] == "12345")
                {
                    var claims = new[] {
                        new Claim("name",credentials[0]),
                        new Claim(ClaimTypes.Role,"Admin")
                    };
                    var identity = new ClaimsIdentity(claims, "Basic");
                    httpContext.User = new ClaimsPrincipal(identity);

                }
            }
            else
            {
                httpContext.Response.StatusCode = 401;
            }
            await _next(httpContext);
        }
    }
}
