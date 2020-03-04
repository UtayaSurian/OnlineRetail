using System.Linq;
using OnlineRetail.Core.Models;

namespace OnlineRetail.Core.Contracts
{                                           //Dependancy Injection
    public interface IRepository<G> where G : BaseEntity
    {
        IQueryable<G> Collection();
        void Commit();
        void Delete(string Id);
        G Find(string Id);
        void Insert(G g);
        void Update(G g);
    }
}