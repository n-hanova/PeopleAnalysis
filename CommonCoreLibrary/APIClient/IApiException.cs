using System;
using System.Collections.Generic;
using System.Text;

namespace CommonCoreLibrary.APIClient
{
    public interface IApiException
    {
        int StatusCode { get; }
    }
}
