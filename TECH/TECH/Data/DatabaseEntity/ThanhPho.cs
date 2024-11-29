using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;

namespace TECH.Data.DatabaseEntity
{
    [Table("ThanhPho")]
    public class ThanhPho : DomainEntity<int>
    {
        [Column(TypeName = "nvarchar(500)")]
        public string? Ten { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? GhiChu { get; set; }
    }
}
