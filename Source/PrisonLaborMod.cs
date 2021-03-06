﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace PrisonLabor
{
    [StaticConstructorOnStartup]
    class PrisonLaborMod : Mod
    {
        public const int versionNumber = 7;
        public const string versionString = "0.7";

        private static string difficulty = "";

        private static bool showNews;
        private static bool allowAllWorktypes;
        private static bool enableMotivationMechanics;
        private static bool advanceGrowing;
        private static bool disableMod;

        public PrisonLaborMod(ModContentPack content) : base(content)
        {
        }

        public static void Init()
        {
            showNews = PrisonLaborPrefs.ShowNews;
            allowAllWorktypes = PrisonLaborPrefs.AllowAllWorkTypes;
            enableMotivationMechanics = PrisonLaborPrefs.EnableMotivationMechanics;
            disableMod = PrisonLaborPrefs.DisableMod;
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Rect leftRect = new Rect(inRect.x, inRect.y, inRect.width * 0.75f, inRect.height);
            Rect rightRect = new Rect(inRect.x + inRect.width * 0.75f + 30f, inRect.y, inRect.width * 0.25f - 30f, inRect.height);

            Listing_Standard listing_options = new Listing_Standard();

            listing_options.Begin(leftRect);

            listing_options.CheckboxLabeled("Show news", ref showNews, "Showing news about changes in mod when prisoners detected.");

            listing_options.GapLine();

            if (!disableMod)
            {
                listing_options.Label("Allowed work types:", -1f);
                listing_options.CheckboxLabeled("   allow all", ref allowAllWorktypes, "allow all work types");
                if (!allowAllWorktypes)
                {
                    if (listing_options.ButtonTextLabeled("   allowed work types:", "browse"))
                        Find.WindowStack.Add(new SelectWorkTypesDialog());
                }
                else
                {
                    listing_options.Gap();
                }

                listing_options.GapLine();

                listing_options.CheckboxLabeled("Motivation mechanics (!)", ref enableMotivationMechanics, "When checked prisoners need to be motivated.\n\nWARINING: Needs reloading save.");

                listing_options.GapLine();

                listing_options.CheckboxLabeled("Prisoners can grow advanced plants", ref advanceGrowing, "When disabled prisoners can only grow plants that not require any skills.");

            }
            else
            {
                listing_options.Gap();
                listing_options.Gap();
                listing_options.Label("Restart then re-save your game.", -1f);
                listing_options.Label("After this steps you can safely disable this mod.", -1f);
                listing_options.Gap();
                listing_options.Gap();
                listing_options.Gap();
            }

            listing_options.Gap();
            listing_options.Gap();
            listing_options.Gap();

            listing_options.CheckboxLabeled("Disable mod", ref disableMod, "When enabled, worlds that are saved are transferred to 'safe mode', and can be played without mod.");
            
            listing_options.End();

            Listing_Standard listing_panel = new Listing_Standard();

            listing_panel.Begin(rightRect);

            listing_panel.Label("Prison Labor Alpha", -1f);
            listing_panel.Label("Version: " + versionString, -1f);

            listing_panel.GapLine();

            listing_panel.Label("Difficulty: " + difficulty, -1f);

            listing_panel.GapLine();

            Listing_Standard listing_buttons = new Listing_Standard();

            listing_buttons.Begin(new Rect(rightRect.width * 0.25f, listing_panel.CurHeight, rightRect.width * 0.5f, rightRect.height - listing_panel.CurHeight));

            if (listing_buttons.ButtonText("Defaults"))
            {
                PrisonLaborPrefs.RestoreToDefault();
                Init();
            }

            if (listing_buttons.ButtonText("ShowNews"))
            {
                NewsDialog.showAll = true;
                NewsDialog.ForceShow();
            }

            listing_buttons.End();

            listing_panel.End();

            Apply();
        }

        public override string SettingsCategory()
        {
            return "Prison Labor";
        }

        public override void WriteSettings()
        {
            Log.Message("saved");
            PrisonLaborPrefs.ShowNews = showNews;
            PrisonLaborPrefs.AllowAllWorkTypes = allowAllWorktypes;
            if(!disableMod)
                PrisonLaborPrefs.EnableMotivationMechanics = enableMotivationMechanics;
            PrisonLaborPrefs.AdvancedGrowing = advanceGrowing;
            PrisonLaborPrefs.DisableMod = disableMod;
            PrisonLaborPrefs.Save();
        }

        private static void Apply()
        {
            PrisonLaborPrefs.Apply();
            CalculateDifficulty();
        }

        private static void CalculateDifficulty()
        {
            int value = 1000;
            if (!enableMotivationMechanics)
                value -= 300;
            if (advanceGrowing)
                value -= 50;
            value -= 500;
            if(!allowAllWorktypes)
            {
                int delta = 500 + 7 * 50 + (DefDatabase<WorkTypeDef>.DefCount - 20) * 25;
                foreach (WorkTypeDef wtd in DefDatabase<WorkTypeDef>.AllDefs)
                {
                    if (!PrisonLaborUtility.WorkDisabled(wtd))
                        delta -= 50;
                }
                if (delta > 0)
                    value += delta;
            }

            if (value >= 1000)
                difficulty = (int)(value / 10) + " (Normal)";
            else if (value >= 800)
                difficulty = (int)(value / 10) + " (Casual)";
            else if (value >= 500)
                difficulty = (int)(value / 10) + " (Easy)";
            else if (value >= 300)
                difficulty = (int)(value / 10) + " (Peaceful)";
            else
                difficulty = (int)(value / 10) + " (A joke)";
        }
    }
}
