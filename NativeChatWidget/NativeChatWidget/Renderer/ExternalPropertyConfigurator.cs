using System;
using System.Collections.Generic;
using System.ComponentModel;
using NativeChatWidget.Client;
using Newtonsoft.Json;
using Progress.Sitefinity.Renderer.Designers;
using Progress.Sitefinity.Renderer.Designers.Dto;
using Telerik.Sitefinity.Abstractions;

namespace NativeChatWidget.Renderer
{
    public class ExternalPropertyConfigurator : IPropertyConfigurator
    {
        public virtual void ProcessPropertyMetadataContainer(PropertyDescriptor descriptor, PropertyMetadataContainerDto propertyContainer, string componentName)
        {
            foreach (Attribute attr in descriptor.Attributes)
            {
                ProcessConfigurationExternalDataChoiceAttribute(attr, propertyContainer);
            }
        }

        private static void ProcessConfigurationExternalDataChoiceAttribute(Attribute attribute, PropertyMetadataContainerDto propertyContainer)
        {
            var externalChoiceAttr = attribute as ExternalDataChoiceAttribute;
            if (externalChoiceAttr != null)
            {
                var serializedChoices = FetchChoices();
                propertyContainer.Properties.Add($"{WidgetMetadataConstants.Prefix}_Choices", serializedChoices);

                var choiceKey = $"{WidgetMetadataConstants.Prefix}_Choice_Choices";
                if (propertyContainer.Properties.ContainsKey($"{WidgetMetadataConstants.Prefix}_Choice_Choices"))
                {
                    propertyContainer.Properties[choiceKey] = serializedChoices;
                }
                else
                {
                    propertyContainer.Properties.Add(choiceKey, serializedChoices);
                }
            }
        }

        private static string FetchChoices()
        {
            var choices = new List<ChoiceValueDto>() { new ChoiceValueDto("Select", "") };

            var client = ObjectFactory.Resolve<INativeChatClient>();

            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            using (client)
            {
                foreach (var bot in client.Bots())
                {
                    choices.Add(new ChoiceValueDto(bot.DisplayName ?? bot.Name, bot.Id));
                }
            }

            return JsonConvert.SerializeObject(choices);
        }
    }
}
