using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TECH.Data.DatabaseEntity;

namespace TECH.Areas.Admin.Models
{
    public class QuanHuyenModelView
    {
        public int Id { get; set; }
        public int? MaTP { get; set; }
        public string? Ten { get; set; }
        public string? GhiChu { get; set; }
    }
}
