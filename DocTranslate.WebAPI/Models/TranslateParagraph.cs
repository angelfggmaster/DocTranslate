using System.Collections.Generic;

namespace DocTranslate.WebAPI.Models
{
    public class TranslateParagraph
    {
        public List<string> OriginLanguage { get; set; }
        public List<string> TargetLanguage { get; set; }
    }
}