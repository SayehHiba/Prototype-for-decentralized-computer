using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RessourcesHumaines.Models;
using Syncfusion.XlsIO;

namespace RessourcesHumaines.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmployerController : ControllerBase
    {
        /// <summary>
        /// recuperation d'une feuille à partir d'un fichier excel 
        /// </summary>
        /// <returns> feuille excel </returns>
      

        #region getEmp
        /// <summary>
        /// Get tout les employers
        /// </summary>
        /// <returns>Liste</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employe>>> GetEmployees()
        {
            IWorksheet worksheet =await Excel.get();
            int length = worksheet.Rows.Count();
            List<Employe> List = new List<Employe>();
            Employe emp;
            for (int i = 1; i <= length; i++)
            {
                emp = new Employe();
                string cellA = "A" + i;
                string cellB = "B" + i;
                string cellC = "C" + i;
                emp.id = i;
                emp.Prenom = worksheet.Range[cellA].Text;
                emp.Nom = worksheet.Range[cellB].Text;
                emp.Email = worksheet.Range[cellC].Text;
                List.Add(emp);
            }

            return List;

        }
        #endregion
        /// <summary>
        /// Get un employe
        /// </summary>
        /// <param name="id">id de l'employe</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Employe>> GetEmploye(int id)
        {
            IWorksheet worksheet =await Excel.get();
            Employe emp = new Employe();
            string cellA = "A" + id;
            string cellB = "B" + id;
            string cellC = "C" + id;
            emp.id = id;
            emp.Prenom = worksheet.Range[cellA].Text;
            emp.Nom = worksheet.Range[cellB].Text;
            emp.Email = worksheet.Range[cellC].Text;
            return emp;

        }


        [HttpPost]
        public async Task<int> PostEmploye(Employe e)
        
        {
                IWorksheet worksheet =await Excel.get();
            int id = worksheet.Rows.Count()+1;
                string cellA = "A" + id;
                string cellB = "B" + id;
                string cellC = "C" + id;
                worksheet.Range[cellA].Text=e.Prenom;
                worksheet.Range[cellB].Text=e.Nom;
                worksheet.Range[cellC].Text =e.Email;



    
            return id;

  
        }

        [HttpPut]
        public async Task<Employe> PutEmploye(Employe e)
        {
            IWorksheet worksheet = await Excel.get();
            string cellA = "A" + e.id;
            string cellB = "B" + e.id;
            string cellC = "C" + e.id;
            worksheet.Range[cellA].Text = e.Prenom;
            worksheet.Range[cellB].Text = e.Nom;
            worksheet.Range[cellC].Text = e.Email;
            await Excel.save();
            return e;
        }
        
        [HttpDelete("{id}")]
        public async Task<int> DeleteEmploye(int id)
        {
            IWorksheet worksheet =await Excel.get();
            int length = worksheet.Rows.Count();
            Employe emp; 
            for (int i = id; i < length; i++)
            {   emp = new Employe();
                string cellA = "A" + i+1;
                string cellB = "B" + i+1;
                string cellC = "C" + i+1;
                emp.Prenom = worksheet.Range[cellA].Text;
                emp.Nom = worksheet.Range[cellB].Text;
                emp.Email = worksheet.Range[cellC].Text;
                cellA = "A" + i;
                cellB = "B" + i;
                cellC = "C" + i;
                worksheet.Range[cellA].Text= emp.Prenom;
                worksheet.Range[cellB].Text = emp.Nom;
                worksheet.Range[cellC].Text = emp.Email;
            }
            await Excel.save();
            return id;
        }
        




    }
    
}
