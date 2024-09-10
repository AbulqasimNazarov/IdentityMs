using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;
using IdentityMs.Data;
using IdentityMs.Models;
using IdentityMs.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using RabbitMQ.Client;

namespace IdentityMs.Controllers
{
    [Route("[controller]")]
    public class IdentityController : Controller
    {
        private readonly IdentityMsDbContext dbContext;
        private readonly ConnectionFactory rabbitMqConnectionFactory;

        public IdentityController(IdentityMsDbContext dbContext, IOptionsSnapshot<RabbitMqOptions> optionsSnapshot)
        {
            this.dbContext = dbContext;
            this.rabbitMqConnectionFactory = new ConnectionFactory()
            {
                HostName = optionsSnapshot.Value.HostName,
                UserName = optionsSnapshot.Value.UserName,
                Password = optionsSnapshot.Value.Password
            };
        }



        [HttpPost("[action]")]
        public async Task<IActionResult> SignUp([FromForm] User user)
        {
            this.dbContext.Users.Add(user);

            using var connection = this.rabbitMqConnectionFactory.CreateConnection();
            using var channel = connection.CreateModel();
            var destination = "new_user";

            var result = channel.QueueDeclare(
                queue: destination,
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            var userJson = JsonSerializer.Serialize(user);

            var messageInBytes = Encoding.UTF8.GetBytes(userJson);

            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: destination,
                basicProperties: null,
                body: messageInBytes
            );

            return base.Created();
        }
    }
}