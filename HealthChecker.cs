using System;
using System.Collections.Generic;
using System.Linq;

public enum HealthStatus
{
    Healthy,
    Down,
    Degraded,
    Partial,
    Uncertain
}

public class HealthChecker
{
    private static readonly object _lock = new object();
    private static HealthChecker _instance;
    private readonly Queue<(bool isSuccess, DateTime timestamp)> _requests = new Queue<(bool, DateTime)>();
    private readonly TimeSpan _windowSize = TimeSpan.FromMinutes(5);
    private readonly double _degradedThreshold = 0.10;

    private HealthChecker() { }

    public static HealthChecker Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new HealthChecker();
                }
                return _instance;
            }
        }
    }

    public void AddRequestResult(bool isSuccess)
    {
        lock (_lock)
        {
            _requests.Enqueue((isSuccess, DateTime.UtcNow));
            RemoveOldRequests();
        }
    }

    public HealthStatus GetCurrentHealthStatus()
    {
        lock (_lock)
        {
            RemoveOldRequests();

            if (_requests.Count == 0)
            {
                return HealthStatus.Uncertain;
            }

            int failedRequests = _requests.Count(r => !r.isSuccess);
            double percentageFailed = failedRequests / (double)_requests.Count;

            if (_requests.All(r => r.isSuccess))
            {
                return HealthStatus.Healthy;
            }
            else if (_requests.All(r => !r.isSuccess))
            {
                return HealthStatus.Down;
            }
            else if (percentageFailed > _degradedThreshold)
            {
                return HealthStatus.Degraded;
            }
            else
            {
                return HealthStatus.Partial;
            }
        }
    }

    private void RemoveOldRequests()
    {
        DateTime currentTime = DateTime.UtcNow;
        while (_requests.Count > 0 && currentTime - _requests.Peek().timestamp > _windowSize)
        {
            _requests.Dequeue();
        }
    }
}
