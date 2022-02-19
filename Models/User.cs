using Market_App.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market_App.Models
{

    [Table("Admins")]
    internal class User : SignIn
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Column("user_role")]
        public UserRole Role { get; set; } = UserRole.Merchant;

        public DateTime Created { get; set; }
    }
}