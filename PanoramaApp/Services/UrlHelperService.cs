// <copyright file="UrlHelperService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using PanoramaApp.Data;
    using PanoramaApp.Interfaces;
    using PanoramaApp.Models;

    public class UrlHelperService : IUrlHelperService
    {
        public string ConvertToEmbedUrl(string youtubeUrl)
        {
            if (string.IsNullOrEmpty(youtubeUrl))
            {
                return string.Empty;
            }

            try
            {
                var uri = new Uri(youtubeUrl);
                var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                var videoId = query["v"];

                return videoId != null ? $"https://www.youtube.com/embed/{videoId}" : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
