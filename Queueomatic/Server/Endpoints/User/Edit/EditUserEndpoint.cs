using FastEndpoints;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Endpoints.User.Edit;

public class EditUserEndpoint: Endpoint<EditUserRequest, EditUserResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public EditUserEndpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public override void Configure()
    {
        Put("/users/{email}/update/profile");
        Policies("SignedInUser");
    }

    public override async Task HandleAsync(EditUserRequest req, CancellationToken ct)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(req.Email);
        if (user is not null)
        {
            user.Email = req.User.Email;
            user.NickName = req.User.NickName;
        }

        await _unitOfWork.UserRepository.UpdateAsync(user);
        
        var result = await _unitOfWork.SaveAsync();
        
        if (result == 0)
        {
            await SendErrorsAsync();
            return;
        }

        await SendAsync(new(new UserDto
        {
            Email = user.Email,
            NickName = user.NickName
        }));
    }
}