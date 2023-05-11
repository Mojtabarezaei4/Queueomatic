using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Endpoints.SignUp;

public record SignUpRequest(UserDto User);