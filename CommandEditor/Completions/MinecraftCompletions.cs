﻿using System.Collections.Generic;
using System.Linq;

namespace Cafemoca.CommandEditor.Completions
{
    public static class MinecraftCompletions
    {
        public static IEnumerable<CompletionData> GetCommandCompletion()
        {
            return new[]
            {
                new CompletionData("achievement", ""),
                new CompletionData("blockdata", ""),
                new CompletionData("clear", ""),
                new CompletionData("clone", ""),
                new CompletionData("debug", ""),
                new CompletionData("defaultgamemode", ""),
                new CompletionData("difficulty", ""),
                new CompletionData("effect", ""),
                new CompletionData("execute", ""),
                new CompletionData("fill", ""),
                new CompletionData("gamemode", ""),
                new CompletionData("gamerule", ""),
                new CompletionData("give", ""),
                new CompletionData("kill", ""),
                new CompletionData("particle", ""),
                new CompletionData("playsound", ""),
                new CompletionData("publish", ""),
                new CompletionData("replaceitem", ""),
                new CompletionData("say", ""),
                new CompletionData("scoreboard", ""),
                new CompletionData("seed", ""),
                new CompletionData("setblock", ""),
                new CompletionData("setworldspawn", ""),
                new CompletionData("spawnpoint", ""),
                new CompletionData("spreadplayers", ""),
                new CompletionData("stats", ""),
                new CompletionData("summon", ""),
                new CompletionData("tellraw", ""),
                new CompletionData("testfor", ""),
                new CompletionData("testforblock", ""),
                new CompletionData("testforblocks", ""),
                new CompletionData("time", ""),
                new CompletionData("title", ""),
                new CompletionData("toggledownfall", ""),
                new CompletionData("tp", ""),
                new CompletionData("weather", ""),
                new CompletionData("worldborder", ""),
                new CompletionData("xp", ""),
                new CompletionData("ban", ""),
                new CompletionData("ban-ip", ""),
                new CompletionData("banlist", ""),
                new CompletionData("deop", ""),
                new CompletionData("kick", ""),
                new CompletionData("list", ""),
                new CompletionData("op", ""),
                new CompletionData("pardon", ""),
                new CompletionData("pardon-ip", ""),
                new CompletionData("save-all", ""),
                new CompletionData("save-off", ""),
                new CompletionData("save-on", ""),
                new CompletionData("setidletimeout", ""),
                new CompletionData("stop", ""),
                new CompletionData("whitelist", ""),
                new CompletionData("help", ""),
                new CompletionData("?", ""),
                new CompletionData("me", ""),
                new CompletionData("tell", ""),
                new CompletionData("msg", ""),
                new CompletionData("w", ""),
                new CompletionData("trigger", ""),
            };
        }

