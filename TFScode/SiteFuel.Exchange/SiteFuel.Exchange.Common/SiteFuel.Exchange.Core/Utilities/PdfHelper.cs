using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.IO;
using System.Web;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Core.StringResources;
using PdfSharp.Pdf.IO;
using System.Collections.Generic;

namespace SiteFuel.Exchange.Utilities
{
    public class PdfHelper
    {
        public static PdfHelper Instance { get; } = new PdfHelper();
        public static string tempImagePath = HttpContext.Current.Server.MapPath(@"\Content\AdditionalImages\");
        public void SaveImageAsPdf(PdfDocument document, string imageFileName, int width = 600)
        {
            try
            {
                PdfPage page = document.AddPage();
                using (XImage img = XImage.FromFile(imageFileName))
                {
                    // Calculate new height to keep image ratio
                    var height = (int)(((double)width / (double)img.PixelWidth) * img.PixelHeight);

                    // Change PDF Page size to match image
                    page.Width = width;
                    page.Height = height;

                    // draw image on page
                    using (XGraphics gfx = XGraphics.FromPdfPage(page))
                    {
                        gfx.DrawImage(img, 0, 0, width, height);
                    }
                }
                page.Close();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PdfHelper", "SaveImageAsPdf", ex.Message, ex);
            }
        }

        public void SetStreamInImageModel(HttpPostedFileBase[] files, int userId, dynamic imageViewModel)
        {
            if (files == null || files.Length == 0)
                return;

            var document = new PdfDocument();
            try
            {
                RemoveTempFiles();
                // save posted file to local folder
                SaveFiles(files);
                
                // set pdf doc file name
                var fName = GenerateFileName(userId, ".pdf");
                var docFile = Path.Combine(tempImagePath, fName);

                // create pdf document
                foreach (var file in files)
                {
                    if (file != null)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(tempImagePath, fileName);
                        var fileExtension = Path.GetExtension(fileName);
                        // open doc if exists
                        if (File.Exists(docFile))
                            document = PdfReader.Open(docFile, PdfDocumentOpenMode.Modify);

                        // for pdf
                        if (fileExtension == ".pdf")
                        {
                            using (PdfDocument inputPDFDocument = PdfReader.Open(filePath, PdfDocumentOpenMode.Import))
                            {
                                foreach (PdfPage page in inputPDFDocument.Pages)
                                {
                                    document.AddPage(page);
                                    page.Close();
                                }

                                document.Save(docFile);
                                document.Close();
                                inputPDFDocument.Close();
                            }
                        }
                        else
                        {
                            // convert image to pdf and add to pdf document
                            SaveImageAsPdf(document, filePath);
                            document.Save(docFile);
                            document.Close();
                        }

                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                    }
                }

                // set image model details
                if (File.Exists(docFile))
                    document = PdfReader.Open(docFile, PdfDocumentOpenMode.Modify);

                var memoryStream = GetStream(docFile);
                imageViewModel.InputStream = memoryStream;
                imageViewModel.Name = fName;
                imageViewModel.IsPdf = true;
                imageViewModel.Data = memoryStream.ToArray();
                imageViewModel.Id = 0;

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PdfHelper", "SetStreamInImageModel", ex.Message, ex);
            }
            finally
            {
                document.Close();
                document.Dispose();
            }
        }

        private string GenerateFileName(int userId, string fileName)
        {
            return string.Concat(values: userId + Resource.lblSingleHyphen + DateTime.Now.Ticks + fileName);
        }

        private MemoryStream GetStream(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                MemoryStream ms = new MemoryStream();

                fs.CopyTo(ms);
                return ms;
            }
        }

        private void SaveFiles(HttpPostedFileBase[] files)
        {
            foreach (var file in files)
            {
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    bool isExists = System.IO.Directory.Exists(tempImagePath);
                    if (!isExists)
                        System.IO.Directory.CreateDirectory(tempImagePath);

                    var path = Path.Combine(tempImagePath, fileName);
                    file.SaveAs(path);
                }
            }
        }

        public void RemoveTempFiles()
        {
            try
            {
                bool isExists = System.IO.Directory.Exists(tempImagePath);
                if (isExists)
                {
                    var directoyinfo = new DirectoryInfo(tempImagePath);
                    foreach (var item in directoyinfo.GetFiles())
                    {
                        item.Delete();
                    }
                }
            }
            catch
            {
            }
        }
    }
}
