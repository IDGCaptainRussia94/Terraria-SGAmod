using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using AAAAUThrowing;
using Terraria.Utilities;
using SGAmod.Tiles;

namespace SGAmod.Items.Weapons.Auras
{

	public class AuraBorealisStaff : AuraStaffBase, IAuroraItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aura Borealis Staff");
			Tooltip.SetDefault("Summons a Hallowed Celestial Aura around the player");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}

        public override string Texture => "SGAmod/Items/Weapons/Aurora/AuraBorealisStaff";

        public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);
			int thetarget = -1;
			if (Main.LocalPlayer.ownedProjectileCounts[item.shoot] > 0)
			{
				for (int i = 0; i < Main.maxProjectiles; i += 1)
				{
					if (Main.projectile[i].active && Main.projectile[i].type == item.shoot && Main.projectile[i].owner == Main.LocalPlayer.whoAmI)
					{
						thetarget = i;
						break;
					}
				}
			}


			if (thetarget > -1 && Main.projectile[thetarget].active && Main.projectile[thetarget].type==item.shoot)
			{
				AuraMinionBorealis shoot = Main.projectile[thetarget].modProjectile as AuraMinionBorealis;
				tooltips.Add(new TooltipLine(mod, "Bonuses", "Power Level: "+ shoot.thepower));
				tooltips.Add(new TooltipLine(mod, "Bonuses", "Passive: Grants life Regen per Power Level"));

				if (shoot.thepower >= 1.0)
					tooltips.Add(new TooltipLine(mod, "Bonuses", "Lv1: Applies Betsy's Curse to enemies"));
				if (shoot.thepower >= 2.0)
					tooltips.Add(new TooltipLine(mod, "Bonuses", "Lv2: Applies Daybroken to enemies"));
				if (shoot.thepower >= 3.0)
					tooltips.Add(new TooltipLine(mod, "Bonuses", "Lv3: Applies Moonlight Curse to enemies (replaces Daybroken)"));

			}
		}

		public override void SetDefaults()
		{
			item.damage = 0;
			item.knockBack = 3f;
			item.mana = 10;
			item.width = 32;
			item.height = 32;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = 1;
			item.value = Item.buyPrice(0, 0, 50, 0);
			item.rare = 1;
			item.UseSound = SoundID.Item44;

			// These below are needed for a minion weapon
			item.noMelee = true;
			item.summon = true;
			item.buffType = ModContent.BuffType<AuraBorealisBuff>();
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			item.shoot = ModContent.ProjectileType<AuraMinionBorealis>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			/*recipe.AddIngredient(ModContent.ItemType<StoneBarrierStaff>(), 1);
			recipe.AddIngredient(ModContent.ItemType<AuroraTearAwoken>(), 1);
			recipe.AddIngredient(ItemID.CrystalShard, 10);
			recipe.AddTile(ModContent.TileType<LuminousAlter>());
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);*/
			recipe.AddIngredient(ModContent.ItemType<AuroraTearAwoken>(), 1);
			recipe.AddIngredient(ModContent.ItemType<IlluminantEssence>(), 10);
			recipe.AddIngredient(ItemID.CrystalShard, 10);
			recipe.AddIngredient(ItemID.LunarBar, 12);
			recipe.AddTile(ModContent.TileType<LuminousAlter>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class AuraMinionBorealis : AuraMinion
	{

		protected override int BuffType => ModContent.BuffType<AuraBorealisBuff>();

        protected override float AuraSize => 120f;

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Borealis");
			Main.projFrames[projectile.type] = 1;

			// These below are needed for a minion
			// Denotes that this projectile is a pet or minion
			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			ProjectileID.Sets.Homing[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.tileCollide = false;
			projectile.friendly = false;
			projectile.minion = true;
			projectile.minionSlots = 1f;
			projectile.penetrate = -1;
			projectile.timeLeft = 60;
		}

		public override void AuraAI(Player player)
		{
			Lighting.AddLight(projectile.Center, Color.Pink.ToVector3() * 0.78f);
		}

		public override void InsideAura<T>(T type, Player player)
		{
			if (type is NPC)
			{
				NPC himas = (type as NPC);
				if (!(himas.townNPC || himas.friendly))
				{
					himas.AddBuff(BuffID.BetsysCurse, 3);
					if (thepower >= 2)
					{
						himas.AddBuff(thepower >= 3 ? ModContent.BuffType<Buffs.MoonLightCurse>() : BuffID.Daybreak, 3);
					}
				}
			}
			if (type is Player alliedplayer && alliedplayer.team == player.team)
			{
				alliedplayer.lifeRegen += (int)(thepower*2f);
			}

		}

		public override void AuraEffects(Player player, int type)
		{


			if (type == 1)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			}

			UnifiedRandom rando = new UnifiedRandom(projectile.whoAmI);

			Texture2D mainTex = mod.GetTexture("Extra_57b");
			Vector2 halfsize = mainTex.Size() / 2f;

			for (float i = 0; i < 360; i += 360f / projectile.minionSlots)
			{
				float angle = MathHelper.ToRadians(i + projectile.localAI[0] * 2f);
				Vector2 loc2 = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
				Vector2 loc = loc2 * 32f;
				//loc -= Vector2.UnitY * 54f * player.gravDir;

				if (type == 0)
				{
					/*float velmul = Main.rand.NextFloat(0.75f, 1f);
					Vector2 vels = loc2.RotatedBy(-90) * 4f * velmul;

					int dustIndex = Dust.NewDust(projectile.Center + loc - new Vector2(6, 6), 12, 12, DustID.AncientLight, 0, 0, 150, default(Color), 0.75f);
					Main.dust[dustIndex].velocity = vels + player.velocity;
					Main.dust[dustIndex].noGravity = true;
					Main.dust[dustIndex].color = Color.Lime;*/
				}

				if (type == 1)
				{
					float colorhue = rando.NextFloat();
					Main.spriteBatch.Draw(mainTex, (projectile.Center + loc) - Main.screenPosition,null, Main.hslToRgb(colorhue, 0.75f,0.75f), angle*2f + MathHelper.ToRadians(90), halfsize, projectile.scale, SpriteEffects.None, 0f);
					Main.spriteBatch.Draw(mainTex, (projectile.Center + loc) - Main.screenPosition, null, Main.hslToRgb(colorhue, 0.5f, 0.75f), -angle*2f - MathHelper.ToRadians(90), halfsize, projectile.scale*0.75f, SpriteEffects.None, 0f);
				}
			}

			if (type == 1)
			{
				Effect RadialEffect = SGAmod.RadialEffect;

				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

				RadialEffect.Parameters["overlayTexture"].SetValue(SGAmod.PearlIceBackground);//SGAmod.PearlIceBackground
				RadialEffect.Parameters["alpha"].SetValue(0.75f);
				RadialEffect.Parameters["texOffset"].SetValue(new Vector2(-Main.GlobalTime * 0.125f, Main.GlobalTime * 0.075f));
				RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(2f, 2f));
				RadialEffect.Parameters["ringScale"].SetValue(0.075f);
				RadialEffect.Parameters["ringOffset"].SetValue(0.50f);
				RadialEffect.Parameters["ringColor"].SetValue(Color.White.ToVector3());
				RadialEffect.Parameters["tunnel"].SetValue(false);


				RadialEffect.CurrentTechnique.Passes["Radial"].Apply();

				Main.spriteBatch.Draw(mainTex, projectile.Center - Main.screenPosition, null, Color.White, 0, halfsize, (new Vector2(thesize, thesize) /(halfsize*1.5f)) * MathHelper.Pi, default, 0);

				RadialEffect.Parameters["texOffset"].SetValue(new Vector2(-Main.GlobalTime * -0.125f, Main.GlobalTime * 0.075f));
				RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(2f, 2f));
				RadialEffect.Parameters["ringScale"].SetValue(0.05f);
				RadialEffect.CurrentTechnique.Passes["Radial"].Apply();

				Main.spriteBatch.Draw(mainTex, projectile.Center - Main.screenPosition, null, Color.LightGray, 0, halfsize, ((new Vector2(thesize, thesize)+new Vector2(8f,8f)) / (halfsize * 1.5f)) * MathHelper.Pi, default, 0);

				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			}

			/*if (type == 0)
			{
				for (float i = 0; i < 5f; i += 1f)
				{
					float angle = MathHelper.ToRadians(Main.rand.NextFloat(0, 360));
					Vector2 loc2 = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
					Vector2 loc = loc2 * thesize;

					Vector2 vels = loc2.RotatedBy(-90) * 0f;

					int dustIndex = Dust.NewDust(projectile.Center + loc, 0, 0, DustID.AncientLight, 0, 0, 150, default(Color), 0.65f);
					Main.dust[dustIndex].velocity = vels + player.velocity;
					Main.dust[dustIndex].noGravity = true;
					Main.dust[dustIndex].color = Color.Lime;
				}
			}*/

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			AuraEffects(Main.player[projectile.owner], 1);
			//Texture2D tex = ModContent.GetTexture("SGAmod/Items/Weapons/Auras/StoneGolem");
			//spriteBatch.Draw(tex, projectile.Center+new Vector2(0,-32+(float)Math.Sin(projectile.localAI[0]/30f)*4f)-Main.screenPosition, null, lightColor, 0, new Vector2(tex.Width, tex.Height)/2f, projectile.scale, Main.player[projectile.owner].direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return false;
		}

	}

	public class AuraBorealisBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Hallow Lights Aura");
			Description.SetDefault("An aura of Hallow Lights projects around you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/AuraBorealisBuff";
			return base.Autoload(ref name, ref texture);
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<AuraMinionBorealis>()] > 0)
			{
				player.buffTime[buffIndex] = 18000;
			}
			else
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}

}