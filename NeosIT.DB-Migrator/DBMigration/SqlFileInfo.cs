using System.IO;

namespace NeosIT.DB_Migrator.DBMigration
{
    public class SqlFileInfo
    {
        public FileInfo FileInfo { get; set; }
        public bool SqlInsertMigration { get; set; }
        public bool NoTransaction { get { return FileInfo?.Name.EndsWith("notran.sql") ?? false; } }
    }
}