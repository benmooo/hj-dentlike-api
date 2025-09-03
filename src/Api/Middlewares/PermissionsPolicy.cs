namespace Dentlike.Api.Middlewares
{
    public class PermissionsPolicyMiddleware
    {
        private readonly RequestDelegate _next;

        public PermissionsPolicyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.Headers.Append("Permissions-Policy", "geolocation=(), midi=()");
            await _next(context);
        }
    }

    public static class PermissionsPolicyMiddlewareExtensions
    {
        public static IApplicationBuilder UsePermissionPolicy(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PermissionsPolicyMiddleware>();
        }
    }
}
