namespace BlazorChat.Models
{
    public class UserDatabaseSettings : IDatabaseSettings
    {
        public string UsersCollectionName { get; set; }
        public string AccountsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IDatabaseSettings
    {
        string UsersCollectionName { get; set; }
        string AccountsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}