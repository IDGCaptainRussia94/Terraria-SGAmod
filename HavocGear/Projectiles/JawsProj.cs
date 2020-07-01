using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Projectiles
{
    public class JawsProj : ModProjectile
    {
    	int started=0;
    	Projectile [] orbitors={null,null,null,null,null,null};
    	int [] spinners={-6,-6,-6,-6,-6,-6};
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jaws");
			ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 240f;
			ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 15f;
		}
       
	    public override void SetDefaults()
        {
        	Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Amarok);
			projectile.extraUpdates = 0;
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = 99;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.melee = true;
			projectile.scale = 1f;
        }

        public override void AI()
        {
			for (int k = 0; k < 5; k += 1)
			{

			if (spinners[k]==-6){spinners[k]=1;
			int newb=Projectile.NewProjectile(projectile.Center,new Vector2(0f,0f),mod.ProjectileType("SnappyTooth"),60,5,Main.myPlayer, 0f, (float)Main.player[projectile.owner].whoAmI);
			Main.projectile[newb].damage=(int)(projectile.damage*0.75);
			Main.projectile[newb].penetrate=4;
			Main.projectile[newb].ranged=false;
			Main.projectile[newb].melee=true;
			Main.projectile[newb].netUpdate=true;
			orbitors[k]=Main.projectile[newb];


			}else{
			if (orbitors[k]!=null){
			if (orbitors[k].type==mod.ProjectileType("SnappyTooth")){
			double anglez=(k / ((double)5f));
  			double angle=((float)(started/5f))+ 2.0* Math.PI * anglez;
  			Vector2 loc=new Vector2(-1f+(float)((Math.Cos(angle) * 16f)), (float)((Math.Sin(angle) * 16f)));
  			Vector2 gohere=projectile.Center+loc;
  			orbitors[k].Center=gohere+projectile.velocity;
  			orbitors[k].timeLeft=3;
  			orbitors[k].velocity=loc*0.05f;

		}}}
		}



		started+=1;
		}


    }
}