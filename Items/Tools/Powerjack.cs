using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;

namespace SGAmod.Items.Tools
{
	public class Powerjack : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Powerjack");
			Tooltip.SetDefault("Whie in hand:\n15% increase to all movement speed, 25 life restored on kill\n"+Idglib.ColorText(Color.Red,"20% increased damage taken")+"\nCan smash alters");
		}

        /*public override void ModifyTooltips(List<TooltipLine> tooltips)
        {

            Color c = Main.hslToRgb((float)(Main.GlobalTime/4)%1f, 0.4f, 0.45f);
            string potion="[i:" + ItemID.RedPotion + "]";
            tooltips.Add(new TooltipLine(mod,"IDG Debug Item", potion+Idglib.ColorText(c,"Mister Creeper's dev weapon")+potion));
        }*/

		public override void SetDefaults()
		{
			item.damage = 25;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 12;
			item.useAnimation = 24;
			item.hammer = 80;
			item.useStyle = 1;
			item.knockBack = 8;
			item.value = 10000;
			item.rare = 6;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useTurn = true;
		}

	}

}