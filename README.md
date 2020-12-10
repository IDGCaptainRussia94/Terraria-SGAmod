# SGAmod
IDGCaptainRussia94's content mod

//Items with place holder sprites and need resprites: Heart Guard, Hellion's Cascade, Twines of Fate, MVM Upgrade, Jagged Overgrown Spike, Joyrider, Truely Suspicious Looking Eye, Hellion's Crown (Item Sprite ?), All Buff Icons

//Items that are NOT ok to be resprited: Stormbreaker, Wave Beam, Serpent's Redemption, Creeper's Throw, The trap related weapons for now

Github was moved: previous builds of the mod can be found over here: https://github.com/IDGCaptainRussia94/SGAmod

-Crimson only: Jarate, Jarocket, Jarate Shurikens, Heart Guard, Sanguine Bident, Crimson Catastrophe (can be bypassed with fishing)
-Corruption only: Gas Passer, Wraith Arrows, Dosed Arrows, Corrupted Tome, Cursed Hail, Jagged Spike Buckler (can be bypassed with fishing)


-Ideas: Banners, the Couch, Bannerlord=collection of banners, Spectre homing bullets/projectiles
Cruciatus, Charon's Ferry, Overwatch (Null Watcher), Torch God's Summon

-Add Hellion Army progress Bar, Add config options to lower effects, Add lower-end murk fog, rework cobalt wraith water bolts

Changelog:

3.460-(12/5/20)-Patch 9-Shield-N-Spells:


--Added:

-Prismal Air Tank, Prismal Diving Gear, Fluid Displacer, Normal Quiver, Magus Slippers, Star Collector, Murky Charm

-Shields have been expanded into a full sub-set of weapons with these new additions: Dank Wood Shield, Riot Shield

-Changes to the vanilla drowing mechanic: drowning for more than 5 seconds causes the damage to start increasing gradually


--Changed:

-Space Diver Armor visual overhaul!

-Cobalt Wraith has undergone some changes to try to make the fight more fair and less running away

-Finally fixed the Dank Shrines, there should now always be atleast one narrow entry point at highest top

-Terra Diving Gear has been moved later into progression

-Added Bone Throwing Knives to the list of items the Ninja Stash can summon on attacks

-Demonsteppers can be hidden on your player to disable the double jump

-Owning the Caliburn Compass in your inventory is now required to lesson the darkness of the Dark Sector

-Yoyo Gauntlet has been moved to post golem, it was honestly too strong for pre-plantera

-Reworked Shooting Star: Less Damage, less overall mana usage, speeds up as you hold it down for longer causing more accurate hits, scales with mana sickness now.

-Buffed Joyrider: Grants far more flight time per bullet damage, base flight time reduced, is no longer restricted to basic bullets

-Nerfed Dragon Commander: now does cause immunity frames to make cross-weapon damage stacking less viable

-Nerfed Tidal Charm: only gives half as much breath now

-Nerfed Lunar Slime Heart: no longer grants any listed defense (having buffs/debuffs still gives defense)

-Nerfed Gunsmith's Belt of Tools: had a hidden 25% reload bonus, is now 20% and mentioned

-Removed Moist Stone's slowing debuffs when mined, reverted to previous state

-Adjusted item prices


--Fixed:

-Lunar Slime Heart giving twice as much defense than what was listed

-Kou Sash using the wrong Jab-lin accessory in its crafting recipe

-Various issues relating to Dank Wood Doors

-Shooting the moist walls triggering the slowdown effect; it now only triggers if your holding a mining tool

-Serious Sam 4 line with Draken not saying what I inteded it to



Changelog:

3.455-(12/5/20)-Hot Fix #13:


--Changed:

-A Cosmic Fragment is now required in your inventory to buy the Dragon Commander

-Stygian Veins have been made more common and can very rarely spawn in Limbo now, but their spawning distance has been increased, Null Watchers spawn rates reduced

-Luminite Wraith's Targets no longer shoot any projectiles as giving it thought, it was causing cheap hits, I'll figure out another use for them later

-Luminite Wraith can no longer do contact damage when not trying to ram the player

-Reworked Captain America's Shield: blocks damage from a wider angle, Just Block grants striking moment, crit chance reduced to 15% (from 25%)

-Buffed Assassin and Elite Emblem: Assassin gives +1 minions, Elite gives +2

-Buffed Mister Creeper's Crowning Attire: Defense increased to 40 (from 35)

-Omega Sigil, Prismal Bar, and Portable Hive Resprite, Portable Hive has an equip sprite now (much appericated JellyBru!)


--Fixed:

-Not receiving the upgraded Caliburn compass after defeating Wrath of Caliburn

-Blazewyrm set bonus not triggering on 'True melee' projectile weapons like Yoyos and Spears

-Nightmare Mode Triggering events more than once on specific boss drops

-Bench God's Favor being granted by defeating Copper Wraith and reloading your character (whoops!)

-Stone Aura Staff hurting friend NPCs



3.440-(12/3/20)-Hot Fix #12:


--Changed:

-Strange Portal no longer tries to slowly fly away from enemies

-Moist Stone (Dank Shrine walls) can now be mined before beating murk as to help with the generation locking players out, but it takes longer and inflicts debuffs on you, it is still only explodable after Murk thou


--Fixed:

-Fixed trying to access sound instances server-side (IE you can hopefully summon Cirno now)

-Removed Dragon Commander's crafting recipe (it was not suppose to have one), also was missing the Hit Scan Item property for the Wraith Targeting Gamepad

-Dies Irae Stone missing from the Strange Portal's shop in multiplayer



3.420-(12/2/20)-Patch 8:

-This patch aims to add content and features that were missing from 3.400 and fix bugs, but wern't able to get into the mod before the planned release date


--Added:

-Dies Irae Stone

-'Just Block', a new mechanic with shields where blocking an attack at the last second negates it and grants the player 30 IFrames


--Changed:

-Draken has a few more 'forth wall breaking' lines

-Added a Dank Varient of Cratrosity that uses Dank crates in place of hallowed/evil ones - use a Dank key while opening a Terraria Co Supply Crate to summon it

-Limbo has gained a more menacing, less empty sky...

-Added some missing unused gores from 3.400

-Cirno now has a death sequence, lol

-Hellion now has something different to say after the fight if you choose to not diverge your privacy

-Sharkvern's body segments are no longer chasable

-Buffed Pocket Rocks: Damage increased by 1 (5)

-Buffed Big Bang: Damage increased, mana cost reduced

-Buffed Reality Shaper: Damage of projectiles increased, mana cost reduced

-Buffed Dragon Commander: The damage resist is no longer based on endurance, and is now exponential

-Nerfed Corroded Shield: Your hands remained tied the entire time during the dash (to make up for the added Just Block mechanic)


--Fixed:

-An oversight that Steamworks doesn't exist on the server end (the more you know! Also, I don't test this mod enough in netplay!)

-A bug that let you block damage by using a Granite/Cobalt Magnet like a shield

-The way shields handle blocking: This code was honestly trash and didn't work half the time, and has been upgraded into proper dot product code

-Cirno's summoning item being able to be used outside the Snow biome

-Cataclysmic Catalyst missing the Omega Sigil in its crafting recipe

-Dank Keys not stacking



3.400-(12/1/20)-Century's Millennium:

THANK YOU ALL FOR 100,000 DOWNLOADS! <3, it seriously means alot, you have no idea <3<3<3


--Added:

-BOOMerang, Coralrang, FridgeFlamarang, Wirang, Specterang, Fortune Falchion, Gnat-ades, Sludge Bomb, Laser Marker, Re-Router, Divine Shower Storm, Cracked Mirror, Red Phasebrand, Cyber Scythe, Shadeflame Staff, Volcanic Space Blaster, Crystal Comet, Thermal Jab-lin, Swamp Sovnya, Crimson Catastrophe, Dragon Commander, Gnat and Horsefly Staff, Sybarite Gem, Blazing Heart, Restoration Flower, Glacial Stone, Terra Diving Gear, Aversion Charm, Pocket Rocks, Granite Magnet, Cobalt Magnet, TPDCPU, Wraith Targeting Gamepad, Novus Summoning Orb, Novite Command Chip, Prismal Necklace, Enchanted Bubble, Universal Music Box (is infact part of IDGlib), Assembly Star, Bottled Mud, Stygian Star, Dank Key, Hopeful Heart, Spanner, Bench God's Favor

-New Potions: Energy, True Strike, Trigger Finger, Toxicity, Intimacy, Tooltime, Tinker

-Dark Sectors and Limbo! Defeating the 3 mech bosses awakens a darker presence in your world. Defeat the Wrath of Caliburn to upgrade your Caliburn Compess to help you. But watch out what treads below...

-The Reverse Engineering Station now has a 2nd use: uncrafting! But at a cost...

-Flasks for various weapon debuffs

-Added a new Town NPC: the Goat

-Proper trophies for all the bosses! For some bosses they always drop one on death.

-Luminite Wraith's entire resprite collection; about time!

-Resprites for the Amulets, updated Bosscheck list icons, proper sprites for Book of Jones, Revolving West, Shark Bait, Makeshift Spear Trap Gun, EA Logo, Corporate Epiphany, Several Buff icons, some other items, and many other sprite updates and resprites

-Music tracks for Sharkvern and Spider Queen, composed by Musicman, And the Caliburns composed by Rijam, and music boxes for all (except 1) of the custom themes!

