using Microsoft.Azure.Cosmos;
using SantaTracker.Shared.Constants;
using SantaTracker.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;

namespace SantaTracker.Generator
{
    public class TelemetryGenerator
    {
        private const int TelemetryIntervalMs = 10;
		private const int AverageFlightSpeed = 5000; // mph
		private const int TelemetryRetentionDays = 30;

		private readonly City[] _cities = new[]
		{
			new City { Code = "YYT", Longitude = -52.7525, Latitude = 47.618611, Name = "St. John's", Region = "NL", Country = "Canada" },
			new City { Code = "YQY", Longitude = -60.048056, Latitude = 46.161389, Name = "Sydney", Region = "NS", Country = "Canada" },
			new City { Code = "YYG", Longitude = -63.115278, Latitude = 46.289167, Name = "Charlottetown", Region = "PE", Country = "Canada" },
			new City { Code = "YHZ", Longitude = -63.510278, Latitude = 44.879722, Name = "Halifax", Region = "NS", Country = "Canada" },
            new City { Code = "YFC", Longitude = -66.537222, Latitude = 45.868889, Name = "Fredericton", Region = "NB", Country = "Canada" },
			new City { Code = "YFB", Longitude = -68.556111, Latitude = 63.756667, Name = "Iqaluit", Region = "NU", Country = "Canada" },
			new City { Code = "YQB", Longitude = -71.393333, Latitude = 46.791111, Name = "Quebec City", Region = "QC", Country = "Canada" },
			new City { Code = "YUL", Longitude = -73.740833, Latitude = 45.470556, Name = "Montreal", Region = "QC", Country = "Canada" },
			new City { Code = "YOW", Longitude = -75.667222, Latitude = 45.3225, Name = "Ottawa", Region = "ON", Country = "Canada" },
			new City { Code = "YYZ", Longitude = -79.6305565, Latitude = 43.676667, Name = "Toronto", Region = "ON", Country = "Canada" },
			new City { Code = "YAM", Longitude = -84.509444, Latitude = 46.485, Name = "Sault Ste. Marie", Region = "ON", Country = "Canada" },
			new City { Code = "YQT", Longitude = -89.321667, Latitude = 48.371944, Name = "Thunder Bay", Region = "ON", Country = "Canada" },
			new City { Code = "YWG", Longitude = -97.24, Latitude = 49.91, Name = "Winnipeg", Region = "MN", Country = "Canada" },				
			new City { Code = "YQR", Longitude = -104.666111, Latitude = 50.432222, Name = "Regina", Region = "SK", Country = "Canada" },
			new City { Code = "YXE", Longitude = -106.7, Latitude = 52.170833, Name = "Saskatoon", Region = "SK", Country = "Canada" },
			new City { Code = "YZF", Longitude = -114.440278, Latitude = 62.463056, Name = "Yellowknife", Region = "NT", Country = "Canada" },
			new City { Code = "YEG", Longitude = -113.579444, Latitude = 53.31, Name = "Edmonton", Region = "AB", Country = "Canada" },
			new City { Code = "YCG", Longitude = -114.013333, Latitude = 51.1225, Name = "Calgary", Region = "AB", Country = "Canada" },
			new City { Code = "YVR", Longitude = -123.183889, Latitude = 49.194722, Name = "Vancouver", Region = "BC", Country = "Canada" },
			new City { Code = "YYJ", Longitude = -123.425833, Latitude = 48.647222, Name = "Victoria", Region = "BC", Country = "Canada" },
			new City { Code = "YXS", Longitude = -122.6775, Latitude = 53.884167, Name = "Prince George", Region = "BC", Country = "Canada" },
			new City { Code = "YXY", Longitude = -135.067222, Latitude = 60.709444, Name = "Whitehorse", Region = "YT", Country = "Canada" },
            new City { Code = "ANC", Longitude = -149.998333, Latitude = 61.174167, Name = "Anchorage", Region = "AK", Country = "United States of America" },
        };

