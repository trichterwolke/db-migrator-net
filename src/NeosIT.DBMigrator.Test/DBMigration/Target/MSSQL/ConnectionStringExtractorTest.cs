using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeosIT.DBMigrator.DBMigration.Target;
using NeosIT.DBMigrator.DBMigration.Parsers.MSSQL;
using System.Xml;
using System.Xml.XPath;
using System.Data.SqlClient;
using Xunit;
using System.IO;
using NeosIT.DBMigrator.DBMigration.Target.MSSQL;

namespace NeosIT.DBMigrator.Test.DBMigration.Target.MSSQL
{
    
    public class ConnectionStringExtractorTest
    {
        private IExecutor _executor;

        public ConnectionStringExtractorTest()
        {
            _executor = new Executor();
        }

        [Fact]
        public void TestDefaultConnectionString()
        {
            string fixture = new DirectoryInfo("./Fixtures/MSSQL/ConfigWithIntegratedSecurity.xml").FullName;
            XPathDocument doc = new XPathDocument(fixture);
            var nav = doc.CreateNavigator();
            ConnectionStringExtractor cse = new ConnectionStringExtractor(nav, new XmlNamespaceManager(nav.NameTable));
            _executor.Username = "bla";
            _executor.Password = "blub";
            _executor.Database = "blip";

            cse.LoadConnectionStringInto(@"/configuration/connectionStrings/add[1]/@connectionString",  _executor);
            Assert.Equal("fixture1Host", _executor.Host);
            Assert.Equal("fixture1Db", _executor.Database);
            // SSPI
            Assert.Equal("", _executor.Username);
            Assert.Equal("", _executor.Password);
        }

        [Fact]
        public void FindConnectionStringByNameInConfigurationWithComments()
        {
            string fixture = new DirectoryInfo("./Fixtures/MSSQL/ConfigWithComments.xml").FullName;

            XPathDocument doc = new XPathDocument(fixture);
            var nav = doc.CreateNavigator();
            ConnectionStringExtractor cse = new ConnectionStringExtractor(nav, new XmlNamespaceManager(nav.NameTable));
            _executor.Username = "bla";
            _executor.Password = "blub";
            _executor.Database = "blip";
            _executor.Host = "blop";

            cse.LoadConnectionStringInto(@"/configuration/connectionStrings/add[@name='DatabaseConnection']/@connectionString", _executor);
            Assert.Equal("fixture6Host", _executor.Host);
            Assert.Equal("fixture6Db", _executor.Database);
            Assert.Equal("fixture6User", _executor.Username);
            Assert.Equal("fixture6Password", _executor.Password);
        }
    }
}
