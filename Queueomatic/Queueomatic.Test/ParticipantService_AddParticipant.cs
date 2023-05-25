using FakeItEasy;
using Queueomatic.DataAccess.Repositories.Interfaces;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Server.Services.HashIdService;
using Queueomatic.Server.Services.ParticipantService;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Test;

public class ParticipantService_AddParticipant
{
    [Theory]
    [InlineData(1)]
    [InlineData(25)]
    [InlineData(623)]
    [InlineData(6185)]
    [InlineData(1231)]
    [InlineData(1615)]
    [InlineData(3241)]
    [InlineData(324)]
    [InlineData(74)]
    public async Task AddParticipant_ReturnParticipantDto(int roomId)
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var participantDto = A.Fake<ParticipantDto>();
        var hashidsService = A.Fake<IHashIdService>();
        var encodedRoomId = hashidsService.Encode(roomId);
        var sut = new ParticipantService(unitOfWork, hashidsService);
        participantDto.StatusDate = DateTime.UtcNow;
        // Act
        var result = await sut.CreateOneAsync(participantDto, encodedRoomId);

        // Assert
        Assert.IsType<ParticipantDto>(result);
    }
    
    [Theory]
    [InlineData(StatusDto.Idling)]
    [InlineData(StatusDto.Waiting)]
    [InlineData(StatusDto.Ongoing)]
    public async Task UpdateParticipant_ReturnTrue(StatusDto statusDto)
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var participantDto = A.Fake<ParticipantDto>();
        participantDto.Status = statusDto;
        var hashidsService = A.Fake<IHashIdService>();
        var guid = new Guid();
        var sut = new ParticipantService(unitOfWork, hashidsService);
        // Act
        var result = await sut.UpdateOneAsync(participantDto,guid);

        // Assert
        Assert.True(result);
    }
}