        private FlightSegment[] _flightSegments = new[]
        {
            new FlightSegment { RouteNumber = "100", DepartureCity = "YYT", ArrivalCity = "YQY" },
            new FlightSegment { RouteNumber = "101", DepartureCity = "YQY", ArrivalCity = "YYG" },
            new FlightSegment { RouteNumber = "102", DepartureCity = "YYG", ArrivalCity = "YHZ" },
            new FlightSegment { RouteNumber = "103", DepartureCity = "YHZ", ArrivalCity = "YFC" },
            new FlightSegment { RouteNumber = "104", DepartureCity = "YFC", ArrivalCity = "YFB" },
            new FlightSegment { RouteNumber = "105", DepartureCity = "YFB", ArrivalCity = "YQB" },
            new FlightSegment { RouteNumber = "106", DepartureCity = "YQB", ArrivalCity = "YUL" },
            new FlightSegment { RouteNumber = "107", DepartureCity = "YUL", ArrivalCity = "YOW" },
            new FlightSegment { RouteNumber = "108", DepartureCity = "YOW", ArrivalCity = "YYZ" },
            new FlightSegment { RouteNumber = "109", DepartureCity = "YYZ", ArrivalCity = "YAM" },
            new FlightSegment { RouteNumber = "110", DepartureCity = "YAM", ArrivalCity = "YQT" },
            new FlightSegment { RouteNumber = "111", DepartureCity = "YQT", ArrivalCity = "YWG" },
            new FlightSegment { RouteNumber = "112", DepartureCity = "YWG", ArrivalCity = "YQR" },
            new FlightSegment { RouteNumber = "113", DepartureCity = "YQR", ArrivalCity = "YXE" },
            new FlightSegment { RouteNumber = "114", DepartureCity = "YXE", ArrivalCity = "YZF" },
            new FlightSegment { RouteNumber = "115", DepartureCity = "YZF", ArrivalCity = "YEG" },
            new FlightSegment { RouteNumber = "116", DepartureCity = "YEG", ArrivalCity = "YCG" },
            new FlightSegment { RouteNumber = "117", DepartureCity = "YCG", ArrivalCity = "YVR" },
            new FlightSegment { RouteNumber = "118", DepartureCity = "YVR", ArrivalCity = "YYJ" },
            new FlightSegment { RouteNumber = "119", DepartureCity = "YYJ", ArrivalCity = "YXS" },
            new FlightSegment { RouteNumber = "120", DepartureCity = "YXS", ArrivalCity = "YXY" },
            new FlightSegment { RouteNumber = "121", DepartureCity = "YXY", ArrivalCity = "ANC" },
        }; 
                           
        private static readonly IDictionary<string, SpeedAltitudeEvent> _speedAndAltitude =  new Dictionary<string, SpeedAltitudeEvent>();
        private static readonly object _threadLock = new object();

		private readonly FlightRepository _flightRepo = new FlightRepository();
		private readonly FlightSpatial _spatial = new FlightSpatial();

		private readonly Randomizer _random = new Randomizer();

		private int _telemetryCount;
		private List<string> _completedFlightSegments;
        private int _currentFlightSegment;
        private int _errorCount;

		private (string ContainerName, string PartitionKey, int Throughput, bool AutoScale, int? TimeToLive)[] Containers => new (string, string, int, bool, int?)[]
		{
			(Constants.CosmosDb.LeaseContainerName, "/id", 400, false, null),				// for CFP Library
			(Constants.CosmosDb.MetaDataContainerName, "/type", 400, false, null),			// for small lookup lists; pk=city/flightSegment; id=City Code/Route Number
			(Constants.CosmosDb.LocationContainerName, "/id", 1000, false, null),			// for ingestion; pk/id = GUID
			(Constants.CosmosDb.CurrentLocationContainerName, "/type", 400, false, null),	// for CurrentLocation microservice; pk=location; id=Flight Number
			(Constants.CosmosDb.DeliveryBoardContainerName, "/type", 400, false, null),		// for DeliveryBoard microservice; pk=delivery; id=City Code
		};

		public async Task InitializeDatabase()
		{
			if (!this.Confirm($"This will create the {Constants.CosmosDb.DatabaseName} database. It will be deleted if it already exists."))
			{
				return;
			}

			//await this.CreateDatabase();
			//await this.CreateContainers();
			await this.CreateCities();
			await this.CreateFlightSegments();
		}

