using System;
using RimWorld;
using Verse;

namespace StatusLimitAtPeace;

public class StatusLimitStatPart : StatPart
{
    public string TargetValue;

    private bool verifyRequest(StatRequest req)
    {
        if (!req.HasThing || !req.Thing.Spawned || req.Thing.Map == null)
        {
            return false;
        }

        if (req.Thing.def.race == null && !req.Thing.def.IsBuildingArtificial)
        {
            return false;
        }

        if (req.Thing.def.IsBuildingArtificial)
        {
            return true;
        }

        if (req.Thing.Faction == null || req.Thing is not Pawn pawn)
        {
            return false;
        }

        if (!StatusLimitAtPeace.instance.SettingsObject.ApplyOnDraftedPawns && pawn.Drafted)
        {
            return false;
        }

        if (pawn.Faction == Faction.OfPlayer)
        {
            if (!StatusLimitAtPeace.instance.SettingsObject.ApplyOnPlayerColonists &&
                !StatusLimitAtPeace.instance.SettingsObject.ApplyOnPlayerNotColonists)
            {
                return false;
            }

            if (!StatusLimitAtPeace.instance.SettingsObject.ApplyOnPlayerNotColonists &&
                !pawn.def.race.Humanlike)
            {
                return false;
            }
        }
        else
        {
            if (!StatusLimitAtPeace.instance.SettingsObject.ApplyOnEnemyFactionPawns &&
                pawn.Faction.PlayerRelationKind == FactionRelationKind.Hostile)
            {
                return false;
            }

            if (!StatusLimitAtPeace.instance.SettingsObject.ApplyOnNotEnemyFactionPawns &&
                pawn.Faction.PlayerRelationKind != FactionRelationKind.Hostile)
            {
                return false;
            }
        }

        return true;
    }

