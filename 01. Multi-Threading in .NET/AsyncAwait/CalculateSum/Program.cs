Console.WriteLine("Calculate the sum of all numbers from 0 to N");
Console.WriteLine("Enter any integer number");
Console.WriteLine("Press 'q' to quit");

CalculateSum();

// Calculate the sum of integer numbers. Provide the cancellation of the request.
static void CalculateSum()
{
    var line = Console.ReadLine();
    do
    {
        using var cts = new CancellationTokenSource();
        CancellationToken token = cts.Token;

        if (Int32.TryParse(line, out int num))
        {
            // Register a callback method (lambda) that fires immediately on cancellation event.
            using var registration = token.Register(() =>
            {
                Console.WriteLine($"Sum for {num} has been canceled.");
            });

            // Running async CPU-bound code on a separate thread to unblock ReadLine().
            var task = Task.Run(async () =>
            {
                // Passing token instead of cts.Token provides some security since token cannot initiate cancellation.
                var sum = await SumAsync(num, token);
                Console.WriteLine($"Sum for {num} = {sum}");
            });

            Console.WriteLine($"Calculating sum for {num}... Enter a new number to cancel the request");
            line = Console.ReadLine();

            // If the task hasn't finished by the time a new value is entered, cancel the task.
            if (!task.IsCompleted)
            {
                cts.Cancel();
            }
        }
        else
        {
            Console.WriteLine("The value is not valid");
            line = Console.ReadLine();
        }
    } while (line == string.Empty || line?.ToCharArray()[0] != 'q');
}


static async Task<long> SumAsync(int n, CancellationToken token)
{
    long res = await Task.Run(() =>
    {
        long sum = 0;
        for (int i = 0; i <= n; i++)
        {
            token.ThrowIfCancellationRequested();
            sum += i;
        }
        return sum;
    });
    return res;
}