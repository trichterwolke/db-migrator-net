﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NeosIT.DBMigrator.DBMigration.Target.MSSQL
{
    public class FilterException : Exception
    {
    }

    public class DbInterface : IDbInterface
    {
        private Log log = new Log();
        private const string SqlMajorCol = "major";
        private const string SqlMinorCol = "minor";

        private const string SqlLatestMigration =
            "SELECT TOP 1 " + SqlMajorCol + ", " + SqlMinorCol + " FROM migrations ORDER BY " + SqlMajorCol + " DESC, " +
            SqlMinorCol + " DESC";

        private const string SqlCreateMigration =
            "CREATE TABLE migrations(id INT NOT NULL PRIMARY KEY IDENTITY, installed_on DATETIME NOT NULL DEFAULT getDate(), " +
            SqlMajorCol + " char(8), " + SqlMinorCol + " char(8), filename nvarchar(max))";

        #region IDbInterface Members

        public IExecutor Executor { get; set; }

        public Version FindLatestMigration()
        {
            try
            {
                IList<string> lines = Executor.ExecCommand(SqlLatestMigration).Split(new[] { '\r','\n' }, StringSplitOptions.RemoveEmptyEntries);
                string major = "0";
                string minor = "0";

                if (lines.Count >= 4)
                {
                    string pattern = @"\s*" + SqlMajorCol + @"\s+" + SqlMinorCol;
                    if (!Regex.Match(lines[0], pattern).Success)
                    {
                        throw new FilterException();
                    }

                    Match match = Regex.Match(lines[2], @"\s*(\d*)\s+(\d+)\s+");

                    if (match.Success)
                    {
                        major = match.Groups[1].ToString().Trim();
                        minor = match.Groups[2].ToString().Trim();
                    }
                }

                return new Version {Major = major, Minor = minor};            
            }
            catch (Exception e)
            {
                log.Error(string.Format("could not retrieve latest revision from database: {0}", e.Message));

                if (e is FilterException)
                {
                    throw new Exception("Could not filter output");
                }

                log.Warn("Creating migrations table ...");

                try
                {
                    Executor.ExecCommand(SqlCreateMigration);
                    log.Success("Migrations table successfully created :-)");

                    return FindLatestMigration();
                }
                catch (Exception eCreate)
                {
                    throw new Exception(string.Format("Could not create migrations table: {0}", eCreate.Message));
                }
            }
        }

        #endregion
    }
}