#r "System.Web"
#r "System.Web.Http"
#r "System.Net"
#r "System.Net.Http"
//#r "ScriptCs.WebApi.Pack.dll"
#r "Newtonsoft.Json"

#load "SlackInterpreter.csx"

using System;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Web;
using System.Web.Http;

//public class SlackModelController : ApiController
//{
    public static async Task<object> Run(HttpRequestMessage req)
    {
        string jsonContent = await req.Content.ReadAsStringAsync();
        var command = new SlackInterpreter().ExtractCommand(jsonContent);

        return req.CreateResponse(HttpStatusCode.OK, command);
    }
//}

/*var webApi = Require<WebApi>();
var server = webApi.CreateServer("http://localhost:8082");
server.OpenAsync().Wait();

Console.WriteLine("Listening...");
Console.ReadKey();
server.CloseAsync().Wait();*/