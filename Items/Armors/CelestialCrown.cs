using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Armors
{

	[AutoloadEquip(EquipType.Head)]
	public class CodeBreakerHead : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hellion's Crown");
			Tooltip.SetDefault("'Became one with the pure chaotic code...'");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 0;
			item.rare = 2;
			item.vanity = true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			string addtotip = tooltips[0].text;
			string thetip = "";
			for (int loc = 0; loc < addtotip.Length; loc += 1)
			{
				char character = addtotip[loc];
				if (Main.rand.Next(30) <= 1)
				{
					character = (char)(33 + Main.rand.Next(15));
				}

				thetip += character;
			}
			tooltips[0].text = thetip;

		}
		public override void UpdateEquip(Player player)
		{
			/*player.allDamage += Main.rand.NextFloat(0.20f, 0.50f);
			SGAPlayer sgaplayer = player.GetModPlayer(mod,typeof(SGAPlayer).Name) as SGAPlayer;
            sgaplayer.UseTimeMul+=0.05f;*/
		}
		public override void UpdateVanity(Player player, EquipType type)
		{
			item.rare = Main.rand.Next(0, 12);
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Armors/CelestialCrown"); }
		}
	}
	public class CelestialCrown : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Celestial Crown");
			Tooltip.SetDefault("5% faster item use times");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 10000;
			item.rare = 2;
			item.defense = 2;
		}
		public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.UseTimeMul += 0.05f;
		}
	}
}