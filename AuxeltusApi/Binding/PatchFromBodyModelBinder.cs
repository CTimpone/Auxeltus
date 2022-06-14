using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Auxeltus.Api
{
    public class PatchFromBodyModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {

            using (StreamReader reader = new StreamReader(bindingContext.HttpContext.Request.Body,
                Encoding.UTF8))
            {
                var serialized = reader.ReadToEnd();
                bindingContext.Result = ModelBindingResult.Success(JsonConvert.DeserializeObject(serialized, bindingContext.ModelType));
            }

            return Task.CompletedTask;
        }
    }
}
