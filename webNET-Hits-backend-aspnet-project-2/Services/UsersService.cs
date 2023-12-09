using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using webNET_Hits_backend_aspnet_project_2.Models.EnumModels;
using webNET_Hits_backend_aspnet_project_2.Models.InputModels;

namespace webNET_Hits_backend_aspnet_project_2.Services
{
    public class UsersService
    {

        private readonly TokenService _tokenService;
        readonly AppDbContext _dbContext;

        public UsersService(AppDbContext context, TokenService tokenService)
        {
            _dbContext = context;
            _tokenService = tokenService;
        }

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

        public bool EmailExists(string email)
        {
            return _dbContext.Users.Any(user => user.Email == email);
        }

        public bool EmailForEditExists(string email, Guid userId)
        {
            return _dbContext.Users.Any(user => user.Email == email && user.Id != userId);
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

        public UserData? RegisterUser(InputUserRegisterModel user)
        {
            Guid userId = Guid.NewGuid();
            UserData fullUser = new UserData
            {
                Id = userId,
                CreateTime = DateTime.UtcNow,
                FullName = user.FullName,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                Email = user.Email,
                PhoneNumber = ConvertPhoneNumberToDatabaseFormat(user.PhoneNumber)
            };

            string hashedPassword = HashPassword(user.Password);

            PasswordModel userPassword = new PasswordModel
            {
                UserId = userId,
                HashedPassword = hashedPassword
            };

            _dbContext.Users.Add(fullUser);
            _dbContext.UserPasswords.Add(userPassword);
            _dbContext.SaveChanges();

            return fullUser;
        }

        private string HashPassword(string password)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt();

            // Хеширование пароля с использованием соли
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            return hashedPassword;
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public string ConvertPhoneNumberToDatabaseFormat(string phoneNumber)
        {
            // Удаляем все символы, кроме цифр
            string cleanedPhoneNumber = Regex.Replace(phoneNumber, @"\D", "");

            return cleanedPhoneNumber;
        }

        public UserData Authenticate(string email, string password)
        {
            UserData user = _dbContext.Users.SingleOrDefault(currentUser => currentUser.Email == email);


            if (user == null)
            {
                return null;
            }

            PasswordModel userHashedPassword = _dbContext.UserPasswords.SingleOrDefault(currentUser => currentUser.UserId == user.Id);

            if (userHashedPassword == null)
            {
                return null;
            }
            else if (!VerifyPassword(password, userHashedPassword.HashedPassword))
            {
                return null;
            }

            return user;
        }

        public bool IsUserAuthenticated(Guid userId, out string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            UserData user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                ErrorMessage = "Мы не нашли такого пользователя";
                return false;
            }

            if (!_tokenService.CheckCurrentToken(userId))
            {
                ErrorMessage = "Вы не авторизованы";
                return false;
            }
            else
            {
                return true;
            }
        }

        public UserData GetUser(Guid userId)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Id == userId);
        }

        public bool EditUserProfile(Guid userId, UserProfileEditModel userProfileModel)
        {
            // Получение пользователя из базы данных по userId
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                // Пользователь не найден
                return false;
            }

            // Обновление данных профиля
            user.Email = userProfileModel.Email;
            user.FullName = userProfileModel.FullName;
            if (userProfileModel.BirthDate != null)
            {
                user.BirthDate = DateTime.SpecifyKind(userProfileModel.BirthDate, DateTimeKind.Utc);
            }
            user.Gender = userProfileModel.Gender;
            user.PhoneNumber = ConvertPhoneNumberToDatabaseFormat(userProfileModel.PhoneNumber);

            // Сохранение изменений в базе данных
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();

            return true;
        }
    }

}
