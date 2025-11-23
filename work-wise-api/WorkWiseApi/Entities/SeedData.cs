using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using CsvHelper;
using System.Globalization;

namespace Entities;

/// <summary>
/// Class <c>SeedData</c> static class for bootstraping the required data 
/// during the startup
/// </summary>
public static class SeedData
{
    /// <summary>
    /// Initialize the database context to bootstrap the data.
    /// </summary>
    /// <param name="serviceProvider">To get the context</param>
    public static void Initialize(IServiceProvider serviceProvider)
    {
        RepositoryContext context = serviceProvider.GetRequiredService<RepositoryContext>();
        _ = context.Database.EnsureCreated();
        try
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Adding User
            string resourceName = "Entities.Migrations.User.csv";
            Stream stream = assembly.GetManifestResourceStream(resourceName);
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                System.Collections.Generic.IEnumerable<Models.User> users = csvReader.GetRecords<Models.User>();
                foreach (Models.User user in users)
                {
                    if (context.Users.Find(user.Id) == null)
                    {
                        _ = context.Users.Add(user);
                    }
                }
                SaveEntities(context);
                csvReader.Dispose();
            }

            // Adding UserSecret
            resourceName = "Entities.Migrations.UserSecret.csv";
            stream = assembly.GetManifestResourceStream(resourceName);
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                System.Collections.Generic.IEnumerable<Models.UserSecret> userSecrets = csvReader.GetRecords<Models.UserSecret>();
                foreach (Models.UserSecret userSecret in userSecrets)
                {
                    if (context.UserSecrets.Find(userSecret.Id) == null)
                    {
                        _ = context.UserSecrets.Add(userSecret);
                    }
                }
                SaveEntities(context);
                csvReader.Dispose();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="repositoryContext"></param>
    public static void SaveEntities(RepositoryContext repositoryContext)
    {
        repositoryContext.OnBeforeSaving(Guid.Parse("d0e96ca8-7a2f-41d0-84b8-0c0298208def"));
        _ = repositoryContext.SaveChanges();
    }
}
