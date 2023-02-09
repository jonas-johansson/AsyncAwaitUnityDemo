using System.Threading.Tasks;

class RandomDelay
{
    public static async Task Wait()
    {
        var random = new System.Random();
        await Task.Delay(random.Next(1000, 2000));
    }
}