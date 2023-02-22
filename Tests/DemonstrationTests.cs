using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Server;

namespace Tests; 

[TestFixture]
public class DemonstrationTests {

    // Passes
    [Timeout(5000)]
    [Test]
    public async Task UnhandledWeaklyTypedClientResultMethodCalled() {
    
        var app = BuildApp(port: 7878);

        await app.StartAsync();

        try {
            var clientConnection = new HubConnectionBuilder()
                .AddMessagePackProtocol()
                .WithUrl(app.Urls.Single() + DemonstrationHub.UrlPath)
                .Build();

            await clientConnection.StartAsync();

            var hubContext = app.Services.GetRequiredService<IHubContext<DemonstrationHub>>();

            var client = hubContext.Clients.Client(clientConnection.ConnectionId!);
            
            Assert.That(
                async () => await client.InvokeAsync<Object>("UnhandledMethod", CancellationToken.None),
                Throws.Exception.With.Message.EqualTo("Client didn't provide a result."));
        } finally {
            await app.StopAsync();
        }
    }

    /// Fails with timeout
    [Timeout(5000)]
    [Test]
    public async Task UnhandledWeaklyTypedClientResultMethodWithArgCalled() {
    
        var app = BuildApp(port: 7878);

        await app.StartAsync();

        try {
            var clientConnection = new HubConnectionBuilder()
                .AddMessagePackProtocol()
                .WithUrl(app.Urls.Single() + DemonstrationHub.UrlPath)
                .Build();

            await clientConnection.StartAsync();

            var hubContext = app.Services.GetRequiredService<IHubContext<DemonstrationHub>>();

            var client = hubContext.Clients.Client(clientConnection.ConnectionId!);
            
            Assert.That(
                async () => await client.InvokeAsync<Object>("UnhandledMethod", new Object(), CancellationToken.None),
                Throws.Exception.With.Message.EqualTo("Client didn't provide a result."));
        } finally {
            await app.StopAsync();
        }
    }

    // Passes
    [Timeout(5000)]
    [Test]
    public async Task UnhandledStronglyTypedClientResultMethodCalled() {
    
        var app = BuildApp(port: 7879);

        await app.StartAsync();

        try {
            var clientConnection = new HubConnectionBuilder()
                .WithUrl(app.Urls.Single() + DemonstrationHub.UrlPath)
                .Build();

            //clientConnection.On<Object>("UnhandledClientResultMethod", () => new Object());

            await clientConnection.StartAsync();

            var hubContext = app.Services.GetRequiredService<IHubContext<DemonstrationHub, IClientContract>>();

            var client = hubContext.Clients.Client(clientConnection.ConnectionId!);
            
            Assert.That(
                async () => await client.UnhandledClientResultMethod(),
                Throws.Exception.With.Message.EqualTo("Client didn't provide a result."));
        } finally {
            await app.StopAsync();
        }
    }

    /// Fails with timeout
    [Timeout(5000)]
    [Test]
    public async Task UnhandledStronglyTypedClientResultMethodWithArgCalled() {
    
        var app = BuildApp(port: 7879);

        await app.StartAsync();

        try {
            var clientConnection = new HubConnectionBuilder()
                .WithUrl(app.Urls.Single() + DemonstrationHub.UrlPath)
                .Build();

            //clientConnection.On<Object>("UnhandledClientResultMethod", () => new Object());

            await clientConnection.StartAsync();

            var hubContext = app.Services.GetRequiredService<IHubContext<DemonstrationHub, IClientContract>>();

            var client = hubContext.Clients.Client(clientConnection.ConnectionId!);
            
            Assert.That(
                async () => await client.UnhandledClientResultMethodWithArg(new Object()),
                Throws.Exception.With.Message.EqualTo("Client didn't provide a result."));
        } finally {
            await app.StopAsync();
        }
    }

    private static WebApplication BuildApp(Int32 port) {

        var url = new UriBuilder(
                Uri.UriSchemeHttp,
                IPAddress.Loopback.ToString(),
                port)
            .Uri;

        var args = new[] {
            $"Kestrel:EndPoints:Http:Url={url.AbsoluteUri}"
        };

        return AppBuilder.Build(args);
    }

}