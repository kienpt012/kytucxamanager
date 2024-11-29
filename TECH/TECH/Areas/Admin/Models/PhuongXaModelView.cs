using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TECH.Data.DatabaseEntity;

namespace TECH.Areas.Admin.Models
{
    public class PhuongXaModelView
    {
        public int Id { get; set; }
        public int? MaQH { get; set; }
        public string? Ten { get; set; }
        public string? GhiChu { get; set; }
    }
}
