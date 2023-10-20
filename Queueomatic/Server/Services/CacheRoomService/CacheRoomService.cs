using Microsoft.Extensions.Caching.Memory;
using Queueomatic.Client.Pages;
using Queueomatic.DataAccess.Models;
using Queueomatic.Shared.DTOs;
using Queueomatic.Shared.Models;

namespace Queueomatic.Server.Services.CacheRoomService
{
    public class CacheRoomService : ICacheRoomService
    {
        private readonly IMemoryCache _cache;
        public CacheRoomService(IMemoryCache cache)
        {
            _cache = cache ?? throw new ArgumentException($"The value of cache cannot be null");
        }

        public RoomModel GetRoom(string roomId) => (RoomModel)_cache.Get(roomId);
        

        public bool InitRoom(string roomId, string roomName, ParticipantRoomDto participant)
        {
            if (!_cache.TryGetValue(roomId, out _))
            {
                var room = new RoomModel
                {
                    RoomId = roomId,
                    RoomName = roomName,
                    IdlingParticipants = new(),
                    WaitingParticipants = new(),
                    ActiveParticipants = new()
                };

                participant.StatusDate = DateTime.UtcNow;
                room!.IdlingParticipants.Add(participant);
                _cache.Set(roomId, room);

                return true;
            }
            return false;
        }

        public ParticipantRoomDto UpdateRoom(ParticipantRoomDto participant, StatusDto status, string roomId)
        {
            participant.StatusDate = DateTime.UtcNow;
            var participantToBeUpdated = new ParticipantRoomDto
            {
                Id = participant.Id,
                Status = participant.Status,
                NickName = participant.NickName,
                Room = participant.Room,
                StatusDate = participant.StatusDate
            };

            var room = _cache.Get<RoomModel>(roomId);

            var oldList = GetList(participant.Status, room);
            oldList.RemoveAll(p => p.Id == participant.Id);

            var activeList = GetList(status, room);
            participant.Status = status;
            activeList.Add(participant);

            _cache.Set(roomId, room);

            return participantToBeUpdated;
        }

        public void CleanRoom(ParticipantRoomDto participantRoomDto, string roomId)
        {
            var room = _cache.Get<RoomModel>(roomId);
            room.IdlingParticipants.RemoveAll(x => x.Id == participantRoomDto.Id);
            room.WaitingParticipants.RemoveAll(x => x.Id == participantRoomDto.Id);
            room.ActiveParticipants.RemoveAll(x => x.Id == participantRoomDto.Id);

            _cache.Set(roomId, room);
        }

        private List<ParticipantRoomDto> GetList(StatusDto participantStatus, RoomModel room)
        {
            return participantStatus switch
            {
                StatusDto.Idling => room.IdlingParticipants,
                StatusDto.Waiting => room.WaitingParticipants,
                StatusDto.Ongoing => room.ActiveParticipants,
                _ => throw new ArgumentOutOfRangeException(nameof(participantStatus), participantStatus, null)
            };
        }
    }
}
