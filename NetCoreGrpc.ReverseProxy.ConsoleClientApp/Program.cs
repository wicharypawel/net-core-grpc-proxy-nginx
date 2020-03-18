using System;
using System.Net.Http;
using System.Threading;
using Grpc.Net.Client;
using NetCoreGrpc.HelloWorld.Proto;

namespace NetCoreGrpc.ReverseProxy.ConsoleClientApp
{
    public class Program
    {
        public static void Main()
        {
            Thread.Sleep(TimeSpan.FromSeconds(2)); // wait for server to wake up
            var channelOptions = new GrpcChannelOptions() { HttpClient = CreateGrpcHttpClient(acceptSelfSignedCertificate: true) };
            var channel = GrpcChannel.ForAddress("https://grpc-reverseproxy-nginx:443", channelOptions);
            var client = new Greeter.GreeterClient(channel);
            var user = "Pawel";
            var reply = client.SayHello(new HelloRequest { Name = user });
            Console.WriteLine("Greeting: " + reply.Message);
            channel.ShutdownAsync().Wait();
        }

        private static HttpClient CreateGrpcHttpClient(bool allowHttp2Unencrypted = true, bool acceptSelfSignedCertificate = false)
        {
            if (allowHttp2Unencrypted)
            {
                // https://docs.microsoft.com/en-us/aspnet/core/grpc/troubleshoot?view=aspnetcore-3.1#call-insecure-grpc-services-with-net-core-client
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            }
            if (acceptSelfSignedCertificate)
            {
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (msg, cert, chain, errors) => true;
                var httpClient = new HttpClient(handler);
                httpClient.Timeout = Timeout.InfiniteTimeSpan;
                return httpClient;
            }
            else
            {
                var httpClient = new HttpClient();
                httpClient.Timeout = Timeout.InfiniteTimeSpan;
                return httpClient;
            }
        }
    }
}
