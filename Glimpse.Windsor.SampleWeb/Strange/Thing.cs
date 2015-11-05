using System;

namespace Glimpse.Windsor.SampleWeb.Strange
{
    class Thing : IThing
    {
        private readonly IConvertible _stuff;

        public Thing(IConvertible stuff)
        {
            _stuff = stuff;
        }
    }
}