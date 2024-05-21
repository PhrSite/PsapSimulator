/////////////////////////////////////////////////////////////////////////////////////
//  File:   IdentitySettings.cs                                     7 Feb 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator.Settings;

/// <summary>
/// NG9-1-1 identity settings
/// </summary>
public class IdentitySettings
{
    /// <summary>
    /// Default setting for the Agency ID
    /// </summary>
    public const string AgencyIDDefault = "ng911test.net";
    /// <summary>
    /// Default setting for the Agent ID
    /// </summary>
    public const string AgentIDDefault = "psapsimulator1@ng911test.net";
    /// <summary>
    /// Default setting for the Element ID
    /// </summary>
    public const string ElementIDDefault = "psapsimulator1.ng911test.net";

    /// <summary>
    /// Agency identifier
    /// </summary>
    public string AgencyID { get; set; } = AgencyIDDefault;
    /// <summary>
    /// Agent identifier
    /// </summary>
    public string AgentID { get; set; } = AgentIDDefault;
    /// <summary>
    /// Element identifier
    /// </summary>
    public string ElementID { get; set; } = ElementIDDefault;

    /// <summary>
    /// Constructor
    /// </summary>
    public IdentitySettings() 
    {
    }
}
