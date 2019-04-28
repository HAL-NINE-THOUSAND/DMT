using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace DMT
{
    public class ModInfo
    {
        private static string[] requiredFields = new string[]
        {
            "name",
            "mod_version",
        };


        public bool IsValid;
        public string Name;
        public string Location;
        public bool Enabled;
        public string Author;
        public string Description;
        public string ModVersion;
        public string HelpFilePath;
        public List<string> Dependencies = new List<string>();
        public List<string> Conflicts = new List<string>();

        public static ModInfo Create(string directory)
        {

            string modXmlPath = Path.Combine(directory, "mod.xml");
            if (!File.Exists(modXmlPath)) return null;

            var ret = new ModInfo();
            ret.Location = directory;
            ret.Enabled = true;
            ret.Name = "Unknown";
            ret.Author = "Unknown";
            ret.Description = "None";
            ret.ModVersion = "0.0";
            ret.IsValid = false;

            var configDoc = new XmlDocument();
            configDoc.Load(modXmlPath);
            ret.IsValid = ret.FromXml(configDoc);
            return ret;
        }

        public bool FromXml(XmlDocument doc)
        {
            XmlElement ele = doc.DocumentElement["info"];
            if (ele == null)
            {
                Logging.LogError("Mod config is missing 'info' node");
                return false;
            }

            for (int i = 0; i < requiredFields.Length; i++)
            {
                string fieldName = requiredFields[i];
                if (ele[fieldName] == null)
                {
                    Logging.LogError("Mod info is missing '" + fieldName + "' node");
                    return false;
                }
            }


            Name = ele["name"].GetValue();
            ModVersion = ele["mod_version"].GetValue();

            Author = ele["author"].GetValue();
            Description = ele["description"].GetValue();
            HelpFilePath = Path.Combine(this.Location, ele["help"].GetValue());

            var dependencies = doc.DocumentElement["dependencies"] ?? doc.DocumentElement["Dependencies"];
            if (dependencies != null)
            {
                foreach (XmlElement e in dependencies.ChildNodes)
                {
                    Dependencies.Add(e.InnerText);
                }
            }

            var conflicts = doc.DocumentElement["conflicts"] ?? doc.DocumentElement["Conflicts"];
            if (conflicts != null)
            {
                foreach (XmlElement e in conflicts.ChildNodes)
                {
                    Conflicts.Add(e.InnerText);
                }
            }

            return true;
        }


        public bool HasFile(string filter, string modLocation)
        {

            return File.Exists(modLocation + filter);

        }

        public List<string> FindFiles(string path, string filter, bool recursive = false)
        {
            List<string> fileNames = new List<string>();
            Stack<string> dirStack = new Stack<string>();

            dirStack.Push(Path.Combine(Location, path));

            while (dirStack.Count > 0)
            {
                string dir = dirStack.Pop();

                if (Directory.Exists(dir))
                {
                    fileNames.AddRange(Directory.GetFiles(dir, filter));

                    if (recursive)
                    {
                        foreach (var subDir in Directory.GetDirectories(dir))
                        {
                            dirStack.Push(subDir);
                        }
                    }
                }
                else
                {
                    //Logging.Log("Dir not found: " + dir);
                }
            }

            return fileNames;
        }
    }
}