        public static IEnumerable<CompletionData> GetItemCompletion()
        {
            return new[]
            {
                new CompletionData("stone", ""),
                new CompletionData("grass", ""),
                new CompletionData("dirt", ""),
                new CompletionData("cobblestone", ""),
                new CompletionData("planks", ""),
                new CompletionData("sapling", ""),
                new CompletionData("bedrock", ""),
                new CompletionData("sand", ""),
                new CompletionData("gravel", ""),
                new CompletionData("gold_ore", ""),
                new CompletionData("iron_ore", ""),
                new CompletionData("coal_ore", ""),
                new CompletionData("log", ""),
                new CompletionData("leaves", ""),
                new CompletionData("sponge", ""),
                new CompletionData("glass", ""),
                new CompletionData("lapis_ore", ""),
                new CompletionData("lapis_block", ""),
                new CompletionData("dispenser", ""),
                new CompletionData("sandstone", ""),
                new CompletionData("noteblock", ""),
                new CompletionData("golden_rail", ""),
                new CompletionData("detector_rail", ""),
                new CompletionData("sticky_piston", ""),
                new CompletionData("web", ""),
                new CompletionData("tallgrass", ""),
                new CompletionData("deadbush", ""),
                new CompletionData("piston", ""),
                new CompletionData("wool", ""),
                new CompletionData("yellow_flower", ""),
                new CompletionData("red_flower", ""),
                new CompletionData("brown_mushroom", ""),
                new CompletionData("red_mushroom", ""),
                new CompletionData("gold_block", ""),
                new CompletionData("iron_block", ""),
                new CompletionData("stone_slab", ""),
                new CompletionData("brick_block", ""),
                new CompletionData("tnt", ""),
                new CompletionData("bookshelf", ""),
                new CompletionData("mossy_cobblestone", ""),
                new CompletionData("obsidian", ""),
                new CompletionData("torch", ""),
                new CompletionData("fire", ""),
                new CompletionData("mob_spawner", ""),
                new CompletionData("oak_stairs", ""),
                new CompletionData("chest", ""),
                new CompletionData("diamond_ore", ""),
                new CompletionData("diamond_block", ""),
                new CompletionData("crafting_table", ""),
                new CompletionData("farmland", ""),
                new CompletionData("furnace", ""),
                new CompletionData("lit_furnace", ""),
                new CompletionData("ladder", ""),
                new CompletionData("rail", ""),
                new CompletionData("stone_stairs", ""),
                new CompletionData("lever", ""),
                new CompletionData("stone_pressure_plate", ""),
                new CompletionData("wooden_pressure_plate", ""),
                new CompletionData("redstone_ore", ""),
                new CompletionData("redstone_torch", ""),
                new CompletionData("stone_button", ""),
                new CompletionData("snow_layer", ""),
                new CompletionData("ice", ""),
                new CompletionData("snow", ""),
                new CompletionData("cactus", ""),
                new CompletionData("clay", ""),
                new CompletionData("jukebox", ""),
                new CompletionData("fence", ""),
                new CompletionData("pumpkin", ""),
                new CompletionData("netherrack", ""),
                new CompletionData("soul_sand", ""),
                new CompletionData("glowstone", ""),
                new CompletionData("portal", ""),
                new CompletionData("lit_pumpkin", ""),
                new CompletionData("stained_glass", ""),
                new CompletionData("trapdoor", ""),
                new CompletionData("monster_egg", ""),
                new CompletionData("stonebrick", ""),
                new CompletionData("brown_mushroom_block", ""),
                new CompletionData("red_mushroom_block", ""),
                new CompletionData("iron_bars", ""),
                new CompletionData("glass_pane", ""),
                new CompletionData("melon_block", ""),
                new CompletionData("vine", ""),
                new CompletionData("fence_gate", ""),
                new CompletionData("brick_stairs", ""),
                new CompletionData("stone_brick_stairs", ""),
                new CompletionData("mycelium", ""),
                new CompletionData("waterlily", ""),
                new CompletionData("nether_brick", ""),
                new CompletionData("nether_brick_fence", ""),
                new CompletionData("nether_brick_stairs", ""),
                new CompletionData("enchanting_table", ""),
                new CompletionData("end_portal_frame", ""),
                new CompletionData("end_stone", ""),
                new CompletionData("dragon_egg", ""),
                new CompletionData("redstone_lamp", ""),
                new CompletionData("wooden_slab", ""),
                new CompletionData("cocoa", ""),
                new CompletionData("sandstone_stairs", ""),
                new CompletionData("emerald_ore", ""),
                new CompletionData("ender_chest", ""),
                new CompletionData("tripwire_hook", ""),
                new CompletionData("emerald_block", ""),
                new CompletionData("spruce_stairs", ""),
                new CompletionData("birch_stairs", ""),
                new CompletionData("jungle_stairs", ""),
                new CompletionData("command_block", ""),
                new CompletionData("beacon", ""),
                new CompletionData("cobblestone_wall", ""),
                new CompletionData("wooden_button", ""),
                new CompletionData("anvil", ""),
                new CompletionData("trapped_chest", ""),
                new CompletionData("light_weighted_pressure_plate", ""),
                new CompletionData("heavy_weighted_pressure_plate", ""),
                new CompletionData("daylight_detector", ""),
                new CompletionData("redstone_block", ""),
                new CompletionData("quartz_ore", ""),
                new CompletionData("hopper", ""),
                new CompletionData("quartz_block", ""),
                new CompletionData("quartz_stairs", ""),
                new CompletionData("activator_rail", ""),
                new CompletionData("dropper", ""),
                new CompletionData("stained_hardened_clay", ""),
                new CompletionData("stained_glass_pane", ""),
                new CompletionData("log2", ""),
                new CompletionData("acacia_stairs", ""),
                new CompletionData("dark_oak_stairs", ""),
                new CompletionData("slime", ""),
                new CompletionData("barrier", ""),
                new CompletionData("iron_trapdoor", ""),
                new CompletionData("prismarine", ""),
                new CompletionData("sea_lantern", ""),
                new CompletionData("hay_block", ""),
                new CompletionData("carpet", ""),
                new CompletionData("hardened_clay", ""),
                new CompletionData("coal_block", ""),
                new CompletionData("packed_ice", ""),
                new CompletionData("double_plant", ""),
                new CompletionData("iron_shovel", ""),
                new CompletionData("iron_pickaxe", ""),
                new CompletionData("iron_axe", ""),
                new CompletionData("flint_and_steel", ""),
                new CompletionData("apple", ""),
                new CompletionData("bow", ""),
                new CompletionData("arrow", ""),
                new CompletionData("coal", ""),
                new CompletionData("diamond", ""),
                new CompletionData("iron_ingot", ""),
                new CompletionData("gold_ingot", ""),
                new CompletionData("iron_sword", ""),
                new CompletionData("wooden_sword", ""),
                new CompletionData("wooden_shovel", ""),
                new CompletionData("wooden_pickaxe", ""),
                new CompletionData("wooden_axe", ""),
                new CompletionData("stone_sword", ""),
                new CompletionData("stone_shovel", ""),
                new CompletionData("stone_pickaxe", ""),
                new CompletionData("stone_axe", ""),
                new CompletionData("diamond_sword", ""),
                new CompletionData("diamond_shovel", ""),
                new CompletionData("diamond_pickaxe", ""),
                new CompletionData("diamond_axe", ""),
                new CompletionData("stick", ""),
                new CompletionData("bowl", ""),
                new CompletionData("mushroom_stew", ""),
                new CompletionData("golden_sword", ""),
                new CompletionData("golden_shovel", ""),
                new CompletionData("golden_pickaxe", ""),
                new CompletionData("golden_axe", ""),
                new CompletionData("string", ""),
                new CompletionData("feather", ""),
                new CompletionData("gunpowder", ""),
                new CompletionData("wooden_hoe", ""),
                new CompletionData("stone_hoe", ""),
                new CompletionData("iron_hoe", ""),
                new CompletionData("diamond_hoe", ""),
                new CompletionData("golden_hoe", ""),
                new CompletionData("wheat_seeds", ""),
                new CompletionData("wheat", ""),
                new CompletionData("bread", ""),
                new CompletionData("leather_helmet", ""),
                new CompletionData("leather_chestplate", ""),
                new CompletionData("leather_leggings", ""),
                new CompletionData("leather_boots", ""),
                new CompletionData("chainmail_helmet", ""),
                new CompletionData("chainmail_chestplate", ""),
                new CompletionData("chainmail_leggings", ""),
                new CompletionData("chainmail_boots", ""),
                new CompletionData("iron_helmet", ""),
                new CompletionData("iron_chestplate", ""),
                new CompletionData("iron_leggings", ""),
                new CompletionData("iron_boots", ""),
                new CompletionData("diamond_helmet", ""),
                new CompletionData("diamond_chestplate", ""),
                new CompletionData("diamond_leggings", ""),
                new CompletionData("diamond_boots", ""),
                new CompletionData("golden_helmet", ""),
                new CompletionData("golden_chestplate", ""),
                new CompletionData("golden_leggings", ""),
                new CompletionData("golden_boots", ""),
                new CompletionData("flint", ""),
                new CompletionData("porkchop", ""),
                new CompletionData("cooked_porkchop", ""),
                new CompletionData("painting", ""),
                new CompletionData("golden_apple", ""),
                new CompletionData("sign", ""),
                new CompletionData("wooden_door", ""),
                new CompletionData("bucket", ""),
                new CompletionData("water_bucket", ""),
                new CompletionData("lava_bucket", ""),
                new CompletionData("minecart", ""),
                new CompletionData("saddle", ""),
                new CompletionData("iron_door", ""),
                new CompletionData("redstone", ""),
                new CompletionData("snowball", ""),
                new CompletionData("boat", ""),
                new CompletionData("leather", ""),
                new CompletionData("milk_bucket", ""),
                new CompletionData("brick", ""),
                new CompletionData("clay_ball", ""),
                new CompletionData("reeds", ""),
                new CompletionData("paper", ""),
                new CompletionData("book", ""),
                new CompletionData("slimeball", ""),
                new CompletionData("chest_minecart", ""),
                new CompletionData("furnace_minecart", ""),
                new CompletionData("egg", ""),
                new CompletionData("compass", ""),
                new CompletionData("fishing_rod", ""),
                new CompletionData("clock", ""),
                new CompletionData("glowstone_dust", ""),
                new CompletionData("fish", ""),
                new CompletionData("cooked_fished", ""),
                new CompletionData("dye", ""),
                new CompletionData("bone", ""),
                new CompletionData("sugar", ""),
                new CompletionData("cake", ""),
                new CompletionData("bed", ""),
                new CompletionData("repeater", ""),
                new CompletionData("cookie", ""),
                new CompletionData("filled_map", ""),
                new CompletionData("shears", ""),
                new CompletionData("melon", ""),
                new CompletionData("pumpkin_seeds", ""),
                new CompletionData("melon_seeds", ""),
                new CompletionData("beef", ""),
                new CompletionData("cooked_beef", ""),
                new CompletionData("chicken", ""),
                new CompletionData("cooked_chicken", ""),
                new CompletionData("rotten_flesh", ""),
                new CompletionData("ender_pearl", ""),
                new CompletionData("blaze_rod", ""),
                new CompletionData("ghast_tear", ""),
                new CompletionData("gold_nugget", ""),
                new CompletionData("nether_wart", ""),
                new CompletionData("potion", ""),
                new CompletionData("glass_bottle", ""),
                new CompletionData("spider_eye", ""),
                new CompletionData("fermented_spider_eye", ""),
                new CompletionData("blaze_powder", ""),
                new CompletionData("magma_cream", ""),
                new CompletionData("brewing_stand", ""),
                new CompletionData("cauldron", ""),
                new CompletionData("ender_eye", ""),
                new CompletionData("speckled_melon", ""),
                new CompletionData("spawn_egg", ""),
                new CompletionData("experience_bottle", ""),
                new CompletionData("fire_charge", ""),
                new CompletionData("writable_book", ""),
                new CompletionData("written_book", ""),
                new CompletionData("emerald", ""),
                new CompletionData("item_frame", ""),
                new CompletionData("flower_pot", ""),
                new CompletionData("carrot", ""),
                new CompletionData("potato", ""),
                new CompletionData("baked_potato", ""),
                new CompletionData("poisonous_potato", ""),
                new CompletionData("map", ""),
                new CompletionData("golden_carrot", ""),
                new CompletionData("skull", ""),
                new CompletionData("carrot_on_a_stick", ""),
                new CompletionData("nether_star", ""),
                new CompletionData("pumpkin_pie", ""),
                new CompletionData("fireworks", ""),
                new CompletionData("firework_charge", ""),
                new CompletionData("enchanted_book", ""),
                new CompletionData("comparator", ""),
                new CompletionData("netherbrick", ""),
                new CompletionData("quartz", ""),
                new CompletionData("tnt_minecart", ""),
                new CompletionData("hopper_minecart", ""),
                new CompletionData("prismarine_shard", ""),
                new CompletionData("prismarine_crystals", ""),
                new CompletionData("iron_horse_armor", ""),
                new CompletionData("golden_horse_armor", ""),
                new CompletionData("diamond_horse_armor", ""),
                new CompletionData("lead", ""),
                new CompletionData("name_tag", ""),
                new CompletionData("command_block_minecart", ""),
                new CompletionData("record_13", ""),
                new CompletionData("record_cat", ""),
                new CompletionData("record_blocks", ""),
                new CompletionData("record_chirp", ""),
                new CompletionData("record_far", ""),
                new CompletionData("record_mall", ""),
                new CompletionData("record_mellohi", ""),
                new CompletionData("record_stal", ""),
                new CompletionData("record_strad", ""),
                new CompletionData("record_ward", ""),
                new CompletionData("record_11", ""),
                new CompletionData("record_wait", ""),
                new CompletionData("banner", ""),
            };
        }

