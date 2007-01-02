#region DotNetNuke License
// DotNetNukeŽ - http://www.dotnetnuke.com
// Copyright (c) 2002-2006
// by Perpetual Motion Interactive Systems Inc. ( http://www.perpetualmotion.ca )
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
#endregion
using System;
using System.Collections;
using System.Text;
using System.Xml.Serialization;

namespace DotNetNuke.Services.Log.EventLog
{
    [Serializable(), XmlInclude(typeof(LogDetailInfo))]
    public class LogProperties : ArrayList
    {
        const int MAX_LEN = 75;

        public string Summary
        {
            get
            {
                string summary = this.ToString();
                if(String.IsNullOrEmpty(summary))
                {
                    return "Empty Summary";
                }
                else if (summary.Length > MAX_LEN)
                {
                    return summary.Substring( 0, MAX_LEN );
                }
                else
                {
                    return summary;
                }
            }
        }

        public override string ToString()
        {
            StringBuilder t = new StringBuilder();
            for (int i = 0; i < this.Count; i++)
            {
                LogDetailInfo ldi = this[i] as LogDetailInfo;
                if(ldi!=null)
                {
                    t.Append(ldi.ToString());
                }
            }
            return t.ToString();
        }
    }
}