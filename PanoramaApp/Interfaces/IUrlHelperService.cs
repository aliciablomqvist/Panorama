// <copyright file="IUrlHelperService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using PanoramaApp.Models;

    public interface IUrlHelperService
    {
        string ConvertToEmbedUrl(string youtubeUrl);
    }
}