        public static IEnumerable<CompletionData> GetBlockCompletion()
        {
            // リソースファイルで管理するべき
            //return resources.Where(r => r.Type == ItemType.Block).Select(r => new CompletionData(r.Name, r.Desc));

            return new[]
            {
                new CompletionData("air", "空気"),
                new CompletionData("stone", "石"),
                new CompletionData("grass", "草ブロック"),
                new CompletionData("dirt", "土"),
                new CompletionData("cobblestone", "丸石"),
                new CompletionData("planks", "木材"),
                new CompletionData("sapling", "苗木"),
                new CompletionData("bedrock", "岩盤"),
                new CompletionData("flowing_water", "水流"),
                new CompletionData("water", "水源"),
                new CompletionData("flowing_lava", "溶岩流"),
                new CompletionData("lava", "溶岩源"),
                new CompletionData("sand", "砂"),
                new CompletionData("gravel", "砂利"),
                new CompletionData("gold_ore", "金鉱石"),
                new CompletionData("iron_ore", "鉄鉱石"),
                new CompletionData("coal_ore", "石炭鉱石"),
                new CompletionData("log", "原木"),
                new CompletionData("leaves", "葉ブロック"),
                new CompletionData("sponge", "スポンジ"),
                new CompletionData("glass", "ガラス"),
                new CompletionData("lapis_ore", "ラピスラズリ鉱石"),
                new CompletionData("lapis_block", "ラピスラズリブロック"),
                new CompletionData("dispenser", "ディスペンサー"),
                new CompletionData("sandstone", "砂岩"),
                new CompletionData("noteblock", "音符ブロック"),
                new CompletionData("golden_rail", "パワードレール"),
                new CompletionData("detector_rail", "ディテクターレール"),
                new CompletionData("sticky_piston", "粘着ピストン"),
                new CompletionData("web", "クモの巣"),
                new CompletionData("tallgrass", "草"),
                new CompletionData("deadbush", "枯れ木"),
                new CompletionData("piston", "ピストン"),
                new CompletionData("piston_head", "ピストン (ヘッド)"),
                new CompletionData("wool", "羊毛"),
                new CompletionData("piston_extension", "ピストン (拡張)"),
                new CompletionData("yellow_flower", "タンポポ"),
                new CompletionData("red_flower", "ポピー"),
                new CompletionData("brown_mushroom", "キノコ (茶)"),
                new CompletionData("red_mushroom", "キノコ (赤)"),
                new CompletionData("gold_block", "金ブロック"),
                new CompletionData("iron_block", "鉄ブロック"),
                new CompletionData("double_stone_slab", "ハーフブロック (2重)"),
                new CompletionData("stone_slab", "ハーフブロック"),
                new CompletionData("brick_block", "レンガ"),
                new CompletionData("tnt", "TNT"),
                new CompletionData("bookshelf", "本棚"),
                new CompletionData("mossy_cobblestone", "苔石"),
                new CompletionData("obsidian", "黒曜石"),
                new CompletionData("torch", "松明"),
                new CompletionData("fire", "炎"),
                new CompletionData("mob_spawner", "モンスタースポナー"),
                new CompletionData("oak_stairs", "オークの木の階段"),
                new CompletionData("chest", "チェスト"),
                new CompletionData("redstone_wire", "レッドストーンワイヤー"),
                new CompletionData("diamond_ore", "ダイヤモンド鉱石"),
                new CompletionData("diamond_block", "ダイヤモンドブロック"),
                new CompletionData("crafting_table", "作業台"),
                new CompletionData("farmland", "耕した土"),
                new CompletionData("furnace", "かまど"),
                new CompletionData("lit_furnace", "かまど (燃焼)"),
                new CompletionData("standing_sign", "看板 (地面)"),
                new CompletionData("ladder", "はしご"),
                new CompletionData("rail", "レール"),
                new CompletionData("stone_stairs", "石の階段"),
                new CompletionData("wall_sign", "看板 (壁)"),
                new CompletionData("lever", "レバー"),
                new CompletionData("stone_pressure_plate", "石の感圧版"),
                new CompletionData("wooden_pressure_plate", "木の感圧版"),
                new CompletionData("redstone_ore", "レッドストーン鉱石"),
                new CompletionData("lit_redstone_ore", "レッドストーン鉱石 (発光)"),
                new CompletionData("unlit_redstone_torch", "レッドストーントーチ (消灯)"),
                new CompletionData("redstone_torch", "レッドストーントーチ"),
                new CompletionData("stone_button", "木のボタン"),
                new CompletionData("snow_layer", "雪"),
                new CompletionData("ice", "氷ブロック"),
                new CompletionData("snow", "雪ブロック"),
                new CompletionData("cactus", "サボテン"),
                new CompletionData("clay", "粘土"),
                new CompletionData("jukebox", "ジュークボックス"),
                new CompletionData("fence", "フェンス"),
                new CompletionData("pumpkin", "カボチャ"),
                new CompletionData("netherrack", "ネザーラック"),
                new CompletionData("soul_sand", "ソウルサンド"),
                new CompletionData("glowstone", "グロウストーン"),
                new CompletionData("portal", "ポータル"),
                new CompletionData("lit_pumpkin", "ジャック・オ・ランタン"),
                new CompletionData("unpowered_repeater", "レッドストーンリピーター"),
                new CompletionData("powered_repeater", "レッドストーンリピーター (動作)"),
                new CompletionData("stained_glass", "色付きガラス"),
                new CompletionData("trapdoor", "木のトラップドア"),
                new CompletionData("monster_egg", "モンスターエッグ"),
                new CompletionData("stonebrick", "石レンガ"),
                new CompletionData("brown_mushroom_block", "キノコブロック (茶色)"),
                new CompletionData("red_mushroom_block", "キノコブロック (赤色)"),
                new CompletionData("iron_bars", "鉄格子"),
                new CompletionData("glass_pane", "板ガラス"),
                new CompletionData("melon_block", "スイカ"),
                new CompletionData("pumpkin_stem", "カボチャの苗"),
                new CompletionData("melon_stem", "スイカの苗"),
                new CompletionData("vine", "つる"),
                new CompletionData("fence_gate", "フェンスゲート"),
                new CompletionData("brick_stairs", "レンガの階段"),
                new CompletionData("stone_brick_stairs", "石レンガの階段"),
                new CompletionData("mycelium", "菌糸"),
                new CompletionData("waterlily", "スイレン"),
                new CompletionData("nether_brick", "ネザーレンガ"),
                new CompletionData("nether_brick_fence", "ネザーレンガフェンス"),
                new CompletionData("nether_brick_stairs", "ネザーレンガの階段"),
                new CompletionData("enchanting_table", "エンチャントテーブル"),
                new CompletionData("end_portal", "エンドポータル"),
                new CompletionData("end_portal_frame", "エンドポータル (フレーム)"),
                new CompletionData("end_stone", "エンドストーン"),
                new CompletionData("dragon_egg", "ドラゴンエッグ"),
                new CompletionData("redstone_lamp", "レッドストーンランプ"),
                new CompletionData("lit_redstone_lamp", "レッドストーンランプ (点灯)"),
                new CompletionData("double_wooden_slab", "木のハーフブロック (2重)"),
                new CompletionData("wooden_slab", "木材ハーフブロック"),
                new CompletionData("cocoa", "カカオ"),
                new CompletionData("sandstone_stairs", "砂岩の階段"),
                new CompletionData("emerald_ore", "エメラルド功績"),
                new CompletionData("ender_chest", "得んだーチェスト"),
                new CompletionData("tripwire_hook", "トリップワイヤーフック"),
                new CompletionData("tripwire", "トリップワイヤー"),
                new CompletionData("emerald_block", "エメラルドブロック"),
                new CompletionData("spruce_stairs", "松の木の階段"),
                new CompletionData("birch_stairs", "白樺の木の階段"),
                new CompletionData("jungle_stairs", "ジャングルの木の階段"),
                new CompletionData("command_block", "コマンドブロック"),
                new CompletionData("beacon", "ビーコン"),
                new CompletionData("cobblestone_wall", "石壁"),
                new CompletionData("wooden_button", "木のボタン"),
                new CompletionData("anvil", "金床"),
                new CompletionData("trapped_chest", "トラップチェスト"),
                new CompletionData("light_weighted_pressure_plate", "金の感圧版"),
                new CompletionData("heavy_weighted_pressure_plate", "鉄の感圧版"),
                new CompletionData("unpowered_comparator", "コンパレータ (OFF)"),
                new CompletionData("powered_comparator", "コンパレータ (ON)"),
                new CompletionData("daylight_detector", "日照センサー"),
                new CompletionData("redstone_block", "レッドストーンブロック"),
                new CompletionData("quartz_ore", "ネザー水晶鉱石"),
                new CompletionData("hopper", "ホッパー"),
                new CompletionData("quartz_block", "ネザー水晶"),
                new CompletionData("quartz_stairs", "ネザー水晶の階段"),
                new CompletionData("activator_rail", "アクティベーターレール"),
                new CompletionData("dropper", "ドロッパー"),
                new CompletionData("stained_hardened_clay", "色付き粘土"),
                new CompletionData("stained_glass_pane", "色付きガラス板"),
                new CompletionData("log2", "原木 (2)"),
                new CompletionData("acacia_stairs", "アカシアの木の階段"),
                new CompletionData("dark_oak_stairs", "ダークオークの木の階段"),
                new CompletionData("slime", "スライムブロック"),
                new CompletionData("barrier", "バリアーブロック"),
                new CompletionData("iron_trapdoor", "鉄のトラップドア"),
                new CompletionData("prismarine", "プリズマリン"),
                new CompletionData("sea_lantern", "海のランタン"),
                new CompletionData("hay_block", "干草の俵"),
                new CompletionData("carpet", "カーペット"),
                new CompletionData("hardened_clay", ""),
                new CompletionData("coal_block", "石炭ブロック"),
                new CompletionData("packed_ice", "氷塊"),
                new CompletionData("double_plant", "高い草"),
                new CompletionData("standing_banner", "バナー (地面)"),
                new CompletionData("wall_banner", "バナー (壁)"),
            };
        }