    public override string ExplanationPart(StatRequest req)
    {
        var text = " (Status Limit at Peace)";
        var defaultValue = new TaggedString(string.Empty);
        if (!verifyRequest(req))
        {
            return defaultValue;
        }

        var dangerRating = req.Thing.Map.dangerWatcher.DangerRating;

        string valueExplanation;
        if (req.Thing.def.IsBuildingArtificial)
        {
            var dangerType = "";
            var explanation = "";
            var activityValue = "";
            valueExplanation = "";
            if (TargetValue == "BedRestEffectiveness")
            {
                switch (dangerRating)
                {
                    case StoryDanger.None:
                        dangerType = "Peace".Translate();
                        activityValue = StatusLimitAtPeace.instance.SettingsObject.Bed_P.ToString();
                        break;
                    case StoryDanger.Low:
                        dangerType = "LowDanger".Translate();
                        activityValue = StatusLimitAtPeace.instance.SettingsObject.Bed_LD.ToString();
                        break;
                    case StoryDanger.High:
                        dangerType = "HighDanger".Translate();
                        activityValue = StatusLimitAtPeace.instance.SettingsObject.Bed_HD.ToString();
                        break;
                }

                explanation = "StatusLimitAtPeaceExplanationPart.Explanation".Translate(dangerType);
                valueExplanation = "StatusLimitAtPeaceExplanationPart.MaxBedRestEffectiveness".Translate(activityValue);
            }

            if (TargetValue != "JoyGainFactor")
            {
                return explanation + valueExplanation + text;
            }

            switch (dangerRating)
            {
                case StoryDanger.None:
                    dangerType = "Peace".Translate();
                    activityValue = StatusLimitAtPeace.instance.SettingsObject.Joy_P.ToString();
                    break;
                case StoryDanger.Low:
                    dangerType = "LowDanger".Translate();
                    activityValue = StatusLimitAtPeace.instance.SettingsObject.Joy_LD.ToString();
                    break;
                case StoryDanger.High:
                    dangerType = "HighDanger".Translate();
                    activityValue = StatusLimitAtPeace.instance.SettingsObject.Joy_HD.ToString();
                    break;
            }

            explanation = "StatusLimitAtPeaceExplanationPart.Explanation".Translate(dangerType);
            valueExplanation = "StatusLimitAtPeaceExplanationPart.MaxJoyGainFactor".Translate(activityValue);

            return explanation + valueExplanation + text;
        }

        valueExplanation = "";
        var valueInformation = "";
        switch (dangerRating)
        {
            case StoryDanger.None:
            {
                valueExplanation =
                    "StatusLimitAtPeaceExplanationPart.Explanation".Translate((string)"Peace".Translate());
                switch (TargetValue)
                {
                    case "MoveSpeed":
                    {
                        valueInformation =
                            "StatusLimitAtPeaceExplanationPart.MaxMoveSpeed".Translate(
                                StatusLimitAtPeace.instance.SettingsObject.Move_P.ToString("F2"));
                        break;
                    }
                    case "EatingSpeed":
                    {
                        valueInformation =
                            "StatusLimitAtPeaceExplanationPart.MaxEatingSpeed".Translate(StatusLimitAtPeace.instance
                                .SettingsObject.Eating_P.ToString());
                        break;
                    }
                    case "GeneralLaborSpeed":
                    {
                        valueInformation =
                            "StatusLimitAtPeaceExplanationPart.MaxGeneralLaborSpeed".Translate(StatusLimitAtPeace
                                .instance.SettingsObject.GeneralLabor_P.ToString());
                        break;
                    }
                    case "ConstructionSpeed":
                    {
                        valueInformation =
                            "StatusLimitAtPeaceExplanationPart.MaxConstructionSpeed".Translate(StatusLimitAtPeace
                                .instance.SettingsObject.Construction_P.ToString());
                        break;
                    }
                    case "PlantWorkSpeed":
                    {
                        valueInformation =
                            "StatusLimitAtPeaceExplanationPart.MaxPlantWorkSpeed".Translate(StatusLimitAtPeace.instance
                                .SettingsObject.PlantWork_P.ToString());
                        break;
                    }
                    case "MiningSpeed":
                    {
                        valueInformation =
                            "StatusLimitAtPeaceExplanationPart.MaxMiningSpeed".Translate(StatusLimitAtPeace.instance
                                .SettingsObject.Mining_P.ToString());
                        break;
                    }
                    case "RestRateMultiplier":
                    {
                        valueInformation =
                            "StatusLimitAtPeaceExplanationPart.MaxRestRateMultiplier".Translate(StatusLimitAtPeace
                                .instance.SettingsObject.Rest_P.ToString());
                        break;
                    }
                }

                break;
            }
            case StoryDanger.Low:
            {
                valueExplanation =
                    "StatusLimitAtPeaceExplanationPart.Explanation".Translate((string)"LowDanger".Translate());
                switch (TargetValue)
                {
                    case "MoveSpeed":
                        valueInformation =
                            "StatusLimitAtPeaceExplanationPart.MaxMoveSpeed".Translate(
                                StatusLimitAtPeace.instance.SettingsObject.Move_LD.ToString("F2"));
                        break;
                    case "EatingSpeed":
                        valueInformation =
                            "StatusLimitAtPeaceExplanationPart.MaxEatingSpeed".Translate(StatusLimitAtPeace.instance
                                .SettingsObject.Eating_LD.ToString());
                        break;
                    case "GeneralLaborSpeed":
                        valueInformation =
                            "StatusLimitAtPeaceExplanationPart.MaxGeneralLaborSpeed".Translate(StatusLimitAtPeace
                                .instance.SettingsObject.GeneralLabor_LD.ToString());
                        break;
                    case "ConstructionSpeed":
                        valueInformation =
                            "StatusLimitAtPeaceExplanationPart.MaxConstructionSpeed".Translate(StatusLimitAtPeace
                                .instance.SettingsObject.Construction_LD.ToString());
                        break;
                    case "PlantWorkSpeed":
                        valueInformation =
                            "StatusLimitAtPeaceExplanationPart.MaxPlantWorkSpeed".Translate(StatusLimitAtPeace.instance
                                .SettingsObject.PlantWork_LD.ToString());
                        break;
                    case "MiningSpeed":
                        valueInformation =
                            "StatusLimitAtPeaceExplanationPart.MaxMiningSpeed".Translate(StatusLimitAtPeace.instance
                                .SettingsObject.Mining_LD.ToString());
                        break;
                    case "RestRateMultiplier":
                        valueInformation =
                            "StatusLimitAtPeaceExplanationPart.MaxRestRateMultiplier".Translate(StatusLimitAtPeace
                                .instance.SettingsObject.Rest_LD.ToString());
                        break;
                }

                break;
            }
            case StoryDanger.High:
            {
                valueExplanation =
                    "StatusLimitAtPeaceExplanationPart.Explanation".Translate((string)"HighDanger".Translate());
                switch (TargetValue)
                {
                    case "MoveSpeed":
                        valueInformation =
                            "StatusLimitAtPeaceExplanationPart.MaxMoveSpeed".Translate(
                                StatusLimitAtPeace.instance.SettingsObject.Move_HD.ToString("F2"));
                        break;
                    case "EatingSpeed":
                        valueInformation =
                            "StatusLimitAtPeaceExplanationPart.MaxEatingSpeed".Translate(StatusLimitAtPeace.instance
                                .SettingsObject.Eating_HD.ToString());
                        break;
                    case "GeneralLaborSpeed":
                        valueInformation =
                            "StatusLimitAtPeaceExplanationPart.MaxGeneralLaborSpeed".Translate(StatusLimitAtPeace
                                .instance.SettingsObject.GeneralLabor_HD.ToString());
                        break;
                    case "ConstructionSpeed":
                        valueInformation =
                            "StatusLimitAtPeaceExplanationPart.MaxConstructionSpeed".Translate(StatusLimitAtPeace
                                .instance.SettingsObject.Construction_HD.ToString());
                        break;
                    case "PlantWorkSpeed":
                        valueInformation =
                            "StatusLimitAtPeaceExplanationPart.MaxPlantWorkSpeed".Translate(StatusLimitAtPeace.instance
                                .SettingsObject.PlantWork_HD.ToString());
                        break;
                    case "MiningSpeed":
                        valueInformation =
                            "StatusLimitAtPeaceExplanationPart.MaxMiningSpeed".Translate(StatusLimitAtPeace.instance
                                .SettingsObject.Mining_HD.ToString());
                        break;
                    case "RestRateMultiplier":
                        valueInformation =
                            "StatusLimitAtPeaceExplanationPart.MaxRestRateMultiplier".Translate(StatusLimitAtPeace
                                .instance.SettingsObject.Rest_HD.ToString());
                        break;
                }

                break;
            }
        }

        return valueExplanation + valueInformation + text;
    }

