using AdminPortal.Models;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPortal.Codebase
{
    public class MessageProcessing
    {
        dynamic connection = Database.OpenNamedConnection("MainDB");
        public dynamic COR_WEB_createMessage(Campaign campaign) 
        {
            var result = connection.COR_WEB_createMessage(user_id: campaign.user_id, sender: campaign.sender, receiver: campaign.receiver, msgdata: campaign.msgdata, camp_smstype: campaign.camp_smstype);
            return result;
        }
        public dynamic COR_WEB_createCampaign(Campaign campaign)
        {
            var result = connection.COR_WEB_createCampaign(user_id: campaign.user_id, camp_name: campaign.camp_name, sender: campaign.sender, msgdata: campaign.msgdata, camp_time: campaign.camp_time, camp_smstype: campaign.camp_smstype).FirstOrDefault();
            return result;
        }
        public dynamic COR_WEB_updateCampaign(int camp_id, int total_sms)
        {
            var result = connection.COR_WEB_updateCampaign(camp_id: camp_id, total_sms: total_sms);
            
            return result;
        }
        public void COR_WEB_CampaignManagement(int user_id, out List<dynamic> stats, out List<dynamic> details)
        {
            var result = connection.COR_WEB_CampaignManagement(user_id: user_id);
            stats = result.ToList<dynamic>();

            result.NextResult();
            details = result.ToList<dynamic>();
        }
    }
}