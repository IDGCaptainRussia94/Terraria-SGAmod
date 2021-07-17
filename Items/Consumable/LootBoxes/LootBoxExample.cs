using Microsoft.Xna.Framework;
using Terraria;
using System;
using System.Linq;
using System.Collections.Generic;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Utilities;
using SGAmod.Items.Accessories;
using SGAmod.NPCs.TownNPCs;
using Idglibrary;

namespace SGAmod.Items.Consumable.LootBoxes
{

	public class LootBoxDeeperDungeons : LootBoxAccessories
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Deeper Dungeons Loot : Contraband Crate!");
			Tooltip.SetDefault("'Totally banned in Norway... Aparently also in Terraria'\nWill contain an item exclusive to the Deeper Dungeons");
		}

		public override string Texture
		{
			get { return ("Terraria/Item_" + ItemID.LockBox); }
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = 8;
			item.value = 1000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.shoot = mod.ProjectileType("LootBoxOpenDeeperDungeons");
		}
	}

	public class LootBoxOpenDeeperDungeons : LootBoxOpenAccessories
	{
		protected override int size => 48;
		protected override float speed => 0.15f;
		protected override int maxItems => 1000;
		protected override int slowDownRate => 200;
		protected override int itemsVisible => 10;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Loot Box?");
		}

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.ignoreWater = true;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;

			projectile.scale = 0;//Prefer to keep this at 0


			projectile.timeLeft = 800 + Main.rand.Next(-100, 300);
			projectile.localAI[0] = Main.rand.Next(40, 60);
		}


		protected override void FillLootBox(WeightedRandom<LootBoxContents> WR)
		{
			foreach(int itemtype in Dimensions.DeeperDungeon.CommonItems)
			WR.Add(new LootBoxContents(itemtype, 1), 1);
			foreach (int itemtype in Dimensions.DeeperDungeon.RareItems)
				WR.Add(new LootBoxContents(itemtype, 1), 0.25);

			loots.Add(WR.Get());
		}
	}

	public class LootBoxVanillaHardmodePotions : LootBoxAccessories
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hardmode Potions : Contraband Crate!");
			Tooltip.SetDefault("'Totally banned in Norway... Aparently also in Terraria'\nWill contain a random ammount of one type of Vanilla hardmode potion\nMay contain Super Healing or Super Mana potions");
		}

		public override string Texture
		{
			get { return ("Terraria/Item_" + ItemID.LockBox); }
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = 8;
			item.value = 1000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.shoot = mod.ProjectileType("LootBoxOpenVanillaHardmodePotions");
		}
	}

	public class LootBoxOpenVanillaHardmodePotions : LootBoxOpenAccessories
	{
		protected override int size => 48;
		protected override float speed => 0.15f;
		protected override int maxItems => 1000;
		protected override int slowDownRate => 200;
		protected override int itemsVisible => 10;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Loot Box?");
		}

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.ignoreWater = true;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;

			projectile.scale = 0;//Prefer to keep this at 0


			projectile.timeLeft = 800 + Main.rand.Next(-100, 300);
			projectile.localAI[0] = Main.rand.Next(40, 60);
		}


		protected override void FillLootBox(WeightedRandom<LootBoxContents> WR)
		{
			WR.Add(new LootBoxContents(ItemID.InfernoPotion, 1 + Main.rand.Next(Main.rand.Next(2, 15))), 1);
			WR.Add(new LootBoxContents(ItemID.ObsidianSkinPotion, 1 + Main.rand.Next(Main.rand.Next(2, 15))), 1);
			WR.Add(new LootBoxContents(ItemID.LifeforcePotion, 1 + Main.rand.Next(Main.rand.Next(2, 15))), 1);
			WR.Add(new LootBoxContents(ItemID.EndurancePotion, 1 + Main.rand.Next(Main.rand.Next(2, 15))), 1);
			WR.Add(new LootBoxContents(ItemID.TeleportationPotion, 1 + Main.rand.Next(Main.rand.Next(2, 15))), 1);
			WR.Add(new LootBoxContents(ItemID.GravitationPotion, 1 + Main.rand.Next(Main.rand.Next(2, 15))), 1);
			WR.Add(new LootBoxContents(ItemID.SuperHealingPotion, 5 + Main.rand.Next(Main.rand.Next(5, 15))), 0.5);
			WR.Add(new LootBoxContents(ItemID.SuperManaPotion, 5 + Main.rand.Next(Main.rand.Next(5, 15))), 0.5);

			loots.Add(WR.Get());
		}
	}

	public class LootBoxVanillaPotions : LootBoxAccessories
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Potions : Contraband Crate!");
			Tooltip.SetDefault("'Totally banned in Norway... Aparently also in Terraria'\nWill contain a random ammount of one type of Vanilla prehardmode potion\nMay contain Greater Healing or Greater Mana potions");
		}

		public override string Texture
		{
			get { return ("Terraria/Item_" + ItemID.LockBox); }
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = ItemRarityID.Orange;
			item.value = 1000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.shoot = mod.ProjectileType("LootBoxOpenVanillaPotions");
		}
	}

	public class LootBoxOpenVanillaPotions : LootBoxOpenAccessories
	{
		protected override int size => 48;
		protected override float speed => 0.15f;
		protected override int maxItems => 1000;
		protected override int slowDownRate => 200;
		protected override int itemsVisible => 10;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Loot Box?");
		}

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.ignoreWater = true;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;

			projectile.scale = 0;//Prefer to keep this at 0


			projectile.timeLeft = 800 + Main.rand.Next(-100, 300);
			projectile.localAI[0] = Main.rand.Next(40, 60);
		}


		protected override void FillLootBox(WeightedRandom<LootBoxContents> WR)
		{
			WR.Add(new LootBoxContents(ItemID.RegenerationPotion, 1 + Main.rand.Next(Main.rand.Next(2, 15))), 1);
			WR.Add(new LootBoxContents(ItemID.SwiftnessPotion, 1 + Main.rand.Next(Main.rand.Next(2, 15))), 1);
			WR.Add(new LootBoxContents(ItemID.IronskinPotion, 1 + Main.rand.Next(Main.rand.Next(2, 15))), 1);
			WR.Add(new LootBoxContents(ItemID.WrathPotion, 1 + Main.rand.Next(Main.rand.Next(2, 15))), 1);
			WR.Add(new LootBoxContents(ItemID.RagePotion, 1 + Main.rand.Next(Main.rand.Next(2, 15))), 1);
			WR.Add(new LootBoxContents(ItemID.TitanPotion, 1 + Main.rand.Next(Main.rand.Next(2, 15))), 1);
			WR.Add(new LootBoxContents(ItemID.SummoningPotion, 1 + Main.rand.Next(Main.rand.Next(2, 15))), 1);
			WR.Add(new LootBoxContents(ItemID.FishingPotion, 1 + Main.rand.Next(Main.rand.Next(2, 15))), 1);
			WR.Add(new LootBoxContents(ItemID.CratePotion, 1 + Main.rand.Next(Main.rand.Next(2, 15))), 1);
			WR.Add(new LootBoxContents(ItemID.ThornsPotion, 1 + Main.rand.Next(Main.rand.Next(2, 15))), 1);
			WR.Add(new LootBoxContents(ItemID.HeartreachPotion, 1 + Main.rand.Next(Main.rand.Next(2, 15))), 1);
			WR.Add(new LootBoxContents(ItemID.InvisibilityPotion, 1 + Main.rand.Next(Main.rand.Next(2, 15))), 1);
			WR.Add(new LootBoxContents(ItemID.WarmthPotion, 1 + Main.rand.Next(Main.rand.Next(2, 15))), 1);
			WR.Add(new LootBoxContents(ItemID.ArcheryPotion, 1 + Main.rand.Next(Main.rand.Next(2, 15))), 1);
			WR.Add(new LootBoxContents(ItemID.AmmoReservationPotion, 1 + Main.rand.Next(Main.rand.Next(2, 15))), 1);
			WR.Add(new LootBoxContents(ItemID.FlipperPotion, 1 + Main.rand.Next(Main.rand.Next(2, 15))), 1);

			WR.Add(new LootBoxContents(ItemID.GreaterHealingPotion, 5 + Main.rand.Next(Main.rand.Next(5, 15))), 0.5);
			WR.Add(new LootBoxContents(ItemID.GreaterManaPotion, 5 + Main.rand.Next(Main.rand.Next(5, 15))), 0.5);

			loots.Add(WR.Get());
		}
	}

	public class LootBoxPotions : LootBoxAccessories
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("SGAmod Potions : Contraband Crate!");
			Tooltip.SetDefault("'Totally banned in Norway... Aparently also in Terraria'\nWill contain a random ammount of one type of SGAmod potion\nIncludes a few consumables as well");
		}

		public override string Texture
		{
			get { return ("Terraria/Item_" + ItemID.LockBox); }
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = 8;
			item.value = 1000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.shoot = mod.ProjectileType("LootBoxOpenPotions");
		}
	}

	public class LootBoxOpenPotions : LootBoxOpenAccessories
	{
		protected override int size => 48;
		protected override float speed => 0.15f;
		protected override int maxItems => 1000; 
		protected override int slowDownRate => 200;
		protected override int itemsVisible => 10;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Loot Box?");
		}

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.ignoreWater = true;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;

			projectile.scale = 0;//Prefer to keep this at 0


			projectile.timeLeft = 800 + Main.rand.Next(-100, 300);
			projectile.localAI[0] = Main.rand.Next(40, 60);
		}


		protected override void FillLootBox(WeightedRandom<LootBoxContents> WR)
		{
			WR.Add(new LootBoxContents(ModContent.ItemType<ClarityPotion>(), 1 + Main.rand.Next(Main.rand.Next(2,15))), 1);
			WR.Add(new LootBoxContents(ModContent.ItemType<CondenserPotion>(), 1 + Main.rand.Next(Main.rand.Next(2, 15))), 1);
			WR.Add(new LootBoxContents(ModContent.ItemType<EnergyPotion>(), 1 + Main.rand.Next(Main.rand.Next(2, 15))), 1);
			WR.Add(new LootBoxContents(ModContent.ItemType<IceFirePotion>(), 1 + Main.rand.Next(Main.rand.Next(1, 12))), 1);
			WR.Add(new LootBoxContents(ModContent.ItemType<RadCurePotion>(), 1 + Main.rand.Next(Main.rand.Next(2, 15))), 1);
			WR.Add(new LootBoxContents(ModContent.ItemType<TinkerPotion>(), 1 + Main.rand.Next(Main.rand.Next(1, 15))), 1);
			WR.Add(new LootBoxContents(ModContent.ItemType<TriggerFingerPotion>(), 1 + Main.rand.Next(Main.rand.Next(1, 15))), 1);
			WR.Add(new LootBoxContents(ModContent.ItemType<TrueStrikePotion>(), 1 + Main.rand.Next(Main.rand.Next(1, 15))), 1);
			WR.Add(new LootBoxContents(ModContent.ItemType<RagnarokBrew>(), 1+Main.rand.Next(Main.rand.Next(1, 15))), 1);
			WR.Add(new LootBoxContents(ModContent.ItemType<TooltimePotion>(), 1 + Main.rand.Next(Main.rand.Next(2, 15))), 1);
			WR.Add(new LootBoxContents(ModContent.ItemType<DragonsMightPotion>(), 1 + Main.rand.Next(Main.rand.Next(1, 15))), 0.8);
			WR.Add(new LootBoxContents(ModContent.ItemType<IntimacyPotion>(), 1 + Main.rand.Next(Main.rand.Next(1, 8))), 0.5);
			WR.Add(new LootBoxContents(ModContent.ItemType<ToxicityPotion>(), 1 + Main.rand.Next(Main.rand.Next(1, 8))), 0.5);
			WR.Add(new LootBoxContents(ModContent.ItemType<TimePotion>(), 1 + Main.rand.Next(Main.rand.Next(1, 8))), 0.5);

			WR.Add(new LootBoxContents(ModContent.ItemType<ConsumeHell>(), 1 + Main.rand.Next(Main.rand.Next(1, 6))), 1);
			WR.Add(new LootBoxContents(ModContent.ItemType<EnergizerBattery>(), 1 + Main.rand.Next(Main.rand.Next(1, 8))), 1);
			WR.Add(new LootBoxContents(ModContent.ItemType<DivineShower>(), 1+Main.rand.Next(Main.rand.Next(1, 3))), 1);

			loots.Add(WR.Get());
		}
	}


	public class LootBoxAccessoriesEX : LootBoxAccessories
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vanilla EX : Contraband Crate!");
			Tooltip.SetDefault("'100%, absolutely, totally banned in Norway... Aparently also in Terraria'\nWill contain a random high tier vanilla item");
		}

		public override string Texture
		{
			get { return ("Terraria/Item_" + ItemID.LockBox); }
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = ItemRarityID.Expert;
			item.value = 1000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.shoot = mod.ProjectileType("LootBoxOpenAccessoriesEX");
		}
	}


	public class LootBoxOpenAccessoriesEX : LootBoxOpenAccessories
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Loot Box?");
		}

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.ignoreWater = true;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;

			projectile.scale = 0;//Prefer to keep this at 0


			projectile.timeLeft = 800 + Main.rand.Next(-100, 300);
			projectile.localAI[0] = Main.rand.Next(40, 60);
		}

		protected override void FillLootBox(WeightedRandom<LootBoxContents> WR)
		{
			WR.Add(new LootBoxContents(ItemID.PaladinsShield, 1));
			WR.Add(new LootBoxContents(ItemID.CellPhone, 1));
			WR.Add(new LootBoxContents(ItemID.CosmicCarKey, 1));
			WR.Add(new LootBoxContents(ItemID.TerraBlade, 1));
			WR.Add(new LootBoxContents(ItemID.SnowmanCannon, 1));
			WR.Add(new LootBoxContents(ItemID.BlizzardStaff, 1));
			WR.Add(new LootBoxContents(ItemID.DefenderMedal, 50),1);
			WR.Add(new LootBoxContents(ItemID.ReindeerBells, 1));
			WR.Add(new LootBoxContents(ItemID.PaladinsShield, 1));
			WR.Add(new LootBoxContents(ItemID.DefendersForge, 1));
			WR.Add(new LootBoxContents(ItemID.FireGauntlet, 1));
			WR.Add(new LootBoxContents(ItemID.DestroyerEmblem, 1));
			WR.Add(new LootBoxContents(ItemID.CelestialShell, 1));
			WR.Add(new LootBoxContents(ItemID.AnkhShield, 1));
			loots.Add(WR.Get());
		}
	}

	public class LootBoxAccessories : LootBox
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("SGAmod Accessories : Contraband Crate!");
			Tooltip.SetDefault("'Totally banned in Norway... Aparently also in Terraria'\nWill contain a random SGAmod accessory");
		}

		public override string Texture
		{
			get { return ("Terraria/Item_" + ItemID.LockBox); }
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "LootBoxLine", Idglib.ColorText(Color.SkyBlue, "Rare chance at getting special item!")));
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = 8;
			item.value = 1000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.shoot = mod.ProjectileType("LootBoxOpenAccessories");
		}
	}

	public class LootBoxOpenAccessories : LootBoxOpen
	{
		protected override int size => 48; //How Far each entry is spaced apart
		protected override float speed => 0.15f; //How fast the items scroll be, be mindful of this value to not go over the max items!
		protected override int maxItems => 1000; //Max Items of course, try not to set this any higher as it's unneeded stress
		protected override int slowDownRate => 200; //This is how many frames it takes before the ticker comepletely stops when slowing down
		protected override int itemsVisible => 10; //How many items would be visble left as well as right, so really it's twice this value
		//public virtual float trans => 1f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Loot Box?");
		}

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.ignoreWater = true;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;

			projectile.scale = 0;//Prefer to keep this at 0

			
			projectile.timeLeft = 800 + Main.rand.Next(-100, 300);//Adjust how long the ticker goes, again be mindful of the max item count
			projectile.localAI[0] = Main.rand.Next(40, 60);//Starting position, make sure this is higher than itemsVisible
		}

		public override void ExtraItem(WeightedRandom<LootBoxContents> WR)
		{
			WR.Add(new LootBoxContents(ModContent.ItemType<DevArmorItem>(), 1), 0.10*WR.elements.Count);
			loots.Add(WR.Get());
		}

		//Fun part :p, Control what goes into the loot box! This is per item
		protected override void FillLootBox(WeightedRandom<LootBoxContents> WR)
		{
			WR.Add(new LootBoxContents(ModContent.ItemType<RustedBulwark>(), 1), 1);
			WR.Add(new LootBoxContents(ModContent.ItemType<MurkyCharm>(), 1), 1);
			WR.Add(new LootBoxContents(ModContent.ItemType<StarCollector>(), 1), 1);
			WR.Add(new LootBoxContents(ModContent.ItemType<EnchantedShieldPolish>(), 1), 1);
			WR.Add(new LootBoxContents(ModContent.ItemType<NormalQuiver>(), 1), 1);
			WR.Add(new LootBoxContents(ModContent.ItemType<MagusSlippers>(), 1), 1);
			WR.Add(new LootBoxContents(ModContent.ItemType<YoyoTricks>(), 1), 1);
			WR.Add(new LootBoxContents(ModContent.ItemType<RingOfRespite>(), 1), 1);
			WR.Add(new LootBoxContents(ModContent.ItemType<ThrowerPouch>(), 1), 1);
			WR.Add(new LootBoxContents(ModContent.ItemType<BlinkTech>(), 1), 1);
			WR.Add(new LootBoxContents(ModContent.ItemType<LifeFlower>(), 1), 1);
			WR.Add(new LootBoxContents(ModContent.ItemType<RestorationFlower>(), 1), 1);
			WR.Add(new LootBoxContents(ModContent.ItemType<GeyserInABottle>(), 1), 1);
			WR.Add(new LootBoxContents(ModContent.ItemType<Jabbawacky>(), 1), 1);
			WR.Add(new LootBoxContents(ModContent.ItemType<AversionCharm>(), 1), 1);
			WR.Add(new LootBoxContents(ModContent.ItemType<CobaltMagnet>(), 1), 1);
			WR.Add(new LootBoxContents(ModContent.ItemType<AmberGlowSkull>(), 1), 1);
			WR.Add(new LootBoxContents(ModContent.ItemType<TwinesOfFate>(), 1), 0.80);
			WR.Add(new LootBoxContents(ModContent.ItemType<HeartOfEntropy>(), 1), 0.75);
			WR.Add(new LootBoxContents(ModContent.ItemType<TidalCharm>(), 1), 0.75);
			WR.Add(new LootBoxContents(ModContent.ItemType<FluidDisplacer>(), 1), 0.75);
			WR.Add(new LootBoxContents(ModContent.ItemType<FridgeFlamesCanister>(), 1), 0.6);
			WR.Add(new LootBoxContents(ModContent.ItemType<TerraDivingGear>(), 1), 0.2);
			WR.Add(new LootBoxContents(ModContent.ItemType<OmegaSigil>(), 1), 0.2);
			WR.Add(new LootBoxContents(ModContent.ItemType<BloodCharmPendant>(), 1), 0.2);
			WR.Add(new LootBoxContents(ModContent.ItemType<YoyoGauntlet>(), 1), 0.2);

			loots.Add(WR.Get());
		}

		float tickeffect = 0f;

		public override void AI()
		{
			tickeffect = Math.Max(0f, tickeffect - 1f);
			base.AI();
		}

		protected override void TickEffect()
		{
			tickeffect = 15;
			//Lets you play a sound or otherwise make a client sided effect when the counter ticks over something
			Main.PlaySound(12, -1, -1, 0, 1f, 0.6f);
		}

		protected override void AwardItem(int itemtype)
		{
			//Lets you make effects when you get the item, keep in mind this is CLIENT SIDED!
		}

		//Copy and override PreDraw to make drawing changes of your own!
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D ticker = mod.GetTexture("MatrixArrow");


			Player player = Main.player[projectile.owner];
			for (int f = 0; f < loots.Count; f += 1)
			{
				int lootboxsize = size;
				float offsetsizer = (projectile.localAI[0] * -lootboxsize) + ((float)lootboxsize * f);
				Vector2 hereas = new Vector2((f - projectile.localAI[0]) * (float)lootboxsize, -64);

				Vector2 drawPos = ((hereas * projectile.scale) + projectile.Center) - Main.screenPosition;
				Color glowingcolors1 = Color.White;

				float alpha = MathHelper.Clamp(1f - Math.Abs((((float)f - projectile.localAI[0]) / (float)itemsVisible)), 0f, 1f);

				if (alpha > 0f)
				{

					if ((int)projectile.localAI[0] == f)
						glowingcolors1 = Color.Red;

					Texture2D tex = Main.itemTexture[loots[f].intid];
					spriteBatch.Draw(tex, drawPos, null, glowingcolors1 * projectile.scale * alpha, 0, new Vector2(tex.Width / 2, tex.Height / 2), (0.5f + (0.5f * alpha)) * projectile.scale, SpriteEffects.None, 0f);

				}
			}

			spriteBatch.Draw(ticker, projectile.Center+(new Vector2(0,-28-64) * projectile.scale) - Main.screenPosition, null, Color.White * projectile.scale, tickeffect*0.04f, new Vector2(ticker.Width/2f, 8), new Vector2(1, 1) * projectile.scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(ticker, projectile.Center + (new Vector2(0, 22-64) * projectile.scale) - Main.screenPosition, null, Color.White * projectile.scale, (float)Math.PI-(tickeffect * 0.04f), new Vector2(ticker.Width/2f, 8), new Vector2(1, 1) * projectile.scale, SpriteEffects.None, 0f);
			return false;
		}


	}



}