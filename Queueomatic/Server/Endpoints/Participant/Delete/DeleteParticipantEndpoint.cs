using FastEndpoints;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Server.Services.ParticipantService;

namespace Queueomatic.Server.Endpoints.Participant.Delete;

public class DeleteParticipantEndpoint: Endpoint<DeleteParticipantRequest>
{
    private readonly IParticipantService _participantService;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteParticipantEndpoint(IParticipantService participantService, IUnitOfWork unitOfWork)
    {
        _participantService = participantService;
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Delete("/participants/{id}");
        Roles("Participant", "User");
    }

    public override async Task HandleAsync(DeleteParticipantRequest req, CancellationToken ct)
    {
        if (req.ParticipantId != null && 
            req.ParticipantId != req.Id)
        {
            await SendUnauthorizedAsync();
            return;
        }

        if (!string.IsNullOrEmpty(req.UserId) && 
            !await IsUserOwnerOfTheRoom(req))
        {
            await SendUnauthorizedAsync();
            return;
        }
        
        await _participantService.DeleteOneAsync(req.Id);
        await SendNoContentAsync();
    }

    private async Task<bool> IsUserOwnerOfTheRoom(DeleteParticipantRequest req)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(req.UserId);
        if (user == null) return false;

        var userRooms = user.Rooms.ToList();
        
        var isUserTheOwner =
            userRooms.FirstOrDefault(r =>
                r.Participators.FirstOrDefault(p =>
                    p.Id == req.Id) != null);

        if (isUserTheOwner == null) return false;

        return true;
    }
}