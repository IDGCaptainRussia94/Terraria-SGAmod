
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SGAmod.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Armors.Magatsu
{

	[AutoloadEquip(EquipType.Head)]
	public class MagatsuHood : ModItem
	{
        public override bool Autoload(ref string name)
        {
			//if (GetType() == typeof(ValkyrieHelm))
				//SGAPlayer.PostUpdateEquipsEvent += SetBonus;

			return true;
        }
		protected string tooltip = "1% increased Apocalyptical Chance\n20% increased Apocalyptical Strength";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magatsu Hood");
			Tooltip.SetDefault(tooltip);
		}

		public static void ActivateDecoy(SGAPlayer sgaplayer)
        {
			if (sgaplayer.AddCooldownStack(60 * 60, 2))
            {
				bool decoyExists = Main.npc.Where(testby => testby.active && testby.type == ModContent.NPCType<MagatsuDecoy>() && testby.ai[3] == sgaplayer.player.whoAmI).Count() > 0;
				if (decoyExists)
					return;

				Vector2 spot = sgaplayer.player.Center;
				int npc2 = NPC.NewNPC((int)spot.X, (int)spot.Y, ModContent.NPCType<MagatsuDecoy>(), ai3: sgaplayer.player.whoAmI);
				Main.npc[npc2].life = sgaplayer.player.statLifeMax2*3;
				Main.npc[npc2].lifeMax = Main.npc[npc2].life;
				Main.npc[npc2].defense = sgaplayer.player.statDefense*2;
				Main.PlaySound(SoundID.Item, (int)spot.X, (int)spot.Y, 78, 1f, -0.8f);
				var snd = Main.PlaySound(SoundID.DD2_EtherianPortalOpen, (int)spot.X, (int)spot.Y);
				if (snd != null)
                {
					snd.Pitch = -0.80f;
                }
			}
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0,15,0,0);
			item.rare = ItemRarityID.Lime;
			item.defense = 8;
			item.lifeRegen = 0;
		}

		public static void SetBonus(SGAPlayer sgaplayer)
        {
			Player player = sgaplayer.player;

			sgaplayer.apocalypticalStrength += 0.40f;
			for (int i = 0; i < sgaplayer.apocalypticalChance.Length; i += 1)
				sgaplayer.apocalypticalChance[i] += 2.0;

			sgaplayer.magatsuSet = true;

			if (player.ownedProjectileCounts[ModContent.ProjectileType<Items.Armors.Magatsu.ArmorBoundDarkSectorEye>()] < (player.maxMinions - player.SGAPly().GetMinionSlots))
			{
				Projectile.NewProjectileDirect(player.Center, Vector2.Zero, ModContent.ProjectileType<Items.Armors.Magatsu.ArmorBoundDarkSectorEye>(), 0, 0, player.whoAmI);
			}

		}

		/*public static void SetBonus(SGAPlayer sgaplayer)
		{

		}*/
		public Color ArmorGlow(Player player, int index)
		{
			return Color.White * 0.50f;
		}

		public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod,typeof(SGAPlayer).Name) as SGAPlayer;

			sgaplayer.apocalypticalStrength += 0.20f;
			for (int i = 0; i < sgaplayer.apocalypticalChance.Length; i += 1)
				sgaplayer.apocalypticalChance[i] += 1.0;

		}
		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ)
			{
				sgaplayer.armorglowmasks[0] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
				sgaplayer.armorglowcolor[0] = ArmorGlow;
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<WovenEntrophite>(), 25);
			recipe.AddIngredient(ModContent.ItemType<StygianCore>(), 1);

			if (GetType() == typeof(MagatsuHood))
			recipe.AddIngredient(ModContent.ItemType<HopeHeart>(), 1);

			recipe.AddTile(TileID.Loom);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class MagatsuRobes : MagatsuHood
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magatsu Robes");
			Tooltip.SetDefault(tooltip);
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 20, 0, 0);
			item.rare = ItemRarityID.Lime;
			item.defense = 12;
			item.lifeRegen = 0;
		}

		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ)
			{
				sgaplayer.armorglowmasks[1] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
				sgaplayer.armorglowmasks[2] = "SGAmod/Items/GlowMasks/" + Name + "_GlowArms";
				sgaplayer.armorglowmasks[4] = "SGAmod/Items/GlowMasks/" + Name + "_GlowFemale";
				sgaplayer.armorglowcolor[1] = ArmorGlow;
				sgaplayer.armorglowcolor[2] = ArmorGlow;
				sgaplayer.armorglowcolor[4] = ArmorGlow;
			}
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class MagatsuPants : MagatsuHood
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magatsu Pants");
			Tooltip.SetDefault(tooltip);
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.rare = ItemRarityID.Lime;
			item.defense = 5;
			item.lifeRegen = 0;
		}
		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ)
			{
				sgaplayer.armorglowmasks[3] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
				sgaplayer.armorglowcolor[3] = ArmorGlow;
			}
		}
	}

	public class ExplosionDarkSectorEye : Dimensions.NPCs.SpookyDarkSectorEye, IPostEffectsDraw
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Voided Null Seeker Explosion");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.extraUpdates = 2;
			projectile.timeLeft = 60;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override void AI()
		{
			projectile.velocity /= 1.05f;

			projectile.localAI[0] += 1;

		}

		public override void PostEffectsDraw(SpriteBatch spriteBatch, float drawScale = 2f)
		{

			float alpha = Math.Min((projectile.timeLeft) / 30f,1f);

			if (alpha <= 0)
				return;

			Texture2D tex = ModContent.GetTexture("SGAmod/Dimensions/NPCs/NullWatcher");
			Rectangle rect = new Rectangle(0, (tex.Height / 7) * (2 + (int)(Math.Min(projectile.localAI[0] / 10f, 4))), tex.Width, tex.Height / 7);
			Rectangle recteye = new Rectangle(0, 0, tex.Width, tex.Height / 7);

			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 7) / 2f;

			float scale = (1f - (projectile.timeLeft / 60f)) * 5f;

			for (int k = 0; k < 1; k++)//projectile.oldPos.Length
			{
				Vector2 drawPos = (projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY)) / drawScale;
				float coloralpha = ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

				spriteBatch.Draw(tex, drawPos, rect, Color.GreenYellow * coloralpha * 0.75f * alpha, projectile.rotation, drawOrigin, projectile.scale * 1f * scale, SpriteEffects.None, 0f);
				spriteBatch.Draw(tex, drawPos + Vector2.Zero, recteye, Color.White * coloralpha * 0.75f * alpha, projectile.rotation, drawOrigin, projectile.scale * scale, SpriteEffects.None, 0f);
			}

		}
	}

		public class ArmorBoundDarkSectorEye : Dimensions.NPCs.SpookyDarkSectorEye, IPostEffectsDraw
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Voided Null Seeker");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
			projectile.netImportant = true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }

		public override void AI()
		{
			projectile.velocity /= 1.05f;

			projectile.localAI[0] += 1;
			P = Main.player[projectile.owner];

			bool remove = false;
			int index = 0;
			int countedindex = 0;
			
			foreach(Projectile projectile2 in Main.projectile.Where(testby => testby.active && testby.type == projectile.type))
            {
				countedindex += 1;
				if (projectile2 == projectile)
                {
					index = countedindex;
				}
			}
			//index -= 1;
			countedindex += 1;

			if (!P.active || P.dead || !P.SGAPly().magatsuSet || index > (P.maxMinions-P.SGAPly().GetMinionSlots))
			{
				projectile.Kill();
				return;
			}

			Vector2 halfcircle = new Vector2(0,-P.gravDir*48f).RotatedBy(MathHelper.PiOver2 - ((index / (float)countedindex) * MathHelper.Pi))*new Vector2(1.00f,2.0f);
			Vector2 PCenter = P.Center;

			if (SGAPlayer.centerOverrideTimerIsActive > 0)
			{
				if (P.SGAPly().centerOverrideTimer > 0)
					PCenter = P.SGAPly().centerOverridePosition;
			}

			Vector2 gohere = PCenter + new Vector2(P.direction*0f,P.gravDir*48f)+halfcircle;
			projectile.timeLeft = 3;

			if (projectile.ai[1] < 1000)
            {
				lookat = Main.MouseWorld;
				projectile.netUpdate = true;
			}

			projectile.ai[0] = -1;

			eyeDist = MathHelper.Clamp((projectile.ai[1] - 500) / 500f, 0f, 1f) * 4f;

			projectile.ai[1] = MathHelper.Clamp(projectile.ai[1] - 1f, 0f, 1250);
			int dists = 2400 * 2400;

			int inndx = 0;
			IEnumerable<NPC> targets = Main.npc.Where(testby => testby.active && !testby.dontTakeDamage && testby.chaseable && (testby.Center - projectile.Center).LengthSquared() < dists && (testby.Center - projectile.Center).LengthSquared() < dists * 3 && Collision.CanHitLine(testby.Center, 0, 0, P.Center, 0, 0)).OrderBy(testby => (testby.life));

			if (targets.Count() > 0)
			{
				//.OrderBy(testby => (testby.Center - projectile.Center)
				foreach (NPC target in targets)
				{
					projectile.ai[1] += 2;
					inndx += 1;
					if (projectile.ai[1] > 1000 && index == inndx)
					{
						projectile.ai[0] = target.whoAmI;
					}
				}
			}

			if (projectile.ai[0] >= 0)
            {
				NPC them = Main.npc[(int)projectile.ai[0]];
				Vector2 dist = them.Center - projectile.Center;
				gohere = them.Center+Vector2.Normalize(-dist).RotatedBy(MathHelper.Pi/6f)*64f;
				lookat = them.Center;
				if (dist.LengthSquared() < 240 * 240)
                {
					them.AddBuff(ModContent.BuffType<Buffs.Watched>(), 10);
				}
			}



			//Main.NewText(projectile.ai[1]);
				projectile.velocity += (gohere - projectile.Center) / 600f;
			projectile.Center += (gohere - projectile.Center) / (projectile.ai[1]<1000 ? 5f : 100f);


		}

		public override void PostEffectsDraw(SpriteBatch spriteBatch, float drawScale = 2f)
        {
            float alpha = 1f;
            if (projectile.ai[0] < 1000 && projectile.localAI[0] >= 0)
                alpha = Math.Max((projectile.localAI[0] - 60) / 200f, 0);
            if (projectile.localAI[0] < 0)
                alpha = 1f + projectile.ai[0] / 120f;

            if (alpha <= 0)
                return;

            Texture2D tex = ModContent.GetTexture("SGAmod/Items/Armors/Magatsu/MagatsuNullWatcher");
            Rectangle rect = new Rectangle(0, (tex.Height / 7) * (2+(int)(Math.Min(projectile.ai[1]/250f,4))), tex.Width, tex.Height / 7);
            Rectangle recteye = new Rectangle(0, 0, tex.Width, tex.Height / 7);

            Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 7) / 2f;

            for (int k = 0; k < 1; k++)//projectile.oldPos.Length
			{
                Vector2 drawPos = (projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY)) / drawScale;
                float coloralpha = ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

                float scale = (1f + (projectile.localAI[0] < 0 ? -projectile.localAI[0] / drawScale : 0)) * (2f / drawScale);

                spriteBatch.Draw(tex, drawPos, rect, Color.GreenYellow * coloralpha * 0.75f * alpha, projectile.rotation, drawOrigin, projectile.scale * 1f * scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(tex, drawPos + (lookat == null || lookat == projectile.Center ? Vector2.Zero : (Vector2.Normalize(lookat - projectile.Center) * eyeDist)), recteye, Color.White * coloralpha*0.75f * alpha, projectile.rotation, drawOrigin, projectile.scale * scale, SpriteEffects.None, 0f);
            }

        }

    }

	public class MagatsuDecoy : ModNPC, IPostEffectsDraw
	{

		public override bool Autoload(ref string name)
		{
			name = "Magatsu Decoy";
			return mod.Properties.Autoload;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName automatically assigned from .lang files, but the commented line below is the normal approach.
			// DisplayName.SetDefault("Example Person");
			Main.npcFrameCount[npc.type] = 1;
			NPCID.Sets.ExtraFramesCount[npc.type] = 1;
			NPCID.Sets.AttackFrameCount[npc.type] = 1;
			NPCID.Sets.DangerDetectRange[npc.type] = 0;
			NPCID.Sets.AttackType[npc.type] = 0;
			NPCID.Sets.AttackTime[npc.type] = 90;
			NPCID.Sets.AttackAverageChance[npc.type] = 30;
			NPCID.Sets.HatOffsetY[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.townNPC = false;
			npc.friendly = true;
			npc.width = 32;
			npc.height = 50;
			npc.aiStyle = -1;
			npc.damage = 0;
			npc.noGravity = true;
			npc.defense = 50;
			npc.lifeMax = 500;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.knockBackResist = 0.75f;
			//npc.immortal = true;
			//animationType = NPCID.Guide;
			npc.noGravity = true;
			npc.homeless = true;
			//npc.rarity = 1;

		}
		public override string Texture
		{
			get
			{
				return "SGAmod/Projectiles/FieryRock";
			}
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
			return false;
        }

		public override bool PreAI()
		{
			npc.localAI[0] += 1f;
			//npc.ai[0] = 0;
			//npc.ai[1] = 0;
			//npc.ai[2] = 0;
			//npc.ai[3] = 0;
			npc.velocity /= 1.1f;

			//Main.NewText(npc.ai[3]);

			if (npc.localAI[0]>150)
				npc.life -= 1;

			if (npc.ai[3] >= -90)
            {
				Player owner = Main.player[(int)npc.ai[3]];

				if (owner.dead)
                {
					npc.life = 0;
					return false;
                }

				if (owner.active)
                {
					SGAPlayer.centerOverrideTimerIsActive = 5;
					owner.SGAPly().centerOverrideTimer = (int)Math.Max(owner.SGAPly().centerOverrideTimer,3);
					owner.SGAPly().centerOverridePosition = npc.Center;
                }

            }

			return true;
		}

        public void PostEffectsDraw(SpriteBatch spriteBatch, float drawScale = 2f)
		{
			int framesTotal = 20;
			int frame = 0;

			Texture2D texHead = Main.playerTextures[0, 0];
			Texture2D texBody = Main.playerTextures[0, 3];
			Texture2D texlegs = Main.playerTextures[0, 10];

			Vector2 drawPos = ((npc.Center - Main.screenPosition));

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			Texture2D[] texes = new Texture2D[] { texlegs, texBody, texHead };

			Terraria.Utilities.UnifiedRandom rand = new Terraria.Utilities.UnifiedRandom(npc.whoAmI);

			float trans = MathHelper.Clamp(npc.localAI[0] / 64f, 0f, 1f);
			float smooth = MathHelper.SmoothStep(128f, 0f, trans);

			for (int i = 0; i < 3; i += 1)
			{
				Vector2 loc = Vector2.UnitX.RotatedBy((rand.NextFloat(0.30f, 0.65f)*Main.GlobalTime * (rand.NextBool() ? 1f : -1f))+rand.NextFloat(MathHelper.TwoPi))*(rand.NextFloat(1f, 2f)) *(smooth);
				Texture2D tex = texes[i];
				Rectangle rect = new Rectangle(0, 0, tex.Width, tex.Height / 20);
				Rectangle rect2 = new Rectangle(0, frame * rect.Height, tex.Width, rect.Height);

				spriteBatch.Draw(tex, (drawPos+ loc)/ 2, rect2, Color.Black* trans, 0, rect.Size() / 2f, npc.scale/2, SpriteEffects.FlipHorizontally, 0f);
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
		}

	}


}