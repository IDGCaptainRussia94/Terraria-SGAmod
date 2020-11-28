using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SGAmod.Items.Weapons;

namespace SGAmod
{
    public class SGAprojectile : GlobalProjectile
    {
	public Player myplayer=null;

		public override bool InstancePerEntity
		{
			get
			{
				return true;
			}
		}
	public bool inttime=false;
	public bool enhancedbees=false;
	public bool splittingcoins=false;
	public bool raindown=false;
		public bool embued = false;
		public bool onehit = false;
	public Vector2 splithere=new Vector2(0,0);
		public int shortlightning = 0;
		public bool stackedattack=false;
		public bool rerouted = false;

		/*private List<int> debuffs=new List<int>();
		private List<int> debufftime=new List<int>();

			public void AddDebuff(int debuff,int time) 
			{
			debuffs.Insert(debuffs.Count+1,debuff);
			debufftime.Insert(debufftime.Count+1,time);
			}

			public override void ModifyHitNPC(Projectile prog,NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
				applydebuffs(target,null);
			}
			public override void ModifyHitPlayer(Projectile prog,Player target, ref int damage, ref bool crit){
				applydebuffs(null,target);
			}

			private void applydebuffs(NPC target,Player ply){
			for (int i = 0; i < debuffs.Count;i++ )
			 {
			 if (target!=null){target.AddBuff(debuffs[i], debufftime[i], true);}
			 if (ply!=null){ply.AddBuff(debuffs[i], debufftime[i], true);}
		}
		}*/
		//for (x = 0; x < questvars.Length; x++)
		//{

		public void Embue(Projectile projectile2)
		{
			if (!embued)
			{
				for (float i = 1f; i < 12; i += 0.8f)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					int dust = Dust.NewDust(projectile2.position, projectile2.width, projectile2.height, DustID.AncientLight, 0f, 0f, 100, default(Color), 1f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].fadeIn = 0.6f;
					Main.dust[dust].velocity = randomcircle * i;
					Main.dust[dust].color = Main.hslToRgb(((float)(Main.GlobalTime / 3) + (float)projectile2.whoAmI * 7.16237f) % 1f, 0.9f, 0.65f);
				}

				embued = true;
			}
		}

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			if (projectile.friendly)
			{
				Player owner = Main.player[projectile.owner];
				if (owner != null)
				{
					if (projectile.melee && owner.heldProj == projectile.whoAmI)
						damage = (int)((float)damage * owner.SGAPly().trueMeleeDamage);
				}
			}

