using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class MossYoyo : MangroveBow,IDankSlowText, IMangroveSet
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quagmire");
			Tooltip.SetDefault("Hits apply Dank Slow against your foes");
		}

		public override void SetDefaults()
		{
			Item refItem = new Item();
			refItem.SetDefaults(ItemID.Amarok);
			item.damage = 25;
			item.useTime = 24;
			item.useAnimation = 22;
			item.useStyle = 5;
			item.channel = true;
			item.noMelee = true;
			item.melee = true;
			item.crit = 4;
			item.knockBack = 4.5f;
			item.value = 47000 * 5;
			item.rare = 3;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("MossYoyoProj");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.WoodYoyo);
			recipe.AddIngredient(null, "BiomassBar", 8);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, (int)((double)damage), knockBack, player.whoAmI, 0.0f, 0.0f);
			return false;
		}
	}
    public class Kelvin : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kelvin");
            Tooltip.SetDefault("'Flaming!'\nLava Burns enemies for 2 seconds on hit\nDoes 25% increased damage against lava-immune enemies");
        }

        public override void SetDefaults()
        {
            Item refItem = new Item();
            refItem.SetDefaults(ItemID.TheEyeOfCthulhu);
            item.damage = 40;
            item.useTime = 22;
            item.useAnimation = 22;
            item.useStyle = 5;
            item.channel = true;
            item.melee = true;
            item.knockBack = 2.5f;
            item.value = 10000;
            item.noMelee = true;
            item.rare = 6;
            item.noUseGraphic = true;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<Projectiles.KelvinProj>();
            item.UseSound = SoundID.Item19;
        }

        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, (int)((double)damage), knockBack, player.whoAmI, 0.0f, 0.0f);
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "FieryShard", 10);
            recipe.AddIngredient(mod.ItemType("UnmanedBar"), 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class Jaws : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jaws");
			Tooltip.SetDefault("Throws a yoyo with spinning teeth that halve enemy defense\nThe teeth break on tiles or after they hit enemies enough times");
		}

		public override void SetDefaults()
		{
			Item refItem = new Item();
			refItem.SetDefaults(ItemID.Amarok);
			item.damage = 45;
			item.useTime = 24;
			item.useAnimation = 22;
			item.useStyle = 5;
			item.channel = true;
			item.melee = true;
			item.noMelee = true;
			item.crit = 4;
			item.knockBack = 2.2f;
			item.value = 100000;
			item.rare = 5;
			item.noUseGraphic = true;
			item.autoReuse = true;
			item.UseSound = SoundID.Item19;
			item.shoot = mod.ProjectileType("JawsProj");
		}

		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, (int)((double)damage), knockBack, player.whoAmI, 0.0f, 0.0f);
			return false;
		}
	}

	public class Upheaval : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Upheaval");
			Tooltip.SetDefault("Unleashes flaming boulders as it's held out");
		}

		public override void SetDefaults()
		{
			Item refItem = new Item();
			refItem.SetDefaults(ItemID.TheEyeOfCthulhu);
			item.damage = 105;
			item.useTime = 22;
			item.useAnimation = 22;
			item.noMelee = true;
			item.useStyle = 5;
			item.channel = true;
			item.melee = true;
			item.knockBack = 4f;
			item.value = 500000;
			item.rare = 9;
			item.noUseGraphic = true;
			item.autoReuse = true;
			item.UseSound = SoundID.Item19;
			item.shoot = mod.ProjectileType("UpheavalProj");
		}

		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, (int)((double)damage), knockBack, player.whoAmI, 0.0f, 0.0f);
			return false;
		}
	}

	public class Tornado : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tornado");
			Tooltip.SetDefault("Control a living tornado that fires homing tornadoes at enemies as it's held out");
		}

		public override void SetDefaults()
		{
			Item refItem = new Item();
			refItem.SetDefaults(ItemID.TheEyeOfCthulhu);
			item.damage = 125;
			item.useTime = 22;
			item.useAnimation = 22;
			item.useStyle = 5;
			item.channel = true;
			item.melee = true;
			item.noMelee = true;
			item.knockBack = 2.5f;
			item.value = 1000000;
			item.rare = 9;
			item.noUseGraphic = true;
			item.autoReuse = true;
			item.UseSound = SoundID.Item19;
			item.shoot = mod.ProjectileType("TornadoProj");
		}

		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, (int)((double)damage), knockBack, player.whoAmI, 0.0f, 0.0f);
			return false;
		}
	}


}

