using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;


namespace whiteboardEtutor.SignalRhub
{
    [HubName("whiteboardHub")]
    public class WhiteboardHub : Hub
    {
        public async Task JoinGroup(string groupName)
        {
            await Groups.Add(Context.ConnectionId, groupName);
        }

        public async Task SendDraw(string drawObject, string groupName, string sessinId)
        {
            await Clients.Group(groupName).HandleDraw(drawObject, sessinId);
        }
    }
}