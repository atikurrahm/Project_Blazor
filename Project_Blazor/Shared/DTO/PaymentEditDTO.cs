using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Blazor.Shared.DTO
{
    public class PaymentEditDTO
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
        [Required]
        public int CustomerID { get; set; }
        public virtual ICollection<WorkEditDTO> Works { get; set; } = new List<WorkEditDTO>();
    }
}
