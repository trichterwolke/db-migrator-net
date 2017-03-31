using System.Collections.Generic;
using NeosIT.DBMigrator.DBMigration.Target;
using NeosIT.DBMigrator.DBMigration.Target.PostgreSQL;
using Xunit;

namespace NeosIT.DBMigrator.Test.DBMigration.Target.PostgreSQL
{
    
    public class ExecutorTest
    {
        private readonly IExecutor _executor;

        public ExecutorTest()
        {
            _executor = new Executor();
        }

        [Fact]
        public void TestBuildExecCommandDefaultOpts()
        {
            IList<string> r = _executor.BuildExecCommand("cmd");

            Assert.True(r.Contains("--host=localhost"));
            Assert.True(r.Contains("--username=postgres"));
            /*Assert.True(r.Contains("--no-align"));
            Assert.True(r.Contains("-t"));*/
            Assert.True(r.Contains("--command=\"cmd\""));
        }

        [Fact]
        public void TestBuildExecCommandSetCustomOpts()
        {
            _executor.Username = "username";
            _executor.Host = "host";
            _executor.Password = "password";
            _executor.Args = "--ssl-enabled=true";
            IList<string> r = _executor.BuildExecCommand("cmd", true);

            Assert.True(r.Contains("--host=host"));
            Assert.True(r.Contains("--username=username"));
            //Assert.True(r.Contains("-P password"));
            Assert.True(r.Contains("--ssl-enabled=true"));
            Assert.True(r.Contains("--command=\"cmd\""));
        }
    }
}