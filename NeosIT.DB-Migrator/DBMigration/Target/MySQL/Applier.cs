namespace NeosIT.DB_Migrator.DBMigration.Target.MySQL
{
    public class Applier : Target.Applier
    {
        public override void AppendBeginTransaction()
        {
            StreamWriter.WriteLine("SET autocommit=0;");
            StreamWriter.WriteLine("START TRANSACTION;");
        }

        public override void AppendCommitTransaction()
        {
            StreamWriter.WriteLine("COMMIT;");
        }
    }
}