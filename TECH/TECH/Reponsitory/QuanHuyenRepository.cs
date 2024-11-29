using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IQuanHuyenRepository : IRepository<QuanHuyen, int>
    {
       
    }

    public class QuanHuyenRepository : EFRepository<QuanHuyen, int>, IQuanHuyenRepository
    {
        public QuanHuyenRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
