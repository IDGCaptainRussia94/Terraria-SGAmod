
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;

namespace SGAmod
{
	[Label("SGA Server Options")]
	public class SGAConfig : ModConfig
	{
		public static SGAConfig Instance;
		// You MUST specify a ConfigScope.
		public override ConfigScope Mode => ConfigScope.ServerSide;

		[Header("Vanilla Changes")]
		[Label("Improved Golem")]
		[Tooltip("Enables/Disables the changed Golem fight, this is presented as an option in the case of other mods who alter golem, to avoid issues")]
		[ReloadRequired]
		[DefaultValue(true)]
		public bool GolemImprovement { get; set; }

		[Label("Lethal Drowning")]
		[Tooltip("Enables/Disables the gradually increased damage taken after 5 seconds of drowning, which only resets when your breath fully recovers")]
		[ReloadRequired]
		[DefaultValue(true)]
		public bool DrowningChange { get; set; }

		[Label("Nerfed Mana Regen Potion")]
		[Tooltip("Enables/Disables the nerfed Mana Regen")]
		[ReloadRequired]
		[DefaultValue(true)]
		public bool ManaPotionChange { get; set; }

		[Label("Negative World Effects")]
		[Tooltip("Enables/Disables world debuffs tied to bosses, like Snowfrosted and Drowning Presence outside of boss fights")]
		[DefaultValue(true)]
		public bool NegativeWorldEffects { get; set; }

		[Label("Early Luminite")]
		[Tooltip("Enables/Disables pre-Moonlord Luminite sources (Astrial Luminite), disable this if it breaks progression with other mods")]
		[DefaultValue(true)]
		public bool EarlyLuminite { get; set; }
	}

	[Label("SGA Client Options")]
	public class SGAConfigClient : ModConfig
	{
		public static SGAConfigClient Instance;
		// You MUST specify a ConfigScope.
		public override ConfigScope Mode => ConfigScope.ClientSide;


		//[Header("$Mods.BossChecklist.Configs.Header.BossLogUI")]

		[Label("Hellion Privacy")]
		[Tooltip("Enables/Disables Hellion refering to the player by their computer login name (will refer to local player name when on)")]
		[DefaultValue(false)]
		public bool HellionPrivacy { get; set; }

		[Label("Epic Apocalypticals")]
		[Tooltip("Enables/Disables The TF2 Crit sound and shockwave effect on Apocalypticals; when off, it only displays the text")]
		[DefaultValue(true)]
		public bool EpicApocalypticals { get; set; }

		[Label("HUD Y displacement")]
		[Tooltip("adjust the verticle offset of the SGAmod HUD elements")]
		[Increment(1)]
		[Range(-300, 300)]
		[DefaultValue(0)]
		[Slider]
		public int HUDDisplacement { get; set; }

		[Header("Performance")]
		[Label("Fog Detail")]
		[Tooltip("More is more detailed, but also more demanding")]
		[Increment(1)]
		[Range(5, 100)]
		[DefaultValue(30)]
		[Slider]
		public int FogDetail { get; set; }

		[Label("Hellion Sky Detail")]
		[Tooltip("Lower this if Hellion's fight is lagging you, a value of 0 disables this.")]
		[Increment(20)]
		[Range(0, 300)]
		[DefaultValue(200)]
		[Slider]
		public int HellionSkyDetail { get; set; }

		[Label("Murk-lite")]
		[Tooltip("Disables the fog from drawing during Murk's fight, and uses a dust indicator instead")]
		[DefaultValue(true)]
		public bool Murklite { get; set; }

		[Label("Extra Blending Details")]
		[Tooltip("Enables/Disables Additive Blending; may improve performance")]
		[DefaultValue(true)]
		public bool SpecialBlending { get; set; }

		[Label("Lava Rocks Gun Shader")]
		[Tooltip("Enables/Disables The Solar Dye on the rocks shot by the Lava Rocks Gun, may improve performance")]
		[DefaultValue(false)]
		public bool LavaBlending { get; set; }

	}


}