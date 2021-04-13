using System;
using System.IO;

namespace CapWatchBackend.DataAccess.MongoDB {
  static class DbLogger {
    private static readonly TextWriter txtWriter = File.AppendText("..\\Logs\\log.txt");
    public static void Log(string logMessage) {
      txtWriter.Write("\r\nLog Entry : ");
      txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
          DateTime.Now.ToLongDateString());
      txtWriter.WriteLine("  :");
      txtWriter.WriteLine("  :{0}", logMessage);
      txtWriter.WriteLine("-------------------------------");
    }
  }
}
