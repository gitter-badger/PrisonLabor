﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.AI;

namespace PrisonLabor
{
    class WorkGiver_Supervise : WorkGiver_Warden
    {
        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (!base.ShouldTakeCareOfPrisoner(pawn, t) || !Laziness.pawn((Pawn)t).NeedToBeInspired)
            {
                return null;
            }
            Pawn pawn2 = (Pawn)t;
            if (pawn2.guest.interactionMode == DefDatabase<PrisonerInteractionModeDef>.GetNamed("PrisonLabor_workOption") && (!pawn2.Downed || pawn2.InBed()) && pawn.CanReserve(t, 1, -1, null, false) && pawn2.Awake())
            {
                    return new Job(DefDatabase<JobDef>.GetNamed("PrisonerSupervise"), t);
            }
            return null;
        }
    }
}
