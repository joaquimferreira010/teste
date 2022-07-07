using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProdamSP.CAC.Token.Std
{
    public static class ApiCacExtensions
    {
        public static IApplicationBuilder UseApiCac(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CacJwtMiddleware>();
        }
    }
}
