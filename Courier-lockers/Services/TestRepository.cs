using Courier_lockers.Data;
using Courier_lockers.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace Courier_lockers.Services
{
    public class TestRepository :ITest
    {
        private readonly ServiceDbContext _context;
        public TestRepository(ServiceDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<edpmain>> getAllTest()
        {
            try
            {
                var st = await   _context.edpmains.Where(f => true).ToListAsync();
                return st;
            }catch(Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
            return null;
            

        }
    }
}
