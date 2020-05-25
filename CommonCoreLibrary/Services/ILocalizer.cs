using CommonCoreLibrary.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CommonCoreLibrary.Services
{
    public interface ILocalizer
    {
        string this[string code] { get; }
    }
}
