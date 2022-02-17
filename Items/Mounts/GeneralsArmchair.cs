using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SGAmod.Items.Mounts
{
	public class GeneralsArmchairMount : IceCubeMount
	{
		public override void SetDefaults() {
			base.SetDefaults();
			mountData.spawnDust = 36;
			mountData.buff = BuffType<GeneralsArmchairBuff>();
			mountData.heightBoost = 22;
			mountData.fallDamage = 0.5f;
			mountData.xOffset = 0;
			mountData.yOffset = -28;
			mountData.jumpHeight = 0;
			mountData.jumpSpeed = 0f;
			mountData.blockExtraJumps = true;
			mountData.constantJump = false;

			int[] array = new int[mountData.totalFrames];
			for (int l = 0; l < array.Length; l++)
			{
				array[l] = 18;
			}
			mountData.playerYOffsets = array;

			/*

			mountData.runSpeed = 1f;
			mountData.dashSpeed = 1f;
			mountData.flightTimeMax = 0;
			mountData.fatigueMax = 0;
			mountData.jumpHeight = 0;
			mountData.acceleration = 0f;
			mountData.jumpSpeed = 0f;
			mountData.blockExtraJumps = true;
			mountData.totalFrames = 1;
			mountData.constantJump = false;
			int[] array = new int[mountData.totalFrames];
			for (int l = 0; l < array.Length; l++) {
				array[l] = 10;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 0;
			//mountData.bodyFrame = 3;
			mountData.yOffset = -10;
			mountData.playerHeadOffset = 6;
			*/

			if (Main.netMode == NetmodeID.Server) {
				return;
			}	

			mountData.textureWidth = mountData.frontTexture.Width + 20;
			mountData.textureHeight = mountData.frontTexture.Height;
		}

		public override void UpdateEffects(Player player)
		{
			//player.noKnockback = true;
		}

		internal class GeneralsArmchairSpecificData
		{
			public GeneralsArmchairSpecificData()
			{
				//Unique Stuff here
			}
		}

		public override void Dismount(Player player, ref bool skipDust)
		{
			player.legRotation = 0;
		}

		public override void SetMount(Player player, ref bool skipDust)
		{
			player.legRotation = -player.direction * 0.75f;
			player.mount._mountSpecificData = new GeneralsArmchairSpecificData();

			for (int i = 0; i < 16; i++) {
				Dust.NewDustPerfect(player.Center + new Vector2(20, 0).RotatedBy(i * Math.PI * 2 / 16f), mountData.spawnDust);
			}
			skipDust = true;
		}

		public override bool Draw(List<DrawData> playerDrawData, int drawType, Player drawPlayer, ref Texture2D texture, ref Texture2D glowTexture, ref Vector2 drawPosition, ref Rectangle frame, ref Color drawColor, ref Color glowColor, ref float rotation, ref SpriteEffects spriteEffects, ref Vector2 drawOrigin, ref float drawScale, float shadow)
		{
			// Draw is called for each mount texture we provide, so we check drawType to avoid duplicate draws.

			GeneralsArmchairSpecificData chairData = (GeneralsArmchairSpecificData)drawPlayer.mount._mountSpecificData;
			Texture2D frontChair = mod.GetTexture("Items/Mounts/GeneralsArmchairFrontOnly");
			Texture2D fullChair = mod.GetTexture("Items/Accessories/ArmchairGeneral");
			Vector2 frontChairSize = frontChair.Size() / 2f;

			if (drawType == 0)
			{
				playerDrawData.Add(new DrawData(fullChair, drawPosition + new Vector2(-4, drawPlayer.height - 32).RotatedBy(rotation), null, drawColor * 1f, rotation, frontChairSize, drawScale, drawPlayer.direction >0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0));
			}
			if (drawType == 2)
			{
				//playerDrawData.Add(new DrawData(frontChair, drawPosition + new Vector2(-4, drawPlayer.height - 32).RotatedBy(rotation), null, drawColor * 1f, rotation, frontChairSize, drawScale, drawPlayer.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0));
			}

			// by returning true, the regular drawing will still happen.
			return true;
		}


		public static readonly PlayerLayer GeneralChair = new PlayerLayer("SGAmod", "GeneralChair", PlayerLayer.Arms, delegate (PlayerDrawInfo drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			SGAmod mod = SGAmod.Instance;
			SGAPlayer modply = drawPlayer.GetModPlayer<SGAPlayer>();

			Texture2D frontChair = mod.GetTexture("Items/Mounts/GeneralsArmchairFrontOnly");

			//better version, from Qwerty's Mod
			Color color = drawInfo.mountColor;

			int drawX = (int)((drawPlayer.MountedCenter.X - 4) - Main.screenPosition.X);
			int drawY = 0;
			drawY = (int)((drawPlayer.MountedCenter.Y +drawPlayer.gravDir*10) - Main.screenPosition.Y);

			DrawData data = new DrawData(frontChair, new Vector2(drawX, drawY), null, color, (float)(drawPlayer.fullRotation), frontChair.Size() / 2f, 1f, (drawPlayer.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally) | (drawPlayer.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
			data.shader = (int)drawPlayer.cWings;
			Main.playerDrawData.Add(data);
		});

	}

	public class GeneralsArmchairBuff : ModBuff
	{

		public override void SetDefaults()
		{
			DisplayName.SetDefault("General's Armchair");
			Description.SetDefault("Grants 10 defense, 10% endurance, Shiny Stone regen, and knockback immunity\n'Rule over with a Kaiser's Knuckles'");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.shinyStone = true;
			player.statDefense = 10;
			player.endurance += 0.10f;
			player.noKnockback = true;
			player.mount.SetMount(MountType<GeneralsArmchairMount>(), player);
			player.buffTime[buffIndex] = 10;
		}
	}

}
