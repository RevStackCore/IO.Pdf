using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace RevStackCore.IO.Pdf
{
	public class PdfDbContext
	{
		private const string RASTERIZEJS = "rasterize.js";
		private string _executablePath;
		private string _filePath;
		private string _rasterizeScriptPath;
		private bool _debug;
        private bool _deleteFile;
		public PdfDbContext(string executablePath)
		{
			_executablePath = executablePath;
			_filePath = Path.Combine(_executablePath, "pdfs");
			_rasterizeScriptPath = RASTERIZEJS;
			_debug = false;
            _deleteFile = true;
		}

		public PdfDbContext(string executablePath, bool debug)
		{
			_executablePath = executablePath;
			_filePath = Path.Combine(_executablePath, "pdfs");
			_rasterizeScriptPath = RASTERIZEJS;
			_debug = debug;
            _deleteFile = true;
		}

        public PdfDbContext(string executablePath, bool debug, bool deleteFile)
        {
            _executablePath = executablePath;
            _filePath = Path.Combine(_executablePath, "pdfs");
            _rasterizeScriptPath = RASTERIZEJS;
            _debug = debug;
            _deleteFile = deleteFile;
        }

		public PdfDbContext(string executablePath, string filePath)
		{
			_executablePath = executablePath;
			_filePath = filePath;
			_rasterizeScriptPath = RASTERIZEJS;
			_debug = false;
            _deleteFile = true;
		}

		public PdfDbContext(string executablePath, string filePath, bool debug, bool deleteFile)
		{
			_executablePath = executablePath;
			_filePath = filePath;
			_rasterizeScriptPath = RASTERIZEJS;
			_debug = debug;
            _deleteFile = deleteFile;
		}

		public PdfDbContext(string executablePath, string filePath, string rasterizeScriptPath)
		{
			_executablePath = executablePath;
			_filePath = filePath;
			_rasterizeScriptPath = Path.Combine(rasterizeScriptPath, RASTERIZEJS);
			_debug = false;
            _deleteFile = true;
		}

		public PdfDbContext(string executablePath, string filePath, string rasterizeScriptPath, bool debug)
		{
			_executablePath = executablePath;
			_filePath = filePath;
			_rasterizeScriptPath = Path.Combine(rasterizeScriptPath, RASTERIZEJS);
			_debug = debug;
            _deleteFile = true;
		}

        public PdfDbContext(string executablePath, string filePath, string rasterizeScriptPath, bool debug, bool deleteFile)
        {
            _executablePath = executablePath;
            _filePath = filePath;
            _rasterizeScriptPath = Path.Combine(rasterizeScriptPath, RASTERIZEJS);
            _debug = debug;
            _deleteFile = deleteFile;
        }

		public string GeneratePdf(string url)
		{
			string fileName = Guid.NewGuid().ToString() + ".pdf"; //random name for temp disk document
			string filePath = Path.Combine(_filePath, fileName);
			string args = _rasterizeScriptPath + " " + url + " " + filePath;
			string executableFile = Path.Combine(_executablePath, "phantomjs");
			if (_executablePath == "/")
			{
				executableFile = "phantomjs";
			}

			var start = new ProcessStartInfo
			{
				FileName = executableFile,
				Arguments = args,
				UseShellExecute = false, // needs to be false in order to redirect output
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				RedirectStandardInput = true, // redirect all 3, as it should be all 3 or none
				WorkingDirectory = _executablePath
			};


			int returnCode = 0;
			using (var p = new Process { StartInfo = start })
			{
				p.Start();
				// read the output here...
				//output = p.StandardOutput.ReadToEnd();
				//errorOutput = p.StandardError.ReadToEnd();
				if (_debug)
				{
					p.OutputDataReceived += outputDataReceived;
					p.ErrorDataReceived += errorDataReceived;
				}
				// ...then wait n milliseconds for exit (as after exit, it can't read the output)
				p.WaitForExit(60000);
				// read the exit code, close process
				returnCode = p.ExitCode;
			}
			if (_debug)
			{
				Console.WriteLine("phantomjs url: " + url);
				Console.WriteLine("phantomjs filePath: " + filePath);
				Console.WriteLine("phantomjs executable: " + executableFile);
				Console.WriteLine("phantomjs args: " + args);
				Console.WriteLine("phantomjs exit code: " + returnCode);
			}
			// if 0, it worked so return path of pdf
			if ((returnCode == 0))
				return filePath;
			else
				return null;
		}

		public FileEntity PdfStream(string filePath)
		{
			byte[] bytes = new byte[0];
			bool fail = true;
			while (fail)
			{
				try
				{
					using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
					{
						bytes = new byte[file.Length];
						file.Read(bytes, 0, (int)file.Length);
					}

					fail = false;
				}
				catch
				{
					Thread.Sleep(1000);
				}
			}

            if(_deleteFile)
            {
                File.Delete(filePath);//delete the document on disk before returning
            }
			
			return new FileEntity
			{
				Content = bytes,
                Path=filePath
			};
		}

		void outputDataReceived(object sender, DataReceivedEventArgs e)
		{
			// a line is writen to the out stream. you can use it like:
			Console.WriteLine("phantomjs data: " + e.Data);
		}

		void errorDataReceived(object sender, DataReceivedEventArgs e)
		{
			// a line is writen to the out stream. you can use it like:
			Console.WriteLine("phantomjs error: " + e.Data);
		}


	}
}
