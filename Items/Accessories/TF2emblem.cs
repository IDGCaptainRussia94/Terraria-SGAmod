using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Idglibrary;
using AAAAUThrowing;
using System.IO;
using Terraria.ModLoader.IO;
using System.Linq;

namespace SGAmod.Items.Accessories
{

	public class TF2Emblem : ModItem
	{
		protected virtual int _xpRequiredToMax => Item.buyPrice(0, 15, 0, 0);
		public int XpRequiredToMax => _xpRequiredToMax;
		public float XpPercent => (xp.Item1 / (float)_xpRequiredToMax);

		public (int, bool) xp = (0, false);

        public override bool CloneNewInstances => true;

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("TF2 Emblem");
			Tooltip.SetDefault("5% increased crit chance and damage, 25% increased throwing velocity");
		}
		public static TF2Emblem GetPlayerEmblem(Player player, bool lookataccessories,int matchtype = -1)
		{
			Item[] tf2em;
			int index = -1;
			if (lookataccessories)
			{
				for (int i = 0; i < player.armor.Length; i += 1)
				{
					Item testby = player.armor[i];
					if (testby?.modItem is TF2Emblem && (matchtype < 0 || testby.type == matchtype))
					{
						index = i;
						break;
					}

				}
			}
			else
			{
				for (int i = 0; i < player.inventory.Length; i += 1)
				{
					Item testby = player.inventory[i];
					if (testby?.modItem is TF2Emblem && (matchtype < 0 || testby.type == matchtype))
					{
						index = i;
						break;
					}
				}
			}

			if (index>-1)
			{
				return lookataccessories ? player.armor[index]?.modItem as TF2Emblem : player.inventory[index]?.modItem as TF2Emblem;
			}
			return null;
		}

		public static void AwardXpToPlayer(Player player, int xp)
        {
			TF2Emblem playerhasemblem = GetPlayerEmblem(Main.LocalPlayer, true);
			if (playerhasemblem != null)
			{
				if (playerhasemblem.xp.Item2)
					return;

				//CombatText.NewText(player.Hitbox, Color.White, "XP earned: " + xp, true, true);
				playerhasemblem.xp.Item1 += xp;
				playerhasemblem.CapXpIfCapped(true);
			}

        }

		public static bool CanCraftUp(Recipe recipe)
        {
			if (recipe.createItem?.modItem is TF2Emblem)
			{
				foreach (Item item in recipe.requiredItem)
				{
					if (item?.modItem is TF2Emblem tf2emblemrequired)
					{
						//foreach (Item item2 in Main.LocalPlayer.inventory.Where(testby => testby.type == item.type))
						//{
						TF2Emblem playerhasemblem = GetPlayerEmblem(Main.LocalPlayer, false, item.type);
							if (!playerhasemblem.xp.Item2)
								return false;
						//}
					}
				}
			}
			return true;
		}

		public override bool CanEquipAccessory(Player player, int slot)
		{
			bool canequip = true;
			for (int x = 3; x < 8 + player.extraAccessorySlots; x++)
			{
				if (player.armor[x].modItem != null)
				{
					int myType = (player.armor[x]).type;
					Type myclass = player.armor[x].modItem.GetType();
					if (myclass.BaseType == typeof(TF2Emblem) || myclass == typeof(TF2Emblem) || myclass.IsSubclassOf(typeof(TF2Emblem)))
					{

						canequip = false;
						break;
					}
				}
			}
			return canequip;
		}
		public void CapXpIfCapped(bool dofanfare = false)
		{
			if (xp.Item1 >= XpRequiredToMax)
			{
				xp.Item1 = XpRequiredToMax;
				xp.Item2 = true;

				if (dofanfare)
				{
					if (item.owner >= 0 && item.owner < 255)
					{
						Player player = Main.player[item.owner];
						Projectile.NewProjectile(player.Center, Vector2.UnitY * -5f, ProjectileID.ConfettiGun, 0, 0);
						Main.PlaySound(SGAmod.Instance.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/achievement_earned"), player.Center);
					}

				}
			}
		}

		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(xp.Item1);
		}
		public override void NetRecieve(BinaryReader reader)
		{
			xp.Item1 = reader.ReadInt32();
			CapXpIfCapped();
		}
		public override TagCompound Save()
		{
			TagCompound tag = new TagCompound
			{
				["tf2xp"] = xp.Item1
			};
			return tag;
		}
		public override void Load(TagCompound tag)
		{
			if (tag.ContainsKey("tf2xp"))
			{
				xp.Item1 = tag.GetInt("tf2xp");
				CapXpIfCapped();
			}
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (xp.Item2)
				tooltips.Add(new TooltipLine(mod, "Tf2Elem", Idglib.ColorText(Main.hslToRgb((Main.GlobalTime * 3f) % 1f, 1f, 0.75f), "MAXED!")));
			else
				tooltips.Add(new TooltipLine(mod, "Tf2Elem", "Contract Xp: " + xp.Item1 + "/" + XpRequiredToMax + " (" + XpPercent * 100f + "%)"));

			if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
			{
				tooltips.Add(new TooltipLine(mod, "Tf2Elem", Idglib.ColorText(Color.Orange, "Emblems gain experience as you slay enemies, gradually gaining their stats")));
				tooltips.Add(new TooltipLine(mod, "Tf2Elem", Idglib.ColorText(Color.Orange, "At max experience, they fully gain their listed stats")));
				tooltips.Add(new TooltipLine(mod, "Tf2Elem", Idglib.ColorText(Color.Orange, "Emblems can only be crafted into their higher ranks at max experience")));
			}
			else
			{
				tooltips.Add(new TooltipLine(mod, "Tf2Elem", Idglib.ColorText(Color.Orange, "(hold down LEFT CONTROL for more info)")));
			}
			tooltips.Add(new TooltipLine(mod, "Tf2Elem", "You may wear only one TF2 Emblem at a time"));
			if (GetType() != typeof(TF2Emblem))
				tooltips.Add(new TooltipLine(mod, "Tf2Elem", "Includes all bonuses from lower tier emblems"));
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Orange;
			item.accessory = true;
		}

