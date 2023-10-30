using Queueomatic.Shared.DTOs;
using Queueomatic.Shared.Models;

namespace Queueomatic.Server.Services.CacheRoomService
{
    public interface ICacheRoomService
    {
        public RoomModel GetRoom(string roomId);
        public void InitRoom(string roomId, string roomName, ParticipantRoomDto participant);

        public ParticipantRoomDto UpdateRoom(ParticipantRoomDto participant, StatusDto status, string roomId);
        public void CleanRoom(ParticipantRoomDto participantRoomDto, string roomId);
        public ParticipantRoomDto GetParticipant(Guid participantId, string roomId);
    }
}
