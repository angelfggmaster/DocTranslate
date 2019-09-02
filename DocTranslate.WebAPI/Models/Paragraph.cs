namespace DocTranslate.WebAPI.Models
{
    public class Paragraph
    {
        public int ParagraphId { get; set; }
        public string OriginLanguage { get; set; }
        public string TargetLanguage { get; set; }
    }
}