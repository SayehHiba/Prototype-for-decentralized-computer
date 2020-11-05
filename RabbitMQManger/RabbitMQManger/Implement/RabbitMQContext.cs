using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQManager.Interface;
using RabbitMQManager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQManager.Implement
{
    public class RabbitMQContext : IRabbitMQContext
    {
        #region attribut
        private string _correlationId;
        private IBasicProperties _basicProperties;
        private IModel _model;
        private event EventHandler<MessageBodyEvent> _onConsommerMessage;
        private event EventHandler<MessageBodyEvent> _onRecupererReponse;

        #endregion


        #region propriété d'évenement

        public EventHandler<MessageBodyEvent> OnConsommerMessage { get => _onConsommerMessage; set => _onConsommerMessage = value; }
        public EventHandler<MessageBodyEvent> OnRecupererReponse { get => _onRecupererReponse; set => _onRecupererReponse = value; }

        #endregion

        #region constructeur
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model">le model doit etre égale à :ConnexionSingleton.Connexion.CreateModel</param>
        public RabbitMQContext(IModel model)
        {
            //ConnexionSingleton.Connexion("", "", "", new ConnectionFactory()).Result.CreateModel();
            _model = model;
        }
        #endregion

        #region expéditeur

        /// <summary>
        /// publier un message bien structurer à partir de la nom de la file rabbitMQ 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="nomFile"></param>
        public void PublierMessage(string message)
        {


            _model.QueueDeclare("CommunicationProduitVente", true, false, false, null);//declaration de la queue:channel=chaine(creation si elle n'existe pas)
         


            string rpcResponseQueue = _model.QueueDeclare().QueueName;//recuperation du nom de la queue

           
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);


            _correlationId = Guid.NewGuid().ToString();//creation d'un ID aveclequel on va envoyer 
                                                             //string responseFromConsumer = null;

            _basicProperties = _model.CreateBasicProperties();//creation des prop de a queue
            _basicProperties.ReplyTo = rpcResponseQueue;
            _basicProperties.CorrelationId = _correlationId;


          
            _model.BasicPublish("", "CommunicationProduitVente", _basicProperties, messageBytes);
            EventingBasicConsumer rpcEventingBasicConsumer = new EventingBasicConsumer(_model);
            rpcEventingBasicConsumer.Received += EvenementRecuperationDeReponseDeMessagePublier;
            _model.BasicConsume(rpcResponseQueue, false, rpcEventingBasicConsumer);

        }
        /// <summary>
        /// la methode assister a l'evenement rabbitMQ dans la methode PublierMessageAvecReponse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EvenementRecuperationDeReponseDeMessagePublier(object sender, BasicDeliverEventArgs e)
        {
            IBasicProperties props = e.BasicProperties;
            if (props != null
                && props.CorrelationId == _correlationId)//verifier si je si l'ID du recepteur correspond au mien
            {
                string response = Encoding.UTF8.GetString(e.Body.ToArray());//byte -> string

                if (OnRecupererReponse != null) OnRecupererReponse(this, new MessageBodyEvent(response, props, _model));

              
            }
            _model.BasicAck(e.DeliveryTag, false);
        }
        #endregion



        #region destinataire
        /// <summary>
        /// consommer message à partir de la nom de la file rabbitMQ 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public void ConsommerMessage()
        {
          
            _model.BasicQos(0, 1, false);
            _model.QueueDeclare("CommunicationProduitVente", true, false, false, null);
            EventingBasicConsumer eventingBasicConsumer = new EventingBasicConsumer(_model);
            eventingBasicConsumer.Received += EvenementRecuperationDeMessageAConsommer;
            _model.BasicConsume("CommunicationProduitVente", false, eventingBasicConsumer);

        }
        /// <summary>
        /// la methode assister a l'evenement rabbitMQ dans la methode ConsommerMessage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EvenementRecuperationDeMessageAConsommer(object sender, BasicDeliverEventArgs e)
        {

            string message = Encoding.UTF8.GetString(e.Body.ToArray());

            IBasicProperties basicProperties = e.BasicProperties;
            if (OnConsommerMessage != null) OnConsommerMessage(this, new MessageBodyEvent(message, basicProperties, _model));

            _model.BasicAck(e.DeliveryTag, false);

           
        }

        #endregion


        public void publierReponse(MessageBodyEvent e,string _reponse)
        {
            IBasicProperties replyBasicProperties = e.Model.CreateBasicProperties();
            replyBasicProperties.CorrelationId = e.BasicProperties.CorrelationId;
            byte[] responseBytes = Encoding.UTF8.GetBytes(_reponse);
            e.Model.BasicPublish("", e.BasicProperties.ReplyTo, replyBasicProperties, responseBytes);

        }

      
    }
}
