namespace NeosIT.DBMigrator.DBMigration.Parsers.PostgreSQL
{
    public class Parser : AbstractParser
    {
        public override Migrator InitMigrator(Migrator migrator)
        {
            return Target.Factory.Create("postgresql", migrator);
        }
    }
}