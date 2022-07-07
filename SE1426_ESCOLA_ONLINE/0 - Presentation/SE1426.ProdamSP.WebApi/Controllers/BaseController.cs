using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using ProdamSP.Domain.Constants;
using ProdamSP.CAC.Token.Std;

namespace SE1426.ProdamSP.WebApi.Controllers
{
    [TokenAuthorizeAttribute]
    public abstract class BaseController : Controller
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.ModelState.IsValid)
            {
                filterContext.Result = new ValidationFailedResult(filterContext.ModelState);
            }
        }

    }

    public class ValidationFailedResult : ObjectResult
    {
        public ValidationFailedResult(ModelStateDictionary modelState)
            : base(new ValidationResultModel(modelState))
        {
            StatusCode = StatusCodes.Status400BadRequest;
        }

        public ValidationFailedResult(object value) : base(value)
        {
        }
    }

    public class ValidationError
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string campo { get; }

        public string Message { get; }

        public ValidationError(string field, string message)
        {
            campo = field != string.Empty ? field : null;
            Message = message;
        }
    }

    public class ValidationResultModel
    {
        public string status { get; }
        public string message { get; }
        public string code { get; }

        public List<ValidationError> developerMessage { get; }

        public ValidationResultModel(ModelStateDictionary modelState)
        {
            status = StatusCodes.Status400BadRequest.ToString();
            message = "Bad Request";
            code = "";
            developerMessage = modelState.Keys
                               .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                               .ToList();

        }
    }

    public class ReturnErrorResult
    {
        public int status { get; }
        public string message { get; }
        public string code { get; }
        public string developerMessage { get; }

        public ReturnErrorResult(Task task)
        {
            if (task.Exception.Message.Contains(STATUS_CODE_WEBAPI.STATUS_404_NOTFOUND))
            {
                status = StatusCodes.Status404NotFound;
                code = StatusCodes.Status404NotFound.ToString();
                message = "Not Found";
            }
            else
            {
                status = StatusCodes.Status500InternalServerError;
                code = StatusCodes.Status500InternalServerError.ToString();
                message = "Internal Server Error";
            }
        }
    }
}
