namespace Queueomatic.Shared.DTOs;

public class UserDto
{
    public string Email { get; set; }
    public string NickName { get; set; }
    public IEnumerable<RoomDto> Rooms { get; set; } = new List<RoomDto>();
}