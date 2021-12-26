using Idglibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System; 
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Tools
{

    public class UniversalBait : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Universal Bait");
            Tooltip.SetDefault("Always catches the current quest fish when used as bait");
        }
        public override bool Autoload(ref string name)
        {
            SGAmod.PostUpdateEverythingEvent += SGAmod_PostUpdateEverythingEvent;
            return true;
        }

        public override string Texture => "Terraria/Confuse";

        public static int GetFish()
        {
            if (!Main.anglerQuestFinished && !Main.anglerWhoFinishedToday.Contains(Main.LocalPlayer.name))
                return Main.anglerQuestItemNetIDs[Main.anglerQuest];
            return -1;
        }

        private void SGAmod_PostUpdateEverythingEvent()
        {
            if (GetFish()>=0)
                Main.itemTexture[ModContent.ItemType<UniversalBait>()] = Main.itemTexture[Main.anglerQuestItemNetIDs[Main.anglerQuest]];
            else
                Main.itemTexture[ModContent.ItemType<UniversalBait>()] = Main.confuseTexture;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Main.hslToRgb((-Main.GlobalTime / 4f) % 1f, 0.85f, 0.50f);
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.width = 14;
            item.height = 14;
            item.value = 0;
            item.bait = 250;
            item.rare = ItemRarityID.Expert;
        }
    }

    /*public class AnglerRod : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Angler Rod");
			Tooltip.SetDefault("'the cheeky bastard...'\nCan fish up the quest fish from anywhere if you don't have one already\n" + Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stack, adds 30 seconds"));
		}
		public override void SetDefaults()
		{
			item.width = 56;
			item.height = 32;
			item.useTime = 8;
			item.useAnimation = 8;
			item.useStyle = 1;
			item.value = 100000;
			item.rare = 9;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
			item.shoot = ModContent.ProjectileType<AnglerRodHook>();
			item.shootSpeed = 17f;
			item.fishingPole = 100;
		}
        public override bool CanUseItem(Player player)
        {
            return player.SGAPly().AddCooldownStack(60*30);
        }
        public override void HoldItem(Player player)
		{
			player.GetModPlayer<JoostPlayer>().lunarRod = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FragmentNebula, 3);
			recipe.AddIngredient(ItemID.FragmentSolar, 3);
			recipe.AddIngredient(ItemID.FragmentVortex, 3);
			recipe.AddIngredient(ItemID.FragmentStardust, 3);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();

		}
		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
            Vector2 speed = new Vector2(speedX, speedY);
			double offsetAngle;
			int i;
				Projectile.NewProjectile(position.X, position.Y, speed.X, speed.Y, type, damage, knockBack, player.whoAmI);

			return false;
		}
	}*/

    public class AnglerRodHook : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BobberGolden);
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lunar Fish Hook");
        }

        public override string Texture => "Terraria/NPC_Head_22";

        public override bool PreDrawExtras(SpriteBatch spriteBatch)      //this draws the fishing line correctly
        {

            if (projectile.bobber && Main.player[projectile.owner].inventory[Main.player[projectile.owner].selectedItem].holdStyle > 0)
            {
                Lighting.AddLight(projectile.Center, 0.13f, 0.86f, 0.59f);  //this defines the projectile/bobber light color
                Player player = Main.player[projectile.owner];
                float pPosX = player.MountedCenter.X;
            float pPosY = player.MountedCenter.Y;
            pPosY += Main.player[projectile.owner].gfxOffY;

                SGAUtils.DrawFishingLine(new Vector2(pPosX, pPosY),projectile.Center,projectile.velocity,projectile.Center+new Vector2(0,32), projectile.localAI[0]);

                /*float pPosX = player.MountedCenter.X;
                float pPosY = player.MountedCenter.Y;
                pPosY += Main.player[projectile.owner].gfxOffY;
                int type = Main.player[projectile.owner].inventory[Main.player[projectile.owner].selectedItem].type;
                float gravDir = Main.player[projectile.owner].gravDir;

                if (type == mod.ItemType("LunarRod")) //add your Fishing Pole name here
                {
                    pPosX += (float)(50 * Main.player[projectile.owner].direction);
                    if (Main.player[projectile.owner].direction < 0)
                    {
                        pPosX -= 13f;
                    }
                    pPosY -= 30f * gravDir;
                }

                if (gravDir == -1f)
                {
                    pPosY -= 12f;
                }
                Vector2 value = new Vector2(pPosX, pPosY);
                value = Main.player[projectile.owner].RotatedRelativePoint(value + new Vector2(8f), true) - new Vector2(8f);
                float projPosX = projectile.position.X + (float)projectile.width * 0.5f - value.X;
                float projPosY = projectile.position.Y + (float)projectile.height * 0.5f - value.Y;
                Math.Sqrt((double)(projPosX * projPosX + projPosY * projPosY));
                float rotation2 = (float)Math.Atan2((double)projPosY, (double)projPosX) - 1.57f;
                bool flag2 = true;
                if (projPosX == 0f && projPosY == 0f)
                {
                    flag2 = false;
                }
                else
                {
                    float projPosXY = (float)Math.Sqrt((double)(projPosX * projPosX + projPosY * projPosY));
                    projPosXY = 12f / projPosXY;
                    projPosX *= projPosXY;
                    projPosY *= projPosXY;
                    value.X -= projPosX;
                    value.Y -= projPosY;
                    projPosX = projectile.position.X + (float)projectile.width * 0.5f - value.X;
                    projPosY = projectile.position.Y + (float)projectile.height * 0.5f - value.Y;
                }
                while (flag2)
                {
                    float num = 12f;
                    float num2 = (float)Math.Sqrt((double)(projPosX * projPosX + projPosY * projPosY));
                    float num3 = num2;
                    if (float.IsNaN(num2) || float.IsNaN(num3))
                    {
                        flag2 = false;
                    }
                    else
                    {
                        if (num2 < 20f)
                        {
                            num = num2 - 8f;
                            flag2 = false;
                        }
                        num2 = 12f / num2;
                        projPosX *= num2;
                        projPosY *= num2;
                        value.X += projPosX;
                        value.Y += projPosY;
                        projPosX = projectile.position.X + (float)projectile.width * 0.5f - value.X;
                        projPosY = projectile.position.Y + (float)projectile.height * 0.1f - value.Y;
                        if (num3 > 12f)
                        {
                            float num4 = 0.3f;
                            float num5 = Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y);
                            if (num5 > 16f)
                            {
                                num5 = 16f;
                            }
                            num5 = 1f - num5 / 16f;
                            num4 *= num5;
                            num5 = num3 / 80f;
                            if (num5 > 1f)
                            {
                                num5 = 1f;
                            }
                            num4 *= num5;
                            if (num4 < 0f)
                            {
                                num4 = 0f;
                            }
                            num5 = 1f - projectile.localAI[0] / 100f;
                            num4 *= num5;
                            if (projPosY > 0f)
                            {
                                projPosY *= 1f + num4;
                                projPosX *= 1f - num4;
                            }
                            else
                            {
                                num5 = Math.Abs(projectile.velocity.X) / 3f;
                                if (num5 > 1f)
                                {
                                    num5 = 1f;
                                }
                                num5 -= 0.5f;
                                num4 *= num5;
                                if (num4 > 0f)
                                {
                                    num4 *= 2f;
                                }
                                projPosY *= 1f + num4;
                                projPosX *= 1f - num4;
                            }
                        }
                        rotation2 = (float)Math.Atan2((double)projPosY, (double)projPosX) - 1.57f;
                        Microsoft.Xna.Framework.Color color2 = Lighting.GetColor((int)value.X / 16, (int)(value.Y / 16f), new Microsoft.Xna.Framework.Color(34, 221, 151, 100));

                        Main.spriteBatch.Draw(Main.fishingLineTexture, new Vector2(value.X - Main.screenPosition.X + (float)Main.fishingLineTexture.Width * 0.5f, value.Y - Main.screenPosition.Y + (float)Main.fishingLineTexture.Height * 0.5f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.fishingLineTexture.Width, (int)num)), color2, rotation2, new Vector2((float)Main.fishingLineTexture.Width * 0.5f, 0f), 1f, SpriteEffects.None, 0f);
                    }
                }*/
            }
            return false;
        }
    }

}
