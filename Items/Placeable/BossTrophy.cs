using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Placeable
{
	public class BossTrophy : ModItem
	{
		private int placeStyle;
		private string trophySprite = "SGAmod/Items/Placeable/BossTrophy";
		private string bossSprite = "SGAmod/Invisible";
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.value = Item.buyPrice(gold: 1);
			item.rare = 1;
			item.createTile = mod.TileType("BossTrophies");
			item.placeStyle = placeStyle;
		}
        public override string Texture => trophySprite;

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
			Texture2D texture = ModContent.GetTexture(bossSprite);
			Vector2 slotSize = new Vector2(52f, 52f);
			position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
			Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
			Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			spriteBatch.Draw(texture, drawPos, null, Color.White, 0f, textureOrigin, Main.inventoryScale/2f, SpriteEffects.None, 0f);
		}

        public override bool CloneNewInstances => true;

		public BossTrophy() { } // An empty constructor is needed for tModLoader to attempt Autoload
		public BossTrophy(int placeStyle, string bossSprite = "SGAmod/Invisible", string trophySprite="SGAmod/Items/Placeable/BossTrophy") // This is the real constructor we use in Autoload
		{
			//this.trophySprite = "SGAmod/Items/Placeable/"+GetType().Name;
			this.placeStyle = placeStyle;
			this.bossSprite = bossSprite;
			this.trophySprite = trophySprite;
		}

		public override bool Autoload(ref string name)
        {
			mod.AddItem("MaggotBanner", new SGABanner("Maggot"));
			mod.AddItem("MaggotFlyBanner", new SGABanner("MaggotFly"));
			mod.AddItem("BlackLeechBanner", new SGABanner("BlackLeech"));
			mod.AddItem("DankMimicBanner", new SGABanner("DankMimic"));
			mod.AddItem("DankSlimeBanner", new SGABanner("DankSlime"));
			mod.AddItem("FlyBanner", new SGABanner("Fly"));
			mod.AddItem("FlySwarmBanner", new SGABanner("FlySwarm"));
			mod.AddItem("GiantLizardBanner", new SGABanner("GiantLizard"));
			mod.AddItem("IceFairyBanner", new SGABanner("IceFairy"));
			mod.AddItem("MudBallBanner", new SGABanner("MudBall"));
			mod.AddItem("MudMummyBanner", new SGABanner("MudMummy"));
			mod.AddItem("SandscorchedGolemBanner", new SGABanner("SandscorchedGolem"));
			mod.AddItem("SandscorchedSlimeBanner", new SGABanner("SandscorchedSlime"));
			mod.AddItem("SwampBatBanner", new SGABanner("SwampBat"));
			mod.AddItem("SwampJellyBanner", new SGABanner("SwampJelly"));
			mod.AddItem("TidalElementalBanner", new SGABanner("TidalElemental"));
			mod.AddItem("SkeletonCrossbowerBanner", new SGABanner("SkeletonCrossbower"));
			mod.AddItem("SkeletonGunnerBanner", new SGABanner("SkeletonGunner"));
			mod.AddItem("DungeonBatBanner", new SGABanner("DungeonBat"));
			mod.AddItem("ChaosCasterBanner", new SGABanner("ChaosCaster"));
			mod.AddItem("EvilCasterBanner", new SGABanner("EvilCaster"));
			mod.AddItem("FastSkeletonBanner", new SGABanner("FastSkeleton"));
			mod.AddItem("FlamingSkullBanner", new SGABanner("FlamingSkull"));
			mod.AddItem("HellCasterBanner", new SGABanner("HellCaster"));
			mod.AddItem("LaserSkeletonBanner", new SGABanner("LaserSkeleton"));
			mod.AddItem("RuneCasterBanner", new SGABanner("RuneCaster"));

			mod.AddItem("CopperWraithTrophy", new BossTrophy(0, trophySprite: "SGAmod/Items/Placeable/CopperWraithTrophy"));
			mod.AddItem("CaliburnATrophy", new BossTrophy(1, trophySprite: "SGAmod/Items/Placeable/CaliburnATrophy"));
			mod.AddItem("CaliburnBTrophy", new BossTrophy(2, trophySprite: "SGAmod/Items/Placeable/CaliburnBTrophy"));
			mod.AddItem("CaliburnCTrophy", new BossTrophy(3, trophySprite: "SGAmod/Items/Placeable/CaliburnCTrophy"));
			mod.AddItem("SpiderQueenTrophy", new BossTrophy(4, trophySprite: "SGAmod/Items/Placeable/SpiderQueenTrophy"));
			mod.AddItem("MurkTrophy", new BossTrophy(5, trophySprite: "SGAmod/Items/Placeable/MurkTrophy"));

			mod.AddItem("CirnoTrophy", new BossTrophy(6, trophySprite: "SGAmod/Items/Placeable/CirnoTrophy"));
			mod.AddItem("CobaltWraithTrophy", new BossTrophy(7, trophySprite: "SGAmod/Items/Placeable/CobaltWraithTrophy"));
			mod.AddItem("SharkvernTrophy", new BossTrophy(8, trophySprite: "SGAmod/Items/Placeable/SharkvernTrophy"));
			mod.AddItem("CratrosityTrophy", new BossTrophy(9, trophySprite: "SGAmod/Items/Placeable/CratrosityTrophy"));
			mod.AddItem("TwinPrimeDestroyersTrophy", new BossTrophy(10, bossSprite: "SGAmod/Items/Placeable/BossTrophy_TPD"));
			mod.AddItem("DoomHarbingerTrophy", new BossTrophy(11, trophySprite: "SGAmod/Items/Placeable/DoomHarbingerTrophy"));

			mod.AddItem("LuminiteWraithTrophy", new BossTrophy(12, trophySprite: "SGAmod/Items/Placeable/LuminiteWraithTrophy"));
			mod.AddItem("CratrogeddonTrophy", new BossTrophy(13, trophySprite: "SGAmod/Items/Placeable/CratrogeddonTrophy"));
			mod.AddItem("SupremePinkyTrophy", new BossTrophy(14, trophySprite: "SGAmod/Items/Placeable/SupremePinkyTrophy"));
			mod.AddItem("HellionTrophy", new BossTrophy(15, trophySprite: "SGAmod/Items/Placeable/HellionTrophy"));
			mod.AddItem("PhaethonTrophy", new BossTrophy(16, trophySprite: "SGAmod/Items/Placeable/PhaethonTrophy"));

			return false;
        }
    }

}