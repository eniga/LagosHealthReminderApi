using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagosHealthReminderApi.DbContext
{
    public class QrCodeContext
    {
        public int QrCodeId { get; set; }
        public string QrCode { get; set; }
        public byte[] QrCodeImage { get; set; }
        public int InsertUserId { get; set; }
        public DateTime InsertDate { get; set; }
        public int UpdateUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public int PrintStatus { get; set; }
    }
}
