using Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Endpoint.SignalRHub;

public class VocabHub(IRedisCache RedisCache, ISharedDb sharedDb) : Hub
{
    private readonly IRedisCache _RedisCache = RedisCache;
    private readonly ISharedDb _sharedDb = sharedDb;

    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            var rooms = _sharedDb.GetRoomsByUserId(int.Parse(userId));
            foreach (var room in rooms)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, room.ToString());
            }
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            var rooms = _sharedDb.GetRoomsByUserId(int.Parse(userId));
            foreach (var room in rooms)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.ToString());
            }
        }
        await base.OnDisconnectedAsync(exception);
    }

    public async Task JoinRoom(int roomId)
    {
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            _sharedDb.AddUserToRoom(int.Parse(userId), roomId);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
        }
    }

    public async Task LeaveRoom(int roomId)
    {
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            _sharedDb.RemoveUserFromRoom(int.Parse(userId), roomId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
        }
    }

    public async Task SendMessageToRoom(int roomId, string message)
    {
        await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", message);
    }
}
