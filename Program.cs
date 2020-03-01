using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

namespace DownloadImagesByUrl {
	class Program {
		static void Main() {
			Console.WriteLine("Enter destination folder path to save images: ");
			string destinationFolder = Console.ReadLine();
			string urlFileName = "urls.txt";
			if (Directory.Exists(destinationFolder) == false)
				Console.WriteLine("destination folder not found");
			else if (File.Exists(urlFileName) == false)
				Console.WriteLine("urls.txt file not found");
			else {
				string[] urls = File.ReadAllLines(urlFileName);
				int count = 0;
				Console.Write("Images download: 0");
				foreach (string url in urls) {
					if (DownloadImage(url, Path.Combine(destinationFolder, Path.GetFileName(url)))) {
						count++;
						Console.Write("\rImages download: {0}", count);
					}
				}

				Console.WriteLine("\n{0} images download", count);
			}

			Console.WriteLine("Press any key to exit");
			Console.ReadKey();
		}

		private static bool DownloadImage(string imgUrl, string fileName) {
			bool error = false;
			try {
				using (WebClient client = new WebClient()) {
					Stream stream = client.OpenRead(imgUrl);
					if (stream != null) {
						Bitmap bitmap = new Bitmap(stream);
						ImageFormat imageFormat = ImageFormat.Jpeg;
						if (bitmap.RawFormat.Equals(ImageFormat.Png)) {
							imageFormat = ImageFormat.Png;
						}
						else if (bitmap.RawFormat.Equals(ImageFormat.Bmp)) {
							imageFormat = ImageFormat.Bmp;
						}
						else if (bitmap.RawFormat.Equals(ImageFormat.Gif)) {
							imageFormat = ImageFormat.Gif;
						}
						else if (bitmap.RawFormat.Equals(ImageFormat.Tiff)) {
							imageFormat = ImageFormat.Tiff;
						}

						bitmap.Save(fileName, imageFormat);
						stream.Flush();
						stream.Close();
						client.Dispose();
					}
				}
			}
			catch (WebException ex) {
				error = true;
				Console.WriteLine(ex.Message);
			}
			catch (ExternalException ex) {
				error = true;
				Console.WriteLine("Something is wrong with Format -- Maybe required Format is not applicable here : {0} ", ex.Message);
			}
			catch (ArgumentNullException ex) {
				error = true;
				Console.WriteLine("Something Wrong with Stream : {0}", ex.Message);
			}

			return !error;
		}
	}
}