using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SantaTracker.Generator
{
    public static class Program
    {
        private static readonly FlightRepository _repo = new FlightRepository();
        private static readonly TelemetryGenerator _generator = new TelemetryGenerator();

        public static async Task Main(string[] args)
        {
            Console.WriteLine("Santa Tracker Telemetry Generator");
            Console.WriteLine();

			IConfigurationRoot config = new ConfigurationBuilder()
				.SetBasePath(Environment.CurrentDirectory)				
				.AddJsonFile("appsettings.json")
				.AddUserSecrets("20e02be9-5541-455d-98a1-584bc91e7fab")
				.Build();

			Cosmos.SetAuth(config["CosmosDB-ConnectionStringReadWrite"]);

            if (args.Length > 0)
            {
                await RunOperation(args);
            }
            else
            {
                await RunInteractive();
            }
        }

        private static async Task RunInteractive()
        {
            ShowUsage();
            while (true)
            {
                Console.Write("Santa-Tracker-Telemetry-Generator> ");
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    if ("quit".StartsWith(input.ToLower()))
                    {
                        break;
                    }
                    var args = input.Split(' ');
                    await RunOperation(args);
                }
            }
        }

		private static async Task RunOperation(string[] args)
		{
			try
			{
				var operation = args[0].ToLower();
				var arg1 = args.Length > 1 ? args[1].ToLower() : null;

				if (operation.Matches("initialize"))
				{
					await _generator.InitializeDatabase();
				}
				else if (operation.Matches("delete"))
				{
					await _generator.DeleteDatabase(confirm: true);
				}
				else if (operation.Matches("generate"))
				{
					await _generator.GenerateData();
				}
				else if (operation.Matches("cities"))
				{
					await ShowCities();
				}
				else if (operation.Matches("segments"))
				{
					await ShowFlightSegments();
				}
				else if (operation.Matches("reset"))
				{
					await _generator.ResetFlightSegments();
				}
				else if (operation.Matches("help") || operation == "?")
				{
					ShowUsage();
				}
				else
				{
					throw new Exception("Unrecognized command");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
				ShowUsage();
			}
		}

		private static bool Matches(this string operation, string match) =>
			match.StartsWith(operation);

		private static void ShowUsage()
		{
			Console.WriteLine("Usage:");
			Console.WriteLine("  initialize    Initialize database");
			Console.WriteLine("  delete        Delete database");
			Console.WriteLine("  reset         Reset flight data");
			Console.WriteLine("  generate      Generate data");
			Console.WriteLine("  cities        Show cities");
			Console.WriteLine("  segments      Show flight segments");
			Console.WriteLine("  help (or ?)   Show usage");
			Console.WriteLine("  quit          Exit");
			Console.WriteLine();
		}

		private static async Task ShowFlightSegments()
		{
			var flights = await _repo.GetFlightSegments();
			var ctr = 0;
			foreach (var flight in flights)
			{
				ctr++;
				Console.WriteLine($"[{ctr,2}] {flight.RouteNumber,-8}{flight.DepartureCity} > {flight.ArrivalCity}  {flight.DistanceMiles} miles");
			}
		}

		private static async Task ShowCities()
		{
			var airports = await _repo.GetCities();
			var ctr = 0;
			foreach (var airport in airports)
			{
				ctr++;
				Console.WriteLine($"[{ctr,2}] {airport.Code} - {airport.Name} ({airport.Latitude}, {airport.Longitude})");
			}
		}
	}
}
