using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagosHealthReminderApi.Models
{
    public class QrCodes
    {
        public int QrCodeId { get; set; }
        public string QrCode { get; set; }
        public byte[] QrCodeImage { get; set; }
        public int InsertUserId { get; set; }
        public string InsertUser { get; set; }
        public DateTime InsertDate { get; set; }
        public int UpdateUserId { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
        public int PrintStatus { get; set; }
    }

    public class QrCodesRequest
    {
        public int NumberOfCodes { get; set; }
        public int InsertUserId { get; set; }
    }

    public class QrCodeStats
    {
        public int Generated { get; set; }
        public int Printed { get; set; } 
        public int Unprinted { get; set; }
        public int Mapped { get; set; }
        public int Unmapped { get; set; }
    }
}
