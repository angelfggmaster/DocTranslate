using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.IO;
using System.Text;

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
                        ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                        string currentText =
                            PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy).Replace('�', '.').Trim();

                        ////currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(
                        ////    Encoding.Default, Encoding.UTF8,
                        ////    Encoding.Default.GetBytes(currentText)));
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

        //public string GetText(string PDFFile)
        //{
        //    try
        //    {
        //        PdfReader reader = new PdfReader(PDFFile);
        //        string rextResult = string.Empty;

        //        for (int page = 1; page <= reader.NumberOfPages; page++)
        //        {
        //            rextResult += ExtractTextFromPDFBytes(reader.GetPageContent(page)) + " ";
        //        }

        //        return rextResult;
        //    }
        //    catch
        //    {
        //        return string.Empty;
        //    }
        //}

        //private string ExtractTextFromPDFBytes(byte[] input)
        //{
        //    if (input == null || input.Length == 0) return string.Empty;

        //    try
        //    {
        //        string resultString = string.Empty;

        //        bool inTextObject = false;

        //        bool nextLiteral = false;

        //        int bracketDepth = 0;

        //        char[] previousCharacters = new char[_numberOfCharsToKeep];
        //        for (int j = 0; j < _numberOfCharsToKeep; j++) previousCharacters[j] = ' ';

        //        for (int i = 0; i < input.Length; i++)
        //        {
        //            char c = (char)input[i];

        //            if (inTextObject)
        //            {
        //                // Position the text
        //                if (bracketDepth == 0)
        //                {
        //                    if (CheckToken(new string[] { "TD", "Td" }, previousCharacters))
        //                    {
        //                        resultString += "\n\r";
        //                    }
        //                    else
        //                    {
        //                        if (CheckToken(new string[] { "'", "T*", "\"" }, previousCharacters))
        //                        {
        //                            resultString += "\n";
        //                        }
        //                        else
        //                        {
        //                            if (CheckToken(new string[] { "Tj" }, previousCharacters))
        //                            {
        //                                resultString += " ";
        //                            }
        //                        }
        //                    }
        //                }

        //                // End of a text object, also go to a new line.
        //                if (bracketDepth == 0 &&
        //                    CheckToken(new string[] { "ET" }, previousCharacters))
        //                {

        //                    inTextObject = false;
        //                    resultString += " ";
        //                }
        //                else
        //                {
        //                    // Start outputting text
        //                    if ((c == '(') && (bracketDepth == 0) && (!nextLiteral))
        //                    {
        //                        bracketDepth = 1;
        //                    }
        //                    else
        //                    {
        //                        // Stop outputting text
        //                        if ((c == ')') && (bracketDepth == 1) && (!nextLiteral))
        //                        {
        //                            bracketDepth = 0;
        //                        }
        //                        else
        //                        {
        //                            // Just a normal text character:
        //                            if (bracketDepth == 1)
        //                            {
        //                                // Only print out next character no matter what. 
        //                                // Do not interpret.
        //                                if (c == '\\' && !nextLiteral)
        //                                {
        //                                    nextLiteral = true;
        //                                }
        //                                else
        //                                {
        //                                    if (((c >= ' ') && (c <= '~')) ||
        //                                        ((c >= 128) && (c < 255)))
        //                                    {
        //                                        resultString += c.ToString();
        //                                    }

        //                                    nextLiteral = false;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //            // Store the recent characters for 
        //            // when we have to go back for a checking
        //            for (int j = 0; j < _numberOfCharsToKeep - 1; j++)
        //            {
        //                previousCharacters[j] = previousCharacters[j + 1];
        //            }
        //            previousCharacters[_numberOfCharsToKeep - 1] = c;

        //            // Start of a text object
        //            if (!inTextObject && CheckToken(new string[] { "BT" }, previousCharacters))
        //            {
        //                inTextObject = true;
        //            }
        //        }
        //        return resultString;
        //    }
        //    catch
        //    {
        //        return "";
        //    }
        //}

        //private bool CheckToken(string[] tokens, char[] recent)
        //{
        //    foreach (string token in tokens)
        //    {
        //        if ((recent[_numberOfCharsToKeep - 3] == token[0]) &&
        //            (recent[_numberOfCharsToKeep - 2] == token[1]) &&
        //            ((recent[_numberOfCharsToKeep - 1] == ' ') ||
        //            (recent[_numberOfCharsToKeep - 1] == 0x0d) ||
        //            (recent[_numberOfCharsToKeep - 1] == 0x0a)) &&
        //            ((recent[_numberOfCharsToKeep - 4] == ' ') ||
        //            (recent[_numberOfCharsToKeep - 4] == 0x0d) ||
        //            (recent[_numberOfCharsToKeep - 4] == 0x0a))
        //            )
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}
    }
}