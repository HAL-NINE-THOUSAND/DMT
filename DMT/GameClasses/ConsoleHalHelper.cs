using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using DMT;
using UnityEngine;

public class DMTChanges
{
    public static void FindTypesImplementingBase(Type _searchType, Action<Type> _typeFoundCallback, bool _allowAbstract = false)
    {
        try
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            for (int iAss = 0; iAss < assemblies.Length; iAss++)
            {
                Assembly a = assemblies[iAss];
                try
                {
                    foreach (Type t in a.GetTypes())
                    {
                        if (!t.IsClass)
                        {
                            continue;
                        }

                        if (!_allowAbstract && t.IsAbstract)
                        {
                            continue;
                        }

                        if (!_searchType.IsAssignableFrom(t))
                        {
                            continue;
                        }

                        try
                        {
                            _typeFoundCallback?.Invoke(t);
                        }
                        catch (Exception e)
                        {
                            Log.Error($"Error invoking found type callback for '{t.FullName}'");
                            Log.Exception(e);
                        }
                    }
                }
                catch (ReflectionTypeLoadException e)
                {
                    Log.Error($"Error loading types from assembly '{a.FullName}' ({a.Location})");
                    Log.Exception(e);
                    Console.WriteLine();
                    Console.WriteLine("Successfully loaded Types:");
                    int i = 1;
                    foreach (Type t in e.Types)
                    {
                        Console.WriteLine($"{i++}. {(t != null ? t.FullName : "NULL")}");
                    }
                    Console.WriteLine();
                    Console.WriteLine("Exceptions:");
                    i = 1;
                    foreach (Exception e2 in e.LoaderExceptions)
                    {
                        Console.WriteLine($"{i++}. {(e2 != null ? e2.Message : "NULL")}");
                    }
                    Console.WriteLine();
                }
                catch (Exception e)
                {
                    Log.Error($"Error loading types from assembly {a.Location}");
                    Log.Exception(e);
                }
            }
        }
        catch (Exception e)
        {
            Log.Error("Error loading types");
            Log.Exception(e);
        }
    }
    public static List<Assembly> GetLoadedAssemblies()
    {
        var ret = new List<Assembly>(AppDomain.CurrentDomain.GetAssemblies());
        return ret;
    }

    public static void AddReferencedAssemblies()
    {
        var loc = Assembly.GetExecutingAssembly().Location;
        loc = loc.Substring(0, loc.LastIndexOf(Path.DirectorySeparatorChar));
        var dllPath = loc + "/Mods.dll";

        Log.Out("Looking for mods dll at " + dllPath);
        if (System.IO.File.Exists(dllPath))
        {
            Log.Out("Trying to add mods.dll");
            Assembly assembly = Assembly.LoadFrom(dllPath);
            //assemblies.Add(assembly);
        }

        //List<Assembly> assemblies
        //var asses = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
        //Log.Out("Assemblies: " + asses.Length);
        //foreach (AssemblyName an in asses)
        //{

        //    var path = loc + "/" + an.Name + ".dll";
        //    if (!File.Exists(path))
        //    {
        //        Log.Out("Skipping missing Assembly: " + an.Name);
        //        continue;
        //    }
        //    Assembly asm = Assembly.Load(an.ToString());
        //    if (!assemblies.Contains(asm))
        //    {
        //        Log.Out("Adding Assembly: " + an.Name);
        //        assemblies.Add(asm);
        //    }
        //    else
        //    {
        //        Log.Out("Assembly already added: " + an.Name);
        //    }
        //}



        //var dmt = assemblies.FirstOrDefault(d => d.FullName.StartsWith("DMT, Version="));

        //if (dmt != null)
        //{
        //    Log.Out("Removing DMT assembly");
        //    assemblies.Remove(dmt);
        //}

    }

    public static void DmtInit()
    {
        //cleans up log files
        Application.SetStackTraceLogType(UnityEngine.LogType.Log, StackTraceLogType.None);
        Application.SetStackTraceLogType(UnityEngine.LogType.Warning, StackTraceLogType.None);

        //HookHarmony();

    }

    public static void HookHarmony()
    {


        var assemblies = new List<Assembly>();
        var modPath = Application.platform != RuntimePlatform.OSXPlayer ? (Application.dataPath + "/../Mods") : (Application.dataPath + "/../../Mods");

        if (Directory.Exists(modPath))
        {
            Log.Out("Start harmony loading: " + modPath);
            string[] directories = Directory.GetDirectories(modPath);

            foreach (string path in directories)
            {
                try
                {

                    var harmPath = path + "/Harmony/";
                    var modinfoPath = path + "/ModInfo.xml";

                    if (Directory.Exists(harmPath) && File.Exists(modinfoPath))
                    {
                        var files = Directory.GetFiles(harmPath, "*.dll");

                        foreach (var file in files)
                        {
                            Log.Out("DLL found: " + file);
                            var assembly = AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(file));
                            assemblies.Add(assembly);
                        }

                    }

                }
                catch (Exception e)
                {
                    Log.Error("Failed loading harmony from " + Path.GetFileName(path));
                    Log.Exception(e);
                }
            }

        }

        var harmInterfaces = assemblies.SelectMany(d => d.GetInterfaceImplementers<IHarmony>());
        Log.Out("Found harmony interfaces: " + harmInterfaces.Count());
        foreach (var i in harmInterfaces)
        {
            i.Start();
        }

    }
}

