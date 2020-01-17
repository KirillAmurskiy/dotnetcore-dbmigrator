using DbUp.Builder;
using DbUp.MySql;

namespace Amursoft.DbMigrator
{
    public class MySqlDatabase : IDbMigratorDatabase
    {
        public UpgradeEngineBuilder UseDatabase(SupportedDatabases supportedDatabases, string connectionString)
        {
            return supportedDatabases.MySqlDatabase(connectionString);
        }

        public UpgradeEngineBuilder JournalTo(UpgradeEngineBuilder builder, string database, string schema, string table)
        {
            builder.Configure(c => c.Journal = new MySqlTableJournal(() => c.ConnectionManager, () => c.Log, database, table));
            return builder;
        }
    }
}