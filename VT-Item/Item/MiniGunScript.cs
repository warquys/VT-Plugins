﻿using Synapse.Api;
using Synapse.Api.Enum;
using Synapse.Api.Events.SynapseEventArguments;
using System;
using System.Collections.Generic;
using UnityEngine;
using VT_Referance.Script.ItemScript;
using VT_Referance.ItemScript;
using VT_Referance.Variable;
using VT_Referance.Method;
using VT_Referance.Behaviour;
using CustomPlayerEffects;

namespace VT_Item.Item
{
    class MiniGunScript : BaseWeaponScript
    {
        #region Attributes & Properties
        static Dictionary<Player, DateTime> StartShoot = new Dictionary<Player, DateTime>();
        static Dictionary<Player, DateTime> LastSoot = new Dictionary<Player, DateTime>();

        public override string MessagePickUp => Plugin.PluginTranslation.ActiveTranslation.MessageGetItem;
        public override string MessageChangeTo => Plugin.PluginTranslation.ActiveTranslation.MessageHandItem;
        public override ushort Ammos => 0;
        public override AmmoType AmmoType => (AmmoType)-1;
        public override int ID => (int)ItemID.MiniGun;
        public override ItemType ItemType => ItemType.GunLogicer;
        public override string Name => Plugin.PluginTranslation.ActiveTranslation.NameMiniGun;
        public override float Weight => 21.5f;
        public override int DamageAmmont => Plugin.MiniGunConfig.Damage;
        #endregion

        #region Constructors & Destructor
        public MiniGunScript()
        {
            DamageType = DamageTypes.Logicer; 
            ArmorPenetration = 3; 
            UseHitboxMultipliers = false;
        }
        #endregion

        #region Methods
        protected override void PickUp(PlayerPickUpItemEventArgs ev)
        {
            ev.Item.Durabillity = ev.Player.AmmoBox[AmmoType.Ammo762x39];
            base.PickUp(ev);
        }

        protected override void ChangeToItem(PlayerChangeItemEventArgs ev)
        {
            if (!Plugin.MiniGunConfig.CanMouveEquip && ev.Player.TryGetComponent<MinGunPlayerScript>(out var Script))
                Script.enabled = true;
            else if (!Plugin.MiniGunConfig.CanMouveEquip)
                ev.Player.gameObject.AddComponent<MinGunPlayerScript>();
            ev.NewItem.Durabillity = ev.Player.AmmoBox[AmmoType.Ammo762x39];
            base.ChangeToItem(ev);
        }

        protected override void ChangedFromItem(PlayerChangeItemEventArgs ev)
        {
            if (!Plugin.MiniGunConfig.CanMouveEquip && ev.Player.TryGetComponent<MinGunPlayerScript>(out var Script))
                Script.enabled = false;
            base.ChangedFromItem(ev);
        }

        protected override void Shoot(PlayerShootEventArgs ev)
        {
            ev.Weapon.Durabillity = ev.Player.AmmoBox[AmmoType.Ammo762x39];
            ev.Allow = false;
            if (LastSoot.ContainsKey(ev.Player))
            {
                if ((DateTime.Now - LastSoot[ev.Player]).TotalSeconds < Plugin.MiniGunConfig.TimeFir)
                {
                    ev.Allow = true;
                    LastSoot[ev.Player] = DateTime.Now;
                }
                else
                {
                    LastSoot.Remove(ev.Player);
                }
            }
            if (!ev.Allow)
            {
                if (StartShoot.ContainsKey(ev.Player))
                {
                    if ((DateTime.Now - StartShoot[ev.Player]).TotalSeconds >= Plugin.MiniGunConfig.TimeFir)
                    {
                        LastSoot[ev.Player] = DateTime.Now;
                        StartShoot.Remove(ev.Player);
                        ev.Allow = true;
                    }
                }
                else
                {
                    StartShoot[ev.Player] = DateTime.Now;
                }
            }

            if (ev.Allow)
            {
                ev.Player.GiveEffect(Effect.Ensnared, 1, 2);
                ev.Player.AmmoBox[AmmoType.Ammo762x39] -= (ushort)MultiShoot(ev.Player, ev.Weapon);
                ev.Allow = false;
            }
        }

