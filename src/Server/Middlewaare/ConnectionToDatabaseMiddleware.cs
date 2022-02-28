using MoneyManager.Server.Entities;
using MoneyManager.Server.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Server.Middlewaare
{
    public class ConnectionToDatabaseMiddleware : IMiddleware
    {
        private readonly ILogger<ConnectionToDatabaseMiddleware> _logger;
        private readonly TrackerDbContext _dbContext;
        private Stopwatch _stopwatch;
        public ConnectionToDatabaseMiddleware(ILogger<ConnectionToDatabaseMiddleware> logger, TrackerDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            _stopwatch = new Stopwatch();
            if (!_dbContext.Database.CanConnect())
            {
                _logger.LogError("Database cannot connect");
                throw new InternalServerErrorException("Unable to connect to the database");
            }
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopwatch.Start();
            await next.Invoke(context);
            _stopwatch.Stop();
            var elapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
            if (elapsedMilliseconds > 4000)
            {
                _logger.LogError($"Request [{context.Request.Method}] at {context.Request.Path} took {elapsedMilliseconds} ms");
                //throw new InternalServerErrorException("Unable to connect to the database");
            }
        }
    }
}
