using DbUp.Builder;

namespace Amursoft.DbMigrator
{
    public interface IDbMigratorDatabase
    {
        UpgradeEngineBuilder UseDatabase(SupportedDatabases supportedDatabases, string connectionString);

        UpgradeEngineBuilder JournalTo(UpgradeEngineBuilder builder, string database, string schema, string table);
    }
}