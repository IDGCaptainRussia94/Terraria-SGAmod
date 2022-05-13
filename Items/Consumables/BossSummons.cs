#define DefineHellionUpdate

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using SGAmod.NPCs.Hellion;
using Terraria.GameContent.Events;
using SGAmod.Items.Weapons;
using SGAmod.NPCs;
using Terraria.Localization;
using Microsoft.Xna.Framework.Audio;
using SGAmod.HavocGear.Items;

namespace SGAmod.Items.Consumables
{

	public class BaseBossSummon : ModItem
	{
		public override bool Autoload(ref string name)
		{
			return GetType()!=typeof(BaseBossSummon);
		}

		public override bool CanUseItem(Player player)
		{
			if (SGAmod.anysubworld)
			{
				if (player == Main.LocalPlayer)
					Main.NewText("This cannot be used outside the normal folds of reality...", 75, 75, 80);

				return false;
			}
			return base.CanUseItem(player);
		}


	}

		public class WraithCoreFragment3 : WraithCoreFragment
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lunar Wraith Core Fragment");
			Tooltip.SetDefault("Summons forth the third and final of the Wraiths, who has stolen your ability to make Luminite Bars (and also the Ancient Manipulator from the Cultist)");
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Consumables/LunarCore"; }
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			item.rare = 8;
		}

		public override bool CanUseItem(Player player)
		{
			if (SGAWorld.downedWraiths == 3 && !NPC.downedMoonlord)
			{
				item.consumable = false;
			} else {
				item.consumable = true;
			}
			return base.CanUseItem(player);
		}

		public override bool UseItem(Player player)
		{
			if (item.consumable == false) {
				if (player == Main.LocalPlayer)
					Main.NewText("Our time has not yet come", 25, 25, 80);
				return false;
			} else {
				NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("LuminiteWraith"));
				Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("Fragment", 4);
			recipe.AddIngredient(null, "WraithCoreFragment2", 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class WraithCoreFragment2 : WraithCoreFragment
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Empowered Wraith Core Fragment");
			Tooltip.SetDefault("Summons forth the second of the Wraiths");
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Consumables/EmpoweredCore"; }
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			item.rare = 5;
		}

		public override bool UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("CobaltWraith"));
			Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "WraithCoreFragment", 1);
			recipe.AddRecipeGroup("SGAmod:Tier1HardmodeOre", 10);
			recipe.AddIngredient(mod.ItemType("WraithFragment3"), 5);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}
	public class WraithCoreFragment : BaseBossSummon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wraith Core Fragment");
			Tooltip.SetDefault("Summons forth the first of the Wraiths, who watches you craft bars with envy...");
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Consumables/BasicCore"; }
		}

		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.consumable = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 2;
			item.useAnimation = 2;
			item.useStyle = 4;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.value = 0;
			item.rare = 1;
			item.UseSound = SoundID.Item1;
		}

		public override bool CanUseItem(Player player)
		{
			if (Main.netMode==NetmodeID.Server)
			SGAmod.Instance.Logger.Warn("DEBUG SERVER: item canuse");
			if (Main.netMode == NetmodeID.MultiplayerClient)
				SGAmod.Instance.Logger.Warn("DEBUG CLIENT: item canuse");
			if (!NPC.AnyNPCs(mod.NPCType("CopperWraith")) && !NPC.AnyNPCs(mod.NPCType("CobaltWraith")) && !NPC.AnyNPCs(mod.NPCType("LuminiteWraith")))
			{
				return base.CanUseItem(player);
			} else {
				return false;
			}
		}
		public override bool UseItem(Player player)
		{
			if (Main.netMode == NetmodeID.Server)
				SGAmod.Instance.Logger.Warn("DEBUG SERVER: item used");
			if (Main.netMode == NetmodeID.MultiplayerClient)
				SGAmod.Instance.Logger.Warn("DEBUG CLIENT: item used");

			NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("CopperWraith"));
			Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("SGAmod:Tier1Ore", 15);
			recipe.AddIngredient(ItemID.FallenStar, 2);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class ConchHorn : BaseBossSummon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Conch Horn");
			Tooltip.SetDefault("'It's call pierces the depths of the ocean.' \nSummons the Sharkvern");
		}
		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 12;
			item.maxStack = 99;
			item.rare = 3;
			item.useAnimation = 45;
			item.useTime = 45;
			item.useStyle = 4;
			item.UseSound = SoundID.Item44;
			item.consumable = true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.ZoneBeach && !NPC.AnyNPCs(mod.NPCType("SharkvernHead"))) {
				return base.CanUseItem(player);
			} else {
				if (player == Main.LocalPlayer)
					Main.NewText("The couch blows but no waves are shaken by its ring...", 100, 100, 250);
				return false;

			}
		}

		public override bool UseItem(Player player)
		{
				NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("SharkvernHead"));
				Main.PlaySound(SoundID.Roar, player.position, 0);
				return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Seashell, 1);
			recipe.AddIngredient(ItemID.SharkFin, 1);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class AcidicEgg : BaseBossSummon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acidic Egg");
			Tooltip.SetDefault("'No words for this...' \nSummons the Spider Queen\nRotten Eggs drop from spiders");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.RottenEgg, 1);
			recipe.AddIngredient(ItemID.Cobweb, 25);
			recipe.AddRecipeGroup("SGAmod:EvilBossMaterials", 5);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 12;
			item.maxStack = 99;
			item.rare = 2;
			item.useAnimation = 45;
			item.useTime = 45;
			item.useStyle = 4;
			item.UseSound = SoundID.Item44;
			item.consumable = true;
		}

		public static bool Underground(Entity player) => (int)((double)((player.position.Y + (float)player.height) * 2f / 16f) - Main.worldSurface * 2.0) > 0;
		public static bool Underground(int here) => (int)((double)((here / 16f)*2.0) - Main.worldSurface * 2.0) > 0;

		public override bool CanUseItem(Player player)
		{
			//bool underground = (int)((double)((player.position.Y + (float)player.height) * 2f / 16f) - Main.worldSurface * 2.0) > 0;

			if (Underground(player) && !NPC.AnyNPCs(mod.NPCType("SpiderQueen")))
			{
				return true;
			}
			else
			{
				if (player == Main.LocalPlayer)
					Main.NewText("There are no spiders here, try using it underground", 30, 200, 30);
				return false;

			}
		}
		public override bool UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("SpiderQueen"));
			Main.PlaySound(SoundID.Roar, player.position, 0);
			return true;
		}
	}

	public class PrismaticBansheeStar : BaseBossSummon,IAuroraItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismatic Star");
			Tooltip.SetDefault("'Fallen star imbued with Luminous energy, a fabricated Prismic Egg'\nThrow it on Pearlstone in the underground Hallow\nAfter a short while summons the Aurora Banshee, an empowered version");
		}
		public override void PostUpdate()
		{
			Lighting.AddLight(item.Center, Color.Lerp(Color.BlueViolet, Color.HotPink, (float)Math.Sin((Main.essScale - 0.70f) / 0.30f)).ToVector3() * 0.85f * Main.essScale);
		}
		public override void SetDefaults()
		{
			item.maxStack = 30;
			item.width = 26;
			item.height = 14;
			item.value = Item.sellPrice(0, 1, 50, 0);
			item.consumable = true;
			item.useTime = 32;
			item.useAnimation = 32;
			item.useStyle = 4;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.value = 0;
			item.rare = 9;
			item.UseSound = SoundID.Item35;
		}
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
			if (item.velocity.Y == 0 && item.stack<2)
			{
				//Main.NewText("Debug Message!");
				Point tilePosition = new Point((int)(item.Center.X / 16), ((int)(item.Center.Y) / 16) + 2);
				Tile tile = Framing.GetTileSafely(tilePosition.X, tilePosition.Y);
				//Main.NewText(tile.type + " this type "+item.position);
				if (tile.type == TileID.Pearlstone && AcidicEgg.Underground(item))
				{
					item.ownTime += 2;

					if (item.ownTime % 50 == 0)
					{
						SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_DarkMageCastHeal, item.Center);
						if (sound != null)
						{
							sound.Pitch = -0.65f + (item.ownTime / 1000f);
						}
					}

					if (item.ownTime > 600 && item.stack<2)
                    {
						NPC.NewNPC((int)item.Center.X, (int)item.Center.Y, ModContent.NPCType<PrismBanshee>());
						item.active = false;
                    }
				}

			}
		}
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
			PrismBanshee.DrawPrismCore(spriteBatch, lightColor, item.Center, item.ownTime*0.8f, item.scale, 96f * (item.ownTime/600f));
			return true;
        }
        public override bool CanUseItem(Player player)
		{
			bool underground = (int)((double)((player.position.Y + (float)player.height) * 2f / 16f) - Main.worldSurface * 2.0) > 0;
			;
			if (underground && player.ZoneHoly && !NPC.AnyNPCs(mod.NPCType("PrismBanshee")))
			{
				if (player == Main.LocalPlayer)
					Main.NewText("Here is good, rest it on some pearlstone!", 200, 100, 150);
				return false;
			}
			else
			{
				if (player == Main.LocalPlayer)
					Main.NewText("'it rejects activating where it was not originally from...'", 200, 100, 150);
				return false;

			}
		}
		public override bool UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("PrismBanshee"));
			Main.PlaySound(SoundID.Roar, player.position, 1);
			return true;
		}
	}

	public class RoilingSludge : BaseBossSummon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Roiling Sludge");
			Tooltip.SetDefault("'Ew, Gross!' \nSummons the Murk");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<WraithFragment3>(), 5);
			recipe.AddIngredient(ModContent.ItemType<MoistSand>(), 10);
			recipe.AddIngredient(ItemID.MudBlock, 10);
			recipe.AddIngredient(ItemID.Gel, 20);
			recipe.AddIngredient(ItemID.Bone, 5);
			recipe.AddTile(TileID.Furnaces);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 12;
			item.maxStack = 99;
			item.rare = 2;
			item.useAnimation = 45;
			item.useTime = 45;
			item.useStyle = 4;
			item.UseSound = SoundID.Item44;
			item.consumable = true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.ZoneJungle && !NPC.AnyNPCs(mod.NPCType("Murk")) && !NPC.AnyNPCs(mod.NPCType("BossFlyMiniboss1"))) {
				return base.CanUseItem(player);
			} else {
				if (player == Main.LocalPlayer)
					Main.NewText("There is a lack of mud and sludge for Murk to even exist here...", 40, 180, 60);
				return false;

			}
		}

		public override bool UseItem(Player player)
		{
				NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType(SGAWorld.downedMurk == 0 || TheWholeExperience.Check() ? "BossFlyMiniboss1" : "Murk"));
				Main.PlaySound(SoundID.Roar, player.position, 0);
				return true;
		}
	}

	public class Prettygel : BaseBossSummon,IAuroraItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Luminous Gel");
			Tooltip.SetDefault("Makes pinky very JELLLLYYYYY");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.consumable = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 2;
			item.useAnimation = 2;
			item.useStyle = 4;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.value = 0;
			item.rare = ItemRarityID.Cyan;
			item.UseSound = SoundID.Item1;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (SGAmod.Calamity.Item1)
				tooltips.Add(new TooltipLine(mod, "NoU", "Summoning this boss with automatically disable Revengence and Death Modes"));
		}

		public override bool CanUseItem(Player player)
		{
			if (!NPC.AnyNPCs(mod.NPCType("SPinky")) && !NPC.AnyNPCs(50) && !Main.dayTime)
			{
				return base.CanUseItem(player);
			} else {
				if (player == Main.LocalPlayer)
					Main.NewText("this gel shimmers only in moonlight...", 100, 40, 100);
				return false;
			}
		}

		public override bool UseItem(Player player)
		{
			if (item.consumable == true) {
				SGAmod.CalamityNoRevengenceNoDeathNoU();
				NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("SPinky"));
				Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
				//player.GetModPlayer<SGAPlayer>().Locked=new Vector2(player.Center.X-2000,4000);
			}
			return true;
		}

		public override void AddRecipes()
		{
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LunarBar, 3);
			recipe.AddIngredient(mod.ItemType("IlluminantEssence"), 3);
			recipe.AddIngredient(3111, 10); //pink gel
			recipe.AddTile(220); //Soldifier
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LunarBar, 5);
			recipe.AddIngredient(mod.ItemType("IlluminantEssence"), 3);
			recipe.AddIngredient(mod.ItemType("MurkyGel"), 20);
			recipe.AddTile(220); //Soldifier
			recipe.SetResult(this);
			recipe.AddRecipe();*/
		}
	}

	public class Nineball : BaseBossSummon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nineball");
			Tooltip.SetDefault("Summons the strongest ice fairy\nMake sure you don't hit a wall and aim up");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.consumable = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = 4;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.value = 0;
			item.noUseGraphic = true;
			item.rare = 9;
			item.ammo = AmmoID.Snowball;
			item.UseSound = SoundID.Item1;
			item.shoot = ProjectileID.SnowBallFriendly;
			item.shootSpeed = 10f;
		}

		public override bool CanUseItem(Player player)
		{
			if (!NPC.AnyNPCs(mod.NPCType("Cirno")) && Main.projectile.FirstOrDefault(proj => proj.type == ModContent.ProjectileType<CirnoBall>() && proj.active) == default)
			{
				if (!Main.dayTime || !player.ZoneSnow)
				{
				if (player == Main.LocalPlayer)
					Main.NewText("It's power lies in the snow biome during the day", 50, 50, 250);
					return false;
				}
				else
				{
					return true;
				}
			}
			return false;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            type = ModContent.ProjectileType<CirnoBall>();
			return true;
		}
        public override bool UseItem(Player player)
		{
			if (item.consumable == false)
			{

			}
			else
			{
				//NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("Cirno"));
				//Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Snowball, 1);
			recipe.AddIngredient(ItemID.SoulofNight, 2);
			recipe.AddIngredient(ItemID.SoulofLight, 2);
			recipe.AddIngredient(mod.ItemType("FrigidShard"), 9);
			recipe.AddIngredient(mod.ItemType("IceFairyDust"), 9);
			recipe.AddTile(TileID.IceMachine); //IceMachine
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class CirnoBall : JarateShurikensProg
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cirno Ball");
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Consumables/Nineball"); }
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			//nil
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.SnowBallFriendly);
			projectile.ranged = false;
			projectile.tileCollide = true;
			projectile.friendly = false;
			projectile.hostile = false;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void AI()
		{
			projectile.localAI[1] += 1;
			if (projectile.localAI[1] > 60)
            {
				projectile.aiStyle = -1;
				projectile.velocity *= 0.90f;

				if ((int)projectile.localAI[1] % 30 == 0)
				{
					SoundEffectInstance sound = Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 25);
					if (sound!=null)
						sound.Pitch += (projectile.localAI[1] - 60) / 420f;
				}

				for (int num654 = 0; num654 < 1 + projectile.localAI[1]/9f; num654++)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= (float)(num654 / 9.00);
					Dust num655 = Dust.NewDustPerfect(projectile.Center+new Vector2(2,2) + ogcircle * 10f, 59, -projectile.velocity + randomcircle * 2f, 150, Color.Aqua, 1.5f);
					num655.noGravity = true;
					num655.noLight = true;
				}

				if (projectile.localAI[1] > 360)
                {
					NPC FakeNPC = new NPC();
						FakeNPC.SetDefaults(ModContent.NPCType<Cirno>());
					if (Main.netMode == NetmodeID.SinglePlayer)
					{
						int npc = NPC.NewNPC((int)projectile.Center.X, (int)projectile.Center.Y + 24, ModContent.NPCType<Cirno>());
					}
					else
					{
						if (Main.netMode != NetmodeID.Server && Main.myPlayer == projectile.owner)
						{
							ModPacket packet = mod.GetPacket();
							packet.Write((ushort)999);
							packet.Write((int)projectile.Center.X);
							packet.Write((int)projectile.Center.Y);
							packet.Write(ModContent.NPCType<NPCs.Cirno>());
							packet.Write(0);
							packet.Write(0);
							packet.Write(0);
							packet.Write(0);
							packet.Write(projectile.owner);
							packet.Send();
						}
					}

					string typeName2 = FakeNPC.TypeName;
					if (Main.netMode == 0)
					{
						Main.NewText(Language.GetTextValue("Announcement.HasAwoken", typeName2), 175, 75);
					}
					else if (Main.netMode == 2)
					{
						NetMessage.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasAwoken", FakeNPC.GetTypeNetName()), new Color(175, 75, 255));
					}

					Main.PlaySound(SoundID.Roar, (int)projectile.position.X, (int)projectile.position.Y, 0);
					for (float num654 = 0; num654 < 25 + projectile.localAI[1] / 10f; num654+=0.25f)
					{
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= (float)(num654 / 10.00);
						int num655 = Dust.NewDust(projectile.Center + new Vector2(2, 2) + ogcircle * 16f, 0, 0, 88, -projectile.velocity.X + randomcircle.X * 6f, -projectile.velocity.Y + randomcircle.Y * 6f, 150, Color.Aqua, 1.6f);
						Main.dust[num655].noGravity = true;
						Main.dust[num655].noLight = true;
					}
					projectile.Kill();

				}

			}

		}

	}

	public class CaliburnCompess : BaseBossSummon
	{
		private float effect = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Caliburn Compass");
			Tooltip.SetDefault("When held, it points to Caliburn Altars in your world\nCan be used in Hardmode to fight a stronger Caliburn spirit\nNon-Consumable");
		}
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.rare = 2;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 4;
		}

		public override bool CanUseItem(Player player)
		{
			if (!NPC.AnyNPCs(mod.NPCType("CaliburnGuardianHardmode")) && player.GetModPlayer<SGAPlayer>().DankShrineZone && Main.hardMode)
			{
				return base.CanUseItem(player);
			}
			else
			{
				if (player == Main.LocalPlayer)
					Main.NewText("The compass points the way to a shrine...", 0, 75, 0);
				return false;
			}
		}

		public override bool UseItem(Player player)
		{
			if (Main.hardMode && player.GetModPlayer<SGAPlayer>().DankShrineZone)
			{
				NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("CaliburnGuardianHardmode"));
				Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
			}
			return true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (SGAWorld.darknessVision)
			{
				tooltips.Add(new TooltipLine(mod, "CaliburnCompessUpgrade", Idglib.ColorText(Color.MediumPurple, "Upgraded to also point to Dark Sectors in the world")));
				tooltips.Add(new TooltipLine(mod, "CaliburnCompessUpgrade", Idglib.ColorText(Color.MediumPurple, "Darkness from Dark Sectors is reduced while in your inventory")));
			}
		}

	}
	public class Mechacluskerf : BaseBossSummon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mechanical Clusterfuck");
			Tooltip.SetDefault("Summons the Twin-Prime-Destroyers\nIt is highly encouraged you do not fight this before late Hardmode...");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.consumable = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 2;
			item.useAnimation = 2;
			item.useStyle = 4;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.value = 0;
			item.rare = 9;
			item.UseSound = SoundID.Item1;
		}

		public override bool CanUseItem(Player player)
		{
			if (!NPC.AnyNPCs(mod.NPCType("TPD")) && !NPC.AnyNPCs(50))
			{
				if (Main.dayTime)
				{
					item.consumable = false;
				}
				else
				{
					item.consumable = true;
				}
				return base.CanUseItem(player);
			}
			else
			{
				return false;
			}
		}
		public override bool UseItem(Player player)
		{
			if (item.consumable == false || Main.dayTime)
			{
				if (player == Main.LocalPlayer)
					Main.NewText("Their terror only rings at night", 150, 5, 5);
			}
			else
			{
				NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("TPD"));
				Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			//recipe.AddIngredient(ItemID.LunarBar, 10);
			recipe.AddIngredient(544, 1);
			recipe.AddIngredient(556, 1);
			recipe.AddIngredient(557, 1);
			recipe.AddIngredient(547, 3);
			recipe.AddIngredient(548, 3);
			recipe.AddIngredient(549, 3);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class TruelySusEye : BaseBossSummon
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Truly Suspicious Looking Eye");
			Tooltip.SetDefault("Summons the Servants of the lord of the moon...\nOnly useable after Tier 3 Old One's Army event and Martians are beaten" +
				"\nUse at Night\nDoesn't work online");
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (SGAmod.Calamity.Item1)
				tooltips.Add(new TooltipLine(mod, "NoU", "Summoning this boss with automatically disable Revengence and Death Modes"));
		}
		public override bool CanUseItem(Player player)
	{
			if ((DD2Event.DownedInvasionT3 && NPC.downedMartians) && !Main.dayTime && Main.netMode<1)
			{
				if (NPC.CountNPCS(mod.NPCType("Harbinger"))<1 && NPC.CountNPCS(NPCID.MoonLordFreeEye) < 1)
				{
					return base.CanUseItem(player);
				}
				else
				{
					return false;
				}
			}
			else
			{
				Main.NewText("No...", 0, 0, 75);
				return false;
			}
	}
	public override bool UseItem(Player player)
	{
			SGAmod.CalamityNoRevengenceNoDeathNoU();
			NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("Harbinger"));
		//Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
		return true;
	}

	public override void AddRecipes()
	{
		ModRecipe recipe = new ModRecipe(mod);
		//recipe.AddIngredient(ItemID.LunarBar, 10);
		recipe.AddIngredient(ItemID.Ectoplasm, 5);
		recipe.AddIngredient(ItemID.SuspiciousLookingEye, 1);
		recipe.AddTile(TileID.CrystalBall);
		recipe.SetResult(this);
		recipe.AddRecipe();
	}

	public override void SetDefaults()
	{
		item.rare = 9;
		item.maxStack = 999;
		item.consumable = true;
		item.width = 40;
		item.height = 40;
		item.useTime = 30;
		item.useAnimation = 30;
		item.useStyle = 4;
		item.noMelee = true; //so the item's animation doesn't do damage
		item.value = 0;
		item.UseSound = SoundID.Item8;
	}


}