    public override void TransformValue(StatRequest req, ref float val)
    {
        if (!verifyRequest(req))
        {
            return;
        }

        var dangerRating = req.Thing.Map.dangerWatcher.DangerRating;
        if (req.Thing.def.IsBuildingArtificial)
        {
            switch (TargetValue)
            {
                case "BedRestEffectiveness":
                {
                    switch (dangerRating)
                    {
                        case StoryDanger.None:
                            val = Math.Min(val, StatusLimitAtPeace.instance.SettingsObject.Bed_P / 100f);
                            break;
                        case StoryDanger.Low:
                            val = Math.Min(val, StatusLimitAtPeace.instance.SettingsObject.Bed_LD / 100f);
                            break;
                        case StoryDanger.High:
                            val = Math.Min(val, StatusLimitAtPeace.instance.SettingsObject.Bed_HD / 100f);
                            break;
                    }

                    break;
                }
                case "JoyGainFactor":
                {
                    switch (dangerRating)
                    {
                        case StoryDanger.None:
                            val = Math.Min(val, StatusLimitAtPeace.instance.SettingsObject.Joy_P / 100f);
                            break;
                        case StoryDanger.Low:
                            val = Math.Min(val, StatusLimitAtPeace.instance.SettingsObject.Joy_LD / 100f);
                            break;
                        case StoryDanger.High:
                            val = Math.Min(val, StatusLimitAtPeace.instance.SettingsObject.Joy_HD / 100f);
                            break;
                    }

                    break;
                }
            }

            return;
        }


        switch (dangerRating)
        {
            case StoryDanger.None:
            {
                switch (TargetValue)
                {
                    case "MoveSpeed":
                        val = Math.Min(val, StatusLimitAtPeace.instance.SettingsObject.Move_P);
                        break;
                    case "EatingSpeed":
                        val = Math.Min(val, StatusLimitAtPeace.instance.SettingsObject.Eating_P / 100f);
                        break;
                    case "GeneralLaborSpeed":
                        val = Math.Min(val, StatusLimitAtPeace.instance.SettingsObject.GeneralLabor_P / 100f);
                        break;
                    case "ConstructionSpeed":
                        val = Math.Min(val, StatusLimitAtPeace.instance.SettingsObject.Construction_P / 100f);
                        break;
                    case "PlantWorkSpeed":
                        val = Math.Min(val, StatusLimitAtPeace.instance.SettingsObject.PlantWork_P / 100f);
                        break;
                    case "MiningSpeed":
                        val = Math.Min(val, StatusLimitAtPeace.instance.SettingsObject.Mining_P / 100f);
                        break;
                    case "RestRateMultiplier":
                        val = Math.Min(val, StatusLimitAtPeace.instance.SettingsObject.Rest_P / 100f);
                        break;
                }

                break;
            }
            case StoryDanger.Low:
            {
                switch (TargetValue)
                {
                    case "MoveSpeed":
                        val = Math.Min(val, StatusLimitAtPeace.instance.SettingsObject.Move_LD);
                        break;
                    case "EatingSpeed":
                        val = Math.Min(val, StatusLimitAtPeace.instance.SettingsObject.Eating_LD / 100f);
                        break;
                    case "GeneralLaborSpeed":
                        val = Math.Min(val, StatusLimitAtPeace.instance.SettingsObject.GeneralLabor_LD / 100f);
                        break;
                    case "ConstructionSpeed":
                        val = Math.Min(val, StatusLimitAtPeace.instance.SettingsObject.Construction_LD / 100f);
                        break;
                    case "PlantWorkSpeed":
                        val = Math.Min(val, StatusLimitAtPeace.instance.SettingsObject.PlantWork_LD / 100f);
                        break;
                    case "MiningSpeed":
                        val = Math.Min(val, StatusLimitAtPeace.instance.SettingsObject.Mining_LD / 100f);
                        break;
                    case "RestRateMultiplier":
                        val = Math.Min(val, StatusLimitAtPeace.instance.SettingsObject.Rest_LD / 100f);
                        break;
                }

                break;
            }
            case StoryDanger.High:
            {
                switch (TargetValue)
                {
                    case "MoveSpeed":
                        val = Math.Min(val, StatusLimitAtPeace.instance.SettingsObject.Move_HD);
                        break;
                    case "EatingSpeed":
                        val = Math.Min(val, StatusLimitAtPeace.instance.SettingsObject.Eating_HD / 100f);
                        break;
                    case "GeneralLaborSpeed":
                        val = Math.Min(val, StatusLimitAtPeace.instance.SettingsObject.GeneralLabor_HD / 100f);
                        break;
                    case "ConstructionSpeed":
                        val = Math.Min(val, StatusLimitAtPeace.instance.SettingsObject.Construction_HD / 100f);
                        break;
                    case "PlantWorkSpeed":
                        val = Math.Min(val, StatusLimitAtPeace.instance.SettingsObject.PlantWork_HD / 100f);
                        break;
                    case "MiningSpeed":
                        val = Math.Min(val, StatusLimitAtPeace.instance.SettingsObject.Mining_HD / 100f);
                        break;
                    case "RestRateMultiplier":
                        val = Math.Min(val, StatusLimitAtPeace.instance.SettingsObject.Rest_HD / 100f);
                        break;
                }

                break;
            }
        }
    }
}