		private async Task CreateDatabase()
		{
			Console.WriteLine($"Creating database {Constants.CosmosDb.DatabaseName}");
			await this.DeleteDatabase(confirm: false);
			await Cosmos.Client.CreateDatabaseAsync(Constants.CosmosDb.DatabaseName);
			Console.WriteLine($"Created database {Constants.CosmosDb.DatabaseName}");
		}

		private async Task CreateContainers()
		{
			Console.WriteLine("Creating containers");
			foreach (var container in this.Containers)
			{
				await this.CreateContainer(
					container.ContainerName,
					container.PartitionKey,
					container.Throughput,
					container.AutoScale,
					container.TimeToLive);
			}
			Console.WriteLine($"Created containers");
		}

		protected async Task CreateContainer(string containerName, string partitionKey, int throughput, bool autoScale, int? timeToLive)
		{
			var throughputProperties = default(ThroughputProperties);
			if (autoScale)
			{
				throughputProperties = ThroughputProperties.CreateAutoscaleThroughput(throughput * 10);
				Console.WriteLine($"Creating container '{containerName}', partitionKey = {partitionKey}, throughput = {throughput} - {throughput * 10} RUs");
			}
			else
			{
				throughputProperties = ThroughputProperties.CreateManualThroughput(throughput);
				Console.WriteLine($"Creating container '{containerName}', partitionKey = {partitionKey}, throughput = {throughput} RUs");
			}
			var containerProperties = new ContainerProperties
			{
				Id = containerName,
				PartitionKeyPath = partitionKey,
				DefaultTimeToLive = timeToLive,
			};

			await Cosmos.Client.GetDatabase(Constants.CosmosDb.DatabaseName).CreateContainerAsync(containerProperties, throughputProperties);
		}

		public async Task DeleteDatabase(bool confirm)
		{
			if (confirm && !this.Confirm($"This will delete the {Constants.CosmosDb.DatabaseName} database, if it already exists."))
			{
				return;
			}

			var deleted = false;
			try
			{
				await Cosmos.Client.GetDatabase(Constants.CosmosDb.DatabaseName).DeleteAsync();
				deleted = true;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Could not delete database {Constants.CosmosDb.DatabaseName}: {ex.Message}");
			}
			if (deleted)
			{
				Console.WriteLine($"Deleted database {Constants.CosmosDb.DatabaseName}");
			}
		}

		private async Task CreateCities()
		{
			var container = Cosmos.Client.GetContainer(Constants.CosmosDb.DatabaseName, Constants.CosmosDb.MetaDataContainerName);
			foreach (var city in this._cities)
			{
				city.Id = city.Code;
				city.Type = "city";
				await container.CreateItemAsync(city, new PartitionKey(city.Type));
			}
			Console.WriteLine($"Created {this._cities.Length} cities");
		}

		private async Task CreateFlightSegments()
		{
			var container = Cosmos.Client.GetContainer(Constants.CosmosDb.DatabaseName, Constants.CosmosDb.MetaDataContainerName);
			foreach (var segment in this._flightSegments)
			{
				var departureAirport = this._cities.First(a => a.Code == segment.DepartureCity);
				var arrivalAirport = this._cities.First(a => a.Code == segment.ArrivalCity);
				var distance = this._spatial.CalculateDistance(departureAirport.Latitude, departureAirport.Longitude, arrivalAirport.Latitude, arrivalAirport.Longitude);
				var duration = (distance / AverageFlightSpeed) * 60;  // assume 5000 mph
				var arrivalTime = DateTime.UtcNow.AddMinutes(duration);

				segment.Id = segment.RouteNumber.ToString();
				segment.Type = "flightSegment";
				segment.Latitude = departureAirport.Latitude;
				segment.Longitude = departureAirport.Longitude;
				segment.DistanceMiles = Convert.ToInt32(distance);
				segment.DepartureCityName = $"{departureAirport.Name}, {departureAirport.Region}, {departureAirport.Country}";
				segment.ArrivalCityName = $"{arrivalAirport.Name}, {arrivalAirport.Region}, {arrivalAirport.Country}";
				segment.DurationMinutes = Math.Round(duration, 2);

				await container.CreateItemAsync(segment, new PartitionKey(segment.Type));
			}

			Console.WriteLine($"Created {this._flightSegments.Length} flight segments");
		}

