using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client.Events;
using RabbitMQManager.Models;

namespace RabbitMQManager.Interface
{
    interface IRabbitMQContext
    {
        void PublierMessage(string message);
        void ConsommerMessage();
        void publierReponse(MessageBodyEvent  e, string _reponse);

    }
}
