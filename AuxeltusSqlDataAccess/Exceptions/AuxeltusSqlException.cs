using System;

namespace Auxeltus.AccessLayer.Sql
{
    public class AuxeltusSqlException : Exception
    {
        public AuxeltusSqlException()
        {
        }

        public AuxeltusSqlException(string message)
            : base(message)
        {
        }

        public AuxeltusSqlException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}