using System.Collections.Generic;

namespace NeosIT.DBMigrator.DBMigration
{
    public class SqlStacktrace
    {
        public IList<string> Lines { get; set; }
        public SqlFileInfo File { get; set; }
        public int BeginLine { get; set; }
    }
}