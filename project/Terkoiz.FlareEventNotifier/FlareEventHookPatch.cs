using System;
using System.Linq;
using System.Reflection;
using Comfort.Common;
using SPT.Reflection.Patching;
using EFT;
using EFT.Communications;
using EFT.InventoryLogic;
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
        public static void PatchPrefix(FlareEventType flareType, EZoneEventTypeEnumClone eventType, string playerProfileID)
        {
            if (flareType != FlareEventType.ExitActivate)
            {
                return;
            }

            if (eventType != EZoneEventTypeEnumClone.FiredPlayerAddedInShotList && eventType != EZoneEventTypeEnumClone.PlayerByPartyAddedInShotList)
            {
                return;
            }

            var localPlayer = GetLocalPlayerFromWorld();
            if (localPlayer != null && localPlayer.ProfileId != playerProfileID)
            {
                return;
            }

            NotificationManagerClass.DisplayNotification(new ExfilFlareSuccessNotification());
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

        /// <summary>
        /// Gets the current <see cref="Player"/> instance if it's available
        /// </summary>
        /// <returns>Local <see cref="Player"/> instance; returns null if the game is not in raid</returns>
        private static Player GetLocalPlayerFromWorld()
        {
            var gameWorld = Singleton<GameWorld>.Instance;
            if (gameWorld == null || gameWorld.MainPlayer == null)
            {
                return null;
            }

            return gameWorld.MainPlayer;
        }
    }

    public class ExfilFlareSuccessNotification : NotificationAbstractClass
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