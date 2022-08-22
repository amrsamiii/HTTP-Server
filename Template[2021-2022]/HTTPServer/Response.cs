using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{

    public enum StatusCode
    {
        OK = 200,
        InternalServerError = 500,
        NotFound = 404,
        BadRequest = 400,
        Redirect = 301
    }

    class Response
    {
        string responseString;
        public string ResponseString
        {
            get
            {
                return responseString;
            }
        }
        StatusCode code;
        List<string> headerLines = new List<string>();
        public Response(StatusCode code, string contentType, string content, string redirectoinPath)
        {
            this.code = code;
            //throw new NotImplementedException();
            // TODO: Add headlines (Content-Type, Content-Length,Date, [location if there is redirection])
            string ret_date = "date: " + DateTime.Now + " \r\n";
            //hint
            contentType = "content-type: " + contentType + " \r\n";
            String content_len = "content length: " + content.Length + " \r\n";


            headerLines.Add(contentType);
            headerLines.Add(content_len);
            headerLines.Add(ret_date);

            this.code = code;
            if (code == StatusCode.Redirect)
            {
                string loc = "location: " + redirectoinPath + "\r\n";
                headerLines.Add(loc);
            }

            // TODO: Create the request string
            responseString = GetStatusLine(code);
            for (int i = 0; i < headerLines.Count; i++)
            {
                responseString = responseString + headerLines[i];
            }
            responseString += "\r\n";
            responseString += content;

        }

        private string GetStatusLine(StatusCode code)
        {
            // TODO: Create the response status line and return it
            string statusLine = string.Empty;
            statusLine = "HTTP/1.1 " + ((int)code).ToString() + " " + code.ToString() + "\r\n";
            return statusLine;
        }
    }
}
