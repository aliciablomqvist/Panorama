// <copyright file="Index.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PanoramaApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        this.logger = logger;
    }

    public void OnGet()
    {
    }
}