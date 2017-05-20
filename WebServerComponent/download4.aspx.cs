using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
 
public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strRequest = Request.QueryString["file"];
        string strStart = Request.QueryString["Start"];
        if (String.IsNullOrEmpty(strStart))
        {
            strStart = "0";
        }

        string strLen = Request.QueryString["Len"];
        if (String.IsNullOrEmpty(strLen))
        {
            strLen = "0";
        }
        if (String.IsNullOrEmpty(strRequest))
        {
            strRequest = "/local/../textfile.txt";
        }
        DownloadFile(strRequest, long.Parse(strStart), long.Parse(strLen));

    }

    const int BUFSIZE = 1024;

    protected void DownloadFile(string filename,long start, long lenBlock)
    {

          string path = "";
          long len;
          string fName = filename;
           int ip  = filename.LastIndexOf("/");
           if (ip > 0)
           {
                path = filename.Substring(0, ip);
                fName = filename.Substring(ip+1);
           }
         

            string path2 = Server.MapPath("." + path);


            string filepath = path2 + "\\" +  fName ;
        FileStream fStream = null;
        try
        {
            FileInfo file = new System.IO.FileInfo(filepath);
            len = file.Length - start;
            if (file.Exists)
            {
                DateTime lasUpdate = file.LastWriteTimeUtc;
                fStream =  file.OpenRead();


                long fileSize = (int)fStream.Length;


                byte[] byteBuffer = new byte[BUFSIZE];
                


                Response.Clear();
             
                Response.BufferOutput = false;
                Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
                Response.Cache.SetValidUntilExpires(false);
                Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore(); 
                Response.Buffer = false;
                Response.DisableKernelCache();

                Response.ContentType = "binary/octet-stream";

                long Contextlen = fileSize;



                Response.AddHeader("Content-Length", Contextlen.ToString());
                Response.AddHeader("Content-Disposition", "inline; filename=" + fName + "_" + start.ToString() + "_" + lasUpdate.ToString("MMddyyHHmm"));


                if (!(fileSize < start))
                {




                    fStream.Seek(start, SeekOrigin.Begin);

                    if ((fileSize - fStream.Position) < BUFSIZE)
                    {
                        byteBuffer = new byte[fileSize - fStream.Position];
                    }


                    int readCount = fStream.Read(byteBuffer, 0, byteBuffer.Length);

                    long ReadCount = lenBlock;

                    while (readCount > 0)
                    {

                        Response.BinaryWrite(byteBuffer);
                        Response.Flush();

                        if (lenBlock > 0)
                        {
                            ReadCount -= readCount;
                            if (ReadCount < 0)
                            {
                                break;
                            }
                        }

                        if ((fileSize - fStream.Position) < BUFSIZE)
                        {
                            byteBuffer = new byte[fileSize - fStream.Position];
                        }


                        readCount = fStream.Read(byteBuffer, 0, byteBuffer.Length);
                    }
                }
            }

        }
        catch (Exception ee)
        {

        }
        finally
        {
            if (fStream != null)
            {
                fStream.Close();
                fStream.Dispose();


            }
            Response.End();
            Response.Close();
            
        }
    }
}
