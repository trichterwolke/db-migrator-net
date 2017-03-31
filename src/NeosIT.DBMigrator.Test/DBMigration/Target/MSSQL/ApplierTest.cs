using NeosIT.DBMigrator.DBMigration;
using NeosIT.DBMigrator.DBMigration.Strategy;
using NeosIT.DBMigrator.DBMigration.Target;
using System.Collections.Generic;
using System.IO;
using Xunit;
using DBM = NeosIT.DBMigrator.DBMigration;

namespace NeosIT.DBMigrator.Test.DBMigration.Target.MSSQL
{
    public class ApplierMock : NeosIT.DBMigrator.DBMigration.Target.MSSQL.Applier
    {
        public override void Commit(bool withTransaction)
        {
            // Ignore writing to file
            AppendCommitTransaction();

            StreamWriter.Dispose();
        }
    }


    public class ApplierTest
    {
        private readonly IStrategy _strategy;
        private readonly DBM.Version _version;
        private readonly Guard _guard;

        public ApplierTest()
        {
            _strategy = new Flat();
            _version = new DBM.Version { Major = "20120101", Minor = "001", };
            _guard = new Guard();
        }

        [Fact]
        public void TestUsingOfGo()
        {
            var info =
            new SqlDirInfo
            {
                DirectoryInfo = new DirectoryInfo("./Fixtures/SimpleMigrations/"),
            };

            Dictionary<DBM.Version, SqlFileInfo> r = _strategy.FindUnappliedMigrationsSince(_version, info, _guard);
            Assert.Equal(2, r.Count);

            ApplierMock applier = new ApplierMock();
            applier.Begin(true);
            applier.Prepare(r);
            applier.Commit(true);
            string[] content = File.ReadAllLines(applier.Filename);

            Assert.NotNull(content);
            Assert.True(content.Length > 4);
            Assert.Equal("GO", content[content.Length - 2]);
            applier.Cleanup();
        }
    }
}
