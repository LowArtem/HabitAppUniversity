using HabitApp.Data;
using HabitApp.Implementation;
using System;
using System.Windows;

namespace HabitApp.Services
{
    public class LoginService
    {
        private readonly UserRepository _userRepository;
        public LoginService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool Login(string username, string password)
        {
            var user = _userRepository.GetUserByCredentials(username, password);
            var app = Application.Current;
            if (app is App currentApp)
            {
                currentApp.CurrentUser = user;
            }
            else
            {
                throw new Exception("Неверный класс приложения, не обнаружено свойство CurrentUser");
            }
            return user != null;
        }

        public bool Register(string username, string password)
        {
            var tryUser = _userRepository.GetUserByCredentials(username, password);
            if (tryUser != null)
            {
                // Пользователь с такими данными уже существует
                return false;
            }
            else
            {
                var user = new User(username, password);

                try
                {
                    user = _userRepository.Add(user);
                    var app = Application.Current;
                    if (app is App currentApp)
                    {
                        currentApp.CurrentUser = user;
                    }
                    else
                    {
                        throw new Exception("Неверный класс приложения, не обнаружено свойство CurrentUser");
                    }

                    return true;
                }
                catch(Exception)
                {
                    // Произошла ошибка при добавлении пользователя в БД
                    var app = Application.Current;
                    if (app is App currentApp)
                    {
                        currentApp.CurrentUser = null;
                    }
                    else
                    {
                        throw new Exception("Неверный класс приложения, не обнаружено свойство CurrentUser");
                    }
                    return false;
                }
            }
        }
    }
}
