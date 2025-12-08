static async Task ShortTask(CancellationToken token)
{
    Console.WriteLine("Short task started.");
    await Task.Delay(0, token);
    Console.WriteLine("Short task completed.");
}

static async Task LongTask(CancellationToken token)
{
    Console.WriteLine("The long task task started.");
    await Task.Delay(3000, token);
    Console.WriteLine("The long task completed.");
}

static async Task MustFailTask()
{
    await Task.Run(() =>
    {
        Console.WriteLine("Must fail task started.");
        throw new HttpRequestException("Must fail task: Unable to fetch data.");
        Console.WriteLine("Must fail task completed.");
    });
}

using var tokenSource = new CancellationTokenSource();
tokenSource.CancelAfter(1000);
Task longTask = LongTask(tokenSource.Token);
Task mustFailTask = MustFailTask();
Task shortTask = ShortTask(tokenSource.Token);
Dictionary<string, Task> tasks = new (){
    {nameof(longTask), longTask}, 
    {nameof(mustFailTask), mustFailTask}, 
    {nameof(shortTask), shortTask}
};
try
{
    await Task.WhenAll(tasks.Values);
}
catch
{
    foreach (var entry in tasks)
    {
        string key = entry.Key;
        Task task = entry.Value;
        if (task.IsFaulted)
        {
            Console.WriteLine("{0}: faulted - {1}", key, task.Exception.Message);
        }
        else if (task.IsCanceled)
        {
            Console.WriteLine("{0}: canceled", key);
        }
        else if (task.IsCompletedSuccessfully)
        {
            Console.WriteLine("{0}: completed successfully", key);
        }
    }
}