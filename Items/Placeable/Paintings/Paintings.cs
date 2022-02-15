using Idglibrary;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace SGAmod.Items.Placeable.Paintings
{
	public class SGAPlacablePainting : ModItem
	{

		public static void SetupPaintings()
        {

			SGAmod.Instance.AddItem("DergPainting", new SGAPlacablePainting(0, ModContent.TileType<SGAPaintings3X3>(), "Derg", "Phil Bill"));
			SGAmod.Instance.AddItem("CalmnessPainting", new SGAPlacablePainting(0, ModContent.TileType<SGAPaintings6X4>(), "Calmness", "Julia 'Jarts' Goldfox"));
			SGAmod.Instance.AddItem("MeetingTheSunPainting", new SGAPlacablePainting(1, ModContent.TileType<SGAPaintings6X4>(), "Meeting The Sun", "Julia 'Jarts' Goldfox"));
			SGAmod.Instance.AddItem("SerenityPainting", new SGAPlacablePainting(0, ModContent.TileType<SGAPaintings12X7>(), "Serenity", "Fridaflame and XennaDemonorph"));
			SGAmod.Instance.AddItem("AdventurePainting", new SGAPlacablePainting(1, ModContent.TileType<SGAPaintings12X7>(), "Onwards, Adventure!", "DSW7"));
			SGAmod.Instance.AddItem("UnderTheWaterfallPainting", new SGAPlacablePainting(0, ModContent.TileType<SGAPaintings12X8>(), "Under The Waterfall", "Dreamers of the Blue"));
			SGAmod.Instance.AddItem("ParadoxGeneralPainting", new SGAPlacablePainting(0, ModContent.TileType<SGAPaintings15X11>(), "Paradox General", "Speedymatt123, the art depicts a moth dueling against a reality shaping and mind controlling entity"));


		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Idglib.ColorText(Color.PaleTurquoise, "'" + Name2[0] + "'"));
			Tooltip.SetDefault(Idglib.ColorText(Color.PaleGoldenrod, "Illustrated by " + Name2[1]));
		}

		public override bool CloneNewInstances => true;

		string[] Name2;
		int PlaceStyle;
		int tile;

		public SGAPlacablePainting(int PlaceStyle,int tile, string Title, string Artist)
		{
			Name2 = new string[2] {Title, Artist};
			this.tile = tile;
			this.PlaceStyle = PlaceStyle;
		}

		public override void SetDefaults()
		{
			item.useStyle = 1;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.consumable = true;
			item.createTile = tile;
			item.placeStyle = PlaceStyle;
			item.width = 24;
			item.height = 24;
			item.rare = 4;
			item.value = 0;
		}
		public override bool Autoload(ref string name)
		{
			return false;
		}
	}
	public class SGAPaintings15X11 : SGAPaintings3X3
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.Width = 15;
			TileObjectData.newTile.Height = 11;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 8 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.addTile(Type);
			dustType = 7;
			disableSmartCursor = true;
			//ModTranslation name = CreateMapEntryName();
			//AddMapEntry(new Color(200, 200, 200), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int item = 0;
			switch (frameX / (192))
			{
				case 0:
					item = SGAmod.Instance.ItemType("ParadoxGeneralPainting");
					break;

			}
			if (item > 0)
			{
				Item.NewItem(i * 16, j * 16, 48, 48, item);
			}
		}
	}
	public class SGAPaintings12X8 : SGAPaintings3X3
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.Width = 12;
			TileObjectData.newTile.Height = 8;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16, 16, 16, 8 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.addTile(Type);
			dustType = 7;
			disableSmartCursor = true;
			//ModTranslation name = CreateMapEntryName();
			//AddMapEntry(new Color(200, 200, 200), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int item = 0;
			switch (frameX / (192))
			{
				case 0:
					item = SGAmod.Instance.ItemType("UnderTheWaterfallPainting");
					break;

			}
			if (item > 0)
			{
				Item.NewItem(i * 16, j * 16, 48, 48, item);
			}
		}
	}

	public class SGAPaintings12X7 : SGAPaintings3X3
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.Width = 12;
			TileObjectData.newTile.Height = 7;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16, 16, 7 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.addTile(Type);
			dustType = 7;
			disableSmartCursor = true;
			//ModTranslation name = CreateMapEntryName();
			//AddMapEntry(new Color(200, 200, 200), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int item = 0;
			switch (frameX / (192))
			{
				case 0:
					item = SGAmod.Instance.ItemType("SerenityPainting");
					break;
				case 1:
					item = SGAmod.Instance.ItemType("AdventurePainting");
					break;

			}
			if (item > 0)
			{
				Item.NewItem(i * 16, j * 16, 48, 48, item);
			}
		}
	}

	public class SGAPaintings6X4 : SGAPaintings3X3
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.Width = 6;
			TileObjectData.newTile.Height = 4;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.addTile(Type);
			dustType = 7;
			disableSmartCursor = true;
			//ModTranslation name = CreateMapEntryName();
			//AddMapEntry(new Color(200, 200, 200), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int item = 0;
			switch (frameX / 96)
			{
				case 0:
					item = SGAmod.Instance.ItemType("CalmnessPainting");
					break;
				case 1:
					item = SGAmod.Instance.ItemType("MeetingTheSunPainting");
					break;

			}
			if (item > 0)
			{
				Item.NewItem(i * 16, j * 16, 48, 48, item);
			}
		}
	}

	public class SGAPaintings3X3 : ModTile
	{
		public override void SetDefaults() {
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.addTile(Type);
			dustType = 7;
			disableSmartCursor = true;
			//ModTranslation name = CreateMapEntryName();
			//AddMapEntry(new Color(200, 200, 200), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY) {
			int item = 0;
			switch (frameX / 54) 
			{
				case 0:
					item = SGAmod.Instance.ItemType("DergPainting");
					break;

			}
			if (item > 0) 
			{
				Item.NewItem(i * 16, j * 16, 48, 48, item);
			}
		}
	}
}