using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using SGAmod.Effects;
using Idglibrary;
using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework.Audio;
using Terraria.DataStructures;

namespace SGAmod.NPCs
{
	public class PrismBanshee : ModNPC
	{
		public int bansheeState = 0;
		public int maxattacktime = 300;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismic Banshee");
			Main.npcFrameCount[npc.type] = 1;
			NPCID.Sets.MustAlwaysDraw[npc.type] = true;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.750f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.5f);
		}

        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(bansheeState);
			writer.Write(maxattacktime);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
			bansheeState = reader.ReadInt32();
			maxattacktime = reader.ReadInt32();
		}

		public void ChangeHit(ref int damage, ref bool crit,int player,Projectile proj)
        {
			if (npc.ai[0] < 900)
			{
				damage = (int)(damage * 0.25f);
				crit = false;
			}
			bool pickedone = false;
			foreach(NPC giveMeAHandWouldYa in Main.npc.Where(myhands => myhands.active && myhands.type == ModContent.NPCType<PrismBansheeHand>() && (int)myhands.ai[1] == npc.whoAmI && (myhands.modNPC as PrismBansheeHand).blockTime<1))
            {
				PrismBansheeHand moddedHand = (giveMeAHandWouldYa.modNPC as PrismBansheeHand);
                bool blockDirection = !(proj != null && Main.rand.Next(2)==0);
				Vector2 angleVect;

				if (blockDirection)
				{
					angleVect = Vector2.Normalize(Main.player[player].MountedCenter - giveMeAHandWouldYa.Center);
				}
				else
				{
					angleVect = Vector2.Normalize(proj.Center - giveMeAHandWouldYa.Center);
				}

				moddedHand.blockTime = 60;
				moddedHand.blockAngle = angleVect.ToRotation();
				giveMeAHandWouldYa.netUpdate = true;
			}
		}

        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
			ChangeHit(ref damage, ref crit,player.whoAmI,null);
		}
        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			ChangeHit(ref damage, ref crit, projectile.owner, projectile);
		}

        public override void SetDefaults()
		{
			npc.lifeMax = 80000;
			npc.defense = 75;
			npc.damage = 0;
			npc.scale = 1f;
			npc.width = 48;
			npc.height = 56;
			animationType = -1;
			npc.aiStyle = -1;
			npc.knockBackResist = 0f;
			for(int i = 0;i < npc.buffImmune.Length; i += 1)
			npc.buffImmune[i] = true;

			npc.npcSlots = 0.1f;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.netAlways = true;
			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = Item.buyPrice(0, 5, 0);
		}
		public override string Texture
		{
			get { return ("Terraria/Projectile_" + ProjectileID.Starfury); }
		}

        public override void NPCLoot()
        {
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AuroraTear"), Main.expertMode ? 2 : 1);
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("IlluminantEssence"), Main.rand.Next(12, Main.expertMode ? 30 : 20));
			SGAWorld.downedPrismBanshee = true;
		}

        public void DoAttacks()
		{
			float statetimer = npc.ai[0] - 900;

			List<NPC> NPCspirits = Main.npc.Where(spirittest => spirittest.active && spirittest.type == ModContent.NPCType<PrismSpirit>() && (int)spirittest.ai[1] == npc.whoAmI).ToList();


			if (bansheeState < 1)
			{
				bansheeState = 1;
				maxattacktime = npc.life < (int)(npc.lifeMax / 2f) ? 500 : 300;

				if (Main.rand.Next(0, 2) == 0 && NPCspirits.Count >= 5)
				{
					bansheeState = 2;
					maxattacktime = 600;
				}
				//Filters.Scene.Activate("SGAmod:Shockwave", proj.Center, new object[0]).GetShader().UseColor(rippleCount, rippleSize, expandRate).UseTargetPosition(proj.Center);
				npc.netUpdate = true;
			}


			if (bansheeState == 2)
			{
				if (!Main.dedServ)
				{
					if (!Filters.Scene["SGAmod:ShockwaveBanshee"].IsActive())
					{
						Main.NewText("work!");
						Filters.Scene.Activate("SGAmod:ShockwaveBanshee", npc.Center, new object[0]).GetShader().UseColor(5f, 2f, 10f).UseTargetPosition(npc.Center);
					}


					float progress = (400 - statetimer) / 2400f;
					if (statetimer >= 400)
						progress = MathHelper.Clamp((statetimer - 400f) / 200f, 0f, 1f);

					float alpha = Math.Min(statetimer, 100f);

					Filters.Scene["SGAmod:ShockwaveBanshee"].GetShader().UseProgress(progress).UseOpacity(alpha).UseColor(statetimer >= 400 ? 25f : 5f, 5f, statetimer >= 400 ? 500f : 500f).UseTargetPosition(npc.Center);

				}
				if (statetimer < 400 && statetimer % 30 == 0)
				{
					//RippleBoom.MakeShockwave(npc.Center, 8f, 1f, 10f, 60, 1f);
					SoundEffectInstance sound = Main.PlaySound(SoundID.Zombie, (int)npc.Center.X, (int)npc.Center.Y, 79);
					if (sound != null)
						sound.Pitch += statetimer / 500f;

					foreach (NPC spirit in NPCspirits)
					{
						if (spirit.ai[0] < 100015)
							spirit.ai[0] = 100000;
					}


				}
				if (statetimer == 400)
				{
					SoundEffectInstance sound = Main.PlaySound(SoundID.Roar, (int)npc.Center.X, (int)npc.Center.Y, 2);
					if (sound != null)
						sound.Pitch += 0.75f;

					foreach(Player player2 in Main.player.Where(testplayer => testplayer.active && Collision.CanHit(testplayer.MountedCenter,1,1,npc.Center,1,1)))
                    {
						player2.Hurt(PlayerDeathReason.ByCustomReason(player2.name+" had their brain popped"), 10000, npc.spriteDirection, Crit: true);
					}

					foreach (NPC spirit in NPCspirits)
					{
						if (spirit.ai[0] > 100100)
						{
							int enemyguy = NPC.NewNPC((int)spirit.Center.X, (int)spirit.Center.Y, (int)spirit.ai[2]);
							Main.npc[enemyguy].SpawnedFromStatue = true;
							Main.npc[enemyguy].netUpdate = true;
							spirit.active = false;
						}

					}

				}

			}

		}

		public override void AI()
		{
			npc.timeLeft = 300;
			npc.localAI[0] += 1;
			npc.ai[0] += 1;
			if (npc.ai[0] == 1)
			{
				HalfVector2 half = new HalfVector2(-128, 32);

				int hand = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<PrismBansheeHand>());
				Main.npc[hand].ai[1] = npc.whoAmI;
				Main.npc[hand].ai[2] = ReLogic.Utilities.ReinterpretCast.UIntAsFloat(half.PackedValue);
				Main.npc[hand].netUpdate = true;

				half = new HalfVector2(128, 32);

				hand = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<PrismBansheeHand>());
				Main.npc[hand].ai[1] = npc.whoAmI;
				Main.npc[hand].ai[2] = ReLogic.Utilities.ReinterpretCast.UIntAsFloat(half.PackedValue);
				Main.npc[hand].netUpdate = true;
			}

			bool underground = true;//(int)((double)((npc.position.Y + (float)npc.height) * 2f / 16f) - Main.worldSurface * 2.0) > 0;

			Player P = Main.player[npc.target];
			if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active || !underground)
			{
				npc.TargetClosest(false);
				P = Main.player[npc.target];
				if (!P.active || P.dead || !Main.dayTime || !underground)
				{
					npc.velocity = new Vector2(npc.velocity.X, npc.velocity.Y + 1f);
				}

			}
			else
			{
				Vector2 gotohere = P.Center + ((npc.localAI[0] / 320f).ToRotationVector2()) * 720f;

				foreach(Player player in Main.player.Where(playertest => playertest.active && playertest.DistanceSQ(npc.Center) < 2400 * 2400))
                {
					player.AddBuff(BuffID.WaterCandle, 2);
				}

				if (npc.ai[0] < 900)
				{
					bansheeState = 0;
					if (!Main.dedServ)
					{
						if (Filters.Scene["SGAmod:ShockwaveBanshee"].IsActive())
							Filters.Scene["SGAmod:ShockwaveBanshee"].Deactivate(new object[0]);
					}
					if ((npc.Center - gotohere).Length() > 400)
					{
						npc.velocity += Vector2.Normalize(gotohere - npc.Center) * 0.40f;
						npc.velocity *= 0.90f;
					}
					else
					{
						//stuff


					}

				}
				else
				{
					npc.velocity *= 0.90f;
					DoAttacks();
					if (npc.ai[0] > 900 + maxattacktime)
					{
						npc.ai[0] = Main.rand.Next(1, 100);
						npc.netUpdate = true;
					}
				}
			}
		}

		public static void DrawPrismCore(SpriteBatch spriteBatch, Color drawColor,Vector2 drawWhere,float rotter,float scale = 1,float scaleup=96)
        {
			Vector2 drawPos = drawWhere - Main.screenPosition;
			Texture2D tex2 = Main.projectileTexture[SGAmod.Instance.ProjectileType("JavelinProj")];
			for (int i = 24; i < scaleup; i += 6)
			{
				float peralpha = MathHelper.Clamp(scaleup-(float)i,0f,6f)/6f;
				for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi * 0.1f)
				{
					float rotAngle = f + ((rotter + (i / 8f)) / ((-i * 2) + 16f)) * (((i - 24) % 12 == 0) ? 1f : -1f);
					Vector2 vecAngle = Vector2.UnitX.RotatedBy(rotAngle)*scale;
					spriteBatch.Draw(tex2, drawPos + vecAngle * i, null, Color.Magenta * peralpha * (1f - (i / 84f)),rotAngle + MathHelper.PiOver2, (tex2.Size() / 2f), scale, SpriteEffects.None, 0f);
				}
			}

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Vector2 drawPos = npc.Center - Main.screenPosition;
			Texture2D texture = SGAmod.PrismBansheeTex;
			Texture2D tex = Main.npcTexture[npc.type];
			//Texture2D tex3 = Main.projectileTexture[ProjectileID.CultistBossIceMist];

			List<NPC> NPCHands = Main.npc.Where(handtest => handtest.active && handtest.type == ModContent.NPCType<PrismBansheeHand>() && (int)handtest.ai[1] == npc.whoAmI).ToList();

			float inrc = Main.GlobalTime / 30f;

			DrawPrismCore(spriteBatch,drawColor,npc.Center,npc.localAI[0],npc.scale);

			float strength = 1f;

			if (NPCHands.Count > 0)
			{
				foreach (NPC hand in NPCHands)
				{
					if (strength > 0)
					{
						Vector2 handoffset = new Vector2(hand.Center.X, hand.position.Y);
						List<Vector2> trailspots = new List<Vector2>();
						Vector2 point1 = Vector2.Lerp(npc.Center, handoffset, 0.5f) + new Vector2(0, 96 + (float)Math.Sin(npc.localAI[0] / 40f) * 30f);
						Vector2 point2 = new HalfVector2() { PackedValue = ReLogic.Utilities.ReinterpretCast.FloatAsUInt(hand.ai[2]) }.ToVector2();

						for (float f = 0; f < 1f; f += 0.02f)
						{
							Vector2 curve = npc.Center;
							Vector2 offset2 = new Vector2(Math.Sign(point2.X) * 72, 64);
							Vector2 offset3 = handoffset + (offset2 * (1f - MathHelper.Clamp((handoffset - (handoffset + offset2)).LengthSquared() / (320 * 320), 0f, 1f)));
							curve = curve.BezierCurve(npc.Center + (Vector2.UnitY * -32).RotatedBy(npc.rotation), point1, offset3, handoffset, f);
							trailspots.Add(curve);
						}


						TrailHelper trail = new TrailHelper("DefaultPass", mod.GetTexture("noise"));
						trail.color = delegate (float percent)
						{
							return Color.Lerp(Main.hslToRgb(((-npc.localAI[0] / 90f) + percent) % 1f, 1f, 0.85f), Color.Magenta, Math.Max((float)Math.Sin(npc.localAI[0] / 35f), 0f));
						};
						trail.projsize = Vector2.Zero;
						trail.coordOffset = new Vector2(0, Main.GlobalTime * -1f);
						trail.trailThickness = 8;
						trail.doFade = false;
						trail.trailThicknessIncrease = 0;
						trail.strength = strength;
						trail.DrawTrail(trailspots, npc.Center);
					}
				}
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			Effect hallowed = SGAmod.HallowedEffect;

			hallowed.Parameters["alpha"].SetValue(1);
			hallowed.Parameters["prismColor"].SetValue(Color.Magenta.ToVector3());
			hallowed.Parameters["prismAlpha"].SetValue(0.85f);
			hallowed.Parameters["overlayTexture"].SetValue(mod.GetTexture("Perlin"));
			hallowed.Parameters["overlayProgress"].SetValue(new Vector3(0, -npc.localAI[0] / 250f, npc.localAI[0] / 150f));
			hallowed.Parameters["overlayAlpha"].SetValue(0.25f);
			hallowed.Parameters["overlayStrength"].SetValue(new Vector3(2f, 0.10f, npc.localAI[0] / 150f));
			hallowed.Parameters["overlayMinAlpha"].SetValue(0f);

			hallowed.CurrentTechnique.Passes["Prism"].Apply();

			spriteBatch.Draw(texture, drawPos, null, Color.White, npc.rotation, new Vector2(texture.Width, texture.Height / 1.20f) / 2f, npc.scale, SpriteEffects.None, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			hallowed.Parameters["alpha"].SetValue(1);
			hallowed.Parameters["prismColor"].SetValue(Color.Magenta.ToVector3());
			hallowed.Parameters["prismAlpha"].SetValue(0.85f);
			hallowed.Parameters["overlayTexture"].SetValue(mod.GetTexture("Perlin"));
			hallowed.Parameters["overlayProgress"].SetValue(new Vector3(0, -npc.localAI[0] / 250f, npc.localAI[0] / 150f));
			hallowed.Parameters["overlayAlpha"].SetValue(0.25f);
			hallowed.Parameters["overlayStrength"].SetValue(new Vector3(2f, 0.10f, npc.localAI[0] / 150f));
			hallowed.Parameters["overlayMinAlpha"].SetValue(0f);

			hallowed.CurrentTechnique.Passes["Prism"].Apply();

			if (NPCHands.Count > 0)
			{
				foreach (NPC hand in NPCHands)
				{
					PrismBansheeHand myhand = hand.modNPC as PrismBansheeHand;
					Texture2D handtex = Main.npcTexture[hand.type];
					spriteBatch.Draw(handtex, new Vector2(hand.Center.X, hand.position.Y) - Main.screenPosition, null, Color.White, hand.rotation, handtex.Size() / 2f, hand.scale, hand.spriteDirection > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				}
			}


			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			hallowed.Parameters["alpha"].SetValue(1);
			hallowed.Parameters["prismColor"].SetValue(Color.Magenta.ToVector3());
			hallowed.Parameters["prismAlpha"].SetValue(0.80f);
			hallowed.Parameters["overlayTexture"].SetValue(mod.GetTexture("Perlin"));
			hallowed.Parameters["overlayProgress"].SetValue(new Vector3(0, -npc.localAI[0] / 150f, npc.localAI[0] / 75f));
			hallowed.Parameters["overlayAlpha"].SetValue(0.25f);
			hallowed.Parameters["overlayStrength"].SetValue(new Vector3(2f, 0.10f, npc.localAI[0] / 60f));
			hallowed.Parameters["overlayMinAlpha"].SetValue(0f);

			hallowed.CurrentTechnique.Passes["Prism"].Apply();

			List<NPC> NPCspirits = Main.npc.Where(spirittest => spirittest.active && spirittest.type == ModContent.NPCType<PrismSpirit>() && (int)spirittest.ai[1] == npc.whoAmI).ToList();

			if (NPCspirits.Count > 0)
			{
				foreach (NPC spirit in NPCspirits)
				{
					Texture2D handtex = Main.npcTexture[spirit.type];
					spriteBatch.Draw(handtex, new Vector2(spirit.Center.X, spirit.Center.Y) - Main.screenPosition, new Rectangle(0, ((int)(spirit.localAI[0] / 30f) % 3) * (handtex.Height / 3), handtex.Width, handtex.Height / 3), Color.White, -spirit.velocity.X * 0.05f, new Vector2(handtex.Width, handtex.Height / 3) / 2f, spirit.scale, spirit.spriteDirection > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				}
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);



			spriteBatch.Draw(tex, drawPos, null, Color.White, npc.rotation, (tex.Size() / 2f), npc.scale, SpriteEffects.None, 0f);

			//spriteBatch.Draw(tex2, drawPos, null, Color.Magenta, npc.rotation, (tex2.Size() / 2f), npc.scale, SpriteEffects.None, 0f);


			return false;
		}


	}

	public class PrismBansheeHand : ModNPC
	{
		int bansheesmoothness = 0;
		public float blockAngle = 0;
		public int blockTime = 0;
		public NPC Owner
		{
			get
			{
				var ret = Main.npc[(int)npc.ai[1]];
				if (ret == null || !ret.active || ret.type != ModContent.NPCType<PrismBanshee>())
					return null;

				return Main.npc[(int)npc.ai[1]];
			}
		}
		public Vector2 ArmOffset => new HalfVector2() { PackedValue = ReLogic.Utilities.ReinterpretCast.FloatAsUInt(npc.ai[2]) }.ToVector2();
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismatic Spikefist");
			Main.npcFrameCount[npc.type] = 1;
		}

		public override bool CheckActive()
		{
			return Owner == null;
		}
        public override bool CheckDead()
        {
			if (Owner != null)
			{
				npc.life = npc.lifeMax;
				return false;
			}
			return true;
        }

        public override void SetDefaults()
		{
			npc.lifeMax = 1000;
			npc.defense = 10000;
			npc.damage = 120;
			npc.width = 48;
			npc.height = 48;
			animationType = -1;
			npc.aiStyle = -1;
			npc.knockBackResist = 0.25f;//0.5f;
			npc.npcSlots = 0.1f;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.netAlways = true;
			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.Opacity = 0f;
		}
		public override string Texture
		{
			get { return ("Terraria/Projectile_" + ProjectileID.CultistBossIceMist); }
		}

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write((double)blockAngle);
			writer.Write(blockTime);
			writer.Write(bansheesmoothness);
			writer.Write(npc.localAI[3]);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			blockAngle = (float)reader.ReadDouble();
			blockTime = reader.ReadInt32();
			bansheesmoothness = reader.ReadInt32();
			npc.localAI[3] = reader.ReadInt32();
		}

		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
			projectile.damage = projectile.damage << 2;
		}

        private void ArmOutState(ref Vector2 gohere)
		{
			gohere = Owner.Center + new Vector2(ArmOffset.X * 2f, 20f);
		}
		public void NormalMovement(Vector2 gohere)
		{
			Vector2 distvector = gohere - npc.Center;

			if (blockTime > 0)
			{
				gohere = Owner.Center + (Vector2.UnitX * Math.Abs(ArmOffset.X)).RotatedBy(blockAngle);
				distvector = (gohere - npc.Center) * 1.5f;

			}

			if (distvector.LengthSquared() > 64)
				npc.velocity += Vector2.Normalize(distvector) * Math.Min(25f, distvector.Length() / 32f) * MathHelper.Clamp(1f - ((npc.localAI[3]) / 100f), 0f, 1f);
			else
				npc.Center = gohere;

		}
		public bool BansheeMovement(Vector2 gohere)
		{
			PrismBanshee bantie = Owner.modNPC as PrismBanshee;
			if (bantie.bansheeState == 1)
			{
				npc.ai[0] -= 1;
				bansheesmoothness += 1;
				gohere = Owner.Center + new Vector2(0, -160f);
				Vector2 distvector = gohere - npc.Center;
				if (distvector.LengthSquared() > 64)
					npc.velocity += Vector2.Normalize(distvector) * Math.Min(25f, distvector.Length() / 64f) * MathHelper.Clamp(bansheesmoothness / 90f, 0f, 1f);
				else
					npc.Center = gohere;

				npc.velocity *= 0.85f;

				if (bansheesmoothness > 100 && bansheesmoothness % 5 == 0)
				{
					int aicounter = ((int)bansheesmoothness - 100) % 150;
					Player P = Main.player[npc.target];
					Vector2 offset = new Vector2(npc.Center.X + (aicounter * Math.Sign(ArmOffset.X) * 16f), P.MountedCenter.Y - 300);
					int proj2 = Projectile.NewProjectile(offset, Vector2.UnitY * (aicounter % 20 == 0 ? 24f : 16f), mod.ProjectileType("PrismShardHinted"), 80, 4, 0);
					(Main.projectile[proj2].modProjectile as CirnoIceShardHinted).CirnoStart = npc.Center;
					Main.projectile[proj2].tileCollide = false;
					Main.projectile[proj2].netUpdate = true;

				}
				return true;
			}
			if (bantie.bansheeState == 2)
			{
				npc.ai[0] -= 1;
				bansheesmoothness += 1;
				gohere = Owner.Center + new Vector2(Math.Sign(ArmOffset.X) * (80f - (Owner.ai[0] - 900) / 6f), -16f);
				if (bansheesmoothness > 400)
					gohere = Owner.Center + new Vector2(Math.Sign(ArmOffset.X) * (160), -64f);

				Vector2 distvector = gohere - npc.Center;
				if (distvector.LengthSquared() > 64)
					npc.velocity += Vector2.Normalize(distvector) * Math.Min(25f, distvector.Length() / 64f) * MathHelper.Clamp(bansheesmoothness / 90f, 0f, 1f);
				else
					npc.Center = gohere;

				npc.velocity *= 0.85f;


				return true;
			}


			return false;

		}
		public bool SlamAttack()
		{
			blockTime -= 1;
			Player P = Main.player[npc.target];
			if (npc.ai[0] >= 500)
			{
				//if ((P.Center.X - npc.Center.X < 0 && ArmOffset.X > 0) || (P.Center.X - npc.Center.X < 0 && ArmOffset.X < 0))
				//{
				if (npc.ai[0] < 501)
				{
					npc.ai[3] = (P.MountedCenter - npc.Center).ToRotation();
					npc.netUpdate = true;

					SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_DarkMageAttack, npc.Center);
					if (sound != null)
					{
						sound.Pitch = -0.25f;
					}

					for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 12f)
					{
						Vector2 offset = f.ToRotationVector2();
						int dust = Dust.NewDust(P.MountedCenter+(offset*32f), 0,0, DustID.PurpleCrystalShard);
						Main.dust[dust].scale = 1.5f;
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity = npc.ai[3].ToRotationVector2()*2f;
					}

					return false;
				}

				npc.velocity += (npc.ai[3].ToRotationVector2()) * Math.Min((npc.ai[0] - 500f) / 10f, 5f);
				Vector2 vextestx = Vector2.Normalize(P.MountedCenter - npc.Center);
				Vector2 vextesty = npc.ai[3].ToRotationVector2();
				if (Vector2.Dot(vextestx, vextesty) < -0.1f)
				{
					npc.ai[0] = Main.rand.Next(-300, 100);
					npc.localAI[3] = 200;
					npc.netUpdate = true;
				}
				return true;
				/*}
                else
                {
					npc.ai[0] -= 100;
					return false;
                }*/
			}
			return false;
		}

		public override void AI()
		{
			npc.localAI[0] += 1;
			npc.ai[0] += 1;
			npc.localAI[3] -= 1;
			if (Owner != null)
			{
				//if (npc.target<0)
				npc.TargetClosest();

				Vector2 gohere = Owner.Center + ArmOffset;

				if (!BansheeMovement(gohere))
				{
					bansheesmoothness = 0;
					if (!SlamAttack())
					{

						if (npc.ai[0] > 400)
							ArmOutState(ref gohere);

						NormalMovement(gohere);
						npc.velocity *= 0.75f + (MathHelper.Clamp(npc.localAI[3] / 200f, 0f, 1f) * 0.25f);
					}
					else
					{
						npc.velocity *= 0.90f;
					}
				}

				npc.spriteDirection = ArmOffset.X < 0 ? -1 : 1;

			}
			else
			{
				npc.StrikeNPCNoInteraction(10000, 0, 0);
			}

		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.WitheredArmor, 60 * 10);
			target.AddBuff(BuffID.WitheredWeapon, 60 * 10);
		}
	}

	public class PrismSpirit : ModNPC
	{
		public NPC Owner
		{
			get
			{
				var ret = Main.npc[(int)npc.ai[1]];
				if (ret == null || !ret.active || ret.type != ModContent.NPCType<PrismBanshee>())
					return null;

				return Main.npc[(int)npc.ai[1]];
			}
		}
		public Vector2 flyTo = default;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismic Spirit");
			Main.npcFrameCount[npc.type] = 1;
		}
		public override void SetDefaults()
		{
			npc.lifeMax = 4000;
			npc.defense = 0;
			npc.damage = 0;
			npc.width = 32;
			npc.height = 32;
			animationType = -1;
			npc.aiStyle = -1;
			npc.knockBackResist = 0.5f;
			npc.npcSlots = 0.1f;
			npc.noGravity = true;
			npc.value = Item.buyPrice(0, 1);
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit36;
			npc.DeathSound = SoundID.NPCDeath39;
			npc.Opacity = 0f;
		}
		public override string Texture
		{
			get { return ("Terraria/NPC_" + NPCID.DungeonSpirit); }
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.WriteVector2(flyTo);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			flyTo = reader.ReadVector2();
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			return false;
		}

        public override void NPCLoot()
        {
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("IlluminantEssence"), 1);
		}

        private void FindLocation(Player target, int tries = 10)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
				return;

			for (int i = 0; i < tries; i += 1)
			{
				float distance = Main.rand.NextFloat(200, 500 + i * 3);
				float angle = Main.rand.NextFloat(MathHelper.TwoPi);

				Vector2 checkloc = target.Center;
				Vector2 checkFlyLoc = checkloc + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * distance;
				if (Collision.CanHit(checkloc, 1, 1, checkFlyLoc, 1, 1))
				{
                    flyTo = checkFlyLoc;
					npc.netUpdate = true;
					break;
				}

			}
		}

		public override bool CheckActive()
		{
			return Owner == null;
		}

		public override void AI()
		{
			npc.localAI[0] += 1;
			npc.ai[0] += 1;
			if (npc.ai[0] == 100000)
				npc.ai[0] = 1;

			Lighting.AddLight(npc.Center, Color.DarkMagenta.ToVector3() * 1f);

            if (npc.ai[0] == 100005)
            {
                FindLocation(Main.player[npc.target], 2);
                if (flyTo == default)
                {
                    npc.ai[0] = Main.rand.Next(1, 10000);
                }
                npc.netUpdate = true;
            }

			if (Main.rand.Next(0, 2) == 1)
			{
				int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.PurpleCrystalShard);
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = npc.velocity * (float)(Main.rand.NextFloat(0.1f, 0.25f));
			}


			if (Owner != null)
			{
				//if (npc.target<0)
				npc.TargetClosest();

				NoiseGenerator noisy = new NoiseGenerator(npc.whoAmI);

				float offsetterangle = (((npc.ai[0] / 100f) + (float)noisy.Noise(9000, -9000) * MathHelper.TwoPi) + (float)noisy.Noise((int)npc.ai[0], 0));
				Vector2 gohere = Owner.Center + offsetterangle.ToRotationVector2() * (200f + ((float)noisy.Noise((int)-npc.ai[0], 1000) * 320f));
				if (npc.ai[0] >= 100005)
					gohere = flyTo;

				npc.velocity += Vector2.Normalize(gohere - npc.Center) * 0.2f;
				npc.velocity *= 0.99f;
				if (npc.velocity.LengthSquared() > 8 * 8)
					npc.velocity *= 0.98f;
				npc.rotation = npc.velocity.ToRotation() + MathHelper.PiOver2;

			}
			else
			{
				npc.StrikeNPCNoInteraction(10000, 0, 0);
			}

		}

	}
	public class PrismShardHinted : CirnoIceShardHinted
	{
		float strength => Math.Min(1f - (projectile.localAI[1] / 110f), 1f);
		public static bool ApplyPrismOnce = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismic Shard");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			speedIncrease = 60f;
			oldPos = new Vector2[15];
			projectile.tileCollide = false;
			projectile.timeLeft = 500;
		}

		public override void Moving()
		{
			Lighting.AddLight(projectile.Center, Color.DarkMagenta.ToVector3() * 1f);
			if (Main.rand.Next(0, 2) == 1)
			{
				int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.PurpleCrystalShard);
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = projectile.velocity * (float)(Main.rand.NextFloat(0.1f, 0.25f));
			}
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			PrismShardHinted hinted = this;

			for (int k = hinted.oldPos.Length - 1; k > 0; k--)
			{
				if (hinted.oldPos[k] == default)
					hinted.oldPos[k] = hinted.VectorEffect;
			}

			if (hinted.strength > 0)
			{
				TrailHelper trail = new TrailHelper("DefaultPass", SGAmod.Instance.GetTexture("noise"));
				trail.color = delegate (float percent)
				{
					return Color.Magenta;
				};
				trail.projsize = hinted.projectile.Hitbox.Size() / 2f;
				trail.coordOffset = new Vector2(0, Main.GlobalTime * -1f);
				trail.trailThickness = 4;
				trail.trailThicknessIncrease = 6;
				trail.capsize = new Vector2(4f, 0f);
				trail.strength = hinted.strength;
				trail.DrawTrail(hinted.oldPos.ToList(), hinted.projectile.Center);
			}



			if (hinted.projectile.Opacity > 0)
			{
			if (hinted.projectile.localAI[0] < 100)
				hinted.projectile.localAI[0] = 100 + Main.rand.Next(0, 3);

			Texture2D tex = Main.extraTexture[35];
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 3) / 2f;
			Vector2 drawPos = ((hinted.VectorEffect - Main.screenPosition)) + new Vector2(0f, 4f);
			int timing = (int)(hinted.projectile.localAI[0] - 100);
			timing %= 3;
			timing *= ((tex.Height) / 3);
			spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height) / 3), Color.Lerp(Color.Magenta,Color.White,0.4f).MultiplyRGB(lightColor)*projectile.Opacity, hinted.projectile.rotation, drawOrigin, hinted.projectile.scale, SpriteEffects.None, 0f);
			}

			return false;

		}

