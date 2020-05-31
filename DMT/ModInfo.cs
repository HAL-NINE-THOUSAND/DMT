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
        private static string[] requiredFieldsModInfo = new string[]
        {
            "name",
            "version",
        };


        public bool IsValid;
        public bool UpgradeDetected;
        public string Name;

        public string FolderName;
        public string Location
        {
            get{ return (BuildSettings.Instance.ModFolder.FolderFormat() + FolderName).FolderFormat(); }
        }

        public bool Enabled;
        public string Author;
        public string Description;
        public string ModVersion;
        public string HelpFilePath;
        public string Website;
        public List<string> Dependencies = new List<string>();
        public List<string> Conflicts = new List<string>();

        public static ModInfo Create(string directory)
        {

            var ret = new ModInfo();
            //ret.Location = directory;
            ret.FolderName = new DirectoryInfo(directory).Name;
            ret.Enabled = true;
            ret.Name = "Unknown";
            ret.Author = "Unknown";
            ret.Description = "None";
            ret.ModVersion = "0.0";
            ret.IsValid = false;


            bool hasModFile = false;
            bool hasModInfoFile = false;
            string modXmlPath = Path.Combine(directory, "mod.xml");
            if (hasModFile = File.Exists(modXmlPath))
            {
                var configDoc = new XmlDocument();
                configDoc.Load(modXmlPath);
                ret.IsValid = ret.FromSdxModFileXml(modXmlPath, configDoc);
            }


            string modInfoPath = Path.Combine(directory, "ModInfo.xml");

            if (hasModFile && !File.Exists(modInfoPath))
            {
                File.WriteAllText(modInfoPath, $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<xml>
  <ModInfo>
    <Name value=""{ret.Name}"" />
    <Description value=""{ret.Description}"" />
    <Author value=""{ret.Author}"" />
    <Version value=""{ret.ModVersion}"" />
    <Website value=""{ret.Website}"" />
  </ModInfo>
  <DMT>
    <dependencies />
    <conflicts />
  </DMT>
</xml>
");
            }

            if (hasModInfoFile = File.Exists(modInfoPath))
            {
                var configDoc = new XmlDocument();
                configDoc.Load(modInfoPath);
                ret.IsValid = ret.FromModInfoXml(modInfoPath, configDoc, hasModFile);
            }
            
            if (!hasModFile && !hasModInfoFile)
            {
                return null;
            }

            return ret;
        }

        public bool FromSdxModFileXml(string path, XmlDocument doc)
        {
            XmlElement ele = doc.DocumentElement.GetElement("info");
            if (ele == null)
            {
                Logging.LogError(path + " Mod config is missing 'info' node");
                return false;
            }

            for (int i = 0; i < requiredFields.Length; i++)
            {
                string fieldName = requiredFields[i];
                if (ele.GetElement(fieldName) == null)
                {
                    Logging.LogError(path + " Mod info is missing '" + fieldName + "' node");
                    return false;
                }
            }


            Name = ele.GetElementValue("name");
            ModVersion = ele.GetElementValue("mod_version");

            Author = ele.GetElementValue("author");
            Description = ele.GetElementValue("description");
            HelpFilePath = Path.Combine(this.Location, ele.GetElementValue("help"));

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

        public bool FromModInfoXml(string path, XmlDocument doc, bool hasModFile)
        {

            bool saveIsRequired = hasModFile;
            XmlElement ele = doc.DocumentElement.GetElement("ModInfo");
            if (ele == null)
            {
                Logging.LogError(path + " Mod config is missing 'ModInfo' node");
                return false;
            }

            for (int i = 0; i < requiredFieldsModInfo.Length; i++)
            {
                string fieldName = requiredFieldsModInfo[i];
                if (ele.GetElement(fieldName) == null)
                {
                    Logging.LogError(path + " Mod info is missing '" + fieldName + "' node");
                    return false;
                }
            }



            Name = ele.GetElementValueAttribute("name");
            Description = ele.GetElementValueAttribute("description");
            Author = ele.GetElementValueAttribute("author");
            ModVersion = ele.GetElementValueAttribute("version");
            HelpFilePath = Path.Combine(this.Location, ele.GetElementValueAttribute("help"));
            Website = ele.GetElementValueAttribute("Website");


            var dmtNode = doc.GetElement("DMT");

            if (dmtNode == null)
            {
                dmtNode = doc.DocumentElement.AppendChild(doc.CreateElement("DMT")) as XmlElement;
                //return true;
            }

            var dependencies = dmtNode.GetElement("dependencies");
            if (dependencies == null)
            {
                dependencies = dmtNode.AppendChild(doc.CreateElement("dependencies")) as XmlElement;
            }

            foreach(var d in Dependencies)
            {
                var doAdd = true;
                foreach(XmlElement x in dependencies.ChildNodes)
                {
                    if (x.InnerText == d)
                    {
                        doAdd = false;
                        break;
                    }
                }

                if (doAdd)
                {
                    var e = doc.CreateElement("dependancy");
                    e.InnerText = d;
                    dependencies.AppendChild(e);
                    saveIsRequired = true;
                }
            }

            foreach (XmlElement e in dependencies.ChildNodes)
            {
                if (!Dependencies.Contains(e.InnerText))
                {
                    Dependencies.Add(e.InnerText);
                }
            }

            var conflicts = dmtNode.GetElement("conflicts");
            if (conflicts == null)
            {
                conflicts = dmtNode.AppendChild(doc.CreateElement("conflicts")) as XmlElement;
            }
            foreach (var d in Conflicts)
            {
                var doAdd = true;
                foreach (XmlElement x in conflicts.ChildNodes)
                {
                    if (x.InnerText == d)
                    {
                        doAdd = false;
                        break;
                    }
                }

                if (doAdd)
                {
                    var e = doc.CreateElement("conflict");
                    e.InnerText = d;
                    conflicts.AppendChild(e);
                    saveIsRequired = true;
                }
            }

            foreach (XmlElement e in conflicts.ChildNodes)
            {
                if (!Conflicts.Contains(e.InnerText))
                {
                    Conflicts.Add(e.InnerText);
                }
            }

            if (saveIsRequired)
            {
                using (var stream = new FileStream(path, FileMode.OpenOrCreate))
                {
                    using (var writer = XmlWriter.Create(stream, new XmlWriterSettings()
                    {
                        Indent = true,
                    }))
                    {
                        doc.WriteTo(writer);
                    }
                }
            }
            UpgradeDetected = saveIsRequired;
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