        public static IEnumerable<CompletionData> GetPetternCompletion()
        {
            return new[]
            {
                new CompletionData("bl", "四角形 (左下)\nSquare Bottom Left"),
                new CompletionData("br", "四角形 (右下)\nSquare Bottom Right"),
                new CompletionData("tl", "四角形 (左上)\nSquare Bottom Left"),
                new CompletionData("tr", "四角形 (右上)\nSquare Bottom Right"),
                new CompletionData("bt", "三角形 (下部)\nTriangle Bottom"),
                new CompletionData("tt", "三角形 (上部)\nTriangle Top"),
                new CompletionData("bts", "3つ並んだ三角形 (下部)\nTriangles Bottom"),
                new CompletionData("tts", "3つ並んだ三角形 (上部)\nTriangles Top"),
                new CompletionData("hh", "水平 (上半分塗りつぶし)\nHalf Horizontal"),
                new CompletionData("hhb", "水平 (下半分塗りつぶし)\nHalf Horizontal Bottom"),
                new CompletionData("vh", "垂直 (左半分塗りつぶし)\nHalf Vertical"),
                new CompletionData("vhr", "垂直 (右半分塗りつぶし)\nHalf Vertical Right"),
                new CompletionData("ld", "対角 (左上塗りつぶし)\nDiagnalLeft"),
                new CompletionData("rd", "対角 (右下塗りつぶし)\nDiagnal Right"),
                new CompletionData("mc", "円\nMiddle Circle"),
                new CompletionData("mr", "ひし形 (中央)\nMiddle Rhombus"),
                new CompletionData("ls", "線 (左側)\nStripe Left"),
                new CompletionData("rs", "線 (右側)\nStripe Right"),
                new CompletionData("ts", "線 (上部)\nStripe Top"),
                new CompletionData("bs", "線 (下部)\nStripe Bottom"),
                new CompletionData("cs", "線 (中央 / 横向き)\nStripe Center"),
                new CompletionData("ms", "線 (中央 / 縦向き)\nStripe Middle"),
                new CompletionData("dls", "線 (左下がり)\nStripe Down-Left"),
                new CompletionData("drs", "線 (右下がり)\nStripe Down-Right"),
                new CompletionData("cr", "クロス\nCross"),
                new CompletionData("ss", "小さな網模様 (縦)\nSmall Stripes"),
                new CompletionData("gra", "グラデーション(下方向)\nGradiate"),
                new CompletionData("gru", "グラデーション(上方向)\nGradiate Up"),
                new CompletionData("bri", "レンガ模様\nBrick"),
                new CompletionData("bo", "枠線\nBorder"),
                new CompletionData("cbo", "波模様のボーダー\nCurly Border"),
                new CompletionData("flo", "花模様\nFlower"),
                new CompletionData("cre", "クリーパー模様\nCreeper"),
                new CompletionData("sku", "ガイコツ模様\nSkull"),
                new CompletionData("moj", "Mojang ロゴ\nMojang"),
            };
        }

