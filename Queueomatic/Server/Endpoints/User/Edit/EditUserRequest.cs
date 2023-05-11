using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Endpoints.User.Edit;

public record EditUserRequest(string Email, UserDto User);