using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Enums;
using Idglibrary;
using Idglibrary.Bases;
using AAAAUThrowing;
using CalamityMod.Dusts;
using SGAmod.Buffs;

namespace SGAmod.Items.Weapons.Javelins
{
    public enum JavelinType : byte
    {
        Stone,
        Ice,
        Corruption,
        Crimson,
        Amber,
        Dynasty,
        Hallowed,
        Shadow,
        SanguineBident,
        TerraTrident,
        CrimsonCatastrophe,
        Thermal,
        SwampSovnya
    }

    public class StoneJavelin : ModItem
    {

        //public delegate int PerformCalculation(int x, int y);
        //public Action<string> messageTarget;
        public Func<int, int, bool> testForEquality = (x, y) => x == y;
        public virtual int Penetrate => 3;
        public virtual int PierceTimer => 100;
        public virtual float Stabspeed => 1.20f;
        public virtual float Throwspeed => 6f;
        public virtual float Speartype => 0;
        public virtual int[] Usetimes => new int[] { 30,10};
        public virtual string[] Normaltext => new string[] { "It's a jab-lin made from stone" };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stone Jab-lin");
            //Tooltip.SetDefault("Shoots a spread of bullets");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wood, 5);
            recipe.AddIngredient(ItemID.StoneBlock, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
            //messageTarget = delegate(string s) { bool thisisit = testForEquality(2,6); };


            //messageTarget("this");
        }

        public void drawstuff(SpriteBatch spriteBatch, Vector2 position, Color drawColor, Color itemColor, float scale, bool inventory = true)
        {
            Texture2D textureSpear = ModContent.GetTexture(JavelinProj.tex[(int)Speartype] + "Spear");
            Texture2D textureJave = ModContent.GetTexture(JavelinProj.tex[(int)Speartype]);
            Vector2 slotSize = new Vector2(52f, 52f);
            Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
            Vector2 textureOrigin = new Vector2(textureSpear.Width / 2, textureSpear.Height / 2);

            spriteBatch.Draw(textureSpear, drawPos - new Vector2(8, 8), null, drawColor, 0f, textureOrigin, inventory ? scale : Main.inventoryScale, SpriteEffects.None, 0f);
            spriteBatch.Draw(textureJave, drawPos - new Vector2(8, 8), null, drawColor, 0f, textureOrigin, inventory ? scale : Main.inventoryScale, SpriteEffects.FlipHorizontally, 0f);
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
            drawstuff(spriteBatch,position,drawColor,itemColor,scale);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            drawstuff(spriteBatch, (item.Center-Main.screenPosition) - new Vector2(8, 8), lightColor, lightColor, scale,true);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
            if (tt != null)
            {
                string[] thetext = tt.text.Split(' ');
                string newline = "";
                List<string> valuez = new List<string>();
                foreach (string text2 in thetext)
                {
                    valuez.Add(text2 + " ");
                }
                valuez.Insert(1, "Melee/Throwing ");
                foreach (string text3 in valuez)
                {
                    newline += text3;
                }
                tt.text = newline;
            }

            tt = tooltips.FirstOrDefault(x => x.Name == "CritChance" && x.mod == "Terraria");
            if (tt != null)
            {
                string[] thetext = tt.text.Split(' ');
                string newline = "";
                List<string> valuez = new List<string>();
                int counter = 0;
                foreach (string text2 in thetext)
                {
                    counter += 1;
                    if (counter>1)
                    valuez.Add(text2 + " ");
                }
                int thecrit = Main.GlobalTime % 3f >= 1.5f ? Main.LocalPlayer.meleeCrit : ThrowingUtils.DisplayedCritChance(item);
                string thecrittype = Main.GlobalTime % 3f >= 1.5f ? "Melee " : "Throwing ";
                valuez.Insert(0, thecrit+"% "+ thecrittype);
                foreach (string text3 in valuez)
                {
                    newline += text3;
                }
                tt.text = newline;
            }

            foreach ( string line in Normaltext){
            tooltips.Add(new TooltipLine(mod, "JavaLine", line));
            }
            tooltips.Add(new TooltipLine(mod, "JavaLine1", "Left click to quickly jab like a spear (melee damage done, may break after using if consumable)"));
            tooltips.Add(new TooltipLine(mod, "JavaLine1", "Right click to more slowly throw (throwing damage done, benefits from throwing velocity)"));
            if (item.consumable)
            tooltips.Add(new TooltipLine(mod, "JavaLine1", "Melee attacks have a solid 50% chance to not be consumed"));
            tooltips.Add(new TooltipLine(mod, "JavaLine1", "Thrown jab-lins stick into foes and do extra damage"));
            tooltips.Add(new TooltipLine(mod, "JavaLine1", "benefits from Throwing item saving chance and melee attack speed"));
        }

