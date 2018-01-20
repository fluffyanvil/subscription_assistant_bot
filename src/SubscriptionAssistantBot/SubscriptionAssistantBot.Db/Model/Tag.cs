using System;
using System.Collections.Generic;

namespace SubscriptionAssistantBot.Db.Model
{
    public partial class Tag
    {
        public Tag()
        {
            Subscription = new HashSet<Subscription>();
        }

        public int Id { get; set; }
        public string Value { get; set; }

        public ICollection<Subscription> Subscription { get; set; }
    }
}
