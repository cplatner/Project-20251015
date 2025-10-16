using Microsoft.AspNetCore.Hosting;

public static void Main(string[] args)
{
    var certificatePath = Path.GetFullPath(
        Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "tools", "democertificate.pfx"));

    var host = new WebHostBuilder()
        .UseKestrel(options => options.UseHttps(certificatePath, "democertificate"))
        .UseUrls("http://localhost:5000", "https://localhost:5001")
        .UseIISIntegration()
        .UseStartup<Startup>()
        .Build();

    host.Run();
}