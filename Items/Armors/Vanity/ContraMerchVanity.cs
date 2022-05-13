using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.Utilities;
using System.Linq;

namespace SGAmod.Items.Armors.Vanity
{
	[AutoloadEquip(EquipType.Head)]
	public class ContraMerchHat : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Contraband Merchant's Hat");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 10;
			item.value = Item.sellPrice(gold: 1);
			item.rare = ItemRarityID.Blue;//1
			item.vanity = true;
			item.defense = 0;
		}
        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
			drawAltHair = true;
		}
	}
	[AutoloadEquip(EquipType.Body)]
	public class ContraMerchCoat : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Contraband Merchant's Coat");
		}
		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 32;
			item.value = Item.sellPrice(gold: 1);
			item.rare = ItemRarityID.Blue;//1
			item.vanity = true;
			item.defense = 0;
		}
		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
		{
			robes = true;
			// The equipSlot is added in SGAmod.cs --> Load hook
			equipSlot = mod.GetEquipSlot("ContraMerchCoat_Legs", EquipType.Legs);
			//Legs become "invisible". Idk how to fix it.
		}

		public override void DrawHands(ref bool drawHands, ref bool drawArms)
		{
			drawHands = true;
			drawArms = false;
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class ContraMerchShoes : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Contraband Merchant's Shoes");
		}
		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 10;
			item.value = Item.sellPrice(gold: 1);
			item.rare = ItemRarityID.Blue;//1
			item.vanity = true;
			item.defense = 0;
		}
	}
}