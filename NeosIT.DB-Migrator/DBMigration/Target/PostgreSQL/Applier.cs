namespace NeosIT.DB_Migrator.DBMigration.Target.PostgreSQL
{
    public class Applier : Target.Applier
    {
        public override void AppendBeginTransaction()
        {
            StreamWriter.WriteLine("BEGIN;");
        }

        public override void AppendCommitTransaction()
        {
            StreamWriter.WriteLine("COMMIT;");
        }
    }
}