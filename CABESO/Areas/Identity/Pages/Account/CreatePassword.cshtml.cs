﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CABESO.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class CreatePasswordModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager; //Der Manager der Anmeldeverwaltung
        private readonly UserManager<IdentityUser> _userManager; //Der Manager der Benutzerverwaltung
        private static string _userId;

        public CreatePasswordModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        /// <summary>
		/// Ein Hilfsobjekt, das die Eingabeinformationen der Weboberfläche zwischenspeichert.
		/// </summary>
		[BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
		/// Eine Datenstruktur zur Zwischenspeicherung der Eingabeinformationen
		/// </summary>
		public class InputModel
        {
            [Required]
            [StringLength(100, ErrorMessage = "Das {0} muss mindestens {2} und höchstens {1} Zeichen lang sein.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "neue Passwort")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "Die Passwörter stimmen nicht überein.")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
            public string UserId { get; set; }
        }

        public async Task<IActionResult> OnGet(string userId = null, string code = null)
        {
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(userId) || await _userManager.FindByIdAsync(_userId = userId) == null)
                return RedirectToPage("./AccessDenied");
            else
            {
                Input = new InputModel
                {
                    Code = code,
                    UserId = userId
                };
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            IdentityUser user = await _userManager.FindByIdAsync(_userId);
            if (user == null)
                return Page();

            if (_signInManager.IsSignedIn(User))
                await _signInManager.SignOutAsync();

            IdentityResult result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return LocalRedirect("~/");
            }
            else
                return RedirectToPage("./AccessDenied");
        }
    }
}