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
using System.Linq;

namespace SGAmod.NPCs
{
    public class SplunkerFish : ModNPC
    {

        public int sparkleCounter=0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Splunker Jellyfish");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.PinkJellyfish];
        }
        public override string Texture => "Terraria/NPC_" + NPCID.PinkJellyfish;

        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.PinkJellyfish);
            npc.lifeMax *= 1;
            aiType = NPCID.BlueJellyfish;
            animationType = NPCID.BlueJellyfish;
        }

        public override void AI()
        {
            sparkleCounter++;
            if (sparkleCounter % 15 == 0)
            {
                SGAUtils.SpelunkerGlow(npc.Center,32);
            }

            Lighting.AddLight(npc.Center, Color.Yellow.ToVector3() * 1.00f);
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.position, npc.Hitbox.Size(), ItemID.SpelunkerGlowstick, Main.rand.Next(6, 12));
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D jelly = Main.npcTexture[npc.type];
            Texture2D splunk = Main.itemTexture[ItemID.SpelunkerGlowstick];
            int frameCount = Main.npcFrameCount[NPCID.BlueJellyfish];
            int height = jelly.Height / (frameCount);
            float splunkoffset = -3;
            
            if (npc.frame.Y == ((height) * 1) || npc.frame.Y == (height) * 3)
            splunkoffset = -1;
            if (npc.frame.Y == height * 2)
            splunkoffset = 1;

            Vector2 offset = new Vector2(jelly.Width, height);

            //Main.spriteBatch.End();
            //Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            Effect hallowed = SGAmod.HallowedEffect;

            Color lighter = Lighting.GetColor((int)npc.Center.X >> 4, (int)npc.Center.Y >> 4, Color.White);
            Color col = Color.Yellow.MultiplyRGBA(lighter);

             Main.spriteBatch.Draw(splunk, npc.Center-Vector2.UnitY.RotatedBy(npc.rotation)* npc.scale * splunkoffset - Main.screenPosition, null, Color.White, npc.rotation+0.42f, splunk.Size() / 2f, npc.scale*0.75f, default, 0);

            Main.spriteBatch.Draw(jelly, npc.Center - Main.screenPosition, npc.frame, col * 0.5f, npc.rotation, offset / 2f, npc.scale, default, 0);
            //Main.spriteBatch.Draw(jelly2, npc.Center - Main.screenPosition, npc.frame, col * 0.250f, npc.rotation, offset / 2f, npc.scale, default, 0);
            //Main.spriteBatch.Draw(jelly3, npc.Center - Main.screenPosition, npc.frame, col*0.250f, npc.rotation, offset / 2f, npc.scale, default, 0);



            //Main.spriteBatch.End();
            //Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            Tile tile = Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY];
            bool NoEventsNoTowns = !spawnInfo.playerInTown && !spawnInfo.invasion && !Main.pumpkinMoon && !Main.snowMoon && !Main.eclipse;
            return NoEventsNoTowns && spawnInfo.water && (spawnInfo.spawnTileY > Main.rockLayer / 3) ? 0.08f : 0f;
        }

    }


    public class EliteBat : ModNPC
    {

        List<(Vector2, float,int)> posses = new List<(Vector2, float, int)>();
        int[] bats = { NPCID.CaveBat, NPCID.GiantBat, NPCID.IlluminantBat, NPCID.JungleBat, NPCID.IceBat };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Elite Bat");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.CaveBat];
        }
        public override string Texture => "Terraria/NPC_"+NPCID.CaveBat;

        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.CaveBat);
            npc.lifeMax = 600;
            npc.defense = 20;
            aiType = NPCID.CaveBat;
            animationType = NPCID.CaveBat;
        }

        int counter = 0;
        public override void AI()
        {

            if (npc.target < 0 || npc.target == byte.MaxValue || Main.player[npc.target].dead)
            {
                npc.TargetClosest(true);
            }
                counter += 1;

            Player ply = Main.player[npc.target];

            if (counter % 1000 < 750 || Main.npc.Where(testby => testby.active && bats.Any(testby2 => testby2 == testby.type) && (testby.Center - npc.Center).LengthSquared() < 518400).Count() > 2)
            {
                if (counter % ((npc.life<npc.lifeMax/2) ? 75 : 200) == 0)
                {
                    Vector2 angle = ply.Center - npc.Center;
                    angle.Normalize();
                    npc.velocity = angle * 12f;
                    posses.Add((npc.Center, npc.rotation, 60));

                }
            }
            else
            {
                npc.velocity /= 5f;
                if (counter % 70 == 0)
                {
                    int npc2 = NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2)), (int)npc.position.Y + npc.height / 2, bats.OrderBy(testby => Main.rand.Next()).ToArray()[0], npc.whoAmI, 0f, 0f, 0f, 0f, 255);
                    Main.npc[npc2].SpawnedFromStatue = true;
                    Main.npc[npc2].netUpdate = true;
                }
            }

            posses.Add((npc.Center, npc.rotation, 10));
            posses = posses.Select(testby => (testby.Item1, testby.Item2, testby.Item3 - 1)).Where(testby => testby.Item3 > 0).ToList();

        }

        public override bool SpecialNPCLoot()
        {
            int type = npc.type;
            foreach (int batype in bats)
            {
                npc.type = NPCID.CaveBat;
                npc.NPCLoot();
            }
            npc.type = type;

            return true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            Tile tile = Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY];//!NPC.BusyWithAnyInvasionOfSorts()
            bool NoEventsNoTowns = !spawnInfo.playerInTown && !spawnInfo.invasion && !Main.pumpkinMoon && !Main.snowMoon && !Main.eclipse;
            return NoEventsNoTowns && Main.hardMode && !spawnInfo.player.ZoneUnderworldHeight && (spawnInfo.spawnTileY > Main.rockLayer / 2 || spawnInfo.player.ZoneJungle) ? (spawnInfo.player.ZoneJungle ? 0.025f : 0.005f) : 0f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D battex = Main.npcTexture[npc.type];
            Vector2 offset = new Vector2(battex.Width, battex.Height / (Main.npcFrameCount[npc.type]));

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            Effect hallowed = SGAmod.HallowedEffect;

            Color lighter = Lighting.GetColor((int)npc.Center.X >> 4, (int)npc.Center.Y >> 4, Color.White);
            Color col = Color.Goldenrod.MultiplyRGBA(Lighting.GetColor((int)npc.Center.X >> 4, (int)npc.Center.Y >> 4, Color.White));

            hallowed.Parameters["alpha"].SetValue(1);
            hallowed.Parameters["prismAlpha"].SetValue(1f);
            hallowed.Parameters["prismColor"].SetValue(col.ToVector3());
            hallowed.Parameters["overlayTexture"].SetValue(mod.GetTexture("Space"));
            hallowed.Parameters["overlayProgress"].SetValue(new Vector3(0, Main.GlobalTime / 1f, Main.GlobalTime / 2f));
            hallowed.Parameters["overlayAlpha"].SetValue(0.25f);
            hallowed.Parameters["overlayStrength"].SetValue(0.25f);
            hallowed.Parameters["overlayMinAlpha"].SetValue(0.25f);
            hallowed.Parameters["overlayScale"].SetValue(new Vector2(0.25f, 0.25f));

            foreach ((Vector2, float, int) placeangletime in posses.OrderBy(testby => testby.Item3))
            {
                Main.spriteBatch.Draw(battex, placeangletime.Item1 - Main.screenPosition, npc.frame, Color.White*0.50f*((float)placeangletime.Item3/12f), placeangletime.Item2* MathHelper.Clamp((float)placeangletime.Item3 / 10f,0f,1f), offset / 2f, npc.scale, default, 0);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            return true;
        }

    }
}
