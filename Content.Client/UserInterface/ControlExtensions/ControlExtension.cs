// SPDX-FileCopyrightText: 2023 Hebi <spiritbreakz@gmail.com>
// SPDX-FileCopyrightText: 2025 Aiden <28298836+Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Client._Wormix.Searching;
using Content.Client.Guidebook.Controls;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using System.Text.RegularExpressions;

namespace Content.Client.UserInterface.ControlExtensions;

public static class ControlExtension
{
    public static List<T> GetControlOfType<T>(this Control parent) where T : Control
    {
        return parent.GetControlOfType<T>(typeof(T).Name, false);
    }
    public static List<T> GetControlOfType<T>(this Control parent, string childType) where T : Control
    {
        return parent.GetControlOfType<T>(childType, false);
    }

    public static List<T> GetControlOfType<T>(this Control parent, bool fullTreeSearch) where T : Control
    {
        return parent.GetControlOfType<T>(typeof(T).Name, fullTreeSearch);
    }

    public static List<T> GetControlOfType<T>(this Control parent, string childType, bool fullTreeSearch) where T : Control
    {
        List<T> controlList = new List<T>();

        foreach (var child in parent.Children)
        {
            var isType = child.GetType().Name == childType;
            var hasChildren = child.ChildCount > 0;

            var searchDeeper = hasChildren && !isType;

            if (isType)
            {
                controlList.Add((T) child);
            }

            if (fullTreeSearch || searchDeeper)
            {
                controlList.AddRange(child.GetControlOfType<T>(childType, fullTreeSearch));
            }
        }

        return controlList;
    }

    public static List<ISearchableControl> GetSearchableControls(this Control parent, bool fullTreeSearch = false)
    {
        List<ISearchableControl> controlList = new List<ISearchableControl>();

        foreach (var child in parent.Children)
        {
            var hasChildren = child.ChildCount > 0;
            var searchDeeper = hasChildren && child is not ISearchableControl;

            if (child is ISearchableControl searchableChild)
            {
                controlList.Add(searchableChild);
            }

            if (fullTreeSearch || searchDeeper)
            {
                controlList.AddRange(child.GetSearchableControls(fullTreeSearch));
            }
        }

        return controlList;
    }

    public static bool ChildrenContainText(this Control parent, string search)
    {
        var labels = parent.GetControlOfType<Label>();
        var richTextLabels = parent.GetControlOfType<RichTextLabel>();

        foreach (var label in labels)
        {
            if (label.Text != null && label.Text.Contains(search, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        // Wormix edit start

        foreach (var label in richTextLabels)
        {

            var dirtyLabel = label.GetMessage();

            if (dirtyLabel == null)
                continue;

            string text = FuzzySearching.TrimTags(dirtyLabel).ToLower();


            if (text.Contains(search.ToLower(), StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (text.Contains("\n"))
            {
                var reagents = text.Split(
                    new[] { '\r', '\n' },
                    StringSplitOptions.RemoveEmptyEntries);

                foreach (var reagent in reagents)
                {
                    var cleaned = Regex.Replace(reagent, @"\s*\[\d+\]", "")
                            .Trim();

                    int distance = FuzzySearching.LevensteinAlgorithm(
                        cleaned.ToLower(),
                        search.ToLower());

                    if (distance <= 3)
                    {
                        return true;
                    }
                }

            }

            if (FuzzySearching.LevensteinAlgorithm(text.ToLower(), search) <= 3)
            {
                return true;
            }
        }

        // Wormix edit end
        return false;
    }
}
