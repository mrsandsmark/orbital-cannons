using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

public static class SettingsWidgets
{
    public static bool NearlyEquals(this float a, float b, float tolerance = 0.01f)
    {
        return Math.Abs(a - b) < tolerance;
    }

    public static void CreateSettingsSlider(Listing_Standard listingStandard, string labelName, ref float value, ref string buffer, float min = 0f, float max = 10f, Func<float, string> valueFormatter = null)
    {
        float result;
        if (buffer == null)
        {
            buffer = SetBufferFromValue(value, valueFormatter);
        }
        else if (!float.TryParse(buffer, out result) || !NearlyEquals(result, value))
        {
            buffer = SetBufferFromValue(value, valueFormatter);
        }
        Rect rect = listingStandard.GetRect(Text.LineHeight);
        float width = rect.width * 0.46f;
        float width2 = rect.width * 0.45f;
        float width3 = rect.width * 0.09f;
        Rect rect2 = new Rect(rect.x, rect.y, width, rect.height);
        Rect rect3 = new Rect(rect2.xMax, rect.y, width2, rect.height);
        Rect rect4 = new Rect(rect3.xMax, rect.y, width3, rect.height);
        Widgets.Label(rect2, labelName);
        value = Widgets.HorizontalSlider(rect3, value, min, max, middleAlignment: true);
        Widgets.TextFieldNumeric(rect4, ref value, ref buffer, min, max);
        static string SetBufferFromValue(float arg, Func<float, string> func)
        {
            return (func != null) ? func(arg) : arg.ToString("F1");
        }
    }

    public static void CreateSettingCheckbox(Listing_Standard listingStandard, string labelName, ref bool value)
    {
        Rect rect = listingStandard.GetRect(Text.LineHeight);
        float width = rect.width * 0.9f;
        float width2 = rect.width * 0.1f;
        Rect rect2 = new Rect(rect.x, rect.y, width, rect.height);
        Rect rect3 = new Rect(rect2.xMax, rect.y, width2, rect.height);
        Widgets.Label(rect2, labelName);
        Widgets.Checkbox(rect3.position, ref value);
    }

    public static void CreateSettingsDropdown<T>(Listing_Standard listingStandard, string labelName, T currentValue, Action<T> setValue, T[] options)
    {
        Rect rect = listingStandard.GetRect(Text.LineHeight);
        float labelWidth = rect.width * 0.46f;
        float dropdownWidth = rect.width * 0.54f; 
        Rect labelRect = new Rect(rect.x, rect.y, labelWidth, rect.height);
        Rect dropdownRect = new Rect(labelRect.xMax, rect.y, dropdownWidth, rect.height);

        Widgets.Label(labelRect, labelName);

        // Create a button for the dropdown (displays the current value)
        string buttonLabel = currentValue?.ToString() ?? "None"; // Fallback if value is null
        if (Widgets.ButtonText(dropdownRect, buttonLabel))
        {
            // Create a float menu 
            List<FloatMenuOption> floatOptions = new List<FloatMenuOption>();
            foreach (T option in options)
            {
                T tempOption = option; // Capture option 
                floatOptions.Add(new FloatMenuOption(
                    label: tempOption?.ToString() ?? "None", // Display string for the option
                    action: () =>
                    {
                        setValue(tempOption); // Call the setter callback to update the value
                        Verse.Log.Message($"{labelName} set to: {tempOption}"); // Debug log
                    }
                ));
            }
            // Open the dropdown menu
            Find.WindowStack.Add(new FloatMenu(floatOptions));
        }

        listingStandard.Gap(listingStandard.verticalSpacing);
    }
}