using DbUp.Builder;

namespace Amursoft.DbMigrator
{
    public class PostgresDatabase : IDbMigratorDatabase
    {
        public UpgradeEngineBuilder UseDatabase(SupportedDatabases supportedDatabases, string connectionString)
        {
            return supportedDatabases.PostgresqlDatabase(connectionString);
        }

        public UpgradeEngineBuilder JournalTo(UpgradeEngineBuilder builder, string database, string schema, string table)
        {
            if (string.IsNullOrEmpty(schema))
            {
                schema = "public";
            }
            return builder.JournalToPostgresqlTable(schema, table);
        }
    }
}