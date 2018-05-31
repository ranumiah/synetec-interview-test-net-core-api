using System;

namespace Synetec.CityInfo.Core.Exceptions
{
    public class CityNotFoundException : Exception
    {
        public CityNotFoundException() : base() { }

        public CityNotFoundException(string message) : base(message) { }

        public CityNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
