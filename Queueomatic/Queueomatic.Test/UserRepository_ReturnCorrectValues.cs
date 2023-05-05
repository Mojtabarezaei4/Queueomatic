using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Queueomatic.DataAccess.DataContexts;
using Queueomatic.DataAccess.Models;
using Queueomatic.DataAccess.Repositories.Interfaces;
using Queueomatic.DataAccess.UnitOfWork;

namespace Queueomatic.Test;

public class UserRepository_ReturnCorrectValues
{
    [Fact]
    public async Task GetUser_ReturnUserOrNull()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "QueueomaticTestDatabase")
            .Options;

        var userModel = A.Fake<User>();
        var userRepository = A.Fake<IUserRepository>();
        var roomRepository = A.Fake<IRoomRepository>();
        A.CallTo(() => userRepository.GetAsync(userModel.Email)).Returns(userModel);

        //Act   
        await using var context = new ApplicationContext(options);
        var sut = new UnitOfWork(context, userRepository, roomRepository);
        var result = await sut.UserRepository.GetAsync(userModel.Email);

        //Assert
        Assert.Equivalent(userModel, result);
    }
}