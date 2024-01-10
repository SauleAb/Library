using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    [Serializable]
    public class UsersManagement
    {
        private List<User> users = new List<User>();

        public UsersManagement()
        {
            users = new List<User>();
        }

        public void AddUser(User user)
        {
            users.Add(user);
        }

        public void RemoveUser(User user)
        {
            users.Remove(user);
        }

        public User GetUserByNameAndBirth(string firstName, string lastName, DateTime birthTime)
        {
            foreach (User u in users)
            {
                if (u.GetFirstName() == firstName && u.GetLastName() == lastName && u.GetBirthDate() == birthTime)
                {
                    return u;
                }
            }
            return null;
        }

        public User GetUserByEmail(string email)
        {
            foreach (User user in users)
            {
                if (user.GetEmail() == email)
                {
                    return user;
                }
            }
            return null;
        }

        public List<User> GetAllUsers()
        {
            return users;
        }
    }
}
