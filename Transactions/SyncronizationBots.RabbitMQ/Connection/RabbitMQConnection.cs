
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client;
using SyncronizationBots.RabbitMQ.Connection.Interface;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Polly.Retry;
using Polly;
using Microsoft.Extensions.Options;
using SyncronizationBots.RabbitMQ.Configuration;

namespace SyncronizationBots.RabbitMQ.Connection
{
    public class RabbitMQConnection : IRabbitMQConnection
    {
        private readonly IOptions<RabbitMqConfiguration> _rabbitConfiguration;
        private readonly IConnectionFactory _connectionFactory;
        protected IConnection _connection;
        private readonly int _retryCount;
        bool _disposed;

        object sync_root = new object();

        public RabbitMQConnection(IOptions<RabbitMqConfiguration> rabbitConfiguration, int retryCount = 5)
        {
            this._rabbitConfiguration = rabbitConfiguration ?? throw new ArgumentNullException(nameof(this._rabbitConfiguration));
            this._connectionFactory = CreateConnectionFactory() ?? throw new ArgumentNullException(nameof(this._connectionFactory));
            this._connection = _connectionFactory.CreateConnection();
            this._retryCount = retryCount;
        }

        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen && !_disposed;
            }
        }
        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }
            return _connection.CreateModel();
        }
        public bool TryConnect()
        {
            lock (sync_root)
            {
                var policy = RetryPolicy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                        
                    }
                );
                policy.Execute(() =>
                {
                    _connection = _connectionFactory
                          .CreateConnection();
                });
                if (IsConnected)
                {
                    _connection.ConnectionShutdown += OnConnectionShutdown;
                    _connection.CallbackException += OnCallbackException;
                    _connection.ConnectionBlocked += OnConnectionBlocked;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #region Private Methods

        private IConnectionFactory? CreateConnectionFactory()
        {
            var connection = new ConnectionFactory
            {
                HostName = this._rabbitConfiguration.Value.HostName,
                UserName = this._rabbitConfiguration.Value.UserName,
                Password = this._rabbitConfiguration.Value.Password,
                VirtualHost = this._rabbitConfiguration.Value.VirtualHost,
                AutomaticRecoveryEnabled = this._rabbitConfiguration.Value.AutomaticRecoveryEnabled ?? true,
                RequestedHeartbeat = new TimeSpan(0, 0, this._rabbitConfiguration.Value.RequestedHeartbeat ?? 30)
            };
            return connection;
        }

        #endregion

        #region Events

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;
            TryConnect();
        }
        protected void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;
            TryConnect();
        }
        protected void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            if (_disposed) return;
            TryConnect();
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;

            try
            {
                _connection.Dispose();
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
