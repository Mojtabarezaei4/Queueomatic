using System.ComponentModel.DataAnnotations;

namespace Queueomatic.DataAccess.Models;

public class User
{
    [Key] 
    public string Email { get; set; }

    [MaxLength(20)]
    public string NickName { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public Role Role { get; set; }
    public virtual ICollection<Room> Rooms { get; set; }
}