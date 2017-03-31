using System;
using System.Collections.Generic;
using System.Linq;
using NeosIT.DBMigrator.DBMigration;
using NeosIT.DBMigrator.DBMigration.Strategy;
using Xunit;
using Version = NeosIT.DBMigrator.DBMigration.Version;

namespace NeosIT.DBMigrator.Test.DBMigration
{
    
    public class MigratorTest
    {
        private readonly Migrator _migrator;

        public MigratorTest()
        {
            _migrator = new Migrator();
            _migrator.Guard = new Guard();
        }

        [Fact]
        public void TestCreateDirElementHandelsCustomParameters()
        {
            string opts = "./fixtures" + _migrator.SeparatorOpts + "latest" + _migrator.SeparatorOpts + "yes";
            SqlDirInfo r = _migrator.CreateDirElement(opts);

            Assert.True(r.LatestOnly);
            Assert.True(r.SqlInsertMigration);
        }

        [Fact]
        public void TestCreateDirElementHandlesInvalidDir()
        {
            string opts = "invaliddir";

            try
            {
                SqlDirInfo r = _migrator.CreateDirElement(opts);
                Assert.True(false);
            }
            catch (Exception e)
            {
                Assert.True(e.Message.Contains("is not valid"));
            }
        }

        [Fact]
        public void TestCreateDirElementHandlesParameterDefaults()
        {
            string opts = "./Fixtures" + _migrator.SeparatorOpts + "bla" + _migrator.SeparatorOpts + "no";
            SqlDirInfo r = _migrator.CreateDirElement(opts);

            Assert.True(false == r.LatestOnly);
            Assert.True(null == r.Files);
            Assert.True(false == r.SqlInsertMigration);
        }

        [Fact]
        public void TestGetSqlStacktraceWithoutMagicTag()
        {
            string sep = Environment.NewLine;
            SqlStacktrace r = _migrator.GetSqlStacktrace(new List<string> {"hello", "world", "iam", "evil", "jared"}, 3,
                                                         2, 0);

            Assert.Equal(new List<string> {"world", "iam", "evil"}, r.Lines);
            Assert.Null(r.File);
        }

        [Fact]
        public void TestMergeMigrationsFromDirectoriesCorrectOrderLatestOnly()
        {
            IList<SqlDirInfo> pathdef = new List<SqlDirInfo>
                                            {
                                                _migrator.CreateDirElement("./Fixtures/MergeDirectories/Unittest"),
                                                _migrator.CreateDirElement("./Fixtures/MergeDirectories/Coredata"),
                                                _migrator.CreateDirElement(
                                                    "./Fixtures/MergeDirectories/Migrations,latest,true"),
                                            };
            _migrator.Strategy = new Flat();
            Dictionary<Version, SqlFileInfo> r = _migrator.MergeMigrationsFromDirectories(pathdef, new Version());

            Assert.Equal(4, r.Count);
            Assert.False(r.Keys.Any(x => x.GetVersion() == 201201010001));
            Assert.False(r.Keys.Any(x => x.GetVersion() == 201201010002));
            Assert.False(r.Keys.Any(x => x.GetVersion() == 201201040001));

            var v = r.Keys.Single(x => x.GetVersion() == 201206060001);
            Assert.True(r[v].FileInfo.Name.EndsWith("conflict.sql"));
        }

        [Fact]
        public void TestMergeMigrationsFromDirectory()
        {
            IList<SqlDirInfo> pathdef = new List<SqlDirInfo>
                                            {
                                                _migrator.CreateDirElement("./Fixtures/MergeDirectories/Unittest"),
                                                _migrator.CreateDirElement("./Fixtures/MergeDirectories/Coredata"),
                                                _migrator.CreateDirElement("./Fixtures/MergeDirectories/Migrations"),
                                            };
            _migrator.Strategy = new Flat();
            Dictionary<Version, SqlFileInfo> r = _migrator.MergeMigrationsFromDirectories(pathdef, new Version());

            Assert.Equal(7, r.Count);
            var v = r.Keys.Single(x => x.GetVersion() == 201206060001);
            Assert.True(r[v].FileInfo.Name.EndsWith("conflict.sql"));
        }
    }
}