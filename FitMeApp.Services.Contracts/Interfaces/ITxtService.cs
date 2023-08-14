using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface ITxtService
    {
        Task<string> GetTextContentFromFileAsync(string localPath);
        string GetSpecifiedSectionFromFile(string localPath, string sectionStartMarker, string sectionEndMarker);
        IEnumerable<string> SplitTextIntoParagraphs(string text, string splitMark);
    }
}
