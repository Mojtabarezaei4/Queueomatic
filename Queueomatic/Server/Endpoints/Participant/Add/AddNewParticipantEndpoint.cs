using System.Security.Claims;
using FastEndpoints;
using FastEndpoints.Security;
using Queueomatic.DataAccess.Models;
using Queueomatic.Server.Services.ParticipantService;

namespace Queueomatic.Server.Endpoints.Participant.Add;

public class AddNewParticipantEndpoint: Endpoint<AddNewParticipantRequest>
{
    private readonly IConfiguration _configuration;
    private readonly IParticipantService _participantService;

    public AddNewParticipantEndpoint(IConfiguration configuration, IParticipantService participantService)
    {
        _configuration = configuration;
        _participantService = participantService;
    }

    public override void Configure()
    {
        Post("/rooms/{roomId}/newParticipant");
        Description(builder => builder.WithName("AddNewParticipant"));
        AllowAnonymous();
    }

    public override async Task HandleAsync(AddNewParticipantRequest req, CancellationToken ct)
    {
        var participant = await _participantService.CreateOneAsync(req.Participant, req.RoomId);
        if (participant == null)
        {
            await SendErrorsAsync();
            return;
        }
        
        var jwtToken = JWTBearer.CreateToken(
            signingKey: _configuration.GetSection("JWTSigningKeys").GetSection("DefaultKey").Value!,
            expireAt: DateTime.UtcNow.AddDays(1),
            priviledges: u =>
            {
                u.Roles.Add(Role.Participant.ToString());
                u.Claims.Add(new Claim("ParticipantId", participant.Id.ToString()));
            });
        
        await SendCreatedAtAsync<AddNewParticipantEndpoint>("AddNewParticipant",new
        {
            Token = jwtToken,
            ParticipantName = participant.NickName
        });
    }
}