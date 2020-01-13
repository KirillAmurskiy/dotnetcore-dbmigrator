namespace Amursoft.DbMigrator
{
    public class MigratorConfig
    {
        public string Server { get; set; } = "localhost";

        public string Port { get; set; } = "5432";


        public bool CreateDb { get; set; } = false;


        public string Creator { get; set; }
        
        public string CreatorPassword { get; set; }
        
        public string CreatorDb { get; set; }
        
        
        public string Migrator { get; set; }
        
        public string MigratorPassword { get; set; }
        
        public string MigratorDb { get; set; }


        public string ScriptsPath { get; set; } = string.Empty;

        
        public bool ShowPII { get; set; } = false;

        public string CreatorToShow => ShowPII ? Creator : "***";
        
        public string CreatorPasswordToShow => ShowPII ? CreatorPassword : "***";
        
        public string MigratorToShow => ShowPII ? Migrator : "***";
        
        public string MigratorPasswordToShow => ShowPII ? MigratorPassword : "***";

    }
}