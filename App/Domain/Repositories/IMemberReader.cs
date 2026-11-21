using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Domain.Repositories {
    public interface IMemberReader {
        public IQueryable<Domain.Entities.Member> Load();
    }
}
