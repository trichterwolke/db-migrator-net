using System.IO;
using NeosIT.DBMigrator.DBMigration;
using Xunit;

namespace NeosIT.DBMigrator.Test.DBMigration
{
    
    public class GuardTest
    {
        private Guard _guard = new Guard();
        
        [Fact]
        public void TestIsMigrationAllowed()
        {
            var current = new Version {Major = "20120307", Minor = "001",};
            var file = new Version {Major = "20120308", Minor = "0001",};

            _guard.Suffix = null;

            Assert.True(_guard.IsMigrationAllowed(new FileInfo("./Fixtures/MySQL/Flat/20000101_001.sql"), current,
                                                    file));

            _guard.Suffix = ".sql";

            Assert.True(_guard.IsMigrationAllowed(new FileInfo("./Fixtures/MySQL/Flat/20000101_001.sql"), current,
                                                    file));
            Assert.False(_guard.IsMigrationAllowed(new FileInfo("./Fixtures/MySQL/Flat/20000101_002.ignored"), current,
                                                     file));
        }
    }
}