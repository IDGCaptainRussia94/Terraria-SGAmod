using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SGAmod.Items.Placeable.Relics
{
	public class SGAPlacableRelic : ModItem
	{
		//private string bossString = "Copper_Wraith";
		private string trophySprite = "SGAmod/Items/Placeable/Relics/Relic_Item_";
		private string tileSprite = "SGAmod/Items/Placeable/Relics/Tiles/Relic_Placed_";

		private string tileName = "";
		private string name = "";
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
			item.rare = ItemRarityID.Yellow;
			item.createTile = mod.TileType(tileName);
		}
		public override string Texture => trophySprite;
		public override bool CloneNewInstances => true;
		public SGAPlacableRelic() { } // An empty constructor is needed for tModLoader to attempt Autoload
		public SGAPlacableRelic(string name, string bossSprite = "Copper_Wraith") // This is the real constructor we use in Autoload
		{
			this.trophySprite = trophySprite + bossSprite;
			this.tileName = "Relic_Placed_" + bossSprite;
			this.name = name.Replace("Relic_Item_", "").Replace("_", " ");
			this.name = this.name.Replace("SPinky","Supreme Pinky");
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Relic: " + name);
		}

		public static Color RelicColor => Color.Lerp(Color.Red, Color.Lerp(Color.Red, Color.White, 0.75f), 0.5f + (float)Math.Sin(Main.GlobalTime));

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (TooltipLine line in tooltips)
			{
				line.overrideColor = RelicColor;

			}
			tooltips.Add(new TooltipLine(mod, "Relic", "Awarded for No-Hitting " + name));

		}

		public static List<string> addedRelics;

		public static void AttemptDropRelic(ModNPC modnpc)
		{
			ISGABoss iboss = modnpc as ISGABoss;
			string itemtype = "Relic_Item_" + iboss.RelicName();

			if (itemtype == "Relic_Item_Caliburn")
			{
				itemtype = "Relic_Item_Caliburn_A";
				if (modnpc.npc.ai[2] == 1)
					itemtype = "Relic_Item_Caliburn_B";
				if (modnpc.npc.ai[2] == 2)
					itemtype = "Relic_Item_Caliburn_C";
			}

			Main.NewText(itemtype);
			Item.NewItem(modnpc.npc.position, modnpc.npc.Hitbox.Size(), SGAmod.Instance.ItemType(itemtype));
		}

		public static void AddRelics()
		{
			addedRelics = new List<string>();
			Assembly ass = SGAmod.Instance.Code;
			foreach (Type typeoff in ass.GetTypes())
			{
				//SGAmod.Instance.Logger.Debug("Checking assembly: "+ typeoff.Name);
				if (typeoff.GetInterfaces().Contains(typeof(ISGABoss)))
				{
					//ISGABoss iboss = (ISGABoss)typeoff.GetInterface("ISGABoss");
					ISGABoss iboss = (ass.CreateInstance(typeoff.FullName) as ISGABoss);

					//SGAmod.Instance.Logger.Debug("--Found--");
					string relicstr = iboss.RelicName();

					if (relicstr == "NOU")
						continue;

					if (relicstr == "Caliburn")
					{
						AttemptAddRelic("Caliburn_A");
						AttemptAddRelic("Caliburn_B");
						AttemptAddRelic("Caliburn_C");
						continue;
					}

					AttemptAddRelic(relicstr);
				}
			}
		}

		public static void AttemptAddRelic(string npcname)
		{
			string relicbase = "SGAmod/Items/Placeable/Relics/Tiles/RelicBase";
			//ISGABoss boss = mnpc as ISGABoss;

			string itemName = "Relic_Item_" + npcname;
			string tileName = "Relic_Placed_" + npcname;

			if (!addedRelics.Contains(itemName))
			{
				addedRelics.Add(itemName);

				SGAmod sga = SGAmod.Instance;
				sga.AddItem("Relic_Item_" + npcname, new SGAPlacableRelic("Relic_Item_" + npcname, npcname));
				sga.AddTile("Relic_Placed_" + npcname, new SGARelicTile(), relicbase);
			}

		}

		public override bool Autoload(ref string name)
		{

			//mod.AddItem("CopperWraithTrophy", new BossTrophy(0, trophySprite: "SGAmod/Items/Placeable/CopperWraithTrophy"));
			/*mod.AddItem("CaliburnATrophy", new BossTrophy(1, trophySprite: "SGAmod/Items/Placeable/CaliburnATrophy"));
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
			mod.AddItem("HellionTrophy", new BossTrophy(15, trophySprite: "SGAmod/Items/Placeable/HellionTrophy"));*/

			return false;
		}
	}

	public class SGARelicTile : ModTile
	{

		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = false;
			Main.tileTable[Type] = false;
			Main.tileShine2[Type] = true;
			dustType = DustID.Grass;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 18 };
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.StyleHorizontal = false;
			TileID.Sets.CanBeClearedDuringGeneration[Type] = false;
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Relic");
			AddMapEntry(new Color(227, 216, 195), name);
		}

		public override void DrawEffects(int x, int y, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
		{
			if (Main.tile[x, y].type == base.Type)
			{
				if (nextSpecialDrawIndex < Main.specX.Length)
				{
					Main.specX[nextSpecialDrawIndex] = x;
					Main.specY[nextSpecialDrawIndex] = y;
					nextSpecialDrawIndex += 1;
				}
			}
		}

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			if (Main.tile[i, j].type == base.Type)
			{
				//Main.NewText("test");
				string texstr = "SGAmod/Items/Placeable/Relics/Tiles/RelicBase";//"SGAmod/Tiles/BiomassBarTile"


				Texture2D tex = ModContent.GetTexture(texstr);

				Vector2 zerooroffset = Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange);
				Vector2 location = (new Vector2(i, j) * 16) + zerooroffset + new Vector2(0, 2);

				if (Main.tile[i, j].frameX == 0 && Main.tile[i, j].frameY == 0)
				{

					Main.spriteBatch.End();
					Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(1, 1, 1));

					//SGAmod.FadeInEffect.Parameters["fadeColor"].SetValue(Color.Goldenrod.ToVector3());
					//SGAmod.FadeInEffect.CurrentTechnique.Passes["ColorToAlphaPass"].Apply();

					//Action<SpriteBatch, Vector2> drawCode = delegate (SpriteBatch spriteBatch2, Vector2 location2)
					//{
					Texture2D star = ModContent.GetTexture("SGAmod/Extra_57b");

					Terraria.Utilities.UnifiedRandom rando = new Terraria.Utilities.UnifiedRandom(i + j);

					Vector2 starterloc = location + new Vector2(24, 32);

					Vector2 sizeStar = star.Size() / 2f;

					List<(Vector2, float, float)> entries = new List<(Vector2, float, float)>();

					for (float ix = 0; ix < 1f; ix += 1/30f)
					{
						float shiftOut = -1f + ix * 2f;
						float timePercent = (Main.GlobalTime + (ix))%1f;
						Vector2 newLoc = new Vector2(rando.NextFloat(-8f, 8f)+ shiftOut*4f, -rando.NextFloat(6f, 28f)) * timePercent;
						Vector2 newLoc2 = new Vector2(rando.NextFloat(-12f, 12f)+shiftOut * 8f, 4);
						float rotation = Main.GlobalTime*(rando.NextFloat(-1f, 1f)*(rando.NextBool() ? 1f : -1f)*0.025f);

						entries.Add((newLoc+ newLoc2, timePercent, rotation));
					}

					foreach((Vector2, float, float) tuple in entries.OrderBy(testby => testby.Item1.Y))
                    {
						spriteBatch.Draw(star, starterloc + tuple.Item1 - Main.screenPosition, null, Color.Goldenrod* (1f- tuple.Item2), tuple.Item3, sizeStar, new Vector2(1f, 1f)*0.25f, SpriteEffects.None, 0f);
					}

					//Main.spriteBatch.End();
					//Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(1, 1, 1));

					Texture2D tex2 = ModContent.GetTexture("SGAmod/Extra_60b");
					spriteBatch.Draw(tex2, starterloc - Main.screenPosition, null, Color.Goldenrod, 0f, tex2.Size() / 2f, new Vector2(0.80f,1f), SpriteEffects.None, 0f);
					//};

					//CustomSpecialDrawnTiles cdt = new CustomSpecialDrawnTiles(location);
					//cdt.CustomDraw = drawCode;
					//SGAmod.BeforeTilesAdditive.Add(cdt);

					Main.spriteBatch.End();
					Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.CreateScale(1, 1, 1));
				}

				spriteBatch.Draw(tex, location - Main.screenPosition, new Rectangle(Main.tile[i, j].frameX, Main.tile[i, j].frameY + 18, 16, 16), Lighting.GetColor(i, j), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			return true;
		}

		public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
		{
			if (Main.tile[i, j].type == base.Type && Main.tile[i, j].frameX == 0 && Main.tile[i, j].frameY == 0)
			{
				Tile tile = Main.tile[i, j];
				string texstr = "SGAmod/Items/Placeable/Relics/Tiles/" + this.Name;
				Texture2D tex = ModContent.GetTexture(texstr);
				Vector2 zerooroffset = Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange);
				Vector2 location = (new Vector2(i, j) * 16) + new Vector2(8, 8) + zerooroffset;
				float sinwave = (float)Math.Sin((Main.GlobalTime * 2f) + (i / 2.79f) + (j / 1.353f));
				Vector2 offset2 = new Vector2(16, -6 + sinwave * 6f);

				Vector2 endloc = location + offset2;

				spriteBatch.Draw(tex, new Vector2((int)endloc.X, (int)endloc.Y) - Main.screenPosition, null, Color.White, 0f, tex.Size() / 2f, 1f, SpriteEffects.None, 0f);
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(1, 1, 1));

				float timer = ((sinwave / 4f)+ 0.5f) %1f;

				SGAmod.FadeInEffect.Parameters["fadeColor"].SetValue(2f);
				SGAmod.FadeInEffect.Parameters["alpha"].SetValue(MathHelper.Clamp(0f + (float)Math.Sin(timer * MathHelper.TwoPi) * 1f, 0f, 1f) * 0.75f);
				SGAmod.FadeInEffect.CurrentTechnique.Passes["ColorToAlphaPass"].Apply();
				
				spriteBatch.Draw(tex, new Vector2((int)endloc.X, (int)endloc.Y) - Main.screenPosition, null, Color.White*1f, 0f, tex.Size() / 2f, 1.00f+ (timer)*0.32f, SpriteEffects.None, 0f);
				//*MathHelper.Clamp(0.5f+(float)Math.Sin(Main.GlobalTime)*0.75f,0f,1f)*0.20f
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.CreateScale(1, 1, 1));

			}

			//SGAmod.BeforeTilesAdditive
		}

		public override bool Autoload(ref string name, ref string texture)
        {
			//texture = "SGAmod/Items/Placeable/Relics/Tiles/RelicBase";
			return false;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
			Item.NewItem(new Vector2(i*16,j*16),new Vector2(16,16), mod.ItemType("Relic_Item_" + (Name.Replace("Relic_Placed_", ""))));
		}
    }

}