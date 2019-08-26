using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DocTranslate.WebAPI.Helpers
{
    public class PdfDocumentManager : IReadable
    {
        //obtiene el texto del fichero PDF indicado
        public string GetText(string ficheroPDF)
        {
            StringBuilder textoPDFIndexado = new StringBuilder();
            try
            {
                if (File.Exists(ficheroPDF))
                {
                    PdfReader pdfReader = new PdfReader(ficheroPDF);

                    for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                    {
                        ITextExtractionStrategy strategy =
                            new SimpleTextExtractionStrategy();
                        string currentText =
                            Regex.Replace(PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy).Replace('�','.'), "(?<!\r)\n", "\r\n");

                        currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(
                            Encoding.Default, Encoding.UTF8,
                            Encoding.Default.GetBytes(currentText)));
                        textoPDFIndexado.Append(currentText);
                    }
                    pdfReader.Close();
                }
                return textoPDFIndexado.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}