using System.ComponentModel.DataAnnotations;

namespace Queueomatic.DataAccess.Models;

public class Participant
{
    public Guid Id { get; set; }

    [MaxLength(20)]
    public string NickName { get; set; }
    public DateTime StatusDate { get; set; } = DateTime.UtcNow;
    public Status Status { get; set; } = Status.Idling;
    public virtual Room Room { get; set; }
}