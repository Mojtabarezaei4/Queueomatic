using Microsoft.EntityFrameworkCore;
using Queueomatic.DataAccess.DataContexts;

namespace Queueomatic.Test;

public class UserRepository_ReturnCorrectValues
{
    [Fact]
    public async Task GetUser_ReturnUserOrNull()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "StoreDatabase")
            .Options;
        //Act

        //Assert
    }
}