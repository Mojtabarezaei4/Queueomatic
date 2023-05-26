using FastEndpoints;
using Queueomatic.Server.Services.ParticipantService;

namespace Queueomatic.Server.Endpoints.Participant.Delete;

public class DeleteParticipantEndpoint: Endpoint<DeleteParticipantRequest>
{
    private readonly IParticipantService _participantService;

    public DeleteParticipantEndpoint(IParticipantService participantService)
    {
        _participantService = participantService;
    }

    public override void Configure()
    {
        Delete("/participants/{id}");
        Roles("Participant", "User");
    }

    public override async Task HandleAsync(DeleteParticipantRequest req, CancellationToken ct)
    {
        await _participantService.DeleteOneAsync(req.Id);
        await SendNoContentAsync();
    }
}