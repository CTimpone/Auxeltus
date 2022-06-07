using Auxeltus.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Auxeltus.Api.Attributes
{
    public class ModelValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = BuildErrorResponse(context.ModelState);
            }

            base.OnActionExecuting(context);
        }

        private IActionResult BuildErrorResponse(ModelStateDictionary state)
        {
            AuxeltusObjectResponse rsp = new AuxeltusObjectResponse
            {
                Success = false,
                Errors = new List<Error>()
            };

            foreach (string key in state.Keys)
            {
                foreach (var error in state[key].Errors)
                {
                    rsp.Errors.Add(new Error
                    {
                        Field = key,
                        Message = error.ErrorMessage,
                        Type = ErrorType.Error,
                        Code = MapErrorCode(error.ErrorMessage)
                    });
                }
            }

            return new BadRequestObjectResult(rsp);
        }

        private int MapErrorCode(string error)
        {
            //To be further refined
            if (error.Contains("required", StringComparison.OrdinalIgnoreCase)) {
                return 1;
            } else
            {
                return 2;
            }
        }
    }
}
