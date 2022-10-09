using Lime.Protocol;
using Lime.Protocol.Serialization;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Bot_HTTP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Bot : ControllerBase
    {
        private readonly IEnvelopeSerializer _env;
        private readonly IConfiguration _config;

        public Bot(IConfiguration config, IEnvelopeSerializer envelopeSerializer)
        {
            _config = config;
            _env=envelopeSerializer;
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>        
        /// <returns></returns>
        // POST api/<Bot>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SendMessage([FromBody] object value)
        
        {
            var msgbody = value.ToString();

            var msg = _env.Deserialize(msgbody) as Message;

            if (msg.Type== "text/plain") {                
                var id = Guid.NewGuid();
                var client = new RestClient(_config.GetValue<string>("UrlBase"));
                var request = new RestRequest("/messages", Method.Post);
                var json = "{\r\n    \"id\":   \"" + id + "\", \r\n   \"type\": \"text/plain\",\r\n     \"content\": \"Bem vindo ao bot Http\",\r\n    \"to\": \"" + msg.From.ToIdentity()+ "\",\r\n " + "}";

                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", _config.GetValue<string>("AutoriaztionKey"));
                request.AddParameter("application/json", json, ParameterType.RequestBody);
                var response = client.ExecutePost(request);
                Console.WriteLine(response.Content);
                return Accepted();
            }
            return Accepted();

        }        
    }
}
