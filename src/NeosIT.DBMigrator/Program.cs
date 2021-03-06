﻿using System;
using NeosIT.DBMigrator.DBMigration;
using Factory = NeosIT.DBMigrator.DBMigration.Parsers.Factory;
using NeosIT.DBMigrator.DBMigration.Parsers;
using NeosIT.DBMigrator.DBMigration.Options;

namespace NeosIT.DBMigrator
{
    public static class Program
    {
        private static bool isUnitTest;

        internal static bool IsUnitTest => isUnitTest;

        internal static void Main(string[] args, bool isUnitTest)
        {
            Program.isUnitTest = isUnitTest;
            Main(args);
        }

        public static void Main(string[] args)
        {
            Log log = new Log();

            Migrator migrator = null;
            AbstractParser parser = null;

            try
            {
                parser = Factory.Create(args);
            }
            catch (Exception ex)
            {
                Console.Write(GetHelp(parser));
                log.Error("Could not continue: " + ex.Message);

                if (isUnitTest)
                {
                    throw;
                }
                Exit(1);
            }

            try
            {
                migrator = parser.Parse(args, new Migrator());
            }
            catch (Exception ex)
            {
                Console.Write(GetHelp(parser));
                log.Error("Error: " + ex.Message);

                if (isUnitTest)
                {
                    throw;
                }
                Exit(1);
            }

            if (migrator != null)
            {
                migrator.Run();
            }

            Exit(Environment.ExitCode);
        }

        private static string GetHelp(AbstractParser parser)
        {
            if (parser != null && parser.CurrentOptions != null)
            {
                return parser.CurrentOptions.Help();
            }

            return new DefaultOptions().Help();
        }


        private static void Exit(int code)
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Gray;

            Environment.Exit(code);


        }
    }
}
