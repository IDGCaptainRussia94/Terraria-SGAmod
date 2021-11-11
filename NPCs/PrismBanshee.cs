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
using Terraria.Utilities;
using SGAmod.Items;

namespace SGAmod.NPCs
{
	[AutoloadBossHead]
	public class PrismBanshee : ModNPC
	{
		public int bansheeState = 0;
		public int maxattacktime = 300;
		public int expectedHandCount = 2;
		public int essenceCollected = 0;
		public int essenceCollectedTimer = 0;

		public void CollectEssence(int ammount)
		{
			if (essenceCollectedTimer > 0)
				return;

			foreach (NPC hands in Main.npc.Where(myhands => myhands.active && myhands.type == ModContent.NPCType<PrismBansheeHand>() && (int)myhands.ai[1] == npc.whoAmI).ToList())
			{
				for (int i = 0; i < 32; i += 1)
				{
					Vector2 position = new Vector2(Main.rand.Next(-12,12), Main.rand.Next(-12,12));
					int num128 = Dust.NewDust(hands.Center + position, 0, 0, 254, 0, 0, 240, Color.Pink, 1f);
					Main.dust[num128].noGravity = true;
					Main.dust[num128].scale = 2f;
					Main.dust[num128].color = Color.Pink;
					Main.dust[num128].velocity = (Vector2.Normalize(position) * 6f) + (npc.velocity * 4.5f);
				}

				SoundEffectInstance sound = Main.PlaySound(SoundID.NPCHit, (int)hands.Center.X, (int)hands.Center.Y, 5);
				if (sound != null)
					sound.Pitch = -0.75f+(essenceCollected / 6f);

			}

			essenceCollected += ammount;
			if (essenceCollected > 10)
			{

				SoundEffectInstance sound2 = Main.PlaySound(SoundID.Zombie, (int)npc.Center.X, (int)npc.Center.Y, 73);
				if (sound2 != null)
					sound2.Pitch = 0.75f;

				essenceCollected = 0;
				essenceCollectedTimer = 600;
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismic Banshee");
			Main.npcFrameCount[npc.type] = 2;
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
			writer.Write(expectedHandCount);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
			bansheeState = reader.ReadInt32();
			maxattacktime = reader.ReadInt32();
			expectedHandCount = reader.ReadInt32();
		}

		public void ChangeHit(ref int damage, ref bool crit,int player,Projectile proj)
        {
			if (npc.ai[0] < 900)
			{
				//damage = (int)(damage * 0.25f);
				//crit = false;
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
			npc.lifeMax = Main.expertMode ? 80000 : 50000;
			npc.defense = 50;
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
			npc.SGANPCs().dotResist = 0.10f;
		}
		/*public override string Texture
		{
			get { return ("SGAmod/Items/Consumables/PrismaticBansheeStar"); }
		}*/

        public override void NPCLoot()
        {
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AuroraTear"), (Main.expertMode ? 2 : 1)*(2-(int)npc.ai[3]));
			//Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("IlluminantEssence"), Main.rand.Next(12, Main.expertMode ? 30 : 20) * (2 - (int)npc.ai[3]));
			if (SGAWorld.downedPrismBanshee<1)
			SGAWorld.downedPrismBanshee = 1;
			if (npc.ai[3] < 1 && SGAWorld.downedPrismBanshee<2)
				SGAWorld.downedPrismBanshee = 2;

		}

        public void DoAttacks()
		{

			essenceCollectedTimer -= 1;
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

				if (essenceCollectedTimer > 0 || (Main.rand.Next(0, 4) == 0 && npc.ai[3] < 1))
				{
					bansheeState = 3;
					maxattacktime = 460;
				}

				//Filters.Scene.Activate("SGAmod:Shockwave", proj.Center, new object[0]).GetShader().UseColor(rippleCount, rippleSize, expandRate).UseTargetPosition(proj.Center);
				npc.netUpdate = true;
			}

			if (bansheeState == 3)
            {
				if (statetimer == 60)
                {
					npc.velocity = Vector2.Zero;
					Vector2 starterPos = npc.Center + new Vector2(0, -46);
					Projectile.NewProjectile(starterPos, Vector2.Normalize(Main.player[npc.target].Center - starterPos) * 2f, ModContent.ProjectileType<BansheeBeam>(), 100, 16);

                }

				if (statetimer > 40 && statetimer<450)
				{
					if (Main.netMode != 2)
					{
						Texture2D tex = ModContent.GetTexture("SGAmod/NPCs/PrismicBanshee");
						npc.frame = new Rectangle(0, tex.Height / 2, tex.Width, tex.Height / 2);
					}
				}
			}


			if (bansheeState == 2)
			{
				if (!Main.dedServ)
				{
					if (!Filters.Scene["SGAmod:ShockwaveBanshee"].IsActive())
					{
						//Main.NewText("work!");
						Filters.Scene.Activate("SGAmod:ShockwaveBanshee", npc.Center, new object[0]).GetShader().UseColor(5f, 2f, 10f).UseTargetPosition(npc.Center);
					}


					float progress = (400 - statetimer) / 2400f;
					if (statetimer >= 400)
					{
						progress = MathHelper.Clamp((statetimer - 400f) / 200f, 0f, 1f);
						if (Main.netMode != 2)
						{
							Texture2D tex = ModContent.GetTexture("SGAmod/NPCs/PrismicBanshee");
							npc.frame = new Rectangle(0, tex.Height / 2, tex.Width, tex.Height / 2);
						}
					}

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

					foreach(Player player2 in Main.player.Where(testplayer => testplayer.active && Collision.CanHitLine(testplayer.MountedCenter,1,1,npc.Center,1,1)))
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
			npc.dontTakeDamage = false;

			if (Main.netMode != 2)
			{
				Texture2D tex = ModContent.GetTexture("SGAmod/NPCs/PrismicBanshee");
				npc.frame = new Rectangle(0, 0, tex.Width, tex.Height / 2);
			}

			if (npc.ai[3] < 1)
			{
				if (expectedHandCount < 6)
					npc.dontTakeDamage = true;
				npc.GivenName = "Aurora Banshee";
			}

			if (npc.ai[0] == 1)
			{
				HalfVector2 half = new HalfVector2(-128, -40);

				int hand = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<PrismBansheeHand>());
				Main.npc[hand].ai[1] = npc.whoAmI;
				Main.npc[hand].ai[2] = ReLogic.Utilities.ReinterpretCast.UIntAsFloat(half.PackedValue);
				Main.npc[hand].localAI[2] = expectedHandCount;
				Main.npc[hand].netUpdate = true;

				half = new HalfVector2(128, -40);

				hand = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<PrismBansheeHand>());
				Main.npc[hand].ai[1] = npc.whoAmI;
				Main.npc[hand].ai[2] = ReLogic.Utilities.ReinterpretCast.UIntAsFloat(half.PackedValue);
				Main.npc[hand].localAI[2] = expectedHandCount;				
				Main.npc[hand].netUpdate = true;
			}

			Player P = Main.player[npc.target];

			bool underground = ((int)((double)((npc.position.Y + (float)npc.height) * 2f / 16f) - Main.worldSurface * 2.0) > 0);

			if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active || !underground)
			{
				npc.TargetClosest(false);
				P = Main.player[npc.target];
				if (!P.active || P.dead || !underground)
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

				if (npc.ai[3]< 1 && npc.ai[0] % 91 == 0 && npc.ai[1] < 1 && expectedHandCount<24)
				{
					List<NPC> they = Main.npc.Where(myhands => myhands.active && myhands.type == ModContent.NPCType<PrismBansheeHand>() && (int)myhands.ai[1] == npc.whoAmI).ToList();
					if (Main.npc.Where(myhands => myhands.active && myhands.type == ModContent.NPCType<PrismBansheeHand>() && (int)myhands.ai[1] == npc.whoAmI).ToList().Count < expectedHandCount)
					{
						npc.ai[1] = 1;
						npc.netUpdate = true;
                    }
                    else
                    {
						int index = 0;
						List<NPC> they2 = Main.npc.Where(myhands => myhands.active && myhands.type == ModContent.NPCType<PrismBansheeHand>() && (int)myhands.ai[1] == npc.whoAmI && myhands.localAI[2]==expectedHandCount).ToList();
						if (they2.Count < 2 && they.Count >= 2)
						{
							they[0].localAI[2] = expectedHandCount;
							they[1].localAI[2] = expectedHandCount;
						}

					}
				}

				if (npc.ai[1] > 0)
				{
					npc.ai[1] += 1;

					if (npc.ai[1] == 120)
					{
						if (expectedHandCount<8)
						expectedHandCount += 1;
						HalfVector2 half = new HalfVector2(-Main.rand.NextFloat(48, 160), Main.rand.NextFloat(-80, 80));

						int hand = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<PrismBansheeHand>());
						Main.npc[hand].ai[1] = npc.whoAmI;
						Main.npc[hand].ai[2] = ReLogic.Utilities.ReinterpretCast.UIntAsFloat(half.PackedValue);
						Main.npc[hand].localAI[2] = Math.Min(7,expectedHandCount);
						Main.npc[hand].netUpdate = true;

						half = new HalfVector2(Main.rand.NextFloat(48, 160), Main.rand.NextFloat(-80, 80));

						hand = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<PrismBansheeHand>());
						Main.npc[hand].ai[1] = npc.whoAmI;
						Main.npc[hand].ai[2] = ReLogic.Utilities.ReinterpretCast.UIntAsFloat(half.PackedValue);
						Main.npc[hand].localAI[2] = Math.Min(7, expectedHandCount);
						Main.npc[hand].netUpdate = true;
					}

					if (npc.ai[1] > 240)
					{
						npc.ai[1] = -1;
						npc.velocity *= 0.50f;
					}

				}

			}
		}

        public static void DrawPrismCore(SpriteBatch spriteBatch, Color drawColor,Vector2 drawWhere,float rotter,float scale = 1,float scaleup=96,int startingpoint=0)
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
					float fadeOut = 84f * (scaleup/84f);
					spriteBatch.Draw(tex2, drawPos + vecAngle * (startingpoint+i), null, Color.Magenta * peralpha * (1f - (i / fadeOut)),rotAngle + MathHelper.PiOver2, (tex2.Size() / 2f), scale, SpriteEffects.None, 0f);
				}
			}

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Vector2 drawPos = npc.Center - Main.screenPosition;
			Texture2D texture = ModContent.GetTexture("SGAmod/NPCs/PrismicBanshee");// SGAmod.PrismBansheeTex;
			Texture2D tex = Main.npcTexture[npc.type];
			//Texture2D tex3 = Main.projectileTexture[ProjectileID.CultistBossIceMist];

			List<NPC> NPCHands = Main.npc.Where(handtest => handtest.active && handtest.type == ModContent.NPCType<PrismBansheeHand>() && (int)handtest.ai[1] == npc.whoAmI).ToList();

			float inrc = Main.GlobalTime / 30f;

			float scaleEffect = (float)(Math.Sin((npc.ai[1] / 240f) * MathHelper.Pi));

			DrawPrismCore(spriteBatch,drawColor,npc.Center,npc.localAI[0],npc.scale+(scaleEffect*2f),128,(int)MathHelper.Clamp(npc.localAI[0]/8f,0,24));

			float strength = 1f;

			if (NPCHands.Count > 0)
			{
				foreach (NPC hand in NPCHands)
				{
					if (strength > 0)
					{
						Vector2 handoffset = new Vector2(hand.Center.X, hand.position.Y);
						List<Vector2> trailspots = new List<Vector2>();
						Vector2 point1 = Vector2.Lerp(npc.Center, handoffset, 0.5f) + new Vector2(0, 128 + (float)Math.Sin(npc.localAI[0] / 40f) * 32f);
						Vector2 point2 = new HalfVector2() { PackedValue = ReLogic.Utilities.ReinterpretCast.FloatAsUInt(hand.ai[2]) }.ToVector2();

						for (float f = 0; f < 1f; f += 0.02f)
						{
							Vector2 curve = npc.Center;
							Vector2 offset2 = new Vector2(Math.Sign(point2.X) * 72, 64);
							Vector2 offset3 = handoffset + (offset2 * (1f - MathHelper.Clamp((handoffset - (handoffset + offset2)).LengthSquared() / (320 * 320), 0f, 1f)));
							curve = curve.BezierCurve(npc.Center + (Vector2.UnitY * -32).RotatedBy(npc.rotation), point1, offset3, handoffset, f);
							trailspots.Add(curve);
						}


						TrailHelper trail = new TrailHelper("DefaultPass", mod.GetTexture("Noise"));
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
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			Effect hallowed = SGAmod.HallowedEffect;

			hallowed.Parameters["alpha"].SetValue(1);
			hallowed.Parameters["prismColor"].SetValue(Color.Magenta.ToVector3());
			hallowed.Parameters["prismAlpha"].SetValue(0);
			hallowed.Parameters["overlayTexture"].SetValue(mod.GetTexture("TiledPerlin"));
			hallowed.Parameters["overlayProgress"].SetValue(new Vector3(0, -npc.localAI[0] / 250f, npc.localAI[0] / 150f));
			hallowed.Parameters["overlayAlpha"].SetValue(0.20f);
			hallowed.Parameters["overlayStrength"].SetValue(new Vector3(2f, 0.10f, npc.localAI[0] / 150f));
			hallowed.Parameters["overlayMinAlpha"].SetValue(0f);
			hallowed.Parameters["rainbowScale"].SetValue(1f);
			hallowed.Parameters["overlayScale"].SetValue(new Vector2(2f, 4f));

			hallowed.CurrentTechnique.Passes["Prism"].Apply();

			spriteBatch.Draw(texture, drawPos, npc.frame, Color.White, npc.rotation, new Vector2(texture.Width, (texture.Height/2) * 1.15f) / 2f, npc.scale, SpriteEffects.None, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			hallowed.Parameters["alpha"].SetValue(1);
			hallowed.Parameters["prismColor"].SetValue(Color.Magenta.ToVector3());
			hallowed.Parameters["prismAlpha"].SetValue(0.185f);
			hallowed.Parameters["overlayTexture"].SetValue(mod.GetTexture("Perlin"));
			hallowed.Parameters["overlayProgress"].SetValue(new Vector3(0, -npc.localAI[0] / 250f, npc.localAI[0] / 150f));
			hallowed.Parameters["overlayAlpha"].SetValue(0.25f);
			hallowed.Parameters["overlayStrength"].SetValue(new Vector3(2f, 0.10f, npc.localAI[0] / 150f));
			hallowed.Parameters["overlayMinAlpha"].SetValue(0f);
			hallowed.Parameters["rainbowScale"].SetValue(1f);
			hallowed.Parameters["overlayScale"].SetValue(new Vector2(1f, 1f));

			hallowed.CurrentTechnique.Passes["Prism"].Apply();

				if (NPCHands.Count > 0)
			{
				foreach (NPC hand in NPCHands)
				{
					PrismBansheeHand myhand = hand.modNPC as PrismBansheeHand;
					Texture2D handtex = Main.npcTexture[hand.type];
					spriteBatch.Draw(handtex, new Vector2(hand.Center.X, hand.Center.Y) - Main.screenPosition, null, Color.White, hand.rotation, handtex.Size() / 2f, hand.scale, hand.spriteDirection > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				}
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			hallowed.Parameters["alpha"].SetValue(1);
			hallowed.Parameters["prismColor"].SetValue(Color.Magenta.ToVector3());
			hallowed.Parameters["prismAlpha"].SetValue(0.10f);
			hallowed.Parameters["overlayTexture"].SetValue(mod.GetTexture("Perlin"));
			hallowed.Parameters["overlayProgress"].SetValue(new Vector3(0, -npc.localAI[0] / 150f, npc.localAI[0] / 75f));
			hallowed.Parameters["overlayAlpha"].SetValue(0.25f);
			hallowed.Parameters["overlayStrength"].SetValue(new Vector3(2f, 0.10f, npc.localAI[0] / 60f));
			hallowed.Parameters["overlayMinAlpha"].SetValue(0f);
			hallowed.Parameters["rainbowScale"].SetValue(1f);
			hallowed.Parameters["overlayScale"].SetValue(new Vector2(0.25f, 0.25f));


			hallowed.CurrentTechnique.Passes["Prism"].Apply();

			List<NPC> NPCspirits = Main.npc.Where(spirittest => spirittest.active && spirittest.type == ModContent.NPCType<PrismSpirit>() && (int)spirittest.ai[1] == npc.whoAmI).ToList();

			if (NPCspirits.Count > 0)
			{
				foreach (NPC spirit in NPCspirits)
				{
					Texture2D handtex = Main.npcTexture[spirit.type];
					spriteBatch.Draw(handtex, new Vector2(spirit.Center.X, spirit.Center.Y) - Main.screenPosition, new Rectangle(0, ((int)(spirit.localAI[0] / 30f) % 4) * (handtex.Height / 4), handtex.Width, handtex.Height / 4), Color.White, -spirit.velocity.X * 0.05f, new Vector2(handtex.Width, handtex.Height / 4) / 2f, spirit.scale, spirit.spriteDirection > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				}
			}

			hallowed.Parameters["overlayScale"].SetValue(new Vector2(1,1));

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			spriteBatch.Draw(tex, drawPos, null, Color.White, npc.rotation, (tex.Size() / 2f), npc.scale, SpriteEffects.None, 0f);

			if (npc.ai[1] > 0)
			{

				VertexBuffer vertexBuffer;
				Effect effect = SGAmod.TrailEffect;

				effect.Parameters["WorldViewProjection"].SetValue(WVP.View(Main.GameViewMatrix.Zoom) * WVP.Projection());
				effect.Parameters["imageTexture"].SetValue(SGAmod.Instance.GetTexture("Space"));
				effect.Parameters["coordOffset"].SetValue(0);
				effect.Parameters["coordMultiplier"].SetValue(4f);
				effect.Parameters["strength"].SetValue(Math.Min(0.8f,scaleEffect *1.25f));

				VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[6];

				Vector3 screenPos = (npc.Center - Main.screenPosition).ToVector3();
				float size = 64f + scaleEffect*(360f);

				vertices[0] = new VertexPositionColorTexture(screenPos + new Vector3(-size, -size, 0), Color.Purple, new Vector2(0, 0));
				vertices[1] = new VertexPositionColorTexture(screenPos + new Vector3(-size, size, 0), Color.Purple, new Vector2(0, 1));
				vertices[2] = new VertexPositionColorTexture(screenPos + new Vector3(size, -size, 0), Color.Purple, new Vector2(1, 0));

				vertices[3] = new VertexPositionColorTexture(screenPos + new Vector3(size, size, 0), Color.Purple, new Vector2(1, 1));
				vertices[4] = new VertexPositionColorTexture(screenPos + new Vector3(-size, size, 0), Color.Purple, new Vector2(0, 1));
				vertices[5] = new VertexPositionColorTexture(screenPos + new Vector3(size, -size, 0), Color.Purple, new Vector2(1, 0));

				vertexBuffer = new VertexBuffer(Main.graphics.GraphicsDevice, typeof(VertexPositionColorTexture), vertices.Length, BufferUsage.WriteOnly);
				vertexBuffer.SetData<VertexPositionColorTexture>(vertices);

				Main.graphics.GraphicsDevice.SetVertexBuffer(vertexBuffer);

				RasterizerState rasterizerState = new RasterizerState();
				rasterizerState.CullMode = CullMode.None;
				Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

				effect.CurrentTechnique.Passes["DefaultPassSinShade"].Apply();

				Main.graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);

				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


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
			if (Owner != null && Owner.ai[3] >= 1)
			{
				npc.life = npc.lifeMax;
				return false;
			}
			return true;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
			SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_CrystalCartImpact, (int)npc.Center.X, (int)npc.Center.Y);
			if (sound != null)
			{
				sound.Pitch += Main.rand.NextFloat(0f,0.25f);
			}
		}

        public override void NPCLoot()
        {
			SoundEffectInstance sound = Main.PlaySound(SoundID.Shatter, (int)npc.Center.X, (int)npc.Center.Y, 1);
			if (sound != null)
			{
				sound.Pitch = -0.25f;
			}
			sound = Main.PlaySound(SoundID.DD2_CrystalCartImpact, (int)npc.Center.X, (int)npc.Center.Y);
			if (sound != null)
			{
				sound.Pitch = -0.25f;
			}

			for (float f = 6; f < 12; f += 0.10f)
			{
				Vector2 offset = Main.rand.NextVector2Circular(1f, f);
				int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y)+ Vector2.Normalize(offset)*f, npc.width, npc.height, DustID.PurpleCrystalShard);
				Main.dust[dust].scale = 1;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Main.rand.NextVector2Circular(f,f);
			}

		}

        public override void SetDefaults()
		{
			npc.lifeMax = 10000;
			npc.defense = 20;
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
			//npc.HitSound = SoundID.NPCHit7;
			// npc.DeathSound = SoundID.NPCDeath6;
			npc.Opacity = 0f;
			npc.SGANPCs().dotResist = 0.20f;
		}
		public override string Texture
		{
			get { return ("SGAmod/NPCs/PrismicCrystalCluster"); }
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
			writer.Write(npc.localAI[2]);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			blockAngle = (float)reader.ReadDouble();
			blockTime = reader.ReadInt32();
			bansheesmoothness = reader.ReadInt32();
			npc.localAI[3] = reader.ReadInt32();
			npc.localAI[2] = reader.ReadInt32();
		}

		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
			projectile.SGAProj().damageReduce += (0.25f/(1f+((npc.localAI[2] - 1f)/5f)));
			projectile.SGAProj().damageReduceTime = 60;
			//projectile.damage = projectile.damage >> 2;
		}

		private void ArmOutState(ref Vector2 gohere)
		{
			gohere = Owner.Center + new Vector2(ArmOffset.X * 2f, ArmOffset.Y/2f);
		}
		public void DisabledMovement(Vector2 gohere)
		{

			//Vector2 belowus = Vector2.UnitY.RotatedBy(MathHelper.Pi+((float)Math.Sin(npc.localAI[0] / 12f) * (MathHelper.Pi / 1.5f)));
			float timers = 0.5f+((float)Math.Sin(npc.localAI[0] / 12f)/3f);
			Vector2 distvector = (gohere+new Vector2(ArmOffset.X/2f, 144+ArmOffset.Y/3f) -(ArmOffset*timers)) - npc.Center;

			npc.velocity *= 0.85f;

			//if (distvector.LengthSquared() > 64)
				npc.velocity += Vector2.Normalize(distvector) * Math.Min(8f, distvector.Length() / 64f) * MathHelper.Clamp(1f - ((npc.localAI[3]) / 100f), 0f, 0.75f);
			//else
				//npc.Center = gohere;

		}		
		public void NormalMovement(Vector2 gohere)
		{
			Vector2 distvector = gohere - npc.Center;

			if (blockTime > 0)
			{
				gohere = Owner.Center+Owner.velocity + (Vector2.UnitX * Math.Abs(ArmOffset.X)).RotatedBy(blockAngle);
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

			//Main.NewText(npc.localAI[2] + " : "+ bantie.expectedHandCount);
			if (npc.localAI[2] != bantie.expectedHandCount)
				return false;

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
					Vector2 offset = new Vector2(npc.Center.X + (aicounter * Math.Sign(ArmOffset.X) * (16f+((ArmOffset.Y + 40)*0.1f))), P.MountedCenter.Y - 300);
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
				gohere = Owner.Center + new Vector2(Math.Sign(ArmOffset.X) * (80f - (Owner.ai[0] - 900) / 6f), ArmOffset.Y);
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

			npc.rotation = -npc.velocity.X * 0.15f;

			if (Owner != null)
			{
				//if (npc.target<0)
				npc.TargetClosest();

				Vector2 gohere = Owner.Center + ArmOffset;

				PrismBanshee prism = Owner.modNPC as PrismBanshee;

				if (prism.essenceCollectedTimer > 0)
				{
					DisabledMovement(gohere);
					return;
				}

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
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<IlluminantEssenceBoss>(), 1);
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
				if (Collision.CanHitLine(checkloc, 1, 1, checkFlyLoc, 1, 1))
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

				npc.dontTakeDamage = Owner.ai[3] < 1;

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
				TrailHelper trail = new TrailHelper("DefaultPass", SGAmod.Instance.GetTexture("Noise"));
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
				spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height) / 3), Color.Lerp(Color.Magenta, Color.White, 0.4f).MultiplyRGB(lightColor) * projectile.Opacity, hinted.projectile.rotation, drawOrigin, hinted.projectile.scale, SpriteEffects.None, 0f);
			}

			return false;

		}

		static public void Draw(SpriteBatch spriteBatch, Color lightColor)
		{
			//draw
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
				npc.ai[3] = 1;
				npc.netUpdate = true;
				counter = 0;
				return false;
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
			get { return ("SGAmod/Items/Consumables/PrismaticBansheeStar"); }
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

			return (player.ZoneHoly && underground) ? (SGAWorld.downedPrismBanshee>0 ? 0.020f : 0.15f) : 0f;
		}
	}

	public class IlluminantEssenceBoss : IlluminantEssence
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Illuminant Essence");
			Tooltip.SetDefault("'Shards of Banshee'");
			ItemID.Sets.ItemNoGravity[item.type] = true;
			ItemID.Sets.ItemIconPulse[item.type] = true;
		}
		public override void PostUpdate()
		{
			Lighting.AddLight(item.Center, Color.HotPink.ToVector3() * 0.55f * Main.essScale);
		}
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Lerp(Color.White,Color.Red,0.5f+MathHelper.Clamp((float)Math.Sin(Main.GlobalTime*5f)*0.5f,0f,1f));
        }
        public override bool OnPickup(Player player)
        {
			if (NPC.CountNPCS(ModContent.NPCType<PrismBanshee>()) < 1)
			{
				Item.NewItem(item.Center, ModContent.ItemType<IlluminantEssence>(), item.stack);
				return false;
			}

			Projectile.NewProjectile(player.Center, Vector2.UnitX.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * 8f, ModContent.ProjectileType<BansheeEssenceCollectedProj>(),0,0,player.whoAmI);
			return false;
        }
    }

	public class BansheeEssenceCollectedProj : PinkyMinionKilledProj
	{

		protected override float ScalePercent => MathHelper.Clamp(projectile.timeLeft / 10f, 0f, Math.Min(projectile.localAI[0] / 6f, 1f));
		protected override int EnemyType => ModContent.NPCType<PrismBanshee>();
		protected override float SpinRate => 0f;
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

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Banshee collected essence");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void ReachedTarget(NPC target)
		{

			PrismBanshee prism = target?.modNPC as PrismBanshee;

			if (prism != null)
			{

				prism.CollectEssence(1);

				projectile.ai[0] += 1;
				projectile.timeLeft = (int)MathHelper.Clamp(projectile.timeLeft, 0, 10);
				projectile.netUpdate = true;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			for (int i = 0; i < projectile.oldPos.Length; i += 1)//dumb hack to get the trails to not appear at 0,0
			{
				if (projectile.oldPos[i] == default)
					projectile.oldPos[i] = projectile.position;
			}

			TrailHelper trail = new TrailHelper("DefaultPass", mod.GetTexture("Noise"));
			UnifiedRandom rando = new UnifiedRandom(projectile.whoAmI);
			float colorz = rando.NextFloat();
			trail.color = delegate (float percent)
			{
				return Color.Purple;
			};
			trail.projsize = projectile.Hitbox.Size() / 2f;
			trail.coordOffset = new Vector2(0, Main.GlobalTime * -1f);
			trail.trailThickness = 4 + MathHelper.Clamp(projectile.ai[0], 0f, 30f);
			trail.trailThicknessIncrease = 6;
			//trail.capsize = new Vector2(6f, 0f);
			trail.strength = ScalePercent*0.75f;
			trail.DrawTrail(projectile.oldPos.ToList(), projectile.Center);

			Texture2D mainTex = Main.itemTexture[ModContent.ItemType<IlluminantEssence>()];

			Main.spriteBatch.Draw(mainTex, projectile.Center - Main.screenPosition, null, Color.White * MathHelper.Clamp(projectile.timeLeft / 10f, 0f, 1f), 0, mainTex.Size() / 2f, 1f, default, 0);

			return false;
		}
	}

	public class BansheeBeam : Items.Weapons.Aurora.AuraBeamStikeProj
    {

		int ExplodeTime => 240;
		Vector2 startingLoc = default;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Banshee Beam Fury");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Starfury);
			base.SetDefaults();
			aiType = -1;
			projectile.aiStyle = -1;
			projectile.timeLeft = 1100;
			projectile.extraUpdates = 2;
			projectile.width = 64;
			projectile.height = 64;
			projectile.friendly = false;
			projectile.hostile = true;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (timer < ExplodeTime)
			{
				return false;
			}

			float hitThere = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center,
				projectile.Center + Vector2.Normalize(projectile.velocity) * 6400,32f, ref hitThere);
		}

		public override void AI()
		{
			if (timer < 1)
			{
				startingLoc = projectile.Center;
			}

			if (timer == ExplodeTime)
			{
				SoundEffectInstance sound = Main.PlaySound(SoundID.Zombie, (int)projectile.Center.X, (int)projectile.Center.Y, 104);
				if (sound != null)
				{
					sound.Pitch = 0.85f;
				}
			}

			if (timer < ExplodeTime && timer%20==0)
			{
				SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_DrakinBreathIn, (int)projectile.Center.X, (int)projectile.Center.Y);
				if (sound != null)
				{
					sound.Pitch = -0.4f+(timer/(float)ExplodeTime)*1.25f;
					sound.Volume = 1f;
				}
			}

			if (SGAmod.ScreenShake < 5)
            {
				SGAmod.AddScreenShake(MathHelper.Clamp(((timer-((float)ExplodeTime-80f)))/80f,0f,2f), projectile.timeLeft * 5f, projectile.Center);
            }

			byte playerz = Player.FindClosest(projectile.Center, 1, 1);
			if (playerz >= 0)
			{
				Player player = Main.player[playerz];

				if (player != null && player.active && !player.dead)
				{
					float speed = projectile.velocity.Length();
					float turnspeed = (0.001f*(MathHelper.TwoPi)*((MathHelper.Clamp((timer-ExplodeTime)/800f,0f,1f+(timer - ExplodeTime)/ 2400f)*(Main.expertMode ? 1f : 0f)) + 0.25f));
					projectile.velocity = projectile.velocity.ToRotation().AngleTowards((player.Center-projectile.Center).ToRotation(), turnspeed).ToRotationVector2() * speed;
				}
			}

			projectile.position -= projectile.velocity;
			timer += 1;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{

			Effect hallowed = SGAmod.HallowedEffect;

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			Texture2D tex = Main.projectileTexture[projectile.type];

			Vector2 fromposition = startingLoc;

			float trailalpha = MathHelper.Clamp(projectile.timeLeft / 30f, 0f, 1f);

			float spread = (float)Math.Pow(timer / 64f,0.75f);

			float explodeSize = MathHelper.Clamp((timer - ExplodeTime) / 10f, 0.10f, 1.20f) * trailalpha;

			List<float> sploderEffect = new List<float>();
			for (int i = 0; i < 10; i += 1)
			{
				sploderEffect.Add(((i / 10f) + (Main.GlobalTime * 3f)) % 1f);
			}

			sploderEffect = sploderEffect.OrderBy(testby => testby).ToList();

			spriteBatch.Draw(tex, fromposition - Main.screenPosition, null, Color.White * 0.50f, projectile.velocity.ToRotation() + MathHelper.PiOver2, new Vector2(tex.Width / 2f, tex.Height), new Vector2(0.05f, spread), SpriteEffects.None, 0);

			foreach (float splodsize in sploderEffect)
			{
				Color warningcolor = Main.hslToRgb((splodsize/10f)%1f,255,0.75f) * trailalpha;

				hallowed.Parameters["alpha"].SetValue(0.40f);
				hallowed.Parameters["prismColor"].SetValue(warningcolor.ToVector3());
				hallowed.Parameters["prismAlpha"].SetValue(0);
				hallowed.Parameters["overlayTexture"].SetValue(mod.GetTexture("TiledPerlin"));
				hallowed.Parameters["overlayProgress"].SetValue(new Vector3(0, -timer / 250f, timer / 150f));
				hallowed.Parameters["overlayAlpha"].SetValue(0.20f);
				hallowed.Parameters["overlayStrength"].SetValue(new Vector3(2f, 0.10f, timer / 150f));
				hallowed.Parameters["overlayMinAlpha"].SetValue(0f);
				hallowed.Parameters["rainbowScale"].SetValue(2f);
				hallowed.Parameters["overlayScale"].SetValue(new Vector2(1f, 1f));

				hallowed.CurrentTechnique.Passes["Prism"].Apply();

				spriteBatch.Draw(tex, fromposition - Main.screenPosition, null, warningcolor * 0.40f, projectile.velocity.ToRotation() + MathHelper.PiOver2, new Vector2(tex.Width / 2f, tex.Height), new Vector2(explodeSize * splodsize, spread), SpriteEffects.None, 0);
			}


			Texture2D glowStar = mod.GetTexture("Extra_57b");
			Vector2 glowSize = glowStar.Size();

			UnifiedRandom random = new UnifiedRandom(projectile.whoAmI);

			float alphaIK = MathHelper.Clamp(timer / 6f, 0f, 1f) * trailalpha;

			float trailsize2 = MathHelper.Clamp(projectile.timeLeft / 5f, 0f, 1f);

			float explodeSize2 = MathHelper.Clamp((timer) / (float)ExplodeTime, 0.10f, 1.00f) * trailalpha;

			for (float ff = 1f; ff > 0.25f; ff -= 0.05f)
			{
				Color color = Main.hslToRgb(random.NextFloat(1f), 0.65f, 0.85f);

				hallowed.Parameters["alpha"].SetValue(alphaIK * 0.75f);
				hallowed.Parameters["prismColor"].SetValue(color.ToVector3());
				hallowed.Parameters["prismAlpha"].SetValue(0);
				hallowed.Parameters["overlayTexture"].SetValue(mod.GetTexture("TiledPerlin"));
				hallowed.Parameters["overlayProgress"].SetValue(new Vector3(0, -timer / 640f, timer / 242f));
				hallowed.Parameters["overlayAlpha"].SetValue(0.25f);
				hallowed.Parameters["overlayStrength"].SetValue(new Vector3(2f, 0.10f, timer / 164f));
				hallowed.Parameters["overlayMinAlpha"].SetValue(0f);
				hallowed.Parameters["rainbowScale"].SetValue(1f);
				hallowed.Parameters["overlayScale"].SetValue(new Vector2(1f,1f));

				hallowed.CurrentTechnique.Passes["Prism"].Apply();

				float rot = random.NextFloat(0.05f, 0.15f) * (random.NextBool() ? 1f : -1f) * (Main.GlobalTime * 8f);
				spriteBatch.Draw(glowStar, fromposition - Main.screenPosition, null, color * alphaIK * 0.25f, random.NextFloat(MathHelper.TwoPi) + rot, glowSize / 2f, ((new Vector2(random.NextFloat(0.15f, 0.50f), explodeSize2) + new Vector2(ff, ff)) * trailsize2 * 0.75f* explodeSize2)*(1f+(explodeSize*1f)), SpriteEffects.None, 0);
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


			return false;
		}

	}


}