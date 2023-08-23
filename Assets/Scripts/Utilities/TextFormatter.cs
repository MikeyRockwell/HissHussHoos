using UnityEngine;
using System.Text.RegularExpressions;

namespace Utils {
    public static class TextFormatter {
        // Define formatting rules
        private static string boldPattern = @"\*(.*?)\*"; // Matches text surrounded by asterisks
        private static string italicPattern = @"_(.*?)_"; // Matches text surrounded by underscores
        private static string boldColor = "#FF3189";
        private static string italicColor = "#14FF00";

        // Format the dialogue text
        public static string FormatDialogue(string rawText) {
            // Apply bold formatting
            string formattedText =
                Regex.Replace(rawText, boldPattern, "<b><color=" + boldColor + ">$1</color></b>");

            // Apply italic formatting
            formattedText =
                Regex.Replace(formattedText, italicPattern, "<i><color=" + italicColor + ">$1</color></i>");

            // Assign the formatted text to the TextMeshPro component
            return formattedText;
        }
    }
}