using BusinessLogic.Models.Email;
using FitJourney_BackEnd.Interface;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace FitJourney_BackEnd.Repositories;
//Não funcional dá erro ao enviar o email
public class EmailRepository : IEmailRepository
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _webHostEnvironment;
    
    public EmailRepository(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        _configuration = configuration;
        _webHostEnvironment = webHostEnvironment;
    }
    public async Task SendWelcomeEmail(SendEmailModel emailRequest)
    {
        string smtpServer;
        int smtpPort;
       /* 
       if (emailRequest.To.EndsWith("@gmail.com"))
       {
           smtpServer = "smtp.gmail.com";
           smtpPort = 587;
       }
       else if (emailRequest.To.EndsWith("@outlook.com") || emailRequest.To.EndsWith("@hotmail.com"))
       {
           smtpServer = "smtp.office365.com";
           smtpPort = 587;
       }
       else
       {
           throw new ArgumentException("Impossible to send the email!");
       }
       */
       var email = new MimeMessage();
       email.From.Add(MailboxAddress.Parse(_configuration.GetSection("Email:SmtpUsername").Value));
       email.To.Add(MailboxAddress.Parse(emailRequest.To));
       email.Subject = "Bem-Vindo à família FitJourney!";
       
       var htmlFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "EmailTemplates", "WelcomeEmail.html");
       var htmlBody = await File.ReadAllTextAsync(htmlFilePath);
       email.Body = new TextPart(TextFormat.Html)
       {
           Text = htmlBody
       };

       using var client = new SmtpClient();
       client.Connect(_configuration.GetSection("Email:SmtpServer").Value,int.Parse(_configuration.GetSection("Email:SmtpPort").Value),SecureSocketOptions.StartTls);
       client.Authenticate(_configuration.GetSection("Email:SmtpUsername").Value, _configuration.GetSection("Email:SmtpPassword").Value);
       client.Send(email);
       client.Disconnect(true);
       
    }
}