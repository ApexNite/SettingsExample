using System;
using NCMS;
using UnityEngine;
using ReflectionUtility;
using SettingsBox;
using HarmonyLib;

namespace ExampleMod{
    [ModEntry]
    public class Main : MonoBehaviour{
        private static Setting settingFireRain;
        private static Setting settingSuperHealth;

        void Awake() {
            settingFireRain = SettingsManager.bindSetting("Fire Rain", "Make rain drops ignite people", false, "ui/Icons/fireDropIcon");
            settingSuperHealth = SettingsManager.bindSetting("Super Health Value", 
                "Change how much health the super health trait gives", 
                9999, 25000, 0,
                pIcon: "ui/Icons/iconSuperHealth",
                pAction: changeSuperHealthValue
                );

            Harmony.CreateAndPatchAll(typeof(Main));
        }

        public static void changeSuperHealthValue(int pValue) {
            AssetManager.traits.get("super_health").base_stats[S.health] = pValue;
        }

        [HarmonyPatch(typeof(DropsLibrary), "action_rain")]
        [HarmonyPrefix]
        public static bool action_rain_Prefix(WorldTile pTile = null, string pDropID = null) {
            if ((bool)settingFireRain.Value) {
                ActionLibrary.burnTile(null, null, pTile);
		        ActionLibrary.startBurningUnits(null, null, pTile);
                return false;
            }
            
            return true;
	    }
    }
}