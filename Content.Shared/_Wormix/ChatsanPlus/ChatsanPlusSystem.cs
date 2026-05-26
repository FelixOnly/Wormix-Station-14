using Content.Shared.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Content.Shared._Wormix.ChatsanPlus;
public static class ChatsanPlusSystem
{
    public record Short
    {
        public required string Trigger { get; set; }
        public required string Reaction { get; set; }
        public ChatSelectChannel Channel { get; set; }
    };

    public static List<Short> BasicShorts = new List<Short>()
    {
        new Short() { Trigger = "гв", Reaction = "глав врач",       Channel = Chat.ChatSelectChannel.Local},
        new Short() { Trigger = "гву", Reaction = "глав врачу",     Channel = Chat.ChatSelectChannel.Local},
        new Short() { Trigger = "гва", Reaction = "глав врача",     Channel = Chat.ChatSelectChannel.Local},
        new Short() { Trigger = "км", Reaction = "квартирмейстер",  Channel = Chat.ChatSelectChannel.Local},
        new Short() { Trigger = "кэп", Reaction = "капитан"      ,  Channel = Chat.ChatSelectChannel.Local},
        new Short() { Trigger = "гсб", Reaction = "глава службы безопастности", Channel = Chat.ChatSelectChannel.Local},
        new Short() { Trigger = "хос", Reaction = "глава службы безопастности", Channel = Chat.ChatSelectChannel.Local},
        new Short() { Trigger = "сщ", Reaction = "синий щит", Channel = Chat.ChatSelectChannel.Local},
        new Short() { Trigger = "осщ", Reaction = "офицер синий щит", Channel = Chat.ChatSelectChannel.Local},
        new Short() { Trigger = ")", Reaction = "улыбается",        Channel = Chat.ChatSelectChannel.Emotes},
        new Short() { Trigger = "(", Reaction = "хмурится",         Channel = Chat.ChatSelectChannel.Emotes},

    };


    // За такую хуету мне гореть в аду, но я просто не смог найти альтернатив, в 4 утра... Простите, пожалуйста

    private static string Escape(string text)
    {
        return text
            .Replace("\\", "\\\\")
            .Replace("~", "\\~")
            .Replace(";", "\\;");
    }

    private static string Unescape(string text)
    {
        return text
            .Replace("\\;", ";")
            .Replace("\\~", "~")
            .Replace("\\\\", "\\");
    }

    public static string Serialize(List<Short> list)
    {
        // Format:
        // Trigger~Reaction~Channel;Trigger~Reaction~Channel

        return string.Join(";",
            list.Select(x =>
                $"{Escape(x.Trigger)}~{Escape(x.Reaction)}~{(int) x.Channel}"));
    }

    public static List<Short> Deserialize(string data)
    {
        List<Short> result = new();

        if (string.IsNullOrWhiteSpace(data))
            return result;

        foreach (var item in data.Split(';'))
        {
            var parts = item.Split('~');

            if (parts.Length != 3)
                continue;

            result.Add(new Short
            {
                Trigger = Unescape(parts[0]),
                Reaction = Unescape(parts[1]),
                Channel = (ChatSelectChannel) int.Parse(parts[2])
            });
        }

        return result;
    }

}