        private Quaternion RandomAimcone()
        {
            return Quaternion.Euler(
                UnityEngine.Random.Range(-Plugin.MiniGunConfig.AimCone, Plugin.MiniGunConfig.AimCone),
                UnityEngine.Random.Range(-Plugin.MiniGunConfig.AimCone, Plugin.MiniGunConfig.AimCone),
                UnityEngine.Random.Range(-Plugin.MiniGunConfig.AimCone, Plugin.MiniGunConfig.AimCone)
                );
        }

        private int MultiShoot(Player player, Synapse.Api.Items.SynapseItem Weapon)
        {

            player.PlayerInteract.CallMethod("OnInteract");

            int bullets = Plugin.MiniGunConfig.bullets;
            if (Weapon.Durabillity <= bullets)
                bullets = (int)Weapon.Durabillity;
            Ray[] rays = new Ray[bullets];
            for (int i = 0; i < rays.Length; i++)
                rays[i] = new Ray(player.CameraReference.position + player.CameraReference.forward, RandomAimcone() * player.CameraReference.forward);

            RaycastHit[] hits = new RaycastHit[bullets];
            bool[] didHit = new bool[hits.Length];
            for (int i = 0; i < hits.Length; i++)
                didHit[i] = Physics.Raycast(rays[i], out hits[i], 100f, 1208246273);

            bool hit = false;
            for (int i = 0; i < hits.Length; i++)
            {
                if (!didHit[i]) continue;

                HitboxIdentity hitbox = hits[i].collider.GetComponent<HitboxIdentity>();
                if (hitbox != null)
                {
                    var target = hits[i].collider.GetComponentInParent<Player>();
                    if (target == player)
                        continue;

                    if (SynapseExtensions.GetHarmPermission(player, target))
                    {
                        hitbox.Damage(Plugin.MiniGunConfig.Damage, this, new Footprinting.Footprint(Weapon.ItemHolder.Hub), Weapon.ItemHolder.Position);

                        //component.RpcPlaceDecal(true, (sbyte)target.ClassManager.Classes.SafeGet(target.RoleType).bloodType, hits[i].point + hits[i].normal * 0.01f, Quaternion.FromToRotation(Vector3.up, hits[i].normal));
                        hit = true;
                    }
                    
                    continue;
                }

                IDestructible window = hits[i].collider.GetComponent<IDestructible>();
                if (window != null)
                {
                    window.Damage(Plugin.MiniGunConfig.Damage, this, new Footprinting.Footprint(Weapon.ItemHolder.Hub), Weapon.ItemHolder.Position);
                    hit = true;
                    continue;
                }

                //component.RpcPlaceDecal(false, component.curWeapon, hits[i].point + hits[i].normal * 0.01f, Quaternion.FromToRotation(Vector3.up, hits[i].normal));
            }

            if (hit) player.GetComponent<Hitmarker>().Trigger();
            return bullets;
        }

        private class MinGunPlayerScript : BaseRepeatingBehaviour
        {
            private Player player;

            MinGunPlayerScript() => base.RefreshTime = 480;

            protected override void Start()
            {
                player = gameObject.GetPlayer();
                base.Start();
            }

            protected override void BehaviourAction()
            {
                PlayerEffect effect1 = player.PlayerEffectsController.GetEffect<Disabled>();
                if (effect1 == null || effect1.Duration < 1 )
                    player.GiveEffect(Effect.Disabled, 1, 0.5f);

                PlayerEffect effect2 = player.PlayerEffectsController.GetEffect<Ensnared>();
                if ((effect2 == null || effect2.Duration < 1) && !Plugin.MiniGunConfig.ByPasseID.Contains(player.RoleID))
                    player.GiveEffect(Effect.Ensnared, 1, 0.5f);

                if (player.ItemInHand.ID != (int)ItemID.MiniGun)
                    this.enabled = false;
            }
        }
        #endregion
    }
}
