﻿using System.Text.RegularExpressions;
using Xunit;

namespace NeosIT.DBMigrator.Test.DBMigration.Target.MSSQL
{
    public class DbInterfaceTest
    {
        [Fact]
        public void TestRegExp()
        {
            var s = " 20120820 001      \r";
            Match match = Regex.Match(s, @"\s*(\d*)\s*(\d*)\s*");

            Assert.Equal("20120820", match.Groups[1].ToString());
            Assert.Equal("001", match.Groups[2].ToString());
        }
    }
}
