using FastEndpoints;

namespace Queueomatic.Server.Endpoints.User.Delete;

public record DeleteUserRequest
{
    [FromClaim]
    public string Email { get; set; }
};