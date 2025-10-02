using Library.DAL.Context;
using Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DAL.Repositories
{
    public class HistoryRepository : GenericRepository<History>, IHistoryRepository
    {
        public HistoryRepository(LibraryDbContext context) : base(context) { }
    }
}
