using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Auxeltus.Api
{
    public class PatchFromBodyBindingSource : BindingSource
    {
        public static readonly BindingSource PatchFromBody = new PatchFromBodyBindingSource(
            "PatchFromBody",
            "PatchFromBody",
            true,
            true
        );

        public PatchFromBodyBindingSource(string id, string displayName, bool isGreedy, bool isFromRequest)
            : base(id, displayName, isGreedy, isFromRequest)
        {
        }

        public override bool CanAcceptDataFrom(BindingSource bindingSource)
        {
            return bindingSource == this;
        }
    }
}
