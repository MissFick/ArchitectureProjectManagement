using System;
namespace ArchitectureProjectManagement.Models
{
    public class HomeViewModel
    {
        public ApplicationUserViewModel ApplicationUser;

        public int ApplicationUserId { get; set; }

        public int ClientId { get; set; }

        public PropertyOwnerPropertyViewModel Client { get; set; }

        public DraughtsmanViewModel Draughtsman { get; set; }

        public int DraughtsmanId { get; set; }
    }
}
