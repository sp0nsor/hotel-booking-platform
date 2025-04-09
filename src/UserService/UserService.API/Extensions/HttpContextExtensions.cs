namespace UserService.API.Extensions
{
    public static class HttpContextExtensions
    {
        public static void SetAuthTokens(
            this HttpContext httpContext,
            string refreshToken, 
            string accessToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None
            };

            httpContext.Response.Cookies.Append("testy-cookies", refreshToken, cookieOptions);
            httpContext.Response.Headers["Authorization"] = $"Bearer {accessToken}";
        }

        public static void DeleteAuthTokens(this HttpContext httpContext)
        {
            httpContext.Response.Headers.Remove("Authorization");
            httpContext.Response.Cookies.Delete("testy-cookies");
        }
    }
}
