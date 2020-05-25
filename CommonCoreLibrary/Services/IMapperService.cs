using System;
using System.Collections.Generic;
using System.Text;

namespace CommonCoreLibrary.Services
{
    public interface IMapperService
    {
        R Map<R>(object fromMap);
    }
}
