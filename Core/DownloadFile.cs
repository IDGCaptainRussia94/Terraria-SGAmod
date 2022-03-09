using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Terraria.ModLoader;

//This file is a carbon-copy of TModLoader's Download File, a very useful method that was locked behind being internal, exposed so I may use it myself
//It should go without saying, but I take zero credit for this
//Original is here: https://github.com/tModLoader/tModLoader/blob/1.3/patches/tModLoader/Terraria.ModLoader.UI.DownloadManager/DownloadFile.cs
namespace SGAmod
{
	internal class DownloadFile
	{
		internal const string TEMP_EXTENSION = ".tmp";

		public const int CHUNK_SIZE = 1048576;

		public const SecurityProtocolType Tls12 = SecurityProtocolType.Tls12;

		public readonly string Url;

		public readonly string FilePath;

		public readonly string DisplayText;

		private FileStream _fileStream;

		public SecurityProtocolType SecurityProtocol = SecurityProtocolType.Tls12;

		public Version ProtocolVersion = HttpVersion.Version11;

		private bool _aborted;

		public HttpWebRequest Request
		{
			get;
			private set;
		}

		public event Action<float> OnUpdateProgress;

		public event Action OnComplete;

		public DownloadFile(string url, string filePath, string displayText)
		{
			Url = url;
			FilePath = filePath;
			DisplayText = displayText;
		}

		public bool Verify()
		{
			if (string.IsNullOrWhiteSpace(Url))
			{
				return false;
			}
			if (string.IsNullOrWhiteSpace(FilePath))
			{
				return false;
			}
			return true;
		}

		public Task<DownloadFile> Download(CancellationToken token, Action<float> updateProgressAction = null)
		{
			SetupDownloadRequest();
			if (updateProgressAction != null)
			{
				this.OnUpdateProgress = updateProgressAction;
			}
			return Task.Factory.FromAsync(Request.BeginGetResponse, (IAsyncResult asyncResult) => Request.EndGetResponse(asyncResult), null).ContinueWith((Task<WebResponse> t) => HandleResponse(t.Result, token), token);
		}

		private void AbortDownload(string filePath)
		{
			_aborted = true;
			Request?.Abort();
			_fileStream?.Flush();
			_fileStream?.Close();
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}
		}

		private DownloadFile HandleResponse(WebResponse response, CancellationToken token)
		{
			long contentLength = response.ContentLength;
			if (contentLength < 0)
			{
				string text = "Could not get a proper content length for DownloadFile[" + DisplayText + "]";
				SGAmod.Instance.Logger.Error((object)text);
				throw new Exception(text);
			}
			string text2 = string.Format("{0}{1}{2}{3}", new FileInfo(FilePath).Directory.FullName, Path.DirectorySeparatorChar, DateTime.Now.Ticks, ".tmp");
			_fileStream = new FileStream(text2, FileMode.Create);
			Stream responseStream = response.GetResponseStream();
			int num = 0;
			byte[] array = new byte[1048576];
			try
			{
				int num2;
				while ((num2 = responseStream.Read(array, 0, array.Length)) > 0)
				{
					token.ThrowIfCancellationRequested();
					_fileStream.Write(array, 0, num2);
					num += num2;
					this.OnUpdateProgress?.Invoke((float)((double)num / (double)contentLength));
				}
			}
			catch (OperationCanceledException ex)
			{
				AbortDownload(text2);
				SGAmod.Instance.Logger.Info((object)("DownloadFile[" + DisplayText + "] operation was cancelled"), (Exception)ex);
			}
			catch (Exception ex2)
			{
				AbortDownload(text2);
				SGAmod.Instance.Logger.Info((object)"Unknown error", ex2);
			}
			if (!_aborted)
			{
				_fileStream?.Close();
				PreCopy();
				File.Copy(text2, FilePath, overwrite: true);
				File.Delete(text2);
				this.OnComplete?.Invoke();
			}
			return this;
		}

		private void SetupDownloadRequest()
		{
			ServicePointManager.SecurityProtocol = SecurityProtocol;
			ServicePointManager.ServerCertificateValidationCallback = ServerCertificateValidation;
			Request = WebRequest.CreateHttp(Url);
			if (FrameworkVersion.Framework == Framework.NetFramework)
			{
				Request.ServicePoint.ReceiveBufferSize = 1048576;
			}
			Request.Method = "GET";
			Request.ProtocolVersion = ProtocolVersion;
			Request.UserAgent = "tModLoader/" + ModLoader.versionTag;
			Request.KeepAlive = true;
		}

		private bool ServerCertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			return errors == SslPolicyErrors.None;
		}

		internal virtual void PreCopy()
		{
		}
	}
}
