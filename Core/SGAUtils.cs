//#define WebmilioCommonsPresent
#define DEBUG
#define DefineHellionUpdate
#define Dimensions


using System;
using System.Linq;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.GameContent.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria.DataStructures;
using Terraria.GameContent.UI;
using Idglibrary;
using System.IO;
using System.Diagnostics;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.World;
using SGAmod.NPCs;
using SGAmod.NPCs.Wraiths;
using SGAmod.NPCs.Hellion;
using SGAmod.NPCs.SpiderQueen;
using SGAmod.NPCs.Murk;
using SGAmod.NPCs.Sharkvern;
using SGAmod.NPCs.Cratrosity;
using SGAmod.HavocGear.Items;
using SGAmod.HavocGear.Items.Weapons;
using SGAmod.HavocGear.Items.Accessories;
using SGAmod.Items;
using SGAmod.Items.Weapons;
using SGAmod.Items.Armors;
using SGAmod.Items.Accessories;
using SGAmod.Items.Consumable;
using SGAmod.Items.Weapons.Caliburn;
using SGAmod.UI;
using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using SGAmod.Items.Weapons.SeriousSam;
#if Dimensions
using SGAmod.Dimensions;
#endif

//using SubworldLibrary;

namespace SGAmod
{

	/*public class Blank : Subworld
	{
		public override int width => 800;
		public override int height => 400;
		public override ModWorld modWorld => SGAWorld.Instance;

		public override SubworldGenPass[] tasks => new SubworldGenPass[]
		{
		new SubworldGenPass("Loading", 1f, progress =>
		{
			progress.Message = "Loading"; //Sets the text above the worldgen progress bar
            Main.worldSurface = Main.maxTilesY - 42; //Hides the underground layer just out of bounds
            Main.rockLayer = Main.maxTilesY; //Hides the cavern layer way out of bounds
        })
		};

		public override void Load()
		{
			Main.dayTime = true;
			Main.time = 27000;
			Main.worldRate = 0;
		}
	}*/
	public class PostDrawCollection
	{
		public Vector3 light;

		public PostDrawCollection(Vector3 light)
		{
			this.light = light;
		}
	}

	public static class SGAUtils
	{
		//Again, from Joost, thanks man
		public static Vector2 PredictiveAim(float speed, Vector2 origin, Vector2 target, Vector2 targetVelocity, bool ignoreY)
		{
			Vector2 vel = (ignoreY ? new Vector2(targetVelocity.X, 0) : targetVelocity);
			Vector2 predictedPos = target + targetVelocity + (vel * (Vector2.Distance(target, origin) / speed));
			predictedPos = target + targetVelocity + (vel * (Vector2.Distance(predictedPos, origin) / speed));
			predictedPos = target + targetVelocity + (vel * (Vector2.Distance(predictedPos, origin) / speed));
			return predictedPos;
		}

		public static int ItemToMusic(int itemtype)
		{
			int value;
			if (SGAmod.itemToMusicReference.TryGetValue(itemtype, out value))
			{
				return value;
			}
			else
			{
				return -1;
			}
		}

		public static int MusicToItem(int itemtype)
		{
			int value;
			if (SGAmod.musicToItemReference.TryGetValue(itemtype, out value))
			{
				return value;
			}
			else
			{
				return -1;
			}
		}

