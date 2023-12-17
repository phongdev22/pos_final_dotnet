using System.Security.Claims;

public class UserInfoMiddleware
{
	private readonly RequestDelegate _next;

	public UserInfoMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task Invoke(HttpContext context)
	{
		var userName = context.User.Identity.Name;
		var userEmail = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
		var avatar = context.User.Claims.FirstOrDefault(c => c.Type == "Avatar")?.Value;

		context.Items["UserName"] = userName;
		context.Items["UserEmail"] = userEmail;
		context.Items["Avatar"] = avatar;

		await _next(context);
	}
}