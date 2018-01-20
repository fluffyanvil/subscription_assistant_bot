using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SubscriptionAssistantBot.Db.Model
{
    public partial class Subscription
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public int UserId { get; set; }
        public int ChatId { get; set; }

        public Tag Tag { get; set; }

        public Subscription()
        {

        }
    }
}