        public override bool ConsumeItem(Player player)
        {
            if (player.altFunctionUse != 2 && Main.rand.Next(0, 100) < 50)
                return false;
            return TrapDamageItems.SavingChanceMethod(player,true);
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.DayBreak);
            item.damage = 8;
            item.width = 24;
            item.height = 24;
            item.useTime = 25;
            item.useAnimation = 25;
            item.noMelee = true;
            item.melee = false;
            item.ranged = false;
            item.magic = false;
            item.thrown = false;
            item.knockBack = 5;
            item.reuseDelay = 1;
            item.value = 100;
            item.consumable = true;
            item.rare = 1;
            item.maxStack = 999;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("JavelinProj");
            item.shootSpeed = 1f;

        }
        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            mult *= (player.Throwing().thrownDamage + player.meleeDamage) / 2f;
            add += player.GetModPlayer<SGAPlayer>().JavelinBaseBundle ? 0.10f : 0f;
            add += player.GetModPlayer<SGAPlayer>().JavelinSpearHeadBundle ? 0.15f : 0f;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public virtual void OnThrow(int type, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type2, ref int damage, ref float knockBack, JavelinProj jav)
        {
//nullz
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.useTime = (int)((Usetimes[0] * player.meleeSpeed)/player.GetModPlayer<SGAPlayer>().ThrowingSpeed);
                item.useAnimation = (int)((Usetimes[0] * player.meleeSpeed) / player.GetModPlayer<SGAPlayer>().ThrowingSpeed);
                item.shootSpeed = 1f;

            }
            else
            {
                SGAPlayer sgaply = player.GetModPlayer<SGAPlayer>();
                item.useTime = (int)((Usetimes[1] * (player.meleeSpeed)));
                item.useAnimation = (int)((Usetimes[1] * (player.meleeSpeed)));
                item.shootSpeed = (1f / player.meleeSpeed)*sgaply.UseTimeMultiplier(this.item);
            }
                return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 normalizedspeed = new Vector2(speedX, speedY);
            normalizedspeed.Normalize();
            bool melee = true;
            float basemul = item.shootSpeed;
            if (player.altFunctionUse == 2)
            {
                normalizedspeed *= (Throwspeed*player.Throwing().thrownVelocity);
                normalizedspeed.Y -= Math.Abs(normalizedspeed.Y* 0.1f);
                Vector2 perturbedSpeed = new Vector2(normalizedspeed.X, normalizedspeed.Y).RotatedByRandom(MathHelper.ToRadians(10));
                float scale = 1f - (Main.rand.NextFloat() * .01f);
                perturbedSpeed = perturbedSpeed * scale;
                speedX = normalizedspeed.X; speedY = normalizedspeed.Y;
                item.useStyle = 1;
                type = mod.ProjectileType("JavelinProj");
                melee = false;
            }
           else
            {
                normalizedspeed *= Stabspeed* basemul;
                Vector2 perturbedSpeed = new Vector2(normalizedspeed.X, normalizedspeed.Y).RotatedByRandom(MathHelper.ToRadians(10));
                float scale = 1f - (Main.rand.NextFloat() * .01f);
                perturbedSpeed = perturbedSpeed * scale;
                item.useStyle = 5;
                speedX = normalizedspeed.X; speedY = normalizedspeed.Y;
                type = mod.ProjectileType("JavelinProjMelee");
            }

