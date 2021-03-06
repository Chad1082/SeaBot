﻿// SeaBotCore
// Copyright (C) 2018 - 2019 Weespin
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//  
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
namespace SeaBotCore
{
    #region

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Xml;

    using Newtonsoft.Json;

    using SeaBotCore.Cache;
    using SeaBotCore.Data;
    using SeaBotCore.Localizaion;
    using SeaBotCore.Statistics;
    using SeaBotCore.Utils;

    #endregion

    public static class Networking
    {
        public static Thread _syncThread = new Thread(SyncVoid);

        private static readonly List<Task.IGameTask> _gametasks = new List<Task.IGameTask>();

        private static readonly HttpClientHandler handler = new HttpClientHandler
                                                                {
                                                                    AutomaticDecompression =
                                                                        DecompressionMethods.GZip
                                                                        | DecompressionMethods.Deflate
                                                                };

        private static readonly HttpClient Client = new HttpClient(handler);

        private static readonly MD5 Md5 = new MD5CryptoServiceProvider();

        private static DateTime _lastRaised = DateTime.Now;

        private static string _lastsend = string.Empty;

        private static int _taskId = 1;

        static Networking()
        {
            Events.Events.SyncFailedEvent.SyncFailed.OnSyncFailedEvent += SyncFailedChat_OnSyncFailedEvent;
            _syncThread.IsBackground = true;

            _syncThread.Start();
        }

        public static void AddTask(Task.IGameTask task)
        {
            _gametasks.Add(task);
            _lastRaised = DateTime.Now;
        }

