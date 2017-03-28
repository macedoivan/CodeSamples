using Microsoft.Practices.Unity;
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
    public class UserAddressService : BaseService, IUserAddressService
    {
        [Dependency]
        public IAddressService _AddressService { get; set; }
        public void ProcessJobAddresses(Job JobData, string UserId)
        {
            foreach (var Address in JobData.JobWaypoints)
            {
                DataProvider.ExecuteNonQuery(GetConnection, "dbo.UserAddress_Insert"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                     {
                         paramCollection.AddWithValue("@UserId", UserId);
                         paramCollection.AddWithValue("@AddressId", Address.AddressId);

                     }, returnParameters: null);
            }
        }
        public List<UserAddress> GetUserAddresses(string UserId)
        {
            List<UserAddress> AddressesList = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.UserAddress_SelectByUserId"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@UserId", UserId);

               }, map: delegate (IDataReader reader, short set)
               {
                   int startingIndex = 0;
                   UserAddress List = new UserAddress();
                   List.DefaultAddress = reader.GetSafeBool(startingIndex++);

                   Address SingleAddress = new Address();
                   SingleAddress.AddressId = reader.GetSafeInt32(startingIndex++);
                   SingleAddress.DateCreated = reader.GetSafeDateTime(startingIndex++);
                   SingleAddress.DateModified = reader.GetSafeDateTime(startingIndex++);
                   SingleAddress.Name = reader.GetSafeString(startingIndex++);
                   SingleAddress.ExternalPlaceId = reader.GetSafeString(startingIndex++);
                   SingleAddress.Line1 = reader.GetSafeString(startingIndex++);
                   SingleAddress.Line2 = reader.GetSafeString(startingIndex++);
                   SingleAddress.City = reader.GetSafeString(startingIndex++);
                   SingleAddress.State = reader.GetSafeString(startingIndex++);
                   SingleAddress.StateId = reader.GetSafeInt32(startingIndex++);
                   SingleAddress.ZipCode = reader.GetSafeInt32(startingIndex++);
                   SingleAddress.Latitude = reader.GetSafeDecimal(startingIndex++);
                   SingleAddress.Longitude = reader.GetSafeDecimal(startingIndex++);
                   SingleAddress.Country = reader.GetSafeString(startingIndex++);

                   List.Address = SingleAddress;

                   if (AddressesList == null)
                   {
                       AddressesList = new List<UserAddress>();
                   }

                   AddressesList.Add(List);

               });

            return AddressesList;
        }
        public void DeleteAddress(string UserId, int AddressId)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.UserAddress_Delete"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@UserId", UserId);
                    paramCollection.AddWithValue("@AddressId", AddressId);
                });
        }
        public void InsertAddress(string UserId, AddressAddRequest model)
        {
            var Result = _AddressService.Insert(model);

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.UserAddress_Insert"
            , inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@UserId", UserId);
                paramCollection.AddWithValue("@AddressId", Result);

            }, returnParameters: null);
        }

        public void UpdateCustomerDefaultAddress(string UserId, int AddressId)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.UserAddress_UpdateDefaultAddress"
              , inputParamMapper: delegate (SqlParameterCollection paramCollection)
              {
                  paramCollection.AddWithValue("@UserId", UserId);
                  paramCollection.AddWithValue("@AddressId", AddressId);
              });
        }

        public UserAddress GeDefaultAddress(string UserId)
        {
            UserAddress Item = new UserAddress();

            DataProvider.ExecuteCmd(GetConnection, "dbo.UserAddress_SelectByDefaultAddress"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@UserId", UserId);

                }, map: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    Item.DefaultAddress = reader.GetSafeBool(startingIndex++);

                    Address SingleAddress = new Address();
                    SingleAddress.AddressId = reader.GetSafeInt32(startingIndex++);
                    SingleAddress.DateCreated = reader.GetSafeDateTime(startingIndex++);
                    SingleAddress.DateModified = reader.GetSafeDateTime(startingIndex++);
                    SingleAddress.Name = reader.GetSafeString(startingIndex++);
                    SingleAddress.ExternalPlaceId = reader.GetSafeString(startingIndex++);
                    SingleAddress.Line1 = reader.GetSafeString(startingIndex++);
                    SingleAddress.Line2 = reader.GetSafeString(startingIndex++);
                    SingleAddress.City = reader.GetSafeString(startingIndex++);
                    SingleAddress.State = reader.GetSafeString(startingIndex++);
                    SingleAddress.StateId = reader.GetSafeInt32(startingIndex++);
                    SingleAddress.ZipCode = reader.GetSafeInt32(startingIndex++);
                    SingleAddress.Latitude = reader.GetSafeDecimal(startingIndex++);
                    SingleAddress.Longitude = reader.GetSafeDecimal(startingIndex++);
                    SingleAddress.Country = reader.GetSafeString(startingIndex++);

                    Item.Address = SingleAddress;

                });
            return Item;
        }
    }
}



