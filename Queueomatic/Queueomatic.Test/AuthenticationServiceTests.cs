using FakeItEasy;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Server.Services.AuthenticationService;

namespace Queueomatic.Test;

public class AuthenticationServiceTests
{
	[Fact]
	public void CreateSequence_ShouldBeEqual()
	{
		//Arrange
		var password = "password";
		var unitOfWork = A.Fake<IUnitOfWork>();

		//Act
		var sut = new AuthenticationService(unitOfWork);
		sut.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
		

		//Assert
		Assert.True(sut.VerifyPasswordHash(password, passwordHash, passwordSalt));
	}

	[Theory]
	[InlineData("")]
	[InlineData("\"")]
	[InlineData("12345")]
	[InlineData("2263c33d-9fd2-4fbe-99c7-0ac50694a704")]
	[InlineData("12f6e34f-090c-4c1d-94c2-ce505396cf3f446f6304-766f-42be-98eb-f27d4fdaa61e369ca841-8dff-4cdd-9d6e-274d42758af3c1472881-bb26-49b6-8532-547d7af6c83a")]
	[InlineData("@'*?!.,-_<(={$%&~^|`+>)}")]
	[InlineData("ÅÄÖåäö")]
	public void CreateSequence_ShouldBeNotEqual(string passwordToBeTested)
	{
		//Arrange
		var password = passwordToBeTested;
		var unitOfWork = A.Fake<IUnitOfWork>();

		//Act
		var sut = new AuthenticationService(unitOfWork);
		sut.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

		//Assert
		Assert.True(sut.VerifyPasswordHash(password, passwordHash, passwordSalt));
	}

}