        public static void Login()
        {
            LocalizationController.SetLanguage(Core.Config.language);
            Logger.Logger.Info("Logining ");

            // Get big token
            var tempuid = string.Empty;

            var baseAddress = new Uri("https://portal.pixelfederation.com/");
            var cookieContainer = new CookieContainer();
            var loc_cookies = Cookies.ReadCookiesFromDisk();
            //var pxcookie = loc_cookies.GetCookies(new Uri("https://portal.pixelfederation.com"));
            //if (pxcookie.Count != 0)
            //{
              
            //        if (pxcookie["_pf_login_server_token"].Value == Core.Config.server_token)
            //        {
            //            cookieContainer = loc_cookies;
            //        }
                
            //}
           
            using (var handler = new HttpClientHandler { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                cookieContainer.Add(baseAddress, new Cookie("_pf_login_server_token", Core.Config.server_token));
                cookieContainer.Add(baseAddress, new Cookie("pixsid_portal", "2vavk2elovtqc3nt978a3mkv8t"));
                Logger.Logger.Info(Localization.NETWORKING_LOGIN_1);
                client.DefaultRequestHeaders.UserAgent.ParseAdd(
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.80 Safari/537.36");

                try
                {
                    var result = client.GetAsync("en/seaport/").Result;

                    result = client.GetAsync("en/seaport/").Result;
                    Logger.Logger.Info(Localization.NETWORKING_LOGIN_2);
                    var stringtext = result.Content.ReadAsStringAsync().Result;
                    var regex = new Regex(
                        "portal.pixelfederation.com\\/(_sp\\/\\?direct_login=portal&portal_request=(.*))\" allowfullscreen>");
                    var match = regex.Match(stringtext);
                    if (match.Success)
                    {
                        var data = client.GetAsync(match.Groups[1].Value).Result.Content.ReadAsStringAsync().Result;
                        Logger.Logger.Info(Localization.NETWORKING_LOGIN_3);
                        regex = new Regex(@"session_id': '(.*)', 'test");

                        Core.Ssid = regex.Match(data).Groups[1].Value;
                        regex = new Regex(@"pid': '(.*)', 'platform");
                        tempuid = regex.Match(data).Groups[1].Value;
                        Logger.Logger.Info(Localization.NETWORKING_LOGIN_SUCCESS + Core.Ssid);
                        regex = new Regex(@"'definition_filelist_url1': 'https:\/\/r4a4v3g4\.ssl\.hwcdn\.net\/definitions\/filelists\/(.+)\.xml', 'definition_filelist_url2'");
                        var mtch = regex.Match(data);
                        if (mtch.Success)
                        {
                            DefenitionCache.Update(mtch.Groups[1].Value);
                        }

                        regex = new Regex(
                            @"loca_filelist_url2': 'https:\/\/static\.seaportgame\.com\/localization\/(.+?)\.xml', '");
                        mtch = regex.Match(data);
                        if (mtch.Success)
                        {
                            LocalizationCache.Update(mtch.Groups[1].Value);
                        }

                        regex = new Regex("clientPath = \"(.+)\";");
                        mtch = regex.Match(data);
                        if (mtch.Success)
                        {
                            Client.DefaultRequestHeaders.Referrer = new Uri(mtch.Groups[1].Value);
                        }

                        Client.DefaultRequestHeaders.Host = "portal.pixelfederation.com";
                        Client.DefaultRequestHeaders.Add("Origin", "https://cdn.seaportgame.com");
                        Client.DefaultRequestHeaders.AcceptEncoding.TryParseAdd("gzip, deflate, br");
                        Client.DefaultRequestHeaders.Accept.TryParseAdd(@"*/*");
                        Client.DefaultRequestHeaders.AcceptLanguage.TryParseAdd(
                            "en-GB,en-US;q=0.9,en;q=0.8,ru;q=0.7,uk;q=0.6");
                        Client.DefaultRequestHeaders.Add("DNT", "1");
                        Client.DefaultRequestHeaders.Add("X-Requested-With", "ShockwaveFlash/32.0.0.114");
                    }
                    else
                    {
                        Logger.Logger.Fatal(Localization.NETWORKING_LOGIN_CANT_LOGIN);
                        return;
                    }
                }
                catch (Exception e)
                {
                    Logger.Logger.Fatal(Localization.NETWORKING_LOGIN_CANT_LOGIN + e);
                }
            }

            var values = new Dictionary<string, string> { { "pid", tempuid }, { "session_id", Core.Ssid } };
            var s = SendRequest(values, "client.login");
            SendRequest(values, "client.update");

            if (!s.StartsWith("<xml>"))
            {
                s = Parser.ConvertJSONToXmlString(s);
            }

            if (s.StartsWith("<xml>"))
            {
                Core.GlobalData = Parser.ParseXmlToGlobalData(s);
                var rand = new Random();

                var loadtime = rand.Next(5000, 13000);
                if (!Core.Debug)
                {
                    Logger.Logger.Info(string.Format(Localization.NETWORKING_LOGIN_FAKE_LOAD, loadtime / 1000D));
                    Thread.Sleep(loadtime);
                    Logger.Logger.Info(
                        string.Format(Localization.NETWORKING_LOGIN_FAKE_LOAD_ELAPSED, loadtime / 1000D));
                }

                values.Add("loading_time", loadtime.ToString());
                SendRequest(values, "tracking.finishedLoading");
                Cookies.WriteCookiesToDisk(cookieContainer);
                Events.Events.LoginedEvent.Logined.Invoke();
                StatisticsWriter.Start();
            }
            else
            {
                Logger.Logger.Fatal("Server responded " + s);
                Core.StopBot();
            }
        }

        public static string SendRequest(Dictionary<string, string> data, string action)
        {
            try
            {
                var content = new FormUrlEncodedContent(data);

                var response = Client.PostAsync("https://portal.pixelfederation.com/sy/?a=" + action, content);

                // <xml><time>1548446333</time></xml>
                try
                {
                    var doc = new XmlDocument();
                    try
                    {
                        string resp = response.Result.Content.ReadAsStringAsync().Result;

                        if (resp.IsValidXml())
                        {
                            doc.LoadXml(resp);
                        }
                        else 
                        {
                            resp = Parser.ConvertJSONToXmlString(resp);
                            doc.LoadXml(resp);
                        }
                        
                    }
                    catch (Exception e)
                    {
                        Logger.Logger.Fatal(string.Format(Localization.NETWORKING_NO_RESPONSE, response, e));
                    }

                    if (doc.DocumentElement != null)
                    {
                        var s = Convert.ToInt64(doc.DocumentElement.SelectSingleNode("time")?.InnerText);
                        TimeUtils.CheckForTimeMismatch(s);
                    }
                }
                catch (Exception e)
                {
                    Logger.Logger.Warning(e.ToString());
                }

                return response.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Events.Events.SyncFailedEvent.SyncFailed.Invoke(0);
                Logger.Logger.Fatal(ex.ToString());
            }

            return string.Empty;
        }

        public static void StartThread()
        {
            if (!_syncThread.IsAlive)
            {
                _syncThread = new Thread(SyncVoid);
                _gametasks.Clear();
                _taskId = 1;
                _syncThread.Start();
            }
        }

        //TODO: Change the below to send JSON not XML
        public static void Sync()
        {
            var taskstr = new StringBuilder("<xml>\n");
            var firstfifteen = _gametasks.Take(50).ToList();
            foreach (var task in firstfifteen)
            {
                taskstr.Append("<task>\n");
                taskstr.Append("<action>" + task.Action + "</action>\n");
                foreach (var customobj in task.CustomObjects)
                {
                    taskstr.Append($"<{customobj.Key}>{customobj.Value}</{customobj.Key}>\n");
                }

                taskstr.Append("<time>" + task.Time + "</time>\n");
                taskstr.Append("</task>\n");
                _gametasks.Remove(task);
            }

            taskstr.Append($"<task_id>{_taskId}</task_id>\n");
            taskstr.Append($"<time>{TimeUtils.GetEpochTime()}</time>\n");
            taskstr.Append("</xml>");
            _lastsend = taskstr.ToString();
            var lenght = 0;
            lenght = _lastsend.Length > 224 ? 224 : _lastsend.Length;
            var sig = ToHex(
                Md5.ComputeHash(Encoding.ASCII.GetBytes(_lastsend.Substring(0, lenght) + Core.Ssid + "KNn2R4sK")),
                false); // _loc2_.substr(0,224) + _sessionData.sessionId + "KNn2R4sK"
            var values = new Dictionary<string, string>
                             {
                                 { "pid", Core.GlobalData.UserId.ToString() },
                                 { "session_id", Core.Ssid },
                                 { "data", taskstr.ToString() },
                                 { "sig", sig }
                             };
            _taskId++;
            Logger.Logger.Debug(new XMLMinifier(XMLMinifierSettings.Aggressive).Minify(taskstr.ToString()));
            var response = SendRequest(values, "client.synchronize");
            Logger.Logger.Debug(response);
            var doc = new XmlDocument();
            try
            {

                if (response.IsValidXml())
                {
                    doc.LoadXml(response);
                }
                else
                {
                    response = Parser.ConvertJSONToXmlString(response);
                    doc.LoadXml(response);
                }
                doc.LoadXml(response);
            }
            catch (Exception e)
            {
                Logger.Logger.Fatal(string.Format(Localization.NETWORKING_NO_RESPONSE, response, e));
                Core.restartwithdelay(10);
            }

            if (doc.DocumentElement != null)
            {
                var s = doc.DocumentElement.SelectNodes("task");
                var passed = 0;
                foreach (XmlNode node in s)
                {
                    if (node.SelectSingleNode("result")?.InnerText == "OK")
                    {
                        Logger.Logger.Debug(node.SelectSingleNode("action")?.InnerText + " has been passed");
                        passed++;
                    }
                    else
                    {
                        Logger.Logger.Debug(node.SelectSingleNode("action")?.InnerText + " failed!");
                    }
                }

                if (passed != 0)
                {
                    Logger.Logger.Debug(string.Format(Localization.NETWORKING_SYNC_ACCEPTED_GOOD, passed));
                }
                else
                {
                    Logger.Logger.Warning(string.Format(Localization.NETWORKING_SYNC_ACCEPTED_BAD, passed));
                    Logger.Logger.Info(Localization.NETWORKING_SYNC_FATAL_CHECK);
                    if (doc.SelectSingleNode("xml/task/result")?.InnerText == "ERROR")
                    {
                        var errcode =
                            (Enums.EErrorCode)Convert.ToInt32(doc.SelectSingleNode("xml/task/error_code")?.InnerText);
                        Logger.Logger.Fatal(
                            string.Format(Localization.NETWORKING_SYNC_SERVER_DISCONNECTED, errcode.ToString()));
                        Events.Events.SyncFailedEvent.SyncFailed.Invoke(errcode);
                    }
                    else if (doc.SelectSingleNode("xml/result")?.InnerText == "ERROR")
                    {
                        var errcode =
                            (Enums.EErrorCode)Convert.ToInt32(doc.SelectSingleNode("xml/error_code")?.InnerText);
                        Logger.Logger.Fatal(
                            string.Format(Localization.NETWORKING_SYNC_SERVER_DISCONNECTED, errcode.ToString()));
                        Events.Events.SyncFailedEvent.SyncFailed.Invoke(errcode);
                    }
                }

                var pushnode = doc.DocumentElement.SelectSingleNode("push");
                if (pushnode != null)
                {
                    foreach (XmlNode node in pushnode.ChildNodes)
                    {
                        switch (node.Name)
                        {
                            case "level_up":
                                Core.GlobalData.Level = Convert.ToInt32(node.ChildNodes[0].InnerText);
                                break;
                            case "sailors":
                                Core.GlobalData.Sailors = Convert.ToInt32(node.ChildNodes[0].InnerText);
                                break;
                            case "sync_interval":
                                Core.GlobalData.SyncInterval = Convert.ToByte(node.ChildNodes[0].InnerText);
                                break;
                            case "xp":
                                Core.GlobalData.Xp = Convert.ToInt32(node.ChildNodes[0].InnerText);
                                break;
                            case "material":
                                {
                                    foreach (XmlNode materials in node.ChildNodes)
                                    {
                                        var defid = Convert.ToInt32(materials.SelectSingleNode("def_id")?.InnerText);
                                        var amount = Convert.ToInt32(materials.SelectSingleNode("value")?.InnerText);
                                        if (Core.GlobalData.Inventory.Count(n => n.Id == defid) != 0)
                                        {
                                            Core.GlobalData.Inventory.Where(n => n.Id == defid).First().Amount = amount;
                                        }
                                        else
                                        {
                                            Core.GlobalData.Inventory.Add(new Item { Id = defid, Amount = amount });
                                        }
                                    }

                                    break;
                                }
                        }
                    }
                }
            }
            else
            {
                Logger.Logger.Fatal(Localization.NETWORKING_SYNC_NO_RESPONSE);
                Core.restartwithdelay(10);
            }

            _lastRaised = DateTime.Now;
        }

        public static string ToHex(this byte[] bytes, bool upperCase)
        {
            var result = new StringBuilder(bytes.Length * 2);

            foreach (var t in bytes)
            {
                result.Append(t.ToString(upperCase ? "X2" : "x2"));
            }

            return result.ToString();
        }

        private static void SyncFailedChat_OnSyncFailedEvent(Enums.EErrorCode e)
        {
            System.Threading.Tasks.Task.Run(
                () =>
                    {
                        if (e == Enums.EErrorCode.WRONG_SESSION)
                        {
                            Logger.Logger.Info(
                                string.Format(Localization.NETWORKING_SOMEONE_IS_PLAYING, Core.hibernation));

                            if (Core.IsBotRunning)
                            {
                                _syncThread.Abort();
                                Logger.Logger.Muted = true;
                                Thread.Sleep(Core.hibernation * 1000 * 60);
                                Logger.Logger.Muted = false;
                                Logger.Logger.Info(Localization.NETWORKING_WAKING_UP);
                                StartThread();
                                Login();
                            }
                        }
                    });
        }

        private static void SyncVoid()
        {
            LocalizationController.SetLanguage(Core.Config.language);
            while (true)
            {
                Thread.Sleep(6 * 1000);
                if (_gametasks.Count != 0 && Core.GlobalData.Level != 0)
                {
                    Logger.Logger.Debug(Localization.NETWORKING_SYNCING);
                    Sync();
                }

                if ((DateTime.Now - _lastRaised).TotalSeconds
                    > (Core.GlobalData.HeartbeatInterval == 0 ? 300 : Core.GlobalData.HeartbeatInterval))
                {
                    Logger.Logger.Debug(Localization.NETWORKING_HEARTBEAT);
                    _gametasks.Add(new Task.HeartBeat());

                    Sync();
                }
            }
        }
    }
}