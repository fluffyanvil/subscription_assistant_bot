using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SubscriptionAssistantBot.Db.Repositories
{
    public interface IRepository<T>
    {
        Task<T> Add(T item);
        Task Delete(int id);
        Task<T> Find(Func<T, bool> func);
        Task<IEnumerable<T>> FindAll(Func<T, bool> func);
    }
}