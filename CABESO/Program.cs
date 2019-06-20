using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using OpenPop.Mime;
using OpenPop.Pop3;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;

namespace CABESO
{
    /// <summary>
    /// Startklasse des Projekts
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Standardmäßige Fehlermeldung
        /// </summary>
        public const string ErrorMessage = "Gib bitte {0} an.";

        /// <summary>
        /// Pop-up-Meldung im Buffer
        /// </summary>
        public static string Alert { get; set; }

        /// <summary>
        /// Gibt an, ob sich ein gegebener Jahrgang im G9-System befindet.
        /// </summary>
        /// <param name="year">
        /// Der zu untersuchende Jahrgang
        /// </param>
        /// <returns>
        /// Gibt <c>true</c> zurück, falls der Jahrgang sich im G9-System befindet, sonst <c>false</c>.
        /// </returns>
        public static bool IsG9(int year) => year > 2018;

        /// <summary>
        /// Liefert die Pop-up-Meldung zurück und leert dabei den Buffer.
        /// </summary>
        /// <returns>
        /// Die Pop-up-Meldung
        /// </returns>
        public static string ShowAlert()
        {
            string alert = Alert;
            Alert = null;
            return alert;
        }

        /// <summary>
        /// Bezieht aus dem externen Programm "Greeting.exe" eine passende Begrüßung für die Startseite.
        /// </summary>
        /// <param name="name">
        /// Der Name des zu begrüßenden Benutzers
        /// </param>
        /// <returns>
        /// Die anzuzeigende Begrüßungsformel
        /// </returns>
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

        [DllImport("wininet.dll")] //Aus einer externen DLL importierte Funktionen
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        /// <summary>
        /// Gibt an, ob derzeit eine Verbindung zum Internet besteht.
        /// </summary>
        /// <returns>
        /// Gibt <c>true</c> zurück, wenn eine Verbindung aufgebautb werden kann, sonst <c>false</c>.
        /// </returns>
        public static bool HasInternetConnection() => InternetGetConnectedState(out _, 0);

        /// <summary>
        /// Überprüft, ob eine gegebene E-Mail-Adresse gültig ist, indem ex negativo auf eine Fehlermeldung vom Provider gewartet wird.
        /// </summary>
        /// <param name="mailAddress">
        /// Die zu überprüfende E-Mail-Adresse
        /// </param>
        /// <returns>
        /// Gibt <c>true</c> zurück, wenn die E-Mail-Adresse gültig ist, sonst <c>false</c>.
        /// </returns>
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

        /// <summary>
        /// Versendet eine E-Mail von einem statischen E-Mail-Konto an eine gegebene Adresse.
        /// </summary>
        /// <param name="mailAddress">
        /// Die E-Mail-Adresse des Empfängers
        /// </param>
        /// <param name="name">
        /// Der Name des Empfängers
        /// </param>
        /// <param name="subject">
        /// Der Inhalt der Betreffzeile der E-Mail
        /// </param>
        /// <param name="body">
        /// Der Textinhalt der E-Mail
        /// </param>
        /// <param name="links">
        /// Ggf. in der E-Mail verwandte Hyperlinks
        /// </param>
        /// <returns>
        /// Gibt <c>true</c> zurück, wenn die E-Mail erfolgreich versandt wurde, sonst <c>false</c>.
        /// </returns>
        public static bool SendMail(string mailAddress, string name, string subject, string body, params (string Url, string Text)[] links)
        {
            if (!HasInternetConnection())
                return true;

            SmtpClient client = new SmtpClient(Startup.MailSmtp)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(Startup.MailAddress, Startup.MailPassword),
                EnableSsl = true
            };

            string[] substitutions = Array.ConvertAll(links, link => $"<a href='{HtmlEncoder.Default.Encode(link.Url)}'>{link.Text}</a> ({HtmlEncoder.Default.Encode(link.Url)})");
            body = string.Format(body, substitutions);
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

        /// <summary>
        /// Startmethode; Baut alle Kontexte auf und startet die Weboberfläche.
        /// </summary>
        /// <param name="args">
        /// Befehlszeilenargumente für den Programmstart
        /// </param>
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