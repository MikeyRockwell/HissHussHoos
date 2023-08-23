using UnityEngine;
using System.Collections.Generic;

namespace Managers {
    public class ProfanityFilter : MonoBehaviour {
        [SerializeField] private List<string> profanityList = new();
        [SerializeField] private List<string> whiteList = new();

        private void Awake() {
            LoadProfanityList();
        }

        private void LoadProfanityList() {
            string path = "Assets/Resources/ProfanityList.txt";

            // Read the text from directly from the test.txt file
            string fileContents = System.IO.File.ReadAllText(path);

            // Split the file contents into a list of strings
            profanityList = new List<string>(fileContents.Split('\n'));
        }

        public bool CheckForProfanity(string input) {
            // Check the input string against the profanity list using regex
            foreach (string word in profanityList) {
                string pattern = "\\b" + word + "\\b"; // \b is a word boundary
                if (System.Text.RegularExpressions.Regex.IsMatch
                        (input, pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                    // Check the whitelist
                    /*foreach (string word2 in whiteList)
                    {
                        string pattern2 = "\\b" + word2 + "\\b"; // \b is a word boundary
                        if (System.Text.RegularExpressions.Regex.IsMatch
                                (input, pattern2, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                        {
                            return false;
                        }
                    }*/
                    return true;
            }

            return false;
        }
    }
}