using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SubscriptionAssistantBot.Db.Model;

namespace SubscriptionAssistantBot.Db.Repositories
{
    public class TagRepository: IRepository<Tag>
    {
        private SubscriptionsDbContext _context = new SubscriptionsDbContext();

        public async Task<Tag> Add(Tag item)
        {
            try
            {
                var tag = (await _context.Tag.AddAsync(item)).Entity;
                await _context.SaveChangesAsync();
                return tag;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Tag> Find(Func<Tag, bool> func)
        {
            var tag = await _context.Tag.Include(t => t.Subscription).FirstOrDefaultAsync(t => func.Invoke(t));
            return tag;
        }

        public async Task<IEnumerable<Tag>> FindAll(Func<Tag, bool> func)
        {
            return await Task.Factory.StartNew(() => _context.Tag.Include(t => t.Subscription).Where(t => func.Invoke(t)));
        }
    }
}