using System.Collections.Generic;
using NeosIT.DBMigrator.DBMigration.Target;
using NeosIT.DBMigrator.DBMigration.Target.MySQL;
using Xunit;

namespace NeosIT.DBMigrator.Test.DBMigration.Target.MySQL
{
    
    public class ExecutorTest
    {
        private readonly IExecutor _executor;

        public ExecutorTest()
        {
            _executor = new Executor();
        }

        [Fact]
        public void BuildExecCommandDefaultOpts()
        {
            IList<string> r = _executor.BuildExecCommand("cmd");

            Assert.True(r.Contains("--host=localhost"));
            Assert.True(r.Contains("--password="));
            Assert.True(r.Contains("--user=root"));
            Assert.True(r.Contains("--vertical"));
            Assert.True(r.Contains("--execute=cmd"));
        }

        [Fact]
        public void BuildExecCommandSetCustomOpts()
        {
            _executor.Username = "username";
            _executor.Host = "host";
            _executor.Password = "password";
            _executor.Args = "--ssl-enabled=true";
            IList<string> r = _executor.BuildExecCommand("cmd", true);

            Assert.True(r.Contains("--host=host"));
            Assert.True(r.Contains("--password=password"));
            Assert.True(r.Contains("--user=username"));
            Assert.True(r.Contains("--ssl-enabled=true"));
            Assert.True(r.Contains("--vertical"));
            Assert.True(r.Contains("--verbose"));
            Assert.True(r.Contains("--execute=cmd"));
        }
    }
}