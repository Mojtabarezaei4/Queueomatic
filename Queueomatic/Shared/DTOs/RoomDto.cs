namespace Queueomatic.Shared.DTOs;

public class RoomDto
{
    public string HashIds { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpireAt { get; set; }
    public UserDto Owner { get; set; } = null!;
    public IEnumerable<ParticipantDto> Participators { get; set; } = new List<ParticipantDto>();
}