using System;
using System.Threading;

class Program
{
    static void Main()
    {
        var healthChecker = HealthChecker.Instance;

        // Start a background thread to simulate requests being added
        Thread requestSimulator = new Thread(() =>
        {
            var random = new Random();
            while (true)
            {
                // Randomly generate a success or failure
                bool isSuccess = random.NextDouble() > 0.15; // 85% chance of success
                healthChecker.AddRequestResult(isSuccess);

                // Sleep for a random interval between 100ms and 500ms to simulate variable load
                Thread.Sleep(random.Next(100, 500));
            }
        });

        requestSimulator.Start();

        // Main loop to check and print the health status every second
        while (true)
        {
            HealthStatus status = healthChecker.GetCurrentHealthStatus();
            Console.WriteLine($"Current Health Status: {status}");

            // Sleep for a second before checking again
            Thread.Sleep(1000);
        }
    }
}

