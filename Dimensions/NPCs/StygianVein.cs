using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Idglibrary;
using Microsoft.Xna.Framework.Audio;
using SGAmod.Buffs;
using SubworldLibrary;
using SGAmod.Items;

namespace SGAmod.Dimensions.NPCs
{

    public class StygianVein : ModNPC,IPostEffectsDraw
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stygian Vein");
        }

        public override void SetDefaults()
        {
            npc.width = 42;
            npc.height = 42;
            npc.damage = 0;
            npc.defense = 0;
            npc.lifeMax = 1000;
            //npc.HitSound = SoundID.Ti;
            //npc.DeathSound = SoundID.LiquidsWaterHoney;
            npc.value = 0f;
            npc.knockBackResist = 1.1f;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.aiStyle = -1;
            npc.alpha = 0;
            npc.chaseable = false;
            npc.SGANPCs().dotImmune = true;
            npc.SGANPCs().TimeSlowImmune = true;
            for (int buff=0;buff<npc.buffImmune.Length;buff++)
            {
                npc.buffImmune[buff] = true;
            }
        }
        public override string Texture
        {
            get { return "Terraria/SunOrb"; }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            Main.PlaySound(SoundID.Tink, (int)npc.Center.X, (int)npc.Center.Y, 0, 1, 0.65f);
            npc.ai[1] = ((int)damage*25)+120;
            if (npc.life < 1)
            {
                SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_EtherianPortalDryadTouch, (int)npc.Center.X, (int)npc.Center.Y);
                if (sound != null)
                {
                    sound.Pitch = -0.5f;
                }
            }

        }

        public override void NPCLoot()
        {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<StygianCore>());
            SpookyDarkSectorEye.Release(npc.Center, false, Main.rand.NextVector2Circular(32f, 32f));
        }

        public override void AI()
        {
            NullWatcher.DoAwarenessChecks((int)npc.ai[1], false, true, npc.Center);
            npc.ai[1] = 0;
            npc.ai[0] += 1;
            npc.velocity = new Vector2(0, (float)Math.Cos(npc.ai[0] / 90f)) / 3f;
            foreach (NPC enemy in Main.npc.Where(testnpc => testnpc.active && !testnpc.friendly && testnpc.type != npc.type && testnpc.Distance(npc.Center) < 720))
            {
                enemy.AddBuff(BuffID.Invisibility, 120);
            }

        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }

        public void PostEffectsDraw(SpriteBatch spriteBatch,float drawScale=2f)
        {

            Texture2D inner = Main.npcTexture[npc.type];

            Vector2 drawOrigin = inner.Size()/ 2f;


            Vector2 drawPos = (npc.Center - Main.screenPosition)/ drawScale;
            float drawscale2 = 2f / drawScale;


            for (float i = 0; i < 1f; i += 0.10f)
            {
                float scale = (1f - 0.80f * (((Main.GlobalTime / 2f) + i) % 1f));
                Color color = Color.DarkMagenta * (1f - ((i + (Main.GlobalTime / 2f)) % 1f)) * 0.75f;

                float percent = (((Main.GlobalTime / 2f) + i) % 1f);

                Vector2 drawpos2 = new Vector2((float)Math.Sin((percent*MathHelper.Pi) - Main.GlobalTime*1.25f) *percent*26f, percent * -64f)* drawscale2;

                spriteBatch.Draw(inner, drawPos + drawpos2, null, color, i * MathHelper.TwoPi, drawOrigin, drawscale2 * scale, SpriteEffects.None, 0f);
            }

            for (float i = 0; i < 1f; i += 0.10f)
            {
                float scale = (1f - 0.25f * (((Main.GlobalTime / 2f) + i) % 1f));
                Color color = Color.Magenta * (1f - ((i + (Main.GlobalTime / 2f)) % 1f)) * 0.75f;

                spriteBatch.Draw(inner, drawPos, null, color, i * MathHelper.TwoPi, drawOrigin, drawscale2 * scale, SpriteEffects.None, 0f);
            }

        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            Tile tile = Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY];

            if (DimDingeonsWorld.darkSectors.Count < 1)
                return 0f;

            float chance = 0f;

            if (Main.rand.Next(7) == 0)
            {
                Vector2 spawnPos = new Vector2(spawnInfo.spawnTileX * 16, spawnInfo.spawnTileY * 16);
                foreach (DarkSector sector in DimDingeonsWorld.darkSectors)
                {
                    if (!Main.npc.Any(npc => npc.active && npc.type == ModContent.NPCType<StygianVein>() && Vector2.Distance(npc.Center, spawnPos) <= 720)
                    && (sector.PointInside(new Vector2(spawnInfo.spawnTileX * 16, spawnInfo.spawnTileY * 16), 8)))
                        chance = 10f;
                }
            }
                return chance;
            
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return item.pick>0;
        }
        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            return player!=null && player.heldProj == projectile.whoAmI && player.HeldItem.pick>0;
        }
    }

}
