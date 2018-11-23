using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagosHealthReminderApi.Models
{
    public class LoginResponse : Response
    {
        public Users details { get; set; }
    }
}
