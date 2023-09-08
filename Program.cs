using System;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace BrainFexec
{
    internal class Program
    {
        private static string code = "", fn = "";
        static void Main(string[] args)
        {
            int len = args.Length;
            bool opt = false;
            if(args.ToList().Exists(x => x == "-o")) {
                len--;
                opt = true;
            }
            switch (len)
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
                        Console.WriteLine("Usage:\nBrainFExec <input> [output] [-o]\nBrainFuck to EXE compiler, compiles BrainFuck into a Windows executable.\ninput: Input file name\noutput: Output file name, if this argument is not passed and the input file name is 'a.bf', output file name will be 'a.exe'.\n-o: Optimize");
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
            if (!opt)
            {
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
            }
            else
            {
                List<char> tempsymbol = new List<char>();
                List<int> tempnum = new List<int>();
                for(int i=0;i<t.Length; i++)
                {
                    char c = t[i];
                    if (c != '+' && c != '-' && c != '>' && c != '<' && c != ',' && c != '.' && c != '[' && c != ']') continue;
                    if (tempnum.Count != 0 && (c == '+' || c == '-' || c == '>' || c == '<') && tempsymbol[tempsymbol.Count - 1] == c)
                    {
                        tempnum[tempnum.Count - 1]++;
                    }
                    else
                    {
                        tempnum.Add(1);
                        tempsymbol.Add(c);
                    }
                }
                for (int i = 0; i < tempsymbol.Count; i++)
                {
                    if (tempsymbol[i] == '+')
                    {
                        brainfuck += "tape[p]=(tape[p]+"+Convert.ToString(tempnum[i])+")%256;";
                    }
                    if (tempsymbol[i] == '-')
                    {
                        brainfuck += "tape[p]=(tape[p]-" + Convert.ToString(tempnum[i]) + "+256)%256;";
                    }
                    if (tempsymbol[i] == ',')
                    {
                        brainfuck += "tape[p]=Console.Read();";
                    }
                    if (tempsymbol[i] == '.')
                    {
                        brainfuck += "Console.Write(Convert.ToChar(tape[p]));";
                    }
                    if (tempsymbol[i] == '>')
                    {
                        brainfuck += "p+=" + Convert.ToString(tempnum[i]) + ";";
                    }
                    if (tempsymbol[i] == '<')
                    {
                        brainfuck += "p-=" + Convert.ToString(tempnum[i]) + ";";
                    }
                    if (tempsymbol[i] == '[')
                    {
                        brainfuck += "while(tape[p]!=0){";
                    }
                    if (tempsymbol[i] == ']')
                    {
                        brainfuck += "}";
                    }
                    brainfuck += "\r\n";
                }
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