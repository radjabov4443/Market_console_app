using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market_App.Enums
{
    public enum UserRole
    {
        [NpgsqlTypes.PgName("admin")]
        Admin,
        [NpgsqlTypes.PgName("merchant")]
        Merchant
    }
}