			if (embued)
				damage = (int)(projectile.damage * 1.50f);
		}
        public override bool? CanHitNPC(Projectile projectile, NPC target)
		{
			if (projectile.type == ProjectileID.FlamethrowerTrap && projectile.owner > -1)
				return false;
			return null;
		}
		public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
		{
			if (embued)
			{
				target.AddBuff(mod.BuffType("MoonLightCurse"),90);
			}

				if (onehit)
				projectile.Kill();
		}

		public override bool PreKill(Projectile projectile, int timeLeft)
		{

			Player owner = Main.player[projectile.owner];
			if (owner != null)
			{
				if (owner.SGAPly().SybariteGem)
				{
					if (Main.rand.Next(0, 4) == 0)
					{
						int itemid;
						if (SGAmod.CoinsAndProjectiles.TryGetValue(projectile.type, out itemid))
						{
							Item.NewItem(projectile.Center, Vector2.Zero, itemid);
						}
					}
				}

			}

			return true;
		}

        public override void PostAI(Projectile projectile)
		{
			Player owner = Main.player[projectile.owner];
			if (owner != null)
			{
				if (projectile.melee)
				{
					SGAPlayer sgaply= owner.SGAPly();

					//Main.NewText("test " + projectile.coldDamage);

					if (sgaply.glacialStone)
					{
						if (!owner.frostBurn && projectile.friendly && !projectile.hostile && !projectile.noEnchantments && Main.rand.Next(2 * (1 + projectile.extraUpdates)) == 0)
						{
							int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 135, projectile.velocity.X * 0.2f + (float)(projectile.direction * 3), projectile.velocity.Y * 0.2f, 100, default(Color), 2f);
							Main.dust[num].noGravity = true;
							Main.dust[num].velocity *= 0.7f;
							Main.dust[num].velocity.Y -= 0.5f;
						}
						projectile.coldDamage = true;

					}

					sgaply.FlaskEffects(new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height), projectile.velocity);
				}
			}

			if (embued)
			{
				int dust = Dust.NewDust(projectile.position,projectile.width, projectile.height, DustID.AncientLight, 0f, 0f, 100, default(Color), 1f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].fadeIn = 0.6f;
				Main.dust[dust].velocity = projectile.velocity*Main.rand.NextFloat(0.25f,0.80f);
				Main.dust[dust].color = Main.hslToRgb(((float)(Main.GlobalTime / 3)+(float)projectile.whoAmI*7.16237f) % 1f, 0.9f, 0.65f);
			}

			SGAprojectile modeproj = projectile.GetGlobalProjectile<SGAprojectile>();
			if (projectile.owner < 255 && Main.player[projectile.owner].active && projectile.friendly && !projectile.hostile)
			{
				if (!modeproj.stackedattack)
				{
					owner.GetModPlayer<SGAPlayer>().StackAttack(ref projectile.damage, projectile);
				}
			}
			modeproj.stackedattack = true;

			if (shortlightning > 0)
			{

				for (int i = 0; i < Math.Min(shortlightning, projectile.oldPos.Length); i++)
				{
					projectile.oldPos[i].X = projectile.position.X;
					projectile.oldPos[i].Y = projectile.position.Y;
				}

			}
			if (projectile.modProjectile != null)
			{
				Player projowner = Main.player[projectile.owner];
				if (projectile.modProjectile.mod == SGAmod.Instance && projowner.active && projowner.heldProj == projectile.whoAmI)
				{
					projectile.Opacity = MathHelper.Clamp(projowner.stealth, 0.1f, 1f)*Math.Min(projectile.modProjectile is ClipWeaponReloading sub ? (float)projectile.timeLeft/20f : 1f,1f);
				}
			}
		}
		public override bool PreAI(Projectile projectile)
		{
		SGAprojectile modeproj=projectile.GetGlobalProjectile<SGAprojectile>();



			if (projectile.modProjectile!=null){

		/*if ((projectile.modProjectile).GetType().Name=="JackpotRocket"){
		projectile.velocity.Y+=0.1f;
		projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f; 
		}*/

	}

			/*if (Main.player[projectile.owner]!=null);
			ply=Main.player[projectile.owner];
			if (Main.npc[projectile.owner]!=null);
			npc=Main.npc[projectile.owner];
			if (npc!=null && modeproj.inttime==true){
			if (modeproj.raindown==true && projectile.position.Y>modeproj.splithere.Y && projectile.velocity.Y>0){
			projectile.timeLeft=-1;

			}
			if (modeproj.splittingcoins==true){
			if ((projectile.position.Y>modeproj.splithere.Y && projectile.velocity.Y>0) || (projectile.position.Y<modeproj.splithere.Y && projectile.velocity.Y<0)){
			SgaLib.Shattershots(projectile.position,projectile.position+new Vector2(-100,0),new Vector2(0,0),projectile.type,projectile.damage,projectile.velocity.Length(),0,1,true,0,true,projectile.timeLeft);
			SgaLib.Shattershots(projectile.position,projectile.position+new Vector2(100,0),new Vector2(0,0),projectile.type,projectile.damage,projectile.velocity.Length(),0,1,true,0,true,projectile.timeLeft);
			//projectile.position=new Vector2(0,-900);
			projectile.timeLeft=-1;
		}}

			}*/
			if (Main.player[projectile.owner] != null)
			{
				Player ply = Main.player[projectile.owner];
				if (ply != null)
				{
					SGAPlayer modplayer = ply.GetModPlayer<SGAPlayer>();
					if (ply != null)
					{
						if (modplayer.beefield > 0)
						{
							//modeproj.enhancedbees == true
							if ((projectile.type == 181 || projectile.type==ProjectileID.GiantBee) && modplayer.beefieldtoggle > 0)
							{
								if (projectile.velocity.Length() > 20)
								{
									projectile.velocity.Normalize();
									projectile.velocity = projectile.velocity * 0.98f;
								}
								else
								{
									projectile.velocity = projectile.velocity * 1.15f;
								}
							}
						}
					}
				}
			}
		modeproj.inttime=true;
		return true;
		}

	}

}