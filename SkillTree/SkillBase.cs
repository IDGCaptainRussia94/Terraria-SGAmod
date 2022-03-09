using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

using SGAmod.Effects;
using SGAmod.SkillTree.Survival;

namespace SGAmod.SkillTree
{
    public class SkillManager
    {
        public List<Skill> Skills;
        public Player player;
        public static Color[] SkillTreeColors = { Color.Aqua, Color.Yellow, Color.Orange, Color.LawnGreen, Color.Blue, Color.Red };
        public static int skillmax = 15000;


        public SkillManager(Player player)
        {
            this.player = player;
            Skills = new List<Skill>();

            //Add Skills
            this.AddSkill(new SkillTrueStrike());//Survival
            this.AddSkill(new SkillNoSurprises());//Survival
            this.AddSkill(new SkillBulwark());//Survival

        }

        public Skill AddSkill(Skill skilltype)
        {
            Skills.Add(skilltype);
            skilltype.player = player;
            skilltype.manager = this;
            skilltype.skillID = (ushort)(Skills.Count - 1);
            return skilltype;
        }

        public void LinkSkills(Skill skill1, Skill skill2)
        {
            skill1.linkedSkillsTo.Add(skill2);
            skill2.linkedSkillsFrom.Add(skill1);
        }
        public void DoSKillLinkUps()
        {
            foreach (Skill skill in Skills)
            {
                skill.DoLinkUp();
            }
        }


        //This is REALLY ugly, there has to be a better way to do this :/
        public virtual void PostUpdateRunSpeeds() { foreach (Skill skill in Skills) { if (skill.levels > 0) { skill.PostUpdateRunSpeeds(); } } }
        public virtual void PostUpdateEquips() { foreach (Skill skill in Skills) { if (skill.levels > 0) { skill.PostUpdateEquips(); } } }
        public virtual void GetHealMana(Item item, bool quickHeal, ref int healValue) { foreach (Skill skill in Skills) { if (skill.levels > 0) { skill.GetHealMana(item, quickHeal, ref healValue); } } }
        public virtual void GetHealLife(Item item, bool quickHeal, ref int healValue) { foreach (Skill skill in Skills) { if (skill.levels > 0) { skill.GetHealLife(item, quickHeal, ref healValue); } } }
        public virtual void NaturalLifeRegen(ref float regen) { foreach (Skill skill in Skills) { if (skill.levels > 0) { skill.NaturalLifeRegen(ref regen); } } }
        public virtual void OnPlayerDamage(ref int damage, ref bool crit, NPC npc, Projectile proj) { foreach (Skill skill in Skills) { if (skill.levels > 0) { skill.OnPlayerDamage(ref damage, ref crit, npc, proj); } } }
        public virtual void OnEnemyDamage(ref int damage, ref bool crit, ref float knockback, Item item, Projectile proj) { foreach (Skill skill in Skills) { if (skill.levels > 0) { skill.OnEnemyDamage(ref damage, ref crit, ref knockback, item, proj); } } }
        public virtual void ReforgePrice(Item item, ref int reforgePrice, ref bool canApplyDiscount) { foreach (Skill skill in Skills) { if (skill.levels > 0) { skill.ReforgePrice(item, ref reforgePrice, ref canApplyDiscount); } } }
        public virtual void UseItem(Item item) { foreach (Skill skill in Skills) { if (skill.levels > 0) { skill.UseItem(item); } } }
        public virtual void UseTimeMultiplier(Item item, ref float current) { foreach (Skill skill in Skills) { if (skill.levels > 0) { skill.UseTimeMultiplier(item, ref current); } } }
        public virtual void GetWeaponDamage(Item item, ref int damage) { foreach (Skill skill in Skills) { if (skill.levels > 0) { skill.GetWeaponDamage(item, ref damage); } } }
        //Item item, Player player, ref int damage
    }



