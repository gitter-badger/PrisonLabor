﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Harmony;
using System.Reflection;
using System.Reflection.Emit;
using Verse.AI;

namespace PrisonLabor
{
    [StaticConstructorOnStartup]
    class Initialization
    {
        public static int version = PrisonLaborMod.versionNumber;

        static Initialization()
        {
            HarmonyPatches.run();
            PrisonLaborPrefs.Init();
            PrisonLaborMod.Init();
            checkVersion();
        }

        private static void checkVersion()
        {
            //delete later
            if (PrisonLaborPrefs.Version > 2 && PrisonLaborPrefs.Version < 6)
            {
                PrisonLaborPrefs.LastVersion = PrisonLaborPrefs.Version;
            }

            // Update actual version
            if (PrisonLaborPrefs.Version <= 0)
            {
                PrisonLaborPrefs.Version = version;
                PrisonLaborPrefs.LastVersion = version;
            }
            else if (PrisonLaborPrefs.Version != version)
            {
                PrisonLaborPrefs.Version = version;
            }

            // Check for news
            if (PrisonLaborPrefs.LastVersion < 5)
            {
                Log.Message("Detected older version of PrisonLabor than 0.5");
                NewsDialog.news_0_5 = true;
                NewsDialog.autoShow = true;
            }
            if (PrisonLaborPrefs.LastVersion < 6)
            {
                Log.Message("Detected older version of PrisonLabor than 0.6");
                NewsDialog.news_0_6 = true;
                NewsDialog.autoShow = true;
            }
            if (PrisonLaborPrefs.LastVersion < 7)
            {
                Log.Message("Detected older version of PrisonLabor than 0.7");
                NewsDialog.news_0_7 = true;
                NewsDialog.autoShow = true;
            }

            Log.Message("Loaded PrisonLabor v" + PrisonLaborPrefs.Version);
            PrisonLaborPrefs.Version = version;
            PrisonLaborPrefs.Save();
        }
    }
}