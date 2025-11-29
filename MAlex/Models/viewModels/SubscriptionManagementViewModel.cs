using System.Collections.Generic;

namespace MAlex.ViewModels
{
    public class SubscriptionManagementViewModel
    {
        public List<MAlex.Models.UserSubscription> UserSubscriptions { get; set; } = new();
        public List<MAlex.Models.Subscrubtion> Subscriptions { get; set; } = new();
    }
}
