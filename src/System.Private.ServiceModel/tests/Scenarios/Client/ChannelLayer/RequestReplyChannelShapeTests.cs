// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Common;
using Xunit;

public partial class RequestReplyChannelShapeTests : ConditionalWcfTest
{
    // Creating a ChannelFactory using a binding's 'BuildChannelFactory' method and providing a channel shape...
    //       returns a concrete type determined by the channel shape requested and other binding related settings.
    // The tests in this file use the IRequestChannel shape.

#if FULLXUNIT_NOTSUPPORTED
    [Fact]
#else
    [ConditionalFact(nameof(Root_Certificate_Installed),
                     nameof(SSL_Available))]
#endif
    [OuterLoop]
    public static void IRequestChannel_Https_NetHttpsBinding()
    {
#if FULLXUNIT_NOTSUPPORTED
        bool root_Certificate_Installed = Root_Certificate_Installed();
        bool ssl_Available = SSL_Available();

        if (!root_Certificate_Installed ||
            !ssl_Available)
        {
            Console.WriteLine("---- Test SKIPPED --------------");
            Console.WriteLine("Attempting to run the test in ToF, a ConditionalFact evaluated as FALSE.");
            Console.WriteLine("Root_Certificate_Installed evaluated as {0}", root_Certificate_Installed);
            Console.WriteLine("SSL_Available evaluated as {0}", ssl_Available);
            return;
        }
#endif
        IChannelFactory<IRequestChannel> factory = null;
        IRequestChannel channel = null;
        Message replyMessage = null;

        try
        {
            // *** SETUP *** \\
            NetHttpsBinding binding = new NetHttpsBinding(BasicHttpsSecurityMode.Transport);

            // Create the channel factory
            factory = binding.BuildChannelFactory<IRequestChannel>(new BindingParameterCollection());
            factory.Open();

            // Create the channel.
            channel = factory.CreateChannel(new EndpointAddress(Endpoints.HttpBaseAddress_NetHttps));
            channel.Open();

            // Create the Message object to send to the service.
            Message requestMessage = Message.CreateMessage(
                binding.MessageVersion,
                action,
                new CustomBodyWriter(clientMessage));

            // *** EXECUTE *** \\
            // Send the Message and receive the Response.
            replyMessage = channel.Request(requestMessage);

            // *** VALIDATE *** \\
            var replyReader = replyMessage.GetReaderAtBodyContents();
            string actualResponse = replyReader.ReadElementContentAsString();
            string expectedResponse = "[client] This is my request.[service] Request received, this is my Reply.";
            Assert.Equal(expectedResponse, actualResponse);

            // *** CLEANUP *** \\
            replyMessage.Close();
            channel.Close();
            factory.Close();
        }
        finally
        {
            // *** ENSURE CLEANUP *** \\
            ScenarioTestHelpers.CloseCommunicationObjects(channel, factory);
        }
    }
}