namespace SGAmod.Items.Weapons
{
    public class CreepersThrow : ModItem, IDevItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mister Creeper's Explosive Throw");
            Tooltip.SetDefault("Controls a yoyo shaped creeper that lights a fuse when near enemies and explodes violently shortly after\nHowever, watch out as you can hurt yourself from the creeper's explosion");
        }

        public (string, string) DevName()
        {
            return ("Mister Creeper", "other (legacy)");
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (Main.LocalPlayer.GetModPlayer<SGAPlayer>().devempowerment[1] > 0)
            {
                tooltips.Add(new TooltipLine(mod, "DevEmpowerment", "--- Enpowerment bonus ---"));
                tooltips.Add(new TooltipLine(mod, "DevEmpowerment", "40% increased damage"));
                tooltips.Add(new TooltipLine(mod, "DevEmpowerment", "Creates smaller explosions leading up to the larger one"));
            }
        }

        public override void SetDefaults()
        {
            Item refItem = new Item();
            refItem.SetDefaults(ItemID.TheEyeOfCthulhu);
            item.damage = 250;
            item.useTime = 16;
            item.useAnimation = 16;
            item.useStyle = 5;
            item.crit = 10;
            item.channel = true;
            item.melee = true;
            item.noMelee = true;
            item.knockBack = 2.5f;
            item.value = Item.sellPrice(0, 75, 0, 0);
            item.rare = 11;
            item.expert = true;
            item.noUseGraphic = true;
            item.autoReuse = true;
            item.UseSound = SoundID.Item19;
            item.shoot = mod.ProjectileType("CreepersThrowProj");
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LunarBar, 10);
            recipe.AddIngredient(ItemID.FragmentSolar, 10);
            recipe.AddIngredient(ItemID.ExplosivePowder, 100);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 10);
            recipe.AddIngredient(mod.ItemType("CosmicFragment"), 1);
            recipe.AddIngredient(mod.ItemType("MoneySign"), 10);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            if (player.GetModPlayer<SGAPlayer>().devempowerment[1] > 0)
                add += 0.40f;
        }
        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, (int)((double)damage), knockBack, player.whoAmI, 0.0f, 0.0f);
            return false;
        }
    }

    public class CreepersThrowProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Creeper's Throw");
            ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = 999f;
            ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 500f;
            ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 20f;
        }

        public override string Texture
        {
            get { return ("SGAmod/Projectiles/CreepersThrow"); }
        }

        public override void SetDefaults()
        {
            Projectile refProjectile = new Projectile();
            refProjectile.SetDefaults(ProjectileID.TheEyeOfCthulhu);
            projectile.extraUpdates = 0;
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = 99;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.melee = true;
            projectile.scale = 1f;
            projectile.usesIDStaticNPCImmunity = true;
            projectile.idStaticNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Player owner = Main.player[projectile.owner];
            if (owner != null && !owner.dead)
            {
                if (projectile.localAI[1] < 0)
                    projectile.localAI[1] += 1;


                if (projectile.localAI[1] < 1)
                {
                    NPC target = Main.npc[Idglib.FindClosestTarget(0, projectile.Center, new Vector2(0f, 0f), true, true, true, projectile)];
                    if (target != null && Vector2.Distance(target.Center, projectile.Center) < 72)
                    {
                        projectile.localAI[1] = 1;


                    }
                }
                else
                {
                    projectile.localAI[1] += 1;

                    int dustIndexsmoke = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 1f);
                    Main.dust[dustIndexsmoke].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndexsmoke].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndexsmoke].noGravity = true;
                    Main.dust[dustIndexsmoke].position = projectile.Center + new Vector2(0f, (float)(-(float)projectile.height / 2)).RotatedBy((double)projectile.rotation, default(Vector2)) * 1.1f;
                    dustIndexsmoke = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 1f);
                    Main.dust[dustIndexsmoke].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndexsmoke].noGravity = true;
                    Main.dust[dustIndexsmoke].position = projectile.Center + new Vector2(0f, (float)(-(float)projectile.height / 2 - 6)).RotatedBy((double)projectile.rotation, default(Vector2)) * 1.1f;

                    if (projectile.localAI[1] > 40 && projectile.localAI[1] < 120 && projectile.localAI[1] % 25 == 0 && owner.GetModPlayer<SGAPlayer>().devempowerment[1] > 0)
                    {

                        int thisone = Projectile.NewProjectile(projectile.Center.X - 100, projectile.Center.Y - 100, Vector2.Zero.X, Vector2.Zero.Y, ModContent.ProjectileType<CreepersThrowBoom>(), projectile.damage * 2, projectile.knockBack, Main.player[projectile.owner].whoAmI, 0.0f, 0f);
                        Main.projectile[thisone].timeLeft = 2;
                        Main.projectile[thisone].width = 200;
                        Main.projectile[thisone].penetrate = 1;
                        Main.projectile[thisone].height = 200;
                        Main.projectile[thisone].scale = 0.001f;
                        Main.projectile[thisone].netUpdate = true;

                    }

                    if (projectile.localAI[1] == 121)
                    {
                        projectile.localAI[1] = -60;
                        for (int i = 0; i < 359; i += 36)
                        {
                            double angles = MathHelper.ToRadians(i);
                            float randomx = 64f;//Main.rand.NextFloat(54f, 96f);
                            Vector2 here = new Vector2((float)Math.Cos(angles), (float)Math.Sin(angles));

                            int thisone = Projectile.NewProjectile(projectile.Center.X + (here.X * randomx) - 100, projectile.Center.Y + (here.Y * randomx) - 100, here.X, here.Y, ModContent.ProjectileType<CreepersThrowBoom>(), projectile.damage * 1, projectile.knockBack, Main.player[projectile.owner].whoAmI, 0.0f, 0f);
                            Main.projectile[thisone].timeLeft = 2;
                            Main.projectile[thisone].width = 200;
                            Main.projectile[thisone].height = 200;
                            Main.projectile[thisone].scale = 0.001f;
                            Main.projectile[thisone].netUpdate = true;

                        }
                    }
                    if (projectile.localAI[1] == 120)
                    {

                        for (int i = 0; i < 359; i += 72)
                        {
                            double angles = MathHelper.ToRadians(i);
                            float randomx = 48f;//Main.rand.NextFloat(54f, 96f);
                            Vector2 here = new Vector2((float)Math.Cos(angles), (float)Math.Sin(angles));

                            int thisone = Projectile.NewProjectile(projectile.Center.X + (here.X * randomx) - 100, projectile.Center.Y + (here.Y * randomx) - 100, here.X, here.Y, ModContent.ProjectileType<CreepersThrowBoom>(), projectile.damage * 1, projectile.knockBack, Main.player[projectile.owner].whoAmI, 0.0f, 0f);
                            Main.projectile[thisone].timeLeft = 2;
                            Main.projectile[thisone].width = 200;
                            Main.projectile[thisone].penetrate = 1;
                            Main.projectile[thisone].height = 200;
                            Main.projectile[thisone].scale = 0.001f;
                            Main.projectile[thisone].netUpdate = true;

                        }


                    }

                }

            }
        }

    }

    public class CreepersThrowBoom : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Creeper's KaBoom");
        }

        public override string Texture
        {
            get { return ("SGAmod/Projectiles/CreepersThrow"); }
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (!target.friendly && !target.dontTakeDamage && target.immune[Main.player[projectile.owner].whoAmI] > 0)
                return true;
            return base.CanHitNPC(target);
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.GrenadeIII);
            projectile.scale = 0.001f;
            projectile.melee = true;
            //projectile.penetrate = 1;
            aiType = ProjectileID.GrenadeIII;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            target.immune[projectile.owner] = 1;
        }

        public override bool PreKill(int timeLeft)
        {
            projectile.type = ProjectileID.GrenadeIII;
            return true;
        }
    }

    public class CreepersThrowBoom2 : CreepersThrowBoom
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Creeper's KaBoom");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            projectile.timeLeft = 3;
            projectile.width = 200;
            projectile.height = 200;
            projectile.scale = 0.001f;
            projectile.timeLeft = 2;
            projectile.penetrate = 1;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.life - damage < 0)
            {
                Main.player[projectile.owner].netLife = true;
                Main.player[projectile.owner].statLife += 50;
                Main.player[projectile.owner].GetModPlayer<SGAPlayer>().creeperexplosion = 0;
            }
        }


    }

    public class ThievesThrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Thieves' Throw");
            Tooltip.SetDefault("Steals items off the ground and teleports them to the player\n'Yes, it also steals Hearts, gotta put that joke in there'");
        }

        public override void SetDefaults()
        {
            Item refItem = new Item();
            refItem.SetDefaults(ItemID.TheEyeOfCthulhu);
            item.damage = 0;
            item.useTime = 22;
            item.useAnimation = 22;
            item.useStyle = 5;
            item.channel = true;
            item.melee = true;
            item.knockBack = 2.5f;
            item.value = 50000;
            item.noMelee = true;
            item.rare = ItemRarityID.Green;
            item.noUseGraphic = true;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("ThievesThrowProj");
            item.UseSound = SoundID.Item19;
        }

        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, 0, knockBack, player.whoAmI, 0.0f, 0.0f);
            return false;
        }
    }

    public class ThievesThrowProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Thieves Throw");
            ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = 300.0f;
            ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 1500f;
            ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 25f;
        }

        public override string Texture => "SGAmod/Projectiles/ThievesThrowProj";

        public override void SetDefaults()
        {
            Projectile refProjectile = new Projectile();
            refProjectile.SetDefaults(ProjectileID.TheEyeOfCthulhu);
            projectile.extraUpdates = 0;
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = 99;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.melee = true;
            projectile.scale = 1f;
        }

        public override bool CanDamage()
        {
            return false;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            int dist = GetType() == typeof(RiftThrowProj) ? 50000 : 20000;

            foreach (Item item in Main.item.Where(testby => testby.active && (projectile.Center - testby.Center).LengthSquared() < dist))
            {
                item.velocity += Collision.TileCollision(item.position, Vector2.Normalize(projectile.Center - item.Center) * 0.40f, item.width, item.height);

                if ((item.Center - projectile.Center).LengthSquared() < 600)
                {
                    item.Center = player.Center;
                }
            }
        }
    }

    public class RiftYoyo : ThievesThrow
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Desolate Rift");
            Tooltip.SetDefault("Sucks in enemies and items, deals Damage over Time");
        }

        public override void SetDefaults()
        {
            Item refItem = new Item();
            refItem.SetDefaults(ItemID.TheEyeOfCthulhu);
            item.damage = 250;
            item.useTime = 22;
            item.useAnimation = 22;
            item.useStyle = 5;
            item.channel = true;
            item.melee = true;
            item.value = 50000;
            item.noMelee = true;
            item.rare = ItemRarityID.Yellow;
            item.noUseGraphic = true;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<RiftThrowProj>();
            item.UseSound = SoundID.Item19;
        }

        public override string Texture => "SGAmod/Items/Weapons/RiftYoyo";

        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0.0f, 0.0f);
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<ThievesThrow>(), 1);
            recipe.AddIngredient(ModContent.ItemType<StygianCore>(), 1);
            recipe.AddIngredient(ModContent.ItemType<OverseenCrystal>(), 20);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class RiftThrowProj : ThievesThrowProj
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Desolate Rift");
            ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = 20.0f;
            ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 500f;
            ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 15f;
        }

        public override void SetDefaults()
        {
            Projectile refProjectile = new Projectile();
            refProjectile.SetDefaults(ProjectileID.TheEyeOfCthulhu);
            projectile.extraUpdates = 0;
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = 99;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.scale = 1f;
        }

        public override string Texture => "SGAmod/Projectiles/RiftYoyoProj";

        public override bool CanDamage()
        {
            return false;
        }

        public override void AI()
        {
            base.AI();
            Player player = Main.player[projectile.owner];
            foreach (NPC npc in Main.npc.Where(testby => testby.active && !testby.friendly && !testby.dontTakeDamage && testby.chaseable && (projectile.Center - testby.Center).LengthSquared() < 60000))
            {
                if (!npc.IsDummy() && (npc.Center - projectile.Center).LengthSquared() < 50000)
                {
                    npc.Center += Collision.TileCollision(npc.position, Vector2.Normalize(projectile.Center - npc.Center) * 4f*(npc.knockBackResist+0.25f), npc.width, npc.height);

                    npc.SGANPCs().nonStackingImpaled = Math.Max(npc.SGANPCs().nonStackingImpaled, projectile.damage);
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Dimensions.NPCs.SpaceBoss.DarknessNebulaEffect(mod.GetTexture("GlowOrb"), 0f, projectile.Center, 0.25f, projectile.whoAmI, 10, -5f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            return base.PreDraw(spriteBatch, lightColor);
        }


    }

    public class LeechYoyo : ThievesThrow
    {
        //Extra 23-24, 
        //ProjectileID.MoonLeech

        //Chain 12-Leech
        //NPCID.LeechHead

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Tongue");
            Tooltip.SetDefault("Leeches nearby enemies of their life\nYou are healed when the yoyo returns to you, based off life leeched");
        }

        public override void SetDefaults()
        {
            Item refItem = new Item();
            refItem.SetDefaults(ItemID.TheEyeOfCthulhu);
            item.damage = 50;
            item.useTime = 64;
            item.useAnimation = 64;
            item.useStyle = 5;
            item.channel = true;
            item.melee = true;
            item.value = 10000;
            item.noMelee = true;
            item.rare = ItemRarityID.Yellow;
            item.noUseGraphic = true;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<LeechYoyoProj>();
            item.UseSound = SoundID.Item19;
        }

        public override string Texture => "SGAmod/Items/Weapons/RiftYoyo";

        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0.0f, 0.0f);
            return false;
        }
    }

    public class LeechYoyoProj : ThievesThrowProj
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Leech Yoyo");
            ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = 5.0f;
            ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 320f;
            ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 8f;
        }

        public override void SetDefaults()
        {
            Projectile refProjectile = new Projectile();
            refProjectile.SetDefaults(ProjectileID.TheEyeOfCthulhu);
            projectile.extraUpdates = 0;
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = 99;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.melee = true;
            projectile.scale = 1f;
        }

        public override string Texture => "SGAmod/Projectiles/RiftYoyoProj";

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage /= 3;
        }

        public override void AI()
        {
            base.AI();
            Player player = Main.player[projectile.owner];
            foreach (NPC npc in Main.npc.Where(testby => testby.active && !testby.friendly && !testby.dontTakeDamage && testby.chaseable && (projectile.Center - testby.Center).LengthSquared() < 60000))
            {
                if (!npc.IsDummy() && (npc.Center - projectile.Center).LengthSquared() < 50000)
                {
                    npc.Center += Collision.TileCollision(npc.position, Vector2.Normalize(projectile.Center - npc.Center) * 4f * (npc.knockBackResist + 0.25f), npc.width, npc.height);

                    npc.SGANPCs().nonStackingImpaled = Math.Max(npc.SGANPCs().nonStackingImpaled, projectile.damage);
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Dimensions.NPCs.SpaceBoss.DarknessNebulaEffect(mod.GetTexture("GlowOrb"), 0f, projectile.Center, 0.25f, projectile.whoAmI, 10, -5f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            return base.PreDraw(spriteBatch, lightColor);
        }


    }


}
