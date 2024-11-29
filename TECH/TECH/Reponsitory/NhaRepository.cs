using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface INhaRepository : IRepository<Nha, int>
    {
       
    }

    public class NhaRepository : EFRepository<Nha, int>, INhaRepository
    {
        public NhaRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
