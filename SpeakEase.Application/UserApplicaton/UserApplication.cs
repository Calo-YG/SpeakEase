using SpeakEase.Application.Contracts;
using SpeakEase.Application.Contracts.Users;
using SpeakEase.Domain;

namespace SpeakEase.Application.UserApplicaton;

public class UserApplication(IServiceProvider serviceProvider, SpeakEaseContext speakeaeContext) :
    ApplicationBase<SpeakEaseContext>(serviceProvider, speakeaeContext), IUserApplication
{
}