using Microsoft.AspNetCore.Builder;

namespace SE1426.Internet.ProdamSP.Blazor.Middleware
{
    public static class CaptchaHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCaptchaHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CaptchaHandlerMiddleware>();
        }
    }


}
