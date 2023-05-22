using FastEndpoints;

namespace Queueomatic.Server.Endpoints.User.Delete;

public record DeleteUserRequest([property: FromClaim] string UserId, string Email);
