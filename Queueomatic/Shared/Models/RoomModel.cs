using Queueomatic.Shared.DTOs;

namespace Queueomatic.Shared.Models
{
    public class RoomModel
    {
        public string RoomId { get; set; }
        public string RoomName { get; set; }
        public List<ParticipantRoomDto> IdlingParticipants { get; set; }
        public List<ParticipantRoomDto> WaitingParticipants { get; set; }
        public List<ParticipantRoomDto> ActiveParticipants { get; set; }
    }
}
