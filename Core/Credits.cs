using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace SGAmod.Credits
{
    public static class CreditsManager
    {
        public static List<CreditsLine> credits = new List<CreditsLine>();
        public static List<CreditsLine> creditsToSpawn = new List<CreditsLine>();
        public static RenderTarget2D creditsRenderTarget;
        public static Texture2D ScreenTexture;
        public static bool queuedCredits = false;
        public static float ScrollSpeed
        {
            get
            {
                Microsoft.Xna.Framework.Input.KeyboardState keyState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
                float speed = 1f * (keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down) ? 0.25f : 1f) * (keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up) ? 10f : 1f);
                return speed;
            }
        }

        public static bool creditsRolling = false;
        public static float delayTimer = 60;
        public static float colorAnimation = 1f;
        public static int timePassed = 0;
        public static float previousMusicVolume=1f;
        public static bool CreditsActive
        {
            get
            {
                bool rolling = creditsRolling;
                if (rolling && credits.Count < 1 && creditsToSpawn.Count < 1)
                {
                    EndCredits();
                    creditsRolling = false;
                    return false;
                }

                return rolling;
            }
        }

        public static void AddCreditEntries()
        {
            int Width = Main.screenWidth/2;
            int Height = Main.screenHeight + 48;
            Vector2 top = new Vector2(Width, Height);

            CreditsLine line = new CreditsLine(("", "", ""), top+new Vector2(0,96));
            line.delayTimer = 55;
            line.textDistance = 100;
            line.bufferSpace = 200;
            line.customDrawData = delegate (CreditsLine liner)
            {
                Texture2D logo = SGAmod.Instance.GetTexture("logo_space2_double");
                Main.spriteBatch.Draw(logo, liner.position, null, Color.White, 0, logo.Size() / 2f, 1f, default, 0);
            };
            line.font = Main.fontDeathText;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("", "A mod for Terraria", ""), top);
            line.delayTimer = 40;
            line.textDistance = 0;
            line.font = Main.fontDeathText;
            creditsToSpawn.Add(line);

            line = new CreditsLineRainbowFlavor(("IDGCaptainRussia94", "Owner, Director, Lead Coder", "'I'm not weird, your too normal'"), top);
            line._colors.Item3 = Color.Lime;
            line.bufferSpace += 64;
            line.customDrawData = delegate (CreditsLine liner)
            {
                int frame = (int)(CreditsManager.timePassed/7f) % 7;

                Texture2D Draken = Terraria.ModLoader.ModContent.GetTexture("SGAmod/NPCs/TownNPCs/TrueDergon");
                Vector2 frameSize = new Vector2(Draken.Width, Draken.Height);

                Rectangle rect = new Rectangle(0, (int)(frame * (frameSize.Y / 7)), (int)frameSize.X, (int)(frameSize.Y / 7));

                Main.spriteBatch.Draw(Draken, liner.position + new Vector2(0, -24), rect, Color.White, 0, rect.Size() / 2f, 1f, default, 0);

                Texture2D dev = SGAmod.Instance.GetTexture("Items/Weapons/DragonCommanderStaff");
                Main.spriteBatch.Draw(dev, liner.position + new Vector2(0, 122), null, Color.White, 0, dev.Size() / 2f, 1f, SpriteEffects.None, 0);
                dev = SGAmod.Instance.GetTexture("Items/Weapons/DragonRevolver");
                Main.spriteBatch.Draw(dev, liner.position + new Vector2(120, 50), null, Color.White, 0, dev.Size() / 2f, 1f, default, 0);
                dev = SGAmod.Instance.GetTexture("Items/Armors/Dev/IDGSet");
                Main.spriteBatch.Draw(dev, liner.position + new Vector2(-108, 54), null, Color.White, 0, dev.Size() / 2f, 1f, default, 0);

            };            
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Rijam", "Lead Spriter, composer, wiki editor", "'Literal 2nd hand Jack-of-all-trades 2nd hand man'"), top);
            line._colors.Item3 = Color.CornflowerBlue;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("JellyBru", "Lead Spriter", "'Jetshift was a mistake'"), top);
            line._colors.Item3 = Color.Magenta;
            line.bufferSpace += 64;
            line.customDrawData = delegate (CreditsLine liner)
            {
                Texture2D dev = SGAmod.Instance.GetTexture("Items/Armors/Dev/Jellybru_Armor_dev_vanity_Idle");
                Main.spriteBatch.Draw(dev, liner.position + new Vector2(-64, 48f), null, Color.White, 0, dev.Size() / 2f, 1f, default, 0);
                dev = SGAmod.Instance.GetTexture("Items/Weapons/Shields/AegisaltAetherstone");
                Main.spriteBatch.Draw(dev, liner.position + new Vector2(64, 48f), null, Color.White, 0, dev.Size() / 2f, 1f, default, 0);
                dev = SGAmod.Instance.GetTexture("Items/Weapons/TheJellyBrew");
                Main.spriteBatch.Draw(dev, liner.position + new Vector2(0, 116f), null, Color.White, 0, dev.Size() / 2f, 1f, default, 0);
            };
            creditsToSpawn.Add(line);

            line = new CreditsLine(("BedrockBreaker", "Assistant Coder", "'Knows shaders better than most of us'"), top);
            line._colors.Item3 = Color.LightGoldenrodYellow;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Maskano", "Spriter", "'this is the best LSD mod i've worked on'"), top);
            line._colors.Item3 = Color.Teal;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Musicman", "Composer", "'Universe Curse not included!'"), top);
            line._colors.Item3 = Color.Aquamarine;
            creditsToSpawn.Add(line);


            line = new CreditsLine(("", "", ""), top);
            line.delayTimer = 50;
            creditsToSpawn.Add(line);
            line = new CreditsLine(("Those who have left, but their actions have not!", "Contributions", ""), top);
            line.font = Main.fontDeathText;
            line._colors.Item1 = Color.WhiteSmoke;
            line.textDistance = 0;
            line.delayTimer = 30;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Mr. Creeper (Big E)", "Spriter", "'Wish things didn't end how they did'"), top);
            line._colors.Item3 = Color.Green;
            line.customDrawData = delegate (CreditsLine liner)
            {
                Texture2D dev = SGAmod.Instance.GetTexture("Items/Armors/Dev/CreeperSet");
                Main.spriteBatch.Draw(dev, liner.position+new Vector2(110, 48f), null, Color.White, 0, dev.Size() / 2f, 1f, default, 0);
                dev = SGAmod.Instance.GetTexture("Items/Weapons/CreepersThrow");
                Main.spriteBatch.Draw(dev, liner.position + new Vector2(-110, 48f), null, Color.White, 0, dev.Size() / 2f, 1f, default, 0);
                dev = SGAmod.Instance.GetTexture("Items/Weapons/Stormbreaker");
                Main.spriteBatch.Draw(dev, liner.position + new Vector2(-160, 48f), null, Color.White, 0, dev.Size() / 2f, 1f, default, 0);
            };
            creditsToSpawn.Add(line);

            line = new CreditsLine(("PhilBill44", "Former Spriter", "'Godspeed!'"), top);
            line._colors.Item3 = Color.Silver;
            line.delayTimer += 12;
            line.bufferSpace += 96;
            line.customDrawData = delegate (CreditsLine liner)
            {
                Texture2D dev = SGAmod.Instance.GetTexture("iconOld");
                Main.spriteBatch.Draw(dev, liner.position + new Vector2(0, 140), null, Color.White, 0, dev.Size() / 2f, 1f, default, 0);
                dev = SGAmod.Instance.GetTexture("Items/Weapons/WaveBeam");
                Main.spriteBatch.Draw(dev, liner.position + new Vector2(62, 60), null, Color.White, 0, dev.Size() / 2f, 1f, default, 0);
            };
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Daim", "Former Spriter", "'Legend'"), top);
            line._colors.Item3 = Color.Silver;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Joost", "Code Contributor", "'The OG Random Content Mod!'"), top);
            line._colors.Item3 = Color.Lime;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Qwerty3.14", "Code Contributor", "'The 2nd Random Content Mod!'"), top);
            line._colors.Item3 = Color.Wheat;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Turingcomplete30", "Code Contributor", "'No such thing as too much drawcode and the power of MATH!'"), top);
            line._colors.Item3 = Color.Yellow;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Freya manibrandr", "Commissioned Sprites", "'1st to draw your favorite green derg!'"), top);
            line._colors.Item3 = Color.Red;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Dsurion123", "Commissioned Sprites", "'Thanks for the new mod icon!'"), top);
            line._colors.Item3 = Color.Red;
            line.delayTimer += 12;
            line.bufferSpace += 96;
            line.customDrawData = delegate (CreditsLine liner)
            {
                Texture2D dev = SGAmod.Instance.GetTexture("icon");
                Main.spriteBatch.Draw(dev, liner.position + new Vector2(0, 140), null, Color.White, 0, dev.Size() / 2f, 1f, default, 0);
            };
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Zoomo", "Commissioned Sprites", "'Would recommended for commissions!'"), top);
            line._colors.Item3 = Color.Red;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Squid", "Former Spriter", "'Vulcanit revival one day?'"), top);
            line._colors.Item3 = Color.Orange;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("DeJuiceTD", "Former Spriter", "'No idea where they went'"), top);
            line._colors.Item3 = Color.Purple;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Dejsprite", "Former Spriter", "'Went AWOL'"), top);
            line._colors.Item3 = Color.MediumPurple;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("El3trick_Plays", "Former Spriter", "'Also Went AWOL'"), top);
            line._colors.Item3 = Color.Yellow;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Lonely Star", "Former Spriter", "'One sprite wonder!'"), top);
            line._colors.Item3 = Color.Maroon;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Arcri", "Former Spriter", "'Two sprite wonder!'"), top);
            line._colors.Item3 = Color.Maroon;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Aqua Surfer Games", "Musician", "'Finally, no more boss 1 for Hellion!'"), top);
            line._colors.Item3 = Color.Maroon;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Kooyah", "Former Spriter and Havoc mod Spriter", "'Ya did good mate'"), top);
            line._colors.Item3 = Color.Purple;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("TheRandomMidget", "Asset Contributor and former Havoc mod Director", "'Many of the left over Havoc assets were owned by them'"), top);
            line._colors.Item3 = Color.Yellow;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Iggysaur", "Code Contributor and former Havoc mod Coder", "'Many of the left over Havoc assets were coded by him'"), top);
            line._colors.Item3 = Color.Yellow;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("GabeHasWon", "Code Contributor and former Havoc mod Coder", "'Origin of Murk!'"), top);
            line._colors.Item3 = Color.Yellow;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Vasyanex", "Code+Sprite Contributor and former Havoc mod Dev", "'What a mystery you were...'"), top);
            line._colors.Item3 = Color.LimeGreen;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Ajax", "Sprite Contributor", "'Made some stuff, also cool person'"), top);
            line._colors.Item3 = Color.Maroon;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Wicloud", "Sprite Contributor", "'I am never having those Ice Fairies resprited lol'"), top);
            line._colors.Item3 = Color.Maroon;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("MountainyBear49", "Sprite Contributer", "'Hail Glory mod in their honor!'"), top);
            line._colors.Item3 = Color.Orange;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("", "", ""), top);
            line.delayTimer = 50;
            creditsToSpawn.Add(line);
            line = new CreditsLine(("Yes, you all count too!", "Honorable Mentions", ""), top);
            line.font = Main.fontDeathText;
            line.textDistance = 0;
            line.delayTimer = 30;
            creditsToSpawn.Add(line);



            line = new CreditsLine(("Stormer", "", "'Drekling'"), top);
            line._colors.Item2 = Color.Lime;
            line.delayTimer = 20;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Jubia", "", "'Best Goat'"), top);
            line._colors.Item2 = Color.AntiqueWhite;
            line.customDrawData = delegate (CreditsLine liner)
            {
                int totalFrames = Main.npcFrameCount[Terraria.ID.NPCID.Guide];
                int frame = (int)(CreditsManager.timePassed / 7f) % totalFrames;

                 Texture2D Jubia = SGAmod.Instance.GetTexture("NPCs/TownNPCs/Goat");
                Vector2 frameSize = new Vector2(Jubia.Width, Jubia.Height);

                Rectangle rect = new Rectangle(0, (int)(frame * (frameSize.Y / totalFrames)), (int)frameSize.X, (int)(frameSize.Y / totalFrames));

                Main.spriteBatch.Draw(Jubia, liner.position + new Vector2(64, 32), rect, Color.White, 0, rect.Size() / 2f, 1f, SpriteEffects.FlipHorizontally, 0);

            };            
            line.delayTimer = 20;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Anarog", "", "'Cyber derg that is very heart heating'"), top);
            line._colors.Item2 = Color.Wheat;
            line.delayTimer = 20;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Cataclysmic Armageddon", "", "'Offical Hellion fanfiction!'"), top);
            line._colors.Item2 = Color.Turquoise;
            line.delayTimer = 20;
            line.customDrawData = delegate (CreditsLine liner)
            {
                Texture2D dedicated = SGAmod.Instance.GetTexture("Items/Weapons/NuclearOption");
                Main.spriteBatch.Draw(dedicated, liner.position + new Vector2(-106, 42), null, Color.White, 0, dedicated.Size() / 2f, 1f, default, 0);

                int frame = (int)(CreditsManager.timePassed / 7f) % 10;

                Texture2D NoHitCharm = SGAmod.Instance.GetTexture("Items/Accessories/Charms/NoHitCharmlv1");
                Vector2 frameSize = new Vector2(NoHitCharm.Width, NoHitCharm.Height);

                Rectangle rect = new Rectangle(0, (int)(frame * (frameSize.Y / 10)), (int)frameSize.X, (int)(frameSize.Y / 10));

                Main.spriteBatch.Draw(NoHitCharm, liner.position + new Vector2(120, 32), rect, Color.White, 0, rect.Size() / 2f, 1f, default, 0);

            };
            creditsToSpawn.Add(line);

            line = new CreditsLine(("AP STP", "", "'2nd coming of Ecloud'"), top);
            line._colors.Item2 = Color.Yellow;
            line.delayTimer = 20;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("paniq", "", "'For their Shadertoy Sphere shader, which I backported to HLSL 2.0'"), top);
            line._colors.Item2 = Color.WhiteSmoke;
            line.delayTimer = 20;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("suicidecrew", "", "Seemless fire texture"), top);
            line._colors.Item2 = Color.WhiteSmoke;
            line.delayTimer = 20;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("DRKV333", "", "'While you had no direct involvement, your wisdom did!'"), top);
            line._colors.Item2 = Color.WhiteSmoke;
            line.delayTimer = 20;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Zadum4ivii", "", "'Your Antiaris Stove got my started on tile coding, lol'"), top);
            line._colors.Item2 = Color.AntiqueWhite;
            line.delayTimer = 20;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Kazzymodus", "", "'Great shader tutorial, also: overused shockwave shader'"), top);
            line._colors.Item2 = Color.AntiqueWhite;
            line.delayTimer = 20;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Ðark?ight", "", "'Wouldn't be here without your help all those years ago'"), top);
            line._colors.Item2 = Color.AntiqueWhite;
            line.delayTimer = 20;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Direwolf420", "", "'Same to you!'"), top);
            line._colors.Item2 = Color.AntiqueWhite;
            line.delayTimer = 20;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Boffin", "", "'The one trail shader that started them all!'"), top);
            line._colors.Item2 = Color.WhiteSmoke;
            line.delayTimer = 20;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Zoaklen", "", "'Your mod was a legendary learning tool for me!'"), top);
            line._colors.Item2 = Color.AntiqueWhite;
            line.delayTimer = 20;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("Blushiemagic", "", "'Hope life takes you places!'"), top);
            line._colors.Item2 = Color.WhiteSmoke;
            line.delayTimer = 30;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("All the people involved:", "Dimensional Doors", "'Heavy inspiration for my mod! And also where the music came from!'"), top);
            line._colors.Item2 = Color.WhiteSmoke;
            line.delayTimer = 20;
            creditsToSpawn.Add(line);

            line = new CreditsLine(("", "", ""), top);
            line.delayTimer = 75;
            creditsToSpawn.Add(line);

            line = new CreditsLineRainbowFlavor(("Thank you, for playing my mod!", "And of course... You!", "IDG"), top);
            line.font = Main.fontDeathText;
            line.textDistance = 0;
            line.delayTimer = 30;
            line.bufferSpace = 120;
            creditsToSpawn.Add(line);

        }

        public static void SpawnCreditLine()
        {
            if (creditsToSpawn.Count > 0)
            {
                CreditsLine line = creditsToSpawn[0];
                credits.Add(line);
                delayTimer = line.delayTimer;
                creditsToSpawn.RemoveAt(0);
            }
        }

        public static void RollCredits()
        {
            if (creditsRolling || !queuedCredits)
                return;

            queuedCredits = false;
            delayTimer = 0;
            previousMusicVolume = Main.musicVolume;
            colorAnimation = 1;
            timePassed = 0;
            creditsRolling = true;
            credits = new List<CreditsLine>();
            creditsToSpawn = new List<CreditsLine>();
            creditsRenderTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight, false, Main.graphics.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24, 1, RenderTargetUsage.DiscardContents);
            AddCreditEntries();


            GraphicsDevice GD = Main.graphics.GraphicsDevice;

            SGAmod.ForceDrawOverride = true;
            //sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);
            typeof(Main).GetMethod("DoDraw", SGAmod.UniversalBindingFlags).Invoke(Main.instance, new object[1] { SGAmod.lastTime });
            //sb.End();
            SGAmod.ForceDrawOverride = false;

                int[] screenData;
            screenData = new int[GD.PresentationParameters.BackBufferWidth * GD.PresentationParameters.BackBufferHeight];

                GD.GetBackBufferData(screenData);

                ScreenTexture = new Texture2D(GD, GD.PresentationParameters.BackBufferWidth, GD.PresentationParameters.BackBufferHeight, false, GD.PresentationParameters.BackBufferFormat);

                ScreenTexture.SetData(screenData);


                //save to disk 
                //System.IO.Stream stream = System.IO.File.OpenWrite(SGAmod.filePath + "/test.jpg");

               // ScreenTexture.SaveAsJpeg(stream, ScreenTexture.Width, ScreenTexture.Height);
                //stream.Dispose();

                //ScreenTexture.Dispose();


        }

        public static void EndCredits()
        {
            if (!creditsRolling)
                return;

            creditsRolling = false;
            Main.musicVolume = previousMusicVolume;
            queuedCredits = false;
            credits.Clear();
            creditsToSpawn.Clear();
            creditsRenderTarget.Dispose();
            if (ScreenTexture != null)
            ScreenTexture.Dispose();
        }



        public static void UpdateCredits(GameTime gameTime)
        {
            GraphicsDevice GD = Main.graphics.GraphicsDevice;
            Main.instance.IsFixedTimeStep = true;

            delayTimer -= ScrollSpeed/4f;
            timePassed += 1;
            queuedCredits = false;

            colorAnimation = MathHelper.SmoothStep(0.2f, colorAnimation, 0.9f);

            Main.musicVolume = MathHelper.Clamp(Main.musicVolume+((-0.10f - Main.musicVolume)-Main.musicVolume) *0.005f, 0f, 1f);

            typeof(Main).GetMethod("DoUpdate_AnimateDiscoRGB", SGAmod.UniversalBindingFlags).Invoke(Main.instance, new object[0]);
            typeof(Main).GetMethod("UpdateAudio", SGAmod.UniversalBindingFlags).Invoke(Main.instance, new object[0]);

            foreach (CreditsLine line in credits)
            {
                line.position.Y -= ScrollSpeed;
            }

            credits = credits.Where(testby => testby.position.Y > -testby.bufferSpace).ToList();

            Microsoft.Xna.Framework.Input.KeyboardState keyState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            if (keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                credits.Clear();
                creditsToSpawn.Clear();
                return;
            }

            if (delayTimer < 1)
            {
                SpawnCreditLine();

            }
            DrawToRenderTarget(gameTime);
        }

        public static void DrawToRenderTarget(GameTime gameTime)
        {
            GraphicsDevice GD = Main.graphics.GraphicsDevice;
            SpriteBatch sb = Main.spriteBatch;
            RenderTargetBinding[] binds = GD.GetRenderTargets();

            Main.graphics.GraphicsDevice.SetRenderTarget(Main.screenTarget);

            /*
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);
            if (ScreenTexture != null && !ScreenTexture.IsDisposed)
            Main.spriteBatch.Draw(ScreenTexture, Vector2.Zero, null, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            sb.End();
            */

            Main.graphics.GraphicsDevice.SetRenderTarget(creditsRenderTarget);
            Main.graphics.GraphicsDevice.Clear(Color.Transparent);

            //Main.spriteBatch.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);



            foreach (CreditsLine line in credits)
            {
                int spaceABit = line.textDistance;
                string strrole = line.text.Item2;
                string strname = line.text.Item1;
                string strflavor = line.text.Item3;

                Vector2 strSize1 = line.font.MeasureString(strrole);
                Vector2 strSize2 = Main.fontCombatText[0].MeasureString(strname);
                Vector2 strSize3 = Main.fontItemStack.MeasureString(strflavor);

                if (line.customDrawDataBefore != default)
                    line.customDrawDataBefore(line);

                DynamicSpriteFontExtensionMethods.DrawString(sb, line.font, strrole, line.position, line.Colors.Item1, 0, new Vector2(strSize1.X, 0) / 2f, 1f, SpriteEffects.None, 0);
                DynamicSpriteFontExtensionMethods.DrawString(sb, Main.fontCombatText[0], strname, line.position + new Vector2(0, strSize1.Y + spaceABit), line.Colors.Item2, 0, new Vector2(strSize2.X, 0) / 2f, 1f, SpriteEffects.None, 0);

                DynamicSpriteFontExtensionMethods.DrawString(sb, Main.fontItemStack, strflavor, line.position + new Vector2(0, strSize1.Y + strSize2.Y + spaceABit * 1.2f), (line.Colors.Item3 == Color.Wheat || line.Colors.Item3 == Color.WhiteSmoke || line.Colors.Item3 == Color.AntiqueWhite) ? line.Colors.Item3 : Color.Lerp(line.Colors.Item3,Color.White,0.50f), 0, new Vector2(strSize3.X, 0) / 2f, 1f, SpriteEffects.None, 0);
                if (line.customDrawData != default)
                    line.customDrawData(line);

            }

            float fadeInAlpha = MathHelper.Clamp(timePassed / 150f,0f,1f);

            string hinttex = "Hold Arrow keys to scroll";
            Vector2 hinttexSize1 = Main.fontCombatText[1].MeasureString(hinttex);

            DynamicSpriteFontExtensionMethods.DrawString(sb, Main.fontCombatText[1], hinttex, new Vector2(creditsRenderTarget.Width, creditsRenderTarget.Height), Color.White*fadeInAlpha, 0, new Vector2(hinttexSize1.X+64, hinttexSize1.Y), 1f, SpriteEffects.None, 0);

            hinttex = "Escape to Skip";

            DynamicSpriteFontExtensionMethods.DrawString(sb, Main.fontCombatText[1], hinttex, new Vector2(0, creditsRenderTarget.Height), Color.White * fadeInAlpha, 0, new Vector2(-64, hinttexSize1.Y), 1f, SpriteEffects.None, 0);


            sb.End();

            GD.SetRenderTargets(binds);
        }

        public static void DrawCredits(GameTime gameTime)
        {
            //Vector2 standardSize = new Vector2(Main.screenWidth/1980f, Main.screenHeight/1080f);
            //Main.spriteBatch.End();
            SpriteBatch sb = Main.spriteBatch;
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);

            if (ScreenTexture != null && !ScreenTexture.IsDisposed)
            {
                Vector2 standardSize = new Vector2(Main.screenWidth / (float)ScreenTexture.Width, Main.screenHeight / (float)ScreenTexture.Height);
                Main.spriteBatch.Draw(ScreenTexture, Vector2.Zero, null, Color.White.MultiplyRGBA(new Color(colorAnimation, colorAnimation, colorAnimation, 1f)), 0, Vector2.Zero, Vector2.One* standardSize, SpriteEffects.None, 0f);
            }

            Main.spriteBatch.Draw(creditsRenderTarget, Vector2.Zero, null, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);

            sb.End();
        }



    }

    public class CreditsLine
    {
        public (string, string, string) text;
        public Vector2 position;
        public DynamicSpriteFont font = default;
        public int delayTimer = 40;
        public int bufferSpace = 96;
        public int textDistance = 6;
        public (Color, Color, Color) _colors = (Color.White, Color.White, Color.White);
        public virtual (Color, Color, Color) Colors
        {
            get
            {
                return _colors;
            }
        }

        public Action<CreditsLine> customDrawDataBefore = default;
        public Action<CreditsLine> customDrawData = default;

            public CreditsLine((string, string, string) text,Vector2 where,Action<CreditsLine> customDrawData = default)
        {
            this.text = text;
            this.position = where;
            if (font == default)
                font = Main.fontCombatText[1];

            if (customDrawData != default)
                this.customDrawData = customDrawData;
        }


    }
    public class CreditsLineRainbowFlavor : CreditsLine
    {
        public override (Color, Color, Color) Colors
        {
            get
            {
                return (_colors.Item1, _colors.Item2,Main.DiscoColor);
            }
        }
        public CreditsLineRainbowFlavor((string, string, string) text, Vector2 where, Action<CreditsLine> customDrawData = default) : base(text, where, customDrawData) { }
    }

}