using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using Mono.Cecil.Cil;

namespace DMT
{
    public static class Extensions
    {

        public static string GetNext(this string[] args, string find)
        {
            if (args == null || find == null) return String.Empty;

            for (int x = 0; x < args.Length - 1; x++)
            {
                if (args[x].EqualsIgnoreCase(find))
                {
                    return args[x + 1];
                }
            }

            return String.Empty;
        }

        public static string GetNext(this string[] args, int index)
        {
            if (args == null) return String.Empty;

            if (index + 1 < args.Length)
                return args[index + 1];

            return String.Empty;

        }

        public static string GetValue(this XmlElement ele)
        {
            return ele?.InnerText?.Trim() ?? String.Empty;
        }
        public static string GetElementValue(this XmlElement ele, string name)
        {
            foreach (var e in ele.ChildNodes)
            {
                var element = e as XmlElement;
                if (element != null)
                {
                    if (element.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                        return element.GetValue();
                }
            }
            return String.Empty;
        }
        public static XmlElement GetElement(this XmlElement ele, string name)
        {
            foreach(var e in ele.ChildNodes)
            {
                var element = e as XmlElement;
                if (element != null)
                {
                    if (element.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                        return element;
                }
            }

            return null;
        }

        public static bool EqualsIgnoreCase(this string s, string find)
        {
            if (s == null || find == null) return false;
            return s.IndexOf(find, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static string FolderFormat(this string s)
        {

            if (s == null) return String.Empty;

            if (s.EndsWith("\\"))
            {
                s = s.Substring(0, s.Length - 1);
                s += "/";
            }

            if (!s.EndsWith("/"))
                s += "/";

            return s;
        }


        public static int GetValueAsInt(this Instruction i)
        {

            if (i.OpCode == OpCodes.Ldc_I4_0)
                return 0;
            if (i.OpCode == OpCodes.Ldc_I4_1)
                return 1;
            if (i.OpCode == OpCodes.Ldc_I4_2)
                return 2;
            
            if (i.OpCode == OpCodes.Ldc_I4_S)
                return int.Parse(i.Operand.ToString());

            if (i.OpCode == OpCodes.Ldc_I4)
                return int.Parse(i.Operand.ToString());

            if (i.OpCode == OpCodes.Ldc_I4_M1)
                return -1;

            if (i.OpCode == OpCodes.Ldc_I4_3)
                return 3;
            if (i.OpCode == OpCodes.Ldc_I4_4)
                return 4;
            if (i.OpCode == OpCodes.Ldc_I4_5)
                return 5;
            if (i.OpCode == OpCodes.Ldc_I4_6)
                return 6;
            if (i.OpCode == OpCodes.Ldc_I4_7)
                return 7;
            if (i.OpCode == OpCodes.Ldc_I4_8)
                return 8;

            throw new NotImplementedException("Instruction is not an i4 opcode");
        }

        public static T[] GetInterfaceImplementers<T>(this Assembly asm) where T : class
        {
            return asm.GetInheritors(typeof(T)).Select(d => Activator.CreateInstance(d) as T).ToArray();
        }

        public static Instruction GetNextIntInstruction(this Instruction ins)
        {

            if (ins == null) return null;
            var start= ins.Next;
            while (true)
            {
                if (start == null) break;
                var code = start.OpCode.ToString();
                
                if (code.Contains(".i4"))
                {
                    return start;
                }

                start=start.Next;
            }
            return null;
        }

        public static Type[] GetInheritors(this Assembly asm, Type interfaceType)
        {
            var types = asm.GetTypes();
            return types.Where(d => interfaceType.IsAssignableFrom(d) && !d.IsInterface && !d.IsAbstract).ToArray();
        }

        public static void MakeFolder(this string s)
        {

            if (Directory.Exists(s)) return;
            Directory.CreateDirectory(s);

        }
        public static string MakeSafeFilename(this string s)
        {

            StringBuilder sb = new StringBuilder();
            var invalid = Path.GetInvalidFileNameChars();
            foreach (char c in s)
            {
                if (invalid.Contains(c))
                    continue;

                sb.Append(c);
            }

            var ret = sb.ToString().Trim();

            if (ret == String.Empty)
                ret = new Random(s.GetHashCode()).Next(1000000, 2000000).ToString();

            return ret;
        }

    }
}
