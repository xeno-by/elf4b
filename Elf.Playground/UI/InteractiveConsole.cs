using System;
using System.Diagnostics;
using System.Text;
using Elf.Interactive;
using System.Linq;
using Elf.Helpers;

namespace Elf.Playground.UI
{
    public class InteractiveConsole
    {
        public static void Main(String[] args)
        {
            Console.WriteLine("Welcome to Elf interactive!");

            var ei = new ElfInteractive();
            while(true)
            {
                Console.WriteLine();
                var nextCmd = ReadTillDoubleSemic();

                try
                {
                    if (nextCmd.StartsWith("def "))
                    {
                        ei.Load(nextCmd);
                    }
                    else
                    {
                        var er = ei.Eval(nextCmd.Trim());
                        er.SideEffects.Accept();
                        Console.WriteLine(er);
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine();
                }
            }
        }

        private static String ReadTillDoubleSemic()
        {
            Console.Write("> ");
            var sb = new StringBuilder();

            int c;
            while ((c = Console.Read()) != -1)
            {
                sb.Append((char)c);
                if (sb.ToString().EndsWith("exit" + Environment.NewLine)) 
                    Process.GetCurrentProcess().Kill();
                if (sb.ToString().EndsWith(";;" + Environment.NewLine))
                    break;
            }

            Console.WriteLine();
            return sb.Replace(";;", "").ToString();
        }
    }
}
