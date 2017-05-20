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
            m_smplQueue = new Queue();
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


        public void downloadData(string SourceUrl, string fileName, DowloadInfoLineHandler callback, long TimeStamp)
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
                downloadChunk(istart);
            } while (fileStream.Length < RequestLen);

            if (fileStream != null)
            {
                fileStream.Close();
            }
        }




        private void downloadChunk(long start)
        {



            m_req = (HttpWebRequest)HttpWebRequest.Create(mSourceUrl + "&TS=" + mTimeStamp.ToString() + "&Start=" + start);

            
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


            IAsyncResult result =
                 (IAsyncResult) m_req.BeginGetResponse(ResponseReceived, null);
 

            lock (m_smplQueue)
            {
                Monitor.Wait(m_smplQueue);
            }

            

           m_resp.GetResponseStream().Close();
           m_resp.Close();
        }


 
        HttpWebResponse m_resp = null;


        Byte[] dataBuffer;

        String ErrorMessage = "";


        void ResponseReceived(IAsyncResult res)
        {
            try
            {
                m_resp = (HttpWebResponse)m_req.EndGetResponse(res);

                if (RequestLen == 0)
                {
                    RequestLen = (long)m_resp.ContentLength;
                }
            }
            catch (WebException ex)
            {
                ErrorMessage = ex.Message;
                ExitReadFile();
                return;
            }
   

            long position = 0;
            try
            {
                do
                {
                    dataBuffer = new byte[BUFSIZE];
                    LastRead = 0;
                    st = m_resp.GetResponseStream();
                    st.WriteTimeout = 5000;
                    IAsyncResult result = st.BeginRead(dataBuffer, 0, BUFSIZE,
                     null, this);

                    bool ex = result.AsyncWaitHandle.WaitOne(6000, true);

                    OnDataRead(result);

                    position = fileStream.Position;

                } while (position < RequestLen);
            }
            catch (Exception ex)
            {
            
                m_callback(new DowloadInfoLineHandlerEventArgs(position, RequestLen, String.Copy(mSourceUrl), ex.Message));
            }
            ExitReadFile();
        }

        private FileStream fileStream = null;

        private long RequestLen = 0;


        private Stream st;
        private int LastRead = 0;


        void OnDataRead(IAsyncResult res)
        {
            int nBytes = 0;
            LastRead = 0;
            try
            {

                 nBytes = st.EndRead(res);
                 LastRead = nBytes;
            }
            catch (Exception )
            {
                ExitReadFile();
                return;
            }


            if (nBytes > 0)
            {
                fileStream.Write(dataBuffer, 0, nBytes);
                fileStream.Flush();
      
            }

            long position = fileStream.Position;

            m_callback(new DowloadInfoLineHandlerEventArgs(position, RequestLen, String.Copy(mSourceUrl), null));

  





        }


        //-----------------------------

        void ExitReadFile()
        {
            lock (m_smplQueue)
            {
                Monitor.Pulse(m_smplQueue);
            }
        }





        private Queue m_smplQueue;
       // private int bytesRead = 0;
    //    private Stream stream = null;

 

    }
}
