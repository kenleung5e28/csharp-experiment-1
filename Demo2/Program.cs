static async Task RunLongTask(string name, int delay, CancellationToken token)
{
    Console.WriteLine("Task {0} started.", name);
    await Task.Delay(delay, token);
    Console.WriteLine("Task {0} completed.", name);
}

using var tokenSource = new CancellationTokenSource();
tokenSource.CancelAfter(5000);
List<(string, int)> taskDetails = [
    ("boring", 7000), 
    ("routine", 3000),
    ("exciting", 1000), 
];
Dictionary<string, Task> tasks = taskDetails.ToDictionary(
    k => k.Item1,
    k => RunLongTask(k.Item1, k.Item2, tokenSource.Token)
    );
try
{
    await Task.WhenAll(tasks.Values);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Some tasks are canceled.");
}
var failedTaskNames = from t in tasks 
    where !t.Value.IsCompletedSuccessfully 
    select t.Key;
foreach (string name in failedTaskNames)
{
    Console.WriteLine($"Task {name} failed.");
}