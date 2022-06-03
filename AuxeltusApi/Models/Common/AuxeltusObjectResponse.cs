using System.Collections.Generic;

namespace Auxeltus.Api.Models
{
    public class AuxeltusObjectResponse<T>
    {
        public bool Success { get; set; }
        public T Content { get; set; }
        public List<Error> Errors { get; set; }

        public void AddError(Error error)
        {
            Errors ??= new List<Error>();
            Errors.Add(error);
        }
    }

    public class AuxeltusObjectResponse : AuxeltusObjectResponse<string> { }
}
