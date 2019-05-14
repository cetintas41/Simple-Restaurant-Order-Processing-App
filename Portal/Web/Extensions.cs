using System;
using System.Security.Claims;

namespace Web
{
    public static class Extensions
    {
       
    }

    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Sisteme giriş yapan kullanıcının Id'sini döndürür.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
