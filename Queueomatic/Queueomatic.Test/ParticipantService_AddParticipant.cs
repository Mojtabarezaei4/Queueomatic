using FakeItEasy;
using Queueomatic.DataAccess.Repositories.Interfaces;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Server.Services.HashIdService;
using Queueomatic.Server.Services.ParticipantService;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Test;

public class ParticipantService_AddParticipant
{
    [Fact]
    public async Task AddParticipant_ReturnParticipantDto()
    {
        // Arrange
        var uof = A.Fake<IUnitOfWork>();
        var participantDto = A.Fake<ParticipantDto>();
        var hashidsService = A.Fake<IHashIdService>();
        var sut = new ParticipantService(uof, hashidsService);
        participantDto.StatusDate = DateTime.UtcNow;
        // Act
        var result = await sut.CreateOneAsync(participantDto, "satezx");

        // Assert
        Assert.IsType<ParticipantDto>(result);
    }
}