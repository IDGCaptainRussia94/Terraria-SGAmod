using Terraria;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items
{
	public class Vinefish : ModItem
	{
		public override void SetDefaults()
		{

			item.questItem = true;
			item.maxStack = 1;
			item.width = 26;
			item.height = 26;
			item.uniqueStack = true;
			item.rare = -11;
		}

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Vinefish");
      Tooltip.SetDefault("");
    }


		public override bool IsQuestFish()
		{
			return true;
		}

		public override bool IsAnglerQuestAvailable()
		{
            return true;
		}

		public override void AnglerQuestChat(ref string description, ref string catchLocation)
		{
			description = "Hey! Today as I made my way through a Dank place, I was almost dragged into the water by a fish that's made of vines. I want to see if it tastes like fish and seeweed, so bring it to me!";
			catchLocation = "\nCaught whilst in a Dank Shrine";
		}
	}
}
