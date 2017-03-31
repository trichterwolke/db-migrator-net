using NeosIT.DBMigrator.DBMigration;
using NeosIT.DBMigrator.DBMigration.Parsers;
using NeosIT.DBMigrator.DBMigration.Parsers.PostgreSQL;
using NeosIT.DBMigrator.DBMigration.Strategy;
using NeosIT.DBMigrator.DBMigration.Target.PostgreSQL;
using Xunit;

namespace NeosIT.DBMigrator.Test.DBMigration.Target.PostgreSQL
{
    
    public class ParserTest
    {
        private AbstractParser _parser;
        public ParserTest()
        {
            
            _parser = new Parser();
        }

        [Fact]
        public void TestParseCustomSettings()
        {
            Migrator r =
                _parser.Parse(
                    new[]
                        {
                            "--username=user", "--database=database", "--command=postgresql-custom-command",
                            "--password=custom-password", "--host=remote-host",
                            "--args=--custom-postgresql-args=bla", "--suffix=.bla", "--strategy=hierarchial",
                            "c:/temp,latest,true;", "--target=postgresql"
                        },
                    new Migrator());

            Assert.Equal("c:/temp" + r.SeparatorOpts + "latest" + r.SeparatorOpts + "true;", r.Directories);
            Assert.Equal("user", r.DbInterface.Executor.Username);
            Assert.Equal("custom-password", r.DbInterface.Executor.Password);
            Assert.Equal("postgresql-custom-command", r.DbInterface.Executor.Command);
            Assert.Equal("database", r.DbInterface.Executor.Database);
            Assert.IsType<Hierarchial>(r.Strategy);
            Assert.Equal("--custom-postgresql-args=bla", r.DbInterface.Executor.Args);
            Assert.IsType<Guard>(r.Guard);
            Assert.Equal(".bla", r.Guard.Suffix);
        }

        [Fact]
        public void TestParseDefaultSettings()
        {
            Migrator r = _parser.Parse(new[] { "--username=user", "--database=database", "--target=postgresql" },
                                       new Migrator());

            Assert.Equal("." + r.SeparatorOpts + "all" + r.SeparatorOpts + "false", r.Directories);
            Assert.Equal("user", r.DbInterface.Executor.Username);
            Assert.Equal("", r.DbInterface.Executor.Password);
            Assert.Equal("psql", r.DbInterface.Executor.Command);
            Assert.Equal("database", r.DbInterface.Executor.Database);
            Assert.IsType<Flat>(r.Strategy);
            Assert.Equal("", r.DbInterface.Executor.Args);
            Assert.IsType<Guard>(r.Guard);
            Assert.Equal(".sql", r.Guard.Suffix);
        }
    }
}