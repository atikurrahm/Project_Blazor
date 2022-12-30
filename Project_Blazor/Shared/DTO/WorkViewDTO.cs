using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Blazor.Shared.DTO
{
    public class WorkViewDTO
    {
       
        public int PaymentId { get; set; }
        [Required]
        public string WorkerName { get; set; } = default!;
        [Required]
        public decimal Payrate { get; set; }
        [Required]
        public int TotalWorkHour { get; set; }
    }
}
