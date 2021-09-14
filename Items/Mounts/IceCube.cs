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
	public class IceCubeMount : ModMountData
	{
		public override void SetDefaults() {
			mountData.spawnDust = DustID.Ice;
			mountData.buff = BuffType<IceCubeMountBuff>();
			mountData.heightBoost = 8;
			mountData.fallDamage = 0.5f;
			mountData.runSpeed = 0f;
			mountData.dashSpeed = 0f;
			mountData.flightTimeMax = 0;
			mountData.fatigueMax = 0;
			mountData.jumpHeight = 5;
			mountData.acceleration = 0f;
			mountData.jumpSpeed = 4f;
			mountData.blockExtraJumps = false;
			mountData.totalFrames = 4;
			mountData.constantJump = true;
			int[] array = new int[mountData.totalFrames];
			for (int l = 0; l < array.Length; l++) {
				array[l] = 10;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 0;
			//mountData.bodyFrame = 3;
			mountData.yOffset = -6;
			mountData.playerHeadOffset = 6;
			mountData.standingFrameCount = 4;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 4;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 0;
			mountData.flyingFrameDelay = 0;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 4;
			mountData.idleFrameDelay = 12;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = true;
			mountData.swimFrameCount = mountData.inAirFrameCount;
			mountData.swimFrameDelay = mountData.inAirFrameDelay;
			mountData.swimFrameStart = mountData.inAirFrameStart;
			if (Main.netMode == NetmodeID.Server) {
				return;
			}

			mountData.textureWidth = mountData.backTexture.Width + 20;
			mountData.textureHeight = mountData.backTexture.Height;
		}

		public override void UpdateEffects(Player player)
		{

			player.slippy2 = true;

			// This code spawns some dust if we are moving fast enough.
			if (!(Math.Abs(player.velocity.X) > 4f))
			{
				return;
			}
			Rectangle rect = player.getRect();
			if (player.velocity.Y == 0)
			{
				for (int i = 0; i < MathHelper.Clamp(Math.Abs(player.velocity.X/8),0,3); i++)
				{
					Dust.NewDust(new Vector2(rect.X - 24, rect.Y + rect.Height - 6), rect.Width + 48, 6, 161);
				}
			}
		}

		internal class IceCubeSpecificData
		{
			// count tracks how many balloons are still left. See ExamplePerson.Hurt to see how count decreases whenever damage is taken.
			public List<Vector2> iceCubes = new List<Vector2>();
			public IceCubeSpecificData()
			{
				for (int a = -20; a <= 20; a += 4)
				{
					for (int i = 0; i < 12; i += 3)
					{
						iceCubes.Add(new Vector2(a + Main.rand.Next(-2, 3), i+28 + Main.rand.Next(-1, 2)));
					}
				}
			}
		}

		public override void SetMount(Player player, ref bool skipDust)
		{
			player.mount._mountSpecificData = new IceCubeSpecificData();

			for (int i = 0; i < 16; i++) {
				Dust.NewDustPerfect(player.Center + new Vector2(20, 0).RotatedBy(i * Math.PI * 2 / 16f), mountData.spawnDust);
			}
			skipDust = true;
		}

		public override bool Draw(List<DrawData> playerDrawData, int drawType, Player drawPlayer, ref Texture2D texture, ref Texture2D glowTexture, ref Vector2 drawPosition, ref Rectangle frame, ref Color drawColor, ref Color glowColor, ref float rotation, ref SpriteEffects spriteEffects, ref Vector2 drawOrigin, ref float drawScale, float shadow) {
			// Draw is called for each mount texture we provide, so we check drawType to avoid duplicate draws.
			if (drawType == 2) {
				// We draw some extra balloons before _Back texture
				IceCubeSpecificData iceCubes = (IceCubeSpecificData)drawPlayer.mount._mountSpecificData;
				//int timer = DateTime.Now.Millisecond % 800 / 200;
				Texture2D balloonTexture = Main.itemTexture[ItemID.FrozenSlimeBlock];
				Texture2D iceBlock = mod.GetTexture("Items/Mounts/GiantIceCubePlatform");
				Vector2 orig = balloonTexture.Size() / 2f;

				/*foreach (Vector2 icespot in iceCubes.iceCubes)
				{
					playerDrawData.Add(new DrawData(balloonTexture, drawPosition + icespot.RotatedBy(rotation), null, drawColor*0.4f, rotation, orig, drawScale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0));
				}*/

				playerDrawData.Add(new DrawData(iceBlock, drawPosition + new Vector2(0,drawPlayer.height-22).RotatedBy(rotation), null, drawColor * 0.75f, rotation, iceBlock.Size() / 2f, drawScale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0));			
			}
			// by returning true, the regular drawing will still happen.
			return true;
		}
	}

	public class GiantIceCube : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Ice Cube?");
			Tooltip.SetDefault("'for those hot summer days to cool your feet'\nCannot accelerate, but causes you to slide around at max speed with no friction");
		}

        public override void SetDefaults() {
			item.width = 20;
			item.height = 30;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 1;
			item.value = 30000;
			item.rare = 2;
			item.UseSound = SoundID.Item79;
			item.noMelee = true;
			item.mountType = MountType<IceCubeMount>();
		}
	}

	public class IceCubeMountBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Slide Cube");
			Description.SetDefault("atleast you won't lose your footing! Where your going is another story");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.mount.SetMount(MountType<IceCubeMount>(), player);
			player.buffTime[buffIndex] = 10;
		}
	}

}