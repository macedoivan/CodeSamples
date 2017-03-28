using MailChimp.Net;
using MailChimp.Net.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Web.Services.Interfaces;
using MailChimp.Net.Models;
using App.Web.Models.Requests;
using App.Web.Domain;
using System.Data;
using App.Data;
using System.Data.SqlClient;

namespace App.Web.Services
{
    public class EmailCampaignsService : BaseService, IEmailCampaignsService
    {
        private static readonly IMailChimpManager Manager = new MailChimpManager();

        public async Task<IEnumerable<List>> ReturnLists()
        {
            var model = await Manager.Lists.GetAllAsync();
            return model;
        }
        public async Task<string> InsertList(WebsiteAddRequest model)
        {
            List ThisList = new List();

            string Id = model.ListId;

            if (Id == null)
            {
                ThisList.Name = model.Name;

                Contact Contact = new Contact();
                Contact.Company = model.Name;
                Contact.Address1 = model.Street;
                Contact.City = model.City;
                Contact.State = model.State;
                Contact.Zip = model.ZipCode.ToString();
                Contact.Country = model.Country;

                ThisList.Contact = Contact;
                ThisList.PermissionReminder = model.PermissionReminder;

                CampaignDefaults CampaignDefaults = new CampaignDefaults();
                CampaignDefaults.FromEmail = model.FromEmail;
                CampaignDefaults.FromName = model.FromName;
                CampaignDefaults.Subject = model.Subject;
                CampaignDefaults.Language = "en/us";

                ThisList.CampaignDefaults = CampaignDefaults;
                ThisList.EmailTypeOption = true;
            }
            else
            {
                ThisList.Name = model.Name;

                ThisList.Id = model.ListId;

                Contact Contact = new Contact();
                Contact.Company = model.Name;
                Contact.Address1 = model.Street;
                Contact.City = model.City;
                Contact.State = model.State;
                Contact.Zip = model.ZipCode.ToString();
                Contact.Country = model.Country;

                ThisList.Contact = Contact;

                ThisList.PermissionReminder = model.PermissionReminder;

                CampaignDefaults CampaignDefaults = new CampaignDefaults();
                CampaignDefaults.FromEmail = model.FromEmail;
                CampaignDefaults.FromName = model.FromName;
                CampaignDefaults.Subject = model.Subject;
                CampaignDefaults.Language = "en/us";

                ThisList.CampaignDefaults = CampaignDefaults;

                ThisList.EmailTypeOption = true;
            }
            var result = await Manager.Lists.AddOrUpdateAsync(ThisList);

            var ListId = result.Id;

            return ListId; // return list id ---> pass into web service ---> insert to website table
        }
        public List<MailChimpUsers> GetUsersByDate()
        {
            List<MailChimpUsers> MyList = null;

            DateTime startDate = new DateTime(2017, 2, 27);
            DateTime currentDate = DateTime.Now;

            foreach (DateTime day in UtilityService.EachDay(startDate, currentDate))
            {
                DataProvider.ExecuteCmd(GetConnection, "dbo.MailChimp_SelectUsers"
              , inputParamMapper: delegate (SqlParameterCollection paramCollection)
              {
                  paramCollection.AddWithValue("@Date", day);
              }
              , map: delegate (IDataReader reader, short set)
              {
                  MailChimpUsers Users = new MailChimpUsers();
                  int startingIndex = 0; //startingOrdinal

                  Users.Email = reader.GetSafeString(startingIndex++);
                  Users.ListId = reader.GetSafeString(startingIndex++);

                  if (MyList == null)
                  {
                      MyList = new List<MailChimpUsers>();
                  }
                  MyList.Add(Users);
              });
            }
            return MyList;
        }
        public void InsertUsersToList()
        {
            var result = GetUsersByDate();

            foreach (var item in result)
            {
                var listId = item.ListId;
                var member = new Member
                {
                    EmailAddress = item.Email,
                    StatusIfNew = Status.Subscribed
                };
                var task = Task.Run(async () => await Manager.Members.AddOrUpdateAsync(listId, member));
            }
        }
    }
}