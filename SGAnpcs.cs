using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using System.Threading.Tasks;
using System.Threading;
using SGAmod.NPCs.Hellion;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Idglibrary;
using AAAAUThrowing;

namespace SGAmod
{
    public class SGAnpcs : GlobalNPC
    {

		public override bool InstancePerEntity
		{
			get
			{
				return true;
			}
		}

		public int Snapped = 0;
		public bool NinjaSmoked = false;
		public bool Snapfading = false;
		public float truthbetold=0;
		public float damagemul = 1f;
		public bool DankSlow = false;
		public bool MassiveBleeding = false;
		public bool Napalm = false;
		public bool thermalblaze=false; public bool acidburn =false;
		public bool Gourged=false;
		public bool SunderedDefense = false;
		public bool MoonLightCurse = false;
		public bool DosedInGas=false;
		public bool InfinityWarStormbreaker=false;
		public int InfinityWarStormbreakerint=0;
		public int RemovedFireImmune=0;
		public int Combusted=0;
		public int immunitetolightning=0;
		public float TimeSlow = 0;
		public bool HellionArmy = false;
		public bool Sodden = false;
		public bool ELS = false;
		public bool TimeSlowImmune = false;
		bool fireimmunestate=false;
		bool[] otherimmunesfill=new bool[3];
		public bool Mircotransactions;
		public int counter = 0;


		public int FindBuffIndex(NPC npc,int type)
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
			Sodden = false;
			SunderedDefense = false;
			DankSlow = false;
			ELS = false;
			drawonce = true;
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

		public void DoTheDraw(NPC npc)
		{
			if (Snapped < 3)
				return;

			if (SnappedDrawing == null && Snapped>2)
					SnappedDrawing = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight, false, SurfaceFormat.HdrBlendable, DepthFormat.None);

				RenderTargetBinding[] binds = Main.graphics.GraphicsDevice.GetRenderTargets();
				//Main.spriteBatch.End();
				Main.graphics.GraphicsDevice.SetRenderTarget(SnappedDrawing);
				Main.graphics.GraphicsDevice.Clear(Color.Transparent);

			drawonce = true;

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.CreateScale(1f,1f,1f));

			Main.instance.DrawNPC(npc.whoAmI,true);

			drawonce = false;

			Texture2D texture = mod.GetTexture("NPCs/TPD");

			//Main.spriteBatch.Draw(texture, npc.Center-Main.screenPosition, null, Color.White, npc.spriteDirection + (npc.ai[0] * 0.4f), new Vector2(16, 16), new Vector2(Main.rand.Next(5, 20) / 4f, Main.rand.Next(5, 20) / 4f), SpriteEffects.None, 0f);

			Main.spriteBatch.End();

