using Logos.Repository.Dishes;
using Logos.Repository.Orders;
using Logos.Repositoryes.AccountRepository;
using Logos.Settings;

namespace Logos.Configuration
{
	public static class DependencyStartup
	{

		public static void ConfigureServices(this WebApplicationBuilder builder)
		{
			AddArticles(builder.Services, builder.Configuration);
			SetDatabaseSettings(builder.Configuration);
		}
		private static void AddArticles(IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped<IDichRepository, DichRepository>();
			services.AddScoped<IAccountRepository, AccountRepository>();
			services.AddScoped<IOrdersRepository, OrdersRepository>();
		}
		private static void SetDatabaseSettings(IConfiguration configuration)
		{
			DatabaseSettings.ConnectionString = configuration.GetValue<string>("ConnectionString:MongoDB");
			DatabaseSettings.DatabaseName = configuration.GetValue<string>("ConnectionString:DataBase");
			DatabaseSettings.CollectionNameUser = configuration.GetValue<string>("MongoCollections:Users");
			DatabaseSettings.CollectionNameOrders = configuration.GetValue<string>("MongoCollections:Orders");
			DatabaseSettings.CollectionNameDish = configuration.GetValue<string>("MongoCollections:Dishes");
		}


	}
}
