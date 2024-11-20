using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using ProjectUNIX.GameServer.Network.kcp;
using ProjectUNIX.GameServer.Network.kcp.Session;
using System.Net;

namespace ProjectUNIX.GameServer.Network.Manager
{
    internal class NetSessionManager
    {
        private readonly ConcurrentDictionary<long, NetSession> _sessions;
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory scopeFactory;

        public NetSessionManager(ILogger<NetSessionManager> logger, IServiceScopeFactory scopeFactory)
        {
            _sessions = new();

            this.scopeFactory = scopeFactory;
            _logger = logger;
        }

        public void Add(NetSession session)
        {
            _sessions[session.SessionId] = session;
            _logger.LogInformation("Connection " + session.EndPoint + " SID: " + session.SessionId);
        }

        public bool TryRemove(NetSession session)
        {
            bool removed = _sessions.TryRemove(session.SessionId, out _);
            if (removed)
            {
                _logger.LogInformation("Disconnected " + session.EndPoint);
            }

            return removed;
        }


        public async Task RunSessionAsync(long sessionId, INetworkUnit networkUnit)
        {
            await using AsyncServiceScope serviceScope = scopeFactory.CreateAsyncScope();
            NetSession session = serviceScope.ServiceProvider.GetRequiredService<NetSession>();
            try
            {
                session.Establish(sessionId, networkUnit);
                await session.RunAsync();
            }
            catch (OperationCanceledException oe)
            {
                // OperationCanceled

                _logger.LogInformation(oe.Message);

                session.Close();

            }
            catch (Exception exception)
            {
                _logger.LogError("Exception occurred during handling a session, trace: {exception}", exception);
            }
        }

        public bool IsSessionExisted(long id)
        {
            return _sessions.ContainsKey(id);
        }

        public bool GetSessionEstablishByEndPoint(IPEndPoint endpoint)
        {
            foreach (NetSession session in _sessions.Values)
            {
                if (session.EndPoint.Address.Equals(endpoint.Address))
                {
                    return true;
                }
            }
            return false;
        }

        public NetSession GetSessionById(long id) => _sessions.GetValueOrDefault(id);
    }
}
