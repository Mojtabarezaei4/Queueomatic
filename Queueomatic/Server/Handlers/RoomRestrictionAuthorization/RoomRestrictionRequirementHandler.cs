using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Server.Services.HashIdService;

namespace Queueomatic.Server.Handlers.RoomRestrictionAuthorization;

public class RoomRestrictionRequirementHandler : AuthorizationHandler<RoomRestrictionRequirement, HubInvocationContext>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHashIdService _hashIdService;
    public RoomRestrictionRequirementHandler(IUnitOfWork unitOfWork, IHashIdService hashIdService)
    {
        _unitOfWork = unitOfWork;
        _hashIdService = hashIdService;
    }
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoomRestrictionRequirement requirement,
        HubInvocationContext resource)
    {
        if (context.User.IsInRole("Administrator"))
        {
            context.Succeed(requirement);
            return;
        };

        var userId = context.User.Claims.FirstOrDefault(c => c.Type.Equals("UserId"));

        if (userId == null)
        {
            context.Fail();
            return;
        }

        var roomId = resource.HubMethodArguments[0]!.ToString(); //This gets the room id

        var room = await _unitOfWork.RoomRepository.GetAsync(_hashIdService.Decode(roomId!));

        if (room!.Owner.Email.Equals(userId.Value))
        {
            context.Succeed(requirement);
            return;
        }

        context.Fail();
    }
}