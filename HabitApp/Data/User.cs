using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitApp.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public long Experience { get; set; }
        public long Money { get; set; }
        public int? GroupId { get; set; }

        public User()
        {

        }

        public User(int id, string username, string password, long experience, long money, int? groupId)
        {
            Id = id;
            Username = username;
            Password = password;
            Experience = experience;
            Money = money;
            GroupId = groupId;
        }
    }
}
