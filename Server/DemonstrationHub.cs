using Microsoft.AspNetCore.SignalR;

namespace Server;

public class DemonstrationHub : Hub<IClientContract> {

    public const String UrlPath = "/demonstrationHub";

}