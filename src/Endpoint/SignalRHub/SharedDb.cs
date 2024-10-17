using System.Collections.Concurrent;

namespace Endpoint.SignalRHub
{
    public interface ISharedDb
    {
        IList<int> GetRoomsByUserId(int userId);
        IList<int> GetUsersByRoomId(int roomId);
        void AddUserToRoom(int userId, int roomId);
        void RemoveUserFromRoom(int userId, int roomId);
    }

    public class SharedDb : ISharedDb
    {
        private readonly ConcurrentDictionary<int, IList<int>> UserToRooms = new ConcurrentDictionary<int, IList<int>>();
        private readonly ConcurrentDictionary<int, IList<int>> RoomToUsers = new ConcurrentDictionary<int, IList<int>>();

        public IList<int> GetRoomsByUserId(int userId)
        {
            if (UserToRooms.TryGetValue(userId, out var userRooms))
            {
                return userRooms;
            }
            return new List<int>();
        }

        public IList<int> GetUsersByRoomId(int roomId)
        {
            if (RoomToUsers.TryGetValue(roomId, out var roomUsers))
            {
                return roomUsers;
            }
            return new List<int>();
        }

        public void AddUserToRoom(int userId, int roomId)
        {
            UserToRooms.AddOrUpdate(userId, new List<int> { roomId }, (key, list) =>
            {
                if (!list.Contains(roomId))
                {
                    list.Add(roomId);
                }
                return list;
            });

            RoomToUsers.AddOrUpdate(roomId, new List<int> { userId }, (key, list) =>
            {
                if (!list.Contains(userId))
                {
                    list.Add(userId);
                }
                return list;
            });
        }

        public void RemoveUserFromRoom(int userId, int roomId)
        {
            if (UserToRooms.TryGetValue(userId, out var userRooms))
            {
                userRooms.Remove(roomId);
                if (userRooms.Count == 0)
                {
                    UserToRooms.TryRemove(userId, out _);
                }
            }

            if (RoomToUsers.TryGetValue(roomId, out var roomUsers))
            {
                roomUsers.Remove(userId);
                if (roomUsers.Count == 0)
                {
                    RoomToUsers.TryRemove(roomId, out _);
                }
            }
        }
    }
}