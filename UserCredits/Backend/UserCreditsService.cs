using App.Data;
using App.Web.Domain;
using App.Web.Models.Requests;
using App.Web.Models.Responses;
using App.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace App.Web.Services
{
    public class UserCreditsService : BaseService, IUserCreditsService
    {
        public int InsertUserCredits(UserCreditsRequest model)
        {
            int uid = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.UserCredits_Insert"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   SqlParameter p = new SqlParameter("@Id", System.Data.SqlDbType.Int);
                   p.Direction = System.Data.ParameterDirection.Output;

                   paramCollection.Add(p);

                   if (model.TransactionType == "Subtract")
                   {
                       model.Amount = model.Amount * -1;
                   }

                   paramCollection.AddWithValue("@UserId", model.UserId);
                   paramCollection.AddWithValue("@Amount", model.Amount);
                   paramCollection.AddWithValue("@TransactionId", model.TransactionId);
                   paramCollection.AddWithValue("@JobId", model.JobId);

               }, returnParameters: delegate (SqlParameterCollection param)
               {
                   int.TryParse(param["@Id"].Value.ToString(), out uid);
               }
               );


            return uid;
        }

        public PaginatedItemsResponse<UserCredits> GetTransactionsPaginated(PaginatedRequest model)
        {
            List<UserCredits> TransactionsList = null;
            PaginatedItemsResponse<UserCredits> response = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.UserCredits_SelectAll"
              , inputParamMapper: delegate (SqlParameterCollection paramCollection)
              {
                  paramCollection.AddWithValue("@CurrentPage", model.CurrentPage);
                  paramCollection.AddWithValue("@ItemsPerPage", model.ItemsPerPage);
                  paramCollection.AddWithValue("@Query", model.Query);

              }, map: delegate (IDataReader reader, short set)
              {
                  if (set == 0)

                  {
                      UserCredits data = new UserCredits();
                      int startingIndex = 0; //startingOrdinal

                      data.Id = reader.GetSafeInt32(startingIndex++);
                      data.UserId = reader.GetSafeString(startingIndex++);
                      data.Amount = reader.GetSafeDecimal(startingIndex++);
                      data.TransactionId = reader.GetSafeInt32(startingIndex++);
                      data.JobId = reader.GetSafeInt32(startingIndex++);
                      data.DateAdded = reader.GetSafeDateTime(startingIndex++);

                      UserMini User = new UserMini();

                      User.FirstName = reader.GetSafeString(startingIndex++);
                      User.LastName = reader.GetSafeString(startingIndex++);
                      User.Email = reader.GetSafeString(startingIndex++);
                      User.Phone = reader.GetSafeString(startingIndex++);
                      User.Url = reader.GetSafeString(startingIndex++);

                      data.User = User;

                      if (TransactionsList == null)
                      {
                          TransactionsList = new List<UserCredits>();
                      }

                      TransactionsList.Add(data);
                  }
                  else if (set == 1)
                  {
                      response = new PaginatedItemsResponse<UserCredits>();

                      response.TotalItems = reader.GetSafeInt32(0);

                  }
              }

           );
            response.Items = TransactionsList;
            response.CurrentPage = model.CurrentPage;
            response.ItemsPerPage = model.ItemsPerPage;
            return response;

        }

        public UserCreditsBalance GetBalance(string UserId)
        {
            UserCreditsBalance Balance = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.UserCredits_SelectByUserId"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@UserId", UserId);

                }, map: delegate (IDataReader reader, short set)
                {
                    Balance = new UserCreditsBalance();
                    int startingIndex = 0;

                    Balance.UserId = reader.GetSafeString(startingIndex++);
                    Balance.Amount = reader.GetSafeDecimal(startingIndex++);

                }
            );
            return Balance;
        }

        public static List<UserCreditsForJobs> GetCreditsForJobPage(string UserId)
        {
            List<UserCreditsForJobs> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Coupons_GetCreditsForJob"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@UserId", UserId);
                }, map: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    UserCreditsForJobs t = new UserCreditsForJobs();

                    t.Id = reader.GetSafeInt32(startingIndex++);
                    t.DateCreated = reader.GetSafeDateTime(startingIndex++);
                    t.Used = reader.GetSafeDateTimeNullable(startingIndex++);
                    t.TokenHash = reader.GetSafeGuid(startingIndex++);
                    t.UserId = reader.GetSafeString(startingIndex++);
                    t.TokenType = reader.GetSafeInt32(startingIndex++);
                    t.CouponId = reader.GetSafeInt32(startingIndex++);
                    t.Email = reader.GetSafeString(startingIndex++);

                    UserCredits UserCredits = new UserCredits();

                    UserCredits.Id = reader.GetSafeInt32(startingIndex++);
                    UserCredits.UserId = reader.GetSafeString(startingIndex++);
                    UserCredits.Amount = reader.GetSafeDecimal(startingIndex++);
                    UserCredits.TransactionId = reader.GetSafeInt32(startingIndex++);
                    UserCredits.JobId = reader.GetSafeInt32(startingIndex++);
                    UserCredits.DateAdded = reader.GetSafeDateTime(startingIndex++);

                    t.UserCredits = UserCredits;

                    if (list == null)
                    {
                        list = new List<UserCreditsForJobs>();
                    }
                    list.Add(t);


                });

            return list;
        }
    }
}