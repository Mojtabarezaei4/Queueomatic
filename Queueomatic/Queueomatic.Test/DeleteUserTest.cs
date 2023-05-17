using FakeItEasy;
using Microsoft.AspNetCore.Http.HttpResults;
using Queueomatic.DataAccess.Models;
using Queueomatic.DataAccess.Repositories.Interfaces;
using Queueomatic.DataAccess.Repositories;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Server.Endpoints.User.Delete;

namespace Queueomatic.Test;

public class DeleteUserTest
{
    [Fact]
    public async Task DeleteUserEndpoint_ShouldReturnOk()
    {
        //Arrange
        var user = A.Dummy<User>();
        string email = user.Email;

        var userRepository = A.Fake<IUserRepository>();
        A.CallTo(() => userRepository.DeleteAsync(email)).DoesNothing();

        var request = A.Fake<DeleteUserRequest>();
        var unitOfWork = A.Fake<IUnitOfWork>();
        var sut = new DeleteUserEndpoint(unitOfWork);
        var ct = new CancellationToken();

        //Act
        //var result = await sut.HandleAsync(request, ct);

        //Assert
        //Assert.IsType<Ok>(result);
    }

}