		public static List<NPC> ClosestEnemies(Vector2 Center,float maxdist,Vector2 Center2=default,List<Point>AddedWeight=default)
		{

			if (Center2 == default)
				Center2 = Center;

			if (AddedWeight == default)
				AddedWeight = new List<Point>();

			List<NPC> closestnpcs = new List<NPC>();
			for (int i = 0; i < Main.maxNPCs; i += 1)
			{

				if (Main.npc[i].active)
				{
					bool colcheck = Collision.CheckAABBvLineCollision(Main.npc[i].position, new Vector2(Main.npc[i].width, Main.npc[i].height), Main.npc[i].Center, Center)
	&& Collision.CanHit(Main.npc[i].Center, 0, 0, Center, 0, 0);
					if (!Main.npc[i].friendly && !Main.npc[i].townNPC && !Main.npc[i].dontTakeDamage && Main.npc[i].CanBeChasedBy() && colcheck
					&& (Main.npc[i].Center - Center2).Length() < maxdist)
					{
						closestnpcs.Add(Main.npc[i]);
					}
				}
			}

			Func<NPC, float> sortbydistance = delegate (NPC npc)
			 {
				 float score = (Center - npc.Center).Length();
				 Point weightedscore = AddedWeight.FirstOrDefault(npcid => npcid.X == npc.whoAmI);
				 score += weightedscore != default ? weightedscore.Y : 0;

				 if (weightedscore != default && weightedscore.Y >= 1000000)
					 score = 1000000;

				 return score;

			 };

			if (closestnpcs.Count < 1)
			{
				return null;
			}
			else
			{
				closestnpcs = closestnpcs.ToArray().OrderBy(sortbydistance).ToList();//Closest
				if (AddedWeight != default)
					 closestnpcs.RemoveAll(npc => (int)sortbydistance(npc) == 1000000);//Dups be gone

				return closestnpcs;
			}
		}

	public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
		{
			return listToClone.Select(item => (T)item.Clone()).ToList();
		}
		public static SGAPlayer SGAPly(this Player player)
		{
			return player.GetModPlayer<SGAPlayer>();
		}
		public static SGAprojectile SGAProj(this Projectile proj)
		{
			return proj.GetGlobalProjectile<SGAprojectile>();
		}
		public static bool NoInvasion(NPCSpawnInfo spawnInfo)
		{
			return !spawnInfo.invasion && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface || !Main.dayTime);
		}

		public static float ArrowSpeed(this Player player)
		{
			return player.HasBuff(BuffID.Archery) ? 1.20f : 1f;
		}

		public static void SpawnCoins(Vector2 where, int ammount2, float explodespeed = 0f)
		{
			int ammount = ammount2;
			int[] subanditem;
			while (ammount > 0)
			{
				subanditem = new int[] { ItemID.CopperCoin, 1 };
				if (ammount >= 100)
					subanditem = new int[] { ItemID.SilverCoin, 100 };
				if (ammount >= 10000)
					subanditem = new int[] { ItemID.GoldCoin, 10000 };
				if (ammount >= 1000000)
					subanditem = new int[] { ItemID.PlatinumCoin, 1000000 };

				int item = Item.NewItem(where, Vector2.Zero, subanditem[0]);
				Main.item[item].velocity = new Vector2(Main.rand.NextFloat(-2f, 2f) * explodespeed, Main.rand.NextFloat(-0.75f, 0.75f) * explodespeed);
				ammount -= subanditem[1];


			}

		}

