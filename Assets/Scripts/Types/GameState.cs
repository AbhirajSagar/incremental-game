using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class GameState
{
    public GameState Clone()
    {
        return (GameState)this.MemberwiseClone();
    }

    [Header("Base Combat")]

    /// <summary>
    /// Click Base Damage Before Multipliers
    /// </summary>
    [FormerlySerializedAs("Damage")]
    public float BaseDamage = 1f;

    /// <summary>
    /// Radius of the Attack Detection Sphere Check
    /// </summary>
    [Range(1f, 15f)] public float AttackRadius = 1f;

    [Header("Critical Strikes")]

    /// <summary>0–1 probability of landing a critical hit.</summary>
    [Range(0f, 1f)] public float CritChance = 0f;

    /// <summary>Damage multiplier when a critical hit happens</summary>
    [Range(1.2f, 5f)] public float CritMultiplier = 1.2f;

    // ─────────────────────────────────────────────────────────────────
    [Header("  Conditional Damage")]

    /// <summary>First Strike — damage multiplier applied against fish at exactly 100% health.</summary>
    public float FullHealthDamageMultiplier = 1f;

    /// <summary>Execute — health fraction below which the execute multiplier kicks in.</summary>
    [Range(0f, 0.5f)]
    public float ExecuteHealthThreshold = 0.2f;

    /// <summary>Execute — damage multiplier against fish below ExecuteHealthThreshold.</summary>
    public float ExecuteDamageMultiplier = 1f;

    /// <summary>Boss Hunter — damage multiplier applied specifically to Boss/Large-class fish.</summary>
    public float BossDamageMultiplier = 1f;

    // ─────────────────────────────────────────────────────────────────
    [Header("  Frenzy")]

    /// <summary>Frenzy — damage bonus added per consecutive click within the window (+5% default).</summary>
    [Range(0f, 0.2f)]
    public float FrenzyBonusPerStack = 0.05f;

    /// <summary>Frenzy — maximum stacks before the bonus is capped (10 × 5% = +50%).</summary>
    public int FrenzyMaxStacks = 10;

    /// <summary>Frenzy — seconds within which clicks must occur to build stacks.</summary>
    public float FrenzyWindowSeconds = 1f;

    // ─────────────────────────────────────────────────────────────────
    [Header("  Echo Click")]

    /// <summary>Echo Click — every Nth click fires a free automatic click at the same position.</summary>
    public int EchoClickInterval = 10;

    // ─────────────────────────────────────────────────────────────────
    [Header("  Phantom Harpoon")]

    /// <summary>Phantom Harpoon — ghost damage per second as a fraction of click damage.</summary>
    [Range(0f, 1f)]
    public float PhantomHarpoonDamagePercent = 0.1f;

    /// <summary>Phantom Harpoon — seconds the ghost persists before disappearing.</summary>
    public float PhantomHarpoonDuration = 3f;


    // ═══════════════════════════════════════════════════════════════════════════════
    // TREE 2 ─ PATH OF THE DEEP DIVER   Oxygen & Survival
    // ═══════════════════════════════════════════════════════════════════════════════

    [Header("[ Deep Diver ]  Base Oxygen")]

    /// <summary>Lung Capacity I / II / III — maximum oxygen the player can hold.</summary>
    public int MaxOxygen = 100;

    /// <summary>Efficient Breathing I / II / III — oxygen units drained per second.</summary>
    public float OxygenDrainPerSecond = 1f;

    // ─────────────────────────────────────────────────────────────────
    [Header("[ Deep Diver ]  Restoration")]

    /// <summary>Oxygen on Kill I & II — flat O2 restored each time any fish dies.</summary>
    public float OxygenRestoredPerKill = 0f;

    /// <summary>Symbiosis — extra O2 restored specifically when a Golden Fish dies.</summary>
    public float GoldenFishOxygenRestore = 0f;

    /// <summary>Vampire Fangs — fraction of damage dealt that is converted to O2 (e.g. 0.01 = 1%).</summary>
    [Range(0f, 0.05f)]
    public float VampireFangsPercent = 0f;

    // ─────────────────────────────────────────────────────────────────
    [Header("[ Deep Diver ]  Passive Regen")]

    /// <summary>Gills — O2 restored per regen tick (0 = disabled).</summary>
    public float GillsRegenRate = 0f;

    /// <summary>Gills — seconds between each regen tick.</summary>
    public float GillsRegenInterval = 5f;

    /// <summary>Deep Meditation — seconds the player must not click before regen starts.</summary>
    public float MeditationIdleThreshold = 3f;

    /// <summary>Deep Meditation — O2 regenerated per second while idle (0 = disabled).</summary>
    public float MeditationRegenPerSecond = 0f;

    // ─────────────────────────────────────────────────────────────────
    [Header("[ Deep Diver ]  Bubble Drops")]

    /// <summary>Bubble Drops — per-fish probability of dropping a clickable O2 bubble on death.</summary>
    [Range(0f, 0.2f)]
    public float OxygenBubbleDropChance = 0f;

    /// <summary>Bubble Drops — O2 restored when the player clicks a bubble.</summary>
    public int OxygenBubbleRestoreAmount = 10;

    // ─────────────────────────────────────────────────────────────────
    [Header("[ Deep Diver ]  Filter Feeder")]

    /// <summary>Filter Feeder — O2 gained when clicking on empty water (0 = disabled).</summary>
    public int FilterFeederOxygenGain = 0;

    /// <summary>Filter Feeder — cooldown in seconds between Filter Feeder procs.</summary>
    public float FilterFeederCooldown = 5f;

    // ─────────────────────────────────────────────────────────────────
    [Header("[ Deep Diver ]  Whale Song")]

    /// <summary>Whale Song — cumulative kill count that summons a whale (0 = disabled).</summary>
    public int WhaleSongKillInterval = 50;

    /// <summary>Whale Song — fraction of MaxOxygen restored per whale visit.</summary>
    [Range(0f, 1f)]
    public float WhaleSongOxygenRestorePercent = 0.3f;

    // ─────────────────────────────────────────────────────────────────
    [Header("[ Deep Diver ]  Low-Oxygen Effects")]

    /// <summary>Adrenaline Rush + Panic Thrashing — O2 fraction that counts as 'low' (e.g. 0.2 = 20%).</summary>
    [Range(0f, 0.5f)]
    public float LowOxygenThreshold = 0.2f;

    /// <summary>Adrenaline Rush — damage multiplier applied while O2 is below LowOxygenThreshold.</summary>
    public float AdrenalineDamageMultiplier = 1f;

    /// <summary>Panic Thrashing — attack radius multiplier while O2 is below LowOxygenThreshold.</summary>
    public float PanicRadiusMultiplier = 1f;

    // ─────────────────────────────────────────────────────────────────
    [Header("[ Deep Diver ]  Emergency Mechanics")]

    /// <summary>Last Breath — seconds O2 is paused at zero before the run ends.</summary>
    public float LastBreathPauseDuration = 3f;

    /// <summary>Leviathan's Breath — seconds each kill pauses O2 drain (0 = flag not active).</summary>
    public float LeviathanBreathPauseDuration = 0f;

    // ─────────────────────────────────────────────────────────────────
    [Header("[ Deep Diver ]  Abyssal Contract (Occultist cross-tree)")]

    /// <summary>Abyssal Contract — multiplier applied to MaxOxygen (0.5 = −50%).</summary>
    public float AbyssalContractOxygenMultiplier = 0.5f;

    /// <summary>Abyssal Contract — multiplier applied to BaseDamage (5 = ×5).</summary>
    public float AbyssalContractDamageMultiplier = 5f;


    // ═══════════════════════════════════════════════════════════════════════════════
    // TREE 3 ─ PATH OF THE TYCOON   Economy & Luck
    // ═══════════════════════════════════════════════════════════════════════════════

    [Header("[ Tycoon ]  Sell Price")]

    /// <summary>Haggler I / II / III — flat gold bonus added to every fish's base sell price.</summary>
    public float FishSellPriceBonus = 0f;

    // ─────────────────────────────────────────────────────────────────
    [Header("[ Tycoon ]  Spawn Rates")]

    /// <summary>Shiny Lure I & II — multiplier on the global fish spawn rate.</summary>
    public float FishSpawnRateMultiplier = 1f;

    /// <summary>Chumming I & II — maximum number of fish allowed on screen simultaneously.</summary>
    public int MaxFishOnScreen = 10;

    /// <summary>Golden Hook I / II / III — per-frame spawn probability for Golden Fish.</summary>
    [Range(0f, 0.2f)]
    public float GoldenFishSpawnChance = 0f;

    /// <summary>Diamond Scales — per-frame spawn probability for Diamond Fish (capped at 0.1% by design).</summary>
    [Range(0f, 0.01f)]
    public float DiamondFishSpawnChance = 0f;

    // ─────────────────────────────────────────────────────────────────
    [Header("[ Tycoon ]  Special Fish Values")]

    /// <summary>Gold Rush upgrades this from the default 2× to 3×.</summary>
    public float GoldenFishValueMultiplier = 2f;

    /// <summary>Diamond Scales — sell multiplier for Diamond Fish.</summary>
    public float DiamondFishValueMultiplier = 20f;

    // ─────────────────────────────────────────────────────────────────
    [Header("[ Tycoon ]  Combo System")]

    /// <summary>Combo Multiplier — extra cash percentage added per active combo stack.</summary>
    [Range(0f, 0.2f)]
    public float ComboCashBonusPerStack = 0.05f;

    /// <summary>Combo Mastery extends this from 2 s to 4 s.</summary>
    public float ComboWindowSeconds = 2f;

    // ─────────────────────────────────────────────────────────────────
    [Header("[ Tycoon ]  Proc Effects")]

    /// <summary>Scavenger — probability of a non-lethal hit dropping loose cash.</summary>
    [Range(0f, 0.5f)]
    public float ScavengerDropChance = 0f;

    /// <summary>Scavenger — gold amount dropped per proc.</summary>
    public int ScavengerDropAmount = 1;

    /// <summary>Midas Touch — probability that a nearby normal fish turns Golden after a Golden kill.</summary>
    [Range(0f, 1f)]
    public float MidasTouchChance = 0f;

    // ─────────────────────────────────────────────────────────────────
    [Header("[ Tycoon ]  Bounty Hunter")]

    /// <summary>Bounty Hunter — seconds between bounty fish spawns.</summary>
    public float BountyHunterSpawnInterval = 60f;

    /// <summary>Bounty Hunter — sell multiplier applied to the bounty fish.</summary>
    public float BountyHunterValueMultiplier = 10f;

    // ─────────────────────────────────────────────────────────────────
    [Header("[ Tycoon ]  Treasure Chests")]

    /// <summary>Treasure Chests — per-fish probability of spawning a chest on death.</summary>
    [Range(0f, 0.05f)]
    public float TreasureChestSpawnChance = 0f;

    /// <summary>Treasure Chests — clicks needed to break a chest open.</summary>
    public int TreasureChestClicksToOpen = 10;

    // ─────────────────────────────────────────────────────────────────
    [Header("[ Tycoon ]  Passive & End-of-Dive")]

    /// <summary>Piggy Bank — interest rate applied to lifetime bank balance at dive end (e.g. 0.02 = 2%).</summary>
    [Range(0f, 0.1f)]
    public float PiggyBankInterestRate = 0f;

    /// <summary>Sponsorship — passive gold earned per second, regardless of activity.</summary>
    public float SponsorshipIncomePerSecond = 0f;

    /// <summary>Monopoly — every Nth kill turns all on-screen fish Golden simultaneously.</summary>
    public int MonopolyKillThreshold = 100;


    // ═══════════════════════════════════════════════════════════════════════════════
    // TREE 4 ─ PATH OF THE TRAPPER   Idle & Utility
    // ═══════════════════════════════════════════════════════════════════════════════

    [Header("[ Trapper ]  Auto-Harpoon")]

    /// <summary>Auto-Harpoon I / II / III — automatic shots fired per second (0 = disabled).
    /// Tier I → 0.5, Tier II → 1, Tier III → 2.</summary>
    public float AutoHarpoonFireRate = 0f;

    /// <summary>Harpoon Overclock upgrades this from the default 0.5 to 1.0.</summary>
    [Range(0f, 1f)]
    public float AutoHarpoonDamagePercent = 0.5f;

    // ─────────────────────────────────────────────────────────────────
    [Header("[ Trapper ]  Crowd Control")]

    /// <summary>Heavy Water I & II — multiplier on all fish movement speeds (1 = normal, 0.8 = 20% slower).</summary>
    [Range(0f, 1f)]
    public float FishSpeedMultiplier = 1f;

    /// <summary>Electrified Net — per-click probability of stunning the target fish.</summary>
    [Range(0f, 1f)]
    public float StunOnClickChance = 0f;

    /// <summary>Net Mastery upgrades this from 1 s to 2.5 s.</summary>
    public float StunDuration = 1f;

    /// <summary>Barbed Wire — passive damage per second dealt to all fish inside AttackRadius,
    /// expressed as a fraction of BaseDamage.</summary>
    [Range(0f, 1f)]
    public float BarbedWireDamagePercent = 0f;

    // ─────────────────────────────────────────────────────────────────
    [Header("[ Trapper ]  Damage Over Time")]

    /// <summary>Toxic Coating — poison damage per tick as a fraction of BaseDamage.</summary>
    [Range(0f, 1f)]
    public float PoisonDamagePercent = 0f;

    /// <summary>Toxic Coating — total duration of the poison debuff in seconds.</summary>
    public float PoisonDuration = 3f;

    // ─────────────────────────────────────────────────────────────────
    [Header("[ Trapper ]  Timed Events")]

    /// <summary>Sonar Ping — seconds between radar sweeps (0 = disabled).</summary>
    public float SonarPingInterval = 0f;

    /// <summary>Sonar Ping — seconds fish are frozen after being caught by the sweep.</summary>
    public float SonarPingFreezeDuration = 2f;

    /// <summary>Sea Mines — seconds between mine spawns (0 = disabled).</summary>
    public float SeaMineSpawnInterval = 0f;

    /// <summary>Auto-Chum — seconds between bonus fish spawning near the cursor (0 = disabled).</summary>
    public float AutoChumInterval = 0f;


    // ═══════════════════════════════════════════════════════════════════════════════
    // TREE 5 ─ PATH OF THE OCCULTIST   Chaos & Magic
    // ═══════════════════════════════════════════════════════════════════════════════

    [Header("[ Occultist ]  Area-of-Effect")]

    /// <summary>Corpse Explosion — AoE damage on kill expressed as a fraction of the dead fish's Max HP.</summary>
    [Range(0f, 1f)]
    public float CorpseExplosionDamagePercent = 0f;

    /// <summary>Chain Lightning — extra fish the click damage bounces to.
    /// Chain Lightning = 1, High Voltage upgrades this to 3.</summary>
    public int ChainLightningBounces = 0;

    // ─────────────────────────────────────────────────────────────────
    [Header("[ Occultist ]  Proc Effects")]

    /// <summary>Necromancy — per-kill probability of spawning a friendly Ghost Fish ally.</summary>
    [Range(0f, 0.5f)]
    public float NecromancyChance = 0f;

    /// <summary>Soul Stealer — per-kill probability that the next click is an instant kill.</summary>
    [Range(0f, 0.1f)]
    public float SoulStealerChance = 0f;

    /// <summary>Schrodinger's Fish — per-spawn probability that a fish instantly dies and drops its value.</summary>
    [Range(0f, 0.1f)]
    public float SchrodingerInstantDeathChance = 0f;

    // ─────────────────────────────────────────────────────────────────
    [Header("[ Occultist ]  Per-Kill Scaling")]

    /// <summary>Blood Frenzy — fractional AttackRadius increase gained per kill this dive
    /// (stacks are tracked in GameSession, not here).</summary>
    public float BloodFrenzyRadiusPerKill = 0f;

    // ─────────────────────────────────────────────────────────────────
    [Header("[ Occultist ]  Timed Events")]

    /// <summary>Poseidon's Trident — lightning bolts fire every Nth manual click (0 = disabled).</summary>
    public int PoseidonTridentClickInterval = 0;

    /// <summary>Eldritch Tentacles — seconds between tentacle events that kill the 3 highest-HP fish (0 = disabled).</summary>
    public float EldritchTentaclesInterval = 0f;

    // ─────────────────────────────────────────────────────────────────
    [Header("[ Occultist ]  Alchemist's Greed  (toggled ability)")]

    /// <summary>Alchemist's Greed — O2 drain multiplier while the ability is toggled on.</summary>
    public float AlchemistsGreedOxygenDrainMultiplier = 3f;

    /// <summary>Alchemist's Greed — damage multiplier while toggled on.</summary>
    public float AlchemistsGreedDamageMultiplier = 3f;

    /// <summary>Alchemist's Greed — money multiplier while toggled on.</summary>
    public float AlchemistsGreedMoneyMultiplier = 3f;

    // ─────────────────────────────────────────────────────────────────
    [Header("[ Occultist ]  Sea Witch's Brew")]

    /// <summary>Sea Witch's Brew — global payout multiplier applied when Brew is active.</summary>
    public float SeaWitchBrewPayoutMultiplier = 1.5f;


    // ═══════════════════════════════════════════════════════════════════════════════
    // CUSTOM FLAG UPGRADES
    // Cannot be expressed as simple numbers. Each starts false.
    // UpgradeManager.ApplyNodeOnSession() sets the corresponding flag to true.
    // ═══════════════════════════════════════════════════════════════════════════════

    [Header("[ Flags ]  Tree 1 — Harpooner")]

    [Tooltip("Momentum: +20% damage while the player's cursor is actively moving.")]
    public bool HasMomentum;

    [Tooltip("Sniper: 3× damage when the click lands on the exact centre of a fish.")]
    public bool HasSniper;

    [Tooltip("Piercing: AoE attacks deal full damage to every target instead of dividing it.")]
    public bool HasPiercing;

    [Tooltip("Echo Click: every EchoClickInterval-th click fires an automatic free click at the same spot.")]
    public bool HasEchoClick;

    [Tooltip("Shockwave: killing a fish deals 10% of its Max HP as damage to all fish inside AttackRadius.")]
    public bool HasShockwave;

    [Tooltip("Armor Piercing: click damage ignores any armor/defense stat on Boss-class fish.")]
    public bool HasArmorPiercing;

    [Tooltip("Heavy Hook: click damage scales up with the target fish's missing health percentage.")]
    public bool HasHeavyHook;

    [Tooltip("Tidal Force: each click pushes surviving fish slightly away from the cursor.")]
    public bool HasTidalForce;

    [Tooltip("Vortex Strike: each click pulls surviving fish slightly toward the cursor.")]
    public bool HasVortexStrike;

    [Tooltip("Phantom Harpoon: clicks leave a ghost that deals PhantomHarpoonDamagePercent per second for PhantomHarpoonDuration seconds.")]
    public bool HasPhantomHarpoon;

    [Tooltip("Kraken's Wrath (ULTIMATE): click damage is permanently multiplied by total fish killed this dive.")]
    public bool HasKrakensWrath;

    // ─────────────────────────────────────────────────────────────────
    [Header("[ Flags ]  Tree 2 — Deep Diver")]

    [Tooltip("Second Wind: once per run, automatically restore 50% of MaxOxygen when O2 reaches zero.")]
    public bool HasSecondWind;

    [Tooltip("Depth Adapter: passive O2 drain rate gradually decreases the longer a dive runs.")]
    public bool HasDepthAdapter;

    [Tooltip("Hyperventilation: dives begin at 150% O2; drain proceeds normally once it falls below 100%.")]
    public bool HasHyperventilation;

    [Tooltip("Iron Lungs: MaxOxygen doubles, but all per-kill O2 restoration is halved.")]
    public bool HasIronLungs;

    [Tooltip("Scuba Tank: exposes a UI button that fully restores O2 once per dive.")]
    public bool HasScubaTank;

    [Tooltip("Blood Magic: any O2 restoration that would exceed MaxOxygen is converted into bonus damage instead.")]
    public bool HasBloodMagic;

    [Tooltip("Last Breath: O2 freezes at zero for LastBreathPauseDuration seconds before the run ends.")]
    public bool HasLastBreath;

    [Tooltip("Leviathan's Breath (ULTIMATE): every kill pauses O2 drain for LeviathanBreathPauseDuration seconds.")]
    public bool HasLeviathanBreath;

    // ─────────────────────────────────────────────────────────────────
    [Header("[ Flags ]  Tree 3 — Tycoon")]

    [Tooltip("Appraiser: clicking a fish reveals its HP and exact value, and permanently raises its sell price by 10%.")]
    public bool HasAppraiser;

    [Tooltip("Whale Investor: end-of-dive earnings are multiplied by the fraction of O2 remaining at dive end.")]
    public bool HasWhaleInvestor;

    [Tooltip("Cursed Gold: Golden Fish move 2× faster and have 2× HP, but sell for 5× instead of GoldenFishValueMultiplier.")]
    public bool HasCursedGold;

    [Tooltip("Pirate's Plunder: Boss-class fish spawn 3 Golden Fish on death.")]
    public bool HasPiratesPlunder;

    [Tooltip("Tax Evasion: the player loses no money on death (removes any future money-loss-on-death mechanic).")]
    public bool HasTaxEvasion;

    [Tooltip("Bounty Hunter: enables periodic high-value bounty fish spawns (see BountyHunterSpawnInterval).")]
    public bool HasBountyHunter;

    [Tooltip("Treasure Chests: enables locked chests spawning on fish death (see TreasureChestSpawnChance).")]
    public bool HasTreasureChests;

    [Tooltip("Monopoly (ULTIMATE): every MonopolyKillThreshold kills simultaneously turns all on-screen fish Golden.")]
    public bool HasMonopoly;

    // ─────────────────────────────────────────────────────────────────
    [Header("[ Flags ]  Tree 4 — Trapper")]

    [Tooltip("Targeting AI: Auto-Harpoon targets the fish with the lowest current health instead of a random one.")]
    public bool HasTargetingAI;

    [Tooltip("Venomous Spread: when a poisoned fish dies, the poison jumps to the nearest living fish.")]
    public bool HasVenomousSpread;

    [Tooltip("Magnetizer: all fish are slowly dragged toward the centre of the screen.")]
    public bool HasMagnetizer;

    [Tooltip("Laser Turret: a damage beam sweeps left-to-right across the bottom of the screen continuously.")]
    public bool HasLaserTurret;

    [Tooltip("Decoy Dummy: spawns a fake bait; fish AI targets the bait instead of their normal destination for 5 seconds.")]
    public bool HasDecoyDummy;

    [Tooltip("Freezing Depths: fish spawn with an icy armour shell; breaking it permanently slows that fish by 50%.")]
    public bool HasFreezingDepths;

    [Tooltip("Trawler Net: a net sweeps up the screen, dragging fish to the top edge and bunching them together.")]
    public bool HasTrawlerNet;

    [Tooltip("Electric Eel Pet: a pet eel swims around and permanently stuns any Golden Fish it contacts.")]
    public bool HasElectricEelPet;

    [Tooltip("Drone Collector: automatically clicks Treasure Chests open and collects any physical drops.")]
    public bool HasDroneCollector;

    [Tooltip("Ghost in the Machine (ULTIMATE): Auto-Harpoon fire rate scales with the player's current manual click rate.")]
    public bool HasGhostInTheMachine;

    [Tooltip("Chaos Bolts: Auto-Harpoon fires 3 simultaneous projectiles in random directions each shot.")]
    public bool HasChaosBolts;

    // ─────────────────────────────────────────────────────────────────
    [Header("[ Flags ]  Tree 5 — Occultist")]

    [Tooltip("Void Rift: clicking the same empty spot 5 times rapidly opens a black hole that pulls all fish to the centre.")]
    public bool HasVoidRift;

    [Tooltip("Time Warp: as O2 falls, fish movement slows proportionally — but the player's actions are unaffected.")]
    public bool HasTimeWarp;

    [Tooltip("Alchemist's Greed: player can toggle a mode that applies the Alchemist multipliers to O2 drain, damage, and money.")]
    public bool HasAlchemistsGreed;

    [Tooltip("Abyssal Contract: MaxOxygen × AbyssalContractOxygenMultiplier; BaseDamage × AbyssalContractDamageMultiplier.")]
    public bool HasAbyssalContract;

    [Tooltip("Sea Witch's Brew: fish stats (speed, size, HP) are randomised on spawn; payouts scale with SeaWitchBrewPayoutMultiplier.")]
    public bool HasSeaWitchBrew;

    [Tooltip("Siren Song: fish occasionally freeze, stop pathing, and spin in place for a few seconds.")]
    public bool HasSirenSong;

    [Tooltip("Tidal Wave: grants one spell charge per dive that instantly kills every fish currently on screen.")]
    public bool HasTidalWave;

    [Tooltip("Doppelganger: the player's cursor is mirrored on the opposite side of the screen, doubling every click.")]
    public bool HasDoppelganger;

    [Tooltip("Meteor Swarm: clicking a Boss-class fish causes flaming debris to fall from the top of the screen, hitting other fish.")]
    public bool HasMeteorSwarm;

    [Tooltip("Ascension (ULTIMATE): AttackRadius becomes the entire screen; BaseDamage is applied constantly to all fish.")]
    public bool HasAscension;
}