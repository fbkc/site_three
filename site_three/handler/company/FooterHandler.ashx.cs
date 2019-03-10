using HRMSys.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace xinxi.handler.company
{
    /// <summary>
    /// FooterHandler 的摘要说明
    /// </summary>
    public class FooterHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            StringBuilder _strContent = new StringBuilder();
            if (_strContent.Length == 0)
            {
                string _strAction = context.Request.Params["action"];
                if (string.IsNullOrEmpty(_strAction))
                {
                    _strContent.Append(_strContent.Append(SqlHelper.WriteTemplate("", "404.html")));
                }
                else
                {
                    switch (_strAction.Trim())
                    {
                        //case "companyMain": _strContent.Append(CompanyMain(context)); break;//主页
                        default: break;
                    }
                }
            }
            context.Response.Write(_strContent.ToString());
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}