using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Microsoft.AspNetCore.Http;

namespace AspClassicCore
{
    /// <summary>
    /// The response object that is accessible from ASP code
    /// Get not implement IResponse because the COM interface not supported by C#
    /// </summary>
    public class AspResponse //: IResponse
    {
        public HttpResponse Response;

        public AspResponse(HttpResponse response)
        {
            Response = response;
        }

        #region IResponse Members

        public void Add(string bstrHeaderValue, string bstrHeaderName)
        {
            AddHeader(bstrHeaderName, bstrHeaderValue);
        }

        public void AddHeader(string name, string value)
        {
            Response.Headers.Add(name, value);
        }

        public void AppendToLog(string param)
        {
            throw new NotImplementedException();
            //Response.AppendToLog(param);
        }

        public void BinaryWrite(object varInput)
        {
            throw new NotImplementedException();
            //ResponseBinaryWrite((byte[])varInput);
        }

        public void Clear()
        {
            //Response.Clear(); 
        }

        public void End()
        { 
            //Response.End(); 
        }

        public void Flush()
        {
            //Response.Flush(); 
        }

        public bool IsClientConnected()
        {
            return Response.HasStarted; // ??? IsClientConnected;
        }

        public void Pics(string value)
        {
            //return Response.Buffer;
            //Response.Pics(value); 
        }

        public void Redirect(string url)
        {
            Response.Redirect(url, true);
        }

        public void Write(object output)
        {
            if (output == null) 
                return;

            string strOut = output as string;
            Response.WriteAsync(strOut).GetAwaiter().GetResult();
        }

        public void WriteBlock(short iBlockNumber)
        {
            throw new NotImplementedException();
        }

        public bool Buffer
        {
            get
            {
                throw new NotImplementedException();
                //return Response.Buffer;
            }
            set
            {
                throw new NotImplementedException();
                //Response.Buffer = value;
            }
        }

        public string CacheControl
        {
            get
            {
                throw new NotImplementedException();
                //return Response.CacheControl;
            }
            set
            {
                throw new NotImplementedException();
                //Response.CacheControl = value;
            }
        }

        public string CharSet
        {
            get
            {
                throw new NotImplementedException();
                //return Response.Charset;
            }
            set
            {
                throw new NotImplementedException();
                //Response.Charset = value;
            }
        }

        public int CodePage
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string ContentType
        {
            get { return Response.ContentType; }
            set { Response.ContentType = value; }
        }

        //public IRequestDictionary Cookies
        //{
        //    get
        //    {
        //        return new AspCookieCollection(_context.Response.Cookies, false);
        //    }
        //}

        public int Expires
        {
            get
            {
                throw new NotImplementedException();
                //return Response.Expires;
            }
            set
            {
                throw new NotImplementedException();
                //Response.Expires = value;
            }
        }

        public DateTime ExpiresAbsolute
        {
            get
            {
                throw new NotImplementedException();
                //return Response.ExpiresAbsolute;
            }
            set
            {
                throw new NotImplementedException();
                //Response.ExpiresAbsolute = value;
            }
        }

        public int LCID
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Status
        {
            get
            {
                throw new NotImplementedException();
                //return Response.Status;
            }
            set
            {
                throw new NotImplementedException();
                //Response.Status = value;
            }
        }

        #endregion
    }
}