-Added Obsidian Rose, Lava Charm, Shadow Key, Cobalt Shield, Flipper, Snowball Launcher, Sky Mill, and Frog Leg crafting recipes to the Reverse Engineering Table. All these and previous items now require an Assembly Star (sold by Draken)

-Added a Client Config option to force Hellion to respect your privacy and disable the extra effects of Apocalypticals

-Quite a few new lines for Draken, including a special sequence only during a largely vanity Terraria event...

-Added various missing QoL content, like banners for enemies, tiles for blocks, Gores, so on


--Changed:

-Duke Fishron has been added to the Expertise Rewards, Expertise cap is now 22000, Mimic and Goblin Summoner rewards have been halved, Pillars reduced to 200, added Big Mimics for 100 each (Coruption OR Crimson OR Dank, not all. And Hallow Mimic)

-Murk has been revamped: new attacks, damages adjusted, enters into phase 2 at 85% instead of 75%, moves faster when the player is further (making kiting him harder), and has more HP, speed, and spawns more slimes, and faster bombers in Hardmode. Murky Depths is no longer inflicted during the Hardmode fight and Dank slimes inflict Murky Depths instead of Poison on contact (this is less during the boss fight)

-Murk's flies/Killer Fly Swarm have increasing and decreasing speed caps depending on the distance to the player while charging

-Sharkvern has a new attack and readjusted stats and fight; Has a new drowning mechanic during the fight and before it; shark minions no longer shoot Sapphire Bolts, Sharkvern triggers an effect after the mech bosses til he's beaten akin to Cirno

-Cirno is way more aggressive and faster when chasing the player, but stops charging from a greater distance, this helps to avoid the circing cheese. Also changed the way she is summoned with more flair

-Attempting to spawn Cratrosity during the day tells you to try again at night

-Biomass has been rebranded as Photosyte, spawns less often in clouds, and has a new mining sound

-Action Cooldown Stacks now turn red when your stack count exceeds your max allowed stacks, and decay at only half their normal rate

-Prismic and Prismal ammo types now have a saving chance to not consume the cycled ammo types on use (33% and 25% respectively)

-Armor and mobility accessories have been overhauled (see fixes)

