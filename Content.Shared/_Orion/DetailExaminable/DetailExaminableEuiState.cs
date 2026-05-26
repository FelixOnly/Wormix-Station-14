using Content.Shared.Eui;
using Content.Shared.Humanoid;
using Content.Shared.Humanoid.Prototypes;
using Content.Shared.SD;
using Robust.Shared.Enums;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using System.Net.NetworkInformation;

namespace Content.Shared._Orion.DetailExaminable;

//
// License-Identifier: GPL-3.0-or-later
//

[Serializable, NetSerializable]
public sealed class DetailExaminableEuiState : EuiStateBase
{
    public NetEntity Target;
    public string Name;
    public ProtoId<SpeciesPrototype> Species;
    public Sex Sex;
    public Gender Gender;
    public EnumERPStatus ERPStatus; // SD-ERP-Status
    public string FlavorText;
    public string OOCFlavorText;
    public string CharacterFlavorText;
    public string GreenFlavorText;
    public string YellowFlavorText;
    public string RedFlavorText;
    public string TagsFlavorText;
    public string LinksFlavorText;
    public string NsfwFlavorText;
    public string NsfwOOCFlavorText;
    public string NsfwLinksFlavorText;
    public string NsfwTagsFlavorText;

    public DetailExaminableEuiState(
        NetEntity target,
        string name,
        ProtoId<SpeciesPrototype> species,
        Sex sex,
        Gender gender,
        EnumERPStatus erpStatus,
        string flavorText,
        string oocFlavorText,
        string characterFlavorText,
        string greenFlavorText,
        string yellowFlavorText,
        string redFlavorText,
        string tagsFlavorText,
        string linksFlavorText,
        string nsfwFlavorText,
        string nsfwOOCFlavorText,
        string nsfwLinksFlavorText,
        string nsfwTagsFlavorText
    )
    {
        Target = target;
        Name = name;
        Species = species;
        Sex = sex;
        Gender = gender;
        ERPStatus = erpStatus; // SD-ERP-Status
        FlavorText = flavorText;
        OOCFlavorText = oocFlavorText;
        CharacterFlavorText = characterFlavorText;
        GreenFlavorText = greenFlavorText;
        YellowFlavorText = yellowFlavorText;
        RedFlavorText = redFlavorText;
        TagsFlavorText = tagsFlavorText;
        LinksFlavorText = linksFlavorText;
        NsfwFlavorText = nsfwFlavorText;
        NsfwOOCFlavorText = nsfwOOCFlavorText;
        NsfwLinksFlavorText = nsfwLinksFlavorText;
        NsfwTagsFlavorText = nsfwTagsFlavorText;
    }
}
