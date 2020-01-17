using DbUp.Builder;

namespace Amursoft.DbMigrator
{
    public class PostgresDatabase : IDbMigratorDatabase
    {
        public UpgradeEngineBuilder UseDatabase(SupportedDatabases supportedDatabases, string connectionString)
        {
            return supportedDatabases.PostgresqlDatabase(connectionString);
        }

        public UpgradeEngineBuilder JournalTo(UpgradeEngineBuilder builder, string schema, string table)
        {
            return builder.JournalToPostgresqlTable(schema, table);
        }
    }
}