using AAAAUThrowing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SGAmod.Dimensions.NPCs;
using SGAmod.Effects;
using SGAmod.Items.Weapons.Technical;
using SGAmod.Tiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Armors.Mandala
{

	[AutoloadEquip(EquipType.Head)]
	public class MandalaHood : ModItem
	{
        public override bool Autoload(ref string name)
        {
			if (GetType() == typeof(MandalaHood))
				SGAPlayer.PostUpdateEquipsEvent += SetBonus;

			return true;
        }
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mandala Hood");
			Tooltip.SetDefault("+1 minion slot\n10% increased Summon Damage and Summon weapon Use Speed\nAdditional 10 defense and minion slot in Subworlds");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0,15,0,0);
			item.rare = ItemRarityID.Cyan;
			item.defense = 12;
			item.lifeRegen = 0;
		}
        public static void SwapModes(SGAPlayer sgaplayer)
        {
            sgaplayer.mandalaSet.Item2 += 1;
            if (!Main.dedServ)
            {
                SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_WitherBeastAuraPulse, -1, -1);
                if (sound != null)
                {
                    sound.Pitch = 0.95f;
                }
            }
        }

        public static void SetBonus(SGAPlayer sgaplayer)
		{
			if (sgaplayer.mandalaSet.Item1)
			{
				Player player = sgaplayer.player;
                if (player.ownedProjectileCounts[ModContent.ProjectileType<MandalaSummonProj>()] < 1)
                {
                    Projectile.NewProjectile(player.Center + new Vector2(-player.direction*80f, -600f), Vector2.UnitY * 24, ModContent.ProjectileType<MandalaSummonProj>(), 0, 15f, player.whoAmI);
                }

			}
		}

		public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod,typeof(SGAPlayer).Name) as SGAPlayer;

			player.maxMinions += 1;
			player.minionDamage += 0.10f;
			sgaplayer.summonweaponspeed += 0.10f;

			if (Dimensions.SGAPocketDim.WhereAmI != null)
			{
				player.statDefense += 10;
				player.maxMinions += 1;
			}

		}
		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ)
				sgaplayer.armorglowmasks[0] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<OverseenCrystal>(), 40);
			recipe.AddIngredient(ModContent.ItemType<OmniSoul>(), 6);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class MandalaChestplate : MandalaHood
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mandala Chestplate");
			Tooltip.SetDefault("+2 minion slots\n12% increased Summon Damage and 20% Summon weapon Use Speed\nAdditional life regen in Subworlds");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 20, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.defense = 20;
			item.lifeRegen = 0;
		}
		public override void UpdateEquip(Player player)
		{
			player.minionDamage += 0.12f;
			player.maxMinions += 2;
			player.SGAPly().summonweaponspeed += 0.20f;

			if (Dimensions.SGAPocketDim.WhereAmI != null)
            {
				player.lifeRegen += 6;
            }
		}

		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ)
			{
				sgaplayer.armorglowmasks[1] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
				sgaplayer.armorglowmasks[2] = "SGAmod/Items/GlowMasks/" + Name + "_GlowArms";
			}
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class MandalaLeggings : MandalaHood
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mandala Leggings");
			Tooltip.SetDefault("+1 minion slot\n10% increased Summon Damage and Summon weapon use Speed\nAdditional flying speed and wing time in Subworlds");
		}
        public override bool Autoload(ref string name)
        {
            SGAPlayer.PostPostUpdateEquipsEvent += SGAPlayer_PostPostUpdateEquipsEvent;
			return true;
        }
        private void SGAPlayer_PostPostUpdateEquipsEvent(SGAPlayer player)
        {
            if (!player.player.armor[2].IsAir && player.player.armor[2].type == item.type)
            {
                if (Dimensions.SGAPocketDim.WhereAmI != null)
                {
                    player.player.wingTime += 60;
                    player.player.wingTimeMax += 60;
                }
			}
		}

        public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.defense = 10;
			item.lifeRegen = 0;
		}
		public override void UpdateEquip(Player player)
		{
			player.maxMinions += 1;
			player.minionDamage += 0.10f;
			player.SGAPly().summonweaponspeed += 0.10f;
		}
		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ)
				sgaplayer.armorglowmasks[3] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
		}
	}

    public class ManifestedMandalaControls : ModItem, IManifestedItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Overseer's Tutelage");
            Tooltip.SetDefault("Command the Shadowspirit's arms to attack with 2 attack types:\nDeliver fast short ranged non-piercing punches\nGrab and toss asteriods that can pierce\nAsteriods manifest around you over time, up to 10 plus 2 per sentry slot\nGains more arms per max Minion slots");
        }
        public override string Texture => "SGAmod/Dimensions/Space/OverseenHead";

        public override void SetDefaults()
        {
            //item.CloneDefaults(ItemID.ManaFlower);
            item.width = 16;
            item.height = 16;
            item.rare = ItemRarityID.Blue;
            item.value = 0;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.damage = 300;
            item.summon = true;
            item.shootSpeed = 6f;
            item.shoot = ModContent.ProjectileType<ManifestedMandalaControlProj>();
            item.useTurn = true;
            //ProjectileID.CultistBossLightningOrbArc
            item.width = 16;
            item.height = 16;
            item.useAnimation = 20;
            item.useTime = 20;
            item.reuseDelay = 0;
            item.knockBack = 1;
            //item.UseSound = SoundID.Item1;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.channel = true;
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Desert.ManifestedSandTosser.DrawManifestedItem(item, spriteBatch, position, frame, scale);

            return true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return false;
        }

        public override bool UseItem(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {

            //Do alt fire whatever here

            return true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            string s = "Not Binded!";
            foreach (string key in SGAmod.ToggleRecipeHotKey.GetAssignedKeys())
            {
                s = key;
            }
            tooltips.Add(new TooltipLine(mod, "MaldalaTooltip", "Press the 'Toggle Recipe' (" + s + ") hotkey to swap modes"));
        }

    }

    public class ManifestedMandalaControlProj : NovaBlasterCharging
    {

        public override int chargeuptime => 100;
        public override float velocity => 12f;
        public override float spacing => 32f;
        public override int fireRate => 4;
        int chargeUpTimer = 0;
        public override int FireCount => 1;
        public override (float, float) AimSpeed => (1f, 1f);
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mandola Command");
        }
        public override string Texture
        {
            get { return "SGAmod/Dimensions/Space/BlueAsteroidSmall"; }
        }

        public override void SetDefaults()
        {
            //projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
            projectile.width = 16;
            projectile.height = 16;
            projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.tileCollide = false;
            aiType = 0;
        }

        public override void ChargeUpEffects()
        {
            foreach (Projectile proj in Main.projectile.Where(testby => testby.active && testby.owner == projectile.owner && testby.type == ModContent.ProjectileType<MandalaSummonProj>()))
            {
                proj.damage = projectile.damage;
                MandalaSummonProj proj2 = proj.modProjectile as MandalaSummonProj;
                if (player.SGAPly().mandalaSet.Item2 % 2 == 0)
                    proj2.punching = 5;
                else
                    proj2.throwing = 5;
            }
        }

        public override bool DoChargeUp()
        {
            return true;
        }

        public override void FireWeapon(Vector2 direction)
        {
            float perc = MathHelper.Clamp(projectile.ai[0] / (float)chargeuptime, 0f, 1f);


            float speed = velocity;

            Vector2 perturbedSpeed = (new Vector2(direction.X, direction.Y) * speed).RotatedBy(Main.rand.NextFloat(-MathHelper.Pi / 16f, MathHelper.Pi / 16f));

            int damage = (int)(projectile.damage);

            projectile.Kill();
        }
    }

    public class ManifestedMandalaControlAltProj : ManifestedMandalaControlProj
    {

        public override void ChargeUpEffects()
        {
            foreach (Projectile proj in Main.projectile.Where(testby => testby.active && testby.owner == projectile.owner && testby.type == ModContent.ProjectileType<MandalaSummonProj>()))
            {
                proj.damage = projectile.damage;
                MandalaSummonProj proj2 = proj.modProjectile as MandalaSummonProj;
                proj2.throwing = 5;
            }
        }
    }

    public class MandalaSummonProj : ModProjectile
    {
        Vector2 eyeLookAt;
        public Vector2 eyeCurrentLook;
        public int punching = 0;
        public int throwing = 0;

        public List<MandalaArm> arms = new List<MandalaArm>();

        Vector2 DrawPosition => Vector2.Lerp(projectile.Center, new Vector2((Owner.MountedCenter.X + projectile.Center.X) / 2f, Owner.MountedCenter.Y + projectile.rotation - 64), MathHelper.Clamp(projectile.timeLeft / 60f, 0f, Math.Min(projectile.localAI[0] / 60f, 1f)));

        public List<Projectile> Asteriods => Main.projectile.Where(testby => testby.active && testby.owner == Owner.whoAmI && testby.type == ModContent.ProjectileType<MandalaAsteriodProj>()).ToList();
        public List<Projectile> GrabbableAsteriods => Asteriods.Where(testby => testby.localAI[0] > 30 && testby.timeLeft > 60 && testby.velocity.LengthSquared()<5 && testby.ai[0] < 2 && testby.damage < 1).ToList();

        public class MandalaArm
        {
            private Vector2 position;
            public Vector2 followPosition;

            public Vector2 Position
            {
                get
                {
                    return Vector2.Lerp(position, followPosition, armLerp*0.85f);
                }
                set
                {
                    position = value;
                }
            }

            public float time = 0;
            public float armLerp = 0f;
            public float armLerpGoal = 1f;
            public float angle = 0;
            public float angleSpin = 0;
            public float spinFriction = 1f;
            public int state = 100;
            public int stateVar = 0;
            public float scale = 1f;
            public Vector2 velocity = Vector2.Zero;
            public float friction = 0.95f;
            public Texture2D asteriod;
            public Player player;
            public Vector2 direction = Vector2.Zero;
            public int cooldown = 0;
            public int despawnTimer = 10;
            public float chargeIn = 0;
            public float chargeInDist = 0;
            public float Percent => index/(float)maxIndex;
            public int index = 0;
            public int maxIndex = 0;
            public Projectile rockToGrab;

            public MandalaArm(Vector2 position, Player player,int index,int maxIndex)
            {
                this.position = position;
                this.followPosition = player.MountedCenter;
                Texture2D[] astertype = new Texture2D[] { ModContent.GetTexture("SGAmod/Dimensions/Space/BlueAsteroidSmall"), ModContent.GetTexture("SGAmod/Dimensions/Space/BlueAsteroidSmall2") };
                asteriod = astertype[Main.rand.Next(astertype.Length)];
                scale = 1f;// Main.rand.NextFloat()
                angle = Main.rand.NextFloat(MathHelper.TwoPi);
                angleSpin = Main.rand.NextFloat(0.1f,0.2f)*(Main.rand.NextBool() ? 1f : -1f);
                this.index = index;
                this.maxIndex = maxIndex;
                this.player = player;


                SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_GhastlyGlaiveImpactGhost, (int)position.X, (int)position.Y);
                if (sound != null)
                {
                    sound.Pitch = -0.5f;
                }
            }
            public void ChangeState(int state)
            {
                this.state = state;
                stateVar = 0;
            }
            public void Update(MandalaSummonProj owner)
            {
                time += 1;
                stateVar += 1;

                switch (state)
                {

                    //Intro
                    case 100:
                        armLerpGoal = 0f;
                        position += (followPosition - position) * 0.025f;
                        if (stateVar >= 100)
                        {
                            ChangeState(0);
                        }
                        break;

                    //Punching
                    case 1:
                        armLerpGoal = 1;
                        Vector2 there = Position;

                        chargeIn += (1.20f - chargeIn) / 4f;
                        chargeIn = Math.Min(chargeIn, 1f);
                        chargeInDist += 1f;

                        spinFriction += (0f - spinFriction) / 4f;

                        if (stateVar<5)
                        velocity += Vector2.Normalize(direction) * MathHelper.Clamp(stateVar / 12f, 0f, 1f) * 720f;

                        if (stateVar >= 10 || cooldown>0)
                        {
                            ChangeState(10);
                            //armLerpGoal = 1f;
                        }
                        break;

                    //Grabbing Asteriod
                    case 2:
                        armLerpGoal = 0;
                        armLerp /= 1.25f;

                        velocity *= 0.98f;

                        if (rockToGrab == null || !rockToGrab.active || rockToGrab.type != ModContent.ProjectileType<MandalaAsteriodProj>())
                        {
                            ChangeState(10);
                            //armLerpGoal = 1f;
                        }
                        else
                        {
                            position = rockToGrab.Center;

                            rockToGrab.ai[0] = 10;
                            rockToGrab.netUpdate = true;

                            if ((rockToGrab.Center-Position).Length()<8 && armLerp<0.02f)
                            {
                                ChangeState(3);
                            }
                        }
                        break;

                    //Grabbed Asteriod, tossing
                    case 3:

                        if (stateVar > 20)
                        {
                            velocity += Vector2.Normalize(direction) * MathHelper.Clamp((stateVar - 20) / 20f, 0f, 1f) * 16f;
                        }
                        else
                        {
                            position += (followPosition- position) / 16f;
                        }

                        if (rockToGrab == null || !rockToGrab.active || rockToGrab.type != ModContent.ProjectileType<MandalaAsteriodProj>())
                        {
                            ChangeState(10);
                            //armLerpGoal = 1f;
                        }
                        else
                        {
                            rockToGrab.Center = Position;
                            rockToGrab.ai[0] = 10;
                            if (stateVar>30)
                            {
                                SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_JavelinThrowersAttack, (int)Position.X, (int)Position.Y);
                                if (sound != null)
                                {
                                    sound.Pitch = 0.5f;
                                }

                                rockToGrab.ai[0] = 60;
                                rockToGrab.damage = owner.projectile.damage;
                                rockToGrab.velocity = (Vector2.Normalize(velocity)*8f)+ player.velocity/4f;
                                rockToGrab.netUpdate = true;
                                ChangeState(10);
                                //armLerpGoal = 1f;
                            }
                        }
                        break;

                    //Retracting
                    case 10:
                        if (stateVar > 10)
                        {
                            armLerpGoal = 1f;
                            velocity += (followPosition - position) * (0.025f+ (stateVar/6000f));
                        }

                        chargeIn = MathHelper.Clamp(chargeIn + (-0.25f - chargeIn) / 8f, 0f, 1f);
                        chargeInDist *= 0.98f;

                        velocity *= 0.85f;

                        spinFriction += (0f - spinFriction) / 60f;

                        if (stateVar >= 30)
                        {
                            ChangeState(0);
                        }
                        break;

                    //Idle
                    case 0:
                        position += (followPosition - position) * 0.025f;
                        direction = (player.MountedCenter + owner.eyeCurrentLook)-Position;
                        spinFriction += (1f - spinFriction) / 30f;
                        armLerpGoal = 1f;
                        cooldown = 0;
                        chargeIn = MathHelper.Clamp(chargeIn + (-0.20f - chargeIn) / 5f, 0f, 1f);
                        chargeInDist = 0;
                        rockToGrab = null;

                        if (owner.throwing > 0)
                        {
                            int offset = (int)(player.SGAPly().timer + ((owner.PunchRate) * Percent));
                            if (offset % (owner.PunchRate) == 0)
                            {
                                Projectile[] rocks = owner.GrabbableAsteriods.Where(testby => (testby.Center-player.Center).LengthSquared()< 409600).OrderBy(testby => (testby.Center - Position).LengthSquared()).ToArray();

                                if (rocks.Length > 0)
                                {
                                    rockToGrab = rocks[0];
                                    ChangeState(2);
                                }
                            }
                        }

                        if (owner.punching > 0)
                        {
                            int offset = (int)(player.SGAPly().timer + (owner.PunchRate * Percent));
                            if (offset % owner.PunchRate == 0)
                            {
                                SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_JavelinThrowersAttack, (int)Position.X, (int)Position.Y);
                                if (sound != null)
                                {
                                    sound.Pitch = 0.5f;
                                }
                                ChangeState(1);
                            }
                        }

                        break;

                }

                despawnTimer = 10;
                armLerp = MathHelper.Clamp(armLerp+(Math.Sign(armLerpGoal - armLerp) * 0.025f),0f,1f);
                angle += angleSpin* spinFriction;
                position += velocity;
                velocity *= friction;
            }

            public void Draw(SpriteBatch spriteBatch, MandalaSummonProj owner,float alpha,Vector2 pos = default)
            {
                Vector2 asteriodSize = new Vector2(asteriod.Width, asteriod.Height / 2);
                spriteBatch.Draw(asteriod, (pos == default ? Position : pos) - Main.screenPosition, new Rectangle(0, 0, (int)asteriodSize.X, (int)asteriodSize.Y), Color.White*alpha, angle, asteriodSize / 2f, scale*owner.projectile.scale, SpriteEffects.None, 0);
            }

        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Overseer's Tutelage");
        }
        public override string Texture
        {
            get { return "SGAmod/Doom_Harbinger_Resprite_pupil"; }
        }

        public override void SetDefaults()
        {
            //projectile.CloneDefaults(ProjectileID.ImpFireball);
            projectile.width = 16;
            projectile.height = 16;
            projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.minion = true;
            projectile.minionSlots = 0f;
            projectile.netImportant = true;
            projectile.penetrate = 1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 1;
            aiType = 0;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(eyeLookAt);
            writer.Write(punching);
            writer.Write(throwing);

        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            eyeLookAt = reader.ReadVector2();
            punching = reader.ReadInt32();
            throwing = reader.ReadInt32();
        }

        Player Owner => Main.player[projectile.owner];
        int ArmsCount => Owner.maxMinions;
        public int PunchRate => 80;

        public override void AI()
        {
            float friction = 0.90f;
            projectile.localAI[0] += 1;
            punching -= 1;
            throwing -= 1;

            if (Owner.active && !Owner.dead && projectile.localAI[1] < 1 && Owner.SGAPly().mandalaSet.Item1)
            {
                projectile.timeLeft = 60;

                if (!Main.dedServ && Main.myPlayer == Owner.whoAmI)
                {
                    eyeLookAt = Main.MouseWorld - Owner.MountedCenter;
                    projectile.netUpdate = true;
                }
                eyeCurrentLook = Vector2.SmoothStep(eyeCurrentLook, eyeLookAt, 0.20f);


                Vector2 idealspot = Owner.MountedCenter + Vector2.Normalize(eyeCurrentLook) * (64f * (Owner.SGAPly().mandalaSet.Item2 % 2 == 0 ? 1f : -1f));
                Vector2 difference = idealspot - projectile.Center;

                float lerpSpeed = MathHelper.Clamp(projectile.localAI[0] / 160f, 0f, 1f);

                projectile.Center += (idealspot - projectile.Center) * 0.10f * lerpSpeed;
                projectile.rotation = ((float)Math.Sin(projectile.localAI[0] / 32f) * 10f);

                if (difference.Length() > 128f)
                {
                    projectile.velocity += (Vector2.Normalize(difference) * MathHelper.Clamp(difference.Length() - 128f, 0f, 1280f)) * 0.05f * lerpSpeed;
                };

                //Updates for when arms became "inactive" due to a lack of minion slots, (Update resets despawnTimer to 10, which isn't called here)
                foreach (MandalaArm arm in arms) { arm.despawnTimer--; }
                arms = arms.Where(testby => testby.despawnTimer > 0).ToList();

                for (int i = 0; i < ArmsCount; i += 1)
                {
                    float percent = (float)i / (float)ArmsCount;
                    float percentOne = 1f / (float)ArmsCount;

                    Vector2 offset = -Vector2.UnitX.RotatedBy(eyeCurrentLook.ToRotation() + (((percent + percentOne / 2f) - 0.50f) * (MathHelper.Pi * 0.75f)));

                    //Manage arms, else spawn new ones if we have room
                    if (i < arms.Count)
                    {
                        MandalaArm arm = arms[i];
                        arm.followPosition = Owner.MountedCenter + offset * 96f;
                        arm.index = i;
                        arm.maxIndex = ArmsCount;
                        arm.Update(this);
                    }
                    else
                    {
                        MandalaArm arm = new MandalaArm(Owner.MountedCenter + (offset * 640f) + Main.rand.NextVector2Circular(960f, 960f), Owner, i, ArmsCount);

                        arms.Add(arm);
                    }
                }

                //Spawns asteriods over time
                if (projectile.localAI[0] % 80 == 0 && Owner.ownedProjectileCounts[ModContent.ProjectileType<MandalaAsteriodProj>()] < 10 + Owner.maxTurrets * 2)
                {
                    float widrheight = Main.rand.NextFloat(240f, 640f);
                    Projectile.NewProjectileDirect(Owner.MountedCenter + Main.rand.NextVector2Circular(widrheight, widrheight), Main.rand.NextVector2Circular(2, 2), ModContent.ProjectileType<MandalaAsteriodProj>(), 0, 12, projectile.owner);
                }

            }
            else
            {
                //Up up and away!

                projectile.localAI[1] += 1;
                projectile.velocity -= Vector2.UnitY * (projectile.localAI[1] / 48f);

                foreach (MandalaArm arm in arms)
                {
                    float percent = (float)arm.index / (float)arms.Count;
                    arm.followPosition += ((DrawPosition + Vector2.UnitX.RotatedBy(percent * MathHelper.TwoPi) * 96f) - arm.followPosition) / 16f;
                    arm.Update(this);

                }

            }


            float alphalight = MathHelper.Clamp((projectile.localAI[0] - 20) / 40f, 0f, 1f) * MathHelper.Clamp(projectile.timeLeft / 60f, 0f, 1f);
            //Flashlight functionality

            for (float eyeeffect = 0; eyeeffect < 1f; eyeeffect += 0.01f)
            {
                float invert = 1f - eyeeffect;
                float lightit = 0.50f + eyeeffect / 2f;
                float lightit2 = MathHelper.Clamp(invert * 3f, 0f, 1f);
                Vector2 lighter = DrawPosition + Vector2.Normalize(eyeCurrentLook) * eyeeffect * 640f;

                (Point16, Point16) herethere = ((DrawPosition / 16).ToPoint16(), (lighter / 16).ToPoint16());

                if (Utils.PlotLine(herethere.Item1, herethere.Item2, (x, y) => Framing.GetTileSafely(x, y).collisionType != 1))
                {
                Lighting.AddLight(lighter + projectile.velocity, Color.White.ToVector3() * lightit * lightit2 * alphalight);
                }
                else
                {
                    break;
                }
            }


            // Lighting.AddLight(DrawPosition, Color.White.ToVector3());

            projectile.velocity *= friction;

        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            bool collide = false;

            foreach (MandalaArm arm in arms)
            {
                if (((arm.state == 1 && arm.stateVar>4) || (arm.state == 10 && arm.velocity.LengthSquared() > 100000)) && arm.cooldown<1)
                {
                    for (int i = 0; i < 120; i += 24)
                    {
                        Point pos = (arm.Position + Vector2.Normalize(arm.direction) * i).ToPoint();
                        Rectangle armhit = new Rectangle(pos.X - 8, pos.Y - 8, 16, 16);
                        if (armhit.Intersects(targetHitbox))
                        {
                            arm.cooldown = 20;
                            //arm.velocity = -arm.velocity/4f;
                            collide = true;
                            goto endhere;
                        }
                    }
                }
            }
        endhere:

            return collide;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.penetrate += 1;
            if (SGAmod.ScreenShake<16)
            SGAmod.AddScreenShake(12f, 2000, target.Center);
            SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_MonkStaffGroundImpact, (int)target.Center.X, (int)target.Center.Y);
            if (sound != null)
            {
                sound.Pitch = 0.85f;
            }
        }

        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            //base.DrawBehind(index, drawCacheProjsBehindNPCsAndTiles, drawCacheProjsBehindNPCs, drawCacheProjsBehindProjectiles, drawCacheProjsOverWiresUI);
        }

        //Beams and glowing are drawn under the player
        public override bool PreDrawExtras(SpriteBatch spriteBatch)
        {
            float alpha2 = MathHelper.Clamp((projectile.localAI[0] - 0) / 60f, 0f, 1f) * MathHelper.Clamp(projectile.timeLeft / 60f, 0f, 1f);


            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D glowOrb = ModContent.GetTexture("SGAmod/GlowOrb");
            Vector2 sizer = glowOrb.Size()/2f;

            foreach (Projectile asteriod in Asteriods)
            {
                float alphaglow = MathHelper.Clamp(asteriod.localAI[0] / 60f, 0f, Math.Min(asteriod.timeLeft / 60f, 1f))*alpha2;
                spriteBatch.Draw(glowOrb, asteriod.Center - Main.screenPosition, null, Color.Lerp(Color.White, Color.Aqua, 0.50f) * 0.40f* alphaglow, 0, sizer, projectile.scale*0.20f, SpriteEffects.None, 0);
            }

            foreach (MandalaArm arm in arms)
            {
                float alpha = MathHelper.Clamp((arm.time-60)/30f,0f,1f)* MathHelper.Clamp(projectile.timeLeft / 60f, 0f, 1f)* MathHelper.Clamp(arm.despawnTimer / 8f, 0f, 1f);

                List<Vector2> toThem = new List<Vector2>();

                toThem.Add(arm.Position);
                toThem.Add(DrawPosition);

                TrailHelper trail = new TrailHelper("FadedBasicEffectPass", Main.sunTexture);
                trail.projsize = Vector2.Zero;
                trail.coordOffset = new Vector2(0, 0f);
                trail.coordMultiplier = new Vector2(1f, 0.4f);
                trail.trailThickness = 16;
                trail.trailThicknessIncrease = -12;
                trail.doFade = false;
                trail.strength = 1f;
                trail.color = delegate (float percent)
                {
                    return Color.CornflowerBlue * (MathHelper.Clamp((alpha + percent) - 1f, 0f, 1f));
                };
                trail.DrawTrail(toThem, DrawPosition);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            return false;

        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D[] asteriods = new Texture2D[3] { ModContent.GetTexture("SGAmod/Dimensions/Space/GlowAsteriod"), ModContent.GetTexture("SGAmod/Dimensions/Space/GlowAsteriodalt"), ModContent.GetTexture("SGAmod/Dimensions/Space/GlowAsteriodalt2") };

            float alpha = MathHelper.Clamp((projectile.localAI[0] - 20) / 40f, 0f, 1f)*MathHelper.Clamp(projectile.timeLeft / 60f, 0f, 1f);

            Texture2D eyeTex = Main.projectileTexture[projectile.type];
            Vector2 eyeSize = new Vector2(eyeTex.Width, eyeTex.Height);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            Effect fadeIn = SGAmod.FadeInEffect;

            foreach (MandalaArm arm in arms)
            {
                if (arm.chargeIn <= 0.01f)
                    continue;

                fadeIn.Parameters["strength"].SetValue(1f);
                fadeIn.Parameters["fadeColor"].SetValue(Color.Aqua.ToVector3());
                fadeIn.Parameters["blendColor"].SetValue(Color.White.ToVector3());

                for (float f = arm.chargeInDist; f > 0; f -= 0.5f)
                {
                    float percent = (f / 10f);
                    float chargeScale = percent * 128f* arm.chargeIn;
                    fadeIn.Parameters["alpha"].SetValue((alpha * MathHelper.Clamp(arm.despawnTimer / 8f, 0f, 1f) * (1f-percent))*0.50f*(arm.chargeIn*0.75f));
                    fadeIn.CurrentTechnique.Passes["FadeIn"].Apply();
                    arm.Draw(spriteBatch, this, 1f,arm.Position-Vector2.Normalize(arm.direction)* chargeScale);
                }
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            foreach(MandalaArm arm in arms)
            {
                arm.Draw(spriteBatch,this, alpha * MathHelper.Clamp(arm.despawnTimer / 8f, 0f, 1f));
            }

            foreach (Projectile Asteriod in Asteriods)
            {
                Color asteriodColor = Lighting.GetColor(((int)Asteriod.Center.X) >> 4, ((int)Asteriod.Center.Y) >> 4, Color.White);
                (Asteriod.modProjectile as MandalaAsteriodProj).DoDraw(spriteBatch, asteriodColor * alpha);
            }

            Texture2D tex = asteriods[0];
            Vector2 size = new Vector2(tex.Width, tex.Height / 2);

            float lenToShow = MathHelper.Clamp((float)Math.Pow(eyeCurrentLook.Length(),0.36f)*1.5f,0,16f);
            float lenToShow2 = MathHelper.Clamp((float)Math.Pow(eyeCurrentLook.Length(), 0.36f) * 1.5f, 0, 8f);

            Vector2 lookpos = (Vector2.Normalize(eyeCurrentLook) * lenToShow);

            spriteBatch.Draw(tex, DrawPosition - Main.screenPosition, new Rectangle(0, 0, (int)size.X, (int)size.Y), Color.White * alpha, projectile.velocity.X / 12f, size / 2f, projectile.scale, SpriteEffects.None, 0);

            for (float f = 0; f < 1f; f += 1 / 10f)
            {
                float scale = ((MathHelper.Clamp((float)Math.Sin(((projectile.localAI[0]+(f*60f)) * MathHelper.TwoPi) / 60), 0f, 1f)))*((f/2f)+0.75f);
                spriteBatch.Draw(eyeTex, DrawPosition + lookpos - Main.screenPosition, null, Color.White * alpha*(f/2f)*0.50f, projectile.velocity.X/8f, eyeSize / 2f, (projectile.scale * 1f)+ scale*1f, SpriteEffects.None, 0);
            }

            spriteBatch.Draw(eyeTex, DrawPosition + lookpos - Main.screenPosition, null, Color.White* alpha, 0, eyeSize / 2f, projectile.scale*Main.essScale, SpriteEffects.None, 0);


            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


            return false;
        }

    }

    public class MandalaAsteriodProj : ModProjectile
    {
        float rotationSpeed = 0;
        float scale = 1f;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Overseer's Asteriod");
        }
        public override string Texture
        {
            get { return "SGAmod/Doom_Harbinger_Resprite_pupil"; }
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.ImpFireball);
            projectile.width = 24;
            projectile.height = 24;
            projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.aiStyle = -1;
            //projectile.minion = true;
            //projectile.minionSlots = 0f;
            projectile.netImportant = true;
            projectile.penetrate = 2;
            projectile.extraUpdates = 2;
            projectile.timeLeft = 300;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            aiType = 0;
        }

        public override bool CanDamage()
        {
            return projectile.damage > 0;
        }

        Player Owner => Main.player[projectile.owner];

        Vector2 DrawPosition => projectile.Center;

        public override void AI()
        {
            if (Owner.ownedProjectileCounts[ModContent.ProjectileType<MandalaSummonProj>()] < 1)
            {
                projectile.timeLeft = 1;
                return;
            }

            projectile.localAI[0] += 1;
            if (projectile.localAI[0] == 1)
            {
                projectile.localAI[1] = Main.rand.Next(3);
                rotationSpeed = (Main.rand.NextFloat(1f, 3f) * (Main.rand.NextBool() ? 1f : -1f)) * 0.01f;
                scale = Main.rand.NextFloat(0.6f, 0.8f);
                projectile.netUpdate = true;
            }
            if (projectile.ai[0] > 1)
            {
                if (projectile.ai[0]>12)
                projectile.ai[1] = 1;

                projectile.ai[0] -= 1;
                projectile.timeLeft += 1;
            }
            else
            {
                if (projectile.velocity.Length() > 2f)
                {
                    projectile.velocity *= 0.996f;
                }
                else
                {
                    projectile.damage = 0;
                }

            }

            if (projectile.ai[1] < 1)
            {
                Vector2 tohim = Owner.Center - projectile.Center;
                Vector2 there = Owner.Center + (new Vector2(256f, 0).RotatedBy(tohim.ToRotation() + MathHelper.Pi + 0.05f)) - projectile.Center;

                projectile.Center += there / 32f;
                projectile.timeLeft += 1;
            }

            projectile.rotation += rotationSpeed;
        }

        public override bool PreKill(int timeLeft)
        {
            /*SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_MonkStaffGroundImpact, (int)projectile.Center.X, (int)projectile.Center.Y);
            if (sound != null)
            {
                sound.Pitch = -0.85f;
            }*/

            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.penetrate += 1;
            if (SGAmod.ScreenShake < 12)
                SGAmod.AddScreenShake(12f, 1200, target.Center);
            SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_MonkStaffGroundImpact, (int)target.Center.X, (int)target.Center.Y);
            if (sound != null)
            {
                sound.Pitch = 0.85f;
            }
        }

        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            //base.DrawBehind(index, drawCacheProjsBehindNPCsAndTiles, drawCacheProjsBehindNPCs, drawCacheProjsBehindProjectiles, drawCacheProjsOverWiresUI);
        }

        public override bool PreDrawExtras(SpriteBatch spriteBatch)
        {

            return false;

        }

        public void DoDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D[] asteriods = new Texture2D[3] {ModContent.GetTexture("SGAmod/Dimensions/Space/MeteorLarge3"), ModContent.GetTexture("SGAmod/Dimensions/Space/MeteorLarge4"), ModContent.GetTexture("SGAmod/Dimensions/Space/MeteorLarge5") };

            float alpha = MathHelper.Clamp((projectile.localAI[0] - 20) / 40f, 0f, 1f) * MathHelper.Clamp(projectile.timeLeft / 60f, 0f, 1f);

            Texture2D tex = asteriods[(int)projectile.localAI[1]];

            Vector2 size = new Vector2(tex.Width, tex.Height / 2f);

            spriteBatch.Draw(tex, DrawPosition - Main.screenPosition, new Rectangle(0, 0, (int)size.X, (int)size.Y), lightColor * alpha, projectile.rotation, size / 2f, projectile.scale*scale, SpriteEffects.None, 0);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //DoDraw(spriteBatch, lightColor);


            return false;
        }

    }


}