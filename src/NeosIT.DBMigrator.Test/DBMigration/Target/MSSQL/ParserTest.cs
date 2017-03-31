using NeosIT.DBMigrator.DBMigration;
using NeosIT.DBMigrator.DBMigration.Parsers;
using NeosIT.DBMigrator.DBMigration.Parsers.MSSQL;
using NeosIT.DBMigrator.DBMigration.Strategy;
using System.IO;
using Xunit;

namespace NeosIT.DBMigrator.Test.DBMigration.Target.MSSQL
{
    public class ParserTest
    {
        private readonly AbstractParser _parser;

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
                            "--username=user", "--database=database", "--command=mssql-custom-command",
                            "--password=custom-password", "--host=remote-host",
                            "--args=--custom-mssql-args=bla", "--suffix=.bla", "--strategy=hierarchial",
                            "c:/temp,latest,true;", "--target=mssql"
                        },
                    new Migrator());

            Assert.Equal("c:/temp" + r.SeparatorOpts + "latest" + r.SeparatorOpts + "true;", r.Directories);
            Assert.Equal("user", r.DbInterface.Executor.Username);
            Assert.Equal("custom-password", r.DbInterface.Executor.Password);
            Assert.Equal("mssql-custom-command", r.DbInterface.Executor.Command);
            Assert.Equal("database", r.DbInterface.Executor.Database);
            Assert.IsType<Hierarchial>(r.Strategy);
            Assert.Equal("--custom-mssql-args=bla", r.DbInterface.Executor.Args);
            Assert.IsType<Guard>(r.Guard);
            Assert.Equal(".bla", r.Guard.Suffix);
        }

        [Fact]
        public void TestParseDefaultSettings()
        {
            Migrator r = _parser.Parse(new[] { "--username=user", "--database=database", "--password=password", "--target=mssql" },
                                       new Migrator());

            Assert.Equal("." + r.SeparatorOpts + "all" + r.SeparatorOpts + "false", r.Directories);
            Assert.Equal("user", r.DbInterface.Executor.Username);
            Assert.Equal("password", r.DbInterface.Executor.Password);
            Assert.Equal("sqlcmd", r.DbInterface.Executor.Command);
            Assert.Equal("database", r.DbInterface.Executor.Database);
            Assert.IsType<Flat>(r.Strategy);
            Assert.Equal("", r.DbInterface.Executor.Args);
            Assert.IsType<Guard>(r.Guard);
            Assert.Equal(".sql", r.Guard.Suffix);
        }

        [Fact]
        public void TestParseXmlConfigurationFile()
        {
            string path = new DirectoryInfo("./Fixtures/MSSQL/ConfigWithUsernamePassword.xml").FullName;
            Migrator r = _parser.Parse(new[] { "--username=overwrite", "--password=overwrite", "--config='" + path + "'", "--target=mssql" }, new Migrator());

            Assert.Equal("fixture2User", r.DbInterface.Executor.Username);
            Assert.Equal("fixture2Password", r.DbInterface.Executor.Password);
            Assert.Equal("fixture2Host", r.DbInterface.Executor.Host);
            Assert.Equal("fixture2Db", r.DbInterface.Executor.Database);
        }

        [Fact]
        public void TestParseXpathExpression()
        {
            string path = new DirectoryInfo("./Fixtures/MSSQL/ConfigWithUnityInjection.xml").FullName;
            Migrator r = _parser.Parse(new[] { "--username=overwrite", 
                "--password=overwrite", 
                "--config='" + path + "'",
                "--xpath='/configuration/unity/container/register[@type=\"IDalFactory\"]/constructor/param/value/@value'",
                "--target=mssql" }, new Migrator());

            Assert.Equal("fixture3User", r.DbInterface.Executor.Username);
            Assert.Equal("fixture3Password", r.DbInterface.Executor.Password);
            Assert.Equal("fixture3Host", r.DbInterface.Executor.Host);
            Assert.Equal("fixture3Db", r.DbInterface.Executor.Database);
        }

        [Fact]
        public void TestParseXpathExressionWithNamespaces()
        {
            string path = new DirectoryInfo("./Fixtures/MSSQL/ConfigWithUnityInjectionAndNamespaces.xml").FullName;
            Migrator r = _parser.Parse(new[] { "--username=overwrite", 
                "--password=overwrite", 
                "--config='" + path + "'",
                "--namespaces='ns1=http://schemas.microsoft.com/practices/2010/unity'",
                "--xpath='/configuration/ns1:unity/ns1:container/ns1:register[@type=\"IDalFactory\"]/ns1:constructor/ns1:param/ns1:value/@value'",
                "--target=mssql" }, new Migrator());

            Assert.Equal("fixture4User", r.DbInterface.Executor.Username);
            Assert.Equal("fixture4Password", r.DbInterface.Executor.Password);
            Assert.Equal("fixture4Host", r.DbInterface.Executor.Host);
            Assert.Equal("fixture4Db", r.DbInterface.Executor.Database);
        }
    }
}