using System.Net;
using System.Net.Http.Headers;
using tarea1.Dto;
using Newtonsoft.Json;
using Ocelot.Middleware;
using Ocelot.Multiplexer;


namespace tarea1.Aggregator
{
    public class ExampleAgregator : IDefinedAggregator
    {
        public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        {
            if (responses.Any(x => x.Items.Errors().Count > 0))
            {
                return new DownstreamResponse(null, HttpStatusCode.BadRequest, null as List<Header>, null);
            }
            //LLEGADA DE DATOS
            var userDatas = await responses[0].Items.DownstreamResponse().Content.ReadAsStringAsync();
            var postDatas = await responses[1].Items.DownstreamResponse().Content.ReadAsStringAsync();

            var usuarios = JsonConvert.DeserializeObject<List<User>?>(userDatas);
            var posts = JsonConvert.DeserializeObject<List<Post>?>(postDatas);


            //VERIFICANDO COINCIDENCIAS
            foreach (var user in usuarios!)
            {
                var result = posts?.Where(p => p.UserId == user.Id).ToList();
                user.Posts?.AddRange(result!);
            }

            var postByUserString = JsonConvert.SerializeObject(usuarios);
            var stringContent = new StringContent(postByUserString)

            {
                Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
            };

            return new DownstreamResponse(stringContent, HttpStatusCode.OK, new List<KeyValuePair<string, IEnumerable<string>>>(), "OK");

        }
    }
}