            int thisoned = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, Main.myPlayer);
            Main.projectile[thisoned].ai[1] = Speartype;
            Main.projectile[thisoned].melee = melee;
            Main.projectile[thisoned].Throwing().thrown = !melee;
            if (player.altFunctionUse == 2 && Speartype==7)
            Main.projectile[thisoned].aiStyle = 18;
            if (player.altFunctionUse == 2)
            {
                (Main.projectile[thisoned].modProjectile as JavelinProj).maxstick += (player.GetModPlayer<SGAPlayer>().JavelinSpearHeadBundle ? 1 : 0);
                (Main.projectile[thisoned].modProjectile as JavelinProj).maxStickTime = PierceTimer;
            }

            Main.projectile[thisoned].netUpdate = true;
            if (!melee)
            Main.projectile[thisoned].penetrate = Penetrate;
            IdgProjectile.Sync(thisoned);

            if (player.altFunctionUse == 2)
            {
            OnThrow(1, player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack, (Main.projectile[thisoned].modProjectile as JavelinProj));
            }
            else
            {
            OnThrow(0, player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack, (Main.projectile[thisoned].modProjectile as JavelinProj));
            }

                return false;

        }
    }

    public class JavelinProj : ModProjectile
    {
        public int stickin = -1;
        public Player P;
        public Vector2 offset;
        public int maxstick = 1;
        public int maxStickTime = 100;
        public int javelinType
        {
            get { return (int)projectile.ai[1]; }
        }
        static public string[] tex =
    {"SGAmod/Items/Weapons/Javelins/StoneJavelin",
        "SGAmod/Items/Weapons/Javelins/IceJavelin",
        "SGAmod/Items/Weapons/Javelins/CrimsonJavelin",
        "SGAmod/Items/Weapons/Javelins/CorruptionJavelin",
        "SGAmod/Items/Weapons/Javelins/AmberJavelin",
        "SGAmod/Items/Weapons/Javelins/DynastyJavelin",
        "SGAmod/Items/Weapons/Javelins/PearlWoodJavelin",
        "SGAmod/Items/Weapons/Javelins/ShadowJavelin",
        "SGAmod/Items/Weapons/Javelins/SanguineBident",
         "SGAmod/Items/Weapons/Javelins/TerraTrident",
         "SGAmod/Items/Weapons/Javelins/CrimsonCatastrophe",
         "SGAmod/Items/Weapons/Javelins/ThermalJavelin",
         "SGAmod/Items/Weapons/Javelins/SwampSovnya",
       };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Javelin");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(stickin);
            writer.Write(maxstick);
            writer.Write(maxStickTime);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            stickin = reader.ReadInt32();
            maxstick = reader.ReadInt32();
            maxStickTime = reader.ReadInt32();
        }
        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;
            projectile.friendly = true;
            projectile.melee = false;
            projectile.Throwing().thrown = true;
            projectile.tileCollide = true;
            projectile.penetrate = 5;
            projectile.timeLeft = 300;
            projectile.light = 0.25f;
            projectile.extraUpdates = 1;
            projectile.ignoreWater = true;
        }

        public override string Texture
        {
            get { return ("Terraria/Projectile_"+ ProjectileID.HallowStar); }
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (stickin>-1)
                return false;
            return base.CanHitNPC(target);
        }

        public static void JavelinOnHit(NPC target, Projectile projectile, ref double damage)
        {
        if (projectile.ai[1] == (int)JavelinType.Ice)//Ice
            {
                if (Main.rand.Next(0,4)==1)
                target.AddBuff(BuffID.Frostburn, 60 * (projectile.type==ModContent.ProjectileType<JavelinProj>() ? 2 : 3));
            }
            if (projectile.ai[1] == (int)JavelinType.Dynasty)//Dynasty
            {
                if (projectile.penetrate > 1)
                {
                    int thisoned = Projectile.NewProjectile(projectile.Center.X+ Main.rand.NextFloat(-64, 64), projectile.Center.Y - 800, Main.rand.NextFloat(-2,2), 14f, projectile.type, projectile.damage, projectile.knockBack, Main.player[projectile.owner].whoAmI);
                    Main.projectile[thisoned].ai[1] = projectile.ai[1];
                    Main.projectile[thisoned].Throwing().thrown = true;
                    Main.projectile[thisoned].penetrate = projectile.penetrate-1;
                    Main.projectile[thisoned].netUpdate = true;
                }
            }

            if (projectile.ai[1] == (int)JavelinType.Hallowed)//Hallow
            {
                if (Main.rand.Next(0, projectile.modProjectile.GetType() == typeof(JavelinProjMelee) ? 2 : 0) == 0)
                {

                    int thisoned = Projectile.NewProjectile(projectile.Center.X + Main.rand.NextFloat(-64, 64), projectile.Center.Y - 800, Main.rand.NextFloat(-2, 2), 14f, ProjectileID.HallowStar, (int)(projectile.damage * damage), projectile.knockBack, Main.player[projectile.owner].whoAmI);
                    Main.projectile[thisoned].ai[1] = projectile.ai[1];
                    Main.projectile[thisoned].Throwing().thrown = true;
                    Main.projectile[thisoned].penetrate = 2;
                    Main.projectile[thisoned].netUpdate = true;
                    IdgProjectile.Sync(thisoned);
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, thisoned);

                }
            }
            if (projectile.ai[1] == (int)JavelinType.Shadow)//Shadow
            {
                if (Main.rand.Next(0, 4) == 1)
                    target.AddBuff(BuffID.ShadowFlame, 60 * (projectile.type == ModContent.ProjectileType<JavelinProj>() ? 3 : 5));
            }
            if (projectile.ai[1] == (int)JavelinType.SanguineBident)//Sanguine Bident
            {
                
                if (projectile.modProjectile.GetType() == typeof(JavelinProj))
                {
                    if (target.active && target.life > 0 && Main.rand.Next(0,16)<(target.HasBuff(SGAmod.Instance.BuffType("MassiveBleeding")) || target.HasBuff(BuffID.Bleeding) ? 8 : 1))
                    {
                        projectile.vampireHeal((int)(projectile.damage / 2f), projectile.Center);
                    }
                }
                else
                {
                    target.AddBuff(SGAmod.Instance.BuffType("MassiveBleeding"), 60 * 5);
                }
                projectile.netUpdate = true;


            }
            if (projectile.ai[1] == (int)JavelinType.TerraTrident)//Terra Trident
            {
                if (projectile.modProjectile.GetType() == typeof(JavelinProj))
                {
                    
                    Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 60, 0.6f, 0.25f);
                    Vector2 velo = projectile.velocity; velo.Normalize();
                    int prog = Projectile.NewProjectile(projectile.Center.X + (velo.X * 20f), projectile.Center.Y + (velo.Y * 20f), velo.X * 6f, velo.Y * 6f, SGAmod.Instance.ProjectileType("TerraTridentProj"), (int)(projectile.damage*0.75), projectile.knockBack / 2f, Main.myPlayer);
                    Main.projectile[prog].penetrate = 3;
                    Main.projectile[prog].timeLeft /= 4;
                    Main.projectile[prog].melee = false;
                    Main.projectile[prog].Throwing().thrown = true;
                    Main.projectile[prog].netUpdate = true;
                    IdgProjectile.Sync(prog);
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, prog);
                }
            }
            if (projectile.ai[1] == (int)JavelinType.CrimsonCatastrophe)//Crimson Catastrophe
            {
                int bleed = SGAmod.Instance.BuffType("MassiveBleeding");
                ModProjectile modproj = projectile.modProjectile;
                if (modproj.GetType() == typeof(JavelinProj))
                {
                    (modproj as JavelinProj).maxStickTime = (int)((modproj as JavelinProj).maxStickTime/1.25);
                    foreach (NPC enemy in Main.npc.Where(enemy => enemy.active && enemy.active && enemy.life > 0 && !enemy.friendly && !enemy.dontTakeDamage && enemy.Distance(projectile.Center)<500 && (modproj as JavelinProj).stickin!= enemy.whoAmI && enemy.HasBuff(bleed)))
                    {
                        CrimsonCatastrophe.BloodyExplosion(enemy, projectile);
                    }
                }
                else
                {
                    target.AddBuff(bleed, 60 * 5);
                }
                projectile.netUpdate = true;


            }
            if (projectile.ai[1] == (int)JavelinType.Thermal)//Thermal
            {
                target.AddBuff(ModContent.BuffType<ThermalBlaze>(), 60 * (projectile.type == ModContent.ProjectileType<JavelinProj>() ? 2 : 5));
            }
            if (projectile.ai[1] == (int)JavelinType.SwampSovnya)//Swamp Sovnya
            {
                if (!target.buffImmune[BuffID.Poisoned])
                {
                    ModProjectile modproj = projectile.modProjectile;
                    if (modproj.GetType() == typeof(JavelinProj))
                    {
                        if (Main.rand.Next(0, 100) < 50 && !target.boss)
                            target.AddBuff(ModContent.BuffType<DankSlow>(), (int)(60 * 3f));
                    }
                }
                else
                {
                    damage += 0.25f;
                }
            }
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (projectile.ai[1] == (int)JavelinType.SwampSovnya)//Swamp Sovnya
            {
                if (target.buffImmune[BuffID.Poisoned])
                {
                    damage = (int)(damage * 1.25f);
                }
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.aiStyle = -1;
            double mul = 1.00;
            JavelinProj.JavelinOnHit(target,projectile,ref mul);
            damage = (int)(damage * mul);

            int foundsticker = 0;

            for (int i = 0; i < Main.maxProjectiles; i++) // Loop all projectiles
            {
                Projectile currentProjectile = Main.projectile[i];
                if (i != projectile.whoAmI // Make sure the looped projectile is not the current javelin
                    && currentProjectile.active // Make sure the projectile is active
                    && currentProjectile.owner == Main.myPlayer // Make sure the projectile's owner is the client's player
                    && currentProjectile.type == projectile.type // Make sure the projectile is of the same type as this javelin
                    && currentProjectile.modProjectile is JavelinProj JavelinProjz // Use a pattern match cast so we can access the projectile like an ExampleJavelinProjectile
                    && JavelinProjz.stickin == target.whoAmI)
                {
                    foundsticker += 1;
                   //projectile.Kill();
                }

            }

            if (foundsticker<maxstick)
            {

                if (projectile.penetrate > 1)
                {
                    offset = (target.Center - projectile.Center);
                    stickin = target.whoAmI;
                    projectile.netUpdate = true;
                }
            }

        }

        public override void AI()
        {
            projectile.localAI[0] -= 1f;
            float facingleft = projectile.velocity.X > 0 ? 1f : -1f;
            projectile.rotation = ((float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f) +(float)((Math.PI)*-0.25f);

            if (projectile.ignoreWater==true)
            {
                projectile.ignoreWater = false;
                if (Main.dedServ)
                {
                    Texture2D texture = ModContent.GetTexture(JavelinProj.tex[(int)projectile.ai[1]]);
                    int xx = (texture.Width - (int)(projectile.width)) / 2;
                    int yy = (texture.Height - (int)(projectile.height)) / 2;
                    projectile.position.X -= (xx / 2);
                    projectile.position.Y -= (yy / 2);
                    projectile.width = texture.Width / 2;
                    projectile.height = texture.Height / 2;
                }
            }

            if (stickin > -1)
            {
                NPC himz = Main.npc[stickin];
                projectile.tileCollide = false;

                if ((himz != null && himz.active) && projectile.penetrate>0)
                {
                    projectile.Center = (himz.Center - offset) - (projectile.velocity * 0.2f);
                    if (projectile.timeLeft < 100)
                    {
                        projectile.penetrate -= 1;
                        projectile.timeLeft = 100+maxStickTime;
                        double damageperc = 0.75;

                        if (Main.player[projectile.owner].GetModPlayer<SGAPlayer>().JavelinBaseBundle)
                            damageperc += 0.25;

                        JavelinProj.JavelinOnHit(himz,projectile,ref damageperc);

                        himz.StrikeNPC((int)(projectile.damage*damageperc),0,1);

                        if (Main.netMode != NetmodeID.SinglePlayer)
                        {
                            NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, himz.whoAmI, (float)(projectile.damage * damageperc), 16f, (float)1, 0, 0, 0);
                        }
                    }
                }
                else
                {
                    projectile.Kill();
                }

            }
            else
            {

                if (projectile.ai[1] == (int)JavelinType.CrimsonCatastrophe)//Sanguine Bident
                {
                    if (stickin < 0)
                    {
                        for (float i = 2f; i > 0.25f; i -= Main.rand.NextFloat(0.25f,1.25f))
                        {
                            Vector2 position = projectile.position + Vector2.Normalize(projectile.velocity)*Main.rand.NextFloat(0f,48f);
                            position -= new Vector2(projectile.width, projectile.height) / 2f;
                            Dust dust = Dust.NewDustDirect(position, projectile.height, projectile.width, DustID.Fire,0, 0, 200, Scale: 0.5f);
                            dust.velocity = new Vector2((projectile.velocity.X / 3f) * i, projectile.velocity.Y * i);

                            dust = Dust.NewDustDirect(position, projectile.height, projectile.width, DustID.Blood, 0, 0, 200, Scale: 0.5f);
                            dust.velocity = new Vector2(projectile.velocity.X * i, (projectile.velocity.Y / 3f) * i);
                        }
                    }

                }
                else
                {
                    if (Main.rand.NextBool(3))
                    {
                        Dust dust = Dust.NewDustDirect(projectile.position, projectile.height, projectile.width, DustID.LunarOre,
                        projectile.velocity.X * .2f, projectile.velocity.Y * .2f, 200, Scale: 0.5f);
                        dust.velocity += projectile.velocity * 0.3f;
                        dust.velocity *= 0.2f;
                    }
                }

                if (projectile.ai[1] == (int)JavelinType.CrimsonCatastrophe)
                {
                    if (projectile.aiStyle != 0)
                    {
                        float rotvalue = projectile.timeLeft/300f;
                        projectile.velocity = projectile.velocity.RotatedBy(((projectile.aiStyle + 100f) / 1200f) * -rotvalue);
                    }
                    return;
                }

                if (projectile.velocity.Y<16f)
                if (projectile.aiStyle < 1)
                projectile.velocity = projectile.velocity + new Vector2(0, 0.1f);
            }

        }

        public override bool PreKill(int timeLeft)
        {
            if (projectile.ai[1] == (int)JavelinType.SanguineBident)
            {
                for (int num315 = -40; num315 < 43; num315 = num315 + 4)
                {
                    int dustType = DustID.LunarOre;//Main.rand.Next(139, 143);
                    int dustIndex = Dust.NewDust(projectile.Center + new Vector2(-16, -16) + ((projectile.rotation-MathHelper.ToRadians(45)).ToRotationVector2() * num315), 32, 32, dustType);//,0,5,0,new Color=Main.hslToRgb((float)(npc.ai[0]/300)%1, 1f, 0.9f),1f);
                    Dust dust = Main.dust[dustIndex];
                    dust.velocity.X = projectile.velocity.X * 0.8f;
                    dust.velocity.Y = projectile.velocity.Y * 0.8f;
                    dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
                    dust.fadeIn = 0.25f;
                    dust.noGravity = true;
                    Color mycolor = Color.OrangeRed;//new Color(25,22,18);
                    dust.color = mycolor;
                    dust.alpha = 20;
                }
            }

            return true;
        }

        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
        {
            bool facingleft = projectile.velocity.X > 0;
            Microsoft.Xna.Framework.Graphics.SpriteEffects effect = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
            Texture2D texture = ModContent.GetTexture(JavelinProj.tex[(int)projectile.ai[1]]);
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);


            if (projectile.ai[1] == (int)JavelinType.CrimsonCatastrophe)
            {
                if (stickin >= 0)
                    drawColor = Color.Lerp(drawColor, Color.Red, 0.50f + (float)Math.Sin(projectile.timeLeft / 15f) / 3f);

                float sticktime = (float)maxStickTime;
                float alpha = 1;
                if (projectile.penetrate <= 1)
                    alpha *= MathHelper.Clamp((float)(projectile.timeLeft-100) / 10f, 0f, 1f);

                drawColor *= alpha;

                if (stickin >= 0)
                {
                    for (float k = 0; k < MathHelper.TwoPi; k += MathHelper.Pi / 2f)
                    {
                        float angle = (k + (float)projectile.whoAmI + Main.GlobalTime);
                        Vector2 drawPos = projectile.Center+Vector2.Normalize(angle.ToRotationVector2())*260;
                        Color color = drawColor * MathHelper.Clamp(projectile.localAI[0] / 10f, 0.25f, 1f);

                        spriteBatch.Draw(texture, drawPos - Main.screenPosition, new Rectangle?(), color * 0.5f, angle - (3f*MathHelper.Pi/4f), origin, projectile.scale, effect, 0);

                        angle = (k - (float)projectile.whoAmI - Main.GlobalTime);
                        float timedelay = (float)(projectile.timeLeft - 100) / sticktime;
                        float alpha2 = MathHelper.Clamp((float)Math.Sin((double)timedelay*Math.PI)*2f,0f, 1f);
                        float distancemul = 100f / sticktime;
                        drawPos = projectile.Center + Vector2.Normalize(angle.ToRotationVector2()) * (projectile.timeLeft-100)*(8f*distancemul);
                        color = drawColor * alpha2;

                        spriteBatch.Draw(texture, drawPos - Main.screenPosition, new Rectangle?(), color * 0.5f,MathHelper.Pi + angle - (3f * MathHelper.Pi / 4f), origin, projectile.scale, effect, 0);

                    }
                    return false;
                }

                for (int k = 0; k < projectile.oldPos.Length; k++)
                {
                    drawColor = Color.Lerp(drawColor, Color.Red, 0.50f + (float)Math.Sin((projectile.timeLeft + k) / 15f) / 3f);
                    Vector2 drawPos = new Vector2(projectile.oldPos[k].X + projectile.width / 2, projectile.oldPos[k].Y + projectile.height / 2) - Main.screenPosition;
                    Color color = projectile.GetAlpha(drawColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                    spriteBatch.Draw(texture, drawPos, new Rectangle?(), color * 0.5f* alpha, projectile.rotation + (facingleft ? (float)(1f * Math.PI) : 0f) - (((float)Math.PI / 2) * (facingleft ? 0f : -1f)), origin, projectile.scale, facingleft ? effect : SpriteEffects.FlipHorizontally, 0);
                }
                return false;
            }//Crimson Catastrophe


            if (projectile.ai[1] == (int)JavelinType.SanguineBident || projectile.ai[1] == (int)JavelinType.Thermal)//Sanguine Bident
            {           
                Texture2D glow = null;
                if (projectile.ai[1] == (int)JavelinType.Thermal)
                {
                    glow = ModContent.GetTexture("SGAmod/Items/GlowMasks/ThermalJavelin_Glow");
                }
                    for (int k = 0; k < (stickin < 0 ? projectile.oldPos.Length : 1); k++)
                    {
                        Vector2 drawPos = new Vector2(projectile.oldPos[k].X + projectile.width / 2, projectile.oldPos[k].Y + projectile.height / 2) - Main.screenPosition;
                        Color color = projectile.GetAlpha(drawColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                        spriteBatch.Draw(texture, drawPos, new Rectangle?(), color * 0.5f, projectile.rotation + (facingleft ? (float)(1f * Math.PI) : 0f) - (((float)Math.PI / 2) * (facingleft ? 0f : -1f)), origin, projectile.scale, facingleft ? effect : SpriteEffects.FlipHorizontally, 0);
                    if (glow != null)
                        spriteBatch.Draw(glow, drawPos, new Rectangle?(), color * 0.5f, projectile.rotation + (facingleft ? (float)(1f * Math.PI) : 0f) - (((float)Math.PI / 2) * (facingleft ? 0f : -1f)), origin, projectile.scale, facingleft ? effect : SpriteEffects.FlipHorizontally, 0);

                    }
            }


            Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle?(), drawColor, projectile.rotation + (facingleft ? (float)(1f * Math.PI) : 0f) - (((float)Math.PI / 2)*(facingleft ? 0f : -1f)), origin, projectile.scale, facingleft ? effect : SpriteEffects.FlipHorizontally, 0);
            return false;
        }
    }

    public class JavelinProjMelee : ProjectileSpearBase
    {

        bool mousecurser;
        public bool hitboxchange = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Javelin");
        }

        public override string Texture
        {
            get { return ("SGAmod/Items/Weapons/Javelins/StoneJavelin"); }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            double blank = 0;
            JavelinProj.JavelinOnHit(target, projectile,ref blank);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (projectile.ai[1] == (int)JavelinType.SwampSovnya)//Swamp Sovnya
            {
                if (projectile.ai[1] == (int)JavelinType.SwampSovnya)//Swamp Sovnya
                {
                    if (target.buffImmune[BuffID.Poisoned])
                    {
                        damage = (int)(damage * 1.25f);
                    }
                }
                if (target.HasBuff(ModContent.BuffType<DankSlow>()))
                {
                    crit = true;
                    int index = target.FindBuffIndex(ModContent.BuffType<DankSlow>());
                    damage = (int)(damage * Math.Min(1 + (target.buffTime[index] / 180f),5f));
                    target.DelBuff(index);
                }
            }
        }

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = false;
            projectile.penetrate = -1;
            projectile.ownerHitCheck = true;
            projectile.scale = 1.2f;
            projectile.knockBack = 1f;
            projectile.aiStyle = 19;
            projectile.melee = true;
            projectile.timeLeft = 90;
            projectile.hide = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;

            movein = 3f;
            moveout = 3f;
            thrustspeed = 3.0f;
            this.hitboxchange = false;
        }

        public override bool PreAI()
        {
            if (projectile.ignoreWater == false)
            {
                if (projectile.ai[1] == (int)JavelinType.CrimsonCatastrophe)
                {
                    projectile.extraUpdates = 1;
                    movein = 8f;
                    moveout = 5f;
                    thrustspeed = 5.0f;
                    projectile.localNPCHitCooldown = 1;
                }

                    if (Main.dedServ)
                {
                    Texture2D texture = ModContent.GetTexture(JavelinProj.tex[(int)projectile.ai[1]] + "Spear");
                    int xx = texture.Width - (int)((projectile.width) / 1.5f);
                    int yy = texture.Height - (int)((projectile.height) / 1.5f);
                    projectile.position.X -= (int)((xx / 1.5f));
                    projectile.position.Y -= (int)((yy / 1.5f));
                    projectile.width = (int)(texture.Width / 1.5f);
                    projectile.height = (int)(texture.Height / 1.5f);
                }
            }
            return true;
        }
        public override void MakeProjectile()
        {
            if (projectile.ai[1] == (int)JavelinType.TerraTrident)
            {
                Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 60, 0.6f, 0.25f);
                Vector2 velo = projectile.velocity;velo.Normalize();
                velo *= 8f;
                Projectile.NewProjectile(projectile.Center.X+ (velo.X*5f), projectile.Center.Y + (velo.Y * 5f), velo.X, velo.Y, mod.ProjectileType("TerraTridentProj"), (int)(projectile.damage * 0.75), projectile.knockBack, Main.myPlayer);
            }
        }

        public override void AI()
        {
            base.AI();

            if (projectile.owner == Main.myPlayer)
            {
                mousecurser = (Main.MouseScreen.X - projectile.Center.X)>0;
                projectile.direction = mousecurser.ToDirectionInt();
                projectile.netUpdate = true;


            }

        }

        public new float movementFactor
        {
            get { return projectile.ai[0]; }
            set { projectile.ai[0] = value; }
        }
        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
        {
            Player owner = Main.player[projectile.owner];
            bool facingleft = projectile.direction < 0f;
            Microsoft.Xna.Framework.Graphics.SpriteEffects effect = SpriteEffects.FlipHorizontally;
            Texture2D texture = ModContent.GetTexture(JavelinProj.tex[(int)projectile.ai[1]]+"Spear");
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Vector2 drawpos = projectile.Center;

            if (projectile.ai[1] == (int)JavelinType.CrimsonCatastrophe)
            {
                drawpos += Vector2.Normalize(projectile.Center - owner.Center) * 64f;
                drawColor *= 0.5f;// MathHelper.Clamp((float)(projectile.timeLeft-60f) / 25f,0, 1f);

                for (float i = 0; i < 1f && projectile.ai[1] == 10; i += 0.02f)
                {
                    Vector2 offset = (owner.Center - projectile.Center) * i;
                    drawColor *= (1f - (i / 8f));

                    if (facingleft)
                        Main.spriteBatch.Draw(texture, (drawpos + offset) - Main.screenPosition, new Rectangle?(), drawColor, ((projectile.rotation - (float)Math.PI / 2) - (float)Math.PI / 2) + (facingleft ? (float)(1f * Math.PI) : 0f), origin, projectile.scale, effect, 0);
                    if (!facingleft)
                        Main.spriteBatch.Draw(texture, (drawpos + offset) - Main.screenPosition, new Rectangle?(), drawColor, (projectile.rotation - (float)Math.PI / 2) + (facingleft ? (float)(1f * Math.PI) : 0f), origin, projectile.scale, SpriteEffects.None, 0);
                }

            }
            else
            {

                if (facingleft)
                    Main.spriteBatch.Draw(texture, (drawpos) - Main.screenPosition, new Rectangle?(), drawColor, ((projectile.rotation - (float)Math.PI / 2) - (float)Math.PI / 2) + (facingleft ? (float)(1f * Math.PI) : 0f), origin, projectile.scale, effect, 0);
                if (!facingleft)
                    Main.spriteBatch.Draw(texture, (drawpos) - Main.screenPosition, new Rectangle?(), drawColor, (projectile.rotation - (float)Math.PI / 2) + (facingleft ? (float)(1f * Math.PI) : 0f), origin, projectile.scale, SpriteEffects.None, 0);

            }



            return false;
        }

    }

}