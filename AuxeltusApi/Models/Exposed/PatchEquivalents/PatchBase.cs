using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auxeltus.Api.Models
{
    public class PatchBase
    {
        private Dictionary<string, object> _specifiedProperties;

        public bool PropertySpecified(string propName)
        {
            return _specifiedProperties.ContainsKey(propName);
        }

        protected void SpecifyProperty(string propName, object value)
        {
            _specifiedProperties.Add(propName, value);
        }

        protected T ObtainValue<T>(string propName)
        {
            return (T) _specifiedProperties[propName] ?? default;
        }
    }
}
