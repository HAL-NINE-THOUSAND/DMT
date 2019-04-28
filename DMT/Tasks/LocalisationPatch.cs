using System.IO;
using System.Linq;
using DMT.Attributes;

namespace DMT.Tasks
{
    [RunOrder(RunSection.InitialPatch, RunOrder.Start)]
    public class LocalisationPatch : BaseTask
    {

        public override bool Patch(PatchData data)
        {

            string sourcePath =$"{data.BackupFolder}Config/Localization.txt";
            string destPath = $"{data.GameFolder}/Data/Config/Localization.txt"; 

            if (!File.Exists(sourcePath))
            {
                LogWarning("Localization.txt file not found in the backup folder");
                return true;
            }
            PatchTextFile(data, sourcePath, destPath, "Config/Localization.txt");

            sourcePath = $"{data.BackupFolder}Config/Localization - Quest.txt";
            destPath = $"{data.GameFolder}/Data/Config/Localization - Quest.txt"; 
            PatchTextFile(data, sourcePath, destPath, "Config/Localization - Quest.txt");

            return true;
        }

        private void PatchTextFile(PatchData data, string sourcePath, string destPath, string patchFilePath)
        {
            LocalisationTable locTable = new LocalisationTable();


            var files = data.FindFiles(patchFilePath);


            if (files.Count() == 0)
            {
                File.Copy(sourcePath, destPath, true);
                return;
            }

            locTable.Load(sourcePath);

            foreach (string filePath in files)
            {
                Log("PatchFile: " + filePath);
                LocalisationTable otherTable = new LocalisationTable();
                otherTable.Load(filePath);
                locTable.Merge(otherTable);
            }

            Log("Save text file: " + destPath);
            locTable.Save(destPath);
        }

    }
}
