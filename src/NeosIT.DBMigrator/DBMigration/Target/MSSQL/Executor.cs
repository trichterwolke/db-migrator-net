using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NeosIT.DBMigrator.DBMigration.Target.MSSQL
{
    public class Executor : IExecutor
    {
        private readonly Log log = new Log();

        #region IExecutor Members

        public string Command { get; set; } = "sqlcmd";

        public string Host { get; set; } = "localhost";

        public string Database { get; set; } = "";

        public string Username { get; set; } = "Administrator";

        public string Password { get; set; } = "";

        public string Args { get; set; } = "";

        public string Exec(string cmdArgs)
        {
            log.Debug(String.Format("executing {0} {1}", Command, cmdArgs), "exec");
            var proc = new Process();
            proc.StartInfo = new ProcessStartInfo(Command);
            proc.StartInfo.Arguments = cmdArgs;

            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardOutput = true;

            proc.Start();

            string text = proc.StandardOutput.ReadToEnd();
            string err = proc.StandardError.ReadToEnd();

            proc.WaitForExit(15);

            if (0 != proc.ExitCode)
            {
                throw !string.IsNullOrEmpty(err)
                          ? new Exception(err)
                          : new Exception(
                                "Command did not exit normal but although did not return any error text. Is the executed command correct? Normal text stream follows:\n" +
                                text);
            }

            return text;
        }

        public string ExecCommand(string command)
        {
            return Exec(BuildExecCommand(command).Aggregate((x, y) => x + " " + y));
        }

        public string ExecFile(string path, bool verbose = false)
        {
            IList<string> r = BuildExecCommandDefault(verbose);
            r.Add("-i " + path);
            return Exec(r.Aggregate((x, y) => x + " " + y));
        }

        public IList<string> BuildExecCommand(string cmd, bool verbose = false)
        {
            IList<string> r = BuildExecCommandDefault(verbose);
            r.Add("-Q \"" + cmd + "\"");
            return r;
        }

        #endregion

        public IList<string> BuildExecCommandDefault(bool verbose = false)
        {
            IList<string> r = new List<string>();

            // parameter -b is needed for retrieving the exit code. Otherwise sqlcmd returns always 0
            r.Add("-b");

            if (!string.IsNullOrEmpty(Host))
            {
                r.Add("-S " + Host);
            }

            if (!string.IsNullOrEmpty(Username))
            {
                r.Add("-U " + Username);

                if (!string.IsNullOrEmpty(Password))
                {
                    r.Add("-P " + Password);
                }
            }
            else
            {
                r.Add("-E");
            }

            r.Add(Args);

            if (verbose)
            {
                r.Add("-V 10");
            }

            if (!string.IsNullOrEmpty(Database))
            {
                r.Add("-d " + Database);
            }

            return r;
        }
    }
}