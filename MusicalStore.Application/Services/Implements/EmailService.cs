using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Common.ResponseBase;

namespace MusicalStore.Application.Services.Implements
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public MessageEmail ChangeToMessageEmail(string To, string Subject, string Body)
        {
            var message = new MessageEmail();
            message.To = To;
            message.Subject = Subject;
            message.Body = Body;
            return message;
        }

        public async Task<ResponseMessage> SendEmail(MessageEmail request)
        {
            var response = new ResponseMessage();
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailConfiguration")["EmailUsername"]));
                email.To.Add(MailboxAddress.Parse(request.To));
                email.Subject = request.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_config.GetSection("EmailConfiguration")["EmailHost"], 465, true);

                await smtp.AuthenticateAsync(_config.GetSection("EmailConfiguration")["EmailUsername"], _config.GetSection("EmailConfiguration")["EmailPassword"]);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                response.StatusCode = 200;
                response.Message = "Send Email Success";
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }
        }

    }
}
