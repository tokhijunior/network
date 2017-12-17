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
            throw new NotImplementedException();
            // TODO: Add headlines (Content-Type, Content-Length,Date, [location if there is redirection])
            headerLines.Add("Content-Type: " + contentType);
            headerLines.Add("Content-Length: " + content.Length.ToString());
            headerLines.Add("Date: " + DateTime.Now);
            if (redirectoinPath != "") headerLines.Add("Location: " + redirectoinPath);
            // TODO: Create the respone string
            this.responseString += GetStatusLine(code);
            foreach (var line in headerLines)
            {
                this.responseString += line + "\r\n";
            }
            this.responseString += "\r\n" + content;

        }


        private string GetStatusLine(StatusCode code)
        {
            // TODO: Create the response status line and return it
            string statusLine = string.Format("HTTP/1.1 {0} {1}\r\n", "", ((int)code).ToString(), code.ToString());

            return statusLine;
        }
    }
}
