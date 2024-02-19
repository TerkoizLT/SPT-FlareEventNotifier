using System;
using System.Linq;
using System.Reflection;
using Aki.Reflection.Patching;
using EFT;
using EFT.Communications;
using EFT.PrefabSettings;
using HarmonyLib;
using UnityEngine;

// ReSharper disable ArrangeAccessorOwnerBody

namespace Terkoiz.FlareEventNotifier
{
    public class FlareEventHookPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            var targetType = typeof(AbstractGame).Assembly.GetTypes().SingleOrDefault(t => t.GetProperty("ZoneEventType") != null)
                             ?? throw new Exception("Could not locate target type");

            return AccessTools.DeclaredMethod(targetType, "Invoke");
        }

        [PatchPrefix]
        public static void PatchPrefix(FlareEventType flareType, EZoneEventTypeEnumClone eventType)
        {
            if (flareType != FlareEventType.ExitActivate)
            {
                return;
            }

            if (eventType == EZoneEventTypeEnumClone.FiredPlayerAddedInShotList || eventType == EZoneEventTypeEnumClone.PlayerByPartyAddedInShotList)
            {
                NotificationManagerClass.DisplayNotification(new ExfilFlareSuccessNotification());
            }
        }

        /// <summary>
        /// Copy of the EZoneEventType enum so that a direct GClass reference could be avoided
        /// </summary>
        public enum EZoneEventTypeEnumClone
        {
            None,
            PlayerEnteredZone,
            PlayerExitedZone,
            FiredPlayerAddedInShotList,
            PlayerByPartyAddedInShotList
        }
    }

    public class ExfilFlareSuccessNotification : NotificationClass
    {
        public ExfilFlareSuccessNotification()
        {
            Duration = ENotificationDurationType.Long;
        }

        public override string Description { get => "Exfil activated"; }

        public override ENotificationIconType Icon { get => ENotificationIconType.Default; }

        public override Color? TextColor { get => Color.green; }
    }
}