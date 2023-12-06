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
    }
}
