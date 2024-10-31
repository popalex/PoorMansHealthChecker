# PoorMansHealthChecker

A straightforward, no-frills health-checking tool

# Features

- Sliding Window Analysis: Keeps track of service request statuses within a configurable time window.

- Health States:
    - Healthy: All requests in the current window were successful.
    - Degraded: Failed requests exceed a set threshold (10% by default).
    - Down: All requests in the current window have failed.
    - Partial: A mix of success and failure requests, but not degraded.
    - Uncertain: No requests in the current window to evaluate.

# Usage

Instantiate the Health Checker: Access the singleton instance with `HealthChecker.Instance`.

Add Request Results: Use `AddRequestResult(bool isSuccess)` to add request outcomes. Pass `true` for successful requests and `false` for failures.

Check Health Status: Use `GetCurrentHealthStatus()` to get the current health state.

Example:

```csharp
var healthChecker = HealthChecker.Instance;

// Simulate some requests
healthChecker.AddRequestResult(true);  // Success
healthChecker.AddRequestResult(false); // Failure
healthChecker.AddRequestResult(true);  // Success

// Check the current health status
Console.WriteLine($"Current Health Status: {healthChecker.GetCurrentHealthStatus()}");
```

# Configuration

**Degraded Threshold**: The threshold for `Degraded` status is set to **10%** by default. Adjust this in HealthChecker as needed.

# Contributing

Contributions are welcome! Please open an issue or submit a pull request for any feature suggestions, bug fixes, or improvements.

# License

This project is licensed under the MIT License.