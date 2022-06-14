using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;


namespace Auxeltus.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PatchFromBodyAttribute : Attribute, IBindingSourceMetadata {
        BindingSource IBindingSourceMetadata.BindingSource => PatchFromBodyBindingSource.PatchFromBody;
    }
}