# Queueomatic Planning

## Aim of the project

To develop a queue system that can be used for educational purposes or for any other situations where prioritization of individuals is necessary.

## NOTE

- All Ids which goes out of the server/application has to be encoded in [**hashids**](LINKS).
- All Ids which comes into the server/application has to be decoded in [**hashids**](LINKS).
- Relevant tests should be written when adding new functionalities.

### Database

- SQL as the choice of Database
- Data models

  ```csharp
  public class User{
    [Id]
    [Email]
    public string Email { get; set; }

    [MaxLength(20)]
    public string NickName { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public virtual ICollection<Room> Rooms { get; set; }
  }
  ```

  ```csharp
  public class Room{
    public Guid Id { get; set; }

    [MaxLength(20)]
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpireAt { get; set; } = DateTime.UtcNow.AddHours(12);
    public virtual User Owner { get; set; }
    public virtual ICollection<Participant> Participators { get; set; }
  }
  ```

  ```csharp
  public class Paricipant{
    public Guid Id { get; set; }

    [MaxLength(20)]
    public string NickName { get; set; }
    public DateTime StatusDate { get; set; } = DateTime.UtcNow;
    public Status Status { get; set; } = Status.Idling;
    public virtual Room Room { get; set; }
  }
  ```

  ```csharp
  public enum Status{
    Idling,
    Waiting,
    Ongoing,
  }
  ```

### Example

<hr>
<h3 style="text-align: center;">
🌠 Getting help <br/>
🌍 Earth
 </h3>
<hr>
<table style="text-align: center; margin: auto;">
  <tr>
    <th>Waiting</th>
    <th>Needs help</th>
  </tr>
  <tr>
    <td>🪐 Jupiter</td>
    <td>🌞 Sun</td>
  </tr>
</table>

<br>
<br>

### Design patterns and design principles

- Achiving clean architecture by:
- Interface segregation principle
- Single responsibility principle
- Open-closed principle
- Common closure principle
- Dependency Inversion principle
- REPR Design Pattern (Request-Endpoint-Response)
  - We use FastEndpoints to achive this pattern.
- Unit of Work with Repository Pattern

## LINKS

- Id for users/non server business:

  - [HashIds Doc](https://github.com/ullmark/hashids.net)
  - [Nick Chapsas](https://www.youtube.com/watch?v=tSuwe7FowzE&ab_channel=NickChapsas)

- FastEndpoints creation
  - [Doc](https://fast-endpoints.com/)
