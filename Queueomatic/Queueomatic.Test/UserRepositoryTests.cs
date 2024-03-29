﻿using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Queueomatic.DataAccess.DataContexts;
using Queueomatic.DataAccess.Models;
using Queueomatic.DataAccess.Repositories.Interfaces;
using Queueomatic.DataAccess.UnitOfWork;

namespace Queueomatic.Test;

public class UserRepositoryTests
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
        var participantRepository = A.Fake<IParticipantRepository>();
        A.CallTo(() => userRepository.GetAsync(userModel.Email)).Returns(userModel);

        //Act   
        await using var context = new ApplicationContext(options);
        var sut = new UnitOfWork(context, participantRepository, userRepository, roomRepository);
        var result = await sut.UserRepository.GetAsync(userModel.Email);

        //Assert
        Assert.Equivalent(userModel, result);
    }

    [Fact]
    public async Task GetAllUsers_ReturnTotalUsers()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "QueueomaticTestDatabase")
            .Options;

        var users = A.Fake<IEnumerable<User>>();
        var userRepository = A.Fake<IUserRepository>();
        var roomRepository = A.Fake<IRoomRepository>();
        var participantRepository = A.Fake<IParticipantRepository>();
        A.CallTo(() => userRepository.GetAllAsync()).Returns(users);

        //Act   
        await using var context = new ApplicationContext(options);
        var sut = new UnitOfWork(context, participantRepository, userRepository, roomRepository);
        var result = await sut.UserRepository.GetAllAsync();

        //Assert
        Assert.True(users.Count() == result.Count());
    }
}