		public virtual void GrantBuffs(Player player, bool hideVisual)
        {
			float percent = (xp.Item2 || (GetType() != typeof(TF2Emblem))) ? 1f : XpPercent;
			player.BoostAllDamage(0.05f* percent, (int)(5* percent));
			player.Throwing().thrownVelocity += 0.25f* percent;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			//player.GetModPlayer<SGAPlayer>().Havoc = 1;
			/*player.Throwing().thrownCrit += 5;
			player.rangedCrit += 5;
			player.meleeCrit += 5;
			player.magicCrit += 5;
			player.magicDamage += 0.05f;
			player.rangedDamage += 0.05f;
			player.meleeDamage += 0.05f;
			player.minionDamage += 0.05f;
			player.Throwing().thrownDamage += 0.05f;*/
			GrantBuffs(player,hideVisual);
			player.SGAPly().tf2emblemLevel = 1;
		}
	}

	public class TF2EmblemRed : TF2Emblem
	{
		protected override int _xpRequiredToMax => Item.buyPrice(0, 50, 0, 0);

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("RED Emblem");
			Tooltip.SetDefault("50 Increased Max HP");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Orange;
			item.accessory = true;
		}
		public static void GrantForkedBuffs(TF2Emblem thisguy, Player player, bool hideVisual)
		{
			float percent = (thisguy.xp.Item2 || (thisguy.GetType() != typeof(TF2EmblemRed))) ? 1f : thisguy.XpPercent;
			player.statLifeMax2 += (int)(50 * percent);
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			//player.statLifeMax2 += 50;
			base.UpdateAccessory(player, hideVisual);
			GrantForkedBuffs(this, player, hideVisual);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<TF2Emblem>(), 1);
			recipe.AddIngredient(ModContent.ItemType<HavocGear.Items.BiomassBar>(), 6);
			recipe.AddIngredient(ItemID.HealingPotion, 30);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class TF2EmblemBlu : TF2Emblem
	{
		protected override int _xpRequiredToMax => Item.buyPrice(0, 50, 0, 0);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("BLU Emblem");
			Tooltip.SetDefault("100 Increased Max Mana and 5% mana cost reduction");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Orange;
			item.accessory = true;
		}
		public static void GrantForkedBuffs(TF2Emblem thisguy,Player player, bool hideVisual)
		{
			float percent = (thisguy.xp.Item2 || (thisguy.GetType() != typeof(TF2EmblemBlu))) ? 1f : thisguy.XpPercent;
			player.statManaMax2 += (int)(100 * percent);
			player.manaCost -= 0.05f * percent;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			GrantForkedBuffs(this,player,hideVisual);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<TF2Emblem>(), 1);
			recipe.AddIngredient(ModContent.ItemType<HavocGear.Items.BiomassBar>(), 6);
			recipe.AddIngredient(ItemID.ManaPotion, 30);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class TF2EmblemCommando : TF2Emblem
	{
		protected override int _xpRequiredToMax => Item.buyPrice(1, 0, 0, 0);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Commando Emblem");
			Tooltip.SetDefault("5% extra throwing damage");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 7, 50, 0);
			item.rare = 7;
			item.accessory = true;
		}
		public override void GrantBuffs(Player player, bool hideVisual)
		{
			base.GrantBuffs(player, hideVisual);
			float percent = (xp.Item2 || (GetType() != typeof(TF2EmblemCommando))) ? 1f : XpPercent;
			player.Throwing().thrownDamage += 0.05f * percent;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			TF2EmblemBlu.GrantForkedBuffs(this, player, hideVisual);
			TF2EmblemRed.GrantForkedBuffs(this, player, hideVisual);
			player.SGAPly().tf2emblemLevel = 2;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<TF2EmblemRed>(), 1);
			recipe.AddIngredient(ModContent.ItemType<TF2EmblemBlu>(), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class TF2EmblemAssassin : TF2EmblemCommando
	{
		protected override int _xpRequiredToMax => Item.buyPrice(4, 0, 0, 0);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Assassin Emblem");
			Tooltip.SetDefault("5% increased crit chance and damage, 10% increased throwing velocity\n25 increased Max HP and +1 Max Minions\n25 Increased Max Mana and 2.5% mana cost reduction\n5% extra throwing damage");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(1, 0, 0, 0);
			item.rare = 9;
			item.accessory = true;
		}

		public override void GrantBuffs(Player player, bool hideVisual)
		{
			base.GrantBuffs(player, hideVisual);
			float percent = (xp.Item2 || (GetType() != typeof(TF2EmblemAssassin))) ? 1f : XpPercent;

			player.BoostAllDamage(0.05f* percent, (int)(5*percent));

			player.Throwing().thrownDamage += 0.05f*percent;

			player.Throwing().thrownVelocity += 0.1f*percent;
			player.manaCost -= 0.025f * percent;
			if (percent>=1)
			player.maxMinions += 1;
			player.statLifeMax2 += (int)(25 * percent);
			player.statManaMax2 += (int)(25 * percent);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			/*player.Throwing().thrownCrit += 5;
			player.rangedCrit += 5;
			player.meleeCrit += 5;
			player.magicCrit += 5;
			player.magicDamage += 0.05f;
			player.rangedDamage += 0.05f;
			player.meleeDamage += 0.05f;
			player.minionDamage += 0.05f;
			SGAmod.BoostModdedDamage(player, 0.05f, 5);*/
			base.UpdateAccessory(player, hideVisual);
			player.SGAPly().tf2emblemLevel = 3;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<TF2EmblemCommando>(), 1);
			recipe.AddIngredient(ItemID.DestroyerEmblem, 1);
			recipe.AddIngredient(ModContent.ItemType<PrismalBar>(), 6);
			recipe.AddIngredient(ItemID.CrystalBall, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}


	public class TF2EmblemElite : TF2EmblemAssassin
	{
		protected override int _xpRequiredToMax => Item.buyPrice(10, 0, 0, 0);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elite Emblem");
			Tooltip.SetDefault("10% increased damage, 15% increased throwing velocity\n25 increased Max HP and +1 Max Minions\n25 Increased Max Mana and 2.5% mana cost reduction\n15% extra throwing damage\nEffects of MVM Upgrade");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.rare = 12;
			item.accessory = true;
		}
		public override void GrantBuffs(Player player, bool hideVisual)
		{
			base.GrantBuffs(player, hideVisual);
			float percent = (xp.Item2 || (GetType() != typeof(TF2EmblemElite))) ? 1f : XpPercent;

			player.BoostAllDamage(0.10f*percent, 0);

			player.Throwing().thrownVelocity += 0.15f*percent;
			player.statManaMax2 += (int)(25 * percent);
			player.statLifeMax2 += (int)(25 * percent);
			player.manaCost -= 0.025f * percent;
			if (percent>=1)
			player.maxMinions += 1;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			ModContent.GetInstance<MVMUpgrade>().UpdateAccessory(player, hideVisual);
			player.SGAPly().tf2emblemLevel = 4;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType <TF2EmblemAssassin>(), 1);
			recipe.AddIngredient(ModContent.ItemType <MVMUpgrade>(), 1);
			recipe.AddIngredient(ItemID.ManaCrystal, 3);
			recipe.AddIngredient(ModContent.ItemType <StarMetalBar>(), 16);
			recipe.AddIngredient(ModContent.ItemType <LunarRoyalGel>(), 15);
			recipe.AddIngredient(ModContent.ItemType <MoneySign>(), 10);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

}