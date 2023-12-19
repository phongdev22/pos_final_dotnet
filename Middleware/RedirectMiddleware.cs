using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using pos.Entities;

public class RedirectMiddleware
{
    private readonly RequestDelegate _next;

    public RedirectMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var firstLoginClaim = context.User.Claims.FirstOrDefault(c => c.Type == "FirstLogin");

        if (firstLoginClaim != null)
        {
            if (context.Request.Path.StartsWithSegments("/Auth/first-login"))
            {
                await _next(context);
                return;
            }

            if (bool.TryParse(firstLoginClaim.Value, out var isFirstLogin) && isFirstLogin)
            {
                context.Items["Redirected"] = "true";
                context.Response.Redirect("/Auth/first-login");
                return;
            }
        }

        if (context.User.Identity != null && context.User.Identity.IsAuthenticated && context.Request.Path == "/Auth")
        {
            context.Response.Redirect("/");
            return;
        }

        await _next(context);
    }
}
