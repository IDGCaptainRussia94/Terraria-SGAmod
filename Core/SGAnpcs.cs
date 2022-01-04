using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using System.Threading.Tasks;
using System.Threading;
using SGAmod.NPCs.Hellion;
using SGAmod.Items;
using SGAmod.Items.Accessories;
using SGAmod.Items.Weapons;
using SGAmod.Items.Weapons.Javelins;
using SGAmod.Items.Consumables;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Idglibrary;
using AAAAUThrowing;
using SGAmod.NPCs.Cratrosity;
using SGAmod.Dimensions;
using SGAmod.HavocGear.Items.Weapons;
using System.Linq;
using SGAmod.Buffs;
using SGAmod.Items.Tools;
using SubworldLibrary;
using Terraria.DataStructures;
using Terraria.Graphics;
using SGAmod.NPCs;
using Microsoft.Xna.Framework.Audio;

namespace SGAmod
{
	public class DamageStack
	{
		public int time;
		public int damage;

		public DamageStack(int damage, int time)
		{
			this.damage = damage;
			this.time = time;
		}
		public bool Update()
		{
			time -= 1;
			return time < 1;
		}
	}
	public partial class SGAnpcs : GlobalNPC
	{

		public override bool InstancePerEntity
		{
			get
			{
				return true;
			}
		}

		public int Snapped = 0;
		public bool onlyOnce = false;
		public bool NinjaSmoked = false;
		public bool Snapfading = false;
		public float truthbetold = 0;
		public float damagemul = 1f;
		public bool DankSlow = false;
		public bool MassiveBleeding = false;
		public bool Napalm = false;
		public bool thermalblaze = false; public bool acidburn = false;
		public bool Gourged = false;
		public bool SunderedDefense = false;
		public bool MoonLightCurse = false;
		public bool DosedInGas = false;
		public bool InfinityWarStormbreaker = false;
		public int InfinityWarStormbreakerint = 0;
		public int RemovedFireImmune = 0;
		public int Combusted = 0;
		public int lastHitByItem = 0;
		public int immunitetolightning = 0;
		public float TimeSlow = 0;
		public bool HellionArmy = false;
		public bool Sodden = false;
		public bool ELS = false;
		public bool marked = false;
		public bool petrified = false;
		public int noMovement = 0;
		public bool lavaBurn = false;
		public bool TimeSlowImmune = false;
		public bool dotImmune = false;
		public float dotResist = 1f;
		public float pierceResist = 1f;
		public float overallResist = 1;
		public int reducedDefense = 0;
		bool fireimmunestate = false;
		bool[] otherimmunesfill = new bool[3];
		public bool Mircotransactions;
		public int hellionTimer = 0;
		public int counter = 0;
		public int impaled = 0;
		public int invertedTime = 0;
		public byte crimsonCatastrophe = 0;
		private int nonStackingImpaled_;
		public int PinkyMinion = 0;
		public int watched = 0;
		public bool NoHit = true;
		public bool treatAsNight = false;
		public static bool dropFork = false;
		public List<DamageStack> damageStacks = new List<DamageStack>();

		public int nonStackingImpaled
		{
			get
			{
				return nonStackingImpaled_;
			}
			set
			{
				nonStackingImpaled_ = Math.Max(value, nonStackingImpaled_);
			}
		}
		internal int IrradiatedAmmount_;
		public int IrradiatedAmmount
		{
			get
			{
				return IrradiatedAmmount_;
			}
			set
			{
				IrradiatedAmmount_ = Math.Max(value, IrradiatedAmmount_);
			}
		}

		public int FindBuffIndex(NPC npc, int type)
		{
			for (int i = 0; i < 5; i++)
			{
				if (npc.buffTime[i] >= 1 && npc.buffType[i] == type)
				{
					return i;
				}
			}
			return -1;
		}

		/*public override void SendExtraAI(NPC npc,BinaryWriter writer)   
		{
		writer.Write(truthbetold);
		}

		public override void ReceiveExtraAI(NPC npc,BinaryReader reader)
		{
		truthbetold=reader.ReadFloat32();
		}*/

		public RenderTarget2D SnappedDrawing;
		public bool drawonce = false;
		public bool OnlyOnce()
		{
			if (!onlyOnce)
			{
				onlyOnce = true;
				return true;
			}

			return false;
		}

		public override void ResetEffects(NPC npc)
		{
			immunitetolightning -= 1;
			MassiveBleeding = false;
			Snapfading = false;
			NinjaSmoked = false;
			thermalblaze = false; acidburn = false;
			Gourged = false;
			DosedInGas = false;
			damagemul = 1f;
			MoonLightCurse = false;
			InfinityWarStormbreaker = false;
			Napalm = false;
			petrified = false;
			noMovement = Math.Max(noMovement - 1, 0);
			watched = Math.Max(watched - 1, 0);
			Sodden = false;
			lavaBurn = false;
			SunderedDefense = false;
			DankSlow = false;
			ELS = false;
			marked = false;
			drawonce = true;
			reducedDefense = 0;
			crimsonCatastrophe = (byte)Math.Max(crimsonCatastrophe - 1, 0);
			if (invertedTime > 0)
				invertedTime -= 1;

			if (Snapped > 0)
			{
				if (Snapped == 2)
				{
					if (!Main.dedServ)
						if (SnappedDrawing != null)
							SnappedDrawing.Dispose();
					npc.active = false;
					return;
				}
				Snapped -= 1;
			}
		}

		public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
		{
			if (Snapped > 0 && Snapped < 5)
				return false;

			if (petrified)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

				ArmorShaderData shader = GameShaders.Armor.GetShaderFromItemId(ItemID.SilverAndBlackDye);
				shader.UseOpacity(1f);
				shader.UseSaturation(1f);
				shader.UseColor(1f, 1f, 1f);
				shader.UseSecondaryColor(0f, 0f, 0f);
				DrawData value9 = new DrawData(TextureManager.Load("Images/Misc/Perlin"), new Vector2(Main.GlobalTime * 6, 0), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle((int)(Main.GlobalTime * 4f), 0, 64, 64)), Microsoft.Xna.Framework.Color.White, Main.GlobalTime * 30f, new Vector2(256f, 256f), 1f, SpriteEffects.None, 0);
				shader.Apply(null, new DrawData?(value9));

			}

			if (npc.HasBuff(BuffID.Invisibility))
			{
				return false;
			}

			if (drawonce)
				return base.PreDraw(npc, spriteBatch, drawColor);

			if (Snapped > 0 && SnappedDrawing != null && !drawonce)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

				var deathShader = GameShaders.Misc["SGAmod:DeathAnimation"];
				deathShader.UseOpacity(((float)Snapped / 200f));
				deathShader.Apply(null);

