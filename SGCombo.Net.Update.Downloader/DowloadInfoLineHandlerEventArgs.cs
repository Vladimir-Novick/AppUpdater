using System;

namespace SGCombo.Net.Update.Downloader
{
    internal class DowloadInfoLineHandlerEventArgs : EventArgs
    {
        internal DowloadInfoLineHandlerEventArgs(long count, long fileLen, String strLink, String message)
        {
            Count = count;
            FileLength = fileLen;
            Link = strLink.ToString() ;
            ErrorMessage = message;
        }

        public String Link { get; set; }
        public long Count { get; set; }
        public long FileLength { get; set; }

        public String ErrorMessage { get; set; }
    }
}