				Main.graphics.GraphicsDevice.SetRenderTargets(binds);


		}

		public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
		{
			if (Snapped > 0 && Snapped < 5)
			return false;
			if (drawonce)
			return base.PreDraw(npc, spriteBatch, drawColor);

			if (Snapped > 0 && SnappedDrawing != null && !drawonce)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

				var deathShader = GameShaders.Misc["SGAmod:DeathAnimation"];
				deathShader.UseOpacity(((float)Snapped / 200f));
				deathShader.Apply(null);

				/*ArmorShaderData shader = GameShaders.Armor.GetShaderFromItemId(ItemID.IntenseRainbowDye);
				shader.UseOpacity(0.5f);
				shader.UseSaturation(0.25f);
				shader.Apply(null);*/

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

		public override bool CheckActive(NPC npc)
		{
			if (HellionArmy) {
			if (npc.timeLeft<3)
			npc.StrikeNPCNoInteraction(9999999,1,1);
			return false;

			}
			return true;
		}

		public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
		{
			float spawnrate2 = player.GetModPlayer<SGAPlayer>().morespawns;
			spawnRate = (int)(spawnRate/ spawnrate2);
			maxSpawns += (int)((spawnrate2-1) * 10f);
		}

		public override void UpdateLifeRegen(NPC npc, ref int damage)
		{

			if (Combusted>0)
			{
				npc.lifeRegen -= 100+(int)Math.Pow(npc.lifeMax, 0.5);	
				Combusted-=1;
				if (damage < 50)
					damage = 50;
			}

			if (thermalblaze)
			{
				if (npc.lifeRegen > 0)
				{
					npc.lifeRegen = 0;
				}
				if (damage<10)
				damage = 10;
				int boost = 0;
				if (npc.HasBuff(BuffID.Oiled))
					boost = 50;
				npc.lifeRegen -= (Combusted>0 ? 150 : 30)+boost;	
			}

			if (acidburn)
			{
				npc.lifeRegen -= 20+Math.Min(300, npc.defense*2);
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
				if(npc.lifeRegen > 0)
				{
					npc.lifeRegen = 0;
				}
				npc.lifeRegen -= 40;
				if(damage < 10)
				{
					damage = 10;
				}
			}
		}

		public override void DrawEffects(NPC npc, ref Color drawColor)
		{
			if (Snapped>0 || Snapfading)
			{
				Vector2 position2 = npc.position;
				position2.X -= 2f;
				position2.Y -= 2f;

				if (Main.rand.Next(2) == 0)
				{
					int num52 = Dust.NewDust(position2, npc.width + 4, npc.height + 2, mod.DustType("FadeDust"), 0f, 0f, 50, Color.DarkGreen, ((1.25f-(Snapped/200f))/2f)+(Snapfading ? 0.04f : 0f));
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
					dust.velocity += npc.velocity+new Vector2(Math.Sign(-npc.velocity.X),0f)*(4f+((Snapped/200f))*2f);
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
				drawColor.R = (byte)(drawColor.R*0.8f);
				drawColor.G = (byte)(drawColor.R*0.5f);
				drawColor.B = (byte)(drawColor.R*0.5f);
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

			if (Gourged){
					Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
					int dust = Dust.NewDust(npc.Center+randomcircle*8f, 0,0, 5, -npc.velocity.X * 0.3f, 4f+(npc.velocity.Y * -0.4f), 30, default(Color), 0.85f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].color=Main.hslToRgb(0f, 0.5f, 0.35f);
				}

				if (MassiveBleeding){
					Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
					int dust = Dust.NewDust(new Vector2(npc.position.X,npc.position.Y)+randomcircle*1.2f, npc.width + 4, npc.height + 4, 5, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 30, default(Color), 1.5f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].color=Main.hslToRgb(0f, 0.5f, 0.35f);
				}

				if (InfinityWarStormbreaker || SunderedDefense){
					Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
					int dust = Dust.NewDust(new Vector2(npc.position.X,npc.position.Y)+randomcircle*(1.2f*(float)npc.width), npc.width + 4, npc.height + 4, mod.DustType("TornadoDust"), npc.velocity.X * 0.4f, (npc.velocity.Y-7f) * 0.4f, 30, default(Color)*1f, 0.5f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].color=Main.hslToRgb(0f, 0.5f, 0.35f);
				}
			bool hasbuff = npc.HasBuff(mod.BuffType("DigiCurse"));
			if (MoonLightCurse || hasbuff)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				int dust = Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y) + randomcircle * (1.2f * (float)npc.width), 0, 0, hasbuff && (Main.rand.Next(0,1)==0 || !MoonLightCurse) ? DustID.PinkCrystalShard : DustID.AncientLight, 0, 0, 30, Color.Turquoise, 1.5f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = (new Vector2(npc.velocity.X * 0.4f, (npc.velocity.Y) * 0.4f))-randomcircle*8;
				Main.dust[dust].velocity=Main.dust[dust].velocity.RotatedBy(MathHelper.ToRadians(90));
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



		}

        public override bool PreAI(NPC npc)
        {

       if ((npc.type==NPCID.CultistDevote || npc.type==NPCID.CultistArcherBlue || npc.type==NPCID.CultistTablet) && (SGAWorld.downedHarbinger==false && Main.netMode<1)){
       	npc.active=false;
       	return false;
        }else{
		//npc.dontTakeDamage=false;
        }
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
        InfinityWarStormbreakerint=10;
        if (InfinityWarStormbreakerint>0)
        InfinityWarStormbreakerint-=1;

        fireimmunestate=npc.buffImmune[BuffID.OnFire];
        for (int i = 0; i < SGAmod.otherimmunes.Length;i++ ){
        otherimmunesfill[i]=npc.buffImmune[SGAmod.otherimmunes[i]];}

        if (Combusted<5 && !DosedInGas)
        npc.buffImmune[BuffID.OnFire] = fireimmunestate;
        for (int i = 0; i < SGAmod.otherimmunes.Length;i++ ){npc.buffImmune[SGAmod.otherimmunes[i]]=otherimmunesfill[i];}
        if (DosedInGas){
		npc.buffImmune[BuffID.OnFire]=false;
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
						if (counter % 150 == 0 && npc.value > Item.buyPrice(0, 1, 0, 0))
						{
							npc.value -= Item.buyPrice(0, 0, 50, 0);
							Item.NewItem(npc.position, new Vector2(npc.width, npc.height),50,noGrabDelay: true);
						}
					}
			}

			if (ELS)
			{
				for(int i = 0; i < npc.buffType.Length;i+=1)
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
					npc.lifeRegen = (int)(npc.lifeRegen*2.5f);
			}

			if (HellionArmy)
			{
				if (Hellion.GetHellion()!=null)
				if (Hellion.GetHellion().army.Count<1)
				npc.StrikeNPCNoInteraction(9999999, 1, 1);
				if (Hellion.GetHellion() == null)
				npc.StrikeNPCNoInteraction(9999999, 1, 1);

			}


			if (SunderedDefense)
			{
				for (int i = 0; i < Main.maxPlayers; i += 1)
				{
					if (npc.immune[i] > 0)
						npc.immune[i] = Math.Max(npc.immune[i]-3,0);
				}
			}
			if (TimeSlow > 0 && !TimeSlowImmune)
			{
				npc.position -= npc.velocity-(npc.velocity / (1+TimeSlow));
			}
			TimeSlow = 0;
		}

		public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            Player player = Main.player[Main.myPlayer];
            switch(type)
            {
				case NPCID.Merchant:

					if (WorldGen.crimson)
					{
						shop.item[nextSlot].SetDefaults(ItemID.Leather);
						shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 5, 0);
						nextSlot++;
					}
					if (SGAWorld.downedCratrosity){
                        shop.item[nextSlot].SetDefaults(mod.ItemType("TerrariacoCrateKey"));
                        shop.item[nextSlot].shopCustomPrice = Item.buyPrice(20,0,0,0);
                        nextSlot++;
                    }
                    if (Main.hardMode){
                        shop.item[nextSlot].SetDefaults(mod.ItemType("PremiumUpgrade"));
                        nextSlot++;
                    }
                	break;

				case NPCID.Dryad:

					if (SGAWorld.downedSpiderQueen)
					{
						shop.item[nextSlot].SetDefaults(mod.ItemType("StoneBarrierStaff"));
						shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 1, 50, 0);
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

					if (player.CountItem(mod.ItemType("SnappyShark"))>0){
                        shop.item[nextSlot].SetDefaults(mod.ItemType("SharkTooth"));
                        nextSlot++;
                    }
					if (player.CountItem(mod.ItemType("StarfishBlaster"))+player.CountItem(mod.ItemType("Starfishburster")) > 0)
					{
						shop.item[nextSlot].SetDefaults(ItemID.Starfish);
						shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 2, 0);
						nextSlot++;
					}
					break;
				case NPCID.Pirate:

					if (NPC.CountNPCS(NPCID.PartyGirl)>0)
					{
						shop.item[nextSlot].SetDefaults(ItemID.ExplosiveBunny);
						shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 5, 0);
						nextSlot++;
					}
					break;
			}

			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (sgaplayer.MidasIdol>0)
			{

			foreach(Item item in shop.item)
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
						item.value = (int)(item.value * 0.85);
					}
				}
			}

			if (sgaplayer.greedyperc > 0)
			{

			foreach(Item item2 in shop.item)
				{
					Main.NewText(sgaplayer.greedyperc);
					item2.value = (int)(item2.value * (1f-(Math.Min(0.9f,sgaplayer.greedyperc*1f))));
				}
			}
		}

		public override void SetupTravelShop(int[] shop, ref int nextSlot)
		{
			if (Main.rand.Next(0, 3) == 1 && Main.hardMode)
			{
				shop[nextSlot] = mod.ItemType("PeacekeepersDuster");
				nextSlot++;
			}
			if (Main.rand.Next(0, 3) == 1 && NPC.downedBoss2)
			{
				shop[nextSlot] = mod.ItemType("DynastyJavelin");
				nextSlot++;
			}		
		}


		public override bool PreNPCLoot(NPC npc)
		{

			if (HellionArmy)
			{
				if (Hellion.GetHellion() != null)
				{
					if (Hellion.GetHellion().armyspawned>5)
					Hellion.GetHellion().armyspawned -= 2;
				}
			}


			if (npc.type == NPCID.CultistBoss && SGAWorld.downedWraiths<3)
            {
            NPCLoader.blockLoot.Add(ItemID.LunarCraftingStation);
            if (Main.netMode!=1)
            SGAWorld.stolecrafting=-500;
            }

			if (SGAWorld.NightmareHardcore > 0)
			{
				npc.value *= (int)(SGAWorld.NightmareHardcore * 1.50);
				if (Main.rand.Next(0, 100) < 10)
					NPCLoot(npc);

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
					if (!Main.dedServ)
					{
						ply.GetModPlayer<SGAPlayer>().DoExpertiseCheck(npc);
					}
					else
					{
						if ((ply.Center - npc.Center).Length() < 1400)
						{
							ModPacket packet = mod.GetPacket();
							packet.Write(250);
							packet.Write(npc.type);
							packet.Send(ply.whoAmI);
						}
					}
					if (ply.HasItem(mod.ItemType("EntropyTransmuter")))
					{
						if (npc.Distance(ply.Center) < 1000)
						{
							ply.GetModPlayer<SGAPlayer>().AddEntropy(npc.lifeMax);
						}

					}

				}

			}
			if (npc.boss)
			{
				Achivements.SGAAchivements.UnlockAchivement("Offender", Main.LocalPlayer);
			}

			if (SGAWorld.tf2quest == 2)
			{
				SGAWorld.questvars[0] += 1;
			}
			if (npc.type == NPCID.MoonLordCore)
			{
				if (Main.rand.Next(10) < (Main.expertMode ? 2 : 1))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SwordofTheBlueMoon"));
				if (SGAWorld.downedCratrosity)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SalvagedCrate"));
				if (!Main.expertMode)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("EldritchTentacle"),Main.rand.Next(15,30));
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
if (Main.hardMode && SGAWorld.tf2cratedrops && (Main.rand.Next(0, 300) < 1 || (SGAWorld.downedCratrosity == false && Main.rand.Next(0, 30) < 1)))
	{
		Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TerrariacoCrateBase"));
	}
	if (npc.type == NPCID.WyvernHead && NPC.downedGolemBoss && Main.rand.Next(100) <= 5)
	{
		Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Tornado"));
	}

	if (npc.type == NPCID.Golem && Main.rand.Next(100) <= 20 && !Main.expertMode)
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

	if (npc.type == NPCID.RedDevil)
	{
		if (Main.rand.Next(2) <= 1)
		Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FieryShard"), Main.rand.Next(2, 4));
		Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ConsumeHell"), Main.rand.Next(1, 3));
	}
	if (npc.type == NPCID.Lavabat && Main.rand.Next(4) <= 1 && Main.hardMode)
	{
		Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FieryShard"), Main.rand.Next(1, 2));
	}
	if (npc.Center.Y > Main.maxTilesY - 100 && Main.rand.Next(100) < 1 && Main.hardMode)
	{
		Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FieryShard"));
	}

	if (npc.type==NPCID.WallCreeper || npc.type == NPCID.BloodCrawler || npc.type == NPCID.JungleCreeper ||
	npc.type == NPCID.WallCreeperWall || npc.type == NPCID.BloodCrawlerWall || npc.type == NPCID.JungleCreeperWall || npc.type == NPCID.BlackRecluse || npc.type == NPCID.BlackRecluseWall)
	if (Main.rand.Next(0,2)==0)
	Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.RottenEgg);

	if ((npc.type == NPCID.EnchantedSword || npc.type == NPCID.IlluminantBat || npc.type == NPCID.IlluminantSlime || npc.type == NPCID.ChaosElemental) && Main.rand.Next(2) <= 1 && NPC.downedMoonlord)
	{
		Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("IlluminantEssence"), Main.rand.Next(1, 3));
	}

			if (npc.aiStyle!=107 && npc.aiStyle!= 108 && npc.aiStyle != 109 && npc.aiStyle != 110 && npc.aiStyle != 111)
			{
				if (Main.player[npc.target] != null)
				{
					if (Main.player[npc.target].ZoneHoly && npc.position.Y > Main.rockLayer)
					{
						if (Main.rand.Next(100) <= 1 && NPC.downedMoonlord)
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("IlluminantEssence"), Main.rand.Next(1, 3));
						}
					}
				}
			}
	


			//}


		}

		//Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType(dropitems[chances]));

		public override void OnHitByItem(NPC npc,Player player, Item item, int damage, float knockback, bool crit)
		{
			OnHit(npc,player, damage,knockback,crit,item,null,false);
		}

		public override void OnHitByProjectile(NPC npc,Projectile projectile, int damage, float knockback, bool crit)
		{
			OnHit(npc,Main.player[projectile.owner], damage,knockback,crit,null, projectile, true);
		}

		/*public override bool StrikeNPC (NPC npc, ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
		{

		defense=(int)(defense*0.0085);
		return true;
		}*/

		public override bool? CanBeHitByProjectile(NPC npc,Projectile projectile)
		{
		if (projectile.type==ProjectileID.CultistBossLightningOrbArc && immunitetolightning>0){
		return false;
		}else{
		return base.CanBeHitByProjectile(npc,projectile);
		}

		}

		public override bool SpecialNPCLoot(NPC npc)
		{
			if (NPC.CountNPCS(mod.NPCType("TPD")) > 0){
				if (npc.type == NPCID.SkeletronHead || npc.type == NPCID.Spazmatism || npc.type == NPCID.Retinazer || npc.type == NPCID.TheDestroyer || npc.type == NPCID.TheDestroyerBody || npc.type == NPCID.TheDestroyerTail)
					return false;
			}
			return base.SpecialNPCLoot(npc);
		}

		public void OnCrit(NPC npc, Projectile projectile, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			SGAPlayer moddedplayer = player.GetModPlayer<SGAPlayer>();

			if (moddedplayer.gunslingerLegendtarget > -1)
			{
				if (npc.whoAmI	==	moddedplayer.gunslingerLegendtarget)
				{
					damage = (int)((float)damage * 1.50f);
				}
				else
				{
					damage= (int)((float)damage * 0.25f);
				}

			}

			if (crit)
			{
				if (moddedplayer.ninjaSash > 0 && ((item != null && item.Throwing().thrown) || (projectile != null && projectile.Throwing().thrown)))
				{
					int ammo = 0;
					if (player.HasItem(ItemID.Shuriken))
						ammo = ItemID.Shuriken;
					if (player.HasItem(ItemID.ThrowingKnife))
						ammo = ItemID.ThrowingKnife;
					if (player.HasItem(ItemID.PoisonedKnife))
						ammo = ItemID.PoisonedKnife;


					if (ammo > 0)
					{
						Item itemy = new Item();
						itemy.SetDefaults(ammo);
						int shootype = itemy.shoot;

						Vector2 anglez = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, -2000));
						anglez.Normalize();

						float i = Main.rand.NextFloat(90f, 260f);

						int thisoned = Projectile.NewProjectile(npc.Center + (anglez * i), anglez * -16f, shootype, damage, 0f, Main.myPlayer);
						Main.projectile[thisoned].ranged = false;
						Main.projectile[thisoned].thrown = false;


						for (float gg = 2f; gg > 0.25f; gg -= 0.6f)
						{
							int goreIndex = Gore.NewGore(npc.Center + (anglez * i), -anglez * gg, Main.rand.Next(61, 64), 1f);
							Main.gore[goreIndex].scale = 1.5f;
						}

						player.ConsumeItem(ammo);
					}
				}
			}
		}

		public void DoApoco(NPC npc, Projectile projectile, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			SGAPlayer moddedplayer = player.GetModPlayer<SGAPlayer>();
			int chance = -1;
			if (projectile != null) {
				if (projectile.melee)
					chance = 0;
				if (projectile.ranged)
					chance = 1;
				if (projectile.magic)
					chance = 2;
				if (projectile.thrown)
					chance = 3;
			}
			if (item != null)
			{
				if (item.melee)
					chance = 0;
				if (item.ranged)
					chance = 1;
				if (item.magic)
					chance = 2;
				if (item.thrown)
					chance = 3;

			}
			if (chance > -1 && npc!=null)
			{
				if (Main.rand.Next(0, 100) < moddedplayer.apocalypticalChance[chance] && crit)
				{
					if (moddedplayer.HoE && projectile != null)
					{
						float ammount = damage;
						if (moddedplayer.lifestealentropy > 0)
						{
							projectile.vampireHeal((int)((ammount * moddedplayer.apocalypticalStrength)), npc.Center);
							moddedplayer.lifestealentropy -= ammount;
						}
					}

					if (moddedplayer.dualityshades)
					{
						int ammo = 0;
						for(int i = 0; i < 4; i += 1)
						{
							if (moddedplayer.ammoinboxes[i] > 0)
							{
								int ammox = moddedplayer.ammoinboxes[i];
								Item itemx = new Item();
								itemx.SetDefaults(ammox);
								if (itemx.ammo == AmmoID.Bullet)
								{
									ammo = ammox;
									break;
								}
							}
						}
						if (ammo > 0)
						{
							Item itemy = new Item();
							itemy.SetDefaults(ammo);
							int shootype = itemy.shoot;

							for (int i = 128; i < 260; i += 128)
							{
								Vector2 anglez = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000));
								anglez.Normalize();

								Main.PlaySound(SoundID.Item, (int)((npc.Center.X)+(anglez.X * i)), (int)((npc.Center.Y)+(anglez.Y*i)), 25, 0.5f, Main.rand.NextFloat(-0.9f,-0.25f));
								
								int thisoned = Projectile.NewProjectile(npc.Center+(anglez * i), anglez*-16f, shootype, (int)(damage *1.50f * moddedplayer.apocalypticalStrength), 0f, Main.myPlayer);
								Main.projectile[thisoned].ranged = false;


								for (float gg = 4f; gg > 0.25f; gg -= 0.15f)
								{
									int dustIndex = Dust.NewDust(npc.Center + new Vector2(-8, -8) + (anglez * i), 16, 16, DustID.AncientLight, -anglez.X * gg, -anglez.Y * gg, 150, Color.Purple, 3f);
									Dust dust = Main.dust[dustIndex];
									dust.noGravity = true;
								}

								player.ConsumeItem(ammo);
							}
						}


					}

					if (moddedplayer.CalamityRune)
					{
							Main.PlaySound(SoundID.Item45, npc.Center);
							int boom=Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, mod.ProjectileType("BoulderBlast"), (int)((damage*2) * moddedplayer.apocalypticalStrength), knockback * 2f, player.whoAmI, 0f, 0f);
							Main.projectile[boom].usesLocalNPCImmunity = true;
							Main.projectile[boom].localNPCHitCooldown = -1;
							Main.projectile[boom].netUpdate = true;
							IdgProjectile.AddOnHitBuff(boom, BuffID.Daybreak, (int)(60f* moddedplayer.apocalypticalStrength));
							IdgProjectile.AddOnHitBuff(boom, mod.BuffType("EverlastingSuffering"), (int)(400f * moddedplayer.apocalypticalStrength));
					}

					damage = (int)(damage * (3f + (moddedplayer.apocalypticalStrength - 1f)));
					RippleBoom.MakeShockwave(npc.Center, 8f, 1f, 10f, 60, 1f);
					CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), Color.DarkRed, "Apocalyptical!", true, false);
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/crit_hit").WithVolume(.7f).WithPitchVariance(.25f), npc.Center);


				}
			}

		}

		public void LifeSteal(NPC npc, Player player, ref int damage, ref float knockback, ref bool crit)
		{
			if (player != null)
			{
				if (player.GetModPlayer<SGAPlayer>().HasGucciGauntlet())
				{
					if (player.ownedLargeGems[4] && Main.rand.Next(0, (int)(50000f / Math.Max((float)damage,40f))) == 1)
					{

						Projectile projectile = new Projectile();
						projectile.Center = npc.Center;
						projectile.owner = player.whoAmI;
						projectile.vampireHeal((int)(40), npc.Center);
					}
				}
			}

		}

		public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			SGAPlayer moddedplayer = player.GetModPlayer<SGAPlayer>();
			damage = (int)(damage * damagemul);
			DoApoco(npc,null, player, item, ref damage, ref knockback, ref crit);
			OnCrit(npc, null, player, item, ref damage, ref knockback, ref crit);
			if (acidburn)
				damage += (int)(Math.Min(npc.defense, 5)/2);
			if (Gourged)
				damage += (npc.defense / 2)/2;
			if (MoonLightCurse)
				damage += (int)(Math.Min(npc.defense, 50)/2);
			if (Sodden)
				damage = (int)((float)damage * 1.33f);

			if (moddedplayer != null && moddedplayer.PrimordialSkull)
				if (npc.HasBuff(BuffID.OnFire))
					damage = (int)(damage * 1.25);

			if (moddedplayer.MidasIdol > 0)
			{
				if (npc.HasBuff(BuffID.Midas))
				damage = (int)(damage * 1.15f);
			}

			if (moddedplayer.MisterCreeperset)
			{
				if (item.shoot<1 && item.melee)
				{
					if (player.velocity.Y > 1)
					{
						crit = true;
					}
				}
			}

			if (moddedplayer.Blazewyrmset)
			{
				if (npc.HasBuff(mod.BuffType("ThermalBlaze")) && item.melee)
				{
					damage = (int)(damage * 1.20f);
				}
			}
			LifeSteal(npc, player, ref damage, ref knockback, ref crit);
		}

		public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			Player player = Main.player[projectile.owner];
			damage = (int)(damage * damagemul);
			if (player != null)
			{
				if (projectile.trap)
				damage = (int)(damage * player.GetModPlayer<SGAPlayer>().TrapDamageMul);
				OnCrit(npc, projectile, player, null, ref damage, ref knockback, ref crit);
				DoApoco(npc, projectile, player, null, ref damage, ref knockback, ref crit);
			}
			if (acidburn)
				damage += (int)(Math.Min(npc.defense, 5) / 2);
			if (MoonLightCurse)
				damage += (int)(Math.Min(npc.defense, 30) / 2);
			if (Gourged)
				damage += (npc.defense / 2) / 2;
			if (Sodden)
			damage = (int)((float)damage * 1.33f);
			if (player != null && player.GetModPlayer<SGAPlayer>().PrimordialSkull)
				if (npc.HasBuff(BuffID.OnFire))
					damage = (int)(damage * 1.25);

			if (projectile.friendly)
			{
				Player P = Main.player[projectile.owner];
				SGAPlayer moddedplayer = P.GetModPlayer<SGAPlayer>();
				SGAprojectile modeproj = projectile.GetGlobalProjectile<SGAprojectile>();

				if (moddedplayer.MidasIdol > 0)
				{
					if (npc.HasBuff(BuffID.Midas))
						damage = (int)(damage * 1.15f);

				}

				bool trapdamage = false;
				if (projectile.trap)
				trapdamage = true;


				if (trapdamage)
				{
					float totaldamage = 0f;
					//damage += (int)((npc.defense * moddedplayer.TrapDamageAP) / 2.00);
					totaldamage += moddedplayer.TrapDamageAP;
					if (moddedplayer.JaggedWoodenSpike)
					{
						totaldamage += 0.4f;
						//damage += (int)((npc.defense*0.4)/2.00);
					}
					if (moddedplayer.JuryRiggedSpikeBuckler)
					{
						//damage += (int)(damage * 0.1);
						totaldamage += 0.1f;
						//damage += (int)((npc.defense * 0.1) / 2.00);
					}
					totaldamage = Math.Min(totaldamage, 1f);
					if (moddedplayer.GoldenCog)
					{
						npc.life = npc.life-(int)(damage * 0.10);
						if (Main.netMode==2)
						NetMessage.SendData(23, -1, -1, null, npc.whoAmI, 0f, 0f, 0f, 0, 0, 0);
					}					
					damage += (int)((npc.defense * totaldamage) / 2.00);
				}

				if (moddedplayer.beefield > 0 && (projectile.type==ProjectileID.Bee || projectile.type == ProjectileID.GiantBee))
				{
					damage += (int)(Math.Min(moddedplayer.beedamagemul,10f+(moddedplayer.beedamagemul/50f))*1.5f);
				}

				if (modeproj.myplayer != null)
					P = modeproj.myplayer;

				if (P != null)
				{
					if (moddedplayer.CirnoWings == true && projectile.coldDamage)
					{
						damage = (int)((double)damage * 1.20);
					}
				}

				if (moddedplayer.Blazewyrmset)
				{
					if (npc.HasBuff(mod.BuffType("ThermalBlaze")) && projectile.melee)
					{
						damage = (int)(damage * 1.20f);
					}
				}

			}
			LifeSteal(npc, player, ref damage, ref knockback, ref crit);
		}

		private void OnHit(NPC npc,Player player, int damage,float knockback, bool crit,Item item,Projectile projectile,bool isproj=false)
		{
			SGAPlayer moddedplayer = player.GetModPlayer<SGAPlayer>();

			if (moddedplayer.HasGucciGauntlet())
			{
				if (player.ownedLargeGems[0] && Main.rand.Next(0,10)==0)
					npc.AddBuff(BuffID.ShadowFlame, 60 * 15);

			}

			if (item != null && npc.life - damage < 1 && npc.lifeMax > 50)
			{
				if (item.type == mod.ItemType("Powerjack"))
				{
					player.HealEffect(25, false);
					player.statLife += 25;
					if (player.statLife > player.statLifeMax2)
					{
						player.statLife = player.statLifeMax2;
					}
					NetMessage.SendData(66, -1, -1, null, player.whoAmI, (float)25f, 0f, 0f, 0, 0, 0);
				}
			}

			if (isproj)
			{
				if (projectile.minion && moddedplayer.IDGset)
				{
					if (npc.immune[projectile.owner] > 0)
					{
						npc.immune[projectile.owner] = ((int)(npc.immune[projectile.owner] * 0.75f));

					}

					moddedplayer.digiStacks = Math.Min(moddedplayer.digiStacksMax, moddedplayer.digiStacks+(int)Math.Max((float)projectile.damage,(float)damage));
				}

				if (projectile.type == ProjectileID.CultistBossLightningOrbArc)
				{
					immunitetolightning = projectile.localNPCHitCooldown;
				}

				bool trapdamage = false;
				if (projectile != null && projectile.trap)
					trapdamage = true;


				if (trapdamage)
				{
					if (moddedplayer.JaggedWoodenSpike)
					{
						if (Main.rand.Next(0, 100) < 15)
							npc.AddBuff(mod.BuffType("MassiveBleeding"), 60 * 5);
					}
				}

				if (moddedplayer.Mangroveset && player.ownedProjectileCounts[mod.ProjectileType("MangroveOrb")]<4)
				{
					if (crit && projectile.thrown)
					{
						player.AddBuff(BuffID.DryadsWard, 60 * 5);

						List<Projectile> itz = Idglib.Shattershots(player.Center, player.Center + (player.Center - npc.Center), new Vector2(0, 0), mod.ProjectileType("MangroveOrb"), damage, 8f, 120, 2, false, 0, false, 400);
						//itz[0].damage = 30;
						itz[0].owner = player.whoAmI; itz[0].friendly = true; itz[0].hostile = false;
						itz[1].owner = player.whoAmI; itz[1].friendly = true; itz[1].hostile = false;
						Main.projectile[itz[0].whoAmI].netUpdate = true;
						Main.projectile[itz[1].whoAmI].netUpdate = true;
						if (Main.netMode == 2 && itz[0].whoAmI < 200)
						{
							NetMessage.SendData(27, -1, -1, null, itz[0].whoAmI, 0f, 0f, 0f, 0, 0, 0);
						}

					}

				}

			}

SGAWorld.overalldamagedone=((int)damage)+SGAWorld.overalldamagedone;
			if (projectile != null)
			{
				if (moddedplayer.FieryheartBuff > 0 && projectile.owner == player.whoAmI)
				{
					npc.AddBuff(189, 1 * 30);
				}
				if (moddedplayer.CirnoWings == true && projectile.owner == player.whoAmI)
				{
					if (isproj && projectile.magic == true)
						npc.AddBuff(BuffID.Frostburn, 5 * 60);
				}
			}

			if (moddedplayer.Redmanastar > 0)
			{
				if (isproj && projectile.magic == true)
				{
					int[] buffids = { BuffID.OnFire, mod.BuffType("ThermalBlaze"), BuffID.Daybreak };
					if (projectile != null && Main.rand.Next(0, 100) < 5)
						npc.AddBuff(buffids[moddedplayer.Redmanastar - 1], 4 * 60);
					if (projectile != null && Main.rand.Next(0, 100) < 1)
						npc.buffImmune[buffids[moddedplayer.Redmanastar - 1]]=false;
				}
			}

if (moddedplayer.SerratedTooth==true){
if (damage>npc.defense*5)
npc.AddBuff(mod.BuffType("MassiveBleeding"), Math.Min((int)(1f+((float)damage-(float)npc.defense*5f)*0.02f)*60,60*5));
}

			if (moddedplayer.Blazewyrmset)
			{
				if (crit && ((item != null && item.melee && item.pick+ item.axe+item.hammer<1)))
				{
					if (player.SGAPly().AddCooldownStack(12 * 60))
					{
						Main.PlaySound(SoundID.Item45, npc.Center);
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, mod.ProjectileType("BoulderBlast"), damage * 3, knockback * 3f, player.whoAmI, 0f, 0f);
					}

				}
			}