        public static IEnumerable<CompletionData> GetColorCompletion()
        {
            return new[]
            {
                new CompletionData("black", "黒\nBlack"),
                new CompletionData("dark_blue", "濃い青\nDarkBlue"),
                new CompletionData("dark_green", "濃い緑\nDarkGreen"),
                new CompletionData("dark_aqua", "濃い水\nDarkAqua"),
                new CompletionData("dark_red", "濃い赤\nDarkRed"),
                new CompletionData("dark_purple", "濃い紫\nDarkPurple"),
                new CompletionData("gold", "金\nGold"),
                new CompletionData("gray", "灰\nGray"),
                new CompletionData("dark_gray", "濃い灰\nDarkGray"),
                new CompletionData("blue", "青\nBlue"),
                new CompletionData("green", "緑\nGreen"),
                new CompletionData("aqua", "水\nAqua"),
                new CompletionData("red", "赤\nRed"),
                new CompletionData("light_purple", "薄い紫\nLightPurple"),
                new CompletionData("yellow", "黄\nYellow"),
                new CompletionData("white", "白\nWhite"),
            };
        }

        public static IEnumerable<CompletionData> GetBooleanCompletion()
        {
            return new[]
            {
                new CompletionData("true", "true"),
                new CompletionData("false", "false"),
            };
        }

