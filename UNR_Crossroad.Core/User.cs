using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNR_Crossroad.Core
{
    // Класс юзера симулятора (Мы). В данный момент не используется в полной мере, ибо мы автоматом логинимся под админом
    public class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public AccLevel Level { get; set; }
        public User(string login, string pass, AccLevel lvl)
        {
            Login = login;
            Password = pass;
            Level = lvl;
        }
    }
    // Уровень доступа
    public enum AccLevel
    {
        Player = 0,
        Admin = 1
    }
}
