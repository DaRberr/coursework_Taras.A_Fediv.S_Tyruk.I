using MongoDB.Driver;

namespace Logos.Settings
{
	public static class DatabaseSettings
	{
		public static string ConnectionString {  get; set; }
		public static string DatabaseName { get; set; }
		public static string CollectionNameUser {  get; set; }
		public static string CollectionNameOrders { get; set; }
		public static string CollectionNameDish { get; set; }

	}

}
