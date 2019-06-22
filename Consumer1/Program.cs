using Consumer1.Commands;
using RawRabbit;
using RawRabbit.Configuration;
using RawRabbit.Instantiation;
using System;
using System.Threading.Tasks;

namespace Consumer1
{
	class Program
	{
		static void Main(string[] args)
		{
			var rawRabbitOptions = new RawRabbitOptions
			{
				ClientConfiguration = GetRawRabbitConfiguration()
			};
			var busClient = RawRabbitFactory.CreateSingleton();

			var qq = busClient.SubscribeAsync<CreateInvoiceCommand>(async (msg) =>
			{
				await Handle(msg);
			},
				CFG => CFG.UseSubscribeConfiguration(F => F.OnDeclaredExchange(E => E.WithName(""))
					.FromDeclaredQueue(q => q.WithName("createinvoicecommand")))
			);

			Console.ReadKey();
		}

		private static RawRabbitConfiguration GetRawRabbitConfiguration() => new RawRabbitConfiguration()
		{
			Username = "guest",
			Password = "guest",
			Port = 5672,
			VirtualHost = "/",
			Hostnames = { "localhost" }
		};

		public async static Task Handle(CreateInvoiceCommand cmd)
		{
			Console.WriteLine($"{DateTime.Now} Consumer 1 recived: {cmd.Name}");
			await Task.Yield();
		}
	}
}
