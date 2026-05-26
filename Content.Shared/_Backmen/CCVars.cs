using Robust.Shared.Configuration;

namespace Content.Shared._Backmen.CCVar;

// ReSharper disable once InconsistentNaming
[CVarDefs]
public sealed partial class CCVars
{
    public static readonly CVarDef<bool>
        GameBarotraumaEnabled = CVarDef.Create("game.barotrauma", true, CVar.SERVERONLY);

    public static readonly CVarDef<bool>
        GameDiseaseEnabled = CVarDef.Create("game.disease", true, CVar.SERVERONLY);

    /// <summary>
    /// Whether the Shipyard is enabled.
    /// </summary>
    public static readonly CVarDef<bool> Shipyard =
        CVarDef.Create("shuttle.shipyard", true, CVar.SERVERONLY);

    public static readonly CVarDef<bool>
        EconomyWagesEnabled = CVarDef.Create("economy.wages_enabled", true, CVar.SERVERONLY);

    public static readonly CVarDef<bool>
        WhitelistRolesEnabled = CVarDef.Create("game.whitelist_role_enabled", true, CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    /// Shipwrecked
    /// </summary>
    public static readonly CVarDef<int> ShipwreckedMaxPlayers =
        CVarDef.Create("shipwrecked.max_players", 15);


    /*
     * SpecForces
     */
    public static readonly CVarDef<int> SpecForceDelay =
        CVarDef.Create("specforce.delay", 2, CVar.SERVERONLY);


    /// <summary>
    /// Default volume setting of the boombox audio
    /// </summary>
    public static readonly CVarDef<float> BoomboxVolume =
        CVarDef.Create("boombox.volume", 0.5f, CVar.CLIENTONLY | CVar.ARCHIVE);

    /*
     * Bind Standing - Ataraxia
     */

    public static readonly CVarDef<bool> AutoGetUp =
        CVarDef.Create("laying.auto_get_up", true, CVar.CLIENT | CVar.ARCHIVE | CVar.REPLICATED);

    public static readonly CVarDef<bool> OfferModeIndicatorsPointShow =
        CVarDef.Create("hud.offer_mode_indicators_point_show", true, CVar.ARCHIVE | CVar.CLIENTONLY);

    public static readonly CVarDef<bool> HoldLookUp =
        CVarDef.Create("white.hold_look_up", false, CVar.CLIENT | CVar.ARCHIVE);

  
}
