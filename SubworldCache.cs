using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.IO;
using System.Reflection;

namespace SGAmod
{
	//Hacky workaround for Subworld Data Cache by IDGCaptainRussia94
    public class SubworldCache
    {
		public static List<SubworldCacheClass> cache;
		public static bool postCacheFields=false;
		public static Mod mod;

		public static void InitCache()
		{
			cache = new List<SubworldCacheClass>();
			mod = SGAmod.Instance;
		}

		public static void UnloadCache()
		{
			cache = null;
			mod = null;
		}

		public static void UpdateCache()
		{
			if (!SubworldCache.postCacheFields || SubworldCache.cache.Count<1)
				return;

			for(int i = 0; i < SubworldCache.cache.Count; i += 1)
			{
				SubworldCacheClass cachee = SubworldCache.cache[i];

				SGAmod.Instance.Logger.Debug("Mod: " + mod.GetType().Name + " World: " + cachee.modwld + " bool: " + cachee.mybool);

				Type modwld = mod.GetModWorld(cachee.modwld).GetType();
				FieldInfo fild = modwld.GetField(cachee.field, BindingFlags.Static | BindingFlags.Public);
				if (cachee.myint != null)
				fild.SetValue(mod.GetModWorld(cachee.modwld), (int)cachee.myint);
				else
				fild.SetValue(mod.GetModWorld(cachee.modwld), (bool)cachee.mybool);
			}


			SubworldCache.postCacheFields = false;
			cache.Clear();
		}

		public static bool AddCache(string mod, string modWorld, string field, bool? mybool, int? myint)
		{
				SubworldCacheClass newone = new SubworldCacheClass(mod, modWorld, field, mybool, myint);
				SubworldCache.cache.Add(newone);
				SubworldCache.postCacheFields = true;
				SGAmod.Instance.Logger.Debug("Added---- Mod: " + mod.GetType().Name + " World: " + modWorld + " bool: " + mybool);
				return true;
		}


    }

	public class SubworldCacheClass
	{
		public string field;
		public bool? mybool;
		public int? myint;
		public string modwld;
		public string mod;
		public SubworldCacheClass(string mod, string modWorld, string field, bool? mybool,int? myint)
		{
			this.mod = mod;
			this.modwld = modWorld;
			this.field = field;
			this.mybool = mybool;
			this.myint = myint;
		}

	}
}
