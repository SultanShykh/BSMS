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
        public dynamic createMessage(Campaign campaign) 
        {
            var result = connection.createMessage(user_id: campaign.user_id, sender: campaign.sender, receiver: campaign.receiver, msgdata: campaign.msgdata);
            return result;
        }

        public dynamic createCampaign(Campaign campaign)
        {
            var result = connection.createCampaign(user_id: campaign.user_id, camp_name: campaign.camp_name, sender: campaign.sender, receiver: campaign.receiver, msgdata: campaign.msgdata, camp_time: campaign.camp_time);
            return result;
        }
    }
}