Custom Progress Agentic RAG widgets
======

The following sample demonstrates how to create a set of custom MVC widgets powered by Progress Agentic RAG. The list of widgets is the following:
- AI Assistant Chat
- Search box
- Search results - including facets and AI generated answer

# Install the widgets

1. Clone the [feather-samples](https://github.com/Sitefinity/feather-samples) repository.
2. Check if the version of the Sitefinity nugets referenced in it is the same as the version of your project. It they are different make sure to upgrade the nugets in the desired widget's project to match your version.
3. Build the desired widget's project.
4. Reference the created dll from your Sitefinity’s web application.
5. Create a **Global.asax.cs** file in your Sitefinity web application if you don't have such.
6. Modify its content as shown in the sample **{{PROJECT_NAME}}/SitefinityWebApp/Global.asax.cs**
7. Build your Sitefinity web application.

The widget will appear in your page toolbox.

# Setup the widgets

## Setup Nuclia settings
	- Nua key:
		- Log in to the Agentic RAG Dashboard with your credentials.
		- Navigate to Manage account > NUA Keys.
		- Select the option to create a new NUA Key and save it.
		- Copy the generated key for use in API authentication or SDK configurations. 
		- Paste the key in SF: **Advanced Settings -> Agentic RAG -> NUA key**
	- Account Id:
		- Log in to the Agentic RAG Dashboard with your credentials.
		- Navigate to Manage account > Account.
		- Copy the UID 
		- Paste the ID in SF: **Advanced Settings -> Agentic RAG -> Account ID**
	- Base URL
		- Log in to the Agentic RAG Dashboard with your credentials.
		- Copy NucliaDB API endpoint
		- Paste the URL without the path in SF: **Advanced Settings -> Agentic RAG -> Base URL**

## Setup Knowledge box
    - Knowledge box ID
        - Log in to the Agentic RAG Dashboard with your credentials.
        - Copy Knowledge Box UID
        - Paste the ID in SF: **Advanced Settings -> Agentic RAG -> Knowledge Boxes -> KnowledgeBoxId**

    - Knowledge box API key
        - Log in to the Agentic RAG Dashboard with your credentials.
        - Go to Advanced -> API keys
        - Create a new API key and copy it
        - Paste the key in SF: **Advanced Settings -> Agentic RAG -> Knowledge Boxes -> KnowledgeBoxKey**

## Setup Assistant settings
	- AdminApiBaseUrl - 
	- CdnHostName - assistantcdn.blob.core.windows.net or cdn.assistant.api.sitefinity.cloud
	- CdnRootFolderRelativePath - dev or prod