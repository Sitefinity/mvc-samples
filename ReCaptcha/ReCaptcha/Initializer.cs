using ReCaptcha.Mvc.Models;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Modules.Forms.Configuration;

namespace ReCaptcha
{
    public static class Initializer
    {
        public static void Initialize()
        {
            Bootstrapper.Initialized += Initializer.Bootstrapper_Initialized;
        }

        private static void Bootstrapper_Initialized(object sender, ExecutedEventArgs e)
        {
            if (e.CommandName == "Bootstrapped")
            {
                const string TestGRecaptchaDataSitekey = "6LeTWwwTAAAAADnNmwCb9Rnf41n7UDvgkzs8pYrU";
                const string TestGRecaptchaSecret = "6LeTWwwTAAAAAOTF9tzmlN0C0xvrrDB6nfamLVDJ";

                var manager = ConfigManager.GetManager();
                var formsConfigSection = manager.GetSection<FormsConfig>();
                if (formsConfigSection.Parameters[RecaptchaModel.GRecaptchaParameterDataSiteKey] == null)
                {
                    formsConfigSection.Parameters.Add(RecaptchaModel.GRecaptchaParameterDataSiteKey, TestGRecaptchaDataSitekey);
                }

                if (formsConfigSection.Parameters[RecaptchaModel.GRecaptchaParameterSecretKey] == null)
                {
                    formsConfigSection.Parameters.Add(RecaptchaModel.GRecaptchaParameterSecretKey, TestGRecaptchaSecret);
                }

                using (var a = new ElevatedConfigModeRegion())
                {
                    manager.SaveSection(formsConfigSection);
                }
            }
        }
    }
}
