using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagosHealthReminderApi.Models
{
    public class Settings
    {
        public int SettingsId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
