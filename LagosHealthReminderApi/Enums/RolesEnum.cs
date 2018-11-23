using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace LagosHealthReminderApi.Models
{
    public enum RolesEnum
    {
        [Description("Inputer")]
        Inputer = 1,
        [Description("Administrator")]
        Administrator = 2,
        [Description("Super Administrator")]
        SuperAdministrator = 3,
    }
}
