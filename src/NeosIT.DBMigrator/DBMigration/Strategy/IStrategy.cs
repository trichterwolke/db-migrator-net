using System.Collections.Generic;

namespace NeosIT.DBMigrator.DBMigration.Strategy
{
    public interface IStrategy
    {
        Dictionary<Version, SqlFileInfo> FindUnappliedMigrationsSince(Version version, SqlDirInfo dir,
                                                                      Guard guard = null);
    }
}