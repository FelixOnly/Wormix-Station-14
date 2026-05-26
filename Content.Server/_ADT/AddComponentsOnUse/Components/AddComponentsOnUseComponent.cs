using Robust.Shared.Prototypes;

namespace Content.Server._ADT.AddComponentsOnUse;

[RegisterComponent]
public sealed partial class AddComponentsOnUseComponent : Component
{
    /// <summary>
    /// The components to add when activated.
    /// </summary>
    [DataField(required: true)]
    public ComponentRegistry Components = new();

    [DataField]
    public bool DeleteOnUse = true;
}
