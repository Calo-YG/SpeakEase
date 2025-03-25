using SpeakEase.Application.Contracts.Users;
using SpeakEase.Domain;
using SpeakEase.Infrastructure.EntityFrameworkCore;

namespace SpeakEase.Application.UserApplicaton;

public class UserApplication(IServiceProvider serviceProvider, SpeakEaseContext speakeaeContext) :
    ApplicationBase<SpeakEaseContext>(serviceProvider, speakeaeContext), IUserApplication
{

}