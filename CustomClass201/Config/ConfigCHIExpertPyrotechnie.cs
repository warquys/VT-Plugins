﻿using Synapse.Config;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace CustomClass.Config
{
    public class ConfigCHIExpertPyrotechnie : AbstractConfigSection
    {
        [Description("The Amount of Health the class have")]
        public int Health = 120;

        [Description("The Inventory of the class")]
        public SerializedPlayerInventory inventory = new SerializedPlayerInventory()
        {
            Ammo = new SerializedAmmo(0, 100, 0),
            Items = new List<SerializedPlayerItem>() 
            { 
                new SerializedPlayerItem((int)ItemType.KeycardChaosInsurgency, 1, 0, 0, 0, Vector3.one, 100, true),
                new SerializedPlayerItem((int)ItemType.GunLogicer, 75, 0, 0, 0, Vector3.one, 100, true),
                new SerializedPlayerItem((int)ItemType.GrenadeFrag, 1, 0, 0, 0, Vector3.one, 100, true),
                new SerializedPlayerItem((int)ItemType.GrenadeFrag, 1, 0, 0, 0, Vector3.one, 100, true), 
                new SerializedPlayerItem((int)ItemType.GrenadeFrag, 1, 0, 0, 0, Vector3.one, 100, true),
                new SerializedPlayerItem((int)ItemType.GrenadeFrag, 1, 0, 0, 0, Vector3.one, 100, true),
                new SerializedPlayerItem((int)ItemType.Radio, 100, 0, 0, 0, Vector3.one, 100, true), 
                new SerializedPlayerItem((int)ItemType.WeaponManagerTablet, 1, 0, 0, 0, Vector3.one, 100, true)
            }
        };

        [Description("ArtificialHealthConfig of the class")]
        public int MaxArtificialHealth = 100;
        public int ArtificialHealth = 50;

        [Description("The Chance of which the class spawns")]
        public int SpawnChance = 25;

        [Description("Max alive at the same time")]
        public int MaxAlive = 100;

        [Description("max of this role which can spawn into a respawn")]
        public int MaxRespawn = 100;

        [Description("The number of players required in the same role to have the chance for the class to appear")]
        public int RequiredPlayers = 0;

        [Description("The name of the class")]
        public string RoleName = "Expert en explosif IC"; 
    }
}