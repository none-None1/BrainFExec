using System;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.IO;
namespace BrainFexec
{
    internal class Program
    {
        private static string code = "", fn = "";
        static void Main(string[] args)
        {
            switch (args.Length)
            {
                case 1:
                    {
                        code = File.ReadAllText(args[0]);
                        string[] s = args[0].Split('\\');
                        string [] S = s[s.Length - 1].Split('.');
                        fn = S[0] + ".exe";
                        break;
                    }
                case 2:
                    {
                        code = File.ReadAllText(args[0]);
                        fn = args[1];
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Usage:\nBrainFExec <input> [output]\nBrainFuck to EXE compiler, compiles BrainFuck into a Windows executable.\ninput: Input file name\nOutput: Output file name, if this argument is not passed and the input file name is 'a.bf', output file name will be 'a.exe'.");
                        return;
                    }
            }
            CodeDomProvider compiler = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters param = new CompilerParameters();
            param.ReferencedAssemblies.Add("System.dll");
            param.GenerateExecutable = true;
            param.OutputAssembly = fn;
            param.MainClass = "BF";
            param.GenerateInMemory = true;
            string t = code, brainfuck = "using System;\r\npublic class BF{\r\nprivate static int[] tape=new int[1000000];\r\nprivate static int p=0;\r\nstatic void Main(string[] args){\r\n";
            for (int i = 0; i < t.Length; i++)
            {
                if (t[i] == '+')
                {
                    brainfuck += "tape[p]=(tape[p]==255?0:tape[p]+1);";
                }
                if (t[i] == '-')
                {
                    brainfuck += "tape[p]=(tape[p]==0?255:tape[p]-1);";
                }
                if (t[i] == ',')
                {
                    brainfuck += "tape[p]=Console.Read();";
                }
                if (t[i] == '.')
                {
                    brainfuck += "Console.Write(Convert.ToChar(tape[p]));";
                }
                if (t[i] == '>')
                {
                    brainfuck += "p++;";
                }
                if (t[i] == '<')
                {
                    brainfuck += "p--;";
                }
                if (t[i] == '[')
                {
                    brainfuck += "while(tape[p]!=0){";
                }
                if (t[i] == ']')
                {
                    brainfuck += "}";
                }
                brainfuck += "\r\n";
            }
            brainfuck += "}\r\n}";
            CompilerResults result = compiler.CompileAssemblyFromSource(param, brainfuck);
            if (result.Errors.HasErrors)
            {
                Console.WriteLine("Unmatched bracket");
            }
            else
            {
                Console.WriteLine("Done!");
                Console.WriteLine("Successfully compiled to " + fn);
            }
        }
    }
}