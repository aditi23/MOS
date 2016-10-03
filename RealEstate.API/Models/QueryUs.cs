using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Data.Odbc;
using System.Collections;
using System.Data.SqlClient;


namespace RealEstate.API.Models
{
    public class QueryUs
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string EmailId { get; set; }
        [Required]
        public long ContactNumber { get; set; }
        [Required]
        public int CityId { get; set; }
        [Required]
        public string Message { get; set; }

        public bool SendQuery(QueryUs objQueryUs)
        {
            try {
                string mailSubject = "Query";
                string mailBody = "<b>From: </b>" + objQueryUs.Name + "<br/><br/>";
                mailBody += "<b>Email: </b>" + objQueryUs.EmailId + "<br/><br/>";
                mailBody += "<b>Contact: </b>" + objQueryUs.ContactNumber + "<br/><br/>";
                mailBody += "<b>City: </b>" + objQueryUs.CityId + "<br/><br/>";
                mailBody += "<b>Query: </b>" + objQueryUs.Message;

                Notification objNotification = new Notification();
                bool status = objNotification.SendMail("sukankshi.jain@gmail.com", mailSubject, mailBody);
                
                return status;
            }
            catch(Exception)
            {
                return false;
            }
        }

    }
}