﻿
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
	public class SGAConfig : ModConfig
	{
		public static SGAConfig Instance;
		// You MUST specify a ConfigScope.
		public override ConfigScope Mode => ConfigScope.ServerSide;

		[Label("Improved Golem")]
		[Tooltip("Enables/Disables the changed Golem fight, this is presented as an option in the case of other mods who alter golem, to avoid issues")]
		[ReloadRequired]
		[DefaultValue(true)]
		public bool GolemImprovement { get; set; }

	}

	public class SGAConfigClient : ModConfig
	{
		public static SGAConfigClient Instance;
		// You MUST specify a ConfigScope.
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[Label("Hellion Privacy")]
		[Tooltip("Enables/Disables Hellion refering to the player by their computer login name (will refer to local player name when on)")]
		[DefaultValue(false)]
		public bool HellionPrivacy { get; set; }

	}


}