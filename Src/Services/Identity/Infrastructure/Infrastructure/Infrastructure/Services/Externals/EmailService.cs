﻿using Application.ApplicationServices;
using Domain.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Services.Externals;

public class EmailService(IOptions<EmailSettings> options) : IEmailService
{
    private readonly EmailSettings _emailSettings = options.Value;

    public bool CheckEmail(string Email)
    {

        var trimmedEmail = Email.Trim();

    
        try
        {
            var addr = new MailAddress(trimmedEmail);
      
            return true;
        }
        catch
        {
            return false;
        }




    }

    public async Task SendAsync(string email, string subject, string content, CancellationToken cancellationToken)
    {
        using var smtpClient = new SmtpClient(_emailSettings.SmtpHost, _emailSettings.SmtpPort)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(_emailSettings.UserName, _emailSettings.Password)
        };

        var mailMessage = new MailMessage
        {
            SubjectEncoding = System.Text.Encoding.UTF8,
            BodyEncoding = System.Text.Encoding.UTF8,
            IsBodyHtml = true,
            From = new(_emailSettings.From),
            Subject = subject,
            Body = content
        };

        mailMessage.To.Add(email);

        await smtpClient.SendMailAsync(mailMessage, cancellationToken);
    }
}
