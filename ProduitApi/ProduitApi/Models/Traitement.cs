using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RitegeQueueManager.Model;
using System.Threading;
using RitegeQueueManager.Implement;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Diagnostics;

namespace ProduitApi.Models
{
    public class Traitement
    {

        private string _reponse;

        public void TraiterMessageConsommer(object sender, MessageBodyEvent e)
        {
            DescriptionMessage descriptionMessage = JsonConvert.DeserializeObject<DescriptionMessage>(e.Message);

          

            Task<ActionResult<ProduitItem>> p = new ProduitItemsController(null).GetProduitItem((int)descriptionMessage.Donnees[0]);

            
            if (p.Result.Value.Stock>= (int)descriptionMessage.Donnees[1])
            {
                p.Result.Value.Stock -= (int)descriptionMessage.Donnees[1];
                Task < IActionResult > pp = new ProduitItemsController(null).PutProduitItem(p.Result.Value.Id, p.Result.Value);
                Debug.WriteLine("\n\n" + pp.ToString() + "\n\n");
                if (pp.ToString().Equals("Microsoft.AspNetCore.Mvc.NoContentResult"))
                {
                    _reponse = "Valide";
                }
                else { _reponse = "Non"; }

            }
            else { _reponse = "Non";  }




            IBasicProperties replyBasicProperties = e.Model.CreateBasicProperties();
            replyBasicProperties.CorrelationId = e.BasicProperties.CorrelationId;
            byte[] responseBytes = Encoding.UTF8.GetBytes(_reponse);
            e.Model.BasicPublish("", e.BasicProperties.ReplyTo, replyBasicProperties, responseBytes);


        }

        
    }
}
