using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DataModels.Models;
namespace CheckAnswerService
{
    public class CheckAnswerService
    {
        private compilerContext _CompilerContext = new compilerContext();

        public JObject getRemoteCheck(string clientId, string clientSecret, string script, string language, string versionIndex, long questionId)
        {
            string answer = "null";
            string time = "null";
            string output;
            //long questionId = jsonParams.questionId;
            IQueryable<Sample> samples =
                from sample in _CompilerContext.Samples where sample.QuestionId == questionId && sample.IsTest == 1 select sample;
            List<Sample> samplesList = samples.ToList();
            JObject returnJson = new JObject();
            for (int i = 0; i < samplesList.Count; i++)
            {
                var sampleStr = JsonConvert.SerializeObject(samplesList[i]);
                var sampleParams = JsonConvert.DeserializeObject<dynamic>(sampleStr);
                string stdin = sampleParams.input;
                try
                {
                    string trueResult = sampleParams.output;
                    HttpClient httpClient = new HttpClient();

                    String input = "{\"clientId\": \"" + clientId + "\",\"clientSecret\":\"" + clientSecret + "\",\"script\":\"" + script +
                       "\",\"language\":\"" + language + "\",\"stdin\":\"" + stdin + "\",\"versionIndex\":\"" + versionIndex + "\"} ";

                    HttpContent httpContent = new StringContent(input);

                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var events = httpClient.PostAsync("https://api.jdoodle.com/v1/execute", httpContent).Result;

                    var res = events.Content.ReadAsStringAsync().Result;

                    var jsonRes = JsonConvert.SerializeObject(res);
                    var resParams = JsonConvert.DeserializeObject<dynamic>(jsonRes);

                    var runOutput = JsonConvert.DeserializeObject<dynamic>(res);
                    string runResult = runOutput.output;
                    time = runOutput.time;
                    runResult.Replace("\n", "");
                    Console.WriteLine(trueResult);
                    Console.WriteLine(runResult);
                    if(string.Compare(runResult,trueResult)!=0)
                    {
                        returnJson.Add("code",400);
                        returnJson.Add("message", "Error");
                        returnJson.Add("input", stdin);
                        returnJson.Add("errorMessage", runResult);
                        returnJson.Add("testCases", i);
                        returnJson.Add("allCases", samplesList.Count);
                        returnJson.Add("time", time);
                        returnJson.Add("runResult", runResult);
                        returnJson.Add("realResult", trueResult);
                        return returnJson;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            returnJson.Add("allCases", samplesList.Count);
            returnJson.Add("time", time);
            return returnJson;

        }
    }
}