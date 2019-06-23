using Consumer2.Commands;
using RawRabbit;
using RawRabbit.Configuration;
using RawRabbit.Configuration.Exchange;
using RawRabbit.Instantiation;
using System;
using System.Threading.Tasks;

namespace Consumer2
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine($"{DateTime.Now} Consumer 2 is running");
			var rawRabbitOptions = new RawRabbitOptions
			{
				ClientConfiguration = GetRawRabbitConfiguration()
			};
			var busClient = RawRabbitFactory.CreateSingleton();


			try
			{
				var qq = busClient.SubscribeAsync<CreateInvoiceCommand>(async (msg) =>
				{
					await Handle(msg);
				},
					CFG => CFG.UseSubscribeConfiguration(F => 
						F.Consume(c => c
							.WithRoutingKey("createinvoicecommand1"))
						.OnDeclaredExchange(E => E
							.WithName("amq.topic")
							.WithType(ExchangeType.Topic))
						.FromDeclaredQueue(q => q
							.WithName("createinvoicecommand1"))
						));
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

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
			Console.WriteLine($"{DateTime.Now} Consumer 2 recived: {cmd.Name}");
			await Task.Yield();
		}
	}
}