    public abstract class Skill
    {
        public ushort[] buycost;
        public ushort unlockcost = 0;
        public byte maxlevels = 1;
        public List<ushort> linkedSkillsushort;
        public Vector2 treelocation = new Vector2(0.50f, 0);
        public string displaytext = "Skill Desc";
        public string displayname = "Skill Name";
        public Player player;
        public SkillManager manager;
        public ushort skillID = 0;
        public Color skillColor = Color.White;
        public Rectangle skillBox;

        public byte levels = 0;
        public List<Skill> linkedSkillsTo;
        public List<Skill> linkedSkillsFrom;

        public virtual byte getlevels => levels;

        public virtual bool TryClick()
        {
            OnClick();
            return true;
        }

        public virtual void OnClick()
        {

        }

        public Skill(List<ushort> linkedSkillsushort = null)
        {
            Init(linkedSkillsushort);
        }

        private void Init(List<ushort> linkedSkillsushort = null)
        {
            skillBox = new Rectangle(0, 0, 16, 16);
            linkedSkillsushort = linkedSkillsushort ?? new List<ushort>();
            linkedSkillsTo = new List<Skill>();
            linkedSkillsFrom = new List<Skill>();
            this.linkedSkillsushort = linkedSkillsushort;
        }

        public virtual void PostUpdateRunSpeeds() { }
        public virtual void PostUpdateEquips() { }
        public virtual void GetHealMana(Item item, bool quickHeal, ref int healValue) { }
        public virtual void GetHealLife(Item item, bool quickHeal, ref int healValue) { }
        public virtual void NaturalLifeRegen(ref float regen) { }
        public virtual void OnPlayerDamage(ref int damage, ref bool crit, NPC npc, Projectile proj) { }
        public virtual void OnEnemyDamage(ref int damage, ref bool crit, ref float knockback, Item item, Projectile proj) { }
        public virtual bool ReforgePrice(Item item, ref int reforgePrice, ref bool canApplyDiscount) { return true; }
        public virtual bool UseItem(Item item) { return true; }
        public virtual void UseTimeMultiplier(Item item, ref float current) { }
        public virtual void GetWeaponDamage(Item item, ref int damage) { }

        public void DoLinkUp()
        {
            foreach (Skill skill in manager.Skills)
            {
                if (linkedSkillsushort.Exists(parm => parm == skill.skillID))
                {
                    linkedSkillsTo.Add(skill);
                    skill.linkedSkillsFrom.Add(this);
                }
            }
        }

        public int CheckRequirements()
        {
            int state = 0;

            if (levels == maxlevels)
            {
                state = -1;
            }
            else
            {
                if (Main.LocalPlayer.SGAPly().ExpertiseCollectedTotal < unlockcost)
                {
                    state = 15;
                }
                else
                {
                    if (!CheckLinks())
                        state += 2;
                    if (Main.LocalPlayer.SGAPly().ExpertiseCollected < buycost[levels])
                        state += 1;
                }
            }
            return state;
        }

        public bool CheckLinks()
        {
            if (linkedSkillsTo.Count > 0)
            {
                foreach (Skill skill in linkedSkillsTo)
                {
                    if (skill.levels < 1)
                        return false;
                }
            }
            return true;
        }

        public virtual void SkillActive()
        {
            //Blank;
        }

    }

    public class SKillUI
    {
        public static int SkillUITimer = 0;
        public static VertexBuffer vertexBuffer;
        public static BasicEffect basicEffect;
        public static Matrix world = WVP.World();
        public static Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
        public static Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 600f, 0.01f, 500f);
        public static float UIAngle=0f;
        public static float UIAngleTo = MathHelper.ToRadians(180);

        public static float PercentLerp(float value,float minsize,float maxsize,float totalsize)
        {
            return minsize + (Math.Min(value / totalsize,1f)) * (maxsize - minsize - 64f);
        }

        public static Vector2 CircleLocation(float x, float y,int rad, float minsize, float maxsize)
        {
            int treecount = SkillManager.SkillTreeColors.Length;

            float angle1 = MathHelper.ToRadians((y / (float)treecount) * 360f);
            angle1 += MathHelper.ToRadians(x * (360f / (float)treecount));
            return new Vector2(SKillUI.PercentLerp(rad, minsize, maxsize, SkillManager.skillmax), 0).RotatedBy(-angle1);
        }


