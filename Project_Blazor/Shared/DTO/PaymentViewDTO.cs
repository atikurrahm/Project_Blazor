using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    public class PaymentViewDTO
    {
        [Key]
        public int PaymentId { get; set; }
        [Required, Column(TypeName = "date"),
        Display(Name = "Start Date"),
        DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
        ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [Required, Column(TypeName = "date"),
        Display(Name = "End Date"),
        DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
        ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        public bool PaymentDone { get; set; }
        
        public string CustomerName { get; set; } = default!;
        public int TotalWorker { get; set; }
        public decimal TotalPayment { get; set; }

    }
}
