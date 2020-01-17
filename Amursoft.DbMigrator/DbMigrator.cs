using System.IO;
using System.Linq;
using DbUp;
using DbUp.Engine;
using DbUp.Engine.Output;
using DbUp.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Amursoft.DbMigrator
{
    public class DbMigrator
    {
        public static DbMigrator MakePostgres(MigratorConfig cnfg, ILogger logger = null)
        {
            return new DbMigrator(cnfg, new PostgresDatabase(), logger);
        }
        
        public static DbMigrator MakeMySql(MigratorConfig cnfg, ILogger logger = null)
        {
            return new DbMigrator(cnfg, new MySqlDatabase(), logger);
        }
        
        private readonly ILogger logger;
        private readonly IUpgradeLog upgradeLog;

        private readonly MigratorConfig cnfg;
        private readonly IDbMigratorDatabase database;

        private readonly bool createDb;
        
        private readonly string scriptsPath;

        public string CreateDbConnection { get; }
        public string CreateRolesConnection { get; }
        public string MigrateDbConnection { get; }
        
        public DbMigrator(MigratorConfig cnfg, IDbMigratorDatabase database, ILogger logger = null)
        {
            if (logger == null)
            {
                logger = NullLogger.Instance;
            }
            
            this.cnfg = cnfg;
            this.database = database;

            this.logger = logger;
            upgradeLog = new UpgradeLog(logger);

            
            createDb = cnfg.CreateDb;
            CreateDbConnection = MakeConnectionString(cnfg.Server, cnfg.Port, cnfg.Creator, cnfg.CreatorPassword, cnfg.CreatorDb);
            CreateRolesConnection = MakeConnectionString(cnfg.Server, cnfg.Port, cnfg.Creator, cnfg.CreatorPassword, cnfg.MigratorDb);
            MigrateDbConnection = MakeConnectionString(cnfg.Server, cnfg.Port, cnfg.Migrator, cnfg.MigratorPassword, cnfg.MigratorDb);
            scriptsPath = cnfg.ScriptsPath;
        }

        private string MakeConnectionString(string server, string port, string user, string password, string database)
        {
            return $"Server={server};Port={port};User Id={user}; Password={password};Database={database};";
        }
        
        public DatabaseUpgradeResult Migrate()
        {
            LogConfig();
            CreateDbIfNeed();
            CreateRolesIfNeed();
            return ApplyMigrations();
        }

        public void LogConfig()
        {
            logger.LogInformation($"Database migration configuration:\n" +
                               $"Server={cnfg.Server}\n" +
                               $"Port={cnfg.Port}\n" +
                               $"CreateDb={createDb}\n" +
                               $"Creator={cnfg.CreatorToShow}\n" +
                               $"CreatorPassword={cnfg.CreatorPasswordToShow}\n" +
                               $"CreatorDb={cnfg.CreatorDb}\n" +
                               $"Migrator={cnfg.MigratorToShow}\n" +
                               $"MigratorPassword={cnfg.MigratorPasswordToShow}\n" +
                               $"MigratorDb={cnfg.MigratorDb}\n" +
                               $"ScriptsPath={scriptsPath}");
        }

        private DatabaseUpgradeResult Success()
        {
            return new DatabaseUpgradeResult(Enumerable.Empty<SqlScript>(), true, null);
        }

        public DatabaseUpgradeResult CreateDbIfNeed()
        {
            if (!createDb) return Success();

            logger.LogInformation($"Start executing creation database...");
            var createDbScriptsPath = Path.Combine(scriptsPath, "CreateDb");
            var upgrader =
                DeployChanges.To
                    .UseDatabase(database, CreateDbConnection)
                    .WithScriptsFromFileSystem(createDbScriptsPath, s => s.EndsWith($"CreateDb.sql"))
                    .LogTo(upgradeLog)
                    .JournalTo(new NullJournal())
                    .Build();

            return upgrader.PerformUpgrade();
        }

        public DatabaseUpgradeResult CreateRolesIfNeed()
        {
            if (!createDb) return Success();

            logger.LogInformation($"Start executing creation roles...");
            var createDbScriptsPath = Path.Combine(scriptsPath, "CreateDb");
            var upgrader =
                DeployChanges.To
                    .UseDatabase(database, CreateRolesConnection)
                    .WithScriptsFromFileSystem(createDbScriptsPath, s => s.EndsWith($"CreateRoles.sql"))
                    .LogTo(upgradeLog)
                    .JournalTo(new NullJournal())
                    .Build();

            return upgrader.PerformUpgrade();
        }

        public DatabaseUpgradeResult ApplyMigrations()
        {
            logger.LogInformation($"Start executing migration scripts...");
            var migrationScriptsPath = Path.Combine(scriptsPath, "Migrations");
            var upgrader =
                DeployChanges.To
                    .UseDatabase(database, MigrateDbConnection)
                    .WithScriptsFromFileSystem(migrationScriptsPath)
                    .LogTo(upgradeLog)
                    .JournalTo(database, cnfg.MigratorDb, "migrations_journal")
                    .Build();

            return upgrader.PerformUpgrade();
        }
    }
}