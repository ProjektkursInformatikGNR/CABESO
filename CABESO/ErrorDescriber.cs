using Microsoft.AspNetCore.Identity;

namespace CABESO
{
    /// <summary>
    /// Übersetzungen der normierten Fehlermeldungen
    /// </summary>
    public class ErrorDescriber : IdentityErrorDescriber
    {
        /// <summary>
        /// Unspezifischer Fehler
        /// </summary>
        /// <returns>
        /// Die übersetzte Fehlermeldung
        /// </returns>
        public override IdentityError DefaultError() { return new IdentityError { Code = nameof(DefaultError), Description = "Ein unbekannter Fehler ist aufgetreten." }; }

        /// <summary>
        /// Parallelitätsfehler
        /// </summary>
        /// <returns>
        /// Die übersetzte Fehlermeldung
        /// </returns>
        public override IdentityError ConcurrencyFailure() { return new IdentityError { Code = nameof(ConcurrencyFailure), Description = "Ein Parallelitätsfehler ist aufgetreten; das Objekt wurde verändert." }; }

        /// <summary>
        /// Keine Überstimmung der Passwörter
        /// </summary>
        /// <returns>
        /// Die übersetzte Fehlermeldung
        /// </returns>
        public override IdentityError PasswordMismatch() { return new IdentityError { Code = nameof(PasswordMismatch), Description = "Das Passwort ist falsch." }; }

        /// <summary>
        /// Ungültiger Schlüssel
        /// </summary>
        /// <returns>
        /// Die übersetzte Fehlermeldung
        /// </returns>
        public override IdentityError InvalidToken() { return new IdentityError { Code = nameof(InvalidToken), Description = "Der Schlüssel ist ungültig." }; }

        /// <summary>
        /// Duplikat der Anmeldedaten
        /// </summary>
        /// <returns>
        /// Die übersetzte Fehlermeldung
        /// </returns>
        public override IdentityError LoginAlreadyAssociated() { return new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = "Es existiert bereits ein Benutzer mit diesen Anmeldedaten." }; }

        /// <summary>
        /// Ungültiger Benutzername
        /// </summary>
        /// <returns>
        /// Die übersetzte Fehlermeldung
        /// </returns>
        public override IdentityError InvalidUserName(string userName) { return new IdentityError { Code = nameof(InvalidUserName), Description = $"Der Benutzername \"{userName}\" ist unzulässig, da er nur Ziffern und Buchstaben beinhalten darf." }; }

        /// <summary>
        /// Ungültige E-Mail-Adresse
        /// </summary>
        /// <returns>
        /// Die übersetzte Fehlermeldung
        /// </returns>
        public override IdentityError InvalidEmail(string email) { return new IdentityError { Code = nameof(InvalidEmail), Description = $"Die E-Mail-Adresse \"{email}\" ist ungültig." }; }

        /// <summary>
        /// Duplikat des Benutzernamens
        /// </summary>
        /// <returns>
        /// Die übersetzte Fehlermeldung
        /// </returns>
        public override IdentityError DuplicateUserName(string userName) { return new IdentityError { Code = nameof(DuplicateUserName), Description = "" }; } //$"Der Benutzername \"{userName}\" ist bereits vergeben." }; }

        /// <summary>
        /// Duplikat der E-Mail-Adresse
        /// </summary>
        /// <returns>
        /// Die übersetzte Fehlermeldung
        /// </returns>
        public override IdentityError DuplicateEmail(string email) { return new IdentityError { Code = nameof(DuplicateEmail), Description = $"Die E-Mail-Adresse \"{email}\" ist bereits vergeben." }; }

        /// <summary>
        /// Ungültige Rollenbezeichnung
        /// </summary>
        /// <returns>
        /// Die übersetzte Fehlermeldung
        /// </returns>
        public override IdentityError InvalidRoleName(string role) { return new IdentityError { Code = nameof(InvalidRoleName), Description = $"Die Rollenname \"{role}\" ist ungültig." }; }

        /// <summary>
        /// Duplikat des Rollennamens
        /// </summary>
        /// <returns>
        /// Die übersetzte Fehlermeldung
        /// </returns>
        public override IdentityError DuplicateRoleName(string role) { return new IdentityError { Code = nameof(DuplicateRoleName), Description = $"Es existiert bereits eine Rolle mit dem Namen \"{role}\"." }; }