		public static void DrawFishingLine(Vector2 start, Vector2 end, Vector2 Velocity, Vector2 offset, float reel)
		{
			float pPosX = start.X;
			float pPosY = start.Y;

			Vector2 value = new Vector2(pPosX, pPosY);
			float projPosX = end.X - value.X;
			float projPosY = end.Y - value.Y;
			Math.Sqrt((double)(projPosX * projPosX + projPosY * projPosY));
			float rotation2 = (float)Math.Atan2((double)projPosY, (double)projPosX) - 1.57f;
			bool flag2 = true;
			if (projPosX == 0f && projPosY == 0f)
			{
				flag2 = false;
			}
			else
			{
				float projPosXY = (float)Math.Sqrt((double)(projPosX * projPosX + projPosY * projPosY));
				projPosXY = 12f / projPosXY;
				projPosX *= projPosXY;
				projPosY *= projPosXY;
				value.X -= projPosX;
				value.Y -= projPosY;
				projPosX = end.X - value.X;
				projPosY = end.Y - value.Y;
			}
			while (flag2)
			{
				float num = 12f;
				float num2 = (float)Math.Sqrt((double)(projPosX * projPosX + projPosY * projPosY));
				float num3 = num2;
				if (float.IsNaN(num2) || float.IsNaN(num3))
				{
					flag2 = false;
				}
				else
				{
					if (num2 < 20f)
					{
						num = num2 - 8f;
						flag2 = false;
					}
					num2 = 12f / num2;
					projPosX *= num2;
					projPosY *= num2;
					value.X += projPosX;
					value.Y += projPosY;
					projPosX = end.X - value.X;
					projPosY = end.Y - value.Y;
					if (num3 > 12f)
					{
						float num4 = 0.3f;
						float num5 = Math.Abs(Velocity.X) + Math.Abs(Velocity.Y);
						if (num5 > 16f)
						{
							num5 = 16f;
						}
						num5 = 1f - num5 / 16f;
						num4 *= num5;
						num5 = num3 / 80f;
						if (num5 > 1f)
						{
							num5 = 1f;
						}
						num4 *= num5;
						if (num4 < 0f)
						{
							num4 = 0f;
						}
						num5 = 1f - reel / 100f;
						num4 *= num5;
						if (projPosY > 0f)
						{
							projPosY *= 1f + num4;
							projPosX *= 1f - num4;
						}
						else
						{
							num5 = Math.Abs(Velocity.X) / 3f;
							if (num5 > 1f)
							{
								num5 = 1f;
							}
							num5 -= 0.5f;
							num4 *= num5;
							if (num4 > 0f)
							{
								num4 *= 2f;
							}
							projPosY *= 1f + num4;
							projPosX *= 1f - num4;
						}
					}
					rotation2 = (float)Math.Atan2((double)projPosY, (double)projPosX) - 1.57f;
					Microsoft.Xna.Framework.Color color2 = Lighting.GetColor((int)value.X / 16, (int)(value.Y / 16f), Color.AliceBlue);

					Main.spriteBatch.Draw(Main.fishingLineTexture, new Vector2(value.X - Main.screenPosition.X + (float)Main.fishingLineTexture.Width * 0.5f, value.Y - Main.screenPosition.Y + (float)Main.fishingLineTexture.Height * 0.5f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.fishingLineTexture.Width, (int)num)), color2, rotation2, new Vector2((float)Main.fishingLineTexture.Width * 0.5f, 0f), 1f, SpriteEffects.None, 0f);
				}
			}

		}



		//These next 3 methods came from a cheats forum lol, but hey if I can use them in Terraria... (https://www.unknowncheats.me/forum/battlefield-4-a/143104-absolutely-accurate-aiming-prediction-correction-erros-theory.html)
		private static uint SolveCubic(double[] coeff, ref double[] x)
		{
			/* Adjust coefficients */

			double a1 = coeff[2] / coeff[3];
			double a2 = coeff[1] / coeff[3];
			double a3 = coeff[0] / coeff[3];

			double Q = (a1 * a1 - 3 * a2) / 9;
			double R = (2 * a1 * a1 * a1 - 9 * a1 * a2 + 27 * a3) / 54;
			double Qcubed = Q * Q * Q;
			double d = Qcubed - R * R;

			/* Three real roots */

			if (d >= 0)
			{
				double theta = Math.Acos(R / Math.Sqrt(Qcubed));
				double sqrtQ = Math.Sqrt(Q);

				x[0] = -2 * sqrtQ * Math.Cos(theta / 3) - a1 / 3;
				x[1] = -2 * sqrtQ * Math.Cos((theta + 2 * Math.PI) / 3) - a1 / 3;
				x[2] = -2 * sqrtQ * Math.Cos((theta + 4 * Math.PI) / 3) - a1 / 3;

				return (3);
			}

			/* One real root */

			else
			{
				double e = Math.Pow(Math.Sqrt(-d) + Math.Abs(R), 1.0 / 3.0);

				if (R > 0)
				{
					e = -e;
				}

				x[0] = (e + Q / e) - a1 / 3.0;

				return (1);
			}
		}

		public static uint SolveQuartic(double a, double b, double c, double d, double e, ref double[] x)
		{
			/* Adjust coefficients */

			double a1 = d / e;
			double a2 = c / e;
			double a3 = b / e;
			double a4 = a / e;

			/* Reduce to solving cubic equation */

			double q = a2 - a1 * a1 * 3 / 8;
			double r = a3 - a1 * a2 / 2 + a1 * a1 * a1 / 8;
			double s = a4 - a1 * a3 / 4 + a1 * a1 * a2 / 16 - 3 * a1 * a1 * a1 * a1 / 256;

			double[] coeff_cubic = new double[4];
			double[] roots_cubic = new double[3];
			double positive_root = 0;

			coeff_cubic[3] = 1;
			coeff_cubic[2] = q / 2;
			coeff_cubic[1] = (q * q - 4 * s) / 16;
			coeff_cubic[0] = -r * r / 64;

			uint nRoots = SolveCubic(coeff_cubic, ref roots_cubic);

			for (int i = 0; i < nRoots; i++)
			{
				if (roots_cubic[i] > 0)
				{
					positive_root = roots_cubic[i];
				}
			}

			/* Reduce to solving two quadratic equations */

			double k = Math.Sqrt(positive_root);
			double l = 2 * k * k + q / 2 - r / (4 * k);
			double m = 2 * k * k + q / 2 + r / (4 * k);

			nRoots = 0;

			if (k * k - l > 0)
			{
				x[nRoots + 0] = -k - Math.Sqrt(k * k - l) - a1 / 4;
				x[nRoots + 1] = -k + Math.Sqrt(k * k - l) - a1 / 4;

				nRoots += 2;
			}

			if (k * k - m > 0)
			{
				x[nRoots + 0] = +k - Math.Sqrt(k * k - m) - a1 / 4;
				x[nRoots + 1] = +k + Math.Sqrt(k * k - m) - a1 / 4;

				nRoots += 2;
			}

			return nRoots;
		}

		public static Vector3 PredictAimingPos(Vector3 position, Vector3 targetPosition, Vector3 targetVelocity, float bulletVelocity, float bulletGravity)
		{
			Vector3 predictedAimingPosition = targetPosition;

			// trans target position relate to local player's view position for simplifying equations
			Vector3 p1 = targetPosition;

			// equations about predict time t
			//
			// unknowns
			// t: predict hit time
			// p: predict hit position at time 't'
			// v0: predict local player's bullet velocity vector which could hit target at at time 't' and position 'p'
			//
			// knowns
			// p1: target player's current position relate to local player's view position
			// v1: target player's velocity
			// |v0|: local player's bullet velocity
			// g: local player's bullet gravity
			//
			// =>
			//
			// vx0^2 + vy0^2 + vz0^2 = |v0|^2
			//
			// px = vx0*t
			// py = vy0*t + 0.5*g*t^2
			// pz = vz0*t
			//
			// px = px1 + vx1*t
			// py = py1 + vy1*t
			// pz = pz1 + vz1*t
			//
			// cause all positions are relate to local player's view position, so there is no p0 in above equations
			//
			// =>
			//
			// vx0 = px1/t + vx1
			// vy0 = py1/t + vy1 - 0.5g*t
			// vz0 = pz1/t + vz1
			//
			// with above three equations and the first equation about v0,  we got the final quartic equation about predict time 't'
			// (0.25*g^2)*(t^4) + (-g*vy1)*(t^3) + (vx1^2+vy1^2+vz1^2 - g*py1 - |v|^2)*(t^2) + 2*(px1*vx1+py1*vy1+pz1*vz1)*(t) + (px1^2+py1^2+pz1^2) = 0
			//
			// let's solve this problem...
			//
			double a = bulletGravity * bulletGravity * 0.25;
			double b = -bulletGravity * targetVelocity.Y;
			double c = targetVelocity.X * targetVelocity.X + targetVelocity.Y * targetVelocity.Y + targetVelocity.Z * targetVelocity.Z - bulletGravity * p1.Y - bulletVelocity * bulletVelocity;
			double d = 2.0 * (p1.X * targetVelocity.X + p1.Y * targetVelocity.Y + p1.Z * targetVelocity.Z);
			double e = p1.X * p1.X + p1.Y * p1.Y + p1.Z * p1.Z;

			// some unix guys will not afraid these two lines
			double[] roots = new double[4];
			uint num_roots = SolveQuartic(a, b, c, d, e, ref roots);

			if (num_roots > 0)
			{
				// find the best predict hit time
				// smallest 't' for guns, largest 't' for something like mortar with beautiful arcs
				double hitTime = 0.0;
				for (int i = 0; i < num_roots; ++i)
				{
					if (roots[i] > 0.0 && (hitTime == 0.0 || roots[i] < hitTime))
						hitTime = roots[i];
				}

				if (hitTime > 0.0)
				{
					// get predict bullet velocity vector at aiming direction
					double hitVelX = p1.X / hitTime + targetVelocity.X;
					double hitVelY = p1.Y / hitTime + targetVelocity.Y - 0.5 * bulletGravity * hitTime;
					double hitVelZ = p1.Z / hitTime + targetVelocity.Z;

					// finally, the predict aiming position in world space
					predictedAimingPosition.X = (float)(hitVelX * hitTime);
					predictedAimingPosition.Y = (float)(hitVelY * hitTime);
					predictedAimingPosition.Z = (float)(hitVelZ * hitTime);
				}
			}

			return predictedAimingPosition;
		}

	}

	public class RippleBoom : ModProjectile
	{
		public float rippleSize = 1f;
		public float rippleCount = 1f;
		public float expandRate = 25f;
		public float opacityrate = 1f;
		public float size = 1f;
		int maxtime = 200;
		public override string Texture
		{
			get
			{
				return "SGAmod/MatrixArrow";
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write((double)rippleSize);
			writer.Write((double)rippleCount);
			writer.Write((double)expandRate);
			writer.Write((double)size);
			writer.Write(maxtime);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			rippleSize = (float)reader.ReadDouble();
			rippleCount = (float)reader.ReadDouble();
			expandRate = (float)reader.ReadDouble();
			size = (float)reader.ReadDouble();
			maxtime = reader.ReadInt32();
		}

		public static void MakeShockwave(Vector2 position2, float rippleSize, float rippleCount, float expandRate, int timeleft = 200, float size = 1f, bool important = false)
		{
			if (!Main.dedServ)
			{
				if (!Filters.Scene["SGAmod:Shockwave"].IsActive() || important)
				{
					int prog = Projectile.NewProjectile(position2, Vector2.Zero, SGAmod.Instance.ProjectileType("RippleBoom"), 0, 0f);
					Projectile proj = Main.projectile[prog];
					RippleBoom modproj = proj.modProjectile as RippleBoom;
					modproj.rippleSize = rippleSize;
					modproj.rippleCount = rippleCount;
					modproj.expandRate = expandRate;
					modproj.size = size;
					proj.timeLeft = timeleft - 10;
					modproj.maxtime = timeleft;
					proj.netUpdate = true;
					Filters.Scene.Activate("SGAmod:Shockwave", proj.Center, new object[0]).GetShader().UseColor(rippleCount, rippleSize, expandRate).UseTargetPosition(proj.Center);
				}
			}

		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ripple Boom");
		}

		public override void SetDefaults()
		{
			projectile.width = 4;
			projectile.height = 4;
			projectile.friendly = true;
			projectile.alpha = 0;
			projectile.penetrate = -1;
			projectile.timeLeft = 200;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}

		public override void AI()
		{
			//float progress = (maxtime - (float)projectile.timeLeft);
			float progress = ((maxtime - (float)base.projectile.timeLeft) / 60f) * size;
			Filters.Scene["SGAmod:Shockwave"].GetShader().UseProgress(progress).UseOpacity(100f * ((float)base.projectile.timeLeft / (float)maxtime));
			projectile.localAI[1] += 1f;
		}

		public override void Kill(int timeLeft)
		{
			Filters.Scene["SGAmod:Shockwave"].Deactivate(new object[0]);
		}
	}

	public class ModdedDamage
	{
		public Player player;
		public float damage = 0;
		public int crit = 0;
		public ModdedDamage(Player player, float damage, int crit)
		{
			this.player = player;
			this.damage = damage;
			this.crit = crit;
		}

	}

	public class EnchantmentCraftingMaterial
	{
		public int value = 0;
		public int expertisecost = 0;
		public string text = "";
		public EnchantmentCraftingMaterial(int value, int expertisecost, string text)
		{
			this.value = value;
			this.expertisecost = expertisecost;
			this.text = text;
		}
	}

}