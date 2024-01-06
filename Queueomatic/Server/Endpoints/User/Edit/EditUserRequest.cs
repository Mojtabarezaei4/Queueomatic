using FastEndpoints;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Endpoints.User.Edit;

public record EditUserRequest([property: FromClaim]string UserId, UserDto User, string Email);
