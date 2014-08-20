using System;

namespace McSlimUpdater
{
    public class UpdateException : Exception
    {
        public string Reason { get; private set; }

        public UpdateException(string reason = "")
        {
            this.Reason = reason;
        }
    }
}
