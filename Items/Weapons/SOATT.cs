using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;

namespace SGAmod.Items.Weapons
{
	public class SOATT : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sword of a thousand Truths");
			Tooltip.SetDefault("Strikes apply Truth Be Told, a stacking debuff that makes this weapon deal damage through defense\n4% is added per strike and up to 100% defense penetration can be applied\nAt max Truth Be Told, you inflict immunity-bypassing Sundered Defense; this causes enemies to lose many of their immunity frames to all attacks\nGotta start somewhere you know-This was IDGCaptainRussia's first modded item");
		}
		public override void SetDefaults()
		{
			item.damage = 135;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 10;
			item.useAnimation = 10;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = Item.sellPrice(0, 20, 0, 0);
			item.rare = 6;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}

		public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
		{
		SGAnpcs nyx=target.GetGlobalNPC<SGAnpcs>();
		float it=nyx.truthbetold;
		nyx.truthbetold=it+0.02f;
		if (nyx.truthbetold>0.5f){nyx.truthbetold=0.5f;
		IdgNPC.AddBuffBypass(target.whoAmI, mod.BuffType("SunderedDefense"), 60 * 3);
		}
		damage=(int)(damage+(target.defense*nyx.truthbetold));
		//Idglib.Chat("Defense: "+nyx.truthbetold,244, 179, 66);
		}

	}
}
