﻿using MEC;
using Synapse;
using Synapse.Api;
using System.Collections.Generic;
using VT_Api.Extension;

namespace VTEscape
{
    public class Method
    {
        public static IEnumerator<float> WarHeadEscape(int waitforready = 3)
        {
            yield return Timing.WaitForSeconds(10f);
            while (waitforready > 0)
            {
                Map.Get.PlayAmbientSound(7);
                waitforready--;
                yield return Timing.WaitForSeconds(1f);
            }
            if (Plugin.Config.WarHeadLockEnabled)
            {
                Map.Get.Nuke.InsidePanel.Locked = true;
            }
            if (Round.Get.RoundIsActive)
                Server.Get.Map.Nuke.StartDetonation();
        }

        static public void ChangeRole(Player player, int Role)
        {
            if (!Plugin.Config.keepInvotory)
                player.Inventory.Clear();
            player.RoleID = Role;
        }
    }
}
