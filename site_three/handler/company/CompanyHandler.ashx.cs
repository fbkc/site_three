using AutoSend;
using HRMSys.DAL;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using xinxi;

namespace site_three.handler.company
{
    /// <summary>
    /// CompanyHandler 的摘要说明
    /// </summary>
    public class CompanyHandler : IHttpHandler
    {
        private string hostName = "一路发发网分类资讯";
        private string hostUrl = "http://39.105.196.3:8175";
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
                        case "companyMain": _strContent.Append(CompanyMain(context)); break;//主页
                        case "introduce": _strContent.Append(Introduce(context)); break;//简介
                        case "productions": _strContent.Append(Productions(context)); break;//产品
                        case "infos": _strContent.Append(Infos(context)); break;//新闻
                        case "contact": _strContent.Append(Contact(context)); break;//联系
                        default: break;
                    }
                }
            }
            context.Response.Write(_strContent.ToString());
        }
        private BLL bll = new BLL();

        #region 公司主页
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string CompanyMain(HttpContext context)
        {
            string columnId = context.Request["cId"];
            string Id = context.Request["Id"];
            if (string.IsNullOrEmpty(columnId) || string.IsNullOrEmpty(Id))
                return SqlHelper.WriteTemplate("", "404.html");
            try
            {
                htmlPara hInfo = bll.GetHtmlPara(columnId, Id);
                string keyword = "";//关键词
                string description = "";//描述
                if (hInfo.title.Length > 6)
                    keyword = hInfo.title + "," + hInfo.title.Substring(0, 2) + "," + hInfo.title.Substring(2, 2) + "," + hInfo.title.Substring(4, 2);
                else
                    keyword = hInfo.title;
                description = hInfo.title + "，" + BLL.ReplaceHtmlTag(hInfo.articlecontent, 80)
                    .Replace("\r\n", "").Replace("<br/>", "").Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "") + "...";//产品简介
                var data = new
                {
                    htmlTitle = hInfo.title,
                    keyword = "一路发发网分类资讯",
                    description = "一路发发网分类资讯，热门行业资讯。",
                    hostName,
                    hostUrl,
                    ProductList = bll.GetProFloat(hInfo.userId, "22"),//右侧浮动10条产品
                    NewsList = bll.GetNewsFloat(hInfo.userId, "22")//右侧浮动10条新闻
                };
                return SqlHelper.WriteTemplate(data, "jsdy/CompanyMain.html");
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
        }
        #endregion

        #region 公司简介
        public string Introduce(HttpContext context)
        {
            string columnId = context.Request["cId"];
            string Id = context.Request["Id"];
            if (string.IsNullOrEmpty(columnId) || string.IsNullOrEmpty(Id))
                return SqlHelper.WriteTemplate("", "404.html");
            try
            {
                htmlPara hInfo = bll.GetHtmlPara(columnId, Id);
                string keyword = "";//关键词
                string description = "";//描述
                if (hInfo.title.Length > 6)
                    keyword = hInfo.title + "," + hInfo.title.Substring(0, 2) + "," + hInfo.title.Substring(2, 2) + "," + hInfo.title.Substring(4, 2);
                else
                    keyword = hInfo.title;
                description = hInfo.title + "，" + BLL.ReplaceHtmlTag(hInfo.articlecontent, 80)
                    .Replace("\r\n", "").Replace("<br/>", "").Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "") + "...";//产品简介
                //根据username调用tool接口获取userInfo，做一个编码解码加密
                string strjson = NetHelper.HttpGet("http://tool.100dh.cn/UserHandler.ashx?action=GetUserByUsername&username=" + hInfo.username, "", Encoding.UTF8);//公共接口，调用user信息
                JObject jo = (JObject)JsonConvert.DeserializeObject(strjson);
                cmUserInfo userInfo = JsonConvert.DeserializeObject<cmUserInfo>(jo["detail"]["cmUser"].ToString());
                var data = new
                {
                    htmlTitle = hInfo.title,
                    keyword = "一路发发网分类资讯",
                    description = "一路发发网分类资讯，热门行业资讯。",
                    hostName,
                    hostUrl,
                    companyName=userInfo.companyName,
                    companyRemark=userInfo.companyRemark,//公司简介
                    NewsList = bll.GetNewsFloat(hInfo.userId, "22")//10条新闻
                };
                return SqlHelper.WriteTemplate(data, "jsdy/introduce.html");
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
        }
        #endregion

        #region 公司产品
        public string Productions(HttpContext context)
        {
            string columnId = context.Request["cId"];
            string Id = context.Request["Id"];
            if (string.IsNullOrEmpty(columnId) || string.IsNullOrEmpty(Id))
                return SqlHelper.WriteTemplate("", "404.html");
            try
            {
                htmlPara hInfo = bll.GetHtmlPara(columnId, Id);
                string keyword = "";//关键词
                string description = "";//描述
                if (hInfo.title.Length > 6)
                    keyword = hInfo.title + "," + hInfo.title.Substring(0, 2) + "," + hInfo.title.Substring(2, 2) + "," + hInfo.title.Substring(4, 2);
                else
                    keyword = hInfo.title;
                description = hInfo.title + "，" + BLL.ReplaceHtmlTag(hInfo.articlecontent, 80)
                    .Replace("\r\n", "").Replace("<br/>", "").Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "") + "...";//产品简介
                //根据username调用tool接口获取userInfo，做一个编码解码加密
                string strjson = NetHelper.HttpGet("http://tool.100dh.cn/UserHandler.ashx?action=GetUserByUsername&username=" + hInfo.username, "", Encoding.UTF8);//公共接口，调用user信息
                JObject jo = (JObject)JsonConvert.DeserializeObject(strjson);
                cmUserInfo userInfo = JsonConvert.DeserializeObject<cmUserInfo>(jo["detail"]["cmUser"].ToString());
                var data = new
                {
                    htmlTitle = hInfo.title,
                    keyword = "一路发发网分类资讯",
                    description = "一路发发网分类资讯，热门行业资讯。",
                    hostName,
                    hostUrl,
                    companyName = userInfo.companyName,
                    NewsList = bll.GetNewsFloat(hInfo.userId, "22")//10条新闻
                };
                return SqlHelper.WriteTemplate(data, "jsdy/productions.html");
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
        }
        #endregion

        #region 新闻资讯
        public string Infos(HttpContext context)
        {
            string columnId = context.Request["cId"];
            string Id = context.Request["Id"];
            if (string.IsNullOrEmpty(columnId) || string.IsNullOrEmpty(Id))
                return SqlHelper.WriteTemplate("", "404.html");
            try
            {
                htmlPara hInfo = bll.GetHtmlPara(columnId, Id);
                string keyword = "";//关键词
                string description = "";//描述
                if (hInfo.title.Length > 6)
                    keyword = hInfo.title + "," + hInfo.title.Substring(0, 2) + "," + hInfo.title.Substring(2, 2) + "," + hInfo.title.Substring(4, 2);
                else
                    keyword = hInfo.title;
                description = hInfo.title + "，" + BLL.ReplaceHtmlTag(hInfo.articlecontent, 80)
                    .Replace("\r\n", "").Replace("<br/>", "").Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "") + "...";//产品简介
                //根据username调用tool接口获取userInfo，做一个编码解码加密
                string strjson = NetHelper.HttpGet("http://tool.100dh.cn/UserHandler.ashx?action=GetUserByUsername&username=" + hInfo.username, "", Encoding.UTF8);//公共接口，调用user信息
                JObject jo = (JObject)JsonConvert.DeserializeObject(strjson);
                cmUserInfo userInfo = JsonConvert.DeserializeObject<cmUserInfo>(jo["detail"]["cmUser"].ToString());
                var data = new
                {
                    htmlTitle = hInfo.title,
                    keyword = "一路发发网分类资讯",
                    description = "一路发发网分类资讯，热门行业资讯。",
                    hostName,
                    hostUrl,
                    companyName = userInfo.companyName,
                    NewsList = bll.GetNewsFloat(hInfo.userId, "22")//10条新闻
                };
                return SqlHelper.WriteTemplate(data, "jsdy/infos.html");
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
        }
        #endregion

        #region 联系
        public string Contact(HttpContext context)
        {
            string columnId = context.Request["cId"];
            string Id = context.Request["Id"];
            if (string.IsNullOrEmpty(columnId) || string.IsNullOrEmpty(Id))
                return SqlHelper.WriteTemplate("", "404.html");
            try
            {
                htmlPara hInfo = bll.GetHtmlPara(columnId, Id);
                string keyword = "";//关键词
                string description = "";//描述
                if (hInfo.title.Length > 6)
                    keyword = hInfo.title + "," + hInfo.title.Substring(0, 2) + "," + hInfo.title.Substring(2, 2) + "," + hInfo.title.Substring(4, 2);
                else
                    keyword = hInfo.title;
                description = hInfo.title + "，" + BLL.ReplaceHtmlTag(hInfo.articlecontent, 80)
                    .Replace("\r\n", "").Replace("<br/>", "").Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "") + "...";//产品简介
                //根据username调用tool接口获取userInfo，做一个编码解码加密
                string strjson = NetHelper.HttpGet("http://tool.100dh.cn/UserHandler.ashx?action=GetUserByUsername&username=" + hInfo.username, "", Encoding.UTF8);//公共接口，调用user信息
                JObject jo = (JObject)JsonConvert.DeserializeObject(strjson);
                cmUserInfo userInfo = JsonConvert.DeserializeObject<cmUserInfo>(jo["detail"]["cmUser"].ToString());
                var data = new
                {
                    htmlTitle = hInfo.title,
                    keyword = "一路发发网分类资讯",
                    description = "一路发发网分类资讯，热门行业资讯。",
                    hostName,
                    hostUrl,
                    username=hInfo.username,
                    NewsList = bll.GetNewsFloat(hInfo.userId, "22")//10条新闻
                };
                return SqlHelper.WriteTemplate(data, "jsdy/contact.html");
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}