using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EightElements.Services
{
    public interface ITextService
    {
        IHtmlContent GetHtml(string key, string language = null);
        string GetString(string key, string language = null);
    }
}
