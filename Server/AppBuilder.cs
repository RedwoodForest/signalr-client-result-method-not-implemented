using Microsoft.AspNetCore.Http.Connections;

namespace Server;

public static class AppBuilder {
    public static WebApplication Build(String[] args) {

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSignalR()
            .AddMessagePackProtocol();

        var app = builder.Build();

        app.MapHub<DemonstrationHub>(
            DemonstrationHub.UrlPath,
            options => {
                // We agreed to limit supported transports to just WebSockets for now to minimize the amount of 
                // infrastructure configuration (e.g. sticky sessions for long polling transport) and testing 
                // (e.g. making sure the future RemoteConnect feature has reasonable performance over long polling.).
                //
                // It is also possible to configure supported transports on the client side, but we decided that we
                // prefer it to be controller server-side, so that we can enable additional protocols in the future
                // and have them be supported by existing agents (as long as the agent client library supports them).
                options.Transports = HttpTransportType.WebSockets;
            });

        return app;
    }
}