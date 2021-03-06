﻿using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace NeosIT.DBMigrator.DBMigration.Options
{
    public class DefaultOptions
    {
        [Option("u", "username", HelpText = "Database username")]
        public virtual string Username { get; set; }

        [Option("p", "password", HelpText = "Database password")]
        public virtual string Password { get; set; }

        [Option("c", "command", HelpText = "Path to sqlcmd if not in environment path")]
        public virtual string Command { get; set; }

        [Option("h", "host", HelpText = "Host")]
        public virtual string Host { get; set; }

        [Option("d", "database", HelpText = "Database - can be left empty if default database for user is set")]
        public virtual string Database { get; set; }

        [Option("a", "args", HelpText = "Additional arguments which will be added to sqlcmd command")]
        public virtual string Args { get; set; }

        [Option("S", "suffix", HelpText = "Use only files with this suffix as migration scripts and ignore other resources (default: .sql)")]
        public virtual string Suffix { get; set; }

        [Option("s", "strategy", HelpText = "can be \"flat\" or \"hierarchial\". flat means, that all scripts must be available inside this directory in form of yyyymmdd[-|_]<migration-number>-<name>.suffix. \"hierarchial\" means a directory structure in form of <major>\\<minor>\\<migration-number>[-|_]<name>.suffix")]
        public virtual string Strategy { get; set; }

        [Option("t", "target", HelpText = "Target SQL system (can be MSSQL, MySQL or PostgreSQL", Required = true)]
        public string Target { get; set; }

        [Option("A", "applied-version", HelpText = "Manually set the currently applied version. If set, the current version will not be retrieved from database. Format must be 'yyyymmdd[-|_]:iiii'.")]
        public string AppliedVersion { get; set; }

        [Option("o", "only-simulate", HelpText = "Temporary file will not be deleted and the migrations will not be applied")]
        public bool OnlySimulate { get; set; }

        [Option("k", "keep-temporary-file", HelpText = "Keep temporary file containing all SQL statements")]
        public bool KeepTemporaryFile { get; set; }

        [ValueList(typeof(List<string>), MaximumElements = 1)]
        public IList<string> Directories { get; set; }

        [Option("e", "encoding", HelpText = "Set file enconding; all allowed values for System.Text.Encoding.GetEncoding: utf-8 (default), utf-16, ucs-2,..")]
        public virtual string Encoding { get; set; }

        [HelpOption]
        public string Help()
        {
            var help = new HelpText
            {
                Heading = new HeadingInfo("db-migrator.NET", "Database migration tool for .NET"),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true,
            };
            help.AddPreOptionsLine("");
            help.AddPreOptionsLine("See http://github.com/prunkstar/db-migrator-net or http://github.com/schakko/db-migrator");
            help.AddPreOptionsLine("");
            help.AddPreOptionsLine("Usage: db-migrator.exe --target=[mssql,mysql,postgresql] <options> <migration_definition1>[;<migration_definition2>[;...]]");
            help.AddPreOptionsLine("");
            help.AddPreOptionsLine("The <migration_definition> must have the format:");
            help.AddPreOptionsLine("  path_to_migrations[,<type>[,<append>]]");
            help.AddPreOptionsLine("    <type> can be 'all' or 'latest'. 'all' means that every migration in this directory is applied, 'latest' is only the latest file (determined by filename)");
            help.AddPreOptionsLine("    <append> must be 'true' or 'false'. 'true' means that the migration table will be automatically updated. This is the default. If you choose 'false' your migration script must include an INSERT-statement for the migration table.");
            help.AddOptions(this);
            return help;
        }
    }
}
