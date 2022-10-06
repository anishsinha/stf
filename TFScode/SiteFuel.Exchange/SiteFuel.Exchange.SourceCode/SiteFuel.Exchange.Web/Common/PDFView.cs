using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Common
{
    public class PDFView
    {
        public static ActionResult GetPartialViewAsPdf(string partialViewName, string fileNameWithoutExtension, object pdfModel, string masterName, bool AddPortraitOrientation = true)
        {
            var partialPdfView = new Rotativa.PartialViewAsPdf(partialViewName, pdfModel);
            partialPdfView.MasterName = masterName;
            if (AddPortraitOrientation)
                partialPdfView.PageOrientation = Rotativa.Options.Orientation.Portrait;
            partialPdfView.FileName = fileNameWithoutExtension + ".pdf";
            return partialPdfView;
        }

        public static byte[] GetPartialViewPdfContent(string partialViewName, ControllerContext context, object pdfModel,  string masterName)
        {
            var partialPdfView = new Rotativa.PartialViewAsPdf(partialViewName, pdfModel);
            if (!string.IsNullOrEmpty(masterName))
                partialPdfView.MasterName = masterName;
            //partialPdfView.PageOrientation = Rotativa.Options.Orientation.Landscape;
            return partialPdfView.BuildFile(context);
        }
    }
}