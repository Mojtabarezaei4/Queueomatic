using Microsoft.AspNetCore.SignalR;

namespace Queueomatic.Server.Endpoints.Hubs.Room;

public class RoomHub : Hub
{
    public async Task SendMessage(string user, string message, string roomId)
    {
        await Clients.Groups(roomId).SendAsync("ReceiveMessage", user, message);
    }

    public  Task JoinRoom(string roomName)
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, roomName);
    }

    public Task LeaveRoom(string roomName)
    {
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
    }
}