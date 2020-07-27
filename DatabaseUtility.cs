using System.Diagnostics;
using System.IO;

public sealed class DatabaseUtility
    {
        public void backup(string file_name,string db_username,string db_password,string file_path,string schema_name)
        {

			var dmpFileName = $@"{file_name}";

			string path = Path.Combine(file_path, dmpFileName) + ".dmp";

			ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "cmd.exe";
            psi.RedirectStandardInput = false;
            psi.RedirectStandardOutput = true;
			var exportString = $@"expdp {db_username}/{db_password}@schema_name schemas={dto.db_username} dumpfile={dmpFileName}.DMP LOGFILE={dmpFileName}.Log DIRECTORY = DUMPS version=10.2.0";
			psi.Arguments = "/K"+exportString;
			psi.UseShellExecute = false;

            Process process = Process.Start(psi);
            process.WaitForExit();
            process.Close();
        }

    }