		public async Task ResetFlightSegments()
		{
			var cities = await this._flightRepo.GetCities();
			var flights = await this._flightRepo.GetFlightSegments();

			// CurrentLocation container
			var container = Cosmos.Client.GetContainer(Constants.CosmosDb.DatabaseName, Constants.CosmosDb.CurrentLocationContainerName);
			var ctr = 0;
			try
			{
				await container.DeleteItemAsync<object>(DateTime.Now.Year.ToString(), new PartitionKey("location"));
				ctr++;
			}
			catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
			{
			}			
			Console.WriteLine($"Deleted {ctr} current locations");

			// DeliveryBoard container
			container = Cosmos.Client.GetContainer(Constants.CosmosDb.DatabaseName, Constants.CosmosDb.DeliveryBoardContainerName);
			ctr = 0;
			foreach (var flight in flights)
			{
				try
				{
					await container.DeleteItemAsync<object>(flight.ArrivalCity, new PartitionKey("location"));
					ctr++;
				}
				catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
				{
				}
			}
			Console.WriteLine($"Deleted {ctr} city deliveries");			
		}

		public async Task GenerateData()
		{
			Console.Write($"Press any key to start generating telemetry through flight segment completion, or ESC to cancel... ");

			var key = Console.ReadKey(true);
			if (key.KeyChar == (char)27)
			{
				Console.WriteLine("cancel");
				return;
			}

			Console.Clear();
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine($"Segment From > To  Arrives  Distance  Duration  Remaining           Speed    Altitude   Location");
			Console.WriteLine($"------- ---------- -------  --------  --------  ------------------  -------- ---------  -------------------");

			this._flightSegments = await this._flightRepo.GetFlightSegments();

			var currentLine = Console.CursorTop;
			Console.SetCursorPosition(0, 5 + this._flightSegments.Length);
			Console.Write($"Press space bar to pause, ESC to stop... ");
			Console.SetCursorPosition(0, currentLine);

			this._telemetryCount = 0;
			this._errorCount = 0;
			this._completedFlightSegments = new List<string>();
			this._currentFlightSegment = 100;
			var allFlightsComplete = false;
			var container = Cosmos.Client.GetContainer(Constants.CosmosDb.DatabaseName, Constants.CosmosDb.LocationContainerName);
			var started = DateTime.UtcNow;

			while (true)
			{
				while (!Console.KeyAvailable)
				{
					var elapsed = DateTime.UtcNow.Subtract(started);
					var telemetry = this.GenerateTelemetry();
					if (telemetry.Count == 0)
					{
						allFlightsComplete = true;
						break;
					}
					else
					{
						var errorCount = await this.WriteTelemetry(container, telemetry);
						var rate = Math.Round(this._telemetryCount / elapsed.TotalSeconds, 0);
						this._errorCount += errorCount;
						Console.SetCursorPosition(0, 0);							
						Console.WriteLine($"Flight Segments={this._flightSegments.Length}; Telemetry Count={this._telemetryCount} ({rate}/sec); Errors={this._errorCount}; Elapsed={elapsed}");
						await Task.Delay(TelemetryIntervalMs);
					}
				}

				if (allFlightsComplete)
				{
					break;
				}

				key = Console.ReadKey(true);
				if (key.KeyChar == ' ')
				{
					Console.SetCursorPosition(0, 5 + this._flightSegments.Length);
					Console.Write($"Paused; press any key to resume...       ");
					while (!Console.KeyAvailable) { }
					Console.ReadKey(true);
					Console.SetCursorPosition(0, 5 + this._flightSegments.Length);
					Console.Write($"Press space bar to pause, ESC to stop... ");
				}
				else if (key.KeyChar == (char)27)
				{
					break;
				}
			}

			Console.SetCursorPosition(0, 5 + this._flightSegments.Length);
			Console.WriteLine($"Stopped after generating {this._telemetryCount} telemetry items for {this._flightSegments.Length} flight segments with {this._errorCount} errors");
		}

