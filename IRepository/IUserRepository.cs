using Market_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Market_App.IRepository

{
    interface IUserRepository
    {
        void Create(User user);
        
        IList<User> GetAllUsers();
        
        User GetUser(int id);
        
        User Login(SignIn signIn);
        
        void EditUser(User user);
        
        void RemoveUser(User user);
    }
}
