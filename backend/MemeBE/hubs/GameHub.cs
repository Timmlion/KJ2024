using Microsoft.AspNetCore.SignalR;

namespace MemeBE.hubs;

public class GameHub : Hub
{
    private static int nextRoomID = 1;
    
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task CreateRoom(string playerName)
    {
        await JoinRoom(playerName, nextRoomID.ToString());
        nextRoomID++;
    }

    public async Task JoinRoom(string playerName, string roomIDstr)
    {
        int roomID = Int32.Parse(roomIDstr);
        await Groups.AddToGroupAsync(Context.ConnectionId, nextRoomID.ToString());
        await Clients.Caller.SendAsync("JoinedToRoom", nextRoomID);
        await Clients.OthersInGroup(roomID.ToString()).SendAsync("NewPlayerJoinedToRoom", playerName);
    }
}