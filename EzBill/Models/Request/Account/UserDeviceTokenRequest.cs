namespace EzBill.Models.Request.Account
{
    public class UserDeviceTokenRequest
    {
		public string DeviceId { get; set; }
		public string FCMToken { get; set; }
	}
}
