namespace Timeline47.Api.Shared.Util;

public static class Banner
{
    public static void Log(string message)
    {
        Console.WriteLine("****************************************");
        Console.WriteLine($"[Timeline47] {message}");
        Console.WriteLine("****************************************");
    }
}