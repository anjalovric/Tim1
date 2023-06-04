using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.ReportPatterns
{
    public class OwnerReportPattern : ReportGenerator

    {
        public override void GenerateConclusion()
        {
           
        }

        public override void GenerateContent()
        {
            
        }

        public override void GenerateTableContent()
        {
            
        }

        public override void GenerateTitle()
        {
            
        }

        public override void SavePdf()
        {
            Document.Save("/Resources/Reports/OwnerReport.pdf");
            Document.Close(true);
        }
    }
}
