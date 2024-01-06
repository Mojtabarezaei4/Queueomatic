namespace Queueomatic.Shared.DTOs;

public class RoomDto
{
    public string HashId { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpireAt { get; set; }
    public UserDto Owner { get; set; }
    public IEnumerable<ParticipantDto> Participators { get; set; } = new List<ParticipantDto>();
}