        public static void UpdateUI(bool draw = false)
        {
            int treecount = SkillManager.SkillTreeColors.Length;
            float maxsize = 600;
            float minsize = 64;

            float thesize = SKillUI.PercentLerp((float)Main.LocalPlayer.SGAPly().ExpertiseCollectedTotal, minsize, maxsize, SkillManager.skillmax) ;

            Vector3 loc = new Vector3(Main.screenWidth / 2, Main.screenHeight/2f, 0);

            if (!draw)
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            if (draw)
            {
                Main.spriteBatch.Draw(Main.blackTileTexture, Vector2.Zero, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), (Color.Purple * 1f), 0, Vector2.Zero, new Vector2(1f, 1f), SpriteEffects.None, 0f);

                Matrix DrawMatrix = Matrix.CreateScale(1f, 1f, 1f);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, DrawMatrix);


                basicEffect.World = WVP.World();
                basicEffect.View = WVP.View(new Vector2(1f, 1f));
                basicEffect.Projection = WVP.Projection();
                basicEffect.VertexColorEnabled = true;

                int detail = 60;
                int polys = 1;
                VertexPositionColor[] vertices = new VertexPositionColor[detail * (polys * 3)];

                float ripplecount = 20;
                float ripplesize = 1f;
                float overallradius = thesize * (1f - (1f / (1f + SKillUI.SkillUITimer / 10f)));
                Random rand = new Random(0);

                float[] previous = { 90000f, 90000f };
                float[] starts = { 0f, 0f };

                for (int tree = 0; tree < treecount; tree += 1)
                {

                    for (int i = 0; i < vertices.Length - 3; i += (polys * 3))
                    {
                        float adder = UIAngle + MathHelper.ToRadians(((float)tree / (float)treecount) * 360f);
                        float maxdegre = (360f / (float)treecount);
                        float rad1 = (((float)i / (vertices.Length - (polys * 3))) * 360f) * ripplecount;
                        float rad2 = (((float)(i + (polys * 3)) / (vertices.Length - (polys * 3))) * 360f) * ripplecount;
                        if (previous[0] > 89999f)
                        {
                            previous[0] = (float)rand.NextDouble() * 10f; starts[0] = previous[0];
                            previous[1] = (float)rand.NextDouble(); starts[1] = previous[1];
                        }
                        float radius = overallradius + (float)Math.Sin(MathHelper.ToRadians(rad1 + Main.GlobalTime * (90f + previous[0]))) * ((0.50f + previous[1]) * ripplesize);
                        previous[0] = (float)rand.NextDouble() * 10f;
                        previous[1] = (float)rand.NextDouble();
                        if (i + 3 >= vertices.Length) { previous = starts; }

                        float radius2 = overallradius + (float)Math.Sin(MathHelper.ToRadians(rad2 + Main.GlobalTime * (90f + previous[0]))) * ((0.50f + previous[1]) * ripplesize);

                        float angle = -(adder + MathHelper.ToRadians(((float)i / (vertices.Length - (polys * 3))) * maxdegre));
                        Vector3 theplace = new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), 0) * radius;

                        float angle2 = angle - (MathHelper.ToRadians(((float)(polys * 3) / (vertices.Length - (polys * 3))) * maxdegre));
                        Vector3 theplace2 = new Vector3((float)Math.Cos(angle2), (float)Math.Sin(angle2), 0) * radius2;

                        //Color thecolor = SkillManager.SkillTreeColors[(int)(((float)i / (float)vertices.Length) * (float)treecount)];
                        Color thecolor = SkillManager.SkillTreeColors[tree];

