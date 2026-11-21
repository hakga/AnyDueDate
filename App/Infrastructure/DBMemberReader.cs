using Microsoft.EntityFrameworkCore;
using App.DB;
using App.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Infrastructure {
    public class DBMemberReader : IMemberReader {
        private readonly SampleDbContext _dbContext;
        public DBMemberReader(SampleDbContext context) {
            _dbContext = context;
        }
        public IQueryable<Domain.Entities.Member> Load() {
            return  _dbContext.Members.AsNoTracking();
        }
    }
}
