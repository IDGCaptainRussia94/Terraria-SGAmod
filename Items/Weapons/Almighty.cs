using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Idglibrary;
using SGAmod.Items.Weapons.SeriousSam;
using SGAmod.Projectiles;
using System.Linq;
using Terraria.Utilities;
using SGAmod.Effects;
using AAAAUThrowing;

namespace SGAmod.Items.Weapons
{
	public class AlmightyWeapon : ModItem
	{
		public override bool Autoload(ref string name)
		{
			return GetType() != typeof(AlmightyWeapon);
		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			float[] highestDamage = { player.meleeDamage, player.magicDamage, player.minionDamage, player.rangedDamage, player.Throwing().thrownDamage };
			add += highestDamage.OrderBy(testby => testby).Reverse().ToArray()[0] - 1f;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			// Get the vanilla damage tooltip
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

				valuez.Insert(1, "Almighty ");
				foreach (string text3 in valuez)
				{
					newline += text3;
				}
				tt.text = newline;
			}
			if (GetType() == typeof(NuclearOption) && SGAmod.Calamity.Item1)
				tooltips.Add(new TooltipLine(mod, "NuclearInferdumbFallout", "Will instantly kill any calamity enemies at max charge"));
			tooltips.Add(new TooltipLine(mod, "AlmightyText", "Almighty Deals armor-piercing damage that scales off your highest stat"));
			tooltips.Add(new TooltipLine(mod, "AlmightyText", "Almighty also skips crits and goes straight to Apocalypticals"));
		}
	}

	public class Megido : AlmightyWeapon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Megido");
			Tooltip.SetDefault("Targets 4 nearby enemies on use\n" + Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stack, adds 30 seconds"));
		}
		public override string Texture => "Terraria/Item_" + ItemID.Darkness;
		public override void SetDefaults()
		{
			base.SetDefaults();

			item.damage = 75;
			item.width = 48;
			item.height = 48;
			item.useTurn = true;
			item.rare = ItemRarityID.Orange;
			item.value = 500;
			item.useStyle = 1;
			item.useAnimation = 50;
			item.useTime = 50;
			item.knockBack = 8;
			item.autoReuse = false;
			item.noUseGraphic = true;
			item.consumable = true;
			item.noMelee = true;
			item.shootSpeed = 2f;
			item.maxStack = 30;
			item.shoot = ModContent.ProjectileType<MegidoProj>();
		}

		public override bool CanUseItem(Player player)
		{
			if (player.SGAPly().AddCooldownStack(100, testOnly: true))
			{
				NPC[] findnpc = SGAUtils.ClosestEnemies(player.Center, 1500, checkWalls: false, checkCanChase: true)?.ToArray();
				if (findnpc != null && findnpc.Length > 0)
					return true;
			}
			return false;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Megido").WithVolume(.7f).WithPitchVariance(.15f), player.Center);
			player.SGAPly().AddCooldownStack(60 * 30);

			for (int i = 0; i < 4; i += 1)
			{
				NPC[] findnpc = SGAUtils.ClosestEnemies(player.Center, 1500, checkWalls: false, checkCanChase: true)?.ToArray();
				NPC target = findnpc[i % findnpc.Count()];

				Projectile proj = Projectile.NewProjectileDirect(player.Center, Vector2.UnitX.RotatedBy(MathHelper.PiOver4 + (i * (MathHelper.TwoPi / 4f))) * 8f, ModContent.ProjectileType<MegidoProj>(), damage, knockBack, player.whoAmI, 0, target.whoAmI);
				proj.ai[1] = target.whoAmI;
				proj.netUpdate = true;
			}



			return false;
		}
	}

	public class MegidoProj : NPCs.PinkyMinionKilledProj
	{

		protected override float ScalePercent => MathHelper.Clamp(projectile.timeLeft / 10f, 0f, Math.Min(projectile.localAI[0] / 3f, 0.75f));
		protected override float SpinRate => 0.20f;

		Vector2 startingloc = default;
		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.aiStyle = -1;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.timeLeft = 300;
		}

		public override string Texture
		{
			get { return "SGAmod/HavocGear/Projectiles/BoulderBlast"; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Megido Proj");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public void CheckApoco(ref int damage,NPC npc,Projectile proj)
        {
			float kb = 0f;
			bool crit = false;
			double[] highestApoco = Main.player[projectile.owner].SGAPly().apocalypticalChance.OrderBy(testby => 10000 - testby).ToArray();

			if (npc.realLife >= 0)
				damage = (int)(damage * 0.10f);

			if (Main.rand.NextFloat(100f)< highestApoco[0])
			npc.SGANPCs().DoApoco(npc, proj, Main.player[projectile.owner], null, ref damage, ref kb, ref crit,2,true);
        }

		public override void ReachedTarget(NPC target)
		{
			Player player = Main.player[projectile.owner];
			int damage = Main.DamageVar(projectile.damage) + target.defense / 2;
			CheckApoco(ref damage, target, projectile);
			target.StrikeNPC(damage, 0, 1, false);
			SGAmod.AddScreenShake(6f, 2400, target.Center);
			Main.player[projectile.owner].addDPS(damage);

			if (Main.netMode != NetmodeID.SinglePlayer)
			{
				NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, target.whoAmI, projectile.damage, 0f, (float)1, 0, 0, 0);
			}

			projectile.velocity = Vector2.Zero;

			for (int i = 0; i < 24; i += 1)
			{
				Vector2 position = Main.rand.NextVector2Circular(16f, 16f);
				int num128 = Dust.NewDust(projectile.Center + position, 0, 0, DustID.AncientLight, 0, 0, 240, Color.Aqua, ScalePercent);
				Main.dust[num128].noGravity = true;
				Main.dust[num128].alpha = 180;
				Main.dust[num128].color = Color.Lerp(Color.Aqua, Color.Blue, Main.rand.NextFloat() % 1f);
				Main.dust[num128].velocity = (Vector2.Normalize(position) * Main.rand.NextFloat(2f, 5f));
			}

			var sound = Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 86);
			if (sound != null)
				sound.Pitch -= 0.50f;

			projectile.ai[0] += 1;
			projectile.timeLeft = (int)MathHelper.Clamp(projectile.timeLeft, 0, 10);
			projectile.netUpdate = true;
		}



		public override void AI()
		{
			if (startingloc == default)
			{
				startingloc = projectile.Center;
			}

			projectile.localAI[0] += 0.25f;

			List<Point> weightedPoints2 = new List<Point>();

			int index = 0;
			int us = 0;

			NPC findnpc = Main.npc[(int)projectile.ai[1]];

			if (findnpc != null && findnpc.active)
			{
				projectile.velocity *= 0.94f;
				if (projectile.localAI[0] > 8f && projectile.ai[0] < 1)
				{
					NPC target = findnpc;
					int dist = 60 * 60;
					Vector2 distto = target.Center - projectile.Center;
					projectile.velocity += Vector2.Normalize(distto).RotatedBy((MathHelper.Clamp(1f - (projectile.localAI[0] - 8f) / 5f, 0f, 1f) * 0.85f) * SpinRate) * 3.20f;
					projectile.velocity = Vector2.Normalize(projectile.velocity) * MathHelper.Clamp(projectile.velocity.Length(), 0f, 32f + projectile.localAI[0]);

					if (projectile.timeLeft > 10 && projectile.ai[0] < 1 && distto.LengthSquared() < dist)
					{
						ReachedTarget(target);
					}
				}
			}
			else
			{
				projectile.timeLeft = (int)MathHelper.Clamp(projectile.timeLeft, 0, 10);
			}

			projectile.velocity *= 0.97f;

			if (projectile.ai[0] > 0)
			{
				projectile.ai[0] += 1;
			}

			int num126 = Dust.NewDust(projectile.Center + Main.rand.NextVector2Circular(8f, 8f), 0, 0, DustID.t_Marble, 0, 0, 240, Color.Aqua, ScalePercent);
			Main.dust[num126].noGravity = true;
			Main.dust[num126].velocity = projectile.velocity * Main.rand.NextFloat(0.1f, 0.5f);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			for (int i = 0; i < projectile.oldPos.Length; i += 1)//dumb hack to get the trails to not appear at 0,0
			{
				if (projectile.oldPos[i] == default)
					projectile.oldPos[i] = projectile.position;
			}


			TrailHelper trail = new TrailHelper("DefaultPass", mod.GetTexture("Noise"));
			//UnifiedRandom rando = new UnifiedRandom(projectile.whoAmI);
			Color colorz = Color.Turquoise;
			trail.color = delegate (float percent)
			{
				return Color.Lerp(colorz, Color.DarkCyan, MathHelper.Clamp(projectile.ai[0] / 7f, 0f, 1f));
			};
			trail.projsize = projectile.Hitbox.Size() / 2f;
			trail.coordOffset = new Vector2(0, Main.GlobalTime * -1f);
			trail.trailThickness = 4 + MathHelper.Clamp(projectile.ai[0], 0f, 30f) / 20f;
			trail.trailThicknessIncrease = 6;
			//trail.capsize = new Vector2(6f, 0f);
			trail.strength = ScalePercent;
			trail.DrawTrail(projectile.oldPos.ToList(), projectile.Center);

			trail = new TrailHelper("BasicEffectDarkPass", mod.GetTexture("TrailEffect"));
			//UnifiedRandom rando = new UnifiedRandom(projectile.whoAmI);
			trail.projsize = projectile.Hitbox.Size() / 2f;
			trail.coordMultiplier = new Vector2(1f, 2f);
			trail.coordOffset = new Vector2(0, Main.GlobalTime * -2f);
			trail.trailThickness = 3 + MathHelper.Clamp(projectile.ai[0], 0f, 30f) / 20f;
			trail.trailThicknessIncrease = 6;
			//trail.capsize = new Vector2(6f, 0f);
			trail.strength = ScalePercent;
			trail.DrawTrail(projectile.oldPos.ToList(), projectile.Center);


			Texture2D mainTex = SGAmod.ExtraTextures[96];

			float blobSize = (MathHelper.Clamp(projectile.localAI[0], 0f, 4f) * 0.1f) + (MathHelper.Clamp(projectile.ai[0], 0f, 30f) * 0.150f);

			/*for (float i = projectile.oldPos.Length/2f; i >= 0; i -= 1f)
			{
				float sizer = 1f-(i / (float)projectile.oldPos.Length);
				Main.spriteBatch.Draw(mainTex, projectile.oldPos[(int)i]+projectile.Hitbox.Size()/2f - Main.screenPosition, null, Color.Lerp(Color.Blue,colorz,i) * trail.strength, 0, mainTex.Size() / 2f, blobSize*(sizer), default, 0);
			}*/

			Main.spriteBatch.Draw(mainTex, projectile.Center - Main.screenPosition, null, colorz * trail.strength, 0, mainTex.Size() / 2f, blobSize, default, 0);

			UnifiedRandom random = new UnifiedRandom(projectile.whoAmI);
			for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 32f)
			{
				float angle = random.NextFloat(MathHelper.TwoPi);
				Vector2 loc = Vector2.UnitX.RotatedBy(angle) * (random.NextFloat(6f, 26f) * blobSize);

				Main.spriteBatch.Draw(mainTex, projectile.Center + loc - Main.screenPosition, null, Color.Lerp(Color.Turquoise, Color.Black, 0.50f) * 0.50f * trail.strength, angle, mainTex.Size() / 2f, new Vector2(blobSize / 12f, blobSize / 6f), default, 0);
			}

			return false;
		}
	}

	public class MorningStar : Megido
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Morning Star");
			Tooltip.SetDefault("Calls down Lucifer's signature move to bring massive destruction in a wide area\n" + Idglib.ColorText(Color.Orange, "Requires 4 Cooldown stacks, adds 150 seconds"));
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.damage = 5000;
			item.width = 48;
			item.height = 48;
			item.useTurn = true;
			item.rare = ItemRarityID.Red;
			item.value = 500;
			item.useStyle = 1;
			item.useAnimation = 50;
			item.useTime = 50;
			item.knockBack = 8;
			item.autoReuse = false;
			item.noUseGraphic = true;
			item.consumable = true;
			item.noMelee = true;
			item.shootSpeed = 2f;
			item.maxStack = 30;
			item.shoot = ModContent.ProjectileType<MorningStarProj>();
		}

		public override bool CanUseItem(Player player)
		{
			if (player.SGAPly().AddCooldownStack(100, 4, testOnly: true))
			{
				return true;
			}
			return false;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			//Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Megido").WithVolume(.7f).WithPitchVariance(.15f), player.Center);
			player.SGAPly().AddCooldownStack(60 * 150, 4);

			int pushYUp = -1;
			player.FindSentryRestingSpot(item.shoot, out var worldX, out var worldY, out pushYUp);

			Projectile proj = Projectile.NewProjectileDirect(new Vector2(worldX, worldY), Vector2.Zero, ModContent.ProjectileType<MorningStarProj>(), damage, knockBack, player.whoAmI);


			return false;
		}
	}

	public class MorningStarProj : MegidoProj
	{
		public class CloudBoom
		{
			public Vector2 position;
			public Vector2 speed;
			public float angle;
			public int cloudType;
			public Vector2 scale = new Vector2(1f, 1f);

			public int timeLeft = 20;
			public int timeLeftMax = 20;
			public CloudBoom(Vector2 position, Vector2 speed, float angle, int cloudtype)
			{
				this.position = position;
				this.speed = speed;
				this.angle = angle;
				this.cloudType = cloudtype;
			}
			public void Update()
			{
				timeLeft -= 1;
				position += speed;
			}

		}

		public List<CloudBoom> boomOfClouds = new List<CloudBoom>();

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.aiStyle = -1;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.timeLeft = 500;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Morning Star Proj");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}



		public override void AI()
		{
			Player player = Main.player[projectile.owner];

			projectile.ai[0] += 1;
			projectile.localAI[0] += 1;
			if (projectile.localAI[0] == 1)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/MorningStar").WithVolume(1f).WithPitchVariance(.15f), projectile.Center);
			}

			if (projectile.ai[0] > 180)
			{
				bool endhit = projectile.timeLeft == 30;
				if (SGAmod.ScreenShake < 16)
					SGAmod.AddScreenShake(6f, 3200, projectile.Center);
				if ((projectile.ai[0] % 10 == 0 && projectile.timeLeft > 30) || endhit)
				{
					foreach (NPC enemy in Main.npc.Where(testby => testby.active && !testby.friendly && !testby.dontTakeDamage))
					{
						Rectangle rect = new Rectangle((int)projectile.Center.X - 240, (int)projectile.Center.Y - 1000, 480, 1200);
						if (endhit)
							rect = new Rectangle((int)projectile.Center.X - 1600, (int)projectile.Center.Y - 1600, 3200, 3200);


						if (enemy.Hitbox.Intersects(rect))
						{
							int damage = (int)((Main.DamageVar((projectile.damage)) + enemy.defense / 2) / (endhit ? 1f : 10f));
							CheckApoco(ref damage, enemy, projectile);
							enemy.StrikeNPC(damage, 0, 1, false);
							Main.player[projectile.owner].addDPS(damage);
						}
					}
				}


				float scaleUpeffect = 0.75f + ((float)Math.Pow((projectile.localAI[0] - 180f) / 160f, 4f));

				for (int i = 0; i < 8; i += 1)
				{
					CloudBoom boomer = new CloudBoom(projectile.Center + Main.rand.NextVector2Circular(260f, 120f), Vector2.UnitX.RotatedBy(-Main.rand.NextFloat(MathHelper.Pi)) * Main.rand.NextFloat(20f, 26f) * (0.45f + (scaleUpeffect / 3f)), Main.rand.NextFloat(MathHelper.TwoPi), Main.rand.Next(1, 7));
					boomer.scale = Vector2.One * (0.60f * scaleUpeffect) * new Vector2(Main.rand.NextFloat(0.50f, 0.75f), Main.rand.NextFloat(0.75f, 1f));

					boomOfClouds.Add(boomer);
				}
				foreach (CloudBoom cb in boomOfClouds.Where(testby => testby.timeLeft > 0))
				{
					cb.Update();
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			float alpha = 1f;
			Texture2D statTex = ModContent.GetTexture("SGAmod/Extra_57b");
			Texture2D beamTex = ModContent.GetTexture("SGAmod/LightBeam");
			Texture2D glowOrb = ModContent.GetTexture("SGAmod/GlowOrb");
			Vector2 offsetbeam = new Vector2(beamTex.Width / 2f, beamTex.Height / 4f);

			Vector2 starHalf = statTex.Size() / 2f;
			float timeLeft = MathHelper.Clamp(projectile.timeLeft / 30f, 0f, 1f);
			float beamAlpha = MathHelper.Clamp((projectile.localAI[0] - 180) / 20f, 0f, 1f);
			float scaleUpeffect = 0.75f + ((float)Math.Pow((projectile.localAI[0] - 180f) / 160f, 4f));
			float endalpha = (1f - MathHelper.Clamp((scaleUpeffect - 3f) / 6f, 0f, 1f));


			List<(float, Vector2, float, float, Color)> listofstuff = new List<(float, Vector2, float, float, Color)>();

			UnifiedRandom random = new UnifiedRandom(projectile.whoAmI);

			//Stars

			for (int i = 10; i < 160; i += 1)
			{
				float progress = (random.NextFloat(1f) +
					Main.GlobalTime * (random.NextFloat(0.04f, 0.075f) * (1f + beamAlpha * 25f))
					) % 1f;

				Vector2 pos = new Vector2(random.Next(-256, 256), -1200 + (progress * 1500f));
				float alphaentry = (1f - MathHelper.Clamp(((i * 2) - projectile.localAI[0]) / 60f, 0f, 1f)) * ((float)Math.Sin(progress * MathHelper.Pi));
				float rot = (random.NextFloat(MathHelper.TwoPi) + (random.NextFloat(-0.01f, 0.01f) * Main.GlobalTime)) * (1f - beamAlpha);
				Color color = Main.hslToRgb(random.NextFloat(1f), 0.85f, 0.95f) * 0.5f;
				listofstuff.Add((progress, pos, alphaentry, rot, color));
			}

			foreach ((float, Vector2, float, float, Color) entry in listofstuff.OrderBy(testby => testby.Item1))
			{
				if (entry.Item3 > 0)
					Main.spriteBatch.Draw(statTex, projectile.Center + entry.Item2 - Main.screenPosition, null, entry.Item5 * entry.Item3 * endalpha * timeLeft, entry.Item4, starHalf / 2f, new Vector2(1f, 1f + beamAlpha * 2f) * 0.50f, default, 0);
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			//Sky Beams

			for (int i = 0; i < 10; i += 1)
			{
				float alpha3 = (MathHelper.Clamp((projectile.localAI[0] - (i * 1.25f)) / 160f, 0f, 1f));
				Vector2 beamscale = new Vector2(1f + projectile.localAI[0] / 240f, 8f + projectile.localAI[0] / 320f);
				Vector2 offset = new Vector2(random.NextFloat(-64f, 64f), -640);
				float rot = 0f;

				Main.spriteBatch.Draw(beamTex, projectile.Center + offset - Main.screenPosition, null, Color.Lerp(Color.White, Color.Aqua, 0.50f) * endalpha * alpha3 * timeLeft * (0.20f + (beamAlpha * 0.05f)), rot, offsetbeam, beamscale, default, 0);

			}

			//Big ass laser comes down!
			if (projectile.localAI[0] > 180)
			{
				UnifiedRandom randomz = new UnifiedRandom(projectile.whoAmI);

				float max = 3;
				//3 trails as the lasers
				for (int ii = 0; ii < max; ii += 1)
				{
					List<Vector2> poses = new List<Vector2>();
					for (float f = 0; f < 2200; f += 25)
					{
						poses.Add(new Vector2(projectile.Center.X + (float)Math.Sin((ii * (MathHelper.TwoPi / max)) + (Main.GlobalTime * 12f) + (f / 400f)) * 90f, (projectile.Center.Y - f)));
					}

					TrailHelper trail = new TrailHelper("BasicEffectAlphaPass", mod.GetTexture("TrailEffect"));
					//UnifiedRandom rando = new UnifiedRandom(projectile.whoAmI);
					Color colorz = Color.Aqua;
					trail.projsize = projectile.Hitbox.Size() / 2f;
					trail.coordOffset = new Vector2(0, Main.GlobalTime * randomz.NextFloat(6.2f, 9f));
					trail.coordMultiplier = new Vector2(1f, randomz.NextFloat(1.5f, 4f));

					trail.strength = beamAlpha * endalpha * timeLeft * 8f;
					trail.strengthPow = 2f;
					trail.doFade = true;

					trail.color = delegate (float percent)
					{
						float alphacol = beamAlpha;
						return Color.Lerp(Color.Turquoise, colorz, MathHelper.Clamp(projectile.ai[0] / 7f, 0f, 1f));
					};


					float extra = randomz.NextFloat(MathHelper.TwoPi);
					float randc = randomz.NextFloat(4f, 6f);
					float randd = randomz.NextFloat(2f, 4f);
					float rande = 1f + (float)Math.Sin(Main.GlobalTime * randomz.NextFloat(1f, 1.25f)) * 0.15f;

					trail.trailThicknessFunction = delegate (float percent)
					{
						float math = (float)Math.Sin((Main.GlobalTime * -randc) + (percent * MathHelper.TwoPi * randd) + extra);
						float beamzz = MathHelper.Clamp((beamAlpha * 2f) - percent, 0f, 1f);

						return (90f + math * 45f) * (beamzz * rande);
					};

					trail.DrawTrail(poses, projectile.Center);

				}

				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

				//Expanding orb at epicenter

				Color orbcolor = Color.Lerp(Color.PaleTurquoise, Color.White, MathHelper.Clamp((scaleUpeffect - 2f) / 6f, 0f, 1f)) * beamAlpha * timeLeft;

				Vector2 halfGlow = glowOrb.Size() / 2f;
				float scaleUpeffect2 = 0.75f + ((float)Math.Pow((projectile.localAI[0] - 180f) / 240f, 15f));

				Main.spriteBatch.Draw(glowOrb, projectile.Center - Main.screenPosition, null, orbcolor, 0, halfGlow, (new Vector2(1.45f, 1.15f) * scaleUpeffect) * beamAlpha, default, 0);

				//Smoke and clouds

				foreach (CloudBoom cb in boomOfClouds.Where(testby => testby.timeLeft > 0))
				{
					Texture2D cloudTex = ModContent.GetTexture("SGAmod/NPCs/Hellion/Clouds" + cb.cloudType);
					float cbalpha = MathHelper.Clamp(cb.timeLeft / (float)cb.timeLeftMax, 0f, 1f);
					float cloudfadeAlpha = Math.Min((cb.timeLeftMax - cb.timeLeft) / 12f, 1f) * 0.75f;

					Main.spriteBatch.Draw(cloudTex, cb.position - Main.screenPosition, null, Color.Lerp(Color.Lerp(Color.Aqua, Color.DarkCyan, cbalpha), Color.White, MathHelper.Clamp((scaleUpeffect - 2f) / 3f, 0f, 1f)) * beamAlpha * timeLeft * cbalpha * cloudfadeAlpha * endalpha, cb.angle, cloudTex.Size() / 2f, cb.scale, default, 0);
				}

				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

				//Expanding That covers all in the end

				Main.spriteBatch.Draw(glowOrb, projectile.Center - Main.screenPosition, null, orbcolor * MathHelper.Clamp((scaleUpeffect - 1f) / 12f, 0f, 1f) * (MathHelper.Clamp((projectile.timeLeft - 20f) / 20f, 0f, 1f)), 0, halfGlow, (new Vector2(0.8f, 0.6f) * scaleUpeffect2) * beamAlpha, default, 0);

			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			return false;
		}
	}

	public class CataNukePlayer : ModPlayer
	{
		public int charge = 0;
		public int ChargeMax => 100000;
		public float ChargePercent => (float)charge / (float)ChargeMax;
		public int ChargeSpeed => HeldNuke ? (int)(Math.Min(player.lifeRegen, 20) * MathHelper.Clamp(player.lifeRegenTime / 300f, 0f, 5f)) : 0;

		public bool HasNuke => player.HasItem(ModContent.ItemType<NuclearOption>());
		public bool HeldNuke => player.HeldItem.type == ModContent.ItemType<NuclearOption>()


		public override void ResetEffects()
		{
			//nil
		}

		public override void PostUpdate()
		{
			if (HasNuke)
			{
				if (HeldNuke)
				{
					float square = 96f * 96f;
					foreach (Projectile proj in Main.projectile.Where(testby => testby.active && !testby.friendly && testby.hostile && (testby.Center - player.Center).LengthSquared() < square && testby.SGAProj().grazed == false))
					{
						proj.SGAProj().grazed = true;
						var snd = Main.PlaySound(SoundID.Item35, (int)proj.Center.X, (int)proj.Center.Y);
						if (snd != null)
						{
							snd.Pitch = -0.75f;
						}
						charge += 100 + (proj.damage * 10);
					}
				}
				charge = (int)MathHelper.Clamp(charge + ChargeSpeed, 0f, ChargeMax);
			}
			else
			{
				charge = 0;
			}
		}
	}

	public class NuclearOption : Megido
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nuclear Option");
			Tooltip.SetDefault("'Unleash the full raw, unfiltered, cataclysmic wrath of the british...'\nCharges up by grazing projectiles while holding this item\nSends out a initial shockwave, afterwords only the fireball does damage\nAt 50% or higher, activate to unleash a Nuclear Explosion\nVaporizes most projectiles, and has more range and damage at higher charge");
		}

        public override string Texture => "SGAmod/Items/Weapons/NuclearOption";

        public override void SetDefaults()
		{
			base.SetDefaults();
			item.damage = 250;
			item.width = 48;
			item.height = 48;
			item.useTurn = true;
			item.rare = ItemRarityID.Cyan;
			item.value = 500;
			item.useStyle = 1;
			item.useAnimation = 50;
			item.useTime = 50;
			item.knockBack = 8;
			item.autoReuse = false;
			item.noUseGraphic = true;
			item.consumable = false;
			item.noMelee = true;
			item.shootSpeed = 1f;
			item.maxStack = 1;
			item.shoot = ModContent.ProjectileType<NuclearOptionProj>();
		}

        public override bool CanUseItem(Player player)
		{
			if (player.GetModPlayer<CataNukePlayer>().ChargePercent>0.50f)
			{
				return true;
			}
			return false;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.lifeRegen = 0;
			player.lifeRegenTime = 0;
			float perc = (player.GetModPlayer<CataNukePlayer>().ChargePercent * player.GetModPlayer<CataNukePlayer>().ChargePercent);
			Projectile proj = Projectile.NewProjectileDirect(player.Center, Vector2.Zero, ModContent.ProjectileType<NuclearOptionProj>(), (int)(damage* perc), knockBack, player.whoAmI);
			proj.ai[1] = player.GetModPlayer<CataNukePlayer>().ChargePercent;
			player.GetModPlayer<CataNukePlayer>().charge = 0;
			proj.netUpdate = true;
			return false;
		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{

			Texture2D inner = mod.GetTexture("BoostBar");

			Vector2 slotSize = new Vector2(52f, 52f)* scale;
			position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
			Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
			Vector2 textureOrigin = Vector2.Zero;

			slotSize.X /= 1.0f;
			slotSize.Y = -slotSize.Y / 4f;

			Vector2 HPHeight = new Vector2(1f, 1f);

			spriteBatch.Draw(Main.itemTexture[item.type], drawPos, null, drawColor, Main.GlobalTime, Main.itemTexture[item.type].Size() / 2f, Main.inventoryScale, SpriteEffects.None, 0f);

			//Main.spriteBatch.End();
			//Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);

			Vector2 scalerr = Vector2.One * new Vector2(slotSize.X/2f, 1f);
			if (Main.rand.Next(100) < 999)
			{
				spriteBatch.Draw(inner, drawPos - new Vector2(slotSize.X / 2, slotSize.Y), new Rectangle(2, 0, 2, inner.Height), Color.White, 0, textureOrigin, scalerr * HPHeight, SpriteEffects.None, 0f);
				CataNukePlayer cataply = Main.LocalPlayer.GetModPlayer<CataNukePlayer>();
				spriteBatch.Draw(inner, drawPos - new Vector2(slotSize.X / 2, slotSize.Y), new Rectangle(2, 0, 2, inner.Height), Color.Turquoise, 0, textureOrigin, scalerr * new Vector2(cataply.ChargePercent, 1f) * HPHeight, SpriteEffects.None, 0f);
			}

			spriteBatch.Draw(inner, drawPos - new Vector2(0, slotSize.Y), new Rectangle(0, 2, 2, inner.Height), Color.White, 0, textureOrigin, Main.inventoryScale * HPHeight, SpriteEffects.None, 0f);
			spriteBatch.Draw(inner, drawPos - new Vector2(0, slotSize.Y), new Rectangle(0, 0, 2, inner.Height), Color.White, 0, textureOrigin, Main.inventoryScale * HPHeight, SpriteEffects.FlipHorizontally, 0f);


			//spriteBatch.Draw(inner, drawPos - new Vector2(-slotSize.X / 2, slotSize.Y), new Rectangle(inner.Width - 2, 0, 2, inner.Height), Color.White, 0, textureOrigin, Main.inventoryScale * HPHeight, SpriteEffects.None, 0f);

			//Main.spriteBatch.End();
			//Main.spriteBatch.Begin(SpriteSortMode.Deferred, default, default, default, default, null, Main.UIScaleMatrix);

			return false;
		}

	}

	public class NuclearOptionProj : MorningStarProj
	{

		public List<CloudBoom> raysOfLight = new List<CloudBoom>();

		Vector2 OverallScale => (Vector2.One*3f*projectile.ai[1])*((float)Math.Pow(projectile.localAI[0] / 30f, 0.32f));

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.aiStyle = -1;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.timeLeft = 300;
			projectile.friendly = true;
		}

		public override string Texture
		{
			get { return "Terraria/Misc/MoonExplosion/Explosion"; }
		}

        public override bool Autoload(ref string name)
        {
            SGAmod.PostUpdateEverythingEvent += SGAmod_PostUpdateEverythingEvent;
			return true;
        }

        private void SGAmod_PostUpdateEverythingEvent()
        {
			CataLogo.DrawToRenderTarget();
		}

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nuclear Option Proj");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];

			if (!Main.dedServ)
			{
				CataLogo.Load();
				CataLogo.oneUpdate = true;
				//CataLogo.DrawToRenderTarget();
			}

			projectile.ai[0] += 1;
			projectile.localAI[0] += 1;
			if (projectile.localAI[0] == 1)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Megidolaon").WithVolume(1f).WithPitchVariance(.15f), projectile.Center);
				SGAmod.AddScreenShake(64f, 2400, projectile.Center);
			}
			float lenn = 512 * (projectile.ai[1]* projectile.ai[1]) +(projectile.ai[0]<60 ? projectile.ai[0]*25 : 0)+(OverallScale.X * 16f);

			if (projectile.ai[0] % 10 == 0 && projectile.timeLeft>30)
			{
				foreach (Projectile proj in Main.projectile.Where(testby => testby.active && testby != projectile && testby.owner >= 255 && testby.hostile && !testby.friendly && (testby.Center - projectile.Center).Length() < lenn))
				{
					if (proj.timeLeft > 3 && !proj.SGAProj().raindown && proj.whoAmI != projectile.whoAmI && proj.damage>0 && proj.owner >= 255 && proj.hostile && !proj.friendly && (proj.modProjectile == null || (proj.modProjectile != null && !(proj.modProjectile is INonDestructableProjectile))))
					{
						proj.SGAProj().raindown = true;
						proj.timeLeft = 3;

						for (int i = 0; i < 24; i += 1)
						{
							Vector2 position = Main.rand.NextVector2Circular(16f, 16f);
							int num128 = Dust.NewDust(proj.Center + position, 0, 0, DustID.AncientLight, 0, 0, 240, Color.Aqua, 3.25f - (i / 12f));
							Main.dust[num128].noGravity = true;
							Main.dust[num128].alpha = 160;
							Main.dust[num128].color = Color.Lerp(Color.Aqua, Color.Blue, Main.rand.NextFloat() % 1f);
							Main.dust[num128].velocity = (Vector2.Normalize(position) * Main.rand.NextFloat(2f, 5f)) + (i*Vector2.Normalize(proj.Center - projectile.Center)*0.075f);
						}
					}
				}

				foreach (NPC npc in Main.npc.Where(testby => testby.active && !testby.friendly && !testby.dontTakeDamage && (testby.Center - projectile.Center).Length() < lenn))
				{
					int damage = Main.DamageVar(projectile.damage)+npc.defense/2;
					CheckApoco(ref damage, npc, projectile);
					npc.StrikeNPC(damage, 0, 1, false);
					player.addDPS(damage);
					npc.AddBuff(ModContent.BuffType<Buffs.RadioDebuff>(), 60 * 20);

					if (projectile.ai[1] >= 1f)
					{
						if (SGAmod.Calamity.Item1)
						{
							if (npc.modNPC != null && npc.modNPC.mod.Name == "Calamity")
							{
								npc.life = 1;
								npc.StrikeNPC(666, 1337, 1, true);
								if (npc.active)
								{
									npc.active = false;
									npc.modNPC.NPCLoot();
								}
							}
						}
					}

					for (int i = 0; i < 16; i += 1)
					{
						Vector2 position = Main.rand.NextVector2Circular(16f, 16f);
						int num128 = Dust.NewDust(npc.Center + position, 0, 0, DustID.AncientLight, 0, 0, 240, Color.Aqua, 1.50f - (i / 24f));
						Main.dust[num128].noGravity = true;
						Main.dust[num128].alpha = 130;
						Main.dust[num128].color = Color.Lerp(Color.Aqua, Color.Blue, Main.rand.NextFloat() % 1f);
						Main.dust[num128].velocity = (Vector2.Normalize(position) * Main.rand.NextFloat(6f, 12f)) + Vector2.Normalize(npc.Center - projectile.Center)*20f;
					}
				}
			}



			if (projectile.timeLeft > 30)
            {
				if (SGAmod.ScreenShake<8)
				SGAmod.AddScreenShake(5f, projectile.timeLeft*5, projectile.Center);
			}

			float scaleUpeffect = 1f;

			float explodScale = 16f / MathHelper.Clamp(1f + (projectile.timeLeft / 4f), 0.001f, 100f);
			float cataScale = 8f / MathHelper.Clamp(1f + (projectile.timeLeft / 4f), 0.001f, 100f);

			for (int i = 0; i < 2; i += 1)
			{
				Vector2 velo = new Vector2(Main.rand.NextFloat(-1f, 1f))*0.05f;
				CloudBoom boomer = new CloudBoom(new Vector2(Main.rand.NextFloat(MathHelper.TwoPi),0), velo, Main.rand.NextFloat(MathHelper.TwoPi), Main.rand.Next(1, 7));
				boomer.scale = (Vector2.One * (1f * scaleUpeffect) * new Vector2(Main.rand.NextFloat(0.50f, 0.75f), Main.rand.NextFloat(0.75f, 1f))) * 0.50f;

				boomer.angle = Main.rand.NextFloat(MathHelper.TwoPi);

				raysOfLight.Add(boomer);
			}
			foreach (CloudBoom cb in raysOfLight.Where(testby => testby.timeLeft > 0))
			{
				cb.angle += cb.speed.X;
				cb.position -= cb.speed;
				cb.Update();
			}



			for (int i = 0; i < 32; i += 1)
			{
				Vector2 velo = Vector2.UnitX.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(12f, 18f)* (1f+explodScale+ cataScale) *(OverallScale*0.20f);
				CloudBoom boomer = new CloudBoom(projectile.Center + (Vector2.Normalize(velo)* explodScale), velo * (0.45f + (scaleUpeffect / 3f)), Main.rand.NextFloat(MathHelper.TwoPi), Main.rand.Next(1, 7));
				boomer.scale = (Vector2.One * (1f * scaleUpeffect) * new Vector2(Main.rand.NextFloat(0.50f, 0.75f), Main.rand.NextFloat(0.75f, 1f)))*0.50f;

				boomOfClouds.Add(boomer);
			}
			foreach (CloudBoom cb in boomOfClouds.Where(testby => testby.timeLeft > 0))
			{
				cb.Update();
			}

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			float alpha = MathHelper.Clamp(projectile.timeLeft/60f,0f,1f);
			float alpha2 = MathHelper.Clamp(projectile.timeLeft / 20f, 0f, 1f);
			float alpha3 = 1f-MathHelper.Clamp((projectile.timeLeft-20f) / 90f, 0f, 1f);
			float alpha4 = 1f - MathHelper.Clamp(projectile.localAI[0]/60f, 0f, 1f);
			float alpha5 = MathHelper.Clamp((projectile.timeLeft - 20f) / 20f, 0f, 1f);

			Texture2D explosionTex = Main.projectileTexture[projectile.type];
			Texture2D lightBeamTex = ModContent.GetTexture("SGAmod/LightBeam");
			Texture2D glowOrbTex = ModContent.GetTexture("SGAmod/GlowOrb");

			Vector2 exploorig = new Vector2(explosionTex.Width, explosionTex.Height / 7) / 2f;
			Vector2 lightorig = new Vector2(lightBeamTex.Width, lightBeamTex.Height / 4) / 2f;
			Vector2 orgCenter = glowOrbTex.Size() / 2f;

			float explodScale = 16f / MathHelper.Clamp(1f + (projectile.timeLeft / 4f), 0.001f, 100f);
			float explodScale2 = 4f / MathHelper.Clamp(1f + (projectile.timeLeft / 16f), 0.001f, 100f);
			float cataScale = 64f / MathHelper.Clamp(1f + (projectile.timeLeft / 4f), 0.001f, 100f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			if (alpha4 > 0f)
				Main.spriteBatch.Draw(glowOrbTex, projectile.Center - Main.screenPosition, null, Color.Aqua * alpha4 * 1f, 0, orgCenter, OverallScale * (20f * (1f - alpha4)), default, 0);


			Main.spriteBatch.Draw(glowOrbTex, projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Turquoise, Color.White, MathHelper.Clamp(explodScale * 2f, 0f, 1f)) * alpha * 0.50f, 0, orgCenter, OverallScale * 3f, default, 0);

			Color boomColor = Color.Aqua;

			Main.spriteBatch.Draw(glowOrbTex, projectile.Center - Main.screenPosition, null, boomColor * alpha2, 0, orgCenter, OverallScale* explodScale, default, 0);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


			foreach (CloudBoom cb in raysOfLight)
			{
				float timePercent = (cb.timeLeft / (float)cb.timeLeftMax);
				float timePercentBack = 1f - (cb.timeLeft / (float)cb.timeLeftMax);
				float cbAlpha = MathHelper.Clamp(timePercent * 4f, 0f, 1f);
				Color color = Color.DarkTurquoise;
				Vector2 explosionSize = (Vector2.One * 0.20f) + ((Vector2.One * 0.80f) * timePercent) * cb.scale;

				Main.spriteBatch.Draw(lightBeamTex, projectile.Center - Main.screenPosition, null, color * cbAlpha * alpha * 0.50f, -MathHelper.PiOver2+cb.angle+(cb.position.X), lightorig, explosionSize*new Vector2(0.5f,1.5f) * OverallScale, default, 0);
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			foreach (CloudBoom cb in boomOfClouds)
			{
				float timePercent = (cb.timeLeft / (float)cb.timeLeftMax);
				float timePercentBack = 1f-(cb.timeLeft / (float)cb.timeLeftMax);
				float cbAlpha = MathHelper.Clamp(timePercent * 4f, 0f, 1f);
				Color color = Color.White;
				Vector2 explosionSize = (Vector2.One * 0.20f)+((Vector2.One*0.80f)*timePercent)*cb.scale;

				Rectangle rect = new Rectangle(0, (explosionTex.Height / 7)* (int)(timePercentBack * 7), explosionTex.Width, explosionTex.Height / 7);
				Main.spriteBatch.Draw(explosionTex, cb.position - Main.screenPosition, rect, color * cbAlpha*alpha*0.25f, cb.angle, exploorig / 2f, explosionSize* OverallScale, default, 0);
			}

			CataLogo.Draw(projectile.Center-Main.screenPosition,alpha* alpha5, new Vector2(3f,3f)*OverallScale* cataScale);



			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			Main.spriteBatch.Draw(glowOrbTex, projectile.Center - Main.screenPosition, null, Color.Turquoise * alpha5 * 1f, 0, orgCenter, OverallScale * cataScale * 0.025f, default, 0);

			Main.spriteBatch.Draw(glowOrbTex, projectile.Center - Main.screenPosition, null, Color.White * alpha2* alpha3, 0, orgCenter, OverallScale * explodScale, default, 0);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


			return false;
		}

	}







	public class CataLogo
	{
		public static RenderTarget2D cataSurface;

		public static Effect cataEffect;
		public static Effect radialEffect;
		public static bool hasLoaded = false;
		public static bool oneUpdate = false;


		public static void Load()
		{
			if (hasLoaded)
				return;

			cataEffect = SGAmod.CataEffect;
			radialEffect = SGAmod.RadialEffect;
			cataSurface = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.blackTileTexture.Width * 32, Main.blackTileTexture.Height * 32, false, Main.graphics.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24, 1, RenderTargetUsage.DiscardContents);
			hasLoaded = true;
		}

		public static void Unload()
		{
			if (!cataSurface.IsDisposed)
				cataSurface.Dispose();
			if (!cataEffect.IsDisposed)
				cataEffect.Dispose();
			if (!radialEffect.IsDisposed)
				radialEffect.Dispose();
		}

		public static void DrawToRenderTarget()
		{
			if (Main.dedServ || !hasLoaded || !oneUpdate)
				return;

			oneUpdate = false;

			BlendState negaBlending = new BlendState
			{

				ColorSourceBlend = Blend.Zero,
				ColorDestinationBlend = Blend.InverseSourceColor,

				AlphaSourceBlend = Blend.Zero,
				AlphaDestinationBlend = Blend.InverseSourceColor

			};

			RenderTargetBinding[] binds = Main.graphics.GraphicsDevice.GetRenderTargets();

			Main.graphics.GraphicsDevice.SetRenderTarget(cataSurface);
			Main.graphics.GraphicsDevice.Clear(Color.Transparent);

			//Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);

			float edgesize = 0.40f;// + (float)(Math.Sin(Main.GlobalTime * 2f) * 0.10f);
			float ballsize = 0.05f;// + (float)(Math.Sin(Main.GlobalTime) * 0.05f);
			float ballgapsize = 0.05f;// + (float)(Math.Sin(Main.GlobalTime * 1.2f) * 0.02f);

			Effect RadialEffect = radialEffect;

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);

			for (int i = 0; i < 3; i += 1)
			{
				for (float f = -1f; f < 2; f += 2)
				{
					RadialEffect.Parameters["overlayTexture"].SetValue(SGAmod.Instance.GetTexture("Fire"));
					RadialEffect.Parameters["alpha"].SetValue(0.50f);
					RadialEffect.Parameters["texOffset"].SetValue(new Vector2(f * Main.GlobalTime * 0.25f, -Main.GlobalTime * 0.575f));
					RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(3f, 1f + i));
					RadialEffect.Parameters["ringScale"].SetValue(0.36f);
					RadialEffect.Parameters["ringOffset"].SetValue(0.16f);
					RadialEffect.Parameters["ringColor"].SetValue(Color.Turquoise.ToVector3());
					RadialEffect.Parameters["tunnel"].SetValue(false);

					RadialEffect.CurrentTechnique.Passes["Radial"].Apply();

					Main.spriteBatch.Draw(Main.blackTileTexture, Main.blackTileTexture.Size() * 16f, null, Color.White, 0, Main.blackTileTexture.Size() * 0.5f, 96f, SpriteEffects.None, 0f);
				}
			}

			RadialEffect.Parameters["overlayTexture"].SetValue(SGAmod.Instance.GetTexture("Fire"));
			RadialEffect.Parameters["alpha"].SetValue(5f);
			RadialEffect.Parameters["texOffset"].SetValue(new Vector2(0, -Main.GlobalTime * 0.2575f));
			RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(0.5f, 1f));
			RadialEffect.Parameters["ringScale"].SetValue(0.1f);
			RadialEffect.Parameters["ringOffset"].SetValue((ballsize + ballgapsize) * (32f / 96f) * 2.5f);
			RadialEffect.Parameters["ringColor"].SetValue(Color.Turquoise.ToVector3());
			RadialEffect.Parameters["tunnel"].SetValue(false);

			RadialEffect.CurrentTechnique.Passes["RadialAlpha"].Apply();

			Main.spriteBatch.Draw(Main.blackTileTexture, Main.blackTileTexture.Size() * 16f, null, Color.White, 0, Main.blackTileTexture.Size() * 0.5f, 96f, SpriteEffects.None, 0f);

			Main.spriteBatch.End();

			Main.spriteBatch.Begin(SpriteSortMode.Immediate, negaBlending, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);

			cataEffect.Parameters["angleAdd"].SetValue(Main.GlobalTime * 1f);
			cataEffect.Parameters["edges"].SetValue(3);
			cataEffect.Parameters["ballSize"].SetValue(ballsize);
			cataEffect.Parameters["edgeSize"].SetValue(edgesize);
			cataEffect.Parameters["ballEdgeGap"].SetValue(ballgapsize);

			cataEffect.CurrentTechnique.Passes["CataLogoInverse"].Apply();

			Main.spriteBatch.Draw(Main.blackTileTexture, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 32f, SpriteEffects.None, 0f);

			Main.spriteBatch.End();

			Main.graphics.GraphicsDevice.SetRenderTargets(binds);
		}

		public static void Draw(Vector2 where, float alpha, Vector2 scale)
		{

			Effect RadialEffect = radialEffect;
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			RadialEffect.Parameters["overlayTexture"].SetValue(SGAmod.Instance.GetTexture("Fire"));
			RadialEffect.Parameters["alpha"].SetValue(4f * alpha);
			RadialEffect.Parameters["texOffset"].SetValue(new Vector2(0, Main.GlobalTime * 0.575f));
			RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(3f, 0.75f));
			RadialEffect.Parameters["ringScale"].SetValue(0.20f);
			RadialEffect.Parameters["ringOffset"].SetValue(0.14f);
			RadialEffect.Parameters["ringColor"].SetValue(Color.Turquoise.ToVector3());
			RadialEffect.Parameters["tunnel"].SetValue(true);

			RadialEffect.CurrentTechnique.Passes["Radial"].Apply();

			Main.spriteBatch.Draw(Main.blackTileTexture, where, null, Color.White, 0, Main.blackTileTexture.Size() * 0.5f, scale, SpriteEffects.None, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);


			Main.spriteBatch.Draw(cataSurface, where, null, Color.White * alpha, 0, (Vector2.One * cataSurface.Size()) / 2f, scale / 18f, SpriteEffects.None, 0f);


			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);


		}
	}
}
