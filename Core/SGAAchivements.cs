//#define WebmilioCommonsPresent
#define DEBUG
#define DefineHellionUpdate
#define Dimensions


using System;
using System.Linq;
using Terraria;
using Terraria.ModLoader;


//using SubworldLibrary;

namespace SGAmod.Achivements
{

	public abstract class SGAAchivements
	{
		public static bool AchivementsLoaded = false;
		public static Player who;
		public static Mod SGAchivement = null;

		public static void UnlockAchivement(string achive, Player who2)
		{
			if (who2 != null)
			{
				SGAAchivements.SGAchivement = ModLoader.GetMod("SGAmodAchivements");
				if (SGAAchivements.SGAchivement != null)
				{
					SGAAchivements.who = who2;
					UnlockAchivement2 = achive;
					SGAAchivements.who = null;
				}
			}
		}

		public static string UnlockAchivement2
		{
			set
			{
				if (value == "Copper Wraith")
					SGAAchivements.SGAchivement.Call("Copper Wraith", who);
				if (value == "Caliburn")
					if (SGAWorld.downedCaliburnGuardians > 2)
						SGAAchivements.SGAchivement.Call("Caliburn", who);
				if (value == "Spider Queen")
					SGAAchivements.SGAchivement.Call("Spider Queen", who);
				if (value == "Murk")
				{
					SGAAchivements.SGAchivement.Call("Murk", who);
					if (Main.hardMode)
						SGAAchivements.SGAchivement.Call("Murk2", who);
				}
				if (value == "Cobalt Wraith")
					SGAAchivements.SGAchivement.Call("Cobalt Wraith", who);
				if (value == "Cirno")
					SGAAchivements.SGAchivement.Call("Cirno", who);
				if (value == "Sharkvern")
					SGAAchivements.SGAchivement.Call("Sharkvern", who);
				if (value == "Cratrosity")
					SGAAchivements.SGAchivement.Call("Cratrosity", who);
				if (value == "TPD")
					SGAAchivements.SGAchivement.Call("TPD", who);
				if (value == "Harbinger")
					SGAAchivements.SGAchivement.Call("Harbinger", who);
				if (value == "Luminite Wraith")
					SGAAchivements.SGAchivement.Call("Luminite Wraith", who);
				if (value == "SPinky")
					SGAAchivements.SGAchivement.Call("SPinky", who);
				if (value == "Cratrogeddon")
					SGAAchivements.SGAchivement.Call("Cratrogeddon", who);
				if (value == "Hellion")
					SGAAchivements.SGAchivement.Call("Hellion", who);
				if (value == "Offender")
				{
					if (SGAWorld.downedWraiths > 2 &&
						SGAWorld.downedCaliburnGuardians > 3 &&
						SGAWorld.downedSpiderQueen &&
						SGAWorld.downedMurk > 1 &&
						SGAWorld.downedCirno &&
						SGAWorld.downedSharkvern &&
						SGAWorld.downedCratrosity &&
						SGAWorld.downedHarbinger &&
						SGAWorld.downedTPD &&
						SGAWorld.downedSPinky && Main.expertMode)
						SGAAchivements.SGAchivement.Call("Legendary Offender", who);

					if (SGAWorld.downedWraiths > 0 &&
						SGAWorld.downedCaliburnGuardians > 3 &&
						SGAWorld.downedSpiderQueen &&
						SGAWorld.downedMurk > 0 && Main.expertMode && SGAWorld.NightmareHardcore > 0)
						SGAAchivements.SGAchivement.Call("Mythical Offender", who);

					if (SGAWorld.downedMurk > 1 &&
						SGAWorld.downedWraiths > 1 &&
						SGAWorld.downedCirno &&
						SGAWorld.downedSharkvern &&
						SGAWorld.downedCratrosity &&
						SGAWorld.downedHarbinger &&
						SGAWorld.downedTPD && Main.expertMode && SGAWorld.NightmareHardcore > 0)
						SGAAchivements.SGAchivement.Call("Transcendent Offender", who);
				}
			}
		}



	}

}