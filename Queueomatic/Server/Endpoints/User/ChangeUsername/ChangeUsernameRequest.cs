using FastEndpoints;

namespace Queueomatic.Server.Endpoints.User.ChangeUsername;

public class ChangeUsernameRequest
{
    [FromBody]
    public string Username { get; set; }
    [property: FromClaim]
    public string UserId { get; set; }
}