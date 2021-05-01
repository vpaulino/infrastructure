using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Frameworks.Cqrs.Commands
{
    /// <summary>
    /// Contract that represents what is mandatory that Commands need to have 
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Gets or sets the unique Id of the command
        /// </summary>
        Guid Id { get;  }

        /// <summary>
        /// Gets or sets the Date when this command was created
        /// </summary>
        DateTime Created { get;  }

    }
}
