using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;

namespace WebApplication1.Misc
{
    public class ModelStateUtility
    {
        //從 ModelState 取得一個 Error
        public static string GetErrorMessage(ModelStateDictionary modelState)
        {
            string errorMessage = "";

            if (modelState.Values == null ||
                modelState.Values.Count() == 0 ||
                modelState.Values.First().Errors == null ||
                modelState.Values.First().Errors.Count() == 0)
            {
                return errorMessage;
            }

            var error = modelState.Values.First().Errors.First();

            errorMessage = error.ErrorMessage;
            if (string.IsNullOrEmpty(errorMessage) && error.Exception != null)
            {
                errorMessage = error.Exception.Message;
            }
            
            return errorMessage;
        }
    }
}