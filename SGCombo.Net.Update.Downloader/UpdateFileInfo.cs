////////////////////////////////////////////////////////////////////////////
//	Copyright 2008 : Vladimir Novick    https://www.linkedin.com/in/vladimirnovick/  
//
//    NO WARRANTIES ARE EXTENDED. USE AT YOUR OWN RISK. 
//
// To contact the author with suggestions or comments, use  :vlad.novick@gmail.com
//
////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace SGCombo.Net.Update.Downloader
{
    class UpdateFileInfo
    {
        public String Name { get; set; }
        public String UpdateFile { get; set; }
        public String CreationTime { get; set; }
        public String  LastAccessTime { get; set; }
        public long Length { get; set; }
        public Boolean isCopy { get; set; }
        public Boolean NewFile { get; set; }
        public Boolean backUp { get; set; }
        public String Guid { get; set; }
        public String Keypad { get; set; }
        public Boolean doProcess { get; set; }
        public long CreationTimeUtc { get; set; }
        public long LastWriteTimeUtc { get; set; }



        public UpdateFileInfo(XmlNode node)
        {
                this.Name = node.Attributes["Name"].Value;
                this.UpdateFile = node.Attributes["UpdateFile"].Value;
                this.CreationTime = node.Attributes["CreationTime"].Value;

                this.LastAccessTime = node.Attributes["LastAccessTime"].Value;
                this.Length =  Convert.ToInt32(node.Attributes["Length"].Value);
                this.Guid = node.Attributes["Guid"].Value;
                this.Keypad = node.Attributes["Keypad"].Value;
                this.CreationTimeUtc = Convert.ToInt64(node.Attributes["CID"].Value);
                this.LastWriteTimeUtc = Convert.ToInt64(node.Attributes["LID"].Value); 


                doProcess = false;
                isCopy = false;
                backUp = false;
                NewFile = false;
        }
    }
}
