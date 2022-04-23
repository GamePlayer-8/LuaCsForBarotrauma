local defaultLib = {}

local AddCallMetaTable = LuaSetup.AddCallMetaTable
local CreateStatic = LuaSetup.CreateStatic
local CreateEnum = LuaUserData.CreateEnumTable

defaultLib["Byte"] = CreateStatic("Barotrauma.LuaByte", true)
defaultLib["UShort"] = CreateStatic("Barotrauma.LuaUShort", true)
defaultLib["Float"] = CreateStatic("Barotrauma.LuaFloat", true)

defaultLib["SpawnType"] = CreateEnum("Barotrauma.SpawnType")
defaultLib["ChatMessageType"] = CreateEnum("Barotrauma.Networking.ChatMessageType")
defaultLib["ServerLog_MessageType"] = CreateEnum("Barotrauma.Networking.ServerLog+MessageType")
defaultLib["ServerLogMessageType"] = CreateEnum("Barotrauma.Networking.ServerLog+MessageType")
defaultLib["PositionType"] = CreateEnum("Barotrauma.Level+PositionType")
defaultLib["InvSlotType"] = CreateEnum("Barotrauma.InvSlotType")
defaultLib["LimbType"] = CreateEnum("Barotrauma.LimbType")
defaultLib["ActionType"] = CreateEnum("Barotrauma.ActionType")
defaultLib["AbilityEffectType"] = CreateEnum("Barotrauma.AbilityEffectType")
defaultLib["StatTypes"] = CreateEnum("Barotrauma.StatTypes")
defaultLib["AbilityFlags"] = CreateEnum("Barotrauma.AbilityFlags")
defaultLib["DeliveryMethod"] = CreateEnum("Barotrauma.Networking.DeliveryMethod")
defaultLib["ClientPacketHeader"] = CreateEnum("Barotrauma.Networking.ClientPacketHeader")
defaultLib["ServerPacketHeader"] = CreateEnum("Barotrauma.Networking.ServerPacketHeader")
defaultLib["RandSync"] = CreateEnum("Barotrauma.Rand+RandSync")
defaultLib["DisconnectReason"] = CreateEnum("Barotrauma.Networking.DisconnectReason")
defaultLib["TraitorMessageType"] = CreateEnum("Barotrauma.Networking.TraitorMessageType")
defaultLib["CombatMode"] = CreateEnum("Barotrauma.AIObjectiveCombat+CombatMode")
defaultLib["CauseOfDeathType"] = CreateEnum("Barotrauma.CauseOfDeathType")
defaultLib["CharacterTeamType"] = CreateEnum("Barotrauma.CharacterTeamType")
defaultLib["ClientPermissions"] = CreateEnum("Barotrauma.Networking.ClientPermissions")
defaultLib["InputType"] = CreateStatic("Barotrauma.InputType")