        public static IEnumerable<CompletionData> GetTargetCompletion()
        {
            return new[]
            {
                new CompletionData("@p", "p", "最寄りのプレイヤー 1名\n\n同じ距離に複数のプレイヤーが存在する場合、最後にログインしたプレイヤーを対象とします。"),
                new CompletionData("@r", "r", "ランダムなプレイヤー 1名"),
                new CompletionData("@a", "a", "すべてのプレイヤー"),
                new CompletionData("@e", "e", "すべてのエンティティ\n\nプレイヤーを含みます"),
            };
        }

        public static IEnumerable<CompletionData> GetScoreboardCompletion()
        {
            return new[]
            {
                new CompletionData("objectives"),
                new CompletionData("players"),
                new CompletionData("teams"),
            };
        }

        public static IEnumerable<CompletionData> GetScoreboardObjectivesCompletion()
        {
            return new[]
            {
                new CompletionData("list"),
                new CompletionData("add"),
                new CompletionData("remove"),
                new CompletionData("setdisplay"),
            };
        }

        public static IEnumerable<CompletionData> GetScoreboardPlayersCompletion()
        {
            return new[]
            {
                new CompletionData("list"),
                new CompletionData("set"),
                new CompletionData("add"),
                new CompletionData("remove"),
                new CompletionData("reset"),
                new CompletionData("enable"),
                new CompletionData("test"),
                new CompletionData("operation"),
            };
        }

