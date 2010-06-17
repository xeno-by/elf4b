using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Linq;

namespace Esath.Eval.Ver3.Async
{
    public class MTLog
    {
        private static string logDir = Assembly.GetEntryAssembly() == null ? null :
            Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\mtlog\";

        private static MTLog _current = new MTLog();
        private MTLog()
        {
            lock (SyncRoot)
            {
                if (Directory.Exists(logDir))
                {
                    foreach (var dir in Directory.GetDirectories(logDir)) Directory.Delete(dir, true);
                    foreach (var file in Directory.GetFiles(logDir)) File.Delete(file);
                }
            }
        }

        private Object SyncRoot = new Object();
        private void SayImpl(String msg)
        {
            lock (SyncRoot)
            {
                if (Directory.Exists(logDir))
                {
                    var ct = Thread.CurrentThread;
                    var fullNameOfThread = String.Format("{0} (id={1})", ct.Name ?? "null", ct.ManagedThreadId);
                    var fname = new String((logDir + fullNameOfThread).TakeWhile((c, i) => i < 150).ToArray());

                    if (!File.Exists(fname))
                    {
                        File.WriteAllText(fname, "Thread: " + fullNameOfThread + Environment.NewLine + Environment.NewLine);
                    }

                    using (var sw = new StreamWriter(fname, true))
                    {
                        var now = DateTime.Now;
                        var nows = now.ToString("HH:mm:ss.ffff");
                        sw.WriteLine("[{0}] {1}", nows, msg);
                        foreach (var frame in new StackTrace(2, true).GetFrames())
                        {
                            var asm = frame.GetMethod().DeclaringType.Assembly;
                            var name = asm.GetName().ToString();
                            if (name.StartsWith("iasto") || name.StartsWith("Elf.") || name.StartsWith("Esath."))
                            {
                                var tos = frame.GetMethod().DeclaringType.FullName + "::" + frame.GetMethod().Name;
                                tos += (" in " + frame.GetFileName() + ":line " + frame.GetFileLineNumber());
                                sw.WriteLine("    " + tos);
                            }
                        }
                        sw.WriteLine();
                    }
                }
            }
        }

        [Conditional("TRACE")]
        public static void Say(String msg)
        {
            _current.SayImpl(msg);
        }
    }
}