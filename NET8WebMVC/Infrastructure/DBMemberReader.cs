using Microsoft.EntityFrameworkCore;
using NET8WebMVC.DB;
using NET8WebMVC.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NET8WebMVC.Infrastructure {
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
