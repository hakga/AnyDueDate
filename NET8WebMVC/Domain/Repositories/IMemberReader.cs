using System.Collections.Generic;
using System.Threading.Tasks;

namespace NET8WebMVC.Domain.Repositories {
    public interface IMemberReader {
        public IQueryable<Domain.Entities.Member> Load();
    }
}
