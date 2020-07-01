using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;

namespace SGAmod.Items.Consumable
{
	public class ConsumeHell : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Consumable Hell");
			Tooltip.SetDefault("A Red Devil's favorite, eating it will turn your mouth into a literal flame thrower\nThis lasts for 1 minute, cannot be cancelled\nSeveral peppers may be consumed in succession to increase the power of the fire breath\nDeals non-class damage, does not crit\n" + Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stacks, adds 90 seconds each"));
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
}