using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Idglibrary;
using Terraria.ModLoader.IO;
using System.Security.AccessControl;
using Microsoft.Xna.Framework.Audio;
using System.Security.Cryptography;
using System.IO;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.GameContent.Events;
using System.Collections.ObjectModel;

namespace SGAmod.Items.Consumables
{

	public class PowerToolUpgrade : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Upgrade: Power Tools");
			Tooltip.SetDefault("Upgrades motorized tools, improving their speed by 25%\n"+Idglib.ColorText(Color.Red,"Consume Electric Charge equivalent to their highest tool power")+ "\nRight Click while holding a motorized tool to apply\nOnly one upgrade may be applied at a time");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 30;
			item.rare = ItemRarityID.Orange;
			item.value = 5000;
			item.useTurn = true;
			item.UseSound = SoundID.Item2;
		}

		public override bool CanRightClick()
        {
			Item helditem = Main.LocalPlayer.HeldItem;
			if (helditem.shoot>0 && helditem.GetGlobalItem<SGAUpgradableItemInstance>().toolType<1)
            {
				Projectile them = new Projectile();
				them.SetDefaults(helditem.shoot);
				if (them.aiStyle == 20)
					return true;

			}
            return false;
        }
        public override void RightClick(Player player)
        {
			Item helditem = Main.LocalPlayer.HeldItem;
			helditem.GetGlobalItem<SGAUpgradableItemInstance>().toolType = 1;
			//powertool code here
		}
        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("LaserMarker"), 8);
			recipe.AddIngredient(mod.ItemType("EnergizerBattery"), 3);
			recipe.AddIngredient(mod.ItemType("AdvancedPlating"), 6);
			recipe.AddIngredient(ItemID.MeteoriteBar, 4);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
	public class RedManaStar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Red Mana Star");
			Tooltip.SetDefault("'Otherwise known as the Lucky Star'\n" +
				"'Less Stable than its counterpart; it emanates heat but is thankfully only warm to the touch'" +
				"\nIncreases Max Mana by 20 each time it is used, up to 3 times depending on progression.");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 14;
			item.maxStack = 30;
			item.rare = 8;
			item.value = 1000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item9;
			item.consumable = true;
		}

		public override bool CanUseItem(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
			if (sgaplayer.Redmanastar < 1)
				return true;
			else if (sgaplayer.Redmanastar == 1 && SGAWorld.downedSharkvern)
				return true;
			else if (sgaplayer.Redmanastar == 2 && SGAWorld.downedWraiths > 3)
				return true;
			return false;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			SGAPlayer sgaplayer = Main.LocalPlayer.GetModPlayer<SGAPlayer>();
			if (sgaplayer.Redmanastar < 1)
			{
				tooltips.Add(new TooltipLine(mod, "RedStarLine", "Your magic attacks have a small chance to inflict 'On-Fire!'"));
				tooltips.Add(new TooltipLine(mod, "RedStarLine", "Usable right away"));
			}
			if (sgaplayer.Redmanastar == 1)
			{
				tooltips.Add(new TooltipLine(mod, "RedStarLine", "You instead inflict 'Thermal Blaze' instead of 'On Fire!'"));
				tooltips.Add(new TooltipLine(mod, "RedStarLine", "Usable after Sharkvern is defeated."));
			}
			if (sgaplayer.Redmanastar == 2)
			{
				tooltips.Add(new TooltipLine(mod, "RedStarLine", "You instead inflict 'Daybroken' instead of 'Thermal Blaze'"));
				tooltips.Add(new TooltipLine(mod, "RedStarLine", "Usable after Luminite Wraith is defeated."));
			}
			if (sgaplayer.Redmanastar > 2)
			{
				tooltips.Add(new TooltipLine(mod, "RedStarLine", "Its power is at its max, and can no longer help you gain strength"));
			}
			else
			{
				tooltips.Add(new TooltipLine(mod, "RedStarLine", "Rare chance to permanently strip enemy immunity to the above debuff on magic hit."));
				tooltips.Add(new TooltipLine(mod, "RedStarLine", "-Permanent Upgrade-"));

			}
		}
		public override bool UseItem(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
			sgaplayer.Redmanastar += 1;
			return true;
		}
	}

	public class MossySalve : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mossy Salve");
			Tooltip.SetDefault("'A nasty infection is better than bleeding out right?'\nTreats severe burn wounds, completely restoring your Lava Immunity Time\nStops Bleeding, Massive Bleeding, OnFire!, and Burning\nAlso heals 25 HP\n" + Idglib.ColorText(Color.Red, "However causes Swamp Rot infection and Murky Depths") +"\n" + Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stack, adds 60 seconds each"));
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 14;
			item.maxStack = 30;
			item.rare = ItemRarityID.Green;
			item.value = Item.buyPrice(silver: 50);
			item.useStyle = 2;
			item.useAnimation = 30;
			item.useTime = 30;
			item.useTurn = true;
			//item.UseSound = SoundID.Drown;
			item.consumable = true;
		}

		public override bool CanUseItem(Player player)
		{
			return player.SGAPly().CooldownStacks.Count < player.SGAPly().MaxCooldownStacks;
		}
		public override bool UseItem(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
			sgaplayer.AddCooldownStack(60 * 60, 1);

			player.lavaTime = player.lavaMax;

			int[] buffs = { ModContent.BuffType<NPCs.Murk.PoisonStack>(), BuffID.Bleeding, BuffID.OnFire, BuffID.Burning, ModContent.BuffType<Buffs.MassiveBleeding>() };

			foreach(int buff in buffs)
            {
				int buffindex = player.FindBuffIndex(buff);
				if (buffindex >= 0)
                {
					player.DelBuff(buffindex);
                }
			}

			player.HealEffect(25);
			player.netLife = true;
			player.statLife += 25;

			player.AddBuff(ModContent.BuffType<NPCs.Murk.MurkyDepths>(),60* (Main.expertMode ? 15 : 30));
			player.AddBuff(ModContent.BuffType<Buffs.PlaceHolderDebuff>(), 60 * 10);
			if (player.FindBuffIndex(ModContent.BuffType<Buffs.PlaceHolderDebuff>()) >= 0)
			{
				player.buffType[player.FindBuffIndex(ModContent.BuffType<Buffs.PlaceHolderDebuff>())] = ModContent.BuffType<NPCs.Murk.PoisonStack>();
			}

			Main.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 3, 1f, -0.50f);

			return true;
		}
		public override void AddRecipes()
		{
			if (GetType() == typeof(MossySalve))
			{
				ModRecipe recipe = new ModRecipe(mod);
				recipe.AddIngredient(ItemID.Vine, 5);
				recipe.AddIngredient(ModContent.ItemType<HavocGear.Items.DankCore>(), 1);
				recipe.AddIngredient(ModContent.ItemType<HavocGear.Items.DecayedMoss>(), 3);
				recipe.AddTile(TileID.WorkBenches);
				recipe.SetResult(this, 3);
				recipe.AddRecipe();

			}
		}
	}

	public class EnchantedBubble : MossySalve
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Bubble");
			Tooltip.SetDefault("'A breath of fresh air sealed within this magic bubble!'\n'Biting into this bubble restores your lungs'\nRecovers up to 200 Breath (the vanilla default)\n" + Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stack, adds 90 seconds each"));
			ItemID.Sets.ItemIconPulse[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 14;
			item.maxStack = 30;
			item.rare = 1;
			item.value = Item.buyPrice(silver: 50);
			item.useStyle = 2;
			item.useAnimation = 30;
			item.useTime = 30;
			item.useTurn = true;
			//item.UseSound = SoundID.Drown;
			item.consumable = true;
		}

		public override string Texture
		{
			get { return ("Terraria/Bubble"); }
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Main.hslToRgb((((Main.GlobalTime) * 0.116f)+(item.whoAmI * 0.179f)) % 1f, 0.8f, 0.75f);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (TooltipLine line in tooltips)
			{
				if (line.mod == "Terraria" && line.Name == "ItemName")
				{
					line.overrideColor = Color.Lerp(Color.Aqua, Color.Blue, 0.5f + (float)Math.Sin(Main.GlobalTime * 1.5f));
				}
			}
		}

		public override bool UseItem(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
			sgaplayer.AddCooldownStack(60 * 90, 1);
			sgaplayer.RestoreBreath(200,false);
			return true;
		}
	}

		public class DivineShower : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Divinity Caller");
			Tooltip.SetDefault("'The heavens favor your creation'\nCauses all fallen stars on the ground to rain down on all active enemies; whichever is limited first\nThe entirety of 1 stack will fall over 1 enemy, but spread out the larger the stack is\nHowever this caps out at 5 per stack\nIs limited to once per night, per a long cooldown");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 14;
			item.maxStack = 30;
			item.rare = 1;
			item.value = 0;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.useAnimation = 60;
			item.useTime = 60;
			item.useTurn = true;
			item.UseSound = SoundID.Item9;
			item.consumable = true;
		}

		public override string Texture
		{
			get { return ("Terraria/Item_"+ItemID.FallenStar); }
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Main.hslToRgb((Main.GlobalTime * -0.916f) % 1f, 0.8f, 0.75f);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (TooltipLine line in tooltips)
			{
				if (line.mod == "Terraria" && line.Name == "ItemName")
				{
					line.overrideColor = Color.Lerp(Color.Yellow, Color.Blue, 0.5f + (float)Math.Sin(-Main.GlobalTime * 0.5f));
				}
			}
		}

		public override bool CanUseItem(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
			int buffid = ModContent.BuffType<Buffs.StarStormCooldown>();

			return !player.HasBuff(buffid) && !Main.dayTime;
		}

		public override bool UseItem(Player player)
		{

			int buffid = ModContent.BuffType<Buffs.StarStormCooldown>();

			player.AddBuff(buffid, 60*900);

			List<Item> itemslist = new List<Item>();
			for(int i = 0; i < Main.item.Length; i += 1)
            {
				if (Main.item[i]!=null && Main.item[i].active && Main.item[i].type==ItemID.FallenStar && Main.item[i].stack<=5)
				itemslist.Add(Main.item[i]);
			}

			int itencheck = 0;
			for (int i = 0; i < Main.npc.Length; i += 1)
			{
				NPC enemy = Main.npc[i];
				bool invalid = (enemy == null || !enemy.active || enemy.dontTakeDamage || enemy.friendly || enemy.townNPC || enemy.life < 1 || enemy.immortal);
				if (itencheck < itemslist.Count && !invalid)
                {
					int stacked = itemslist[itencheck].stack;
					for (int j = 0; j < itemslist[itencheck].stack; j += 1)
					{
						Projectile.NewProjectile(new Vector2(enemy.Center.X, enemy.Center.Y - 600 + Main.rand.NextFloat(-400,0)), new Vector2(Main.rand.NextFloat(-3f,3f) * (1f+(stacked/3f)), 16f), ProjectileID.FallingStar, 2500, 8f, player.whoAmI, 1);
					}
					itemslist[itencheck].type = 0;
					itencheck += 1;
				}
			}

			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("VirulentBar"), 3);
			recipe.AddIngredient(mod.ItemType("CryostalBar"), 3);
			recipe.AddIngredient(mod.ItemType("WraithFragment4"), 3);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
	public class VenerableCatharsis : TrueCopperWraithNotch
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Venerable Catharsis");
			Tooltip.SetDefault("Upgrades a Normal world to an Expert World\n-Permanent Upgrade-");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 4));
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return lightColor;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Consumables/VenerableCatharsis"); }
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 14;
			item.maxStack = 1;
			item.rare = ItemRarityID.Quest;
			item.value = Item.sellPrice(gold: 1);
			item.useStyle = 2;
			item.useAnimation = 32;
			item.useTime = 32;
			item.useTurn = true;
			item.UseSound = SoundID.Item123;
			item.consumable = true;
		}

		public override bool CanUseItem(Player player)
		{
			return !Main.expertMode;
		}
		public override bool UseItem(Player player)
		{
			Main.expertMode = true;
			return true;
		}
	}

	public class JoyfulShroom : TrueCopperWraithNotch
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Joyful Shroom");
			Tooltip.SetDefault("This is not a normal Mushroom...");
		}

		public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
		{
			if (GetType() == typeof(JoyfulShroom))
				//if (line.mod == "Terraria" && line.Name == "ItemName")
				//{
					Main.spriteBatch.End();
					Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.UIScaleMatrix);

					Utils.DrawBorderString(Main.spriteBatch, line.text, new Vector2(line.X, line.Y), Color.White);

					Effect TrippyRainbowEffect = SGAmod.TrippyRainbowEffect;

					TrippyRainbowEffect.Parameters["uColor"].SetValue(new Vector3(0.05f, 0.05f, 0f));
					TrippyRainbowEffect.Parameters["uScreenResolution"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight) / 6f);
					TrippyRainbowEffect.Parameters["uOpacity"].SetValue(0.15f);
					TrippyRainbowEffect.Parameters["uDirection"].SetValue(new Vector2(1f, Main.GlobalTime * 0.1f));
					TrippyRainbowEffect.Parameters["uIntensity"].SetValue(1f);
					TrippyRainbowEffect.Parameters["uScreenPosition"].SetValue(Main.screenPosition / 500f);
					TrippyRainbowEffect.Parameters["uTargetPosition"].SetValue(Main.screenPosition / 500f);
					TrippyRainbowEffect.Parameters["uProgress"].SetValue(Main.GlobalTime * 0.05f);
					TrippyRainbowEffect.Parameters["overlayTexture"].SetValue(SGAmod.Instance.GetTexture("TiledPerlin"));
					TrippyRainbowEffect.CurrentTechnique.Passes["ScreenTrippy"].Apply();

					Utils.DrawBorderString(Main.spriteBatch, line.text, new Vector2(line.X, line.Y), Color.White);

					Main.spriteBatch.End();
					Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
					return false;
				//}
		}

		public override bool CanUseItem(Player player)
		{
			return true;
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 14;
			item.maxStack = 1;
			item.rare = ItemRarityID.Quest;
			item.value = Item.buyPrice(gold: 1);
			item.useStyle = ItemUseStyleID.EatingUsing;
			item.useAnimation = 64;
			item.useTime = 64;
			item.useTurn = true;
			item.UseSound = SoundID.Item2;
			item.consumable = true;
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			bool shroom = GetType() == typeof(JoyfulShroom);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
			ArmorShaderData shader = GameShaders.Armor.GetShaderFromItemId(shroom ? ItemID.LivingRainbowDye : ItemID.NebulaDye);
			shader.UseOpacity(0.5f);
			shader.UseSaturation(0.25f);

			if (shroom)
			{
				shader.Apply(null);
			}
			else
			{
				Texture2D stain = mod.GetTexture("Stain");
				DrawData value28 = new DrawData(stain, new Vector2(240, 240), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 600, 600)), Microsoft.Xna.Framework.Color.White, MathHelper.PiOver2, stain.Size() / 2f, 0.2f, SpriteEffects.None, 0);
				shader.Apply(null, new DrawData?(value28));
			}


			spriteBatch.Draw(Main.itemTexture[item.type], position, frame, drawColor, 0, origin, scale, SpriteEffects.None, 0f);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
			return false;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			ArmorShaderData shader = GameShaders.Armor.GetShaderFromItemId(ItemID.LivingRainbowDye);
			shader.UseOpacity(0.5f);
			shader.UseSaturation(0.25f);
			shader.Apply(null);
			spriteBatch.Draw(Main.itemTexture[item.type], item.position-Main.screenPosition, null, lightColor, 0, Main.itemTexture[item.type].Size() / 2f, scale, SpriteEffects.None, 0f);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}

        public override string Texture => "Terraria/Projectile_"+ProjectileID.Mushroom;

		public override bool UseItem(Player player)
		{
			player.AddBuff(ModContent.BuffType<Buffs.CleansedPerception>(),60*60);
			//Main.expertMode = true;
			return true;
		}
	}

	public class BenchGodsFavor : TrueCopperWraithNotch
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bench God's Favor");
			Tooltip.SetDefault("Unlocks a UI slot in your inventory\nAny work station placed into this slot will be always active when crafting\n-Permanent Upgrade-");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 14;
			item.maxStack = 30;
			item.rare = 1;
			item.value = Item.sellPrice(gold: 1);
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item9;
			item.consumable = true;
		}

		public override string Texture
		{
			get { return ("Terraria/Item_"+ItemID.WorkBench); }
		}

		public override bool CanUseItem(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
			return !sgaplayer.benchGodFavor;
		}
		public override bool UseItem(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
			sgaplayer.benchGodFavor = true;
			return true;
		}
	}

	public class TrueCopperWraithNotch : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Wraith Notch");
			Tooltip.SetDefault("'Reward for beating the Copper Wraith without angering it first'" +
				"\nUnlocks IDG's Starter Bags in Draken's shop when consumed\n-Permanent Upgrade-");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 14;
			item.maxStack = 30;
			item.rare = 1;
			item.value = Item.sellPrice(gold: 1);
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item9;
			item.consumable = true;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Consumables/TrueWraithNotch"); }
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Main.hslToRgb((Main.GlobalTime * 0.916f) % 1f, 0.8f, 0.75f);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (TooltipLine line in tooltips)
			{
				if (line.mod == "Terraria" && line.Name == "ItemName")
				{
					line.overrideColor = Color.Lerp(Color.Yellow, Color.Blue, 0.5f + (float)Math.Sin(Main.GlobalTime * 0.5f));
				}
			}
		}

		public override bool CanUseItem(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
			return !sgaplayer.Drakenshopunlock;
		}
		public override bool UseItem(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
			sgaplayer.Drakenshopunlock = true;
			return true;
		}
	}

	public class PortalEssence : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Portal Essence");
			Tooltip.SetDefault("Allows the Strange Portal to 'move in' to free housing in your town");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 14;
			item.maxStack = 30;
			item.rare = 1;
			item.value = Item.sellPrice(gold: 1);
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item9;
			item.consumable = true;
		}

		public override string Texture
		{
			get { return ("SGAmod/Projectiles/FieryRock"); }
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Main.hslToRgb((Main.GlobalTime * 0.916f) % 1f, 0.8f, 0.75f);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (TooltipLine line in tooltips)
			{
				if (line.mod == "Terraria" && line.Name == "ItemName")
				{
					line.overrideColor = Color.Lerp(Color.Yellow, Color.Blue, 0.5f + (float)Math.Sin(Main.GlobalTime * 0.5f));
				}
			}
		}

		public override bool CanUseItem(Player player)
		{
			if (SGAmod.anysubworld)
				return false;
			return !SGAWorld.portalcanmovein;
		}
		public override bool UseItem(Player player)
		{
			SGAWorld.portalcanmovein = true;
			return true;
		}
	}

	public class ConsumeHell : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Consumable Hell");
			Tooltip.SetDefault("A Red Devil's favorite, eating it will turn your mouth into a literal flame thrower\nThis lasts for 1 minute, cannot be cancelled\nSeveral peppers may be consumed in succession to increase the power of the fire breath\nDeals non-class damage, does not crit\n" + Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stack, adds 90 seconds each"));
		}

		public override bool CanUseItem(Player player)
		{
			return player.SGAPly().CooldownStacks.Count < player.SGAPly().MaxCooldownStacks;
		}

		public override bool UseItem(Player player)
		{
			player.SGAPly().AddCooldownStack(60 * 90, 1);
			player.SGAPly().FireBreath += 1;
			player.AddBuff(mod.BuffType("ConsumeHellBuff"), 60 * 60);
			return true;
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = 5;
			item.value = 15000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item2;
			item.consumable = true;
			//item.buffType = mod.BuffType("ConsumeHellBuff");
			//item.buffTime = 60*60;
		}
	}

	public class EnergizerBattery : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Energizer Battery");
			Tooltip.SetDefault("'Keeps going and going...'\nInstantly restores 20% of your Max Electric Charge on use\nIncreases your Max Electric Charge by 200 per use (up to 2000)\nAfter Sharkvern, this is raised to 3000\nAfter Luminite Wraith, it is raised again to 5000\n" + Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stack, adds 40 seconds each"));
		}

		public override bool CanUseItem(Player player)
		{
			return player.SGAPly().CooldownStacks.Count < player.SGAPly().MaxCooldownStacks;
		}

		public override bool UseItem(Player player)
		{
			int max = 2000 + (SGAWorld.downedSharkvern ? 1000 : 0) + (SGAWorld.downedWraiths>3 ? 2000 : 0);
			if (player.SGAPly().Electicpermboost < max)
			player.SGAPly().Electicpermboost += 200;
			player.SGAPly().AddCooldownStack(60 * 40, 1);
			player.SGAPly().AddElectricCharge((int)((float)player.SGAPly().electricChargeMax*0.20f));
			return true;
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = 2;
			item.value = 1000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item2;
			item.consumable = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("BottledMud"), 5);
			recipe.AddIngredient(mod.ItemType("VialofAcid"), 15);
			recipe.AddIngredient(mod.ItemType("Biomass"), 10);
			recipe.AddIngredient(ItemID.Bunny, 1);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this, 5);
			recipe.AddRecipe();
		}
	}

	public class Gong : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eastern Gong");
			Tooltip.SetDefault("'Its ring calls forth the peddler lord himself'\nOpens a portal to bring the Traveling Merchant to your location where you used it\nUsing this item will restock the Traveling Merchant's Shop\nIs not usable if the Traveling Merchant is already in the world plus normal Traveling Merchant spawn rules\n" + Idglib.ColorText(Color.Orange, "Requires 2 Cooldown stacks, adds 300 seconds each"));
		}

		public override bool CanUseItem(Player player)
		{
			return player.SGAPly().CooldownStacks.Count + 1 < player.SGAPly().MaxCooldownStacks && !NPC.travelNPC && NPC.CountNPCS(NPCID.TravellingMerchant)<1 && Main.dayTime && !Main.eclipse && Main.projectile.FirstOrDefault(type2 => type2.type == ModContent.ProjectileType<GongSummon>()) == default;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.SGAPly().AddCooldownStack(60 * 300, 2);
			return true;
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 1;
			item.rare = 5;
			item.value = 50000;
			item.useStyle = 2;
			item.noUseGraphic = true;
			item.useAnimation = 120;
			item.useTime = 120;
			item.shoot = mod.ProjectileType("GongSummon");
			item.useTurn = true;
			item.UseSound = SoundID.Item1;
		}
	}

	public class GongSummon : ModProjectile
	{

		int chargeuptime = 100;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("GOOONGGG!");
		}

		public override bool? CanHitNPC(NPC target) { return false; }

		public override string Texture
		{
			get { return ("SGAmod/Items/Consumables/Gong"); }
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 16;
			projectile.height = 16;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.timeLeft = 220;
			projectile.tileCollide = false;
			projectile.magic = true;
			aiType = 0;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 gohere = new Vector2(0, -160+(160 / ((projectile.ai[0]/25f)+1)));

			Texture2D tex = ModContent.GetTexture("SGAmod/Extra_49");

			float scaleeffect = MathHelper.Clamp(((float)150f-projectile.timeLeft) / 80f,0f,Math.Min((float)projectile.timeLeft / 30f,1f));

			for (float valez = 0.1f; valez < 10f; valez += 0.5f)
			{
				spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White * 0.25f, (Main.GlobalTime + valez * 3f) * 2f, tex.Size() / 2f, (0.5f + (valez / 15f)) * scaleeffect, SpriteEffects.FlipVertically, 0f);
			}

			scaleeffect = MathHelper.Clamp(((float)120f - projectile.timeLeft) / 30f, 0f, Math.Min((float)projectile.timeLeft / 30f, 1f));
			tex = Main.extraTexture[34];

			for (float valez = 0.1f; valez < 10f; valez += 0.2f)
			{
				spriteBatch.Draw(tex, projectile.Center-Main.screenPosition, null, Main.hslToRgb(((Main.GlobalTime + valez * 3f) * 2f)%1f,0.8f,0.75f)*0.05f, (Main.GlobalTime + valez*3f)*2f, tex.Size() / 2f, (0.25f+(valez/10f))* scaleeffect, SpriteEffects.FlipVertically, 0f);
			}

			spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center+ gohere - Main.screenPosition, null, Color.White*Math.Min((float)projectile.timeLeft / 80f,1f), 0, Main.projectileTexture[projectile.type].Size()/2f, new Vector2(1, 1), SpriteEffects.None, 0f);
			return false;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];

			if (player == null)
				projectile.Kill();
			if (player.dead)
				projectile.Kill();
			player.itemTime = 6;
			player.itemAnimation = 6;
			projectile.ai[0] += 1;

			Vector2 gohere = new Vector2(0, -160 + (160 / ((projectile.ai[0] / 25f) + 1)));

			if (projectile.timeLeft == 150)
			{
				RippleBoom.MakeShockwave(projectile.Center+ gohere, 8f, 2f, 20f, 100, 0.5f, true);
				Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 35, 1f, 0.25f);

			}

			if (projectile.timeLeft == 50)
			{
				Chest.SetupTravelShop();
				NetMessage.SendTravelShop(-1);

				if (NPC.CountNPCS(NPCID.TravellingMerchant) < 1)
					NPC.NewNPC((int)projectile.Center.X, (int)projectile.Center.Y, NPCID.TravellingMerchant);
				else
					Main.npc[(NPC.FindFirstNPC(NPCID.TravellingMerchant))].Center = projectile.Center;
			}
		}
	}
	public class BoneBucket : Gong
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bone Bucket");
			Tooltip.SetDefault("'Full of spare bone parts! Someone might want these...'\nOpens a portal to bring the Skeleton Merchant to your location where you used it\nNot usable if the Merchant Merchant is already in the world or the sun is shining\n" + Idglib.ColorText(Color.Orange, "Requires 2 Cooldown stacks, adds 300 seconds each"));
		}

		public override bool CanUseItem(Player player)
		{
			return player.SGAPly().CooldownStacks.Count + 1 < player.SGAPly().MaxCooldownStacks && !NPC.travelNPC && NPC.CountNPCS(NPCID.SkeletonMerchant) < 1 && (!Main.dayTime || Main.eclipse) && Main.projectile.FirstOrDefault(type2 => type2.type == ModContent.ProjectileType<BoneBucketSummon>()) == default;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			item.shoot = mod.ProjectileType("BoneBucketSummon");
		}
	}

	public class BoneBucketSummon : GongSummon
	{

		int chargeuptime = 100;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("BoneBucketSummon!");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Consumables/BoneBucket"); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 gohere = new Vector2(0, -160 + (160 / ((projectile.ai[0] / 25f) + 1)));

			Texture2D tex = ModContent.GetTexture("SGAmod/Extra_49");

			float scaleeffect = MathHelper.Clamp(((float)150f - projectile.timeLeft) / 80f, 0f, Math.Min((float)projectile.timeLeft / 30f, 1f));

			for (float valez = 0.1f; valez < 10f; valez += 0.5f)
			{
				spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White * 0.25f, (Main.GlobalTime + valez * 3f) * 2f, tex.Size() / 2f, (0.5f + (valez / 15f)) * scaleeffect, SpriteEffects.FlipVertically, 0f);
			}

			scaleeffect = MathHelper.Clamp(((float)120f - projectile.timeLeft) / 30f, 0f, Math.Min((float)projectile.timeLeft / 30f, 1f));
			tex = Main.extraTexture[34];

			for (float valez = 0.1f; valez < 10f; valez += 0.2f)
			{
				spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Main.hslToRgb(((Main.GlobalTime + valez * 3f) * 2f) % 1f, 0.8f, 0.75f) * 0.05f, (Main.GlobalTime + valez * 3f) * 2f, tex.Size() / 2f, (0.25f + (valez / 10f)) * scaleeffect, SpriteEffects.FlipVertically, 0f);
			}

			Vector2 drawOffset = new Vector2(Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height*1.5f)/2f;
			spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center + gohere - Main.screenPosition, null, Color.White * Math.Min((float)projectile.timeLeft / 80f, 1f), -((projectile.timeLeft-70)/150f), drawOffset, new Vector2(1, 1), SpriteEffects.None, 0f);
			return false;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];

			if (player == null)
				projectile.Kill();
			if (player.dead)
				projectile.Kill();
			player.itemTime = 6;
			player.itemAnimation = 6;
			projectile.ai[0] += 1;

			Vector2 gohere = new Vector2(0, -160 + (160 / ((projectile.ai[0] / 25f) + 1)));

			if (projectile.timeLeft == 150)
			{
				RippleBoom.MakeShockwave(projectile.Center + gohere, 8f, 2f, 20f, 100, 0.5f, true);
				SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_DarkMageHurt, (int)projectile.Center.X, (int)projectile.Center.Y);
				if (sound != null)
				{
					sound.Pitch += 0.50f;
				}

			}

			if (projectile.timeLeft == 50)
			{

				if (NPC.CountNPCS(NPCID.SkeletonMerchant) < 1)
					NPC.NewNPC((int)projectile.Center.X, (int)projectile.Center.Y, NPCID.SkeletonMerchant);
				else
					Main.npc[(NPC.FindFirstNPC(NPCID.TravellingMerchant))].Center = projectile.Center;
			}
		}
	}

	//[AutoloadEquip(EquipType.Head)]
	public class InterdimensionalPartyHat : JoyfulShroom
	{
        public override string Texture => "Terraria/Item_"+ItemID.PartyHat;
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Interdimensional Party Hat");
			Tooltip.SetDefault("'Well, this explains the party hats...'\nStarts a genuine party, adds the portal to the party as a 3rd wheel if present\n" + Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stacks, adds 200 seconds each"));
		}
		public override bool CanUseItem(Player player)
		{
			return player.SGAPly().CooldownStacks.Count + 1 < player.SGAPly().MaxCooldownStacks && !BirthdayParty.PartyIsUp;
		}
		public override bool UseItem(Player player)
		{
			bool worked = false;
			for (int i = 0; i < 1000; i += 1)
			{
				BirthdayParty.PartyDaysOnCooldown = 0;
				BirthdayParty.CheckMorning();
				if (BirthdayParty.GenuineParty)
				{
					worked = true;
					int indexer = NPC.FindFirstNPC(ModContent.NPCType<Dimensions.NPCs.DungeonPortal>());
					if (indexer >= 0)
					{
						BirthdayParty.CelebratingNPCs.Add(indexer);
					}
					Main.NewText("It would seem the item has worked... try making sure you have the npcs first");
					break;
				}
			}
			if (!worked)
            {
				Main.NewText("It would seem the item has failed... try making sure you have the npcs first");
				return true;
            }
			player.SGAPly().AddCooldownStack(60 * 200, 1);

			return true;
		}

        public override bool Autoload(ref string name)
        {
			return true;
        }

        public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 1;
			item.rare = 5;
			item.value = 50000;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.noUseGraphic = true;
			item.useAnimation = 30;
			item.useTime = 30;
			item.vanity = true;
			item.useTurn = true;
			item.UseSound = SoundID.Item1;
			Item hat = new Item(); hat.SetDefaults(ItemID.PartyHat);
			item.headSlot = hat.headSlot;
		}

		public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
		{
			return true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			//Stuff
		}
	}

	public class SoulJar : ModItem
	{
		public string npcType = "";
		public string modName = "";
		public int npcTypeToUse = -10000;
		public override bool CloneNewInstances => true;

		public Color SoulColor
        {
            get
            {
				if (GetType() == typeof(SoulJar))
                {
					return Color.White;
                }

				return Main.hslToRgb(Main.GlobalTime % 1f, 1f, 0.75f);
            }
        }
		public override string Texture
		{
			get { return ("SGAmod/Items/Consumables/SoulJar"); }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Empty Soul Jar");
			Tooltip.SetDefault("Throw it at an enemy under 10% HP to capture their essence\nFor use with the Numismatic Crucible");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.maxStack = 1;
			item.useTime = 10;
			item.useAnimation = 10;
			item.consumable = true;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.rare = ItemRarityID.Orange;
			item.shoot = ModContent.ProjectileType<SoulJarProj>();
			item.shootSpeed = 16;
		}

		public void ParseLoadingCapture()
		{
			int typeofnpc = -1;
			if (SGAUtils.IsDigitsOnly(npcType))
            {
				typeofnpc = int.Parse(npcType);
			}
			else
			{
				typeofnpc = ModLoader.GetMod(modName).NPCType(npcType);
			}

			if (typeofnpc > -1)
			{
				npcTypeToUse = typeofnpc;
			}
		}

		public void DoCapture(NPC npc)
		{
			ModNPC modnpc = npc.modNPC;
			npcType = modnpc != null ? npc.modNPC.GetType().Name : npc.type.ToString();
			modName = modnpc != null ? modnpc.mod.GetType().Name : "";
			ParseLoadingCapture();

			SoundEffectInstance sound = Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 103);
			if (sound != null)
				sound.Pitch += 0.50f;

		}

		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(npcType);
			writer.Write(modName);

		}
		public override void NetRecieve(BinaryReader reader)
		{
			npcType = reader.ReadString();
			modName = reader.ReadString();
			ParseLoadingCapture();
		}

		public override TagCompound Save()
		{
			TagCompound tag = new TagCompound
			{
				["npcType"] = npcType,
				["modName"] = modName
			};
			return tag;
		}
		public override void Load(TagCompound tag)
		{
			if (tag.ContainsKey("npcType"))
			{
				npcType = tag.GetString("npcType");
				modName = tag.GetString("modName");
				ParseLoadingCapture();
			}
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (npcTypeToUse >= -9999)
			{
				//Main.NewText(npcTypeToUse);
				tooltips.Add(new TooltipLine(mod, "SoulJarText", "Contains a soul of an NPC:"));
				NPC npctoshow = new NPC();
				npctoshow.SetDefaults(npcTypeToUse);
				tooltips.Add(new TooltipLine(mod, "SoulJarText", ""+ npctoshow.FullName));
            }
            else
            {
				if (GetType() == typeof(SoulJarFull))
				tooltips.Add(new TooltipLine(mod, "SoulJarText", "This jar appears to be empty"));
			}
		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{

			Texture2D inner = Main.itemTexture[item.type];
				Texture2D inner2 = Main.itemTexture[ItemID.RedDye];

				Vector2 slotSize = new Vector2(52f, 52f);
				position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
				Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
				Vector2 textureOrigin = new Vector2(inner.Width, inner.Height) / 2f;

			//spriteBatch.Draw(inner, drawPos, null, Color.DarkMagenta, 0, textureOrigin, Main.inventoryScale * 1f, SpriteEffects.None, 0f);

			spriteBatch.Draw(inner2, drawPos, null, Color.White * 1f, 0, textureOrigin, Main.inventoryScale * 1f, SpriteEffects.None, 0f);
				spriteBatch.Draw(inner, drawPos, null, SoulColor * 1f, 0, textureOrigin, Main.inventoryScale * 1f, SpriteEffects.None, 0f);

			return false;
		}

		public override void AddRecipes()
		{
			if (GetType() == typeof(SoulJar))
			{
				ModRecipe recipe = new ModRecipe(mod);
				recipe.AddIngredient(ModContent.ItemType<SoulJarFull>(), 1);
				recipe.SetResult(this, 1);
				recipe.AddRecipe();
			}
		}

        public override bool CanUseItem(Player player)
        {
            return item.type == ModContent.ItemType<SoulJar>();
        }

    }


	public class SoulJarFull : SoulJar
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Filled Soul Jar");
			Tooltip.SetDefault("Can be crafted back into an empty Soul Jar");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.useTime = -1;
			item.useAnimation = -1;
			item.consumable = false;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.rare = ItemRarityID.Orange;
			item.shoot = 0;
		}
	}

		public class SoulJarProj : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thrown Soul Jar");
		}
		NPC capture = null;

		public override string Texture
		{
			get { return ("SGAmod/Items/Consumables/SoulJar"); }
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 12;
			projectile.height = 12;
			projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.ranged = true;
			projectile.timeLeft = 240;
			projectile.arrow = true;
			projectile.damage = 0;
			aiType = ProjectileID.WoodenArrowFriendly;
		}

        public override bool PreKill(int timeLeft)
        {

			if (projectile.owner == Main.myPlayer)
			{
				int item = Item.NewItem(projectile.getRect(), capture != null ? ModContent.ItemType<SoulJarFull>() : ModContent.ItemType<SoulJar>());

				if (item >= 0 && capture != null && Main.item[item].modItem is SoulJar jar)
                {
					jar.DoCapture(capture);
                }

					// Sync the drop for multiplayer
					// Note the usage of Terraria.ID.MessageID, please use this!
					if (Main.netMode == NetmodeID.MultiplayerClient && item >= 0)
				{
					NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f);
				}
			}

			return true;
        }

        public override void AI()
		{

			projectile.ai[0] = projectile.ai[0] + 1;
			projectile.velocity.Y += 0.15f;
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;


			NPC target = Main.npc[Idglib.FindClosestTarget(0, projectile.Center, new Vector2(0f, 0f), true, true, true, projectile)];
			if (target != null && target.life<(int)(target.lifeMax*10.10))
			{
				if (new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height).Intersects
					(new Rectangle((int)target.position.X, (int)target.position.Y, target.width, target.height)))
				{
					capture = target;
					projectile.Kill();
				}
			}
		}

	}

}