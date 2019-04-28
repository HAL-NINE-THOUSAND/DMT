using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DMT;
using Mono.Cecil;
using Mono.Cecil.Cil;

    public class PatcherHelper
    {

        public static void AddNetpackageHelper(ModuleDefinition gameModule, ModuleDefinition modModule, string enumFieldName, byte enumFieldValue, string netPackageTypeName)
        {
        
            SetFieldToPublic(gameModule.Types.First(d => d.Name == "NetPackage").Fields.First(d => d.Name == "m_PackageTypeToClass"));
            var enumTypeDef = gameModule.Types.First(d => d.Name == "NetPackageType");
            var existing = enumTypeDef.Fields.FirstOrDefault(d => d.Constant != null && d.Constant.ToString() == enumFieldValue.ToString());
            if (existing != null)
            {
                
                Logging.LogError(enumTypeDef.Name + " already has an enum defined for " + enumFieldValue + ": " + existing.Name);
                return;
            }

            FieldDefinition literalDef;
            enumTypeDef.Fields.Add(literalDef = new FieldDefinition(enumFieldName, Mono.Cecil.FieldAttributes.Public | Mono.Cecil.FieldAttributes.Static | Mono.Cecil.FieldAttributes.Literal | Mono.Cecil.FieldAttributes.HasDefault, enumTypeDef));
            literalDef.Constant = enumFieldValue;

            InsertNetpackageCall(gameModule, modModule, enumFieldValue, netPackageTypeName);
        }

        public static void InsertNetpackageCall(ModuleDefinition gameModule, ModuleDefinition modModule, byte value, string classType)
        {

            var package = gameModule.Import(modModule.Types.First(d => d.Name == classType));

            var method = gameModule.Types.First(d => d.Name == "NetPackage").Methods.FirstOrDefault(d => d.Name == ".cctor" && d.IsStatic == true);
            var instructions = method.Body.Instructions;
            var pro = method.Body.GetILProcessor();
            var lastInstruction = instructions.Last();

            var fromHandle = (MethodReference)instructions.First(d => d != null && d.OpCode == OpCodes.Call && ((MethodReference)d.Operand).Name.Contains("FromHandle")).Operand;
            var addMethod = (MethodReference)instructions.First(d => d != null && d.OpCode == OpCodes.Callvirt && ((MethodReference)d.Operand).Name.Contains("Add")).Operand;
            var packageDictionary = (FieldReference)instructions.First(d => d != null && d.OpCode == OpCodes.Ldsfld && ((FieldReference)d.Operand).Name.Contains("m_PackageTypeToClass")).Operand;

            pro.InsertBefore(lastInstruction, Instruction.Create(OpCodes.Ldsfld, packageDictionary));
            pro.InsertBefore(lastInstruction, Instruction.Create(OpCodes.Ldc_I4, (int)value));
            pro.InsertBefore(lastInstruction, Instruction.Create(OpCodes.Ldtoken, package));
            pro.InsertBefore(lastInstruction, Instruction.Create(OpCodes.Call, fromHandle));
            pro.InsertBefore(lastInstruction, Instruction.Create(OpCodes.Callvirt, addMethod));

        }


        public static void SetMethodToVirtual(MethodDefinition meth)
        {
            meth.IsVirtual = true;
        }
        public static void SetFieldToPublic(FieldDefinition field)
        {
            field.IsFamily = false;
            field.IsPrivate = false;
            field.IsPublic = true;

        }
        public static void SetClassToPublic(TypeDefinition classType)
        {
            //classType.is = false;
            classType.IsNotPublic = false;
            classType.IsPublic = true;

        }
        public static void SetMethodToPublic(MethodDefinition field)
        {
            field.IsFamily = false;
            field.IsPrivate = false;
            field.IsPublic = true;

        }
    }
