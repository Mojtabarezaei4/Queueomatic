using FastEndpoints;

namespace Queueomatic.Server.Endpoints.User.GetByEmail;

public record GetUserByEmailRequest(string Email, [property: FromClaim] string UserId);