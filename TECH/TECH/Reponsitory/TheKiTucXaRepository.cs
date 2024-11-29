using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface ITheKiTucXaRepository : IRepository<TheKiTucXa, int>
    {
       
    }

    public class TheKiTucXaRepository : EFRepository<TheKiTucXa, int>, ITheKiTucXaRepository
    {
        public TheKiTucXaRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
