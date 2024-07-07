using API_Inovatube.Entidades;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;

namespace API_Inovatube
{
    public class YoutubeBuscador
    {
        public ResultadoBusqueda Buscar(int idSolicitud, string Busqueda)
        {
            ResultadoBusqueda Resultado = null;
            YoutubeBuscador buscador = new YoutubeBuscador();

            try
            {
                Resultado = buscador.BusquedaYoutube(idSolicitud, Busqueda).Result;
            }
            catch (AggregateException ex)
            {
               
            }

            return Resultado;
        }

        public async Task<ResultadoBusqueda> BusquedaYoutube(int idSolicitud, string Busqueda)
        {
            ResultadoBusqueda ResultadoB = new ResultadoBusqueda();

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "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",
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = Busqueda;
            searchListRequest.MaxResults = 50;

            var searchListResponse = await searchListRequest.ExecuteAsync();

            List<string> videos = new List<string>();
            List<string> channels = new List<string>();
            List<string> playlists = new List<string>();

            foreach (var searchResult in searchListResponse.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        videos.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.VideoId));
                        break;

                    case "youtube#channel":
                        channels.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.ChannelId));
                        break;

                    case "youtube#playlist":
                        playlists.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.PlaylistId));
                        break;
                }
            }

            ResultadoB.Videos = String.Format("Videos:\n{0}\n", string.Join("\n", videos));
            ResultadoB.Canales = String.Format("Channels:\n{0}\n", string.Join("\n", channels));
            ResultadoB.Listas = String.Format("Playlists:\n{0}\n", string.Join("\n", playlists));

            return ResultadoB;
        }
    }
}
