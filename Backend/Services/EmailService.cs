using System.Net;
using System.Net.Mail;
using Backend.Helpers;
using Backend.Interfaces;
using Microsoft.Extensions.Options;

namespace Backend.Services;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;

    public EmailService(IOptions<SmtpSettings> smtpOptions)
    {
        _smtpSettings = smtpOptions.Value;
    }

    public async Task SendOtpAsync(string email, string otp)
    {
        using MailMessage mail = new();

        mail.From = new MailAddress(_smtpSettings.FromEmail);
        mail.To.Add(email);

        mail.Subject = "Placement Portal OTP Verification";

        mail.Body =
            $"Your OTP is: {otp}\n\n" +
            $"It will expire in 6 minutes.";

        using SmtpClient smtpClient = new(
            _smtpSettings.Host,
            _smtpSettings.Port);

        smtpClient.Credentials = new NetworkCredential(
            _smtpSettings.Username,
            _smtpSettings.Password);

        smtpClient.EnableSsl = false;

        await smtpClient.SendMailAsync(mail);
    }
}