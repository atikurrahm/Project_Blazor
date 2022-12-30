using Project_Blazor.shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Blazor.Shared.DTO
{
    public class WorkDTO
    {
        [ForeignKey("Payment")]
        public int PaymentId { get; set; }
        [ForeignKey("Worker")]
        public int WorkerId { get; set; }
        [Required]
        public int TotalWorkHour { get; set; }

    }
}