-The Entropy Transmuter is no longer sold by Draken (it has been moved to the Strange Portal's shop in multiplayer) and has been made craftable, Entrophite is now obtained in Limbo. 

-The Fiery Heart is now obtainable and reworked to be stronger

-Nightmare mode now visibly shows next on your character in the character selection screen

-Worldgen chest loot: loot chances increased for Decayed Moss+Photosyte/A Machete or Guerrilla Pistol only have a 18.75% chance to show up in non-Dank Shrine chests (75% in Dank Shrines), and no longer show up in other modded chests anymore in world generation. Dragons Might and Matrix Potions are less common.

-Dank Cores are a bit more common in Dank Shrines, and you can now find more than 1 in expert rarely

-Energizer Battery now requires Photosyte instead of Photosyte Bars (has been moved earlier)

-Forager's Blade has a new function, Merchant's price of Leather was greatly increased to account for this

-Massacre Prototype now costs 50 life to fire, and resets life regen on use, mana cost halved

-Removed the Cobalt Wraith defeat line, and changed Copper Wraith's to reflect that you can craft a furnace before fighting it

-Caliburn Guardians fade in when summoned

-One of Draken's lines now that Serious Sam 4 was released

-Dank Crates now give Bars and crafting materials seperately (you get more look now, also added Transmutation Powder to the drops)

-Some various things to Dank Shrine enemies, Mud mummies have quite the new functionality now, and now each of them usually drops a Dank Shrine material (Cores/Wood) after Murk as to not make fishing the only means of getting more

-Heralds of the horsemen no longer require both a Hardy Saddle and Slimy Saddle, but rather either to make them easier to craft.

-Cratrosity can no longer hit the player if they are not actively moving towards them (they can still hit the playing when flying side to side thou) to prevent cheat hits

-Cobalt Wraith's Swords are more vulnerable to knockback and their Waterbolt attack has an extra 100 frames of cooldown

-Jab-lins/Transmutation Powder are cheaper/can craft more of them

-All Thrower gloves are cheaper to craft and cost only leather now instead of tattered cloth, removed the Detonator from Throwable Boulder Trap's crafting recipe

-Wraith Core Fragments cost more in Mutant's shop

-Peacekeeper's Duster's town life regen now works during boss fights, fighting a boss in your NPC's town is a high risk factor anyways

-Avali Scythe and Glass Swords are cheaper to craft

-Removed the Arctic Diving Gear off Space Diver Helmet's recipe (it still gets the bonuses thou)

-Novus Core/Prismal Core/Tech Master's Gear's Increased Action Cooldown Limit is only increased once per these 3 accessories (wearing all 3 only gives you 1 free stack)

-Mana Battery no longer requires Advanced Plating

-Fridgeflame now has a new crafting recipe involving water and lava; it also gives 2 per craft now instead of 1 to make up for this

-Guerrilla Pistol is now a Revolver-type weapon

-Rebalanced Solis Nova: it has been moved to pre-Luminite Wraith

-Rebalanced Terra Trident: it has been moved to post all mech bosses, crit chance has been improved to make up for its tough crafting recipe

-Rebalanced most preharmode Jab-lins: damage increased, stick-in hits reduced

-Reworked Tidal Charm: it has been given a new game-changing property, check it out!

-Reworked Cosmic Grasp: Creates Shadow Tendrils instead of Shadow Beams, still requires a Shadowbeam Staff

-Reworked Mangrove Shiv: Is now a throwing weapon with a new mechanic

-Reworked Midas Touch: Larger Resprite, damage reduced to 70 (from 85), unique property doesn't function when under the effects of a Flask of Gold, old sprite repurposed into the Fortune Falchion

-Reworked Stormbreaker: Damage nerfed, alt fire damage increased, now consumes Electric Charge instead of mana

-Reworked Cirno's Wings: Magic Attacks only sometimes do Frostburn, the originally functionality belongs to Cold Damage attacks now, now has unlimited flight in the Snow Biome, but greatly reduced in the Jungle, Desert, and Underworld, also provides immunity to Snowfrosted

-Reworked Quasar Cannon: now properly uses mana, and has been added to the Former Havoc Mod item collection

-Reworked Manglove Armor: each piece now provides 1% Apocalyptical Chance, has less defense, the legging's speed was adjusted a little

-Reworked Celestial Flare: Slower swing speed, hits create moving explosions

-Reworked Thermal Pike: attacks now hit through half the enemy's defense, base damage lowered

-Reworked Lava Rocks Gun: now only shoots 1 large rock and bonus crit chance removed plus base damage lowered, but the splash damage is increased and inflicts a new debuff: Lava Burn. Which causes enemies to be treated like they're in lava for 5 seconds. Furthermore the larger rock does 3X damage on direct hit

-Reworked Flametrap 'Thrower': Shoots slower and flames spew at high the rate before, however flames hit more often

-Reworked XOP Flamethrower: The projectile hit delay has been reduced by 1 frame, damage lowered

-Reworked Portable Hive: No longer gives defense, but now gives 10% Summon damage, Honeyed buff is now splashed onto other nearby players 

-Reworked Shadeflare: Arrows no longer cause immunity frames, but hit only once per lifespan

-Reworked Hellion's Cascade: now uses Static immunity VS local immunity, meaning overlapping beams no longer do extra damage, this time has been reduced to 30 frames however, mana cost increased

-Nerfed Laser Lance: damage slightly lowered, hitbox largely increased due to the new sprite

-Nerfed Plythe: damage has been reduced to 110 (from 125), Knockback has been reduced to 0, stacks simliar to light disks to 5, and has gotten a visual overhaul, requires Light Disks instead of Avali Scythe

-Nerfed Maldal: reduced crit bonus heavily

-Nerfed Icicle Fall: Removed all bonus crit chance

-Nerfed Creeper's Explosive Throw, reduced damage and fixed it so 2 yoyos share the same static cooldown (like vanilla yoyos)

-Nerfed Spear/Dart trap guns: their knockback was WAY too high

-Nerfed Snowfall Cloud/Cursed Hail, snowballs/ice shards are now treated as summon damage (no more crits), and they now share the same summoning limits

-Nerfed Novite Helmet: reduced Electic Charge cost reduction to 20% (from 25%)

-Nerfed Wave Beam: Damage lowered quite a bit, crit chance increased by 5

-Nerfed Beam Gun: reduced damage to 80 and Requires 50% more Plasma to fire, Plasma Cells now only stack to 10

-Nerfed Mangrove Bow: less projectile speed

-Nerfed Primordial Skull: removed defense

-Nerfed Trap Accessory prefixes: less damage and armor pierce

-Nerfed Blink Tech Canister: Max Electric charge has been reduced to 1500 (from 2000)

-Nerfed Soul of Secrets: Bee damage has been greatly reduced

-Nerfed Galactic Inferno: Removed all bonus crit chance

-Nerfed Fiery Moon: Reduced Damage and increased mana cost

-Nerfed Assassin Emblem and Elite Emblem: Assassin Emblem gives less damage (10% instead of 15%), Elite (15% instead of 10%), also fixed a bug that gave WAY too much throwing damage boosts

-Buffed Novite Core: now gives 10% trap damage boost instead of 5%

-Buffed Fridge Flames Canister: Now grants bonus Electric Charge Recharge Rate depending on player conditions

-Buffed Tech Master's Gear: Now recives ALL the bonuses from Plasma Pack (really should have to begin with)

-Buffed Rioter's Glove: increased 1 damage and has been moved to hardmode; Ale, Bones, Molotovs, and Spiky Balls all gain more damage from it

-Buffed Tidal Wave/Contagion: Damage increased, Contagion's attacks hit more often, do not cause immunity frames, and knockback reduced,Tidal Wave's water ball now crits wet targets and doesn't expire when moving through water, Tidal Wave is now required to craft Contagion

-Buffed Blazewyrm Breastplate: defense increased by 2

-Buffed Acid Scepter: Does 2 more damage, the debuff it inflicts is much longer lasting than most sources of acid burn are (this weapon was extra hard to properly use, so I made it worth using)

-Buffed Glass Sword: Now ignores enemy defense

-Buffed Brimflame Harbinger: The explosions do more damage

-Buffed Gucci Gauntlet: has a 10% crit chance increase

-Buffed Yoyo Guantlet: Has the effects of the Glacial Stone and slight sprite edit

-Buffed Elemental Cascade: This weapon was just a bit too weak for post all 3 mechs

-Buffed Rubied Blade: more knockback

-Buffed True Moonlight: Gained 5 bonus crit, the level 3 slash wave stays out longer and does 4X damage now (VS 2.5X), Lv2 slash does 3X damage (VS 2.0X), also redid the visuals on True Moonlight's projectiles, enjoy fancy 1.4 style trails!

-Buffed Novite Knife: increased swing speed, projectile size, and now counts as True Melee

-Buffed Novite Blaster: shots are, infact, accurate now

-Buffed Novite Tesla Tower: damage increased

-Buffed Novite Armor set bonus: you gain more speed per charge

-Buffed Space Diver Armor: All pieces now give bonuses to Electric Charge

-Buffed Circuit Breaker Blade: increased damage, halved Electric Charge cost and delay, bolt damage slightly reduced

-Buffed Shark Bait: more fish are spawned when thrown in water

-Buffed Pearlwood Jablins: Stars now do 75% of base damage, and are effected by accessory scaling, and stabs faster and further

-Buffed Jaws: Teeth don't mess with immunity frames and do the weapon's full damage now, limited yoyo's lifespan

-Buffed Djinns Inferno: Hotter tentrils now spawn spirit flames on hit that do base damage, Hotter tentrils are more common, moved to post-any mechs

-Buffed Heat Wave: Reduced mana costs

-Buffed Calamity Rune: Grants 25% more Apocalyptical Strength (new total of 50%)

-Buffed Mangrove Staff (less Mana, more damage, has knockback, changed sounds)

-Stone Barrier Staff and Berserker Aura Staff no longer have a damage value as they don't need one, this means they can no longer receive a prefix. Also Stone Barrier Staff has been removed from the Dryad's shop

-Dart Trap 'gun' has been moved to pre-skeletron, the Super version now requires a Dart Pistol/Rifle to craft

-Braxsaw mining power increased to 250

-The new hardcap for NPC discount prices from prefixes is now 95%, before it was 90%

-Trap and Thrower prefixes now have only 1X chance of being picked (VS 3X chance from before)

-Idol Of Midas's buff conditions changed to player's direction instead of where in the world you were

-Novite and Novus Ore have a stacksize of 999, the bars a stack size of 99

-Novite Drill and Novus Pickaxe are no longer better than an Evil Prehardmode Pickaxe, novus pickaxe is now only slightly faster

-Fiery Shards drop more often, Sharks drop Sharkteeth after Sharkvern is defeated

-Strange Portal can no longer get hit

-Some visual/underlaying changes to the Lava Rocks Gun

-Virulent ore has a different map color to seperate it from Photosyte

-Replaced the High Test Fishing Line with White String in Twins of Fate's crafting recipe (what was I even thinking?)

-Removed Boss Fight Purity off Luminite Wraith

-Removed Treepeater's crafting recipe, as it drops from Dank Mimics in hardmode

-Removed being unable to open a Terraria Co Supply Crate if you had a Shadow Key/Temple Key in your inventory (could be a fix depending on how you look at it)

-Removed Dank Core drops from the Dank Mimic

-Readjusted Item Prices, updated alot of Crafting Recipes, some enemies drop more items, and Potions are cheaper to craft, and other various cosmetic changes


--Fixed:

-Entrophite Tranmuter not working online, also incorrect info on the Entropy Transmuter

-A serious oversight bug with Cosmic Cocktail projectiles being Imbued stacking up their damage to near infinite damage

-A bug involving the Revolving West and being able to use with other items while reloading

-Stone Barrier Staff and Berserker Aura Staff missing from Strange Portal's shop in Multiplayer

-Universal Throwing consumables able to have prefixes

-A bug involving Plasma weapons not recharging to full if the player had an item that increased the max plasma clipsize

-A bug involving the slot a Dragon's Strength or Matrix Potion generating in was incrementing the wrong way (and appearing in seemingly random slots)

-Hellion's Story dialog should now properly work online

-An issue relating to dashing that made it impossible to blink-dash if you didn't have a dash-related accessory or buff active

-Gucci Snap having a 100% chance to effect enemies

-Glass Swords: Shards being counted as magic damage and being uncraftable

-Dank Mimics, Strange Portal, and Tidal Elemental not showing on the Lifeform Analyzer

-The Energized and Trap-related accessory prefixes being unable to be rolled

-All Bows and Repeaters not receiving the speed bonus from the Archery Potion

-Missing aggro reduction from Tech Master's Gear

-Caliburn Guardians dealing damage if you were hit within a frame of them spawning (IE standing directly on the alter)

-Spider Queen not stretching out their legs when doing the spinning webbing attack

-Caliburn Shrines not dropping their swords in multiplayer

-Player's head being visable while wearing Space Diver Armor

-Doom Harbinger not scaling properly in expert mode (base HP lowered to 25000 to make up for this)

-Improper speed increases across all armors and accessories (thanks Qwerty3.14!)

-Mangrove Orbs not doing Magic damage from the Mangrove staff, Mangrove Staff and Mangrove Armor Set Bonus not being seperate projectiles (also Qwerty3.14)

-A bug involving Space Diver Armor and Beserk Aura Staff conflicting with the Breath Meter (may stuff confict with other mods thou)

-Fiery Shards dropping above hell

-Softlocking/drawing issue with the Non-Stationary Snowball Launcher

-An issue possible in Multiplayer where players could summon more than one Traveling merchant if players all used Eastern Gongs at once

-Avali Scythe not recieving bonuses from throwing stats when thrown

-An issue with Draken's "what's next" dialog option showing wrong results due to bad math (I swear I can hear 'Baldi's smacking')

-A bug involving going over the max Electric Charge if you were on a recharge delay

-FlameTrap 'Thrower' would hurt you because the flames it spawned were not friendly

-Jarate Shurikens only using 4 at a time instead of 5

-Peacekeeper's Duster not reducing damage while reloading

-An oversight that caused the player to get sent back to the overworld from a dimension if they survived a fatal blow, they now only get sent back if they die

-Removed unneeded leftover code off the Corrupted Tome that might have messed up the weapon (IE an accidental alt fire ability)

-Removed Obsidian Rose code from the Ragnarok Potion (this was unintended left over code)

-An oversight where the Cosmic Grasp wasn't creating explosions in the proper locations

-A few errors and missing things from the Prismal Ore tile

-An oversight on the Dank Crate that prevented Novus Ore from being obtained

-Photosyte/Prismal Bars not being able to be placed due to having no use style

-A bug relating to Nightmare Mode not dropping extra loot

-Contagion's Item sprite facing the wrong way

-Pickaxes and Drills having wrong balance (Virulent Pickaxe was so slow!, Novus Pickaxe was WAY too fast!), Virulent Drill having knockback (drills do not have knockback)

-Some debuffs that shouldn't be nurse "heal"able

-The Strange Portal 'turning' to face the player

-Spelling mistakes

-Tiding up code, making better use of Radians, fixed netcode for a few weapons, and various other smaller bug fixes



3.350-(8/29/20)-Hellion Revert And update:


--Changed:

-Reverted Hellion back to their state before July 29th, removed Cirno from the fight, added extra army stuff


3.325-(8/22/20)-Hotfix #11:


--Added:

-Novus Core, Novite Core, Prismal Core, Quasar Cannon

-Proper Sprites and resprites for a few things


--Changed:

-Tech Master's Gear now requires the Prismal Core instead of the Night Vision Helmet, has lost 2 point of booster recharge to make up for the new addition

-Plasma Pack's Booster bonus has been increased to 3 (from 2)

-Massacre Prototype requires the Quasar Cannon


--Fixed:

-Potions not being counted as a 'potion', minor, but worth noting

-Online play breaking, hopefully for real this time



3.300-(7/28/20)-Mid Update/Hellion Fix Patch:


--Added:

-Clarity Potion

-A proper personality for Hellion

-Placeholder warnings


--Changed:

-Hellion's fight, made it better

-Greatly increased the duration of most potions (to atleast 5 minutes)

-Gas Passer nerfed to account for the ability to spam throw it now, Jarate and Gas Passer made cheaper to craft

-Altered the visuals a bit on the Berserker Aura Staff


--Fixed

-Hellion



3.200-(7/25/20)-End of the Beginning pt3:


--Added:

-Eastern Gong, Laser Lance, Berserker Aura Staff

-New Dialog options for Draken! No more is there a "button 2"

-Fargo's Mutant Mod Support


--Changed:

-Portal now gets in on the party with a proper party getup

-Acidic Skull and Radiation Cure both now add resistance against the health lose from the Radiation debuffs (This does not include Limbo Fading however)

-Entrophite Tranmuter has been moved to Draken's shop (4000 Expertise to unlock, 750 to purchase)

-More overhauls and changed moves to Hellion

-Nightmare's Expertise bonus has been increased to 25% from 15%

-Dank Wood Armor has been buffed a bit to compensate for the effort required to get it

-Infact added the Stone Barrier Staff to be obtainable, check the Deeper Dungeons

-Removed Mana Battery from Condensing Potion recipe; it really didn't make much sense

-Removed bonus action cooldown stack off Novite Helmet, and replaced with a energy saving buff

-Greatly reduced item values for Cratrosity's drops, and the boss's own gold dropped down to 10 gold (from 45)

-If the Traveling Merchant is selling Soldier's Rocket Launcher or the player owns one, he now also sells Rocket MK1's

-Energizer Battery's restore percent decreased to 20%


--Fixed

-Hopefully, fixed the unkillable Supreme Pinky Glitch

-Throwing Gloves not accounting for Throwing Item Saving Chances

-The broken tools bug that happened when you died in the Deeper Dungeons and couldn't use your tools without restarting your game

-Energizer Battery stacking infinity

-Kou Sash using the wrong Jab-Lin accessory

-Luminite Wraith's debug text showing up

-Caliburn's having no sell value and being practically free to reforge

-Reality's Sunder not craftable without beating Hellion's 1st phase if you've already talked to Draken on that character

-True Copper Wraith Notch dropping 24/7 off Copper Wraith if you got it once, now only drops once per world

-Twin Prime Destroyers listing incorrect info in Boss Checklist's book



3.177-(7/17/20)-Hot Fix #10 Mania:
--WIll be updating Hotfix #10 as I make more smaller changes post-3.100 release

--Added:

-Added Bombs to all chest types in the Deeper Dungeon: a 50% chance to have 5-10 bombs (25% to be sticky bombs), added a hint that they can break the spike walls!

-More Deeper Dungeon Varity hall types-Cobwebs,Frozen Halls, and the occasional traps!

-Glass Swords, Kou Sash


--Changed:

-Buffed Big Bang: Auto swing and +5 crit chance for melee

-Novite Armor has less acceleration per charge and can now be toggled all together with WalkMode

-Tech Master's Gear now requires the Night Vision Helmet instead on Ninja Gear

-Charms have been renamed to Amulets to seperate them from other accessories that bare the name

-Can you summon Hellion's Main fight and skip Hellion core now, Reality's Sunder says how (also updated the text effect to be able to be read lol)

-Dying in a subworld now sends you back to the overworld

-Strange Portal now sells the loot found in the Subworlds for Expertise in Multiplayer

-Greatly Increased Shinobi's Shadow cost

-Reduced Supreme Pinky minion count, but increased HP, removed falling slimes in the final phase, reworked attacked to be easier to dodge and more predictable

-Hellion: Drone HP reduced, reworked the army phases to no longer disable your accessories, but gives Limbo Fading and other debuffs that reduce defense and attack, during said army phases you now have Snowfrosted, Hellion no longer regens HP from taunting (unless during army phases), Getting hit no longer gives Radiation 1, Changed the Brain Scrabler bolts to something that doesn't cause confusion, greatly increased the number of Drakenite Bars that Hellion drops, Removed anti-cheese
-Yet More Hellion reworks... Cirno assists during the goblin army phase and basic (non summoner) goblin count doubled, Cratogeddon's special crates assist during the Pirate/Frost Legion phase, Topaz attack damage reduced, Drones do not deal any contact damage
-Hellion Core: attempted to telegraph charge attacks with speed depending on distance, reduced laser firerates, changed projectile types, removed extra spam attacks

-Cratogeddon: increased drops, reduced attack power, reduced/capped movement speed, slower attacks, bullet hell phase is easier and the easily dodged bullets gained damage, all other attacks have reduced damage, reduced Cratogeddon special crate projectile damage

-Cratrosity Reduced base attack power, charge attack has capped movement speed

-All other Javelins items have been renamed to Jab-lin

-Ice Fairy spawn rate reduced and loot increased

-Electric discharges now have a size depending on how many times they can arc

-Buffed Blazewyrm Armor

-Forgot to remove Magic Cuffs completely off Elite Emblem, whoops!


--Fixed

-Anticipation Charm softlocking players while they were holding no items in their hand

-Sharkvern using Cirno in the Boss Checklist book

-A bug that prevented online play all together

-Cirno's Attacks and Icicle fall not working anymore

-Hopefully fixed the crash on world load bug

-Novite Tesla Tower targeting 'nothing' and consuming electricity, hopefully anyways

-Gunslinger of song and legend's ability causing damage problems

-Boss Spawns no longer working in multiplayer

-Circuit Breaker Blade using Electric Charge without spawning an electric discharge

-Novite and Novus both generating in a world due to a coding oversight
-^fixed the above, again :/



3.100-(7/8/20)-End of the Beginning pt2:

--Added

-Gucci Gauntlet, Soldier's Rocket Launcher, Acid Rocket, Transmutation Powder, Condenser's Potion, Ragnarok's Brew, Energizer Battery, Consumable Hell, Portal Essence, Terra Trident, Solis Nova, Stone Barrier Staff, True Wraith Notch, Ring of Respite, Necklace O' Nerve, Shinobi's Shadow, Plasma Pack, Gun Barrel Parts, Second Cylinder, Peacekeeper's Duster, Sparing Spurs, High Noon, Dualing Deity Shades, Gunslinger of Song and Legend, Ninja Sash, Shin Sash

-Novite Ore, Bars, and Gear!

-Copper and Cobalt Wraith themes composed by 'musicman'!

-All Book entries for boss checklist with some placeholder graphics for now

-The Deeper Dungeons! There's a strange portal in the dungeon... (Still a bit of a WIP, find unique loot, how deep can you go?)

-A complete rework for Action Cooldowns and all items that use them: they are now stackable!

-Electric Charges, heavy WIP, but some items are changed to use this new system +1 new prefix to match.


--Changed:

-All throwing damage in the mod has been changed to a modded type of throwing to prepare for 1.4 TMODLoader, until then thou it should work alongside... most throwing gear until 1.4 releases

-Luminite Wraith has been reworked visually, and gameplay wise a bit as well

-IDG's Starter Bags are now a 'possible' unlock for beating a challenge

-Overhauled the way Charms work, they are now equipped in your Minecart slot, and have downsides attached to them so they are not free accessories

-Caliburn Guardian has been moved to post EoC and the boulder attack has been given a delay

-Added a DPS cap to Hellion, exceeding 50K DPS will make your attacks only do 1 damage

-Popular Modded damage types (Calamity and Thorium's) are accounted for in items that boost class-wide general damage

-Most armors have been reworked to the new Stacking Cooldown system

-Dragon's Might and Matrix Potions are now far rarer in generated chests

-Caliburns should now drop their swords locally on each client

-Tier 3 Charms are now expert only due to requiring a Mecha-Minecart, Enhancing Charms have been greatly buffed

-Handling Gloves have been added to the Tech Master's Gear, Tech Master's Gear no longer has its own trap damage boosts as it now inherits it from the Handling Gloves

-Boreic Hamaxe has been reworked to make up for its smaller sprite

-Acid Rounds renamed to Acid Bullet

-Nerfed Gatlipiller

-Electricc Bolts now arc around rather than having perfect accuracy

-Nerfed Serpent's Redemption of all bonus crit chance

-Reduced Sharkvern's body DR to 50% from 75% and only for piercing weapons, reduced body defense from 50 to 25

-Limited the number of Stardust Cells that spawn during Luminite Wraith's fight

-Oynx Tactical Shotgun and Tactical SMG Rifle slightly nerfed

-Removed the barrier walls from Cratrogeddon

-Tin Wraith has been removed and Tin Wraith Shards are unused for the time being, if he returns, he will return in a different light

-Removed the 2nd set of () off custom angler fish

-Removed the TF2 Questline and all code related to it, this was very old, poorly coded, unfinished, and cringy. Contracker is still in the mod however.

-Removed Magic Cuffs from Elite Emblem

-Replaced DD2 with Old One's Army wherever it is displayed

-Removed a Webmillio from the github

-General Quailty of Life changes like Boss Heads and better cross mod support, and of course resprites


--Fixed:

-The Hellion unlock message has been fixed, hopefully, finally, for good this time! Not this time recipe browser! (Thank god if so! MAJOR props to Ekyo for helping me figure this one out!)

-Hopefully Hellion going non-existant mid fight

-Hellion Beam textures being created at all times and better resource managment, Also redid all uses of ModContent.GetTexture("Terraria/) being called every frame, this should hopefully not crash TML64bit within a few minutes now (Thanks Oli.H!)

-Paradox Mirrors and Hellion's circle arena should no longer draw outside your screen, improving FPS

-Hotbar and Item Curse should now properly block item continuous use like Last Prism

-Tier 3 Dark Mage and Orge not awarding any Expertise

-The tiles under the Caliburn Alters being minable when they shouldn't have been

-Dank Shrines being unminable til after hardmode Murk was beaten, and a mistake relating to murk's defeat code, whoops!

-Murk's bag causing slowdowns when opening due to an unneeded for loop, And fixed murk not being marked as "beaten" properly

-Tech Master's Gear not granting both bonuses as listed

-Dank Mimic being called Swamp Mimics

-Mangrove Hammer not being part of the Mangrove set

-Herald of War of boosting the wrong damage type

-Snowfrosted now tells you to fight Cirno when outside the boss fight

-Ice Faries spawning during the night, underground, and their gold dropped being too much

-Murk/Killer Fly Swarm should hopefully spawn normally now in Multiplayer

-Fixed Acid Scepter shots acting weird in water

-Fixed Typos

-Various 'behind the curtain' improvement tweaks to code, mod structure and bug fixes



3.099-(5/23/20)-Hot Fix #9:

This is the last update before the next major update, thank you ALL who gave my mod a try with the release of terraria 1.4, you have no idea how much it means to me <3


--Fixed:

-Virulent Ore spreading to Demonite And Crimtane (this was left over code from BlueMagic, whoops!)

-removed the tooltip of jab-lins being consumable in melee from non-consumable jab-lins (IE the Sanguine Bident)

-Spelling, rewording, and grammer mistakes abound (Thanks Gpax971!)


3.098-(5/22/20)-Hot Fix #8:


--Changed:

-Life required to fight Caliburns lowered to 200 (from 220)

--Fixed:

-Spider Queen not dropping anything in Mutltiplayer

-Caliburn alters not telling you that you need more HP in multiplayer games if you have less than 200 life


3.097-(5/21/20)-Hot Fix #7:

--Changed:

-Fridgeflame Canister's Frostflames now deal IFrames unique to the projectile type, this means that no matter how many you spew, the Frostflames can only hit once every 10 frames. Fridgeflame Canister has been buffed to make up for this (75% of base damage instead of 50%)

-Rebalanced all boss coin drops (Cirno will no longer drop nearly 2 plat)


--Fixed:

-Cobalt Wraith's swords doing too much damage

-Copper and Cobalt Wraith dropping the wrong ore types

-Oversight with Wrath of Caliburn that didn't cause it to scale properly


Changelog:

3.096-(5/20/20)-Hot Fix #6:

--Added:

-Wrath of Caliburn, a hardmode varient that can be used to farm swords, so you can get them in worlds where say: the worldgen messed up.

-a few more "book" entries for Boss Checklist


--Fixed

-True Moonlight online (I think...)

-Found and fixed bad net data (Caliburn compass works now)

-The Caliburn generation code hopefully, your sword shrines won't get messed up now by chests

-Replaced recipe groups of Fragments with the vanilla ones, basically means SGAmod items can support properly modded in Fragments


3.090-(5/18/20)-Hot Fix #5:

--Added:

-Alot more missing item tooltips (when you don't own an item and get a hint on how to get it)

--Changes

-buffed Tidal Charm significantly

--Fixed

-Tidal Charm properly works now

-jab-lin and Revolving West code

-Redid Caliburn hitframes code (to make sure there really are no more Iframes)

-Gas Passer requiring the Cursed Flames book (Relogic, come on! Naming convention!)


3.3050-(5/16/20)-Hot Fix #4:


-Added:

-a handful of wonderful resprites for Midas Touch, Machete, Jarate, Sword of the Blue Moon, and True Moonlight

--Changed:

-True Moonlight's new sprite has a new visual effect

--Fixed

-True Moonlight Lv3 waves not homing through walls



3.000-(5/16/20)-End of the Beginning Pt1;

-Added:

-Twines of Fate, Tidal Charm,Icicle Fall, Shark Food, Drakenite Bars (they are dropped by Hellion, for now, some items require them now), Heart Guard (Crimson alt of the JuryRigged Spiked Buckler), Anomaly Study Paper, Joyrider

-Truely Suspicious Looking Eye-manually Summon Doom Harbinger and also refight him if you wish

-2 Types of fish quests for the Dank Shrine Zone

-The rest of Havoc's Swamp (now Dank) enemies, most of them spawn in the Dank Shrine

-Resprites for all of Spider Queen's sprites and a massive visual overhaul +added treasure bag, phil would be proud


--Changed:

-Mister Creeper's dev items have been renamed to Legacy Dev items

-Slight buff to Portal Bullets

-Renamed the Crates of Light and Night during Cratrogeddon's fight

-Sepent's Redemption rebalanced a bit (removed bonus crit chance and damage reduced, empowerment bonus made slightly better)

-Nerfed the SBC Cannon's base damage greatly, made cheaper

-Nerfed the SBC Cannon MK2's base damage

-IDG's Armor set slightly altered

-Hellion can no longer use both Laser Reign and Healthy Dose of Lasers at once when in Tyrant Mode

-Large rework to Luminite Wraith: added in missing attacks, Vortex Lightning Reworked, Vortex move has been completely reworked, and Heatwave burst reworked as well, the boss stops moving when doing any of the last 2 and warning effects made more noticable

-Most of Cirno's Ice Bolts have been made into a custom projectile that doesn't use Dust to tell you where it is (no more invisible projectiles)

-Buffed Ice Scepter and reworked

-Big Bang and Reality Shaper have gained the new Ice Scepter Bolt projectile and have been rebalanced around it, Reality Shaper nerfed a bit

-Added another attack to Sharkvern so he's not doing nothing for, half the fight. Flying shark HP buffed

-Hellion's Skeletron Hands also now lock your item type, not just the hotbar as well

-Fixed Cursed hail needing Cursed Flames Book instead of material (ffs relogic lol, naming convention plz)

-Blazewyrm bonuses nerfed, text reworked a bit to reduce reading

-Fixed more item prices

-Made several items cheaper to craft

-True Caliburn's damage reduced, and price fixed

-Mud Absorber can not longer stack with any accessories crafted from it

-Hellion's drones no longer shoot at you during attacks where Hellion becames unhittable

-Mudmore's Mud blobs inflict Oiled when they explode, but damage has been slightly reduced

-Throwing gloves no longer give a damage boost when using Beenades

-Widened Arc of Golem Fireballs and reduced rate of Fireball rings, added new attacks, add telegraphing, reduced damage of Blazing Wheels further

-Buffed Rubied Blade

-Heartreach Magnet crafting recipe changed, and alt added for corruption worlds

-Supreme Pinky's sky effect has been altered to be layered and also less jarring on the eyes


--Fixed

-Tidal Elementals not attacking or doing anything during night

-Mismatched descriptions on Non-Stationary weapons

-Big Dakka consuming ammo that shouldn't be consumable

-Fixed more item prices

-More fixes I couldn't remember to list



2.910-(4/30/20)-Hot Fix #3:
Previous version was suppose to be 2.800, but ended up being 2.900, oh well


--Changed:

-MVM Upgrade has been Added to Elite Emblem's abilities

-IDG's Starting Bag gives you loot differently: you will now always get atleast 1-2 of 4 potions, and won't get more than one mining helmet, Added some extra loot, and added the starter bag to Draken's shop


--Fixed

-IDG's armor set not dropping from treasure bags

-Captain America's Shield displaying some bad info and having a bugged crafting recipe



2.800-(4/30/20)-Patch 7:

--Added

-Revolving West (placeholder sprite), Captain America's Shield, Jarate Shurikens, Sanguine Bident, MVM Upgrade-Gripping Gloves-Handling Gloves and Hellion's Cascade (using place holder sprites), IDGCaptainRussia94's Dev armor (about time!), Hellion's Crown (Vanity Mask/Crafting Material, thing?), Thrower's Pouch

-Non-Stationary Defenses, enjoy carrying around Snowball launchers, Cannons, and Bunny Cannons!

-Pirate sells Explosive Bunnies if you have a party girl

-Limited boosting mechanic, you can now longer boost indefinitely; some items have been changed to account towards increasing this limit

-Merchant sells Leather in a Crimson world

-A Starter bag item to help quick start your adventure!


--Changed:

-The way the copper wraith is summoned, you can now make a furnace and craft bars freely but... make too many, and you might have an angry wraith on your case

-Jab-lin hitboxes are now based on the sprite size

-Added another crafting recipe for Grenadier's Glove, and made it cheaper to craft

-Adjusted the prices of alot of crafting materials

-Jarate has been made cheaper to craft and no longer requires bowls

-Nerfed Portable Hive/Soul Of Secrets Bee Damage, you could kill bosses with just the Bee gun alone faster than anything else! (Is now soft capped at 10X damage bonus of the normal bee damage)

-Beam Cannon has been moved to post Luminite Wraith, beam Cannon Alt Fire Spread and mana costs reduced, primary consumes small amounts of plasma. Attempting to not out-class the Lunar Cascade

-Buffed Lunar Cascade

-Buffed SpaceDiver Armor (I think)-they now receive bonuses to booster capacity and recharge rate

-Buffed Cosmic Grasp: Mana cost greatly reduced and crit chance slightly upped

-Buffed Acid and Thermal Grenades-they have a wider blast area

-Hellion Changes:

-Removed all defense off the DPS drones, HP buffed slightly, Healing drones HP buffed a good chunk

-Removed all defense off Hellion

-Nerfed Hellion Core's 2nd phase, charges less often, damage of eye lasers reduced

-Buffed Surt/Brimflame Harbinger, the amount of time you get OnFire! for has been greatly increased

-Buffed Hellion Core's 3rd phase, if the player is close enough to an arm when it shoots a hellion beam, it now also summons a portal to attack the player

-Hellion stops moving when doing the topaz attack during phase 4, Hellion stays put longer after doing her laser spread

-Nerfed Golem Buff, he fires the trio of cultist fireballs far less often and blaze wheels hurt less

-Removed damage code from DPS Drones

-Dank Crates will be a bit more generous with their loot

-Nerfed Aimbot Bullet damage lowered. Portal Bullets slightly Nerfed

-Nerfed Serpent's Redemption

-Buffed Both SBC Cannnons, since they can't crit their main damage, they now instead gain additional damage based on your ranged crit chance

-Buffed Rubied Blade, swing speed and crit chance up

-F.S.R.G has been nerfed, again, damage lowered to 70, and Spore cloud damage lowered to 2X the projectile damage (from 3X)

-Nerfed Big Dakka

-Nerfed Mangrove Armor and limited Mangrove projectiles

-Nerfed Jarockets, they inflict Sodden for less time


--Fixed:

-Old One's Army causing errors when you kill them

-UIScaling issues, I wasn't even aware UI scaling was something people used (but when you use 4K, it makes far more sense!)

-Beenades no longer be thrown at a faster rate via buffs or the gloves, it was possible to exceed the max projectile limit with this

-Acid Grenade, Thermal Grenade, and Celestial Cocktail receiving velocity bonuses they should not have due to an oversight in how player.throwingVelocity works

-Shortened more tooltips to be readable

-Powerjack does 'finally' infact, heal on kill, sorry it took so long to fully add this feature in

-Maybe now I fixed the Hellion message pop up, please? (nope.avi, sigh)



2.700-(4/20/2020)-Patch 6:

--Added

-Cataclysmic Catalyst, Golden Cog, Jindosh Buckler, Book of Jones (is using landslide sprite atm)

-Expertise rewards to the Lunar Pillars

-EALogo Got a new passive ability

-Walk Mode: a Hotkey activated mode that disabled the speed boosts of Demonsteppers&Luminary Wings (To help with moving around uncontrollably)


--Changed:

-Removed F.S.R.G from Technological weapons, damage slightly buffed, spores can now crit

-Changed the way the wraiths gate progession, you now need their respective shards to craft their items (except Luminite Bars which are still just locked), however if you try to craft anything at a furnace before beating Copper Wraith, they spawn in and attack you

-updated glowmask codes yet again to add more features and fix bugs (Emnity changes rainbow colors!)

-Hellion DPS drones have far less HP, but now instead have 120 defense

-Cosmic Grasp Mana costs reduced

-Beam Cannon damage slightly nerfed

-Novus Ore is now destructable by bombs after Copper Wraith is beaten

-Fridgeflame and Blink Tech Canisters now boost Technological Damage by 5% each

-Quagmire rebalance-reduced damage, now inflicts Dank Slow instead of Poison

-Jab-lins now display their crit chance and damage properly, melee attacks have a solid 50% to not be consumed, and both modes are boosted further by throwing item saving chances

-Caliburn Altar code has been changed yet again to try to prevent the alters from getting messed up

-Spider Queen has a delay now before they start spinning acid in their main attack (this is cued with a sound effect as well)

-Copper Wraith's speed has been decreased in Expert and Normal (same in Nightmare), Cobalt Wraith's speed has been increased in all difficulties

-In mutliplayer, players need to be close to a slain expertise-giving enemy to recive the expertise

-Crafting Recipes


--Fixed:

-Entrophite Tranmuter working online hopefully

-Hellion and Craterogeddon now properly reset when loading into a new world (they were not properly reset in the mod, whoops)

-Murk and Cobalt Wraith have been made far less likely to randomly despawn from when you get too far from them

-Sharkvern no longer uses water as a means to tell how high he should be, and should no longer be just a head online

-Hopefully fixed Luminite Wraith online

-Tech Master's Gear was gaining too much trap damage



2.650-(4/15/2020)-Hot Fix #2:


--Added

-Draken now tells you what is unlocked next in his shop and how much Total Expertise you need for it

-Corrorded Shield resprite

-Tooltips for the Furnace, Hardmode anvil, and Luminite Bars that hint at them being uncraftable for the time being


--Changed:

-Buffed Brimflame Harbinger: flames have more reach

-Buffed Cosmic Grasp: can hit one more target (6 targets) and damage and crit up, Cosmillash nerfed (direct hit does 1.5X damage)

-Merky Depths debuff moved to be expert exclusive, and damage of killer fly swarm in hardmode Murk's fight in normal mode reduced

-Trap Weapons balanced a bit more, some buffed

-Many items made cheaper to craft (largely the trap stuff)


--Fixed:

-An oversight that made Sharkvern fly upwards and despawn half way through the fight

-Cosmillash only giving +2 damage instead of 2X damage on direct hits

-More spelling mistakes

-Rioter's Glove not mentioning it can throw spiky balls

-'Throwable' Trap SpikyBall throwing Avarice coins when used by the Rioter's Glove



2.600-(4/14/20)-Patch #5:


--Added

-Radiation Cure Potion, Beam Cannon, Heralds of the 4 Horsemen

-Copper Wraith supports the full Boss Checklist

-More lines to hint at what the player needs to do next (Talk to the guide)

-Killing Moonlord gives hints to what is happening in the underground Hallow and what is to come for the world


--Changed:

-Nerfed many of Luminite Wraith's attacks, reduced attack rate and increased attack speed of Solar Flare Axe

-Beam gun damage nerfed and made cheaper to craft

-Buffed Cosmillash, the projectiles do 2X contact damage (since explosions aren't affected by this, this won't be a super duper buff)

-Most weapons from the mod should now cloak along with the player

-Touching Hellion Core's Arms inflicts a new debuff

-Reality Shaper's crafting recipe has a new item, crit increased by 5%, and mana costs reduced

-Added Spiky Balls (Both Types) to the Rioter's Glove of throwable ammos

-Novus and Notchus Arrows are cheaper to craft

-Tech Master's Gear boosts Trap Armor Penetration now as well

-Blaze Bullets nerfed slighty

-Massacre Prototype's mana costs have been increased again to 200

-F.S.R.G Has been changed to be a post moonlord and Technological gun: stronger damage, more debuffs and spawns spores!

-Big dakka's explosion damage and base damage has been reduced a bit, weapon has been moved to be Post Cratogeddon

-Creeper's Explosion Throw's crit chance increased by 10%, explosion damage halved

-Emnity's main projectiles have been changed cosmetically, we need more RAINBOWS!

-Healing Wraiths heal Hellion Core far less, and they can now heal Hellion more

-Hellion Core Arm contract damage reduced to 100 (from 200), damage of all subphase 1 and 2 attacks reduced

-Hellion is now Immune to all forms of Time Slowing


--Fixed:

-Changed the mod for the Caliburn Compass to better support lower-res games

-Crafting recipe for the Reverse Engineering Station

-Dank Wood Armor/Photosynthesizer giving you a free 10% damage increase to everything

-Fixed the Expertise and several issues with netplay

-Tidal Elemental dropping 2K Platinum (lol)

-Lack of support for where files get written on Linux and Mac (Thanks Kyoto!)

-Dank Walls can no longer be destroyed by bombs (before Murk is beaten)

-Blaze Bullets being free to craft

-Further tweaks to the glowmasks

-Cirno using an expert-only attack during her 2nd phase in normal mode

-Sharkvern despawning when he flys too far out of bounds and during his 2nd phase

-Hellion's defeat event triggering when the mod is first installed

-Improved the Shrine Generation code, this was largely placeholder code I forgot to replace. If it still generates in the temple/dungeon, I will shoot myself through the foot

-Improved the armor glowmask code a bit more, related to netcode and stealth

-Due to serrated tooth being combined into the Soul of Secrets, I have nerfed the damage over time bleeding

-Weapons being able to cut glass and break pots for no reason

-More item descriptions being too long on lower-res games

-Alot of netcode related stuff, and to avoid common errors during normal gameplay, however, there's still some issues that I cannot really fix, and summoning Cratrosity/Caliburns might be broken now, my system is "rejecting" net messages for some reason so I cannot test this

-Furthermore, alot of things I know may not work online, but online functionality is seriously frustrating, sometimes it works and then it doesn't, I'll work on it a bit at a time but don't be surprised if your favorite item doesn't work

-Various other Bugs



2.500-(4/6/20)-Patch #4:

--Added

-Cosmic Grasp, Photosynthesizer

-More glow masks, glow masks for armor (Blazewyrm, Space Diver)

-Empowerment bonuses for Dev items and armors

-Frozen Turtle Shell and Water Walking Boots to craftable items


--Changed:

-Blink Tech Gear renamed to Tech Master's Gear, and added alot of features and crafting items required

-Novus Bars are cheaper (you now make 2)

-Hellion Cosmetic changes to some attacks

-Cosmillash has faster projectiles

-Havoc's Fragmented Remains has been added to Soul of Secrets

-Crafting Recipes!


--Fixed:

-Terraria Co Supply Crates not dropping at all after activating the contracker

-Fixed a major bug involving shaders that stopped network games (I hope)

-Cratrosity and Cratrogeddon no longer despawn, and Murk is less likely to despawn during the fly swarm phases

-Doom Harbinger's death not counting, also him dropping items when he shouldn't

-Items that shouldn't cause melee hits (the yoyos largely)

-Creeper's explosive throw doesn't block immunity frames now for the explosions

-Hellion's repair drones sticking to target dummies (and also not being called repair wraiths)

-Removed lines from Draken that refer to there being no discord server.

-Hellion being below some harder bosses on boss checklist

-Even more items with broken prices



2.40-(4/1/2020)-Hellion's Wrath:

--Added

-Helen 'Hellion' Weygold, the 'final' boss of the mod. And a small story of sorts leading up to it

-Cratogeddon, a post-moonlord boss and varient of Cratrosity

-Nightmare mode! Beat the final boss on expert first to unlock them! (This feature is still rather heavy WIP, Despite sounding really generic, bonus points to whoever can guess the reference :P)

-Frigid Shard, Entrophite, Raw Avarice, Mana Battery, Soul of Byte, Omni Soul, and Fridgeflame crafting materials, Calamity Rune,Entrophite Transmuter, Heart of Entropy, Fridgeframe Concoction, Rigged Jackpot, Fridgeflame Canister, Blink Tech Canister, Blink Tech Gear, Big Dakka, Brimeflame Harbinger, Rioter's Glove, Demolitionist's Glove, Lance a-lot, Yoyo Gauntlet, Rod of Enforcement, Corrupted Tome, Charms of Anticiptation, EALogo, Corperate Epiphany

-A bunch of items and weapons related to Hellion which I will not spoil :p

-Apocalypticals, IE Critical Crits!

-Some new prefixes

-Proper sprites for many items and even some resprites!

-A Discord Server is back! See the home page for a link!

-However a few Items are using the same/placeholder sprites and even features that are disabled because they are either incomplete or I lack the sprites needed to fully add them in


--Changed:

-Idol of Midas's coin feature changed slightly

-SBC cannons reworked to be more like the source material: buffed damage, less gravity on cannonballs and explode always against knockback immune enemies, and cannonballs have a new visual effect

-Adjusted prices of alot of items, again

-Draken now sells Empty Charms sooner with less total Expertise, likely after the first boss

-You can no longer craft anything at a furnace (or hellforce) or a mythril anvil until you defeat the respective wraiths

-Stormbreaker's mana regen usage lasts the whole animation

-Cobalt Wraith's ore drops reworked, also now drops hellstone

-Worldgen chest loot changed a bit

-Grenadier's Glove has had its functionality split, the Molotovs are now part of the upgraded form: the Rioter's Glove

-Javelins are now called Jab-lins, and have a new set of sprites! Also prismal Launcher got a new sprite.

-Luminary Wings are now post-SPinky (And also no longer require Frostspark boots)

-Omega Sigil reworked

-Avali Scythe and Wave Beam have been made into Technical Weapons

-Nerfed Caliburn Guardians' damage, attempts to stay closer to the player so melee can hit them, and can no longer hurt the player by touch while not charging at them

-Luminite Wraith Buffed: First fight has more HP, 2nd fight has more contact damage and slightly more defense

-Tooltip on Dank Arrows has been clarified a bit, the slowing effect is stronger, not the enemies

-Removed Obsidian Skull from Demon Steppers as Lava Waders already uses it

-Buffed Demon Steppers quite a bit, Luminary Wings got a buff as well

-Big Bang's crafting recipe changed again, and buffed

-Recipes changed, such as Surt not longing being required to make Reality Shaper

-Nerfed Lunar Cascade, Serpent's Redemption, Massacre prototype, Prismal Launcher

-Lunar Slime Heart not longer deletes projectiles, but rather cuts out most of the their damage

-Treepeater firerate and accuracy increased

-Cosmic Cocktail's Stats changed, again

-Djinn's Inferno now requires a shadowflame hex doll

-Surt got a rework: a buff to help it synergize with Primordial Skull better and the projectile is faster, and nerf that OnFire is based on how long you charge the sword

-Blazewyrm Armor has been reworked, I want to think of it as more of a beserker type armor VS tank armor that is a direct upgrade to molten armor

-Space Diver Armor has been reworked, again. The damage penality on the set has been upped from 15% to 40%, however, you receive a bonus 25% to tech damage weapons.

-Swamp Slimes renamed to Dank Slimes, they can drop Biomass, and they may spawn after Murk is defeated in the Jungle

-Bronze Wraith Core Fragments are less grindy to craft as now you make 2

-Ice-related and alot of other crafting recipes changed, many items made cheaper to craft (especially the charms)

-Demon Steppers now includes the effects of the Blue Horseshoe Balloon and Frog Leg in the crafting recipe

-Supreme Pinky drops less Royal Lunar Gel

-World Gen involving biomass generation

-MANY other smaller changes


--Fixed:

-Some missing "missing item" subtexts that led to confusion

-Matrix Buff giving the wrong description

-Fixed Dynasty Javelins being craftable

-A bug where simply killing one of Supreme Pinky's Dopplegangers was enough to count as beating the boss

-Demon Steppers and Luminary Wings Stacking allowing crazy fast speeds

-Prismal Booster-style flying allowing you to fly through blocks while going straight up, you can still pass through 1-block thick tiles however

-Dank Arrows being free to craft

-A possible bug where Portal Bullets could pierce and do extremely high damage

-Improved the Beam Gun's code to hopefully improve FPS

-Changed the way the HUD is drawn again, this time it's using interface layers, which means it should not conflict with other mods at all! Yay!

-Fixed various spelling mistakes

-MANY other smaller fixes



2.30-(3/2/20)-Patch #3:

--Changed:

-Lunar Cascade costs slightly more mana

-Adjusted prices of alot of items

-Because of some changed code, Cirno's homing bolts behave differently

-Damage on some weapons

-Removed most bonus damage off the Grenadier's Glove, this weapon is already very strong for its uses especially now that you can get powerful speed prefixes on thrower items

-Nerfed some of the Dank gear


--Fixed:

-Issues with the Corroded Shield

-Cursed Flame blocks being used in crafting recipes instead of Cursed Flames

-Improving FPS on various projectiles that use nearby enemy/player detection

-Massacre Prototype didn't cost any mana

-Acid Scepter's shots staying in place when you died

-Grenadier's Glove not reciving any bonus velocity from throwing velocity; I tend to forget this one isn't automatically added to throwing weapons


2.20-(2/27/20)-Patch #2:

--Added:
-Acid Scepter, Flail-O-Nades,Demon Steppers, Bundle of Javelin Spear Heads and Javelin Parts, Corroded Shield and Amber Glow Skull (Spider Queen Expert Drop and bonus Drop respectively), Cursed Hail, Massacre Prototype, Heartreach/Booster/Omni Magnets

-A few more chat lines, as well as a hint to how to easily get more of the custom ammo types

-Crafting recipes for the SandStorm in a Bottle, Flying Carpet, Slime Staff, and Shark Tooth Necklace


--Changed:

-Opening a Terraria Co Supply Crate now has a chance to drop a Gold Ring, Lucky Coin, and Discount Card

-recipe changes for Bundle of Javelin Bases, Prismal Ore, and Luminary Wings

-Blazewyrm's set bonus explosion has been changed, mostly nerfed

-Omega Sigil is now an accessory, might be overpowered, let me know

-Buffed XOP FlameThrower: Does not cause immunity frames with other projectiles

-Buffed Mudrock Crasher: which got a rework-Nerfed SBC Cannon/Mk2: SBC Cannonballs don't crit, crits on the SBC Cannon and MK2 were honestly way too overpowered given the weapons' low rate of fire and very high damage, however, they ignore enemy immunity frames and the explosion CAN crit

-Spider Queen's Acid Venom has been buffed to do a type of debuff splash damage, this applies to player projectiles too

-Cosmic Cocktail has been reworked-it now ignores immunity frames, projectiles do variable damage, and is slower and does less base damage

-True Moonlight's projectiles hit slightly slower and penetrate less, the movement speed of said projectiles doesn't change, and the weapon charges faster with a quick melee swing speed, Lv3 Slashwave hits less often and homes from a greater distance

-Assassin and Elite Emblem have been reworked as the Omni-Magnet now uses the grab bonus

-Novus, Notchvus, and Dosed arrows have had their homing adjusted

-Starfish Burster's and Starfish Blaster's Starfish projectiles all hit on a target


--Fixed:

-Spider Queen not dropping any Gold

-Accessories that were stackable

-Not really a fix, but updating framework for 1.4, so grenades don't end up doing ranged damage when the update comes

-Being able to Dos enemies in gas who were already combusted

-The F.S.R.G not being able to get crits

-True Moonlight and Surt, the projectiles don't interfer with the melee hits anymore

-Star Burster and Starfish-Burster taking mana when you have no ammo left

-Optimized Homing Arrow code a bit, should lead to better FPS with alot of enemies on screen

-Other smaller bugs


2.11-(2/24/20)-Patch #1b:

This version is the same as below, but I was running into a MariaDB error that kept me from posting, and was debugging it.


2.10-(2/24/20)-Patch #1:

--Changed:

-When Calamity is installed-Doom Harbinger and Surpreme Pinky will automatically force Revengence and Death mode off, this is due to a critical conflict between the mods during these fights

-Enhancing Charm turned out to be alot stronger than it said it was and has been nerfed accordingly

-Normal Difficulty now only gives you 80% of the Expertise when rewarded, while expert mode gives the full amount

-Removed Draken's lines that talk about him being sorry to Val, because he's not anymore

-Reverse Engineering Station's crafting recipe has been changed a bit, as well as a few other items, but nothing pace breaking

-Lined-out some tooltips to shorten them being too long on lower resolutions

-Crafting recipes for the Heart Lantern and Star in a Bottle Pets have been altered

-Added Glow Masks for a quite a few weapons


--Fixed:

-HOPEFULLY fixed the Sword Shrines getting deleted by generation (atleast the annoying ones, like traps)

-Cosmillash and Beam Gun now do variable damage where they didn't do any before

-Fixed soft-locking issues with the Grenadier's Glove and speed-related modifiers (upped value worth a bit more too to make reforging less cheap)

-Fixed Virulent and Mangrove item crafting issues and adjusted them to require Dank Wood (where needed)

-I forgot to mention a weapon the previous version added- the Circuit Breaker Blade, so yeah, whoops, also updating how I do changelogs numbers by also adding dates.

-A possible issue with Massive Bleeding and Gourged setting the buff type to a different buff

-Typoes and a few other things



2.0-(2/22/20)-Caliburn's Gifts:

--A majorish-sorta update for you! 3 Caliburn Alters can now be found on new worlds, they each contain a strong pre-hardmode sword, at a price... But it will be worth it for their true form.

--Added the Dank Shrines-a series of 3 mini dungeons brimmed with traps, but with great rewards for those willing to take the challange, can you find all 3 in your world? (Hint-entry point is near the tops, swords are near the bottom, usually)

--Added the Caliburn Trio and their upgrade: True Caliburn

--Added Onyx Tactical Shotgun, Automatic SMG Rifle, Tesla Staff, Circuit Breaker Blade, and Gatlipillar

--Added the Dank Compass to Draken's shop

--Added Dank Wood, Items, and Armor, you can find what you need in Dank Shrine Chests, and when you run out, you can fish em up in there!

--Added a new (mini)Boss-The Spirit of Caliburn, those swords won't come free, you'll need to prove your mettle first

--Added 2 light pets

--Added Revamped Trap Damage system

--Added Prefixes for trap-related items, as well as releated to throwing items and accessories!

--The Grenadier's Glove and Primordial Skull gained a new a passive ability

--Ice Scepter got an alt fire

--Expertise cap is now 16000 (before was 15000)


--Changed:

-The way the HUD above the player is drawn, it is no longer a player layer, but rather an overlay

-XOP Flamethrower's flames can no longer exist going into water, and die like lava rocks do on touching it

-Enemies who are immune to bleeding are now also immune to Massive Bleeding and Gourged

-Updated the tooltip for Havoc's Fragmented Remains

-Added Treepeater tooltip to mention how fast the projectile is.

-Buffed Acid Scepter (charges up faster)

-Reduced speed of Cosmic Cocktail when thrown by hand, added a tip to the Grenadier's Glove

-Altered some items' crafting recipes

-Buffed Sword of a Thousand Truths, is no longer expert only, is now sold by Draken post-Cratrosity

-Nerfed TPS's boss fight, but made the predicted aim of lasers properly... uh, aim. Also nerfed Cratrosity's attacks.

-Nerfed Flame-Trap Thrower and Heat Beater

-Buffed Super Dart Trap 'gun'

-Buffed Sharkvern-his body segments have 75% DR now

-Buffed F.S.R.G and has been moved a bit later in progression as to not serve as a direct sidegrade to the MegaShark

-Nerfed Space Diver Armor's base defense and set bonus: Taking any amount of damage takes off a small potion of the breath meter in addition, also each pierce gives you -5% damage

-Various changes to crafting recipes, mostly to account for the new items

-The mod name again, because I don't think 'content mod' is really that entricing


--Fixed:

-The Expertise system, it was broken with modded npc types, this will effect your save, and will allow you to get over 15K Expertise now. It was that or your characters get bricked.

-Removed left over debug code on Sword of a Thousand Truths

-A possible crash from hovering over items, maybe

-Murk's treasure bag not dropping the Mudmore

-Fixed FlameTrap Thrower doing stupidly high damage up close

-Super Dart Trap 'gun' not showing crit chance-as it's the only trap-related weapon that can infact, crit.

-Draken talking about Calamity related things while only Boss Checklist is installed, also Draken only talks about a home while he is not homeless

-Murk being marked as beaten in hardmode the first time you kill him

-Netcode related stuff, your javelins shouldn't be desyncing so badly now

-Rebalanced some other things


-Known Bugs

-Netcode not 100% guaranteed to work, I have done some tests here and there to iron out the major issues, but still, problems may arise; be aware.

-FlameTrap Thrower bugs out in Enigma's Etherial; the particles are invisible; this is coming from their side, not mine. As to why I have no idea.

-Dank Shrines can sometimes not grow large enough to have any entrances, also they can generate only the center if they generate inside the temple/dungeon (this has happened, quite alot, thou it's rare and I'll fix it later :/)

-When Caliburn Spirits summon traps, they can possibly break the Sword Alter, now I have added 2 extra blocks on both sides to help buffer this, but it might still happen, so try to not be fighting near the alter in this case.

-Wraiths don't seem to drop the proper materials atm per world



1.02-(2/14/20)-Hot Fix #1:

--Added 'technological' to the beginning of all Sam weapons, this is cosmetic for the time being, but I felt they needed their own special damage text

--Changed the tooltips for all trap weapons that cannot crit: the crit chances doesn't show up

--Removed Hammer Editor, for now

--Fixed a nasty bug that would cause null exception errors when making a new character, whoops!


1.01-(2/13/20)-Changed the mod title to try to make it sound more exciting

1.0-(2/13/20)- Released!


Credits:

--------------------------------------------------------------Credits (so far):--------------------------------------------------------------



The current active team:

[USER=104197]@IDGCaptainRussia[/USER] (me): Coder and mod owner, and a few miscellaneous sprites

yes, it's literally just me, but I'm fine with this, because:



Anarog/Kuronaki#9669 on discord: Made some sprites for different items, has cool ideas too.



JellyBru#3929 (Donno if he has a forums account likely not), did the epic Wraith Resprites



Just DudeSquid#4617 and daimgamer#6490 on discord, did some great (Calamity/"Superior") sprites, Squid also has his own mod called Volcanit, where most of his work can be found, check it out!



--------------------------------------------------------------Contributions--------------------------------------------------------------



[USER=61456]@PhilBill44[/USER] : Spriter, made the mod icon, suggestions and ideas, and other things (I honestly couldn't have made that icon, love ya) (I miss you very much Phil :( )



[USER=75780]@Mister Creeper[/USER] : Former Havoc team Spriter, currently made alot of items including Stormbreaker's sprite. Left the team over personal reasons. Dev items are left in because I refuse to remove my hard work over his petty reasons.



Freya manibrandr: Whom I had commissioned some paid sprites (oh where that come from?) of some very specific things I wanted to make in the mod, this would include all the Serious Sam weapons, and Draken



Dejsprite#0880 : did some sprites



[USER=161571]@El3trick_Plays[/USER] : Made some sprites for items that needed them, pretty awesome! Later dropped out of the team without a word and cut all contact with me for seemly no reason.



[USER=156265]@Lonely Star[/USER] : did one sprite and kinda went missing, has shown very little activity.



[USER=55315]@Kooyah[/USER] : Retired and former Havoc team Spriter, and all of sharkvern's sprites, and a good chunk of havoc item and tile sprites. (also includes backlog sprites, and he even made an improved one, awesome!)



[USER=58157]@Iggysaur[/USER] : the Havoc team coder before myself, spriter as well, did most of the code for the ported items from that mod.



[USER=48283]@GabeHasWon[/USER] for making Murk originally.



Vasyanex - formally on the Havoc team, they need their own mention as they are the only people I don't have 100% perms to use content from, but I really don't want Vasyanex's sprites going to waste or AlexusIchimonji's Caliburn sprites, and they were meant for havoc which I have perms to use now anyways.

(AlexusIchimonji's sprites have been resprited, so that's one less issue)



Wicloud#1470 on discord: Made Cirno's wings sprites, Ice Fairy Sprites, and a few other things.



[USER=99212]@qwerty3.14[/USER]  :  Allowing me to use his GlowMask Library from his own mod, on top of being a massive inspiration for alot of the more creative things I've made here.



And [USER=56363]@TheRandomMidget[/USER] for permission to port havoc content (including Murk and... pretty much anything I could fit in).



--------------------------------------------------------------And a thanks to:--------------------------------------------------------------

[USER=2644]@zadum4ivii[/USER] : used and edited your stove code because I'm garbage at making multi-tiles :p



[USER=210]@Kazzymodus[/USER]: Gonna be honest, I don't think most people know that Shockwave shader was yours, but the moment I found out... You get the credit!



[USER=75173]@Ðark?ight[/USER] + [USER=123064]@direwolf420[/USER] + [USER=37401]@jopojelly[/USER] : you 3 have helped me so much since I started modding, I wouldn't be here without your help!



And lastly: someone who pulled me back to finish what I started, they wish to leave the Terraria scene themselves and wish for their name to not be mentioned, but if your reading this, you know who you are. Thank you.


And now finally, some used code mentions:

Boffin#8216 : Thanks for the Trail Shader and Matrix logic required for it! It's gonna be complete epic with how I plan to use it!

[USER=8638]@Joost8910[/USER] Years ago, I adapted some code from his Gigamesh boss to use in IDGLib, and ironically enough, I did it again in recent times! I still use it to this day and very heavily in SGAmod <3 (To be more specific: his fan projectile code from years ago and adapted, and his movement prediction code of recent times)

[USER=75857]@ThatOneJuicyOrange_[/USER] I donno how much of this you wrote yourself, you always had trouble with code, but the blizzard code was borrowed from EA, you are credited in the version of the mod that features it

[USER=52199]@Zoaklen[/USER], I know your not longer here, but if not for your bare bones sky code, I wouldn't have learned to make custom skies, thank you!

[USER=16350]@blushiemagic[/USER] again, many things, mostly stuff from Bluemagic which have served as a major learning tool for me, but in this case it would be your pureium world gen code, and using a cut down version of your World Reaver code to learn the basis of overlays, thank you!