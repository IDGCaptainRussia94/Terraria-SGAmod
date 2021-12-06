using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Terraria;
using Terraria.ModLoader;

namespace SGAmod.Core
{
    class WinForm
    {

        public static void MakeTexture()
        {
            Texture2D atex = ModContent.GetTexture("SGAmod/Extra_60b");
            Texture2D tex = new Texture2D(Main.graphics.GraphicsDevice, atex.Width, atex.Height);

            var datacolors2 = new Color[atex.Width * atex.Height];
            atex.GetData(datacolors2);

            for (int y = 0; y < tex.Height; y++)
            {
                for (int x = 0; x < tex.Width; x += 1)
                {
                    Color here = datacolors2[(int)x + y * tex.Width];
                    datacolors2[(int)x + y * tex.Width] = new Color((int)(here.R * (here.A / 255f)), (int)(here.G * (here.A / 255f)), (int)(here.B * (here.A / 255f)), here.A);
                }

            }

            atex.SetData(datacolors2);

            using (FileStream MS = File.Create(SGAmod.filePath + "/Extra_60AlphaBlend.png"))
            {
                atex.SaveAsPng(MS, tex.Width, tex.Height);
            }

        }

        static internal bool WinHandled
        {
            get
            {

                //MakeTexture();
                if (Main.dedServ || Environment.Is64BitProcess)// || SGAmod.OSType != 0)
                    return false;

                bool didItWork = false;

                Random rand = new Random();
                int chance = SGAConfigClient.Instance.CaptionChance != null ? SGAConfigClient.Instance.CaptionChance : 20;
                if (rand.Next(100) < chance)
                {
                    string[] stringsOfMessages = new string[] {"SGAmod: one 'Hell'ion of a time!",
                    "SGAmod: Coming soon: Viperdrive! By 'Atlas' (it's state of the art!)",
                    "SGAmod: Terraria 1.39999999...",
                    "SGAmod: Dergon edition!",
                    "SGAmod: Does this crash a MAC?",
                    "SGAmod: How low can da frames go?",
                    "SGAmod: Several addictions to IL patches later...",
                    "SGAmod: The stars are quite literally above",
                    "SGAmod: Also try Dimensional Doors!",
                    "SGAmod: You detoured ON the wrong neighborhood",
                    "SGAmod: Shake dat 'ass'embly for me",
                    "SGAmod: What are sprites?",
                    "SGAmod: This update took way too long",
                    "SGAmod: Mixing mods surpreme",
                    "SGAmod: I'm not weird, your too normal",
                    "SGAmod: Oh lookie here!",
                    "SGAmod: Also try Polarities!",
                    "SGAmod: Welcome back Ultranium!",
                    "SGAmod: Shader overdose",
                    "SGAmod: Dragons are best <3",
                    "SGAmod: Ok I'm out of ideas for these lines"
                    };

                    //This is the SLR(Andy) method of grabbing the windows form, Sir AFK's requires a DLL import which... uh, won't work on MACs (corrected me, his updated method does infact work!)
                    //Will have a MAC user test this later (Turing, ya did it!)

                    Form windowForm = default;

                    //This won't take effect if the window isn't focused while loading, not gonna add that for now

                    using (Control control = new Control())//Gotta remake it on this yhread cus it would appear you can't just try to access it otherwise, learning is fun!
                    {
                        _ = control.Handle;//Initalizing property to set it up, disposed
                        if (control.IsHandleCreated)
                        {
                            //Oh I see... when it comes to Forms, invoking does it on THEIR thread, makes alot more sense! Thanks VS!
                            control.Invoke((Action)(() => { windowForm = (Form)Control.FromHandle(Main.instance.Window.Handle); }));
                        }
                        if (windowForm != default)
                        {
                            //Form windowForm = ((Form)Control.FromHandle(Main.instance.Window.Handle));
                            windowForm.Invoke((Action)(() =>
                            {
                                windowForm.Text = stringsOfMessages[Main.rand.Next(stringsOfMessages.Length)];
                                didItWork = true;
                            }));
                        }

                    }
                }

                return didItWork;

            }

    }



    }
}
