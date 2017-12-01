using ReCaptcha.Mvc.Models;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Modules.Forms.Configuration;

namespace ReCaptcha
{
    public static class Initializer
    {
        public static void Initialize()
        {
            Bootstrapper.Bootstrapped += Initializer.Bootstrapper_Bootstrapped;
        }

        private static void Bootstrapper_Bootstrapped(object sender, System.EventArgs e)
        {
            const string TestGRecaptchaDataSitekey = "6LeTWwwTAAAAADnNmwCb9Rnf41n7UDvgkzs8pYrU";
            const string TestGRecaptchaSecret = "6LeTWwwTAAAAAOTF9tzmlN0C0xvrrDB6nfamLVDJ";

            var manager = ConfigManager.GetManager();
            var formsConfigSection = manager.GetSection<FormsConfig>();
            var formsConfigParameters = formsConfigSection.Parameters;
            if (formsConfigParameters[RecaptchaModel.GRecaptchaParameterDataSiteKey] == null)
                formsConfigParameters.Add(RecaptchaModel.GRecaptchaParameterDataSiteKey, TestGRecaptchaDataSitekey);

            if (formsConfigParameters[RecaptchaModel.GRecaptchaParameterSecretKey] == null)
                formsConfigParameters.Add(RecaptchaModel.GRecaptchaParameterSecretKey, TestGRecaptchaSecret);

            using (new ElevatedConfigModeRegion())
            {
                manager.SaveSection(formsConfigSection);
            }
        }
    }
}