		private List<LocationEvent> GenerateTelemetry()
		{
			Console.SetCursorPosition(0, 4);
			var list = new List<LocationEvent>();
			var ctr = 0;

            foreach (var flight in this._flightSegments)
            {
				if (this._completedFlightSegments.Contains(flight.RouteNumber))
				{
					Console.WriteLine();
					continue;
				}

				ctr++;
				var arrivalCity = this._cities.First(a => a.Code == flight.ArrivalCity);
				var remainingMiles = (int)this._spatial.CalculateDistance(flight.Latitude, flight.Longitude, arrivalCity.Latitude, arrivalCity.Longitude);
				var remainingMinutes = remainingMiles / Convert.ToDouble(AverageFlightSpeed) * 60;

				if (flight.RouteNumber != _currentFlightSegment.ToString())
				{
					Console.WriteLine($"{flight.RouteNumber,6}  {flight.DepartureCity} > {flight.ArrivalCity} ");

					var line = $"{flight.RouteNumber,6}  {flight.DepartureCity} > {flight.ArrivalCity}  WAITING";
					line += new string(' ', Console.WindowWidth - line.Length - 1);
					Console.SetCursorPosition(0, Console.CursorTop - 1);
					Console.WriteLine(line);

					continue;
				}

				var speedAndAltitude = this.GenerateSpeedAndAltitude(flight.RouteNumber);
				var arrivalTime = DateTime.UtcNow.AddMinutes(remainingMinutes);

				var item = new LocationEvent
				{
					Id = Guid.NewGuid().ToString(),
					Type = "location",
					FlightNumber = DateTime.Today.Year.ToString(),
					RouteNumber = flight.RouteNumber,
					DepartureCity = flight.DepartureCity,
					DepartureCityName = flight.DepartureCityName,
					ArrivalCity = flight.ArrivalCity,
					ArrivalCityName = flight.ArrivalCityName,
					DurationMinutes = flight.DurationMinutes,
					RemainingMinutes = remainingMinutes,
					DistanceMiles = flight.DistanceMiles,
					RemainingMiles = remainingMiles,
					ArrivalTime = arrivalTime,
					Latitude = flight.Latitude,
					Longitude = flight.Longitude,
					Altitude = speedAndAltitude.Altitude,
					Speed = speedAndAltitude.Speed,
					EventDate = DateTimeOffset.UtcNow,
					Ttl = GetDocumentTimeToLive()
				};

				if (remainingMiles < 20)
				{
					item.Speed = 0;
					item.Altitude = 0;
					item.RemainingMiles = 0;
					item.RemainingMinutes = 0;
					item.IsComplete = true;					
				}

				Console.WriteLine($"{item.RouteNumber,6}  {item.DepartureCity} > {item.ArrivalCity}    {flight.ArrivalTime:HH:mm}   {flight.DistanceMiles,4} mi   {Math.Round(flight.DurationMinutes / 60, 2),-4} hr    {remainingMiles,4} mi, {Math.Round(remainingMinutes / 60, 2),-4:0.00} hr   {item.Speed} mph  {item.Altitude} ft {item.Latitude,9:##00.0000}, {item.Longitude,9:##00.0000}   ");
				list.Add(item);
				this._telemetryCount++;

				if (item.IsComplete)
				{
					this._completedFlightSegments.Add(flight.RouteNumber);
					var line = $"{flight.RouteNumber,6}  {flight.DepartureCity} > {flight.ArrivalCity}  WRAPPED UP";
					line += new string(' ', Console.WindowWidth - line.Length - 1);
					Console.SetCursorPosition(0, Console.CursorTop - 1);
					Console.WriteLine(line);

					_currentFlightSegment += 1;

					continue;
				}

				this.NudgeFlight(flight);
			}

			return list;
		}