        public static IEnumerable<CompletionData> GetScoreboardTeamsCompletion()
        {
            return new[]
            {
                new CompletionData("list"),
                new CompletionData("add"),
                new CompletionData("remove"),
                new CompletionData("empty"),
                new CompletionData("join"),
                new CompletionData("leave"),
                new CompletionData("option"),
            };
        }

        public static IEnumerable<CompletionData> GetScoreboardCriteriaCompletion()
        {
            return new[]
            {
                new CompletionData("dummy"),
                new CompletionData("trigger"),
                new CompletionData("deathCount"),
                new CompletionData("playerKillCount"),
                new CompletionData("totalKillCount"),
                new CompletionData("health"),
            };
        }

        public static IEnumerable<CompletionData> GetScoreboardSlotsCompletion()
        {
            return new[]
            {
                new CompletionData("list"),
                new CompletionData("sidebar"),
                new CompletionData("sidebar.team"),
                new CompletionData("belowName"),
            };
        }

        public static IEnumerable<CompletionData> GetScoreboardOperationCompletion()
        {
            return new[]
            {
                new CompletionData("+=", "+=\n" +
                                         "<targetName> <targetObjective> += <selectorName> <selectorObjective>\n" +
                                         "セレクタのセレクタスコアをターゲットのターゲットスコアに加算します。\n\n" +
                                         "(例):\n\t/scoreboard players operation @a score1 += @a[team=team1] score2"),
                new CompletionData("-=", "-=\n" +
                                         "/scoreboard players operation <target> <>"),
                new CompletionData("*="),
                new CompletionData("/="),
                new CompletionData("%="),
                new CompletionData("="),
                new CompletionData("<"),
                new CompletionData(">"),
                new CompletionData("><"),
            };
        }

        public static IEnumerable<CompletionData> GetScoreboardTeamOptionCompletion()
        {
            return new[]
            {
                new CompletionData("color"),
                new CompletionData("friendlyfire"),
                new CompletionData("seeFriendlyInvisibles"),
                new CompletionData("nametagVisibility"),
                new CompletionData("deathMessageVisibility"),
            };
        }

        public static IEnumerable<CompletionData> GetScoreboardTeamOptionArgsCompletion()
        {
            return new[]
            {
                new CompletionData("never"),
                new CompletionData("hideForOtherTeams"),
                new CompletionData("hideForOwnTeam"),
                new CompletionData("always"),
            };
        }

        public static IEnumerable<CompletionData> ToCompletionData(this IEnumerable<string> text)
        {
            if (text == null || text.IsEmpty())
            {
                return null;
            }
            return text.Select(x => new CompletionData(x, x));
        }
    }
}
