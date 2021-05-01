using System;
using Frameworks.Cqrs.Commands;

namespace VPFrameworks.Cqrs.Tests
{
    public class MyCommand : ICommand
    {
        public MyCommand()
        {
            this.Id = Guid.NewGuid();
            this.Created = DateTime.UtcNow;
        }

        public Guid Id {get;}

        public DateTime Created { get; }
    }
}