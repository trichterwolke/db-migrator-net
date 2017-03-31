using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NeosIT.DBMigrator.DBMigration.Target.PostgreSQL
{
    public class Executor : IExecutor
    {
        private readonly Log log = new Log();

        #region IExecutor Members

        public string Command { get; set; } = "psql";

        public string Host { get; set; } = "localhost";

        public string Database { get; set; } = "";

        public string Username { get; set; } = "postgres";

        public string Password { get; set; } = "";

        public string Args { get; set; } = "";

        public string Exec(string cmdArgs)
        {
            log.Debug(String.Format("executing {0} {1}", Command, cmdArgs), "exec");

            var proc = new Process();
            proc.StartInfo = new ProcessStartInfo(Command);
            proc.StartInfo.Arguments = cmdArgs;
            proc.StartInfo.EnvironmentVariables.Add("PGPASSWORD", Password);

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

        public IList<string> BuildExecCommand(string cmd, bool verbose = false)
        {
            IList<string> r = BuildExecCommandDefault(verbose);
            r.Add("--command=\"" + cmd + "\"");
            return r;
        }

        public string ExecFile(string path, bool verbose = false)
        {
            IList<string> r = BuildExecCommandDefault(verbose);
            r.Add("--file=" + path);

            return Exec(r.Aggregate((x, y) => x + " " + y));
        }

        public string ExecCommand(string command)
        {
            return Exec(BuildExecCommand(command).Aggregate((x, y) => x + " " + y));
        }

        #endregion

        public IList<string> BuildExecCommandDefault(bool verbose = false)
        {
            IList<string> r = new List<string>();
            r.Add("-F;");

            if (!string.IsNullOrEmpty(Host))
            {
                r.Add("--host=" + Host);
            }

            if (!string.IsNullOrEmpty(Username))
            {
                r.Add("--username=" + Username);
            }

            if (!string.IsNullOrEmpty(Password))
            {
                r.Add("-w");
            }

            if (!string.IsNullOrEmpty(Args))
            {
                r.Add(Args);
            }

            if (!string.IsNullOrEmpty(Database))
            {
                r.Add("--dbname=" + Database);
            }

            if (!string.IsNullOrEmpty(Args))
            {
                r.Add(Args);
            }

            //r.Add("--no-align");
            //r.Add("-t");

            return r;
        }
    }
}