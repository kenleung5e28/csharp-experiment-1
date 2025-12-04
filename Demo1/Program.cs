List<string> todoIds = ["35", "9"];
using var client = new HttpClient();
client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
using var tokenSource = new CancellationTokenSource();
tokenSource.CancelAfter(5000);
Dictionary<string, Task<string>> fetchTasks = todoIds.ToDictionary(
    id => id, id => client.GetStringAsync($"todos/{id}", tokenSource.Token)
    );
try
{
    await foreach (Task<string> task in Task.WhenEach(fetchTasks.Values))
    {
       // TODO
       
    };
}
catch (HttpRequestException e) {
    Console.WriteLine("Fetch error occured:");
    Console.WriteLine(e.Message);
    Environment.Exit(1);
}