static public void Draw(SpriteBatch spriteBatch, Color lightColor)
{


/*int testitem = ModContent.ProjectileType<PrismShardHinted>();

var sortedGoods = Main.projectile.Where(testproj => testproj.active && testproj.type == testitem);

foreach (Projectile proj in sortedGoods)
{
	PrismShardHinted hinted = proj.modProjectile as PrismShardHinted;

	for (int k = hinted.oldPos.Length - 1; k > 0; k--)
	{
		if (hinted.oldPos[k] == default)
			hinted.oldPos[k] = hinted.VectorEffect;
	}

	if (hinted.strength > 0)
	{
		TrailHelper trail = new TrailHelper("DefaultPass", SGAmod.Instance.GetTexture("noise"));
		trail.color = delegate (float percent)
		{
			return Color.Magenta;
		};
		trail.projsize = hinted.projectile.Hitbox.Size() / 2f;
		trail.coordOffset = new Vector2(0, Main.GlobalTime * -1f);
		trail.trailThickness = 4;
		trail.trailThicknessIncrease = 6;
		trail.capsize = new Vector2(4f, 0f);
		trail.strength = hinted.strength;
		trail.DrawTrail(hinted.oldPos.ToList(), hinted.projectile.Center);
	}
}

Main.spriteBatch.End();
Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

foreach (Projectile proj in sortedGoods)
{
	PrismShardHinted hinted = proj.modProjectile as PrismShardHinted;

	Effect hallowed = SGAmod.HallowedEffect;

	hallowed.Parameters["prismColor"].SetValue(Color.Magenta.ToVector3());
	hallowed.Parameters["prismAlpha"].SetValue(0.75f);
	hallowed.Parameters["overlayTexture"].SetValue(SGAmod.Instance.GetTexture("Perlin"));
	hallowed.Parameters["overlayProgress"].SetValue(new Vector3(0, -hinted.projectile.localAI[1] / 50f, hinted.projectile.localAI[1] / 25f));
	hallowed.Parameters["overlayAlpha"].SetValue(0.25f);
	hallowed.Parameters["overlayStrength"].SetValue(new Vector3(2f, 0.20f, hinted.projectile.localAI[1] / 20f));
	hallowed.Parameters["overlayMinAlpha"].SetValue(0f);
	hallowed.Parameters["alpha"].SetValue(hinted.projectile.Opacity);

	hallowed.CurrentTechnique.Passes["Prism"].Apply();

	//if (hinted.projectile.Opacity > 0)
	//{
		if (hinted.projectile.localAI[0] < 100)
			hinted.projectile.localAI[0] = 100 + Main.rand.Next(0, 3);

		Texture2D tex = Main.extraTexture[35];
		Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 3) / 2f;
		Vector2 drawPos = ((hinted.VectorEffect - Main.screenPosition)) + new Vector2(0f, 4f);
		int timing = (int)(hinted.projectile.localAI[0]-100);
		timing %= 3;
		timing *= ((tex.Height) / 3);
		spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height) / 3), Color.White, hinted.projectile.rotation, drawOrigin, hinted.projectile.scale, SpriteEffects.None, 0f);
	//}
}

Main.spriteBatch.End();
Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
*/
		}

	}

	public class PrismEgg : ModNPC
	{
		int counter, counter2 = 0;
		public override void SetDefaults()
		{
			npc.width = 20;
			npc.height = 20;
			npc.damage = 0;
			npc.defense = 100;
			npc.lifeMax = 1500;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 0f;
			npc.noTileCollide = false;
			npc.knockBackResist = 0.25f;
			npc.aiStyle = 66;
			npc.chaseable = false;
			aiType = NPCID.Worm;
			animationType = NPCID.Worm;
			banner = npc.type;
			npc.rarity = 2;
			npc.scale = 0.50f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prism Seed");
		}

		public override bool PreAI()
		{
			npc.timeLeft = 30;
			if (counter == 3)
            {
				Idglib.Chat("A prism seed has appeared!" ,Color.Magenta.R, Color.Magenta.G, Color.Magenta.B);
            }
			if (counter % 80 == 0)
			{
				SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_DarkMageCastHeal, npc.Center);
				if (sound != null)
				{
					sound.Pitch = -0.65f + (counter / 1000f);
				}
			}
			if (counter++ == 1500)
			{
				SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_DarkMageHealImpact, npc.Center);
				if (sound != null)
				{
					sound.Pitch = 0.50f;
				}
				npc.Transform(mod.NPCType("PrismBanshee"));
				counter = 0;
			}
			if (counter2++ == 15)
			{
				npc.scale += 0.005f;
				counter2 = 0;
			}
			return true;
		}
		public override string Texture
		{
			get { return ("Terraria/Projectile_" + ProjectileID.Starfury); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
			PrismBanshee.DrawPrismCore(spriteBatch, drawColor, npc.Center, counter, npc.scale, 96f*(counter/1500f));
			Texture2D tex = Main.npcTexture[npc.type];
			spriteBatch.Draw(tex, npc.Center-Main.screenPosition, null, Color.White, npc.rotation, (tex.Size() / 2f), npc.scale * Main.essScale, SpriteEffects.None, 0f);
			return false;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
			if (!NPC.downedMoonlord || (NPC.CountNPCS(ModContent.NPCType<PrismEgg>())+NPC.CountNPCS(ModContent.NPCType<PrismBanshee>()))>0)
				return 0;

			bool underground = (int)((double)((player.position.Y + (float)player.height) * 2f / 16f) - Main.worldSurface * 2.0) > 0;

			return (player.ZoneHoly && underground) ? (SGAWorld.downedPrismBanshee ? 0.020f : 0.15f) : 0f;
		}
	}


}