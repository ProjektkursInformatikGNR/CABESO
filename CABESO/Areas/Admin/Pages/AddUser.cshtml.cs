using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace CABESO.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class AddUserModel : PageModel
    {
        public readonly object[][] Forms;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<IndexModel> _logger;
        public string ReturnUrl { get; set; }

        public AddUserModel(UserManager<IdentityUser> userManager, ILogger<IndexModel> logger, string returnUrl = null)
        {
            _userManager = userManager;
            _logger = logger;
            ReturnUrl = returnUrl ?? ReturnUrl;
            Forms = Database.SqlQuery("Forms", null, "Name", "Id");
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Gib bitte die E-Mail-Adresse an.")]
            [EmailAddress(ErrorMessage = "Gib bitte eine gültige E-Mail-Adresse an.")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            public string Role { get; set; }

            public string Form { get; set; }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                byte[] password = new byte[32];
                RandomNumberGenerator.Fill(password);
                string pwd = Convert.ToBase64String(password);
                var result = await _userManager.CreateAsync(user, pwd);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created with random password.");
                    await _userManager.AddToRoleAsync(user, Input.Role);
                    if (Input.Role.Equals("Student"))
                        Database.SqlExecute($"UPDATE [dbo].[AspNetUsers] SET [Form]='{Input.Form}' WHERE [Id]='{user.Id}';");

                    await _userManager.ConfirmEmailAsync(user, await _userManager.GenerateEmailConfirmationTokenAsync(user));

                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    //var callbackUrl = Url.Page(
                    //    "~/Identity/Account/CreatePassword",
                    //    pageHandler: null,
                    //    values: new { userId = user.Id, code = code },
                    //    protocol: Request.Scheme);
                    string callbackUrl = $"https://{HttpContext.Request.Host}/Identity/Account/CreatePassword?userId={user.Id}&code={code.Replace("+", "%2B")}";

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
                        Body = $"Hallo {new Name(user.Email, Input.Role.Equals("Student")).ToString()}!<br /><br />Du wurdest zur Online-Wahl am GNR berechtigt. Klicke <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>hier</a> zur Erstanmeldung.<br /><br /><br />Viele Grüße<br /><br />Das Online-Wahl-Team des GNR",
                        Subject = "Anmeldung zur GNR-Online-Wahl"
                    };
                    mailMessage.To.Add(Input.Email);
                    client.Send(mailMessage);

                    return RedirectToPage();
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}