namespace Queueomatic.Shared.DTOs;

public class ParticipantDto
{
    public Guid Id { get; set; }
    public string NickName { get; set; }
    public DateTime StatusDate { get; set; }
    public StatusDto Status { get; set; }
    public RoomDto Room { get; set; }
}