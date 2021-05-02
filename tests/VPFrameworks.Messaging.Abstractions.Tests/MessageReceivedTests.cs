using System;
using VPFrameworks.Messaging.Abstractions.Subscriber;
using Xunit;

namespace InfrastrutureClients.Messaging.Abstractions.Tests
{

    public class MessageReceivedTests
    {
        [Fact]
        public void WhenCtorSetAllValuesNull_ThenAllPropertiesWillHAveNullValues()
        {
            MessageReceived<byte[]> messageReceived = new MessageReceived<byte[]>(null, null, null, null);

            Assert.Null(messageReceived.MessageId);
            Assert.Null(messageReceived.PopReceipt);
            Assert.Null(messageReceived.ClientId);
            Assert.Null(messageReceived.Payload);


        }

        [Fact]
        public void WhenCtorSetOnlyMessageIdValue_ThenAllOthersPropertiesWillHAveNullValues()
        {
            var expected = "1";
            MessageReceived<byte[]> messageReceived = new MessageReceived<byte[]>(expected, null, null, null);


            Assert.Equal(expected, messageReceived.MessageId);
            Assert.Null(messageReceived.PopReceipt);
            Assert.Null(messageReceived.Payload);
        }

        [Fact]
        public void WhenCtorSetOnlyPopReceiptValue_ThenAllOthersPropertiesWillHAveNullValues()
        {
            var expected = "1";
            MessageReceived<byte[]> messageReceived = new MessageReceived<byte[]>(null, expected, null, null);


            Assert.Null(messageReceived.MessageId);
            Assert.Equal(expected, messageReceived.PopReceipt);
            Assert.Null(messageReceived.Payload);
        }

        [Fact]
        public void WhenCtorSetOnlyClientIdValue_ThenAllOthersPropertiesWillHAveNullValues()
        {
            var expected = "1";
            MessageReceived<byte[]> messageReceived = new MessageReceived<byte[]>(null, null, expected, null);


            Assert.Null(messageReceived.MessageId);
            Assert.Null(messageReceived.PopReceipt);
            Assert.Equal(expected, messageReceived.ClientId);
            Assert.Null(messageReceived.Payload);
        }
    }
}