		private SpeedAltitudeEvent GenerateSpeedAndAltitude(string routeNumber)
		{
			lock (_threadLock)
			{
				if (_speedAndAltitude.TryGetValue(routeNumber, out SpeedAltitudeEvent speedAndAltitude))
				{
					if (DateTime.Now.Subtract(speedAndAltitude.Timestamp).TotalSeconds < 1)
					{
						return speedAndAltitude;
					}
				}

				speedAndAltitude = new SpeedAltitudeEvent
				{
					Speed = this._random.Number(AverageFlightSpeed-100, AverageFlightSpeed+100),
					Altitude = this._random.Number(37000, 38000),
					Timestamp = DateTime.Now,
				};

				if (!_speedAndAltitude.ContainsKey(routeNumber))
				{
					_speedAndAltitude.Add(routeNumber, speedAndAltitude);
				}
				else
				{
					_speedAndAltitude[routeNumber] = speedAndAltitude;
				}

				return speedAndAltitude;
			}
		}

		private void NudgeFlight(FlightSegment flight)
		{
			while (true)
			{
				var subtractLat = this._random.Number(0, 1) == 0;
				var subtractLon = this._random.Number(0, 1) == 0;
				var valueLat = this._random.Number(50, 60) / 1500.0;
				var valueLon = this._random.Number(50, 60) / 1500.0;

				var lat = subtractLat ? flight.Latitude - valueLat : flight.Latitude + valueLat;
				var lon = subtractLon ? flight.Longitude - valueLon : flight.Longitude + valueLon;

				// Get the previous distance to the arrival airport
				var arrivalAirport = this._cities.First(a => a.Code == flight.ArrivalCity);
				var prevRemainingDistance = this._spatial.CalculateDistance(lat, lon, arrivalAirport.Latitude, arrivalAirport.Longitude);

				if (prevRemainingDistance < 10)
				{
					// Complete the flight if we're within 10 mi of our arrival airport
					break;
				}

				// Get the new distance to the arrival airport
				var newRemainingDistance = this._spatial.CalculateDistance(flight.Latitude, flight.Longitude, arrivalAirport.Latitude, arrivalAirport.Longitude);

				if (newRemainingDistance < 20)
				{
					// Restart the flight if we're within 10 mi of our arrival airport
					var departureAirport = this._cities.First(a => a.Code == flight.DepartureCity);
					flight.Latitude = departureAirport.Latitude;
					flight.Longitude = departureAirport.Longitude;
					break;
				}

				// Accept the nudge if the new distance is less than the previous
				if (prevRemainingDistance < newRemainingDistance)
				{
					flight.Latitude = Math.Round(lat, 4);
					flight.Longitude = Math.Round(lon, 4);
					break;
				}
			}
		}

		protected async Task<int> WriteTelemetry(Container container, List<LocationEvent> docs)
		{
			var tasks = new Task[docs.Count];
			var taskCtr = 0;
			var errorCtr = 0;
			foreach (var item in docs)
			{
				tasks[taskCtr] = container
					.CreateItemAsync(item, new PartitionKey(item.Id))
					.ContinueWith(task => errorCtr = this.CatchInsertDocumentError(task, errorCtr));

				taskCtr++;
			}

			await Task.WhenAll(tasks);

			return errorCtr;
		}

		private int CatchInsertDocumentError(Task<ItemResponse<LocationEvent>> task, int errorCtr)
		{
			if (!task.IsCompletedSuccessfully)
			{
				var currentLineNumber = Console.CursorTop;
				Console.SetCursorPosition(0, 5 + this._flightSegments.Length);
				errorCtr++;
				if (task.Exception is AggregateException ae)
				{
					var ctr = 0;
					foreach (var ex in ae.Flatten().InnerExceptions)
					{
						Console.WriteLine($"[{++ctr}] {ex.Message}");
					}
				}
				else
				{
					Console.WriteLine($"Task failed for unknown reasons");
				}
				Console.SetCursorPosition(0, currentLineNumber);
			}

			return errorCtr;
		}

		private int GetDocumentTimeToLive()
		{
			// Number of days to retain flight telemetry
			return 60 * 60 * 24 * TelemetryRetentionDays;
		}

		private bool Confirm(string message)
		{
			Console.WriteLine(message);
			while (true)
			{
				Console.Write("Are you sure (Y/N)? ");
				var input = Console.ReadLine();
				if (input.ToLower().StartsWith("y"))
				{
					return true;
				}
				if (input.ToLower().StartsWith("n"))
				{
					return false;
				}
			}
		}

	}
}