defaultLib["Inventory"] = CreateStatic("Barotrauma.Inventory")
defaultLib["ContentPackageManager"] = CreateStatic("Barotrauma.ContentPackageManager")
defaultLib["GameSettings"] = CreateStatic("Barotrauma.GameSettings")
defaultLib["RichString"] = CreateStatic("Barotrauma.RichString", true)
defaultLib["Identifier"] = CreateStatic("Barotrauma.Identifier", true)
defaultLib["ContentPackage"] = CreateStatic("Barotrauma.ContentPackage", true)
defaultLib["WayPoint"] = CreateStatic("Barotrauma.WayPoint", true)
defaultLib["Submarine"] = CreateStatic("Barotrauma.Submarine", true)
defaultLib["Client"] = CreateStatic("Barotrauma.Networking.Client", true)
defaultLib["Character"] = CreateStatic("Barotrauma.Character")
defaultLib["CharacterPrefab"] = CreateStatic("Barotrauma.CharacterPrefab")
defaultLib["CharacterInfo"] = CreateStatic("Barotrauma.CharacterInfo", true)
defaultLib["CharacterInfoPrefab"] = CreateStatic("Barotrauma.CharacterInfoPrefab")
defaultLib["Item"] = CreateStatic("Barotrauma.Item", true)
AddCallMetaTable(defaultLib["Item"].ChangePropertyEventData)
defaultLib["ItemPrefab"] = CreateStatic("Barotrauma.ItemPrefab", true)
defaultLib["TalentTree"] = CreateStatic("Barotrauma.TalentTree", true)
defaultLib["TalentPrefab"] = CreateStatic("Barotrauma.TalentPrefab", true)
defaultLib["FactionPrefab"] = CreateStatic("Barotrauma.FactionPrefab", true)
defaultLib["Level"] = CreateStatic("Barotrauma.Level")
defaultLib["Job"] = CreateStatic("Barotrauma.Job", true)
defaultLib["JobPrefab"] = CreateStatic("Barotrauma.JobPrefab", true)
defaultLib["AfflictionPrefab"] = CreateStatic("Barotrauma.AfflictionPrefab", true)
defaultLib["ChatMessage"] = CreateStatic("Barotrauma.Networking.ChatMessage")
defaultLib["Structure"] = CreateStatic("Barotrauma.Structure", true)
defaultLib["Hull"] = CreateStatic("Barotrauma.Hull", true)
defaultLib["Gap"] = CreateStatic("Barotrauma.Gap", true)
defaultLib["Signal"] = CreateStatic("Barotrauma.Items.Components.Signal", true)
defaultLib["SubmarineInfo"] = CreateStatic("Barotrauma.SubmarineInfo", true)
defaultLib["Entity"] = CreateStatic("Barotrauma.Entity", true)
defaultLib["Physics"] = CreateStatic("Barotrauma.Physics")
defaultLib["FireSource"] = CreateStatic("Barotrauma.FireSource", true)
defaultLib["TextManager"] = CreateStatic("Barotrauma.TextManager")
defaultLib["NetEntityEvent"] = CreateStatic("Barotrauma.Networking.NetEntityEvent")
defaultLib["Screen"] = CreateStatic("Barotrauma.Screen")
defaultLib["AttackResult"] = CreateStatic("Barotrauma.AttackResult", true)
defaultLib["TempClient"] = CreateStatic("Barotrauma.Networking.TempClient", true)

defaultLib["AIObjective"] = CreateStatic("Barotrauma.AIObjective", true)
defaultLib["AIObjectiveChargeBatteries"] = CreateStatic("Barotrauma.AIObjectiveChargeBatteries", true)
defaultLib["AIObjectiveCleanupItem"] = CreateStatic("Barotrauma.AIObjectiveCleanupItem", true)
defaultLib["AIObjectiveCleanupItems"] = CreateStatic("Barotrauma.AIObjectiveCleanupItems", true)
defaultLib["AIObjectiveCombat"] = CreateStatic("Barotrauma.AIObjectiveCombat", true)
defaultLib["AIObjectiveContainItem"] = CreateStatic("Barotrauma.AIObjectiveContainItem", true)
defaultLib["AIObjectiveDecontainItem"] = CreateStatic("Barotrauma.AIObjectiveDecontainItem", true)
defaultLib["AIObjectiveEscapeHandcuffs"] = CreateStatic("Barotrauma.AIObjectiveEscapeHandcuffs", true)
defaultLib["AIObjectiveExtinguishFire"] = CreateStatic("Barotrauma.AIObjectiveExtinguishFire", true)
defaultLib["AIObjectiveExtinguishFires"] = CreateStatic("Barotrauma.AIObjectiveExtinguishFires", true)
defaultLib["AIObjectiveFightIntruders"] = CreateStatic("Barotrauma.AIObjectiveFightIntruders", true)
defaultLib["AIObjectiveFindDivingGear"] = CreateStatic("Barotrauma.AIObjectiveFindDivingGear", true)
defaultLib["AIObjectiveFindSafety"] = CreateStatic("Barotrauma.AIObjectiveFindSafety", true)
defaultLib["AIObjectiveFixLeak"] = CreateStatic("Barotrauma.AIObjectiveFixLeak", true)
defaultLib["AIObjectiveFixLeaks"] = CreateStatic("Barotrauma.AIObjectiveFixLeaks", true)
defaultLib["AIObjectiveGetItem"] = CreateStatic("Barotrauma.AIObjectiveGetItem", true)
defaultLib["AIObjectiveGoTo"] = CreateStatic("Barotrauma.AIObjectiveGoTo", true)
defaultLib["AIObjectiveIdle"] = CreateStatic("Barotrauma.AIObjectiveIdle", true)
defaultLib["AIObjectiveOperateItem"] = CreateStatic("Barotrauma.AIObjectiveOperateItem", true)
defaultLib["AIObjectiveOperateItem"] = CreateStatic("Barotrauma.AIObjectiveOperateItem", true)
defaultLib["AIObjectivePumpWater"] = CreateStatic("Barotrauma.AIObjectivePumpWater", true)
defaultLib["AIObjectiveRepairItem"] = CreateStatic("Barotrauma.AIObjectiveRepairItem", true)
defaultLib["AIObjectiveRepairItems"] = CreateStatic("Barotrauma.AIObjectiveRepairItems", true)
defaultLib["AIObjectiveRescue"] = CreateStatic("Barotrauma.AIObjectiveRescue", true)
defaultLib["AIObjectiveRescueAll"] = CreateStatic("Barotrauma.AIObjectiveRescueAll", true)
defaultLib["AIObjectiveReturn"] = CreateStatic("Barotrauma.AIObjectiveReturn", true)

