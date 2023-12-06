using System.Net.Mail;
using System.Text.RegularExpressions;
using webNET_Hits_backend_aspnet_project_2.Models.EnumModels;
using webNET_Hits_backend_aspnet_project_2.Models.InputModels;

namespace webNET_Hits_backend_aspnet_project_2.Services
{
    public class UsersService
    {

        public bool FUllNameIsValid(string name)
        {
            // Проверка, что полное имя состоит из фамилии, имени и отчества
            // Предполагаем, что они разделены пробелами
            string[] nameParts = name.Split(' ');

            // Проверяем, что есть хотя бы три части (фамилия, имя, отчество) и каждая часть состоит из букв
            return nameParts.Length == 3 && nameParts.All(part => part.All(char.IsLetter));
        }
        public bool PasswordIsValid(string password, out string ErrorMessage)
        {
            var input = password;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                throw new Exception("Пароль не должен быть пустым\r\n");
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (!hasLowerChar.IsMatch(input))
            {
                ErrorMessage = "Пароль должен содержать по крайней мере одну строчную букву";
                return false;
            }
            else if (!hasUpperChar.IsMatch(input))
            {
                ErrorMessage = "Пароль должен содержать по крайней мере одну заглавную букву";
                return false;
            }
            else if (password.Length < 8)
            {
                ErrorMessage = "Длина пароля не должна быть не менее чем из 8 символов";
                return false;
            }
            else if (!hasNumber.IsMatch(input))
            {
                ErrorMessage = "Пароль должен содержать по крайней мере одно числовое значение";
                return false;
            }

            else if (!hasSymbols.IsMatch(input))
            {
                ErrorMessage = "Пароль должен содержать по крайней мере один символ особого регистра";
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool EmailIsValid(string email)
        {
            try
            {
                MailAddress mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public bool BirthDateIsValid(DateTime date)
        {
            // Проверка, что пользователь старше 14 лет и младше 130 лет от текущей даты
            DateTime currentDate = DateTime.Now;
            int userAge = currentDate.Year - date.Year;

            if (currentDate.Month < date.Month || (currentDate.Month == date.Month && currentDate.Day < date.Day))
            {
                userAge--;
            }

            return userAge > 14 && userAge < 130;
        }

        public bool GenderIsValid(Gender gender)
        {
            return gender == Gender.Male || gender == Gender.Female;
        }

        public bool PhoneNumberIsValid(string phoneNumber)
        {
            string phoneNumberPattern = @"^(\+7)[\s(]*\d{3}[)\s]*\d{3}[\s-]?\d{2}[\s-]?\d{2}$";

            return Regex.IsMatch(phoneNumber, phoneNumberPattern);
        }
    }
}
