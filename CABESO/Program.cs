using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using OpenPop.Mime;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace CABESO
{
    public class Program
    {
        public const string ErrorMessage = "Gib bitte {0} an.";

        public static string Alert { get; set; }

        public static bool IsG9(int year) => year > 2018;

        public static string ShowAlert()
        {
            string alert = Alert;
            Alert = null;
            return alert;
        }

        public static string Greeting(Name name = null)
        {
            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = Path.Combine(Environment.CurrentDirectory, "Greeting.exe"),
                    Arguments = $"\"{name}\"",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardOutput = true,
                    StandardOutputEncoding = Encoding.Unicode
                }
            };
            proc.Start();
            string greeting;
            using (StreamReader output = proc.StandardOutput)
                greeting = output.ReadToEnd();
            return greeting;
        }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        public static bool HasInternetConnection() => InternetGetConnectedState(out _, 0);

        public static bool MailValid(string mailAddress)
        {
            if (!HasInternetConnection())
                return true;

            Pop3Client client = new Pop3Client();
            client.Connect(Startup.MailPop3, 995, true);
            client.Authenticate(Startup.MailAddress, Startup.MailPassword);

            DateTime start = DateTime.Now;
            while (DateTime.Now - start < new TimeSpan(0, 0, 15))
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
                    if (!message.Headers.From.MailAddress.Address.Equals(Startup.MailReturn))
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
            if (!HasInternetConnection())
                return true;

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
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build()
                .MigrateDatabase(out Startup.Context)
                .Run();
        }
    }
}