        /// <summary>
        /// Benutzerpasswort bereits eingerichtet
        /// </summary>
        /// <returns>
        /// Die übersetzte Fehlermeldung
        /// </returns>
        public override IdentityError UserAlreadyHasPassword() { return new IdentityError { Code = nameof(UserAlreadyHasPassword), Description = "Der Benutzer hat bereits ein Passwort eingerichtet." }; }

        /// <summary>
        /// Benutzer-Lockout deaktiviert
        /// </summary>
        /// <returns>
        /// Die übersetzte Fehlermeldung
        /// </returns>
        public override IdentityError UserLockoutNotEnabled() { return new IdentityError { Code = nameof(UserLockoutNotEnabled), Description = "Eine Funktionssperre ist für diesen Benutzer nicht möglich." }; }

        /// <summary>
        /// Benutzer bereits der Rolle zugeordnet
        /// </summary>
        /// <returns>
        /// Die übersetzte Fehlermeldung
        /// </returns>
        public override IdentityError UserAlreadyInRole(string role) { return new IdentityError { Code = nameof(UserAlreadyInRole), Description = $"Der Benutzer ist bereits der Rolle \"{role}\" zugeordnet." }; }

        /// <summary>
        /// Benutzer der Rolle nicht zugeordnet
        /// </summary>
        /// <returns>
        /// Die übersetzte Fehlermeldung
        /// </returns>
        public override IdentityError UserNotInRole(string role) { return new IdentityError { Code = nameof(UserNotInRole), Description = $"Der Benutzer ist der Rolle \"{role}\" nicht zugeordnet." }; }

        /// <summary>
        /// Passwortlänge unzureichend
        /// </summary>
        /// <returns>
        /// Die übersetzte Fehlermeldung
        /// </returns>
        public override IdentityError PasswordTooShort(int length) { return new IdentityError { Code = nameof(PasswordTooShort), Description = $"Passwörter müssen mindestens {length} Zeichen lang sein." }; }

        /// <summary>
        /// Kein Sonderzeichen im Passwort
        /// </summary>
        /// <returns>
        /// Die übersetzte Fehlermeldung
        /// </returns>
        public override IdentityError PasswordRequiresNonAlphanumeric() { return new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "Passwörter müssen mindestens ein Sonderzeichen beinhalten." }; }

        /// <summary>
        /// Keine Ziffer im Passwort
        /// </summary>
        /// <returns>
        /// Die übersetzte Fehlermeldung
        /// </returns>
        public override IdentityError PasswordRequiresDigit() { return new IdentityError { Code = nameof(PasswordRequiresDigit), Description = "Passwörter müssen mindestens eine Ziffer (\"0\"-\"9\") beinhalten." }; }

        /// <summary>
        /// Kein Kleinbuchstabe im Passwort
        /// </summary>
        /// <returns>
        /// Die übersetzte Fehlermeldung
        /// </returns>
        public override IdentityError PasswordRequiresLower() { return new IdentityError { Code = nameof(PasswordRequiresLower), Description = "Passwörter müssen mindestens einen Kleinbuchstaben (\"a\"-\"z\") beinhalten." }; }

        /// <summary>
        /// Kein Großbuchstabe im Passwort
        /// </summary>
        /// <returns>
        /// Die übersetzte Fehlermeldung
        /// </returns>
        public override IdentityError PasswordRequiresUpper() { return new IdentityError { Code = nameof(PasswordRequiresUpper), Description = "Passwörter müssen mindestens einen Großbuchstaben (\"A\"-\"Z\") beinhalten." }; }

        /// <summary>
        /// Unzureichend einzigartige Zeichen
        /// </summary>
        /// <returns>
        /// Die übersetzte Fehlermeldung
        /// </returns>
        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars) { return new IdentityError { Code = nameof(PasswordRequiresUniqueChars), Description = $"Passwörter müssen mindestens {uniqueChars} einzigartige Zeichen beinhalten." }; }

        /// <summary>
        /// Unmögliche Verwertung des Wiederherstellungscodes
        /// </summary>
        /// <returns>
        /// Die übersetzte Fehlermeldung
        /// </returns>
        public override IdentityError RecoveryCodeRedemptionFailed() { return new IdentityError { Code = nameof(RecoveryCodeRedemptionFailed), Description = "Der Wiederherstellungscode konnte nicht verwertet werden." }; }
    }
}