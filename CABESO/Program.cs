using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using OpenPop.Mime;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Threading;

namespace CABESO
{
    public class Program
    {
        public const string ErrorMessage = "Gib bitte {0} an.";

        public static Dictionary<string, string> Translations { get; private set; }

        public static string Alert { get; set; }

        public static string ShowAlert()
        {
            string alert = Alert;
            Alert = null;
            return alert;
        }

        public static string Greeting(Name name = null)
        {
            string greeting;
            switch (DateTime.Now.Hour)
            {
                case int hour when (hour < 5 || hour >= 22):
                    greeting = "Gute Nacht";
                    break;
                case int hour when (hour >= 5 && hour < 12):
                    greeting = "Guten Morgen";
                    break;
                case int hour when (hour >= 12 && hour < 18):
                    greeting = "Guten Nachmittag";
                    break;
                case int hour when (hour >= 18 && hour < 22):
                    greeting = "Guten Abend";
                    break;
                default:
                    greeting = "Hallo";
                    break;
            }
            return string.Format("{0}, {1}!", greeting, string.IsNullOrEmpty(name?.ToString().Trim()) ? "ihr Luschen" : name);
        }

        public static bool Matches(string entry, string search)
        {
            return !string.IsNullOrEmpty(entry) && !string.IsNullOrEmpty(search) && entry.Contains(search, StringComparison.OrdinalIgnoreCase);
        }

        public static string TimeDisplay(DateTime dt)
        {
            return dt.ToLocalTime().ToString(CultureInfo.CurrentCulture);
        }

        public static bool MailValid(string mailAddress)
        {
            Pop3Client client = new Pop3Client();
            client.Connect(Startup.MailPop3, 995, true);
            client.Authenticate(Startup.MailAddress, Startup.MailPassword);

            DateTime start = DateTime.Now;
            while (DateTime.Now - start < new TimeSpan(0, 0, 10))
            {
                if (!Verify())
                    return false;
                Thread.Sleep(100);
            }
            return true;

            bool Verify()
            {
                for (int i = client.GetMessageCount(); i >= 0; i--)
                {
                    Message message = client.GetMessage(i);
                    if (DateTime.Now - DateTime.Parse(message.Headers.Date) > new TimeSpan(0, 5, 0))
                        return true;
                    if (!message.Headers.From.MailAddress.Address.Equals("keineantwortadresse@web.de"))
                        continue;
                    if (!message.Headers.Subject.Equals("Mail delivery failed: returning message to sender"))
                        continue;
                    if (message.MessagePart.GetBodyAsText().Contains("To: " + mailAddress))
                        return false;
                }
                return true;
            }
        }

        public static bool SendMail(string mailAddress, string subject, string body, string name = null)
        {
            SmtpClient client = new SmtpClient(Startup.MailSmtp)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(Startup.MailAddress, Startup.MailPassword),
                EnableSsl = true
            };
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(Startup.MailAddress),
                IsBodyHtml = true,
                Subject = $"CABESO | {subject}",
                Body = $"Hallo {name ?? mailAddress}!<br /><br />{body}<br /><br /><br />Viele Grüße<br /><br />Das CABESO-Team des GNR"
            };
            mailMessage.To.Add(mailAddress);

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (SmtpFailedRecipientException)
            {
                return false;
            }
        }

        public static void Main(string[] args)
        {
            Translations = new Dictionary<string, string>
            {
                { "Student", "Schüler*in" },
                { "Teacher", "Lehrer*in" },
                { "Employee", "Mitarbeiter*in" },
                { "True", "ja" },
                { "False", "nein" }
            };

            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build()
                .MigrateDatabase()
                .Run();
        }
    }
}