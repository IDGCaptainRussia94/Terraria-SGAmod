using System.IO;
using System;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.GameContent.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.World.Generation;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria.Utilities;
using Idglibrary;
using SubworldLibrary;
using SGAmod.Dimensions.NPCs;
using SGAmod.Effects;
using SGAmod.Items;

namespace SGAmod.Dimensions
{
    public class LimboDim : SGAPocketDim
    {
        public override int width => 2400;
        public override int height => 1400;
        public override bool saveSubworld => false;
        public override string DimName => "Limbo";
        public override UserInterface loadingUI => base.loadingUI;
        public override UIState loadingUIState => new LimborinthLoad();

        public static Texture2D[] staticeffects=new Texture2D[20];
        public override float maxSpawns => 5f;
        public override float spawnRate => 0.15f;

        public override int? Music
        {

            get
            {
                return SGAmod.Instance.GetSoundSlot(SoundType.Music, "Sounds/Music/creepy");
            }

        }

        public override int DimType => 1;

        public virtual void AGenPass(GenerationProgress prog)
        {

            UnifiedRandom UniRand = new UnifiedRandom(DimDungeonsProxy.DungeonSeeds);
            int lastseed = WorldGen._genRandSeed;
            WorldGen._genRandSeed = DimDungeonsProxy.DungeonSeeds;
            enemyseed = (DimDungeonsProxy.DungeonSeeds);
            prog.Message = "Loading"; //Sets the text above the worldgen progress bar
            Main.worldSurface = Main.maxTilesY - 2; //Hides the underground layer just out of bounds
            Main.rockLayer = Main.maxTilesY; //Hides the cavern layer way out of bounds
            //Main.spawnTileX = (Main.maxTilesX / 2) / 16;
            prog.Message = "Once was nothing...";
            int tileheight = UniRand.Next(250, 400);
            List<int> surfacelevel = new List<int>();
            int updown= UniRand.Next(0, 2)==0 ? 1 : -1;

            EnemySpawnsOverride = delegate (IDictionary<int, float> pool, NPCSpawnInfo spawnInfo, SGAPocketDim pocket)
            {
                UnifiedRandom UniRand2 = new UnifiedRandom(pocket.enemyseed);
                for (int i = 0; i < pool.Count; i += 1)
                {
                    pool[i] = 0f;

                }
                pool[ModContent.NPCType<NullWatcher>()] = 1f;
                pool[ModContent.NPCType<StygianVein>()] = 0.05f;

                pocket.chooseenemies = true;
                return 1;
            };

            //Base Terrain
            for (int x = 0; x < Main.maxTilesX; x += 1)
            {
                for (int y = tileheight; y < Main.maxTilesY; y += 1)
                {
                    Tile thetile = Framing.GetTileSafely(x, y);
                    thetile.active(true);
                    thetile.type = (ushort)SGAmod.Instance.TileType("Fabric");
                }
                if (Math.Abs(x - Main.maxTilesX / 2) < 30) {
                    Main.spawnTileY = tileheight;
                }
                else
                {
                if (UniRand.Next(0, 10) == 1)
                    {
                        tileheight += UniRand.Next(1, 3)* updown;
                        if (UniRand.Next(0, 4) == 1)
                            updown = UniRand.Next(0, 2) == 0 ? 1 : -1;
                    }
                }
                if (surfacelevel.Count < x + 1)
                {
                    surfacelevel.Add(tileheight);
                }
            }


                for (int i = 0; i < 100; i += 1)
                {
                int randomx = UniRand.Next(Main.maxTilesX);
                 int randomy = UniRand.Next(surfacelevel[randomx]+ UniRand.Next(50,100), Main.maxTilesY);
                IDGWorldGen.TileRunner(randomx, randomy, (double)UniRand.Next(5, 15), UniRand.Next(5, 15), SGAmod.Instance.TileType("EntrophicOre"), false, 0f, 0f, false, true, UniRand);
                }

                for (int i = 0; i < 300; i += 1)
                {
                int randomx = UniRand.Next(Main.maxTilesX);
                 int randomy = UniRand.Next(surfacelevel[randomx]+(i%20==0 ? -10 : 60), Main.maxTilesY);
                IDGWorldGen.TileRunner(randomx, randomy, (double)UniRand.Next(10, 50), UniRand.Next(15, 45)+i, -2, false, 0f, 0f, false, true, UniRand);
                }

                for (int i = 0; i < 150; i += 1)
                {
                int randomx = UniRand.Next(Main.maxTilesX);
                int randomy = UniRand.Next(surfacelevel[randomx] + UniRand.Next(20, 160), Main.maxTilesY);
                IDGWorldGen.TileRunner(randomx, randomy, (double)UniRand.Next(3, 6), UniRand.Next(2, 4), SGAmod.Instance.TileType("HopeOre"), false, 0f, 0f, false, true, UniRand);
                }

            //Noisegen.Frequency

            surfacelevel.Clear();


            //Ancient Fabric and fill in
        tileheight = Main.maxTilesY - Main.rand.Next(400,400);

            for (int x = Main.maxTilesX-1; x > 0; x -= 1)
            {
                Tile thetile = Framing.GetTileSafely(x, tileheight);
                IDGWorldGen.TileRunner(x, tileheight, (double)UniRand.Next(5, 10), UniRand.Next(2, 4), SGAmod.Instance.TileType("AncientFabric"), true, 0f, 0f, false, true, UniRand);
                thetile.active(true);
                thetile.type = (ushort)SGAmod.Instance.TileType("AncientFabric");

                for (int i = tileheight; i < Main.maxTilesY; i += 1)
                {
                    Framing.GetTileSafely(x, i).wall = (ushort)SGAmod.Instance.WallType("NullWall");
                }

                    if (UniRand.Next(0, 10) == 1)
                {
                    tileheight += UniRand.Next(1, 3) * updown;
                    if (UniRand.Next(0, 4) == 1)
                        updown = (UniRand.Next(0, 2) == 0 || tileheight > Main.maxTilesY-300) ? -1 : 1;
                }
            }

            NoiseGenerator Noisegen = new NoiseGenerator(DimDungeonsProxy.DungeonSeeds);

            Noisegen.Amplitude = 1;
            Noisegen.Octaves = 4;
            Noisegen.Persistence = 0.750;
            Noisegen.Frequency *= 1.25;


            for (int y = 0; y < Main.maxTilesY; y += 1)
            {
                for (int x = 0; x < Main.maxTilesX; x += 1)
                {
                    float tilerate = -0.25f + (1f - (y / (float)Main.maxTilesY)) * 1f;
                    Tile tile = Main.tile[x, y];
                    if (tile.active() && tile.type == (ushort)SGAmod.Instance.TileType("Fabric"))
                    {
                        float nousey = MathHelper.Clamp((float)Noisegen.Noise(x, y), -1.00f, 1.00f);
                        if (nousey > tilerate)
                        {
                            Main.tile[x, y].type = (ushort)SGAmod.Instance.TileType("HardenedFabric");
                            Main.tile[x, y].color((byte)FakeOverworld.Paints.Shadow);
                        }
                        else
                        {
                            if (nousey > tilerate - 0.20f)
                            {
                                Main.tile[x, y].color((byte)FakeOverworld.Paints.Gray);
                            }
                        }
                    }

                }
            }


            WorldGen._genRandSeed = lastseed;

        }
        public override List<GenPass> tasks { get; }

