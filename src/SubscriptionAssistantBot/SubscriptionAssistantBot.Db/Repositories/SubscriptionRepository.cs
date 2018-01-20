using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SubscriptionAssistantBot.Db.Model;

namespace SubscriptionAssistantBot.Db.Repositories
{
    public class SubscriptionRepository : IRepository<Subscription>
    {
        private SubscriptionsDbContext _context = new SubscriptionsDbContext();
        public async Task<Subscription> Add(Subscription item)
        {
            try
            {
                var tag = (await _context.Subscription.AddAsync(item)).Entity;
                await _context.SaveChangesAsync();
                return tag;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                var sub = await Find(s => s.Id.Equals(id));
                await Task.Factory.StartNew(() => _context.Subscription.Remove(sub));
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Subscription> Find(Func<Subscription, bool> func)
        {
            try
            {
                return await _context.Subscription.Include(s => s.Tag).FirstOrDefaultAsync(t => func.Invoke(t));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<Subscription>> FindAll(Func<Subscription, bool> func)
        {
            try
            {
                return await Task.Factory.StartNew(() => _context.Subscription.Include(s => s.Tag).Where(t => func.Invoke(t)));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}