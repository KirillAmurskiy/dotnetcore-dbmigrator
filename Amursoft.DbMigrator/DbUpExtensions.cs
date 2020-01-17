using DbUp.Builder;
using Org.BouncyCastle.Bcpg.OpenPgp;

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
            IDbMigratorDatabase database, string databaseName, string schema, string table)
        {
            return database.JournalTo(builder, databaseName, schema, table);
        }
    }
}