bool hasabuff=false;

if (npc.HasBuff(BuffID.OnFire))
hasabuff=true;
for (int i = 0; i < SGAmod.otherimmunes.Length;i++ ){
if (npc.HasBuff(SGAmod.otherimmunes[i])){
hasabuff=true;
}}


if (DosedInGas && hasabuff){
Combusted=60*10;
int buff=npc.FindBuffIndex(mod.BuffType("DosedInGas"));

				if ((isproj && Main.player[projectile.owner].GetModPlayer<SGAPlayer>().MVMBoost) || (item != null && player.GetModPlayer<SGAPlayer>().MVMBoost))
				{
					int prog = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Vector2.Zero.X, Vector2.Zero.Y, ProjectileID.GrenadeIII, 350, 5f, player.whoAmI);
					Main.projectile[prog].thrown = false; Main.projectile[prog].ranged = false; Main.projectile[prog].timeLeft = 2; Main.projectile[prog].netUpdate = true;
					IdgProjectile.Sync(prog);

				}


			if (buff > -1)
			{
				npc.DelBuff(buff);
				IdgNPC.AddBuffBypass(npc.whoAmI, BuffID.OnFire, 60 * 10);
			}

}

}

public override bool StrikeNPC(NPC npc,ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit){
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
					if (SGAWorld.downedWraiths == 0 && Main.rand.Next(0, 2) == 0)
						chat = "I'd be careful using the furnace if I were you, something is watching... Prehaps you can lure it out beforehand with a [i:" + mod.ItemType("WraithCoreFragment") + "] before it catches you off guard, I heard these can be made with copper/tin ore and fallen stars";
					if (SGAWorld.downedWraiths == 1 && Main.rand.Next(0, 2) == 0 && Main.hardMode)
						chat = "Hmmm... Another creature has done something worse this time, they have stolen your knowledge to make a hardmode anvil, you can fight it by using a [i:" + mod.ItemType("WraithCoreFragment2") + "] which is as made from tier 1 hardmode ores and the previous summoning item";
					if (SGAWorld.downedWraiths < 4 && Main.rand.Next(0, 2) == 0 && NPC.downedAncientCultist)
						chat = "Yet Another creature has stolen knowledge from you, this time the Anicent Manipulator AND made you lose your knowledge to craft Luminite Bars, yes I know this is getting old but this is the last one, you can fight it by using a [i:" + mod.ItemType("WraithCoreFragment3") + "] made with lunar fragments and the previous summoning item. Rematch will unlock Luminite bars but require defeating Moonlord first.";
					break;
				case NPCID.ArmsDealer:
					if (Main.rand.Next(0, 5) == 0)
					{
						chat = "Somewhere along the way I got all these Starfish and Shark Teeth, now if only you could find guns that use them I could sell them to you";
						return;

					}
						if (Main.rand.Next(0, 3) == 0 && NPC.CountNPCS(mod.NPCType("Dergon")) > 0)
					{
						string[] lines = { "I'm sure the dragon is worth alot on the black market, just need to find the right person",
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