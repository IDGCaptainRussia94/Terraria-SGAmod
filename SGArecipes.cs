using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;

namespace SGAmod
{
	class SGArecipes : ModRecipe
	{
        public double tempvar = 0;

        public SGArecipes(Mod mod, double tempvar) : base(mod)
        {
            this.tempvar = tempvar;
        }

        public override bool RecipeAvailable()
        {
            //if (Main.LocalPlayer.GetModPlayer<MyPlayer>(mod).GetExp() < experienceNeeded)
            //    return false;
            //else
                return SgaLib.EnforceDuplicatesInRecipe(this);
        }
    }
    class ShadowJavelinRecipe : ModRecipe
    {

        public double tempvar = 0;

        public ShadowJavelinRecipe(Mod mod) : base(mod)
        {
            this.tempvar = tempvar;
        }
        

        public override int ConsumeItem (int type, int numRequired)
        {
            if (type==ItemID.ShadowKey || type == mod.ItemType("OmegaSigil"))
                return 0;
            else
                return numRequired;
        }

        public override bool RecipeAvailable()
        {
            //if (Main.LocalPlayer.GetModPlayer<MyPlayer>(mod).GetExp() < experienceNeeded)
            //    return false;
            //else
                return SgaLib.EnforceDuplicatesInRecipe(this);
        }

    }
    class StarMetalRecipes : ModRecipe
    {

        public double tempvar = 0;

        public StarMetalRecipes(Mod mod) : base(mod)
        {
            this.tempvar = tempvar;
        }
        

        public override int ConsumeItem (int type, int numRequired)
        {
            if (type==mod.ItemType("StarMetalMold"))
                return 0;
            else
                return numRequired;
        }

        public override bool RecipeAvailable()
        {
            //if (Main.LocalPlayer.GetModPlayer<MyPlayer>(mod).GetExp() < experienceNeeded)
            //    return false;
            //else
                return SgaLib.EnforceDuplicatesInRecipe(this);
        }

    }

    class HellionItems : ModRecipe
    {

        public double tempvar = 0;

        public HellionItems(Mod mod) : base(mod)
        {
            this.tempvar = tempvar;
        }

        public override bool RecipeAvailable()
        {
            //if (Main.LocalPlayer.GetModPlayer<MyPlayer>(mod).GetExp() < experienceNeeded)
            //    return false;
            //else
            if (SGAWorld.downedHellion < 1)
                return false;
            return SgaLib.EnforceDuplicatesInRecipe(this);
        }

    }

    class SGAGlobalRecipes : GlobalRecipe
	{


        public override bool Autoload(ref string id)
        {
        return true;
        }

        public override void OnCraft(Item item, Recipe recipe)
        {
            if ((recipe.createItem.type == ItemID.Furnace || recipe.requiredTile.Any(tile => tile == TileID.Furnaces)) && (SGAWorld.downedWraiths < 1))
            {

                if (!NPC.AnyNPCs(mod.NPCType("CopperWraith")))
                {

                    if (Main.netMode > 0)
                    {
                        mod.Logger.Debug("Copper Wraith: Server Craft Warning");
                        ModPacket packet = mod.GetPacket();
                        packet.Write(995);
                        packet.Send();
                    }
                    else
                    {
                        SGAWorld.CraftWarning();
                        mod.Logger.Debug("Copper Wraith: SP Craft Warning");
                    }

                }
            }
        }

        public override bool RecipeAvailable(Recipe recipe)
        {

            bool canwemakeit=base.RecipeAvailable(recipe);
            if (recipe.createItem.type == mod.ItemType("HellionSummon") && SGAWorld.downedHellion < 1)
                canwemakeit = false;
            //if ((((recipe.createItem.type==ItemID.MythrilAnvil || recipe.requiredTile.Any(tile => tile == TileID.MythrilAnvil)) || recipe.createItem.type==ItemID.OrichalcumAnvil) && (SGAWorld.downedWraiths<2))
            if ((recipe.createItem.type==ItemID.LunarBar && SGAWorld.downedWraiths<4))
                return false;
            else
                return canwemakeit;//return SgaLib.EnforceDuplicatesInRecipe(recipe as ModRecipe); //base.RecipeAvailable(recipe);
        }
    }


}