				spriteBatch.Draw(SnappedDrawing, new Vector2(0, 0), null, Color.White * 1f, 0, new Vector2(0, 0), new Vector2(1f, 1f), SpriteEffects.None, 0f);

				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);

			}
			//if (Snapped > 0 && !drawonce)
			//return false;
			if (Snapped > 0)
			{
				drawColor = Color.Black;
				//npc.Opacity = ((float)Snapped / 200f);
				return false;
			}
			//drawColor = Color.Lerp(drawColor, Color.Transparent, MathHelper.Clamp(((float)Snapped / 200f),0f,1f));
			return base.PreDraw(npc, spriteBatch, drawColor);
		}

		public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
		{
			if (petrified)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
			}
			//blank
		}

		public override bool CheckActive(NPC npc)
		{

			if ((npc.type == NPCID.SkeletronPrime || npc.type == NPCID.TheDestroyer || npc.type == NPCID.Spazmatism || npc.type == NPCID.Retinazer) && NPC.CountNPCS(ModContent.NPCType<TPD>())>0)
            {
				return false;
            }

			if (HellionArmy)
			{
				if (npc.timeLeft < 3)
					npc.StrikeNPCNoInteraction(9999999, 1, 1);
				return false;

			}
			if (PinkyMinion > 0 && NPC.AnyNPCs(ModContent.NPCType<SPinkyTrue>()))
			{
				if (npc.timeLeft < 3)
					npc.StrikeNPCNoInteraction(9999999, 1, 1);
				return false;

			}
			return true;
		}

		public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
		{
			float spawnrate2 = player.GetModPlayer<SGAPlayer>().morespawns;
			spawnRate = (int)(spawnRate / spawnrate2);
			maxSpawns += (int)((spawnrate2 - 1) * 10f);
			if (NPC.AnyNPCs(ModContent.NPCType<SPinkyTrue>()))
			{
				//spawnRate = (int)(spawnRate / 25f);
				//maxSpawns += 20;
			}
		}

		public override void SetDefaults(NPC npc)
		{
			if (SGAmod.overpoweredMod > 0)
			{
				if (!npc.friendly)
				{
					npc.life += (int)(npc.life * (1f + SGAmod.overpoweredMod));
					npc.lifeMax += (int)(npc.lifeMax * (1f + SGAmod.overpoweredMod));
					npc.damage = npc.damage + (int)(1f + SGAmod.overpoweredMod);
				}

			}
		}

		public override void UpdateLifeRegen(NPC npc, ref int damage)
		{
			//if (dotImmune)
			//	goto Endjump;

			if (!npc.townNPC && !npc.friendly && npc.HasBuff(BuffID.Lovestruck))
			{
				int maxdamage = 0;
				foreach (Player player in Main.player.Where(player => player.active && player.SGAPly().intimacy > 0 && npc.Distance(player.MountedCenter) < 600))
				{
					maxdamage = Math.Max(maxdamage, player.lifeRegen * (5 * player.SGAPly().intimacy));
				}
				npc.lifeRegen -= maxdamage;
			}

			if (npc.HasBuff(BuffID.Stinky))
			{
				foreach (Player player in Main.player.Where(player => player.active && player.SGAPly().toxicity > 0 && npc.Distance(player.MountedCenter) < 600))
				{
					foreach (NPC npc2 in Main.npc.Where(npc2 => npc2.active && npc2.Distance(npc.Center) < 300))
					{
						if (Main.rand.Next(0, 500) == 0)
							npc2.AddBuff(BuffID.Stinky, 60 * 5);
					}
				}

			}

			if (Combusted > 0)
			{
				npc.lifeRegen -= 100 + (int)(Math.Pow(npc.lifeMax, 0.5) / 2.5);
				Combusted -= 1;
				if (damage < 50)
					damage = 50;
			}

			if (thermalblaze)
			{
				if (npc.lifeRegen > 0)
				{
					npc.lifeRegen = 0;
				}
				if (damage < 10)
					damage = 10;
				int boost = 0;
				if (npc.HasBuff(BuffID.Oiled))
					boost = 50;
				npc.lifeRegen -= (Combusted > 0 ? 150 : 30) + boost;
			}

			if (acidburn)
			{
				int tier = 2;
				if (npc.HasBuff(ModContent.BuffType<RustBurn>()) && RustBurn.IsInorganic(npc))
					tier = 3;
				npc.lifeRegen -= 20 + Math.Min(tier * 150, npc.defense * tier);
				if (damage < 5)
					damage = 5;
			}

			if (Napalm)
			{
				if (npc.lifeRegen > 0)
				{
					npc.lifeRegen = 0;
				}
				npc.lifeRegen -= 50;
				if (damage < 10)
					damage = 10;
			}

			if (MoonLightCurse)
			{
				if (npc.lifeRegen > 0)
				{
					npc.lifeRegen = -npc.lifeRegen;
				}
				if (damage < 25)
					damage = 25;
				npc.lifeRegen -= 300;
			}

			if (Snapfading)
			{
				if (npc.life > 5000)
				{
					if (npc.lifeRegen > 0)
					{
						npc.lifeRegen = 0;
					}
					if (damage < 500)
						damage = 500;
					npc.lifeRegen -= 15000;
				}
				else
				{
					if (Snapped < 1)
					{
						npc.NPCLoot();
						Snapped = 200;
					}
					else
					{
						npc.lifeRegen = 0;
					}
				}
			}

			if (npc.HasBuff(BuffID.Daybreak))
			{
				if (npc.lifeRegen > 0)
				{
					npc.lifeRegen = 0;
				}
				npc.lifeRegen -= 150;
			}

			if (MassiveBleeding)
			{
				if (npc.lifeRegen > 0)
				{
					npc.lifeRegen = 0;
				}
				npc.lifeRegen -= 40;
				if (damage < 10)
				{
					damage = 10;
				}
			}
			if (!npc.dontTakeDamage)
			{
				if (damageStacks.Count > 0)
				{
					for (int i = 0; i < damageStacks.Count; i += 1)
					{
						impaled += damageStacks[i].damage;
						if (damageStacks[i].Update())
							damageStacks.RemoveAt(i);
					}
				}

				impaled += nonStackingImpaled;
				if (impaled > 0)
				{
					if (npc.lifeRegen > 0) npc.lifeRegen = 0;

					int damageDot = (int)((npc.realLife > 0 ? (impaled * 0.10f) : impaled));

					npc.lifeRegen -= damageDot;
					damage = Math.Max(damageDot / 4, damage);
				}
			}
		Endjump:
			//ResetEffects seems to be called after projectile AI it seems, but this works, for now
			impaled = 0;
			nonStackingImpaled_ = 0;
			nonStackingImpaled = 0;
		}

		public override void DrawEffects(NPC npc, ref Color drawColor)
		{
			if (Snapped > 0 || Snapfading)
			{
				Vector2 position2 = npc.position;
				position2.X -= 2f;
				position2.Y -= 2f;

				if (Main.rand.Next(2) == 0)
				{
					int num52 = Dust.NewDust(position2, npc.width + 4, npc.height + 2, mod.DustType("FadeDust"), 0f, 0f, 50, Color.DarkGreen, ((1.25f - (Snapped / 200f)) / 2f) + (Snapfading ? 0.04f : 0f));
					Dust dust;
					if (Main.rand.Next(2) == 0)
					{
						dust = Main.dust[num52];
						dust.alpha += 25;
					}
					if (Main.rand.Next(2) == 0)
					{
						dust = Main.dust[num52];
						dust.alpha += 25;
					}

					Main.dust[num52].noLight = true;
					dust = Main.dust[num52];
					dust.velocity *= 0.2f;
					Dust dust9 = Main.dust[num52];
					dust9.velocity.Y = dust9.velocity.Y + 0.2f;
					dust = Main.dust[num52];
					dust.velocity += npc.velocity + new Vector2(Math.Sign(-npc.velocity.X), 0f) * (4f + ((Snapped / 200f)) * 2f);
				}

			}

			if (IrradiatedAmmount > 0)
			{
				for (int i = 0; i < Math.Min(12, IrradiatedAmmount / 32); i += 1)
				{
					if (Main.rand.Next(100) < 1)
					{
						int num126 = Dust.NewDust(npc.Center + Main.rand.NextVector2Circular(npc.width, npc.height), 0, 0, 184, 0, 0, 140, new Color(30, 30, 30, 20), 1f);
						Main.dust[num126].noGravity = true;
						Main.dust[num126].velocity = new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-6f, 1f));
					}
				}
				if (counter % 10 == 0)
				{
					for (int num654 = 0; num654 < 1; num654++)
					{
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= (float)(num654 / 10.00);
						int num655 = Dust.NewDust(npc.Center + Main.rand.NextVector2Circular(npc.width, npc.height), 0, 0, ModContent.DustType<Dusts.RadioDust>(), npc.velocity.X + randomcircle.X * 1f, npc.velocity.Y + randomcircle.Y * 1f, 200, Color.Lime, 0.5f);
						Main.dust[num655].noGravity = true;
					}
				}
			}

			if (NinjaSmoked)
			{
				Vector2 position2 = npc.position;
				position2.X -= 2f;
				position2.Y -= 2f;

				if (Main.rand.Next(2) == 0)
				{
					int num52 = Dust.NewDust(position2, npc.width + 4, npc.height + 2, DustID.Smoke, 0f, 0f, 50, default(Color), Main.rand.NextFloat(2f, 4f));
					Dust dust;
					if (Main.rand.Next(2) == 0)
					{
						dust = Main.dust[num52];
						dust.alpha += 25;
					}
					if (Main.rand.Next(2) == 0)
					{
						dust = Main.dust[num52];
						dust.alpha += 25;

						Main.dust[num52].noLight = true;
						dust = Main.dust[num52];
						dust.velocity *= 0.2f;
						Dust dust9 = Main.dust[num52];
						dust9.velocity.Y = dust9.velocity.Y + 0.2f;
						dust = Main.dust[num52];
						dust.velocity += npc.velocity + new Vector2(Math.Sign(npc.velocity.X), npc.velocity.Y / 2f) * -0.5f;
					}
				}

			}
			if (DosedInGas)
			{
				Vector2 position2 = npc.position;
				position2.X -= 2f;
				position2.Y -= 2f;

				if (Main.rand.Next(2) == 0)
				{
					int num52 = Dust.NewDust(position2, npc.width + 4, npc.height + 2, 211, 0f, 0f, 50, Color.DarkGreen, 0.8f);
					Dust dust;
					if (Main.rand.Next(2) == 0)
					{
						dust = Main.dust[num52];
						dust.alpha += 25;
					}
					if (Main.rand.Next(2) == 0)
					{
						dust = Main.dust[num52];
						dust.alpha += 25;
					}
					Main.dust[num52].noLight = true;
					dust = Main.dust[num52];
					dust.velocity *= 0.2f;
					Dust dust9 = Main.dust[num52];
					dust9.velocity.Y = dust9.velocity.Y + 0.2f;
					dust = Main.dust[num52];
					dust.velocity += npc.velocity;
				}
				else
				{
					int num53 = Dust.NewDust(position2, npc.width + 8, npc.height + 8, 211, 0f, 0f, 50, Color.DarkGreen, 1.1f);
					Dust dust;
					if (Main.rand.Next(2) == 0)
					{
						dust = Main.dust[num53];
						dust.alpha += 25;
					}
					if (Main.rand.Next(2) == 0)
					{
						dust = Main.dust[num53];
						dust.alpha += 25;
					}
					Main.dust[num53].noLight = true;
					Main.dust[num53].noGravity = true;
					dust = Main.dust[num53];
					dust.velocity *= 0.2f;
					Dust dust10 = Main.dust[num53];
					dust10.velocity.Y = dust10.velocity.Y + 1f;
					dust = Main.dust[num53];
					dust.velocity += npc.velocity;
				}
			}
			if (thermalblaze)
			{
				if (Main.rand.Next(5) < 4)
				{
					int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, ModContent.DustType<Dusts.HotDust>(), npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 1f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.8f;
					Main.dust[dust].velocity.Y -= 0.5f;
					if (Main.rand.Next(4) == 0)
					{
						Main.dust[dust].noGravity = false;
						Main.dust[dust].scale *= 0.5f;
					}
				}
				drawColor.R = (byte)(drawColor.R * 0.8f);
				drawColor.G = (byte)(drawColor.R * 0.5f);
				drawColor.B = (byte)(drawColor.R * 0.5f);
				Lighting.AddLight(npc.position, 0.1f, 0.2f, 0.7f);
			}

			if (acidburn)
			{
				if (Main.rand.Next(5) < 4)
				{
					int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, ModContent.DustType<Dusts.AcidDust>(), npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 1f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.8f;
					Main.dust[dust].velocity.Y -= 0.5f;
					if (Main.rand.Next(4) == 0)
					{
						Main.dust[dust].noGravity = false;
						Main.dust[dust].scale *= 0.5f;
					}
				}
				drawColor.R = (byte)(drawColor.R * 0.2f);
				drawColor.G = (byte)(drawColor.G * 0.8f);
				drawColor.B = (byte)(drawColor.B * 0.2f);
			}

			if (Gourged)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				int dust = Dust.NewDust(npc.Center + randomcircle * 8f, 0, 0, 5, -npc.velocity.X * 0.3f, 4f + (npc.velocity.Y * -0.4f), 30, default(Color), 0.85f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].color = Main.hslToRgb(0f, 0.5f, 0.35f);
			}

			if (MassiveBleeding)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y) + randomcircle * 1.2f, npc.width + 4, npc.height + 4, 5, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 30, default(Color), 1.5f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].color = Main.hslToRgb(0f, 0.5f, 0.35f);
			}

			if (InfinityWarStormbreaker || SunderedDefense)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y) + randomcircle * (1.2f * (float)npc.width), npc.width + 4, npc.height + 4, mod.DustType("TornadoDust"), npc.velocity.X * 0.4f, (npc.velocity.Y - 7f) * 0.4f, 30, default(Color) * 1f, 0.5f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].color = Main.hslToRgb(0f, 0.5f, 0.35f);
			}
			bool hasbuff = npc.HasBuff(mod.BuffType("DigiCurse"));
			if (MoonLightCurse || hasbuff)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				int dust = Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y) + randomcircle * (1.2f * (float)npc.width), 0, 0, hasbuff && (Main.rand.Next(0, 1) == 0 || !MoonLightCurse) ? DustID.PinkCrystalShard : DustID.AncientLight, 0, 0, 30, Color.Turquoise, 1.5f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = (new Vector2(npc.velocity.X * 0.4f, (npc.velocity.Y) * 0.4f)) - randomcircle * 8;
				Main.dust[dust].velocity = Main.dust[dust].velocity.RotatedBy(MathHelper.ToRadians(90));
				Main.dust[dust].color = Color.Turquoise;
				drawColor.R = (byte)(drawColor.R * 0.9f);
				drawColor.G = (byte)(drawColor.G * 0.9f);
			}
			if (ELS)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				int dust = Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y) + randomcircle * (1.25f * (float)npc.height), 0, 0, DustID.HealingPlus, 0, 0, 30, Color.DarkOliveGreen, 1.25f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = (new Vector2(npc.velocity.X * 0.7f, npc.velocity.Y * 0.7f)) - randomcircle * 4;
				Main.dust[dust].velocity = Main.dust[dust].velocity.RotatedBy(MathHelper.ToRadians(-45));
				Main.dust[dust].color = Color.DarkOliveGreen;
			}
			if (Sodden)
			{
				drawColor = Color.Lerp(drawColor, Color.LightGoldenrodYellow, 0.75f);

				Vector2 position2 = npc.position;
				position2.X -= 2f;
				position2.Y -= 2f;

				if (Main.rand.Next(2) == 0)
				{
					int num52 = Dust.NewDust(position2, npc.width + 4, npc.height + 2, 75, 0f, 0f, 75, Color.Goldenrod, 0.8f);
					Dust dust;
					if (Main.rand.Next(2) == 0)
					{
						dust = Main.dust[num52];
						dust.alpha += 25;
					}
					if (Main.rand.Next(2) == 0)
					{
						dust = Main.dust[num52];
						dust.alpha += 25;
					}
					Main.dust[num52].noLight = true;
					dust = Main.dust[num52];
					dust.velocity *= 0.2f;
					Dust dust9 = Main.dust[num52];
					dust9.velocity.Y = dust9.velocity.Y + 0.2f;
					dust = Main.dust[num52];
					dust.velocity += npc.velocity;
				}
				else
				{
					int num53 = Dust.NewDust(position2, npc.width + 8, npc.height + 8, 75, 0f, 0f, 75, Color.Goldenrod, 1.1f);
					Dust dust;
					if (Main.rand.Next(2) == 0)
					{
						dust = Main.dust[num53];
						dust.alpha += 25;
					}
					if (Main.rand.Next(2) == 0)
					{
						dust = Main.dust[num53];
						dust.alpha += 25;
					}
					Main.dust[num53].noLight = true;
					Main.dust[num53].noGravity = true;
					dust = Main.dust[num53];
					dust.velocity *= 0.2f;
					Dust dust10 = Main.dust[num53];
					dust10.velocity.Y = dust10.velocity.Y + 1f;
					dust = Main.dust[num53];
					dust.velocity += npc.velocity;
				}
			}

			if (marked)
				drawColor = drawColor.MultiplyRGBA(Color.Red);



		}

		public override bool PreAI(NPC npc)
		{

			CrucibleArenaMaster.UpdatePortal(npc);

			if (PinkyMinion < 1 && NPC.AnyNPCs(ModContent.NPCType<SPinkyTrue>()) && (npc.type == NPCID.BlueSlime || npc.type == NPCID.GreenSlime || npc.type == NPCID.PurpleSlime || npc.type == NPCID.YellowSlime || npc.type == NPCID.RedSlime || npc.type == NPCID.BlackSlime))
			{
				PinkyMinion = 1;
				npc.aiStyle = -1;
			}

			if (petrified)
			{
				npc.velocity.Y += 0.4f;
				npc.velocity.X *= 0.85f;
				return false;
			}
			if (noMovement > 0)
			{
				npc.velocity *= 0.85f;
				return false;
			}

			//Until I add Doom Harbinger back
			/*if ((npc.type == NPCID.CultistDevote || npc.type == NPCID.CultistArcherBlue || npc.type == NPCID.CultistTablet) && (SGAWorld.downedHarbinger == false && Main.netMode < 1))
		{
			npc.active = false;
			return false;
		}
		else
		{
			//npc.dontTakeDamage=false;
		}

		*/

			return true;
		}
		public override void AI(NPC npc)
		{

			if (thermalblaze)
			{
				if (npc.HasBuff(BuffID.Oiled))
				{
					npc.AddBuff(BuffID.OnFire, 60 * 5);
				}
			}
			if (Napalm)
				npc.onFire = true;

			if (InfinityWarStormbreaker)
				InfinityWarStormbreakerint = 10;
			if (InfinityWarStormbreakerint > 0)
				InfinityWarStormbreakerint -= 1;

			fireimmunestate = npc.buffImmune[BuffID.OnFire];
			for (int i = 0; i < SGAmod.otherimmunes.Length; i++)
			{
				otherimmunesfill[i] = npc.buffImmune[SGAmod.otherimmunes[i]];
			}

			if (Combusted < 5 && !DosedInGas)
				npc.buffImmune[BuffID.OnFire] = fireimmunestate;
			for (int i = 0; i < SGAmod.otherimmunes.Length; i++) { npc.buffImmune[SGAmod.otherimmunes[i]] = otherimmunesfill[i]; }
			if (DosedInGas)
			{
				npc.buffImmune[BuffID.OnFire] = false;
				for (int i = 0; i < SGAmod.otherimmunes.Length; i++) { npc.buffImmune[SGAmod.otherimmunes[i]] = false; }
			}
		}

		public override void PostAI(NPC npc)
		{
			counter++;
			if (Mircotransactions)
			{
				if (Main.netMode != 2)
				{
					if (!npc.dontTakeDamage && counter % 30 == 0 && npc.value >= Item.buyPrice(0, 0, 25, 0))
					{
						npc.value -= Item.buyPrice(0, 0, 25, 0);
						npc.StrikeNPC(Item.buyPrice(0, 0, 1, 0), 0, 1, false);
						Main.PlaySound(SoundID.Coins, (int)npc.Center.X, (int)npc.Center.Y, 0, 1f, 0.25f);
						Item.NewItem(npc.position, new Vector2(npc.width, npc.height),ItemID.SilverCoin ,Main.rand.Next(25,36), noGrabDelay: true);
					}
				}
			}

			if (ELS)
			{
				for (int i = 0; i < npc.buffType.Length; i += 1)
				{
					if (npc.buffType[i] != mod.BuffType("EverlastingSuffering") && (npc.buffTime[i] > 10))
					{
						npc.buffTime[i] += 1;
						if (npc.buffTime[i] < 15)
							npc.buffTime[i] = 15;
					}

				}

				npc.lifeRegen = 0;
				int damage = 1;
				UpdateLifeRegen(npc, ref damage);
				if (npc.lifeRegen < 0)
					npc.lifeRegen = (int)(npc.lifeRegen * 2.5f);
			}

			if (HellionArmy)
			{
				Hellion hell = Hellion.GetHellion();
				if (hell != null)
				{
					Player ply = Main.player[hell.npc.target];
					hellionTimer += hellionTimer >= 0 ? 1 : 1;
					if (hellionTimer % 5 == 0)
					{
						if (!Collision.CanHitLine(npc.Center, 1, 1, Main.player[hell.npc.target].Center, 1, 1) && npc.aiStyle < 15 && npc.aiStyle > -1)
						{
							hellionTimer += 200;
						}
					}
					if (hellionTimer > 100 + (hell.phase * 200))
						hellionTimer = -300;

					if (hellionTimer < 0)
					{
						Vector2 dists = new Vector2(5 + (float)Math.Sin(MathHelper.ToRadians((hellionTimer + npc.whoAmI * 9f) / 100f)) * 20f, (float)Math.Cos(npc.whoAmI / 4f));
						hellionTimer += 1;
						Vector2 dists2 = (ply.Center + new Vector2(npc.spriteDirection * (dists.X * -64f), -64 + dists.Y * 64f));
						npc.position.X += (dists2.X - npc.Center.X) / 200f;
						npc.position.Y += (dists2.Y - npc.Center.Y) / 100f;

						npc.position.X += (ply.Center.X + (float)Math.Sin((hell.npc.ai[0] + (npc.whoAmI * 9f)) * 400f) - npc.Center.X) / 150f;
						npc.velocity.Y = (float)Math.Sin(hellionTimer / 90f) * 5f;

						int typr = mod.DustType("TornadoDust");

						Dust dust = Dust.NewDustPerfect(npc.position + new Vector2(Main.rand.Next(npc.width), Main.rand.Next(npc.height)), typr);
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
						Vector2 normvel = npc.velocity;
						normvel.Normalize(); normvel *= 2f;
						Color Rainbow = Main.hslToRgb(((Main.GlobalTime + npc.whoAmI) / 3.63f) % 1f, 0.81f, 0.5f);
						dust.color = Rainbow;
						dust.scale = 1f;
						dust.velocity = ((randomcircle / 1f) + (-normvel)) - npc.velocity;
						dust.noGravity = true;

					}
				}


				if (hell != null)
					if (hell.army.Count < 1)
						npc.StrikeNPCNoInteraction(9999999, 1, 1);
				if (hell == null)
					npc.StrikeNPCNoInteraction(9999999, 1, 1);

			}

			if (PinkyMinion > 0 && !NPC.AnyNPCs(ModContent.NPCType<SPinkyTrue>()))
			{
				npc.StrikeNPCNoInteraction(9999999, 1, 1);
			}


			if (SunderedDefense)
			{
				for (int i = 0; i < Main.maxPlayers; i += 1)
				{
					if (npc.immune[i] > 0)
						npc.immune[i] = Math.Max(npc.immune[i] - 3, 0);
				}
			}
			if (TimeSlow > 0 && !npc.IsDummy() && !TimeSlowImmune)
			{
				npc.position -= npc.velocity - (npc.velocity / (1 + TimeSlow));
			}
			TimeSlow = 0;
		}

		public override void SetupShop(int type, Chest shop, ref int nextSlot)
		{
			Player player = Main.player[Main.myPlayer];
			switch (type)
			{
				case NPCID.Merchant:

					if (WorldGen.crimson)
					{
						shop.item[nextSlot].SetDefaults(ItemID.Leather);
						shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 25, 0);
						nextSlot++;
					}
					if (SGAWorld.downedCratrosity)
					{
						shop.item[nextSlot].SetDefaults(mod.ItemType("TerrariacoCrateKey"));
						shop.item[nextSlot].shopCustomPrice = Item.buyPrice(20, 0, 0, 0);
						nextSlot++;
					}
					if (Main.netMode != NetmodeID.SinglePlayer && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
					{
						shop.item[nextSlot].SetDefaults(mod.ItemType("EntropyTransmuter"));
						shop.item[nextSlot].shopCustomPrice = Item.buyPrice(1, 0, 0, 0);
						nextSlot++;
					}

					if (true)
					{
						shop.item[nextSlot].SetDefaults(mod.ItemType("PremiumUpgrade"));
						nextSlot++;
					}
					if (SGAWorld.downedSPinky && SGAWorld.downedSPinky && SGAWorld.downedCratrosityPML && SGAWorld.downedWraiths > 3 && Main.netMode > 0)
					{
						shop.item[nextSlot].SetDefaults(mod.ItemType("AncientFabricItem"));
						shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 1, 0, 0);
						nextSlot++;
						shop.item[nextSlot].SetDefaults(mod.ItemType("VibraniumCrystal"));
						shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 1, 0, 0);
						nextSlot++;
					}
					break;

				case NPCID.ArmsDealer:

					if (!SGAWorld.WorldIsTin || Main.hardMode)
					{
						shop.item[nextSlot].SetDefaults(mod.ItemType("GunBarrelParts"));
						shop.item[nextSlot].shopCustomPrice = Main.hardMode ? Item.buyPrice(0, 7, 50, 0) : Item.buyPrice(0, 2, 50, 0);
						nextSlot++;
					}
					if (SGAWorld.WorldIsTin || Main.hardMode)
					{
						shop.item[nextSlot].SetDefaults(mod.ItemType("SecondCylinder"));
						shop.item[nextSlot].shopCustomPrice = Main.hardMode ? Item.buyPrice(0, 7, 50, 0) : Item.buyPrice(0, 2, 50, 0);
						nextSlot++;
					}

					if (player.CountItem(mod.ItemType("SnappyShark")) > 0)
					{
						shop.item[nextSlot].SetDefaults(mod.ItemType("SharkTooth"));
						nextSlot++;
					}
					if (player.CountItem(mod.ItemType("StarfishBlaster")) + player.CountItem(mod.ItemType("Starfishburster")) > 0)
					{
						shop.item[nextSlot].SetDefaults(ItemID.Starfish);
						shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 2, 0);
						nextSlot++;
					}
					break;
				case NPCID.Pirate:

					if (NPC.CountNPCS(NPCID.PartyGirl) > 0)
					{
						shop.item[nextSlot].SetDefaults(ItemID.ExplosiveBunny);
						shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 5, 0);
						nextSlot++;
					}
					break;

				case NPCID.Mechanic:

					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Spanner>());
					nextSlot++;
					break;

				case NPCID.Demolitionist:
					if (NPC.CountNPCS(NPCID.GoblinTinkerer) > 0)
					{
						shop.item[nextSlot].SetDefaults(ModContent.ItemType<ExplosionBoomerang>());
						nextSlot++;
					}
					break;


				case NPCID.Wizard:

					shop.item[nextSlot].SetDefaults(ModContent.ItemType<EnchantedBubble>());
					shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 50, 0);
					nextSlot++;
					break;

				case NPCID.TravellingMerchant:

					if (Main.LocalPlayer.ZoneUnderworldHeight)
					{
						shop.item[nextSlot].SetDefaults(ModContent.ItemType<MagicMusicBox>());
						shop.item[nextSlot].shopCustomPrice = Item.buyPrice(1, 0, 0, 0);
						nextSlot++;
					}
					break;

				case NPCID.SkeletonMerchant:

					shop.item[nextSlot].SetDefaults(ModContent.ItemType<RustedBulwark>());
					shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 2, 50, 0);
					nextSlot++;

					shop.item[nextSlot].SetDefaults(ModContent.ItemType<SnakeEyes>());
					shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 5, 0, 0);
					nextSlot++;

					shop.item[nextSlot].SetDefaults(ModContent.ItemType<RussianRoulette>());
					shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 5, 0, 0);
					nextSlot++;


					if (NPC.downedBoss2)
					{
						shop.item[nextSlot].SetDefaults(ModContent.ItemType<Jabbawacky>());
						shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 5, 0, 0);
						nextSlot++;
					}

					if (Main.LocalPlayer.inventory.Any(testitem => testitem.modItem != null && testitem.modItem is IShieldItem))
					{
						shop.item[nextSlot].SetDefaults(ModContent.ItemType<EnchantedShieldPolish>());
						shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 5, 0, 0);
						nextSlot++;
					}
					break;
			}

			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;

			if (sgaplayer.intimacy > 0)
			{
				shop.item[nextSlot].SetDefaults(ItemID.LovePotion);
				shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 25, 0);
				nextSlot++;
			}

			if (sgaplayer.toxicity > 0)
			{
				shop.item[nextSlot].SetDefaults(ItemID.StinkPotion);
				shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 25, 0);
				nextSlot++;
			}


			if (sgaplayer.MidasIdol > 0)
			{

				foreach (Item item in shop.item)
				{
					item.value = (int)(item.value * 0.8);
				}
			}

			if (sgaplayer.HasGucciGauntlet())
			{
				if (player.ownedLargeGems[6])
				{
					foreach (Item item in shop.item)
					{
						if (item.shopCustomPrice != null)
							item.value = (int)(item.shopCustomPrice * 0.85);
					}
				}
			}

			if (sgaplayer.intimacy > 0 && Main.npc[player.talkNPC].HasBuff(BuffID.Lovestruck))
			{
				foreach (Item item in shop.item)
				{
					item.value = (int)(item.value * 0.80);
				}
			}

			if (sgaplayer.toxicity > 0 && player.HasBuff(BuffID.Stinky))
			{
				foreach (Item item in shop.item)
				{
					item.value = (int)(item.value * 1.25);
				}
			}

			if (sgaplayer.greedyperc > 0)
			{

				foreach (Item item2 in shop.item)
				{
					Main.NewText(sgaplayer.greedyperc);
					item2.value = (int)(item2.value * (1f - (Math.Min(0.95f, sgaplayer.greedyperc * 1f))));
				}
			}
		}

		public override void SetupTravelShop(int[] shop, ref int nextSlot)
		{
			if (Main.rand.Next(0, 3) == 1 && Main.hardMode)
			{
				shop[nextSlot] = mod.ItemType(Main.rand.NextBool() ? "ShinobiShiv" : "PeacekeepersDuster");
				nextSlot++;
			}

			bool rocket = false;
			if (Main.rand.Next(0, 3) == 0)
			{
				shop[nextSlot] = ModContent.ItemType<Items.Mounts.GiantIceCube>();
				nextSlot++;
			}
			if (Main.rand.Next(0, 3) == 0 && Main.hardMode)
			{
				int[] weapon = { ModContent.ItemType<SeraphimShard>(), ModContent.ItemType<SoldierRocketLauncher>(), ModContent.ItemType<Gunarang>() };
				int intex = weapon[Main.rand.Next(weapon.Length)];
				shop[nextSlot] = weapon[Main.rand.Next(weapon.Length)];
				if (shop[nextSlot] == ModContent.ItemType<SoldierRocketLauncher>())
					rocket = true;
				nextSlot++;

			}
			if (Main.LocalPlayer.HasItem(ModContent.ItemType<SoldierRocketLauncher>()))
				rocket = true;

			if (rocket)
			{
				shop[nextSlot] = ItemID.RocketI;
				nextSlot++;
			}

			if (Main.rand.Next(0, 3) == 1 && NPC.downedBoss2)
			{
				shop[nextSlot] = ModContent.ItemType<DynastyJavelin>();
				nextSlot++;
			}
		}

		public override bool PreNPCLoot(NPC npc)
		{

			//if (onlyOnce)
			//{
			if (Main.expertMode)
			{
				if (!npc.boss && npc.lifeMax > 25 && npc.lifeMax < 10000 && !npc.dontTakeDamage && npc.type != ModContent.NPCType<PrismSpirit>() && !npc.SpawnedFromStatue && NPC.AnyNPCs(ModContent.NPCType<PrismBanshee>()))
				{
					List<NPC> banshee = Main.npc.Where(testnpc => testnpc.active && testnpc.type == ModContent.NPCType<PrismBanshee>()).ToList();
					if (banshee.Count > 0)
					{
						banshee = banshee.OrderBy(ordertest => ordertest.DistanceSQ(npc.Center)).ToList();
						NPC myguy = banshee[0];
						if (myguy.DistanceSQ(npc.Center) < 2500 * 2500)
						{
							int npcnew = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<PrismSpirit>());
							Main.npc[npcnew].ai[1] = myguy.whoAmI;
							Main.npc[npcnew].ai[2] = npc.type;
							Main.npc[npcnew].netUpdate = true;
						}
					}

				}
			}


			if (HellionArmy)
			{
				if (Hellion.GetHellion() != null)
				{
					if (Hellion.GetHellion().armyspawned > 5)
						Hellion.GetHellion().armyspawned -= 2;
				}
			}


			if (npc.type == NPCID.CultistBoss && SGAWorld.downedWraiths < 3)
			{
				NPCLoader.blockLoot.Add(ItemID.LunarCraftingStation);
				if (Main.netMode != 1)
					SGAWorld.stolecrafting = -500;
			}

			if (npc.type == NPCID.MoonLordCore && Main.rand.Next(10) < 1 && !Main.expertMode)
			{
				dropFork = true;
				NPCLoader.blockLoot.Add(ItemID.LunarFlareBook);
				NPCLoader.blockLoot.Add(ItemID.FireworksLauncher);
				NPCLoader.blockLoot.Add(ItemID.LastPrism);
				NPCLoader.blockLoot.Add(ItemID.RainbowCrystalStaff);
				NPCLoader.blockLoot.Add(ItemID.MoonlordTurretStaff);
				NPCLoader.blockLoot.Add(ItemID.SDMG);
				NPCLoader.blockLoot.Add(ItemID.StarWrath);
				NPCLoader.blockLoot.Add(ItemID.Meowmere);
				NPCLoader.blockLoot.Add(ItemID.Terrarian);
			}

			return true;
		}

		public override void NPCLoot(NPC npc)
		{

			for (int playerid = 0; playerid < Main.maxPlayers; playerid += 1)
			{
				Player ply = Main.player[playerid];
				if (ply.active)
				{
					SGAPlayer sgaply = ply.SGAPly();

					if (!Main.dedServ)
					{
						sgaply.DoExpertiseCheck(npc);
					}
					else
					{
						if ((ply.Center - npc.Center).Length() < 2000)
						{
							ModPacket packet = mod.GetPacket();
							packet.Write((ushort)MessageType.GrantExpertise);
							packet.Write(npc.type);
							packet.Send(ply.whoAmI);
						}
					}

					if (npc.Distance(ply.Center) < 1200)
					{

						if (sgaply.tf2emblemLevel > 0)
						{
							if (Main.netMode != NetmodeID.MultiplayerClient)
								TF2Emblem.AwardXpToPlayer(ply, (int)npc.value);

							if (Main.dedServ)
							{
								ModPacket packet = mod.GetPacket();
								packet.Write((ushort)MessageType.GrantTf2EmblemXp);
								packet.Write((int)npc.value);
								packet.Send(ply.whoAmI);
							}
						}
					}

					if (ply.HasItem(mod.ItemType("EntropyTransmuter")))
					{
						if (Main.netMode != NetmodeID.MultiplayerClient)
							ply.GetModPlayer<SGAPlayer>().AddEntropy(npc.lifeMax);

						if (Main.dedServ)
						{
							ModPacket packet = mod.GetPacket();
							packet.Write((ushort)MessageType.GrantEntrophite);
							packet.Write(npc.lifeMax);
							packet.Send(ply.whoAmI);
						}
					}

				}
			}

			IrradiatedExplosion(npc, IrradiatedAmmount);

			if (npc.boss)
			{
				Achivements.SGAAchivements.UnlockAchivement("Offender", Main.LocalPlayer);
			}

			if (SGAWorld.tf2quest == 2)
			{
				SGAWorld.questvars[0] += 1;
			}

			if (npc.modNPC != null)
			{
				if (npc.modNPC is ISGABoss interfacenpc)
				{
					if (interfacenpc.Chance())
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType(interfacenpc.Trophy()));
					if (interfacenpc.PetChance() && Main.expertMode) //Change to Master Mode in 1.4
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType(interfacenpc.MasterPet()));
				}
			}

			if (npc.type == NPCID.MoonLordCore)
			{
				if (Main.rand.Next(20) < 1)
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType < FistOfMoonlord>());
				if (Main.rand.Next(10) < (Main.expertMode ? 2 : 1))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType < SwordofTheBlueMoon>());
				if (dropFork && !Main.expertMode)
				{
					dropFork = false;
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<SoulPincher>());
				}

				if (SGAWorld.downedCratrosity)
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType < SalvagedCrate>());
				if (!Main.expertMode)
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType < EldritchTentacle>(), Main.rand.Next(15, 30));
			}
			if (npc.type == NPCID.Golem && SGAWorld.bossprgressor == 0)
			{
				SGAWorld.bossprgressor = 1;
				if (Main.netMode < 1)
				{
					Main.NewText("The Moon's dark gaze is apon the world.", 25, 25, 80);
				}
			}

			if (npc.type == NPCID.MoonLordCore && SGAWorld.bossprgressor < 2)
			{
				SGAWorld.bossprgressor = 2;
				if (Main.netMode != 2)
				{
					Idglib.Chat("The Underground Hallow's creatures glow brighter...", 200, 90, 80);
					Idglib.Chat("A being from below the folds of reality notices you...", 50, 50, 50);
				}
			}


			//if (!NPC.BusyWithAnyInvasionOfSorts())
			//{
			if (npc.SpawnedFromStatue)
				return;

			if (lastHitByItem == ModContent.ItemType<ForagersBlade>())
			{
				if (npc.HitSound == SoundID.NPCHit1)//Yup... lol
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Leather);
			}
			if (SGAWorld.tf2cratedrops && npc.lifeMax>50)
			{
				int craterates = SGAWorld.downedCratrosity ? (Main.hardMode ? 300 : 1500) : (Main.hardMode ? 30 : 200);

				var setting = SGAConfig.Instance.CrateFieldDropChance;

				if ((Main.rand.Next(0, (int)(craterates * (setting != null ? (201-setting.rate)/25f : 1f))) == 0))
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TerrariacoCrateBase"));
				}
			}
			if (npc.type == NPCID.WyvernHead && NPC.downedGolemBoss && Main.rand.Next(100) < 5)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Tornado"));
			}

			if (npc.type == NPCID.Vulture && Main.rand.Next(100) < 50)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Feather);
			}

			if (npc.type == NPCID.Golem && Main.rand.Next(100) < 20 && !Main.expertMode)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Upheaval"));
			}
			if (npc.type == NPCID.DD2Betsy && !Main.expertMode)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("OmegaSigil"));
			}

			if (npc.type == NPCID.WallofFlesh && Main.rand.Next(100) <= 10 && !Main.expertMode)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Powerjack"));
			}

			if (npc.type == NPCID.GoblinSorcerer && Main.rand.Next(100) <= 5)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ShadeflameStaff"));
			}

			if (npc.type == NPCID.TacticalSkeleton && Main.rand.Next(100) <= 25)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("RiotShield"));
			}

			if (npc.value >= Item.buyPrice(gold: 3) && Main.rand.Next(0, 100) < ((npc.type == NPCID.PirateCaptain || npc.type == NPCID.PirateShip || npc.type == ModContent.NPCType<Cratrosity>() || npc.type == ModContent.NPCType<Cratrogeddon>()) ? 3 : 2))
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SybariteGem"));
			}

			if (Main.rand.Next(100) < 50 && SGAWorld.downedSharkvern && npc.type == NPCID.Shark)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SharkTooth"), 10 + Main.rand.Next(21));
			}

			if (npc.type == NPCID.RedDevil)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FieryShard"), Main.rand.Next(3, 5));
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ConsumeHell"), Main.rand.Next(1, 3));
			}

			if ((npc.type == NPCID.Lavabat || npc.type == NPCID.Hellbat) && Main.rand.Next(4) < 1 && Main.hardMode)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FieryShard"), Main.rand.Next(1, 2));
			}

			if (npc.Center.Y > ((Main.maxTilesY * 0.90f) * 16) && Main.rand.Next(100) < 2 && Main.hardMode)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FieryShard"));
			}

			if (npc.type == NPCID.WallCreeper || npc.type == NPCID.BloodCrawler || npc.type == NPCID.JungleCreeper ||
			npc.type == NPCID.WallCreeperWall || npc.type == NPCID.BloodCrawlerWall || npc.type == NPCID.JungleCreeperWall || npc.type == NPCID.BlackRecluse || npc.type == NPCID.BlackRecluseWall)
				if (Main.rand.Next(0, 2) == 0)
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.RottenEgg);

			if (!NPC.downedMoonlord)
				return;

			if (SGAWorld.downedPrismBanshee>0)
			{

				if ((npc.type == NPCID.EnchantedSword || npc.type == NPCID.IlluminantBat || npc.type == NPCID.IlluminantSlime || npc.type == NPCID.ChaosElemental) && Main.rand.Next(3) <= 1)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("IlluminantEssence"), Main.rand.Next(1, Main.rand.Next(1,3)));
				}


				//OOA support
				if (npc.aiStyle != 107 && npc.aiStyle != 108 && npc.aiStyle != 109 && npc.aiStyle != 110 && npc.aiStyle != 111 && !NPCID.Sets.BelongsToInvasionOldOnesArmy[npc.type])
				{
					if (Main.player[npc.target] != null)
					{
						if (Main.player[npc.target].ZoneHoly && npc.position.Y > Main.rockLayer)
						{
							if (Main.rand.Next(100) <= 1)
							{
								Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("IlluminantEssence"), 1);
							}
						}
					}
				}
			}

		}

		//Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType(dropitems[chances]));

		public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
		{
			OnHit(npc, player, damage, knockback, crit, item, null, false);
		}

		public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
		{
			OnHit(npc, Main.player[projectile.owner], damage, knockback, crit, null, projectile, true);
		}

		/*public override bool StrikeNPC (NPC npc, ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
		{

		defense=(int)(defense*0.0085);
		return true;
		}*/

		public override bool? CanBeHitByProjectile(NPC npc, Projectile projectile)
		{
			if (projectile.type == ProjectileID.CultistBossLightningOrbArc && immunitetolightning > 0)
			{
				return false;
			}
			else
			{
				return base.CanBeHitByProjectile(npc, projectile);
			}

		}

		public override bool SpecialNPCLoot(NPC npc)
		{
			if (NPC.CountNPCS(mod.NPCType("TPD")) > 0)
			{
				if (npc.type == NPCID.SkeletronHead || npc.type == NPCID.Spazmatism || npc.type == NPCID.Retinazer || npc.type == NPCID.TheDestroyer || npc.type == NPCID.TheDestroyerBody || npc.type == NPCID.TheDestroyerTail)
					return false;
			}

			if (npc.type == NPCID.CultistBoss)
			{
				if (NPC.CountNPCS(NPCID.CultistBossClone) >= 6 && npc.SGANPCs().NoHit)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Weapons.Almighty.NuclearOption>());
				}
			}

			if (npc.lastInteraction != 255 && !npc.IsDummy())
			{

				DropRelic(npc);

				Player ply = Main.player[npc.lastInteraction];
				if (NoHit && ply.SGAPly().avariceRing == 2 || (ply.SGAPly().avariceRing == 1 && Main.rand.Next(5) == 0))
				{
					npc.NPCLoot();
				}

				if (SGAWorld.NightmareHardcore > 0)
				{
					npc.value *= (int)(SGAWorld.NightmareHardcore * 1.50);
					if (Main.rand.Next(0, 100) < 20)
						npc.NPCLoot();
				}

			}

			return base.SpecialNPCLoot(npc);
		}

		public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			lastHitByItem = item.type;
			DoModifies(npc,player,null,item,ref damage,ref knockback,ref crit);
		}

		public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			lastHitByItem = -projectile.type;
			Player player = Main.player[projectile.owner];

			if (projectile.friendly && player != null)
			{
				DoModifies(npc, player, projectile, null, ref damage, ref knockback, ref crit);
			}
		}

		public override bool StrikeNPC(NPC npc, ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
		{
			return true;
		}

		public override bool? DrawHealthBar(NPC npc, byte hbPosition, ref float scale, ref Vector2 position)
		{
			if (npc.type == NPCID.MoonLordFreeEye && NPC.CountNPCS(mod.NPCType("DoomHarbinger")) > 0)
				return false;
			return base.DrawHealthBar(npc, hbPosition, ref scale, ref position);
		}

		public override void GetChat(NPC npc, ref string chat)
		{
			switch (npc.type)
			{
				case NPCID.Guide:
					if (Main.rand.Next(0, 3) == 0 && NPC.CountNPCS(mod.NPCType("Dergon")) > 0)
					{
						string[] lines = { "A dragon is the last person I'd expect to move in to be honest.",
						"Draken seems upset over his past, I feel sorry for his past."};
						chat = lines[Main.rand.Next(lines.Length)];
					}
					else
					{
						if (Main.rand.Next(0, 3) == 0 && NPC.CountNPCS(mod.NPCType("Dergon")) < 1)
						{
							chat = "I think I see something flying above, maybe if you clear the area of powerful monsters, it might land...";
							if (Main.rand.Next(0, 2) == 0)
								chat = "What is that up there... It looks like a dragon...";
						}

					}
					break;
				case NPCID.ArmsDealer:
					if (Main.rand.Next(0, 5) == 0)
					{
						chat = "Somewhere along the way I got all these Starfish and Shark Teeth, now if only you could find guns that use them I could sell them to you";
						return;

					}
					if (Main.rand.Next(0, 3) == 0 && NPC.CountNPCS(mod.NPCType("Dergon")) > 0)
					{
						string[] lines = { "I'm sure the dragon is worth a lot on the black market, just need to find the right person",
						"How much do you think he could get for selling the dragon? People would pay well for beasts like him."};
						chat = lines[Main.rand.Next(lines.Length)];
					}
					break;
				case NPCID.PartyGirl:
					if (Main.rand.Next(0, 3) == 0 && NPC.CountNPCS(mod.NPCType("Dergon")) > 0)
					{
						string[] lines = { "I tried coloring Draken pink but he didn't seem to like it, strange.",
						"Working on a way to way a party hat fit on the derg, though I might just need 2! Twice the party!"};
						chat = lines[Main.rand.Next(lines.Length)];
					}
					break;
				case NPCID.Merchant:
					if (Main.rand.Next(0, 3) == 0 && NPC.CountNPCS(mod.NPCType("Dergon")) > 0)
					{
						string[] lines = { "I found it odd Draken was asking me about apples even though you know I don't sell those, don't dragons eat meat?",
						"Those scales on Draken might be worth quite a bit, might peel a few off later when he's sleeping."};
						chat = lines[Main.rand.Next(lines.Length)];
					}
					break;
				case NPCID.TravellingMerchant:
					if (Main.rand.Next(0, 3) == 0 && NPC.CountNPCS(mod.NPCType("Dergon")) > 0)
					{
						string[] lines = { "Oh a tamed dragon! Are you selling it by any chance?",
						"What do you mean the dragon isn't for sale? I'll offer you top dollar for it!"};
						chat = lines[Main.rand.Next(lines.Length)];
					}

					break;			
				case NPCID.TaxCollector:
					if (Main.rand.Next(0, 3) == 0 && NPC.CountNPCS(mod.NPCType("Dergon")) > 0)
					{
						string[] lines = { "I don't expect for one second that scaled lizard is hiding his hoard, tax evasion I say!",
						"I'll find that dragon's hoard sooner or later, he can't keep lying forever."};
						chat = lines[Main.rand.Next(lines.Length)];
					}
					break;


			}

		}

	}


}