        public LimboDim()
        {
            tasks = new List<GenPass>();

            tasks.Add(new SubworldGenPass(2f, progress =>
            {
                progress.Message = "Loading"; //Sets the text above the worldgen progress bar
                Main.worldSurface = Main.maxTilesY - 2; //Hides the underground layer just out of bounds
                Main.rockLayer = Main.maxTilesY + 2; //Hides the cavern layer way out of bounds

                AGenPass(progress);


            }));

        }

        //        new SubworldGenPass("Loading", 1f, AGenPass)
        //                        new SubworldGenPass("Loading", 1f, genpass)

        public override void Load()
        {
            Main.dayTime = false;
            Main.time = 40000;
        }


		public static void CreateTextures()
		{
            if (!Main.dedServ)
            {
                for (int index = 0; index < staticeffects.Length; index++)
                {

                    int width = 16; int height = 16;
                    LimboDim.staticeffects[index] = new Texture2D(Main.graphics.GraphicsDevice, width, height);
                    Color[] dataColors = new Color[width * height];


                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x += 1)
                        {
                            if (Main.rand.Next(0, 20) == 1)
                                dataColors[(int)x + y * width] = Main.hslToRgb(Main.rand.NextFloat(0f, 1f) % 1f, 0.5f, 0.5f);
                        }

                    }

                    LimboDim.staticeffects[index].SetData(dataColors);
                }
            }

		}

        public static void DestroyTextures()
        {
            if (!Main.dedServ)
            {
                for (int index = 0; index < staticeffects.Length; index++)
                {
                    if (LimboDim.staticeffects[index] != null)
                        LimboDim.staticeffects[index].Dispose();
                }

            }
        }



	}


        public class LimboSky : CustomSky
	{
		private Random _random = new Random();
		private bool _isActive;
		private float[] xoffset = new float[200];
		private Color acolor = Color.Gray;
        Effect effect => SGAmod.TrailEffect;


        public override void OnLoad()
		{
		}

		public override void Update(GameTime gameTime)
		{
			acolor = Main.hslToRgb(0f, 0.0f, 0.5f);
		}

		public override Color OnTileColor(Color inColor)
		{
			return Main.hslToRgb(0,0,0.01f);
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{

            /*basicEffect.World = WVP.World();
            basicEffect.View = WVP.View(Main.GameViewMatrix.Zoom);
            basicEffect.Projection = WVP.Projection();
            basicEffect.VertexColorEnabled = true;
            basicEffect.TextureEnabled = true;
            basicEffect.Texture = SGAmod.ExtraTextures[21];*/

            VertexBuffer vertexBuffer;
            Vector2 parallex = new Vector2(Main.screenPosition.X / 9000f, -Main.GlobalTime * 0.1f);
            Color skycolor = Color.Red*0.75f;

            effect.Parameters["WorldViewProjection"].SetValue(WVP.View(Main.GameViewMatrix.Zoom) * WVP.Projection());
            effect.Parameters["imageTexture"].SetValue(SGAmod.Instance.GetTexture("Space"));
            effect.Parameters["coordOffset"].SetValue(parallex);
            effect.Parameters["coordMultiplier"].SetValue(4f);
            effect.Parameters["strength"].SetValue(1f);

            VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[12];

            Vector3 screenPos = new Vector3(-16, 0, 0);
            float skymove = ((Math.Max(Main.screenPosition.Y - 8000, 0)) / (Main.maxTilesY * 16f));

            Vector3 screenPosParallex = screenPos+new Vector3(0, -Main.screenHeight * (skymove) /2f, 0);

            vertices[0] = new VertexPositionColorTexture(screenPos + new Vector3(-16, 0, 0), Color.Black, new Vector2(0, 0));
            vertices[1] = new VertexPositionColorTexture(screenPos + new Vector3(-16, Main.screenHeight, 0), Color.Black, new Vector2(0, 0));
            vertices[2] = new VertexPositionColorTexture(screenPos + new Vector3(Main.screenWidth + 16, 0, 0), Color.Black, new Vector2(0, 0));

            vertices[3] = new VertexPositionColorTexture(screenPos + new Vector3(Main.screenWidth + 16, Main.screenHeight, 0), Color.Black, new Vector2(0, 0));
            vertices[4] = new VertexPositionColorTexture(screenPos + new Vector3(-16, Main.screenHeight, 0), Color.Black, new Vector2(0, 0));
            vertices[5] = new VertexPositionColorTexture(screenPos + new Vector3(Main.screenWidth + 16, 0, 0), Color.Black, new Vector2(0, 0));

            vertices[6] = new VertexPositionColorTexture(screenPosParallex + new Vector3(-16, 0, 0), skycolor, new Vector2(0, 0));
            vertices[7] = new VertexPositionColorTexture(screenPosParallex + new Vector3(-16, Main.screenHeight / 2f, 0), Color.Transparent, new Vector2(0, 0.5f));
            vertices[8] = new VertexPositionColorTexture(screenPosParallex + new Vector3(Main.screenWidth+16, 0, 0), skycolor, new Vector2(1, 0));

            vertices[9] = new VertexPositionColorTexture(screenPosParallex + new Vector3(Main.screenWidth + 16, Main.screenHeight/2f, 0), Color.Transparent, new Vector2(1, 0.5f));
            vertices[10] = new VertexPositionColorTexture(screenPosParallex + new Vector3(-16, Main.screenHeight/2f, 0), Color.Transparent, new Vector2(0, 0.5f));
            vertices[11] = new VertexPositionColorTexture(screenPosParallex + new Vector3(Main.screenWidth + 16, 0, 0), skycolor, new Vector2(1, 0));


            vertexBuffer = new VertexBuffer(Main.graphics.GraphicsDevice, typeof(VertexPositionColorTexture), vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColorTexture>(vertices);

            Main.graphics.GraphicsDevice.SetVertexBuffer(vertexBuffer);

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

            effect.CurrentTechnique.Passes["BasicEffectPass"].Apply();
            Main.graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);

            effect.CurrentTechnique.Passes["FadedBasicEffectPass"].Apply();
            Main.graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 6, 2);

            if (maxDepth >= 0 && minDepth < 0)
            {
                //spriteBatch.Draw(Main.blackTileTexture, Vector2.Zero, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), (Color.Black * 0.8f), 0, Vector2.Zero, new Vector2(1f, 1f), SpriteEffects.None, 0f);
            }


            if (maxDepth >= 0 && minDepth < 0)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                ArmorShaderData shader2 = GameShaders.Armor.GetShaderFromItemId(ItemID.VoidDye); shader2.Apply(null);
                Texture2D sun = SGAmod.ExtraTextures[100];

                Texture2D inner = Main.itemTexture[ModContent.ItemType<AssemblyStar>()];

                Vector2 position = new Vector2((Main.screenWidth / 2), (Main.screenHeight / 8)-((Main.screenHeight*16)*skymove*0.04f));

                Vector2 textureOrigin = new Vector2(inner.Width / 2, inner.Height / 2);

                for (float i = 0; i < 1f; i += 0.10f)
                {
                    spriteBatch.Draw(inner, position, null, (Color.DarkMagenta * (1f - ((i + (Main.GlobalTime / 2f)) % 1f)) * 0.5f) * 0.50f, i * MathHelper.TwoPi, textureOrigin, 16f * (0.5f + 1.25f * (((Main.GlobalTime / 2f) + i) % 1f)), SpriteEffects.None, 0f);
                }

                spriteBatch.Draw(sun, position, null, Color.DarkRed, 0, sun.Size() / 2f, 3f, SpriteEffects.None, 0f);

                //spriteBatch.Draw(sun, new Vector2(Main.screenWidth / 2, Main.screenHeight / 8), null, Color.Red, 0, new Vector2(sun.Width / 2f, sun.Height / 2f), new Vector2(5f, 5f), SpriteEffects.None, 0f);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);

            UnifiedRandom alwaysthesame = new UnifiedRandom(DimDungeonsProxy.DungeonSeeds);

            Texture2D texx = ModContent.GetTexture("SGAmod/Items/WatchersOfNull");

            for (float i = 0.04f; i < 0.35f; i += 0.005f)
            {
                for (int x = -alwaysthesame.Next(900, 3200); x < ((Main.maxTilesX * 16)*i)+Main.screenWidth; x += alwaysthesame.Next(900, 3200))
                {
                    for (int y = -alwaysthesame.Next(900, 3200); y < ((Main.maxTilesY * 16)*i)+Main.screenHeight; y += alwaysthesame.Next(900, 3200))
                    {
                        Vector2 loc = ((-Main.screenPosition * i)+ new Vector2(x, y));
                        if (loc.X>-64 && loc.Y > -64 && loc.X < Main.screenWidth+ 64 && loc.Y < Main.screenHeight + 64)
                        {
                            spriteBatch.Draw(texx, loc, new Rectangle(0, 0, texx.Width, texx.Height / 13), (Color.White * MathHelper.Clamp(i,0f,1f)), 0, Vector2.Zero, new Vector2(1f, 1f), SpriteEffects.None, 0f);
                        }

                    }
                }
            }

            Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
		}

		public override float GetCloudAlpha()
		{
			return 0f;
		}

		public override void Activate(Vector2 position, params object[] args)
		{
			this._isActive = true;
		}

		public override void Deactivate(params object[] args)
		{
			this._isActive = false;
		}

		public override void Reset()
		{
			this._isActive = false;
		}

		public override bool IsActive()
		{
			return this._isActive;
		}
	}
}
