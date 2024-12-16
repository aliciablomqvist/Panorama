// <copyright file="IUrlHelperService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PanoramaApp.Models;

    /// <summary>
    /// Interface for Url's
    /// </summary>
    public interface IUrlHelperService
    {
        /// <summary>
        /// Converts to embed URL.
        /// </summary>
        /// <param name="youtubeUrl">The youtube URL.</param>
        /// <returns></returns>
        string ConvertToEmbedUrl(string youtubeUrl);
    }
}