                        vertices[0 + (i)] = new VertexPositionColor(loc, Color.White);
                        vertices[1 + (i)] = new VertexPositionColor(loc + theplace2, thecolor);
                        vertices[2 + (i)] = new VertexPositionColor(loc + theplace, thecolor);
                    }

                    SKillUI.vertexBuffer = new VertexBuffer(Main.graphics.GraphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.WriteOnly);
                    SKillUI.vertexBuffer.SetData<VertexPositionColor>(vertices);

                    Main.graphics.GraphicsDevice.SetVertexBuffer(vertexBuffer);

                    RasterizerState rasterizerState = new RasterizerState();
                    rasterizerState.CullMode = CullMode.None;
                    Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

                    foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        Main.graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, polys * detail);
                    }
                }

            }

                Vector2 loc2 = new Vector2(loc.X, loc.Y);

            if (draw) 
            {
                //Lines and stuff
                for (int i = 0; i < treecount; i += 1)
                {
                    Main.spriteBatch.Draw(Main.blackTileTexture, loc2, new Rectangle(0, 0, 10, 10), Color.Black, -UIAngle + MathHelper.ToRadians(((float)i / (float)treecount) * 360f), new Vector2(0f, 5f), new Vector2(maxsize / 10f, 1f), SpriteEffects.None, 0f);
                }
                for (int i = 0; i < treecount; i += 1)
                {
                    Main.spriteBatch.Draw(Main.blackTileTexture, loc2, new Rectangle(0, 0, 10, 6), Color.Gray, -UIAngle + MathHelper.ToRadians(((float)i / (float)treecount) * 360f), new Vector2(0f, 3f), new Vector2(maxsize / 10f, 1f), SpriteEffects.None, 0f);
                }
                for (int i = 0; i < treecount; i += 1)
                {
                    Main.spriteBatch.Draw(Main.blackTileTexture, loc2, new Rectangle(0, 0, 10, 4), Color.White, -UIAngle + MathHelper.ToRadians(((float)i / (float)treecount) * 360f), new Vector2(0f, 2f), new Vector2(maxsize / 10f, 1f), SpriteEffects.None, 0f);
                }
            }

            foreach(Skill skill in Main.LocalPlayer.SGAPly().skillMananger.Skills)
            {
                //float angle1 = MathHelper.ToRadians((skill.treelocation.Y / (float)treecount) * 360f);
                //angle1 += MathHelper.ToRadians(skill.treelocation.X * (360f/(float)treecount));
                //Vector2 loc3 = new Vector2(SKillUI.PercentLerp(skill.unlockcost, minsize, maxsize, SkillManager.skillmax),0).RotatedBy(-angle1);

                Vector2 loc3 = SKillUI.CircleLocation(skill.treelocation.X, skill.treelocation.Y, skill.unlockcost, minsize,maxsize);

                if (draw)
                    Main.spriteBatch.Draw(Main.blackTileTexture, loc2+loc3, skill.skillBox, Color.Black*0.75f, 0, skill.skillBox.Size()/2f, new Vector2(1f, 1f), SpriteEffects.None, 0f);

            }

            if (draw)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
            }



            if (!draw)
                UIAngle=UIAngle.AngleLerp(UIAngleTo, 0.1f);

        }



        public static bool DrawSkillUI()
        {
        SKillUI.UpdateUI(true);
            return true;
        }

        public static void InitThings()
        {
            if (!Main.dedServ)
            {
                SKillUI.basicEffect = new BasicEffect(Main.graphics.GraphicsDevice);

                /*VertexPositionColor[] vertices = new VertexPositionColor[3];
                vertices[0] = new VertexPositionColor(new Vector3(0, 1f, 0), Color.Red);
                vertices[1] = new VertexPositionColor(new Vector3(+1f, -1f, 0), Color.Green);
                vertices[2] = new VertexPositionColor(new Vector3(-1f, -1f, 0), Color.Blue);

                SKillUI.vertexBuffer = new VertexBuffer(Main.graphics.GraphicsDevice, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
                SKillUI.vertexBuffer.SetData<VertexPositionColor>(vertices);*/
            }
        }

    }

}
