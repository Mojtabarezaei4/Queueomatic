using FastEndpoints;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Endpoints.User.ChangeUsername;

public class ChangeUsernameRequest
{
    public UserDto User { get; set; }
    [property: FromClaim]
    public string UserId { get; set; }
}