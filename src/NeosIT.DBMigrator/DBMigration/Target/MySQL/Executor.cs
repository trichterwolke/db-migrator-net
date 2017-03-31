using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NeosIT.DBMigrator.DBMigration.Target.MySQL
{
    public class Executor : IExecutor
    {
        private readonly Log log = new Log();

        #region IExecutor Members

        public string Command { get; set; } = "mysql";

        public string Host { get; set; } = "localhost";

        public string Database { get; set; } = "";

        public string Username { get; set; } = "root";

        public string Password { get; set; } = "";

        public string Args { get; set; } = "";

        public string Exec(string cmdArgs)
        {
            log.Debug(string.Format("executing {0} {1}", Command, cmdArgs), "exec");

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
            if (!command.EndsWith(";"))
            {
                command = "\"" + command + ";" + "\"";
            }

            return Exec(BuildExecCommand(command).Aggregate((x, y) => x + " " + y));
        }

        public string ExecFile(string path, bool verbose)
        {
            IList<string> r = BuildExecCommand("", verbose);
            r.Add("-e \"source " + path + "\"");

            return Exec(r.Aggregate((x, y) => x + " " + y));
        }

        public IList<string> BuildExecCommand(string cmd, bool verbose = false)
        {
            IList<string> r = new List<string>();

            if (!string.IsNullOrEmpty(Host))
            {
                r.Add("--host=" + Host);
            }

            if (!string.IsNullOrEmpty(Username))
            {
                r.Add("--user=" + Username);
            }

            r.Add("--password=" + Password);

            if (!string.IsNullOrEmpty(Args))
            {
                r.Add(Args);
            }

            r.Add("--vertical");

            if (verbose)
            {
                r.Add("--verbose");
            }

            if (!string.IsNullOrEmpty(cmd))
            {
                r.Add("--execute=" + cmd);
            }

            if (!string.IsNullOrEmpty(Database))
            {
                r.Add("--database=" + Database);
            }

            return r;
        }

        #endregion
    }
}