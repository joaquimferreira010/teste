using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using ProdamSP.Domain.Interfaces.Business;

namespace SE1426.Internet.ProdamSP.Blazor.Middleware
{
    public class CaptchaHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICaptchaBusiness _captchaBusiness;

        public CaptchaHandlerMiddleware(RequestDelegate next,
           ICaptchaBusiness captchaBusiness)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
           
           

            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case KeyNotFoundException e:
                        // not found error
                        break;
                    default:
                        // unhandled error
                        break;
                }

               
            }



        }
    }


}
