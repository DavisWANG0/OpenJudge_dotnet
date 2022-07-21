using CompilerService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DataModels.Models;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;

namespace dotNET_HDOJ2_FinalProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompileController : ControllerBase
    {
        private compilerContext _CompilerContext = new compilerContext();

        [HttpPost("doCompile")]
        public async Task<JObject> doCompile([FromBody] JObject jObject)
        {
            var jsonStr = JsonConvert.SerializeObject(jObject);
            var jsonParams = JsonConvert.DeserializeObject<dynamic>(jsonStr);

            string clientId = "cd6d8a995171b3c59d622b2175d3b5d8"; //Replace with your client ID
            string clientSecret = "3e72cad6c57d97d3f3c015c0a59538a937b1c0b697b454db14a9b65cd1aa877c"; //Replace with your client Secret
            string script = jsonParams.script;  //program to compile and execute
            string stdin = jsonParams.stdin;  //StdIn
            string language = jsonParams.language;  //language of the script
            string versionIndex = jsonParams.versionIndex; //version of the language to be used

            JObject returnJson = CompilerService.CompileService.getRemoteCompile(clientId, clientSecret, script, stdin, language, versionIndex);

            return returnJson;
        }

    }
}
