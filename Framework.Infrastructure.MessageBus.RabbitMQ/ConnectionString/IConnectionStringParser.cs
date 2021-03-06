﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sprache;

namespace Framework.Infrastructure.MessageBus.RabbitMQ.ConnectionString
{
    public interface IConnectionStringParser
    {
        IConnectionConfiguration Parse(string connectionString);
    }

    public class ConnectionStringParser : IConnectionStringParser
    {
        public IConnectionConfiguration Parse(string connectionString)
        {
            try
            {
                var updater = ConnectionStringGrammar.ConnectionStringBuilder.Parse(connectionString);
                var connectionConfiguration = updater.Aggregate(new ConnectionConfiguration(), (current, updateFunction) => updateFunction(current));
                connectionConfiguration.Validate();
                return connectionConfiguration;
            }
            catch (ParseException parseException)
            {
                throw new Exception(string.Format("Connection String {0}", parseException.Message));
            }
        }
    }
}
