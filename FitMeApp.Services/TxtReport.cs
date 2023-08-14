using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FitMeApp.Services.Contracts.Interfaces;

namespace FitMeApp.Services
{
    public class TxtReport: ITxtService
    {
        public async Task<string> GetTextContentFromFileAsync(string localPath)
        {
            FileInfo fileInfo = new FileInfo(localPath);
            string pathToTextMessage = fileInfo.FullName;
            string text = string.Empty;

            using (FileStream fstream = new FileStream(pathToTextMessage, FileMode.Open))
            {
                byte[] buffer = new byte[fstream.Length];
                await fstream.ReadAsync(buffer, 0, buffer.Length);
                text = Encoding.Default.GetString(buffer);
            }

            return text;
        }


        public string GetSpecifiedSectionFromFile(string localPath, string sectionStartMarker,
            string sectionEndMarker)
        {
            bool isInSection = false;
            var sectionContent = new StringBuilder();
            using (StreamReader reader = new StreamReader(localPath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains(sectionStartMarker, StringComparison.CurrentCultureIgnoreCase))
                    {
                        isInSection = true;
                        continue;
                    }

                    if (line.Contains(sectionEndMarker, StringComparison.CurrentCultureIgnoreCase))
                    {
                        isInSection = false;
                        break;
                    }

                    if (isInSection)
                    {
                        sectionContent.AppendLine(line);
                    }
                }
            }

            string sectionText = sectionContent.ToString();

            return sectionText;
        }


        public IEnumerable<string> SplitTextIntoParagraphs(string text, string splitMark)
        {
            var paragraphs = new List<string>();
            if (!string.IsNullOrEmpty(text))
            {
                paragraphs = Regex.Split(text, splitMark).Where(p => p.Any(char.IsLetterOrDigit)).ToList();
            }

            return paragraphs;
        }

    }
}
