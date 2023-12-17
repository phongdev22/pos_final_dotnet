public class FirstLoginMiddleware
{
	private readonly RequestDelegate _next;

	public FirstLoginMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task Invoke(HttpContext context)
	{
		var firstLoginClaim = context.User.Claims.FirstOrDefault(c => c.Type == "FirstLogin")?.Value;

		if (bool.TryParse(firstLoginClaim, out var isFirstLogin) && isFirstLogin)
		{
			context.Response.Redirect("/Auth/first-login");
			return;
		}
		await _next(context);
	}
}
