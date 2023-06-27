﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class RechargeRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
        public int Id { get; set; }
        public long UserId { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public string? Code { get; set; }
        public long Amount { get; set; }
        public bool IsPay { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; } = null;
    }
}
