﻿using Synapse.Api.Plugin;
using VT_Api.Core.Plugin;
using HarmonyLib;
using Synapse;
using System.Collections.Generic;
using MEC;
using Synapse.Api.Teams;
using VT_Api.Core.Roles;
using VT_Api.Extension;
using VT_Api.Reflexion;

namespace Common_Utiles
{
    /*
     * 
     *  *****  *****
     * **************
     *  ************
     *   *********
     *     *****
     *       * 
     */
    [PluginInformation(
        Name = "Common Utiles",
        Author = "Oka, update by Warkis",
        Description = "add new functionality and config",
        LoadPriority = 5,
        SynapseMajor = SynapseController.SynapseMajor,
        SynapseMinor = SynapseController.SynapseMinor,
        SynapsePatch = SynapseController.SynapsePatch,
        Version = "v.1.5.0"
        )]
    public class Plugin : VtAbstractPlugin<Plugin, EventHandlers, Config.Config>
    {
        public override bool AutoRegister => false;

        public bool RespawnAllow { get; set; }

        public Dictionary<int, List<int>> TeamIDRolesID { get; } = new Dictionary<int, List<int>>();

        public override void ReloadConfigs()
        {
            base.ReloadConfigs();
            //whait the realod of all other plugins
            Timing.CallDelayed(0f, () =>
            {
                 foreach (var team in Synapse.Api.Teams.TeamManager.Get.GetFieldValueOrPerties<List<ISynapseTeam>>("teams"))
                 {
                    int teamID = team.Info.ID;
                    var roles = VT_Api.Core.Roles.RoleManager.Get.GetRoles(teamID);
                    Plugin.Instance.TeamIDRolesID.Add(teamID, roles);
                 }

                 List<int> vanillateam = new List<int>()
                 {
                     0, 1, 2, 3, 4
                 };
                 foreach (var team in vanillateam)
                 {
                     var roles = RoleManager.Get.GetRoles(team);
                     Plugin.Instance.TeamIDRolesID.Add(team, roles);
                 }
            });
        }

    }
}