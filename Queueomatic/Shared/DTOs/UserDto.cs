namespace Queueomatic.Shared.DTOs;

public class UserDto
{
    public string Email { get; set; } = null!;
    public string NickName { get; set; } = null!;
    public IEnumerable<RoomDto> Rooms { get; set; } = new List<RoomDto>();
}