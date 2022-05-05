using System.Collections.Generic;
using CommonComponents.Models;

namespace ApiGateway.Services
{
    public interface IUserService
    {
        public Dictionary<string, string> users { get; set; }

        public bool checkIfUserExisted(LoginUser loginUser);
    }

    public class UserService : IUserService
    {
        public Dictionary<string, string> users { get; set; }

        public UserService()
        {
            users = new Dictionary<string, string>
            {
                { "user1", "user1" },
                { "user2", "user2" },
                { "user3", "user3" },
                { "user4", "user4" },
                { "user5", "user5" },
                { "user6", "user6" },
                { "user7", "user7" },
                { "user8", "user8" },
                { "user9", "user9" },
                { "user10", "user10" }
            };
        }

        public bool checkIfUserExisted(LoginUser loginUser)
        {
            bool result = true;

            if (users.ContainsKey(loginUser.Username))
            {
                if (users[loginUser.Username] == loginUser.Password)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }

            return result;
        }
    }
}