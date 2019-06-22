using RawRabbit;
using RawRabbit.Configuration;
using RawRabbit.Instantiation;
using System;
using System.Threading;

namespace Producer
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
			var random = new Random();
			while (true)
			{
				var message = random.Next(1, 1000);
				var cmd = new CreateInvoiceCommand() { Name = message .ToString() };
				busClient.PublishAsync(cmd, CFG => CFG.UsePublishConfiguration(C => C.OnExchange("")
					.WithRoutingKey("createinvoicecommand1")));
				Console.WriteLine($"{DateTime.Now} Sended: {cmd.Name}");
				Thread.Sleep(2000);
			}
		}

		private static RawRabbitConfiguration GetRawRabbitConfiguration() => new RawRabbitConfiguration()
		{
			Username = "guest",
			Password = "guest",
			Port = 5672,
			VirtualHost = "/",
			Hostnames = { "localhost" }
		};
	}
}
