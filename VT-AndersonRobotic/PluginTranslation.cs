﻿using Synapse.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VT_AndersonRobotic
{
    public class PluginTranslation : IPluginTranslation
    {
        public string SpawnMessage { get; set; } = "You are a %RoleName% in the <color=yellow>Anderson-Robotic</color> Team\\nYour Goal is it to steal all data in the serveur\\n<b>Press Esc to close</b>";
    }
}
