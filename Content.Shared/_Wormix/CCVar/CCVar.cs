using Robust.Shared.Configuration;
using static Content.Shared._Wormix.ChatsanPlus.ChatsanPlusSystem;

namespace Content.Shared._Wormix.CCVar;

[CVarDefs]
public sealed partial class CCVar
{
    public static readonly CVarDef<string> ShortArchive = CVarDef.Create("Short.archive", "", CVar.CLIENTONLY | CVar.ARCHIVE);
}
