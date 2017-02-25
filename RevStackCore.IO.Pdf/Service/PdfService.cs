using System;

namespace RevStackCore.IO.Pdf
{
	public class PdfService : IOService
	{
		public PdfService(IPdfRepository repository) : base(repository) { }
	}
}
