using BusinessLogic.Models.Email;

namespace FitJourney_BackEnd.Interface;

public interface IEmailRepository
{
    Task SendWelcomeEmail(SendEmailModel emailRequest);
}