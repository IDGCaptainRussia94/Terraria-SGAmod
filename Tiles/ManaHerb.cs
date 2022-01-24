using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;

namespace SGAmod.Tiles
{
	public class ManaHerb : ModTile
	{
        public override bool Autoload(ref string name, ref string texture)
        {
			texture = "Terraria/Tiles_" + 3;

			return base.Autoload(ref name, ref texture);
        }
        public override void SetDefaults() 
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileCut[Type] = true;
			//Main.tileAlch[Type] = true;
			Main.tileNoFail[Type] = true;
			//Main.tileLavaDeath[Type] = true;
			//dustType = -1;
			//disableSmartCursor = true;
			//AddMapEntry(new Color(13, 88, 130), "Banner");
			//TileObjectData.newTile.Width = 1;
			//TileObjectData.newTile.Height = 1;
			//TileObjectData.newTile.Origin = Point16.Zero;
			//TileObjectData.newTile.UsesCustomCanPlace = true;
			//TileObjectData.newTile.CoordinateHeights = new int[]
			//{
			//	20
			//};
			//TileObjectData.newTile.CoordinateWidth = 16;
			//TileObjectData.newTile.CoordinatePadding = 2;
			//TileObjectData.newTile.DrawYOffset = -1;
			//TileObjectData.newTile.StyleHorizontal = true;
			//TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.AlternateTile, TileObjectData.newTile.Width, 0);
			//TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
			//TileObjectData.newTile.LavaDeath = true;
			//TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
			//TileObjectData.addBaseTile(out TileObjectData.StyleAlch);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleAlch);
			TileObjectData.newTile.AnchorValidTiles = new[]
			{
				2, //TileID.Grass
				109, // TileId.HallowedGrass
				TileID.JungleGrass

			};
			TileObjectData.newTile.AnchorAlternateTiles = new[]
			{
				78, //ClayPot
				TileID.PlanterBox
			};
			TileObjectData.addTile(Type);
			soundType = 6;
			//drop = ItemType()
		}
		//public override bool CanPlace(int i, int j)
		//{
		//	return base.CanPlace(i, j);
		//}
		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects) {
			if (i % 2 == 1) {
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
		}

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
				Vector2 zerooroffset = Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange);
				Vector2 drawOffset = new Vector2((float)(i * 16), (float)(j * 16)) + zerooroffset;

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(1,1,1));

			ArmorShaderData shader2 = GameShaders.Armor.GetShaderFromItemId(ItemID.StardustDye);

			Texture2D sun2 = ModContent.GetTexture("SGAmod/TiledPerlin");

			/*
			DrawData value9 = new DrawData(TextureManager.Load("Images/Misc/Perlin"), drawOffset + Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 512, 512)), Microsoft.Xna.Framework.Color.White, 0, new Vector2(256f, 256f), 5f, SpriteEffects.None, 0);
			shader.UseOpacity(0.25f);
			shader.Apply(null, new DrawData?(value9));
			*/

			DrawData value28 = new DrawData(sun2, new Vector2(4, 4), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, sun2.Width, sun2.Height)), Microsoft.Xna.Framework.Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
			shader2.UseColor(Color.Blue);
			shader2.UseOpacity(1f);
			shader2.Apply(null, new DrawData?(value28));


			for (int z = -2; z < 5; z += 4)
			{
				spriteBatch.Draw(Main.tileTexture[Type], drawOffset - Main.screenPosition, new Rectangle(Main.tile[i, j].frameX, Main.tile[i, j].frameY, 16, 16), Color.White, 0f, new Vector2(z, 0), new Vector2(1f, 1f), SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(Main.tileTexture[Type], drawOffset - Main.screenPosition, new Rectangle(Main.tile[i, j].frameX, Main.tile[i, j].frameY, 16, 16), Color.Blue.MultiplyRGB(Lighting.GetColor(i, j)), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.CreateScale(1, 1, 1));


			return false;
        }

        public override bool Drop(int i, int j)
		{
			int stage = Main.tile[i, j].frameX / 18;
				Item.NewItem(i * 16, j * 16, 0, 0, ItemID.Star);
			return false;
		}

		//public override void RightClick(int i, int j)
		//{
		//	base.RightClick(i, j);
		//}
	}
}
