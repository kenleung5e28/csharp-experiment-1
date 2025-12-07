static async Task RunLongTask(CancellationToken token)
{
    Console.WriteLine("The long task task started.");
    await Task.Delay(3000, token);
    Console.WriteLine("The long task completed.");
}

static async Task MustFailTask()
{
    Console.WriteLine("Must fail task started.");
    throw new HttpRequestException("Must fail task: Unable to fetch data.");
    Console.WriteLine("Must fail task completed.");
}

using var tokenSource = new CancellationTokenSource();
tokenSource.CancelAfter(1000);
Task longTask = RunLongTask(tokenSource.Token);
Task mustFailTask = MustFailTask();
Console.WriteLine(longTask.Status);
Console.WriteLine(mustFailTask.Status);
try
{   
    await Task.WhenAll(longTask, mustFailTask);
}
catch (Exception e)
{
    Console.WriteLine(e.GetType().Name);
    Console.WriteLine(e.Message);
}