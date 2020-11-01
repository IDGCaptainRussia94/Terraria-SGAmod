using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Idglibrary;

namespace SGAmod.Items.Accessories
{
	public class LostNotes : ModItem
	{
		public int notetype = 0;
		public virtual int totallength=>4;
        public override bool CloneNewInstances => true;
        public virtual string[,] NoteWords => new string[,] { { ":Date 62 AC:", "This is it, I've finally made it to the forgotten lands in hopes of finding wealth and fortune!","It wouldn't be long now! I could already see the coast, eagerly awaiting it!", "With my rucksack and tools in hand, surely nothing can go wrong, right?" },
		{":Date 62 AC:","I met up with the settlment on the isles, looking for a resupply before heading out, and given my desire for some booze went to the tavern","Pretty lighthearted place, a little on the shady side however, but you never know in these times, I took a seat on the first stool at the bar I could find","When the tavernkeep came over, I ordered up some wine,rented a room, and asked what jobs there are for some quick cash." },
		{":Date 62 AC:","Monster hunting, mining, and exploring where none had gone before, pretty standard jobs they had","He also said something about Etheria, but at the same time where were people pointing out he let kids stay in a clearly adult tavern, something about defenders, I laughed it off, and went up to my room","So? This is it? The adventure I've dreamed of? Well, I guess I can consider my actions after I take a wee nap." },
		{":Date 68 AC:","I had first run into something odd... A little girl, in the middle of the dark. Being the parent back home that I am I approuched her.","This however proved to be my folly, as her sweet and innocent apperence quickly turned twisted and... lusty","Needless to say I ran" },
		{":Date 74 AC:","With one final blow, the monsterious wall of meat came crashing down, for one moment I felt like an utter god!","But it seems said gods had something else in mind when I had unwillingly released","but non the less, I had high hopes!"},
		{":Date 74 AC:","Things didn't quite go as well as I had hoped, when I got back to the settlement everyone was dead, impaled through the chest but a sharp horn-like object","Furthermore... the entire area was a pastel rainbow color, and it wasn't too long before I found out the cause...","(something was wrote here, but it is too badly written to read)"}
		};
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lost Notes");
			//Tooltip.SetDefault("25% increased mining/hammering/chopping speed");
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			for(int i=0;i< totallength; i+=1)
			{
				if (NoteWords[notetype, i].Length>0)
				tooltips.Add(new TooltipLine(mod, "NoteWords", NoteWords[notetype, i]));
			}
		}

		public override TagCompound Save()
		{
			TagCompound tag = new TagCompound();
			tag["notetype"] = notetype;
			return tag;
		}
		public override void Load(TagCompound tag)
		{
			notetype = tag.GetInt("notetype");
		}

		public override void NetSend(BinaryWriter writer)
		{
			writer.Write((short)notetype);
		}

		public override void NetRecieve(BinaryReader reader)
		{
			notetype = reader.ReadInt16();
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 0;
			//item.accessory = true;
		}

	}

	public class BossHints : ModItem
	{
		public int notetype = 0;
		public int totallength => 10;
		public string[,] NoteWords => new string[,] {

			{"Contains research notes into the current SGAmod boss yet to be fought","How did Draken get these?","","","","","","","",""},


			{"Case Speciman: WC01-S -aka- Copper Wraith", "Threat: None","Known Alliegence: Paradox Coalition, Allied",
				"Following the excape of the DRAKEN experiment, scout classes were manufactured",
				"Armed with the ability to animate light and weak metals, WC01-S searches the lands for the experiment",
				"Like all Wraith Cores it is able to discharge beams of energy in a pitch from its core",
				"These cores are, however, suspectible to direct shattering hits, such as primative arms like arrows",
				"More research is needed into better protecting the main frame...","",""},

{"Case Speciman: Caliburn Gaurdian", "Threat: Unknown, Likely low","Known Alliegence: Unknown",
				"These animated swords are strange, they resonate the same energy used to power wraith cores",
				"They could be orginal source of power before 'Her Supremacy' aquired the tech elsewhere",
				"Non the less, their purpose, and the purpose of these shrines remains unknown",
				"What is known is that there are 3, and each defeated gaurdian only serves to empower the next",
				"More research is required into the matter...","",""},

{"Case Speciman: Spider Queen", "Threat: low","Known Alliegence: Fauna",
				"A giant spider, not unlike the other spiders seen across the various worlds",
				"This one however has particually powerful acidic venom, which could melt the strongest of metals",
				"Not the priority of the head of research, but could be used in future weapons",
				"Provided of course the acid just doesn't melt the wraith soldiers themselves",
				"Outside of the acid she seems to have the ability to weave webs to stop attackers in their tracks","Quite a powerful, brutish creature indeed...",""},

		{"Case Speciman: Murk", "Threat: True power unknown, likely low","Known Alliegence: Slimes?",
				"A possible relative to the King Slime Specimen, this 'Murk' seems to possess an uncanny ability to control flies",
				"Flies... that have stingers, an obey Slime's every order, surprisingly powerful all things considered",
				"There's something off about this creature, it seems to resonate an unholy power",
				"Maybe that power would be released if the power of all other things in this world would be released?",
				"Until then, there's nothing of much use in this creature to us","",""},

					{"Case Speciman: WC02 -aka- Cobalt Wraith", "Threat: None","Known Alliegence: Paradox Coalition, Allied",
				"A standard issue battleclass Wraith, able combat large groups of dissident forces",
				"This one was released to this world when a great surge of energy was detected",
				"it has a form of primitive self learning, in the current process of stealing local knowledge",
				"Like before however, the cores are suspectible to direct shattering hits, even bullets",
				"Head of RnD is currently looking into expanding on the artifical intelligence fields of wraiths","They claim they can create something called an 'Emissary' from it, we don't have clearence","For more into this matter, signing off for now"},

		{"Case Profile: Hellon 'Hellion' Weygold", "Threat: I would honestly not like to know","Known Alliegence: Paradox Coalition, Leader",
				"'Her Supremacy' as many of us are forced to address, is a Hellion, the leader of the Paradox Coalition",
				"We are not allowed to talk much about Hellion, but she has almost godly powers as we can call them",
				"Extremely cruel and cunning, has no problems with scorching entire planets when they refuse to meet her demands",
				"But this power is nothing compared to what the DRAKEN experiment will be, far, far more",
				"Hellion has deemed it necessary to elimanate the Terrarian herself, and take the experiment back",
				"We have no dount that the mission will succeed, sighing off",""},


		};
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Anomaly Study Paper");
			//Tooltip.SetDefault("25% increased mining/hammering/chopping speed");
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Accessories/LostNotes"); }
		}

		public override void UpdateInventory(Player player)
		{
			if (notetype == 0)
			{
				notetype = 1;
				if (SGAWorld.downedWraiths > 0)
					notetype = 2;
				if (SGAWorld.downedCaliburnGuardians > 0)
					notetype = 3;
				if (SGAWorld.downedSpiderQueen)
					notetype = 4;
				if (SGAWorld.downedMurk>0)
					notetype = 5;
				if (SGAWorld.downedWraiths > 1)
					notetype = 6;

				item.rare = notetype;
			}
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{

			for (int i = 0; i < totallength; i += 1)
			{
				if (NoteWords[item.rare, i].Length > 0)
				{
					tooltips.Add(new TooltipLine(mod, "NoteWords", NoteWords[item.rare, i]));
				}
			}
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.buyPrice(0, 0, 10, 0);
			item.rare = 0;
			//item.accessory = true;
		}
	}
}