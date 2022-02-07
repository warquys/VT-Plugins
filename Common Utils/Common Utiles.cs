﻿using Synapse.Api.Plugin;

namespace Common_Utiles
{

    [PluginInformation(
        Name = "Common Utiles",
        Author = "Oka",
        Description = "add new functionality and config",
        LoadPriority = 5,
        SynapseMajor = SynapseController.SynapseMajor,
        SynapseMinor = SynapseController.SynapseMinor,
        SynapsePatch = SynapseController.SynapsePatch,
        Version = "v.1.4.0"
        )]
    public class CommonUtiles : VT_Api.Core.Plugin.VtAbstractPlugin<EventHandlers, Config>
    {
        public override bool AutoRegister => false;
    }
}