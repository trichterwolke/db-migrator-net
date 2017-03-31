using System.Collections.Generic;
using NeosIT.DBMigrator.DBMigration.Target;
using NeosIT.DBMigrator.DBMigration.Target.MSSQL;
using Xunit;

namespace NeosIT.DBMigrator.Test.DBMigration.Target.MSSQL
{
    
    public class ExecutorTest
    {
        private IExecutor _executor;
        public ExecutorTest()
        {
            
            _executor = new Executor();
        }

        [Fact]
        public void TestBuildExecCommandDefaultOpts()
        {
            IList<string> r = _executor.BuildExecCommand("cmd");

            Assert.True(r.Contains("-S localhost"));
            Assert.True(r.Contains("-U Administrator"));
            Assert.True(r.Contains("-Q \"cmd\""));
        }

        [Fact]
        public void TestBuildExecCommandSetCustomOpts()
        {
            _executor.Username = "username";
            _executor.Host = "host";
            _executor.Password = "password";
            _executor.Args = "--ssl-enabled=true";
            IList<string> r = _executor.BuildExecCommand("cmd", true);

            Assert.True(r.Contains("-S host"));
            Assert.True(r.Contains("-U username"));
            Assert.True(r.Contains("-P password"));
            Assert.True(r.Contains("--ssl-enabled=true"));
            Assert.True(r.Contains("-V 10"));
            Assert.True(r.Contains("-Q \"cmd\""));
        }
    }
}