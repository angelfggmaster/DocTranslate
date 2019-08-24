using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Word = Microsoft.Office.Interop.Word;

namespace DocTranslate.WebAPI.Helpers
{
    public class WordDocumentManager
    {
        public static IList<string> GetParagraphs(string docPath)
        {
            object oMiss = Missing.Value;
            object file = docPath;
            object onlyread = true;
            object visible = false;

            Word.Application word = new Word.Application();

            Word.Document oDoc = word.Documents.Open(ref file, ref oMiss, ref onlyread, ref oMiss, ref oMiss, ref oMiss, ref oMiss,
                ref oMiss, ref oMiss, ref oMiss, ref oMiss, ref visible, ref oMiss, ref oMiss, ref oMiss, ref oMiss);

            IList<string> paragraphs = new List<string>();

            for (int i = 0; i < 200; i++)
            {
                string temp = oDoc.Paragraphs[i + 1].Range.Text.Trim();
                if (temp != string.Empty)
                    paragraphs.Add(temp);
            }

            oDoc.Close();
            word.Quit();

            return paragraphs;
        }
    }
}