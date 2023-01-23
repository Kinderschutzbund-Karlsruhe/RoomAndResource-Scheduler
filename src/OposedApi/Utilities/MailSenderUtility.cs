﻿using OposedApi.Enum;
using OposedApi.MailType;
using OposedApi.Models;
using System.Net;
using System.Net.Mail;

namespace OposedApi.Utilities
{
    internal class MailSenderUtility
    {
        private static SmtpClient? _smtpClient = null;
        static readonly object lockname = new object();

        private static void Init()
        {
            var host = Settings.SmtpServerHost;
            var port = Convert.ToInt32(Settings.SmtpServerPort);
            var mail = Settings.SmtpServerMailAddress;
            var password = Settings.SmtpServerMailPassword;
            var ignoreInvalidCertificate = Convert.ToBoolean(Settings.SmtpServerIgnoreInvalidCertificate);
            var ssl = Convert.ToBoolean(Settings.SmtpServerIsSsl);

            if (host == null)
                return;

            _smtpClient = new SmtpClient(host)
            {
                Port = port,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(mail, password),
                EnableSsl = ssl,
            };
            if (ignoreInvalidCertificate)
            {
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            }
        }

        internal static async void Send(User to, MailTypBase type)
        {
            if (_smtpClient == null)
                Init();
            if (_smtpClient == null)
                return;

            await Task.Run(() => {
                lock (lockname)
                {
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(Settings.SmtpServerMailAddress),
                        Subject = type.GetSubject(to),
                        Body = type.GetHtmlContent(to),
                        IsBodyHtml = true,
                    };
                    mailMessage.To.Add(to.Mail);

                    _smtpClient.Send(mailMessage);
                }
            });

           
        }
    }
}
