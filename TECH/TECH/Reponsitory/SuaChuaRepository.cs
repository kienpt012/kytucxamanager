using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface ISuaChuaRepository : IRepository<SuaChua, int>
    {
       
    }

    public class SuaChuaRepository : EFRepository<SuaChua, int>, ISuaChuaRepository
    {
        public SuaChuaRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
