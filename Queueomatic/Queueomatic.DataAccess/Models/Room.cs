using System.ComponentModel.DataAnnotations;

namespace Queueomatic.DataAccess.Models;

public class Room
{
    public int Id { get; set; }

    [MaxLength(20)]
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpireAt { get; set; } = DateTime.UtcNow.AddHours(12);
    public virtual User Owner { get; set; } //Could be a list for multiple owners
    public virtual ICollection<Participant> Participators { get; set; }
}