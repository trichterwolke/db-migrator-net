namespace NeosIT.DBMigrator.DBMigration.Target
{
    public interface IDbInterface
    {
        IExecutor Executor { get; set; }
        Version FindLatestMigration();
    }
}