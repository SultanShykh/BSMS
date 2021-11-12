using AdminPortal.Models;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPortal.Codebase
{
    public class MaskProcessing
    {
        static dynamic AppDB = Database.OpenNamedConnection("MainDB");
        public static List<Masking> GetMaskings()
        {
            var maskings = AppDB.masking.All().ToList<Masking>();
            return maskings;
        }
        public static List<dynamic> FindByMobilink()
        {
            List<dynamic> list = new List<dynamic>();
            var maskings = AppDB.routes_info.FindAll(AppDB.routes_info.network_name == 1 || AppDB.routes_info.network_name == 7);
            foreach (var v in maskings)
            {
                list.Add(new
                {
                    route_name = v.route_name,
                    id = v.id
                });
            }
            return list;
        }
        public static List<dynamic> FindByZong()
        {
            List<dynamic> list = new List<dynamic>();
            var maskings = AppDB.routes_info.FindAllBy(network_name: 4);
            foreach (var v in maskings)
            {
                list.Add(new
                {
                    route_name = v.route_name,
                    id = v.id
                });
            }
            return list;
        }
        public static List<dynamic> FindByUfone()
        {
            List<dynamic> list = new List<dynamic>();
            var maskings = AppDB.routes_info.FindAllBy(network_name: 3);
            foreach (var v in maskings)
            {
                list.Add(new
                {
                    route_name = v.route_name,
                    id = v.id
                });
            }
            return list;
        }
        public static List<dynamic> FindByTelenor()
        {
            List<dynamic> list = new List<dynamic>();
            var maskings = AppDB.routes_info.FindAllBy(network_name: 6);
            foreach (var v in maskings)
            {
                list.Add(new
                {
                    route_name = v.route_name,
                    id = v.id
                });
            }
            return list;
        }
        public static List<dynamic> FindBySCOM()
        {
            List<dynamic> list = new List<dynamic>();
            var maskings = AppDB.routes_info.FindAllBy(network_name: 05);
            foreach (var v in maskings)
            {
                list.Add(new
                {
                    route_name = v.route_name,
                    id = v.id
                });
            }
            return list;
        }
        public static string AddMaskingRoute(string mask, int userId, int route_name_mobilink, int route_name_warid, int route_name_ufone, int route_name_zong, int route_name_telenor, string route_name_SCOM)
        {
            route_name_SCOM = route_name_SCOM == "" ? null : route_name_SCOM;
            var result = AppDB.COR_Mask_AddMaskingRoutes(mask, userId, route_name_mobilink, route_name_warid, route_name_ufone, route_name_zong, route_name_telenor, route_name_SCOM).FirstOrDefault();
            return result.message;
        }
        public static List<dynamic> viewRoutes(string mask)
        {
            var list = AppDB.COR_Mask_ViewRoutes(mask).ToList<dynamic>();
            return list;
        }
        public static string UpdateMaskingRoute(string masking_hidden, int userId, int route_name_mobilink, int route_name_warid, int route_name_ufone, int route_name_zong, int route_name_telenor, string route_name_SCOM)
        {
            route_name_SCOM = route_name_SCOM == "" ? null : route_name_SCOM;
            var result = AppDB.COR_Mask_UpdateMaskingRoute(masking_hidden, userId, route_name_mobilink, route_name_warid, route_name_ufone, route_name_zong, route_name_telenor, route_name_SCOM).FirstOrDefault();
            return result.message;
        }
        public static string AddMaskingRequest(string masking, int reseller_id,string fullname, string nic, string contact, string masking_letter) 
        {
            var result = AppDB.COR_Mask_AddMaskingRequest(masking, reseller_id, fullname, nic, contact, masking_letter).FirstOrDefault();
            return result.message.ToString();
        }
        public static List<dynamic> MaskingRequests() 
        {
            List<dynamic> list = AppDB.masking_request.All().ToList<dynamic>();
            return list;
        }
        public static List<Masking> GetMasks(int id)
        {
            List<Masking> list = new List<Masking>();
            var maskings = AppDB.maskings.FindAllBy(network_name: 3);
            foreach (var v in maskings)
            {
                list.Add(new Masking
                {
                    masking = v.masking,
                    id = v.id
                });
            }
            return list;
        }

    }
}