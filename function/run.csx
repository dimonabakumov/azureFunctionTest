#r "System.Web"
#r "System.Net"
#r "System.Net.Http"
#r "Newtonsoft.Json"

#load "SlackInterpreter.csx"

using System;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Web;
using System.Web.Http;


public static async Task<object> Run(HttpRequestMessage req)
{
    string jsonContent = await req.Content.ReadAsStringAsync();
    var command = new SlackInterpreter().ExtractCommand(jsonContent);
    return req.CreateResponse(HttpStatusCode.OK, command);
}