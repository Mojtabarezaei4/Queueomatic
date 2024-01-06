using FastEndpoints;
using Queueomatic.Server.Services.ParticipantService;

namespace Queueomatic.Server.Endpoints.Participant.Edit;

public class EditParticipantEndpoint: Endpoint<EditParticipantRequest>
{
    private readonly IParticipantService _participantService;

    public EditParticipantEndpoint(IParticipantService participantService)
    {
        _participantService = participantService;
    }

    public override void Configure()
    {
        Put("/participants/{id}");
        Roles("Participant", "User");
    }

    public override async Task HandleAsync(EditParticipantRequest req, CancellationToken ct)
    {
        var updatedParticipant = await _participantService.UpdateOneAsync(req.Participant, req.Id);

        if (updatedParticipant == false)
        {
            await SendErrorsAsync();
            return;
        }

        await SendNoContentAsync();
    }
}