local componentsToReference = { "DockingPort", "Door", "GeneticMaterial", "Growable", "Holdable", "LevelResource", "ItemComponent", "ItemLabel", "LightComponent", "Controller", "Deconstructor", "Engine", "Fabricator", "OutpostTerminal", "Pump", "Reactor", "Steering", "PowerContainer", "Projectile", "Repairable", "Rope", "Scanner", "ButtonTerminal", "ConnectionPanel", "CustomInterface", "MemoryComponent", "Terminal", "WifiComponent", "Wire", "TriggerComponent", "ElectricalDischarger", "EntitySpawnerComponent", "ProducedItem", "VineTile", "GrowthSideExtension", "IdCard", "MeleeWeapon", "Pickable", "Propulsion", "RangedWeapon", "RepairTool", "Sprayer", "Throwable", "ItemContainer", "Ladder", "LimbPos", "MiniMap", "OxygenGenerator", "Sonar", "SonarTransducer", "Vent", "NameTag", "Planter", "Powered", "PowerTransfer", "Quality", "RemoteController", "AdderComponent", "AndComponent", "ArithmeticComponent", "ColorComponent", "ConcatComponent", "Connection", "DelayComponent", "DivideComponent", "EqualsComponent", "ExponentiationComponent", "FunctionComponent", "GreaterComponent", "ModuloComponent", "MotionSensor", "MultiplyComponent", "NotComponent", "OrComponent", "OscillatorComponent", "OxygenDetector", "RegExFindComponent", "RelayComponent", "SignalCheckComponent", "SmokeDetector", "StringComponent", "SubtractComponent", "TrigonometricFunctionComponent", "WaterDetector", "XorComponent", "StatusHUD", "Turret", "Wearable", "CustomInterface"
}

defaultLib["Components"] = {}

for key, value in pairs(componentsToReference) do
	defaultLib["Components"][value] = CreateStatic("Barotrauma.Items.Components." .. value, true)
end

defaultLib["Vector2"] = CreateStatic("Microsoft.Xna.Framework.Vector2", true)
defaultLib["Vector3"] = CreateStatic("Microsoft.Xna.Framework.Vector3", true)
defaultLib["Vector4"] = CreateStatic("Microsoft.Xna.Framework.Vector4", true)
defaultLib["Color"] = CreateStatic("Microsoft.Xna.Framework.Color", true)
defaultLib["Point"] = CreateStatic("Microsoft.Xna.Framework.Point", true)
defaultLib["Rectangle"] = CreateStatic("Microsoft.Xna.Framework.Rectangle", true)
defaultLib["Matrix"] = CreateStatic("Microsoft.Xna.Framework.Matrix", true)

return defaultLib
