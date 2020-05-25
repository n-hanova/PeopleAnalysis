using AuthAPI.Settings;
using CommonCoreLibrary.Auth.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace AnalyticAPI.Services
{
    public class VirtualHttpContext : HttpContext
    {
        public override ConnectionInfo Connection => throw new NotImplementedException();

        public override IFeatureCollection Features => throw new NotImplementedException();

        public override IDictionary<object, object> Items { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override HttpRequest Request => throw new NotImplementedException();

        public override CancellationToken RequestAborted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override IServiceProvider RequestServices { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override HttpResponse Response => throw new NotImplementedException();

        public override ISession Session { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string TraceIdentifier { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override ClaimsPrincipal User { get; set; }

        public override WebSocketManager WebSockets => throw new NotImplementedException();

        public override void Abort()
        {
            throw new NotImplementedException();
        }
    }

    public class VirtualIdentity : IIdentity
    {
        public string AuthenticationType => JwtBearerDefaults.AuthenticationScheme;

        public bool IsAuthenticated => true;

        public string Name => string.Empty;
    }

    public class VirtualHttpContextAccessor : IHttpContextAccessor
    {
        public VirtualHttpContextAccessor()
        {
            HttpContext = new VirtualHttpContext()
            {
                User = new ClaimsPrincipal(new VirtualIdentity())
            };
        }

        public HttpContext HttpContext { get; set; }
    }

    public class VirtualTokenService : ClientTokenService
    {
        public VirtualTokenService(AuthSettings authSettings, IHttpContextAccessor httpContextAccessor) : base(authSettings, httpContextAccessor)
        {
        }

        public override async Task SignInAsync(IAuthResult loginResult)
        {
            if (loginResult == null)
                return;
            var principal = GetPrincipalFromExpiredToken(loginResult.AccessToken);
            principal.Identities.First().AddClaim(new Claim("Token", loginResult.AccessToken));
            principal.Identities.First().AddClaim(new Claim("Refresh", loginResult.RefreshToken));
            /*for next request in this request*/
            httpContextAccessor.HttpContext.User = principal;
        }
    }
}
