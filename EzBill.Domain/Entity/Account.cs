using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.Entity
{
    public enum AccountRole
    {
        FREE_USER, BASIC_USER, VIP_USER, DAILY_USER,ADMIN
    }
	public enum AccountStatus
	{
		ACTIVE, INACTIVE, BLOCKED
	}
	public class Account
    {
        public Guid AccountId { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

		public string PhoneNumber { get; set; }

		public string? AvatarUrl { get; set; }

        public string NickName { get; set; }

        public bool Gender { get; set; }   

        public string Role { get; set; }

        public string Status { get; set; }

		public virtual ICollection<Trip> Trip { get; set; }

        public virtual ICollection<TripMember> TripMembers { get; set; }

        public virtual ICollection<Event_Use> Event_Use { get; set; }

        public virtual ICollection<Event> Events { get; set; }
        public ICollection<Settlement> SettlementsFrom { get; set; }


        public ICollection<Settlement> SettlementsTo { get; set; }
        public virtual ICollection<PaymentHistory> PaymentHistoriesFrom { get; set; }
        public virtual ICollection<PaymentHistory> PaymentHistoriesTo { get; set; }
        public virtual ICollection<TaxRefund> TaxRefunds { get; set; }
        public virtual ICollection<TaxRefund_Usage> TaxRefund_Usages { get; set; }

		public virtual ICollection<AccountSubscriptions> AccountSubscriptions { get; set; }

		public virtual ICollection<ForgotPassword> ForgotPasswords { get; set; }

		public virtual ICollection<UserDeviceToken> UserDeviceTokens { get; set; }

	}
}
