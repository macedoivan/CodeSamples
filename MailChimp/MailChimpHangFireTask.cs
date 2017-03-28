   public static void MailChimpUsers()
        {
            string cronExp = "0 3 * * 0-6";

            IEmailCampaignsService EmailCampaignsService = UnityConfig.GetContainer().Resolve<IEmailCampaignsService>();

            RecurringJob.AddOrUpdate("MailChimp Users",  () => EmailCampaignsService.InsertUsersToList(), cronExp, TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
        }