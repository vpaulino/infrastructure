using Frameworks.Cqrs;
using Frameworks.Cqrs.Commands;
using Moq;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace InfrastrutureClients.Cqrs.Tests
{
    public class CommandDispatcherTests
    {

        [Fact]
        public async Task WhenRegisting_NewCommandDispatcher_ShouldDispatchCommands()
        {
            CommandDispatcher commandDispatcher = new CommandDispatcher();

            ManualResetEventSlim triggerAssert = new ManualResetEventSlim(false);
                    
            var mockedCOmmandHandler = new Mock<IUnitOfWork<MyCommand>>();
            mockedCOmmandHandler.Setup((entity) => entity.Execute(It.IsAny<MyCommand>(), It.IsAny<CancellationToken>())).Callback<MyCommand, CancellationToken>((command, ct) => triggerAssert.Set()).Returns(Task.CompletedTask);

            CommandHandler<MyCommand> commandHandler = new CommandHandler<MyCommand>(mockedCOmmandHandler.Object);

            commandDispatcher.Register<MyCommand>(commandHandler);

            await commandDispatcher.Dispatch(new MyCommand(), CancellationToken.None);

            bool result = triggerAssert.Wait(new TimeSpan(0,0,1));

            Assert.True(result);

        }
    }
}
