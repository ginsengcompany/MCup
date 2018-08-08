using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.DownloadManager;
using Plugin.DownloadManager.Abstractions;

namespace MCup.Model
{
    public class Downloader
    {
        public IDownloadFile File;

        public Downloader()
        {
            CrossDownloadManager.Current.CollectionChanged += (sender, e) =>
                System.Diagnostics.Debug.WriteLine(
                    "[DownloadManager] " + e.Action +
                    " -> New items: " + (e.NewItems?.Count ?? 0) +
                    " at " + e.NewStartingIndex +
                    " || Old items: " + (e.OldItems?.Count ?? 0) +
                    " at " + e.OldStartingIndex
                );
        }

        public void InitializeDownload(string url)
        {
            File = CrossDownloadManager.Current.CreateDownloadFile(url
            // If you need, you can add a dictionary of headers you need.
            //, new Dictionary<string, string> {
            //    { "Cookie", "LetMeDownload=1;" },
            //    { "Authorization", "Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==" }
            //}
            );
        }

        public void StartDownloading(bool mobileNetworkAllowed)
        {
            CrossDownloadManager.Current.Start(File, mobileNetworkAllowed);
        }

        public void AbortDownloading()
        {
            CrossDownloadManager.Current.Abort(File);
        }

        public bool IsDownloading()
        {
            if (File == null) return false;

            switch (File.Status)
            {
                case DownloadFileStatus.INITIALIZED:
                case DownloadFileStatus.PAUSED:
                case DownloadFileStatus.PENDING:
                case DownloadFileStatus.RUNNING:
                    return true;

                case DownloadFileStatus.COMPLETED:
                case DownloadFileStatus.CANCELED:
                case DownloadFileStatus.FAILED:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