#if DefineHellionUpdate
	public class HellionSummon : BaseBossSummon
	{
		public static ModItem instance;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reality's Sunder");
		}

		public override bool ConsumeItem(Player player)
		{
			return false;
		}

		public override bool Autoload(ref string name)
		{
			instance = this;
			return true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (SGAWorld.downedHellion < 2)
			{
				TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Material" && x.mod == "Terraria");
				if (tt != null)
				{
					int index = tooltips.FindIndex(here => here == tt);
					tooltips.RemoveAt(index);
				}
			}
			else
			{
				tooltips.Add(new TooltipLine(mod, "Nmxx", "Useable in crafting"));
			}

			if (SGAWorld.downedSPinky && SGAWorld.downedCratrosityPML && SGAWorld.downedWraiths>3)
			{
				if (SGAWorld.downedHellion > 0)
					tooltips.Add(new TooltipLine(mod, "Nmxx", "Hold 'left control' while you use the item to skip Hellion Core, this costs 25 Souls of Byte"));
				if (SGAWorld.downedHellion < 2)
				{
					if (SGAWorld.downedHellion == 0)
					{
						tooltips.Add(new TooltipLine(mod, "Nmxx", "'Well done " + SGAmod.HellionUserName + ". Yes, I know your real name behind that facade you call " + Main.LocalPlayer.name + ".'"));
						tooltips.Add(new TooltipLine(mod, "Nmxx", "'And thanks to your Dragon's signal, I have found my way to your world, this one tear which will let me invade your puny little " + Main.worldName + "'"));
						tooltips.Add(new TooltipLine(mod, "Nmxx", "'Spend what little time you have left meaningful, if you were expecting to save him, I doubt it'"));
						tooltips.Add(new TooltipLine(mod, "Nmxx", "'But let us not waste anymore time, come, face me'"));
					}
					else
					{
						tooltips.Add(new TooltipLine(mod, "Nmxx", "'Getting closer, I guess now I'll just have to use more power to stop you'"));
						tooltips.Add(new TooltipLine(mod, "Nmxx", "'But enough talk, lets finish this'"));
					}
				}
				else
				{
					tooltips.Add(new TooltipLine(mod, "Nmxx", "'Hmp, very Well done " + SGAmod.HellionUserName + ", you've bested me, this time"));
					tooltips.Add(new TooltipLine(mod, "Nmxx", "But next time you won't be so lucky..."));
					tooltips.Add(new TooltipLine(mod, "Nmxx", "My tears have stabilized..."));
					tooltips.Add(new TooltipLine(mod, "Nmxx", "Enjoy your fancy reward, you've earned that much..."));
					tooltips[0].text += " (Stabilized)";
				}
				tooltips.Add(new TooltipLine(mod, "Nmxx", "Tears a hole in the bastion of reality to bring forth the Paradox General, Helen 'Hellion' Weygold"));
				tooltips.Add(new TooltipLine(mod, "Nmxx", "Non Consumable"));


					foreach (TooltipLine line in tooltips)
				{
					string text = line.text;
					string newline = "";
					for (int i = 0; i < text.Length; i += 1)
					{
						newline += Idglib.ColorText(Color.Lerp(Color.White, Main.hslToRgb((Main.rand.NextFloat(0, 1)) % 1f, 0.75f, Main.rand.NextFloat(0.25f, 0.5f)),MathHelper.Clamp(0.5f+(float)Math.Sin(Main.GlobalTime*2f)/1.5f,0.2f,1f)), text[i].ToString());


					}
					line.text = newline;
				}
			}
			else
			{
				tooltips = new List<TooltipLine>();
			}

		}

		public override bool CanUseItem(Player player)
		{
			if (Main.netMode > 0)
			{
				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					Hellion hell = new Hellion();
					hell.HellionTaunt("This fight is not possible in Multiplayer, comeback in Single Player");
				}
				return false;
            }

			if (Hellion.GetHellion()==null && !IdgNPC.bossAlive && SGAWorld.downedSPinky && SGAWorld.downedCratrosityPML && SGAWorld.downedWraiths > 3 && NPC.CountNPCS(mod.NPCType("HellionMonolog"))<1)
			{
				if (!Main.expertMode)
				{
					Hellion hell = new Hellion();
					hell.HellionTaunt("What makes you think I'm going to challenge a NORMAL difficulty player? You shouldn't even have this item, cheater...");
					return false;
				}
				return base.CanUseItem(player);
			}
			else
			{
				return false;
			}
		}
		public override bool UseItem(Player player)
		{
			if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl) && player.CountItem(mod.ItemType("ByteSoul")) > 24 && SGAWorld.downedHellion > 0)
			{
				for (int i = 0; i < 25; i += 1)
				{
					player.ConsumeItem(mod.ItemType("ByteSoul"));
				}
				NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("Hellion"));
			}
			else
			{
				NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("HellionCore"));
			}
			//Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			//recipe.AddIngredient(ItemID.LunarBar, 10);
			recipe.AddIngredient(mod.ItemType("WraithCoreFragment3"), 1);
			recipe.AddIngredient(mod.ItemType("RoilingSludge"), 1);
			recipe.AddIngredient(mod.ItemType("Mechacluskerf"), 1);
			recipe.AddIngredient(mod.ItemType("Nineball"), 1);
			recipe.AddIngredient(mod.ItemType("AcidicEgg"), 1);
			recipe.AddIngredient(mod.ItemType("Prettygel"), 1);
			recipe.AddIngredient(mod.ItemType("ConchHorn"), 1);
			recipe.AddIngredient(mod.ItemType("CosmicFragment"), 1);
			recipe.AddIngredient(mod.ItemType("MoneySign"), 10);
			recipe.AddIngredient(mod.ItemType("LunarRoyalGel"), 20);


			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void SetDefaults()
		{
			item.rare = 12;
			item.maxStack = 1;
			item.consumable = false;
			item.width = 40;
			item.height = 40;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 4;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.value = 0;
			item.rare = -1;
			item.expert = true;
			item.UseSound = SoundID.Item8;
		}

		public override string Texture
		{
			get { return ("Terraria/Extra_19"); }
		}

		private static void drawit(Vector2 where, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI, Matrix zoomitz)
		{

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, zoomitz);

			int width = 32; int height = 32;

			Texture2D beam = new Texture2D(Main.graphics.GraphicsDevice, width, height);
			var dataColors = new Color[width * height];

			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					float dist = (new Vector2(x, y) - new Vector2(width / 2, height / 2)).Length();
					if (Main.rand.NextFloat(dist, 32)<16f)
					{
						float alg = ((-Main.GlobalTime + ((float)(dist) / 4f)) / 2f);
						dataColors[x + y * width] = Main.hslToRgb(alg % 1f, 0.75f, 0.5f);
					}
				}
			}

			beam.SetData(0, null, dataColors, 0, width * height);
			spriteBatch.Draw(beam, where + new Vector2(2, 2), null, Color.White, 0, new Vector2(beam.Width / 2, beam.Height / 2), scale * 2f, SpriteEffects.None, 0f);

		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			float gg = 0f;
			drawit(position + new Vector2(11, 11), spriteBatch, drawColor, drawColor, ref gg, ref scale, 1, Main.UIScaleMatrix);
			return false;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{


			drawit(item.Center - Main.screenPosition, spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI, Main.GameViewMatrix.ZoomMatrix);
			return false;
		}

	}
#endif


}
