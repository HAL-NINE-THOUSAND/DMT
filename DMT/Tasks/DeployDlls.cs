using System;
using System.Collections.Generic;
using System.IO;

using DMT.Attributes;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace DMT.Tasks
{
    [RunOrder(RunSection.InitialPatch, RunOrder.Start)]
    public class DeployDlls : BaseTask
    {

        public override bool Patch(PatchData data)
        {

            try
            {

                var dllsToCopy = new[]
                {
                    "0Harmony.dll",
                    "DMT.dll",
                };
                Logging.Log("Deploying DLLs");

                foreach (var s in dllsToCopy)
                {
                    File.Copy(s, data.ManagedFolder + Path.GetFileName(s), true);
                }

                return true;
            }
            catch (Exception ex)
            {
                Logging.LogError(ex.Message);
            }

            return false;
        }


    }
}
