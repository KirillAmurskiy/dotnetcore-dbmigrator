using DbUp.Builder;

namespace Amursoft.DbMigrator
{
    public static class DbUpExtensions
    {
        public static UpgradeEngineBuilder UseDatabase(this SupportedDatabases supportedDatabases,
            IDbMigratorDatabase database, string connectionString)
        {
            return database.UseDatabase(supportedDatabases, connectionString);
        }
        
        public static UpgradeEngineBuilder JournalTo(this UpgradeEngineBuilder builder,
            IDbMigratorDatabase database, string schema, string table)
        {
            return database.JournalTo(builder, schema, table);
        }
    }
}