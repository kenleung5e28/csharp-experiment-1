using HttpClient client = new();
client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
var fetchTask1 = client.GetAsync("todos/1");
var fetchTask2 = client.GetAsync("todos/8");
try
{
    var results = await Task.WhenAll(fetchTask1, fetchTask2);
    if (results.Any(r => !r.IsSuccessStatusCode))
    {
        Console.WriteLine("Some fetches failed.");
        Environment.Exit(1);
    }
    var responses = await Task.WhenAll(results.Select(
        r => r.Content.ReadAsStringAsync()
        ));
    Console.WriteLine("Fetch results:");
    foreach (var response in responses)
    {
        Console.WriteLine(response);
    }
}
catch (HttpRequestException e) {
    Console.WriteLine("Fetch error occured:");
    Console.WriteLine(e.Message);
    Environment.Exit(1);
}