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
    public class WorkerEdit
    {
        public int WorkerId { get; set; }
        [Required, StringLength(50), Display(Name = "Worker Name")]
        public string WorkerName { get; set; } = default!;
        [Required, EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }
        [Required, StringLength(150)]
        public string Picture { get; set; } = default!;
        [Required, StringLength(50)]
        public string Phone { get; set; } = default!;
        [Required, Column(TypeName = "money"), DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal Payrate { get; set; }
    }
}
