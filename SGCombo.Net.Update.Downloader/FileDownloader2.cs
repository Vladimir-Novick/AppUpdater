using System;
using System.Collections;
using System.IO;
using System.Net;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

using System.Security.AccessControl;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Net.Cache;


namespace SGCombo.Net.Update.Downloader

{

    delegate void DowloadInfoLineHandler(DowloadInfoLineHandlerEventArgs e);

    class FileDownloader 

    {



        private const int BUFSIZE = 1024;
            



        public FileDownloader()
        {
          
        }



        //-----------------------------


        HttpWebRequest m_req = null;

        String newFileName;

        DowloadInfoLineHandler m_callback = null;

        String mSourceUrl;
        long mTimeStamp = 0;

        public static void checkDir(string fileName){
            DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(fileName));
            if (!di.Exists) di.Create();
        }


        public void downloadData(string SourceUrl, string fileName, DowloadInfoLineHandler callback, long TimeStamp,long lenData)
        {
            newFileName = fileName;
            mSourceUrl = SourceUrl;
            this.m_callback = callback;
            mTimeStamp = TimeStamp;



            checkDir(newFileName);

            fileStream = new FileStream(newFileName, FileMode.Create);
            long istart = 0;
            int ReRead = 0;
            do
            {
                ReRead++;
                if (ReRead > 20)
                {
                    break;
                }
                istart = fileStream.Length;
                downloadChunk(istart,lenData);

                long position = fileStream.Position;
                long d = RequestLen;
                DowloadInfoLineHandlerEventArgs e = new DowloadInfoLineHandlerEventArgs(position, d, String.Copy(SourceUrl), null);
                m_callback( e);
 
            } while (fileStream.Length < RequestLen);

            if (fileStream != null)
            {
                fileStream.Close();
            }
        }




        private void downloadChunk(long start,long len)
        {

            String uri = mSourceUrl + "&TS=" + mTimeStamp.ToString() + "&Start=" + start;
            if (len > 0)
            {
                uri += "&len=" + len.ToString();
            }

            m_req = (HttpWebRequest)HttpWebRequest.Create(uri);

            
            IWebProxy proxy = WebRequest.GetSystemWebProxy();
 
            proxy.Credentials = CredentialCache.DefaultCredentials;
            m_req.Proxy = proxy;
            m_req.Method = "GET";
            m_req.Pipelined = true;
            m_req.MaximumResponseHeadersLength = 2000000;
            m_req.KeepAlive = false;
            m_req.UserAgent = "Mozilla/4.0+(compatible;+MSIE+6.0;+Windows+NT+5.1;+.NET+CLR+1.1.4322)";
            m_req.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            m_req.ReadWriteTimeout = 5000;
            m_req.Timeout = 5000;


            HttpWebResponse m_resp =  (HttpWebResponse) m_req.GetResponse();
            st = m_resp.GetResponseStream();
             dataBuffer = new byte[BUFSIZE];
             int nBytes;
             while (true) {
                 nBytes = st.Read(dataBuffer, 0, BUFSIZE);
                 if (nBytes <= 0) break;
                 fileStream.Write(dataBuffer, 0, nBytes);
                 fileStream.Flush();
             }
           st.Close();
           m_resp.Close();
        }


 
        HttpWebResponse m_resp = null;


        Byte[] dataBuffer;

        String ErrorMessage = "";




        private FileStream fileStream = null;

        private long RequestLen = 0;


        private Stream st;



        //-----------------------------







        private int bytesRead = 0;
        private Stream stream = null;

 

    }
}
