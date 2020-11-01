using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;

namespace SGAmod.NPCs
{
	public class IceFairy : ModNPC
	{
		int shooting=0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Fairy");
			Main.npcFrameCount[npc.type] = 4;
		}
		public override void SetDefaults()
		{
			npc.width = 40;
			npc.height = 40;
			npc.damage = 60;
			npc.defense = 8;
			npc.lifeMax = 400;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 0f;
			npc.knockBackResist = 0.2f;
			npc.aiStyle = 22;
			aiType = 0;
			animationType = 0;
			npc.noTileCollide = true;
			npc.noGravity = true;
			npc.value = 300f;
			banner = npc.type;
			bannerItem = mod.ItemType("IceFairyBanner");
		}

				public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Tile tile = Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY];
			bool underground = (int)((double)(spawnInfo.spawnTileY+20) - Main.worldSurface) > 0;
			return !spawnInfo.playerInTown && !NPC.BusyWithAnyInvasionOfSorts() && !spawnInfo.invasion && !Main.pumpkinMoon && !Main.snowMoon && !Main.eclipse && !underground && Main.dayTime && spawnInfo.player.ZoneSnow && Main.hardMode ? 0.25f : 0f;
		}

		public override void NPCLoot()
		{

				for (int i = 0; i <= Main.rand.Next(3,6); i++){
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("IceFairyDust"));
				}
		
        }


		public override void AI()
		{
			npc.spriteDirection=npc.velocity.X>0 ? -1 : 1;
			shooting=shooting+1;
			if (shooting%200==0){
				npc.netUpdate = true;
				Player P = Main.player[npc.target];
			if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active){
			}else{
		    if (Collision.CanHitLine(new Vector2(npc.Center.X, npc.Center.Y), 1, 1, new Vector2(P.Center.X, P.Center.Y), 1, 1)){
			Idglib.Shattershots(npc.Center,P.position,new Vector2(P.width,P.height),118,npc.lifeMax>2000 ? 25 : 40,(float)Main.rand.Next(60,80)/6,35,2,true,0,true,100);
			}}


		}
	}


				public override void FindFrame(int frameHeight)
		{
			int frats = (int)((1+Math.Sin(shooting/15)*1.5));

			npc.frame.Y=frats*40;
		}

    		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

/*Texture2D orgtex=mod.GetTexture("NPCs/TPD");
Vector2 origin = new Vector2((float)orgtex.Width * 0.5f, (float)orgtex.Height * 0.5f);

spriteBatch.End();

var text = "something";
var measure = Main.fontMouseText.MeasureString(text);
var target = new RenderTarget2D(Main.graphics.GraphicsDevice, (int)100, (int)100);
var position = new Vector2(100, 100);

//RenderTargetUsage.PreserveContents = true;
Main.graphics.GraphicsDevice.SetRenderTarget(target);
//Main.graphics.GraphicsDevice.Clear(Color.Black);
spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
spriteBatch.Draw(orgtex, new Vector2((float)Main.rand.Next(0,100),(float)Main.rand.Next(0,100)),null, lightColor, 0f, origin,new Vector2(3f,3f), SpriteEffects.None, 0f);

//Utils.DrawBorderStringFourWay(spriteBatch, encounterFont, "Test", Vector2.Zero.X, Vector2.Zero.Y, new Color(255, 255, 255, 150), new Color(0, 0, 0, 150), new Vector2());
spriteBatch.End(); // call it anyway
target.GraphicsDevice.SetRenderTarget(null);

//target.GraphicsDevice.SetRenderTarget(null);
spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);

spriteBatch.Draw(target, npc.Center - Main.screenPosition,null, lightColor, npc.velocity.X/5f, origin,new Vector2(1f,1f), SpriteEffects.None, 0f);
target = null;
//RenderTargetUsage.PreserveContents = false;
//Main.spriteBatch.Draw(target, position, Color.White);
//spriteBatch.End(); // if Begin was called by game
*/

		return true;

		}



	}
}

