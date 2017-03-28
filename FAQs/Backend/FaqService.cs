using App.Data;
using App.Web.Domain;
using App.Web.Models.Requests;
using App.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace App.Web.Services
{
    public class HelpService : BaseService, IHelpService
    {
        public int AddFaq(HelpInsertRequest model)
        {
            int uid = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Help_Insert"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {


                   SqlParameter p = new SqlParameter("@Id", System.Data.SqlDbType.Int);
                   p.Direction = System.Data.ParameterDirection.Output;

                   paramCollection.Add(p);

                   paramCollection.AddWithValue("@SortOrder", model.SortOrder);
                   paramCollection.AddWithValue("@WebsiteId", model.WebsiteId);
                   paramCollection.AddWithValue("@Question", model.Question);
                   paramCollection.AddWithValue("@Answer", model.Answer);
                   


               }, returnParameters: delegate (SqlParameterCollection param)
               {
                   int.TryParse(param["@Id"].Value.ToString(), out uid);
               }
               );


            return uid;
        }

        public void UpdateFaq(HelpUpdateRequest model)
        {
           // int uid = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Help_Update"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Id", model.Id);
                   paramCollection.AddWithValue("@WebsiteId", model.WebsiteId);
                   paramCollection.AddWithValue("@Question", model.Question);
                   paramCollection.AddWithValue("@Answer", model.Answer);
                  

               }, returnParameters: delegate (SqlParameterCollection param)
               {
              //     int.TryParse(param["@Id"].Value.ToString(), out uid);
               }
               );

        }

        public List<Help> GetAllFaqs()
        {
            List<Help> HelpList = new List<Help>();

            DataProvider.ExecuteCmd(GetConnection, "dbo.Help_SelectAll"
              , inputParamMapper: delegate (SqlParameterCollection paramCollection)
              {


              }, map: delegate (IDataReader reader, short set)
              {
                  Help Faq = new Help();
                  int startingIndex = 0; //startingOrdinal

                  Faq.Id = reader.GetSafeInt32(startingIndex++);
                  Faq.Question = reader.GetSafeString(startingIndex++);
                  Faq.Answer = reader.GetSafeString(startingIndex++);
                  Faq.SortOrder = reader.GetSafeInt32(startingIndex++);
                  Faq.WebsiteId = reader.GetSafeInt32(startingIndex++);
                  Faq.DateAdded = reader.GetSafeDateTime(startingIndex++);
                  Faq.DateModified = reader.GetSafeDateTime(startingIndex++);

                  HelpList.Add(Faq);

              }

           );
            return HelpList;

        }

        public Help GetFaqById(int id)
        {

            Help FaqItem = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Help_SelectById"
              , inputParamMapper: delegate (SqlParameterCollection paramCollection)
              {
                  paramCollection.AddWithValue("@Id", id);

              }, map: delegate (IDataReader reader, short set)
              {
                  FaqItem = new Help();
                  int startingIndex = 0; //startingOrdinal

                  FaqItem.Id = reader.GetSafeInt32(startingIndex++);
                  FaqItem.Question = reader.GetSafeString(startingIndex++);
                  FaqItem.Answer = reader.GetSafeString(startingIndex++);
                  FaqItem.SortOrder = reader.GetSafeInt32(startingIndex++);
                  FaqItem.WebsiteId = reader.GetSafeInt32(startingIndex++);
                  FaqItem.DateAdded = reader.GetSafeDateTime(startingIndex++);
                  FaqItem.DateModified = reader.GetSafeDateTime(startingIndex++);

              }

           );
            return FaqItem;

        }

        public void DeleteFaqById(int id)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Help_Delete"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Id", id);

                });
        }

        public void UpdateToOrder(int Id, int Order)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Help_UpdateOrder"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Id", Id);
                    paramCollection.AddWithValue("@SortOrder", Order);

                });
        }

        public List<Help> GetAllByWebId(int WebsiteId)
        {
            List<Help> HelpList = new List<Help>();

            DataProvider.ExecuteCmd(GetConnection, "dbo.Help_SelectByWebsiteId"
              , inputParamMapper: delegate (SqlParameterCollection paramCollection)
              {
                  paramCollection.AddWithValue("@WebsiteId", WebsiteId);

              }, map: delegate (IDataReader reader, short set)
              {
                  Help Faq = new Help();
                  int startingIndex = 0; //startingOrdinal

                  Faq.Id = reader.GetSafeInt32(startingIndex++);
                  Faq.Question = reader.GetSafeString(startingIndex++);
                  Faq.Answer = reader.GetSafeString(startingIndex++);
                  Faq.SortOrder = reader.GetSafeInt32(startingIndex++);
                  Faq.WebsiteId = reader.GetSafeInt32(startingIndex++);
                  Faq.DateAdded = reader.GetSafeDateTime(startingIndex++);
                  Faq.DateModified = reader.GetSafeDateTime(startingIndex++);

                  HelpList.Add(Faq);

              }

           );
            return HelpList;

        }
    }
}