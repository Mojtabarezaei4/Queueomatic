using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Endpoints.User.GetByEmail;

public record GetUserByEmailResponse(UserDto User);