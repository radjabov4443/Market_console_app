using Market_App.IRepository;
using Market_App.Models;
using Market_App.Registration;
using Market_App.Repositories;

namespace Market_App
{
    internal class Program
    {
        static void Main(string[] args)
        {

            MainMenu regist = new MainMenu();
            regist.Menu();

        }

    }
}
