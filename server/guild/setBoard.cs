﻿using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using db;

namespace server.guild
{
    internal class setBoard : RequestHandler
    {
        public override void HandleRequest(HttpListenerContext context)
        {
            NameValueCollection query;
            using (var rdr = new StreamReader(context.Request.InputStream))
                query = HttpUtility.ParseQueryString(rdr.ReadToEnd());

            using (var db = new Database(Program.Settings.GetValue("conn")))
            {
                Account acc = db.Verify(query["guid"], query["password"]);
                byte[] status;
                if (acc == null)
                {
                    status = Encoding.UTF8.GetBytes("<Error>Account credentials not valid</Error>");
                }
                else
                {
                    status = Encoding.UTF8.GetBytes(db.SetGuildBoard(query["board"], acc));
                }
                context.Response.OutputStream.Write(status, 0, status.Length);
            }
        }
    }
}