# Custom Progress Agentic RAG widgets

This guide demonstrates how to create and configure a set of custom widgets powered by Progress Agentic RAG.

## Available Widgets

1. AI Assistant Chat
2. AI Ask Box
3. AI Answer
4. AI Search Results

## Installing the widgets

Follow these steps to install the custom Progress Agentic RAG widgets in your Sitefinity application:

1. Clone the [mvc-samples](https://github.com/Sitefinity/mvc-samples) repository.
2. Check Sitefinity NuGet versions
	1. Ensure that the Sitefinity NuGet package versions used in the sample match those used in your project. If they differ, update the NuGet references in the widget project to match your Sitefinity version.
3. Build the desired widget's project.
4. Reference the DLLs
	1. Add references to the generated DLLs in your Sitefinity web application.
5. Create a `Global.asax.cs` file, if your Sitefinity web application doesn’t already include one.
6. Configure the `Global.asax.cs` file
	1. Modify its contents to match the sample located at: `{{PROJECT_NAME}}/SitefinityWebApp/Global.asax.cs`
7. If you are using a Sitefinity version >= 15.4.8623 disable the `Progress Agentic RAG connector` module
8. Rebuild your Sitefinity web application

> **RESULT**: After rebuilding your project, the new widgets will appear in your Sitefinity Page Toolbox.

### Configure Sitefinity CMS

Before you can use Progress Agentic RAG, you must configure the respective setting in Sitefinity CMS:

1. Log in to the [Progress Agentic RAG Dashboard](https://rag.progress.cloud/).
1. From the _NucliaDB API endpoint_, copy **the host part** of the URL without the rest of the URL.
	Save the value somewhere, for example &ndash; in Notepad.<br>
	You will need this value later.
1. Copy the _Knowledge Box_ and save it somewhere.
1. In the Agentic RAG Dashboard, navigate to _Advanced » API Keys_
1. Create a new API key, copy it, and save it somewhere, for example &ndash; in Notepad.
1. In Sitefinity CMS backend, navigate to _Administration » Settings » Advanced_.
1. In the tree on the left, expand the _AgenticRAG » Knowledge Boxes_ node.
1. Click _Create new_.
1. In _Base URL_, paste the endpoint you from _Step 2_.<br>
	For example, <code>https://europe-1.rag.progress.cloud</code>.
1. In _Knowledge box UID_, paste the UID from _Step 3_.
1. In _Knowledge box API key_, paste the API key from _Step 5_.
1. Save your changes.

### Configure Assistant Settings

To configure the Sitefinity AI Assistant settings, perform the following:

1. In Sitefinity CMS backend, navigate to _Administration » Settings » Advanced_.
1. In the tree on the left, expand the _AgenticRAG » Assistant_ node.
1. In _AdminApiBaseUrl_ enter `https://api.sitefinity.cloud/Version`.
1. In _CdnHostName_ enter `cdn.assistant.cloud.sitefinity.com`.

### Configure Security Headers

1. In Sitefinity CMS backend, navigate to _Administration » Settings » Basic » Web Security_.
1. Under _Trusted Sources_, click _Edit_.
1. Add the value from `CdnHostName` (`cdn.assistant.cloud.sitefinity.com`) under the following sections:
	- `Scripts`
	- `Styles`
	- `Images`
