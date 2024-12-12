using PanoramaApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramaApp.Interfaces
{
    public interface IUrlHelperService
    {
        string ConvertToEmbedUrl(string youtubeUrl);
    }
}