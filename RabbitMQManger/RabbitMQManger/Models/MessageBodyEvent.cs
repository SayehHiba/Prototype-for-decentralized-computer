using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RabbitMQManager.Models
{
    public class MessageBodyEvent : EventArgs
    {
        

        public MessageBodyEvent(string message, IBasicProperties basicProperties, IModel model)
        {
            Message = message;
            BasicProperties = basicProperties;
            Model = model;
          
        }

        public string Message { get ; set; }
        public IBasicProperties BasicProperties { get; set; }
        public IModel Model { get; set; }
        
    }
}