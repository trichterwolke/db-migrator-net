namespace NeosIT.DB_Migrator.DBMigration.Target.MSSQL
{
    public class Applier : Target.Applier
    {
        public override void AppendBeginTransaction()
        {
            StreamWriter.WriteLine("BEGIN TRANSACTION;");
            StreamWriter.WriteLine("GO");
        }

        public override void AppendCommitTransaction()
        {
            StreamWriter.WriteLine("COMMIT;");
        }

        public override void AfterMigrationFile(Version version, SqlFileInfo file)
        {
            // GO is needed for applying multiple DDL statements in one transaction
            StreamWriter.WriteLine("GO");

            base.AfterMigrationFile(version, file);
            StreamWriter.WriteLine("GO");
        }
    }
}