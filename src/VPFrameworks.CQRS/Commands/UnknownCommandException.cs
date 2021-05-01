using System;
using System.Runtime.Serialization;

namespace Frameworks.Cqrs.Commands
{
    [Serializable]
    internal class UnknownCommandException : Exception
    {
        private ICommand command;

        public UnknownCommandException()
        {
        }

        public UnknownCommandException(ICommand command)
        {
            this.command = command;
        }

        public UnknownCommandException(string message) : base(message)
        {
        }

        public UnknownCommandException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnknownCommandException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}