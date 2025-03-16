using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using YemekSepeti.Models;

public class YemekSepetContextFactory : IDesignTimeDbContextFactory<YemekSepetContext>
{
    public YemekSepetContext CreateDbContext(string[] args)
    {
        // appsettings.json dosyasından bağlantı dizesini oku
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<YemekSepetContext>();

        // Bağlantı dizesini kullanarak SQL Server yapılandır
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("YemekSepeti"));

        return new YemekSepetContext(optionsBuilder.Options);
    }
}
