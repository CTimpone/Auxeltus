using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Auxeltus.Api
{
    public class PatchFromBodyModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.BindingInfo.BindingSource != null
                && context.BindingInfo.BindingSource.CanAcceptDataFrom(PatchFromBodyBindingSource.PatchFromBody))
            {
                return new PatchFromBodyModelBinder();
            }
            else
            {
                return null;
            }
        }
    }
}
